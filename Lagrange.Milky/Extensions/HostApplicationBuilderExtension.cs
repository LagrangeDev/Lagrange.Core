using System;
using System.IO;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Api;
using Lagrange.Milky.Api.Handlers;
using Lagrange.Milky.Api.Handlers.System;
using Lagrange.Milky.Caching;
using Lagrange.Milky.Captcha;
using Lagrange.Milky.Configurations;
using Lagrange.Milky.Converters;
using Lagrange.Milky.Events;
using Lagrange.Milky.Events.Extensions;
using Lagrange.Milky.Http;
using Lagrange.Milky.Logging;
using Lagrange.Milky.Login;
using Lagrange.Milky.Serialization;
using Lagrange.Milky.Signing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Lagrange.Milky.Extensions;

public static class HostApplicationBuilderExtension
{
    public static HostApplicationBuilder ConfigureLagrange(this HostApplicationBuilder builder)
    {
        var configuration = builder.Configuration.GetRequiredSection("Lagrange").Get<LagrangeConfiguration>()
            ?? throw new Exception("Failed to load 'Lagrange' configuration");
        builder.Services.AddSingleton(configuration);

        builder.Services.TryAddSingleton<BotSignProvider, HttpSigner>();
        builder.Services.AddSingleton(sp =>
        {
            var environment = sp.GetRequiredService<IHostEnvironment>();

            var config = new BotConfig
            {
                Protocol = (Protocols)configuration.Protocol.Platform,
                LogLevel = LogLevel.Trace,
                AutoReconnect = configuration.Server.AutoReconnect,
                UseIPv6Network = configuration.Server.UseIPv6Network,
                GetOptimumServer = configuration.Server.GetOptimumServer,
                AutoReLogin = configuration.Login.AutoReLogin,
                SignProvider = sp.GetRequiredService<BotSignProvider>(),
            };

            string ksPath = Path.Combine(environment.ContentRootPath, $"{configuration.Login.Uin}.ks");
            var ks = File.Exists(ksPath)
                ? Serializer.JsonDeserialize<BotKeystore>(File.ReadAllText(ksPath))
                    ?? throw new Exception("Failed to deserialize BotKeystore")
                : BotKeystore.CreateEmpty();

            var appInfo = configuration.Protocol.AppInfo
                ?? BotAppInfo.ProtocolToAppInfo[(Protocols)configuration.Protocol.Platform];

            return BotFactory.Create(config, ks, appInfo);
        });

        if (configuration.Login.UseOnlineCaptchResolver)
        {
            builder.Services.AddSingleton<ICaptchaResolver, OnlineCaptchaResolver>();
        }
        else builder.Services.AddSingleton<ICaptchaResolver, OnlineCaptchaResolver>();

        builder.Services.AddHostedService<LagrangeLoggingService>();
        builder.Services.AddHostedService<LoginService>();

        return builder;
    }

    public static HostApplicationBuilder ConfigureMilky(this HostApplicationBuilder builder)
    {
        var configuration = builder.Configuration.GetRequiredSection("Milky").Get<MilkyConfiguration>()
            ?? throw new Exception("Failed to load 'Milky' configuration");
        builder.Services.AddSingleton(configuration);

        builder.Services.AddSingleton<MessageCache>();
        builder.Services.AddHostedService<CacheService>();

        builder.Services.AddSingleton<MilkyConverter>();
        builder.Services.AddKeyedSingleton<IApiHandler, GetLoginInfoHandler>("get_login_info");
        builder.Services.AddEventConverters();

        builder.Services.AddHostedService<HttpService>();

        if (configuration.Api.Http != null)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpHandler, HttpApiHandler>(sp
                => ActivatorUtilities.CreateInstance<HttpApiHandler>(sp, configuration.Api.Http)
            ));
        }

        if (configuration.Event.WebSocket?.Enabled ?? false)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpHandler, WebSocketEventHandler>(sp
                => ActivatorUtilities.CreateInstance<WebSocketEventHandler>(sp, configuration.Event.WebSocket)
            ));
        }
        if (configuration.Event.SSE?.Enabled ?? false)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpHandler, SSEEventHandler>(sp
                => ActivatorUtilities.CreateInstance<SSEEventHandler>(sp, configuration.Event.SSE)
            ));
        }
        if (configuration.Event.WebHook?.Enabled ?? false)
        {
            builder.Services.AddHostedService(sp
                => ActivatorUtilities.CreateInstance<WebHookEventHandler>(sp, configuration.Event.WebHook)
            );
        }

        return builder;
    }
}