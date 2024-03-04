using System.Collections.Concurrent;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;
using Lagrange.Core;
using Lagrange.OneBot.Core.Network.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lagrange.OneBot.Core.Network.Service;

public sealed partial class HttpService : ILagrangeWebService
{
    public event EventHandler<MsgRecvEventArgs>? OnMessageReceived;

    private readonly HttpServiceOptions _options;

    private readonly ILogger _logger;

    private readonly BotContext _context;

    private readonly HttpListener _listener;

    private readonly ConcurrentDictionary<string, HttpListenerResponse> _responses;

    private readonly string _accessToken;

    public HttpService(IOptionsSnapshot<HttpServiceOptions> options, ILogger<HttpService> logger, BotContext context)
    {
        _options = options.Value;
        _logger = logger;
        _context = context;
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://{_options.Host}:{_options.Port}/");
        _responses = new ConcurrentDictionary<string, HttpListenerResponse>();
        _accessToken = _options.AccessToken ?? "";
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        uint port = _options.Port;
        if (IsPortInUse(port))
        {
            Log.LogPortInUse(_logger, port);
            return Task.CompletedTask;
        }

        Task.Run(() =>
        {
            _listener.Start();
            while (true)
            {
                try
                {
                    var context = _listener.GetContext();
                    Task.Run(() => HandleRequest(context.Request, context.Response), cancellationToken);
                }
                catch (HttpListenerException)
                {
                    break;
                }
            }
        }, cancellationToken);

        return Task.CompletedTask;
    }

    private void HandleRequest(HttpListenerRequest request, HttpListenerResponse response)
    {
        string identifier = Guid.NewGuid().ToString();
        if (!string.IsNullOrEmpty(_accessToken))
        {
            var authorization = request.Headers.Get("Authorization");
            if (authorization != $"Bearer {_accessToken}")
            {
                Log.LogAuthFailed(_logger, identifier);
                response.StatusCode = authorization is null ? 401 : 403;
                response.Close();
                return;
            }
        }

        var action = request.Url!.AbsolutePath[1..];
        string payload;
        if (request.HttpMethod == "GET")
        {
            var @params = request.QueryString.AllKeys
                                 .Where(key => key is not null)
                                 .ToDictionary(key => key!, key => request.QueryString[key]);
            payload = JsonSerializer.Serialize(new { action, @params });
        }
        else if (request.HttpMethod == "POST")
        {
            if (request.ContentType == "application/json")
            {
                using var reader = new StreamReader(request.InputStream);
                payload = reader.ReadToEnd();
            }
            else if (request.ContentType == "application/x-www-form-urlencoded")
            {
                using var reader = new StreamReader(request.InputStream);
                var body = reader.ReadToEnd();
                var @params = body.Split('&')
                                 .Select(pair => pair.Split('='))
                                 .ToDictionary(pair => pair[0], pair => pair[1]);
                payload = JsonSerializer.Serialize(new { action, @params });
            }
            else
            {
                Log.LogUnsupportedContentType(_logger, request.ContentType ?? string.Empty);
                response.StatusCode = 406; // make them happy
                response.Close();
                return;
            }
        }
        else
        {
            Log.LogUnsupportedMethod(_logger, request.HttpMethod);
            response.StatusCode = 405;
            response.Close();
            return;
        }

        Log.LogReceived(_logger, identifier, payload);
        _responses.TryAdd(identifier, response);
        OnMessageReceived?.Invoke(this, new MsgRecvEventArgs(payload, identifier));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _listener.Stop();
        _listener.Close();
        return Task.CompletedTask;
    }

    public async ValueTask SendJsonAsync<T>(T json, string? identifier = null, CancellationToken cancellationToken = default)
    {
        if (identifier is null) return;

        string payload = JsonSerializer.Serialize(json);
        Log.LogSend(_logger, identifier, payload);

        if (_responses.TryRemove(identifier, out var response))
        {
            response.ContentType = "application/json";
            response.ContentLength64 = System.Text.Encoding.UTF8.GetByteCount(payload);
            using (var writer = new StreamWriter(response.OutputStream))
            {
                await writer.WriteAsync(payload);
            }
            response.Close();
        }
    }

    private static bool IsPortInUse(uint port) =>
        IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Any(endpoint => endpoint.Port == port);

    private static partial class Log
    {
        [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Receive(Conn: {identifier}: {s})")]
        public static partial void LogReceived(ILogger logger, string identifier, string s);

        [LoggerMessage(EventId = 1, Level = LogLevel.Trace, Message = "Send(Conn: {identifier}: {s})")]
        public static partial void LogSend(ILogger logger, string identifier, string s);

        [LoggerMessage(EventId = 2, Level = LogLevel.Critical, Message = "The port {port} is in use, service failed to start")]
        public static partial void LogPortInUse(ILogger logger, uint port);

        [LoggerMessage(EventId = 3, Level = LogLevel.Critical, Message = "Conn: {identifier} auth failed")]
        public static partial void LogAuthFailed(ILogger logger, string identifier);

        [LoggerMessage(EventId = 4, Level = LogLevel.Critical, Message = "Unsupported content type: {contentType}")]
        public static partial void LogUnsupportedContentType(ILogger logger, string contentType);

        [LoggerMessage(EventId = 5, Level = LogLevel.Critical, Message = "Unsupported method: {method}")]
        public static partial void LogUnsupportedMethod(ILogger logger, string method);
    }
}
