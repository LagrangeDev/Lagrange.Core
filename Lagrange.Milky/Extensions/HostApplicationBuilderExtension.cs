using System;
using System.IO;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Events.EventArgs;
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
            ?? throw new Exception();
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
                    ?? throw new Exception()
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
        var configuration = builder.Configuration.GetRequiredSection("Milky").Get<MilkyConfiguration>() ?? new();
        builder.Services.AddSingleton(configuration);

        builder.Services.AddSingleton<MessageCache>();
        builder.Services.AddHostedService<CacheService>();

        builder.Services.AddSingleton<MilkyConverter>();
        builder.Services.AddEventConverters();


        if (configuration.Http != null)
        {
            if (configuration.Http.WS?.Event != null)
            {
                builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpHandler, WebSocketEventHandler>(sp
                    => ActivatorUtilities.CreateInstance<WebSocketEventHandler>(sp, configuration.Http.WS.Event)
                ));
            }

            builder.Services.AddHostedService(sp
                => ActivatorUtilities.CreateInstance<HttpService>(sp, configuration.Http)
            );
        }

        return builder;
    }
}