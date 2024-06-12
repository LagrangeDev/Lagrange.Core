using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Lagrange.OneBot.Core.Network.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lagrange.OneBot.Core.Network.Service;

public sealed partial class HttpService(
    IOptionsSnapshot<HttpServiceOptions> options,
    ILogger<HttpService> logger
) : BackgroundService, ILagrangeWebService
{
    public event EventHandler<MsgRecvEventArgs>? OnMessageReceived;

    private readonly HttpServiceOptions _options = options.Value;

    private readonly ILogger _logger = logger;

    private readonly HttpListener _listener = new();

    private readonly ConcurrentDictionary<string, HttpListenerResponse> _responses = new();

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        string prefix = $"http://{_options.Host}:{_options.Port}/";

        try
        {
            _listener.Prefixes.Add(prefix);
            _listener.Start();
            Log.LogStarted(_logger, prefix);
        }
        catch (Exception e)
        {
            Log.LogStartFailed(_logger, e);
            return;
        }

        await ReceiveLoop(token);

        if (_listener.IsListening)
        {
            try
            {
                _listener.Close();
            }
            catch (Exception e)
            {
                Log.LogCloseFailed(_logger, e);
            }
        }
    }

    private async Task ReceiveLoop(CancellationToken token)
    {
        while (_listener.IsListening && !token.IsCancellationRequested)
        {
            try
            {
                var context = await _listener.GetContextAsync().WaitAsync(token);
                _ = HandleRequestAsync(context, token);
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                break;
            }
            catch (Exception e)
            {
                Log.LogGetContextError(_logger, e);
            }
        }
    }

    private async Task HandleRequestAsync(HttpListenerContext context, CancellationToken token = default)
    {
        var request = context.Request;
        var response = context.Response; // no using cause we might need to use it in SendJsonAsync
        var query = request.QueryString; // avoid creating a new nvc every get


        try
        {
            string identifier = Guid.NewGuid().ToString();
            Log.LogRequest(_logger, identifier, request.RemoteEndPoint.ToString());

            if (!string.IsNullOrEmpty(_options.AccessToken))
            {
                var authorization = request.Headers.Get("Authorization") ??
                                    (query["access_token"] is { } accessToken ? $"Bearer {accessToken}" : null);
                if (authorization is null)
                {
                    Log.LogAuthFailed(_logger, identifier);
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Headers.Add("WWW-Authenticate", "Bearer");
                    response.Close();
                    return;
                }

                if (authorization != $"Bearer {_options.AccessToken}")
                {
                    Log.LogAuthFailed(_logger, identifier);
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    response.Close();
                    return;
                }
            }

            var action = request.Url!.AbsolutePath[1..];
            string payload;

            switch (request.HttpMethod)
            {
                case "GET":
                {
                    var @params = query.AllKeys
                        .OfType<string>()
                        .ToDictionary(key => key, key => query[key]);
                    Log.LogReceived(_logger, identifier, request.Url.Query);
                    payload = JsonSerializer.Serialize(new { action, @params });
                    break;
                }
                case "POST":
                {
                    if (!MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaType))
                    {
                        Log.LogCannotParseMediaType(_logger, request.ContentType ?? string.Empty);
                        response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                        response.Close();
                        return;
                    }

                    switch (mediaType.MediaType)
                    {
                        case "application/json":
                        {

                            using var reader = new StreamReader(request.InputStream);
                            var body = await reader.ReadToEndAsync(token);
                            Log.LogReceived(_logger, identifier, body);
                            payload = $"{{\"action\":\"{action}\",\"params\":{body}}}";
                            break;
                        }
                        case "application/x-www-form-urlencoded":
                        {
                            using var reader = new StreamReader(request.InputStream);
                            var body = await reader.ReadToEndAsync(token);
                            Log.LogReceived(_logger, identifier, body);
                            var @params = body.Split('&')
                                .Select(pair => pair.Split('=', 2))
                                .ToDictionary(pair => pair[0], pair => Uri.UnescapeDataString(pair[1]));
                            payload = JsonSerializer.Serialize(new { action, @params });
                            break;
                        }
                        default:
                        {
                            Log.LogUnsupportedContentType(_logger, request.ContentType ?? string.Empty);
                            response.StatusCode = (int)HttpStatusCode.NotAcceptable; // make them happy
                            response.Close();
                            return;
                        }
                    }
                    break;
                }
                default:
                {
                    Log.LogUnsupportedMethod(_logger, request.HttpMethod);
                    response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    response.Close();
                    return;
                }
            }

            Log.LogReceived(_logger, identifier, payload);
            _responses.TryAdd(identifier, response);
            OnMessageReceived?.Invoke(this, new MsgRecvEventArgs(payload, identifier));
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
            // ignore
        }
        catch (Exception e)
        {
            Log.LogHandleError(_logger, e);
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.Close();
        }
    }

    public async ValueTask SendJsonAsync<T>(T json, string? identifier = null,
        CancellationToken cancellationToken = default)
    {
        if (identifier is null) return;

        string payload = JsonSerializer.Serialize(json);
        Log.LogSend(_logger, identifier, payload);

        if (_responses.TryRemove(identifier, out var response))
        {
            response.ContentType = "application/json";
            response.ContentLength64 = System.Text.Encoding.UTF8.GetByteCount(payload);
            await using (var writer = new StreamWriter(response.OutputStream))
            {
                await writer.WriteAsync(payload);
            }

            response.Close();
        }
    }

    private static partial class Log
    {
        private enum EventIds
        {
            Started = 1,
            Request,
            Received,
            Send,

            StartFailed = 1001,
            CloseFailed,
            GetContextError,
            HandleError,
            AuthFailed,
            CannotParseMediaType,
            UnsupportedContentType,
            UnsupportedMethod,
        }

        [LoggerMessage(EventId = (int)EventIds.Started, Level = LogLevel.Information, Message = "HttpService started at {prefix}")]
        public static partial void LogStarted(ILogger logger, string prefix);

        [LoggerMessage(EventId = (int)EventIds.Request, Level = LogLevel.Information, Message = "Request(Conn: {identifier} from {ip})")]
        public static partial void LogRequest(ILogger logger, string identifier, string ip);

        [LoggerMessage(EventId = (int)EventIds.Received, Level = LogLevel.Information, Message = "Receive(Conn: {identifier}: {s})")]
        public static partial void LogReceived(ILogger logger, string identifier, string s);

        [LoggerMessage(EventId = (int)EventIds.Send, Level = LogLevel.Trace, Message = "Send(Conn: {identifier}: {s})")]
        public static partial void LogSend(ILogger logger, string identifier, string s);

        [LoggerMessage(EventId = (int)EventIds.CannotParseMediaType, Level = LogLevel.Warning, Message = "Cannot parse media type: {mediaType}")]
        public static partial void LogCannotParseMediaType(ILogger logger, string mediaType);

        [LoggerMessage(EventId = (int)EventIds.AuthFailed, Level = LogLevel.Warning, Message = "Conn: {identifier} auth failed")]
        public static partial void LogAuthFailed(ILogger logger, string identifier);

        [LoggerMessage(EventId = (int)EventIds.UnsupportedContentType, Level = LogLevel.Warning, Message = "Unsupported content type: {contentType}")]
        public static partial void LogUnsupportedContentType(ILogger logger, string contentType);

        [LoggerMessage(EventId = (int)EventIds.UnsupportedMethod, Level = LogLevel.Warning, Message = "Unsupported method: {method}")]
        public static partial void LogUnsupportedMethod(ILogger logger, string method);

        [LoggerMessage(EventId = (int)EventIds.HandleError, Level = LogLevel.Warning,
            Message = "An error occurred while handling the request")]
        public static partial void LogHandleError(ILogger logger, Exception e);

        [LoggerMessage(EventId = (int)EventIds.GetContextError, Level = LogLevel.Warning,
            Message = "An error occurred while getting the context")]
        public static partial void LogGetContextError(ILogger logger, Exception e);

        [LoggerMessage(EventId = (int)EventIds.CloseFailed, Level = LogLevel.Warning, Message = "Failed to gracefully close the listener")]
        public static partial void LogCloseFailed(ILogger logger, Exception e);

        [LoggerMessage(EventId = (int)EventIds.StartFailed, Level = LogLevel.Error,
            Message = "An error occurred while starting the listener")]
        public static partial void LogStartFailed(ILogger logger, Exception e);
    }
}
