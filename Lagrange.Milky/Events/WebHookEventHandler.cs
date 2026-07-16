using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Events;
using Lagrange.Milky.Configurations;
using Lagrange.Milky.Events.Converters;
using Lagrange.Milky.Events.Extensions;
using Lagrange.Milky.Models;
using Lagrange.Milky.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lagrange.Milky.Events;

public class WebHookEventHandler(IServiceScopeFactory scopeFactory, ILogger<WebHookEventHandler> logger, MilkyConfiguration configuration, MilkyWebHookEventConfiguration webHookConfiguration, BotContext lagrange) : IHostedService, IGenericEventHandler
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ILogger<WebHookEventHandler> _logger = logger;
    private readonly BotContext _lagrange = lagrange;

    private readonly string? _token = configuration.AccessToken;

    private readonly string[] _targetUrls = webHookConfiguration.TargetUrls;

    private readonly HttpClient _http = new();

    private readonly CancellationTokenSource _cts = new();

    public Task StartAsync(CancellationToken ct)
    {
        _lagrange.RegisterConvertibleEvents(this);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken ct)
    {
        _lagrange.UnregisterConvertibleEvents(this);
        return Task.CompletedTask;
    }

    public async Task OnEvent<TEvent>(BotContext lagrange, TEvent @event) where TEvent : EventBase
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var converter = scope.ServiceProvider.GetRequiredService<IEventConverter<TEvent>>();
        byte[] bytes = Serializer.JsonSerializeToUtf8Bytes(new MilkyEvent
        {
            EventType = converter.Name,
            Time = DateTimeOffset.Now.ToUnixTimeSeconds(),
            SelfId = _lagrange.BotUin,
            Data = await converter.ConvertAsync(@event, _cts.Token)
        });

        await Task.WhenAll(_targetUrls.Select(async url =>
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new ByteArrayContent(bytes)
                };
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                if (_token != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                }
                await _http.SendAsync(request, _cts.Token);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to send webhook to {Url}", url);
            }
        }));
    }
}