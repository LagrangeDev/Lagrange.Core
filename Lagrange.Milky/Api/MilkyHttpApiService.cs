using System.Net;
using System.Text;
using Lagrange.Core.Exceptions;
using Lagrange.Milky.Api.Exception;
using Lagrange.Milky.Api.Handler;
using Lagrange.Milky.Api.Result;
using Lagrange.Milky.Configuration;
using Lagrange.Milky.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lagrange.Milky.Api;

public class MilkyHttpApiService(ILogger<MilkyHttpApiService> logger, IOptions<MilkyConfiguration> options, IServiceProvider services) : IHostedService
{
    private readonly ILogger<MilkyHttpApiService> _logger = logger;

    private readonly string _host = options.Value.Host ?? throw new System.Exception("Milky.Host cannot be null");
    private readonly ulong _port = options.Value.Port ?? throw new System.Exception("Milky.Port cannot be null");
    private readonly string _prefix = $"{options.Value.Prefix}{(options.Value.Prefix.EndsWith('/') ? "" : "/")}api";
    private readonly string? _token = options.Value.AccessToken;

    private readonly IServiceProvider _services = services;

    private readonly HttpListener _listener = new();
    private CancellationTokenSource? _cts;
    private Task? _task;

    public Task StartAsync(CancellationToken token)
    {
        _listener.Prefixes.Add($"http://{_host}:{_port}{_prefix}/");
        _listener.Start();

        foreach (string prefix in _listener.Prefixes) _logger.LogServerRunning(prefix);

        _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
        _task = GetHttpContextLoopAsync(_cts.Token);

        return Task.CompletedTask;
    }

    private async Task GetHttpContextLoopAsync(CancellationToken token)
    {
        try
        {
            while (true)
            {
                _ = HandleHttpContextAsync(await _listener.GetContextAsync().WaitAsync(token), token);

                token.ThrowIfCancellationRequested();
            }
        }
        catch (OperationCanceledException) { throw; }
        catch (System.Exception e)
        {
            _logger.LogGetHttpContextException(e);
            throw;
        }
    }

    private async Task HandleHttpContextAsync(HttpListenerContext context, CancellationToken token)
    {
        var request = context.Request;
        var identifier = request.RequestTraceIdentifier;
        var remote = request.RemoteEndPoint;
        string method = request.HttpMethod;
        string? rawUrl = request.RawUrl;

        try
        {
            _logger.LogReceive(identifier, remote, method, rawUrl);

            if (!await ValidateHttpContextAsync(context, token)) return;

            var handler = await GetApiHandlerAsync(context, token);
            if (handler == null) return;

            object? parameter = await GetParameterAsync(context, handler.ParameterType, token);
            if (parameter == null) return;

            object? result = await GetResultAsync(context, handler, parameter, token);
            if (result == null) return;

            await SendWithLoggerAsync(context, result, token);
        }
        catch (OperationCanceledException) { throw; }
        catch (System.Exception e)
        {
            _logger.LogHandleHttpContextException(identifier, remote, e);

            await SendWithLoggerAsync(context, HttpStatusCode.InternalServerError, token);
        }
    }

    public async Task StopAsync(CancellationToken token)
    {
        _cts?.Cancel();
        if (_task != null) await _task.WaitAsync(token).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

        _listener.Stop();
    }

    private async Task<bool> ValidateHttpContextAsync(HttpListenerContext context, CancellationToken token)
    {
        var request = context.Request;
        var identifier = request.RequestTraceIdentifier;
        var remote = request.RemoteEndPoint;

        if (!context.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
        {
            await SendWithLoggerAsync(context, HttpStatusCode.MethodNotAllowed, token);
            return false;
        }

        if (!ValidateAccessToken(context))
        {
            _logger.LogValidateAccessTokenFailed(identifier, remote);
            await SendWithLoggerAsync(context, HttpStatusCode.Unauthorized, token);
            return false;
        }

        return true;
    }

    private bool ValidateAccessToken(HttpListenerContext context)
    {
        if (string.IsNullOrEmpty(_token)) return true;

        string? authorization = context.Request.Headers["Authorization"];
        if (authorization == null) return false;
        if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) return false;
        return authorization.AsSpan(7).SequenceEqual(_token);
    }

    private async Task<IApiHandler?> GetApiHandlerAsync(HttpListenerContext context, CancellationToken token)
    {
        string? path = context.Request.Url?.LocalPath;
        string? api = path?.Length > _prefix.Length + 1 ? path[(_prefix.Length + 1)..] : null;
        var handler = _services.GetKeyedService<IApiHandler>(api);
        if (handler == null) await SendWithLoggerAsync(context, HttpStatusCode.NotFound, token);
        return handler;
    }

    private async Task<object?> GetParameterAsync(HttpListenerContext context, Type type, CancellationToken token)
    {
        var request = context.Request;
        var identifier = request.RequestTraceIdentifier;
        var remote = request.RemoteEndPoint;
        var input = request.InputStream;

        try
        {
            using var content = new StreamContent(input);
            byte[] body = await content.ReadAsByteArrayAsync(token);
            _logger.LogReceiveBody(identifier, remote, body);

            return JsonUtility.Deserialize(type, body) ?? throw new NullReferenceException();
        }
        catch (System.Exception e)
        {
            _logger.LogDeserializeParameterException(identifier, remote, e);

            var result = new ApiFailedResult(-400, "parameter deserialize failed");
            await SendWithLoggerAsync(context, result, token);

            return null;
        }
    }

    private async Task<object?> GetResultAsync(HttpListenerContext context, IApiHandler handler, object parameter, CancellationToken token)
    {
        var request = context.Request;
        var identifier = request.RequestTraceIdentifier;
        var remote = request.RemoteEndPoint;

        try
        {
            return new ApiOkResult(await handler.HandleAsync(parameter, token));
        }
        catch (OperationException e)
        {
            _logger.LogHandleApiException(identifier, remote, e);

            return new ApiFailedResult(e.Result, e.ErrMsg ?? string.Empty);
        }
        catch (ApiException e)
        {
            _logger.LogHandleApiException(identifier, remote, e);

            return new ApiFailedResult(e.Retcode, e.Error);
        }
        catch (System.Exception e)
        {
            _logger.LogHandleApiException(identifier, remote, e);

            return new ApiFailedResult(-400, "Internal server error");
        }
    }

    private async Task SendWithLoggerAsync(HttpListenerContext context, HttpStatusCode status, CancellationToken token)
    {
        var request = context.Request;
        var identifier = request.RequestTraceIdentifier;
        var remote = request.RemoteEndPoint;

        var response = context.Response;
        var output = response.OutputStream;

        try
        {
            int code = (int)status;

            response.StatusCode = code;
            await output.WriteAsync(Encoding.UTF8.GetBytes($"{code} {status}"), token);
            response.Close();

            _logger.LogSend(identifier, remote, status);
        }
        catch (System.Exception e)
        {
            _logger.LogSendException(identifier, remote, e);
        }
    }

    private async Task SendWithLoggerAsync<TBody>(HttpListenerContext context, TBody body, CancellationToken token) where TBody : notnull
    {
        var request = context.Request;
        var identifier = request.RequestTraceIdentifier;
        var remote = request.RemoteEndPoint;

        var response = context.Response;
        var output = response.OutputStream;

        try
        {
            byte[] buffer = JsonUtility.SerializeToUtf8Bytes(body.GetType(), body);

            response.ContentType = "application/json; charset=utf-8";
            await output.WriteAsync(buffer, token);
            response.Close();

            _logger.LogSend(identifier, remote, buffer);
        }
        catch (System.Exception e)
        {
            _logger.LogSendException(identifier, remote, e);
        }
    }
}

public static partial class MilkyApiServiceLoggerExtension
{
    [LoggerMessage(LogLevel.Information, "Api http server is running on {prefix}")]
    public static partial void LogServerRunning(this ILogger<MilkyHttpApiService> logger, string prefix);

    [LoggerMessage(LogLevel.Debug, "{identifier} {remote} -->> {method} {path}")]
    public static partial void LogReceive(this ILogger<MilkyHttpApiService> logger, Guid identifier, IPEndPoint remote, string method, string? path);

    [LoggerMessage(LogLevel.Debug, "{identifier} {remote} -->> {body}")]
    private static partial void LogReceiveBody(this ILogger<MilkyHttpApiService> logger, Guid identifier, IPEndPoint remote, string body);
    public static void LogReceiveBody(this ILogger<MilkyHttpApiService> logger, Guid identifier, IPEndPoint remote, Span<byte> body)
    {
        if (logger.IsEnabled(LogLevel.Debug)) logger.LogReceiveBody(identifier, remote, Encoding.UTF8.GetString(body));
    }

    [LoggerMessage(LogLevel.Debug, "{identifier} {remote} <<-- {status}")]
    public static partial void LogSend(this ILogger<MilkyHttpApiService> logger, Guid identifier, IPEndPoint remote, HttpStatusCode status);

    [LoggerMessage(LogLevel.Debug, "{identifier} {remote} <<-- {body}", SkipEnabledCheck = true)]
    private static partial void LogSend(this ILogger<MilkyHttpApiService> logger, Guid identifier, IPEndPoint remote, string body);
    public static void LogSend(this ILogger<MilkyHttpApiService> logger, Guid identifier, IPEndPoint remote, Span<byte> body)
    {
        if (logger.IsEnabled(LogLevel.Debug)) logger.LogSend(identifier, remote, Encoding.UTF8.GetString(body));
    }

    [LoggerMessage(LogLevel.Error, "{identifier} {remote} <!!> Send failed")]
    public static partial void LogSendException(this ILogger<MilkyHttpApiService> logger, Guid identifier, IPEndPoint remote, System.Exception e);

    [LoggerMessage(LogLevel.Error, "{identifier} {remote} <!!> Handle http context failed")]
    public static partial void LogHandleHttpContextException(this ILogger<MilkyHttpApiService> logger, Guid identifier, IPEndPoint remote, System.Exception e);

    [LoggerMessage(LogLevel.Error, "{identifier} {remote} <!!> Handle api failed")]
    public static partial void LogHandleApiException(this ILogger<MilkyHttpApiService> logger, Guid identifier, IPEndPoint remote, System.Exception e);

    [LoggerMessage(LogLevel.Error, "{identifier} {remote} <!!> Deserialize parameter failed")]
    public static partial void LogDeserializeParameterException(this ILogger<MilkyHttpApiService> logger, Guid identifier, IPEndPoint remote, System.Exception e);

    [LoggerMessage(LogLevel.Error, "{identifier} {remote} <!!> Validate access token failed")]
    public static partial void LogValidateAccessTokenFailed(this ILogger<MilkyHttpApiService> logger, Guid identifier, IPEndPoint remote);

    [LoggerMessage(LogLevel.Error, "Get http context failed")]
    public static partial void LogGetHttpContextException(this ILogger<MilkyHttpApiService> logger, System.Exception e);
}