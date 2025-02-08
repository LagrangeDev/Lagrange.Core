using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.OneBot.Core.Login;
using Lagrange.OneBot.Core.Network;
using Lagrange.OneBot.Core.Network.Service;
using Lagrange.OneBot.Core.Notify;
using Lagrange.OneBot.Core.Operation;
using Lagrange.OneBot.Message;
using Lagrange.OneBot.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Realms;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Lagrange.OneBot.Extensions;

public static class HostApplicationBuilderExtension
{
    public static HostApplicationBuilder ConfigureLagrangeCore(this HostApplicationBuilder builder)
    {
        builder.Services
            .AddSingleton<OneBotSigner>() // Signer
            .AddSingleton((services) => // BotConfig
            {
                var configuration = services.GetRequiredService<IConfiguration>();

                return new BotConfig
                {
                    Protocol = configuration["Account:Protocol"] switch
                    {
                        "Windows" => Protocols.Windows,
                        "MacOs" => Protocols.MacOs,
                        _ => Protocols.Linux,
                    },
                    AutoReconnect = configuration.GetValue("Account:AutoReconnect", true),
                    UseIPv6Network = configuration.GetValue("Account:UseIPv6Network", false),
                    GetOptimumServer = configuration.GetValue("Account:GetOptimumServer", true),
                    AutoReLogin = configuration.GetValue("Account:AutoReLogin", true),
                    CustomSignProvider = services.GetRequiredService<OneBotSigner>()
                };
            })
            .AddSingleton((services) => // Device
            {
                var configuration = services.GetRequiredService<IConfiguration>();
                string path = configuration["ConfigPath:DeviceInfo"] ?? "device.json";

                var device = File.Exists(path)
                    ? JsonSerializer.Deserialize<BotDeviceInfo>(File.ReadAllText(path)) ?? BotDeviceInfo.GenerateInfo()
                    : BotDeviceInfo.GenerateInfo();

                string deviceJson = JsonSerializer.Serialize(device);
                File.WriteAllText(path, deviceJson);

                return device;
            })
            .AddSingleton((services) => // Keystore
            {
                var configuration = services.GetRequiredService<IConfiguration>();
                string path = configuration["ConfigPath:Keystore"] ?? "keystore.json";

                return File.Exists(path)
                    ? JsonSerializer.Deserialize<BotKeystore>(File.ReadAllText(path)) ?? new()
                    : new();
            })
            .AddSingleton((services) => services.GetRequiredService<OneBotSigner>().GetAppInfo()) // AppInfo
            .AddSingleton((services) => BotFactory.Create( // BotContext
                services.GetRequiredService<BotConfig>(),
                services.GetRequiredService<BotDeviceInfo>(),
                services.GetRequiredService<BotKeystore>(),
                services.GetRequiredService<BotAppInfo>()
            ))
            .AddHostedService<LoginService>();

        return builder;
    }

    public static HostApplicationBuilder ConfigureOneBot(this HostApplicationBuilder builder)
    {
        builder.Services.AddOptions()
            .AddSingleton(services => // Realm Configuration
            {
                var logger = services.GetRequiredService<ILogger<RealmConfiguration>>();
                var configuration = services.GetRequiredService<IConfiguration>();
                var host = services.GetRequiredService<IHost>();

                string? dpath = configuration["ConfigPath:Database"];

                // Check LiteDB
                string litedb = dpath ?? $"./lagrange-{configuration["Account:Uin"]}.db";
                if (File.Exists(litedb)) {
                    logger.LogCritical("Found LiteDB database, currently Lagrange.OneBot has been migrated to Realm database");
                    logger.LogCritical("Please delete {} if you no longer need the data in the database; otherwise, please refer to https://lagrangedev.github.io/Lagrange.Doc/Lagrange.OneBot/#从-litedb-迁移到-realm to migrate the database to Realm", litedb);
                    logger.LogCritical("Press any key to terminate the program");
                    Console.ReadKey(true);
                    host.StopAsync(default);
                }

                string prefix = dpath ?? $"./lagrange-{configuration["Account:Uin"]}-db";
                if (!Directory.Exists(prefix)) Directory.CreateDirectory(prefix);
                string path = Path.GetFullPath(Path.Join(prefix, ".realm"));

                return new RealmConfiguration(path)
                {
                    SchemaVersion = 2,
                    MigrationCallback = null,
                };
            })
            .AddSingleton<RealmHelper>()

            // // OneBot Netword Service
            .AddSingleton<LagrangeWebSvcCollection>()

            .AddScoped<ILagrangeWebServiceFactory<ForwardWSService>, ForwardWSServiceFactory>()
            .AddScoped<ForwardWSService>()

            .AddScoped<ILagrangeWebServiceFactory<ReverseWSService>, ReverseWSServiceFactory>()
            .AddScoped<ReverseWSService>()

            .AddScoped<ILagrangeWebServiceFactory<HttpService>, HttpServiceFactory>()
            .AddScoped<HttpService>()

            .AddScoped<ILagrangeWebServiceFactory<HttpPostService>, HttpPostServiceFactory>()
            .AddScoped<HttpPostService>()

            .AddScoped<ILagrangeWebServiceFactory, DefaultLagrangeWebServiceFactory>()
            .AddScoped(services => services.GetRequiredService<ILagrangeWebServiceFactory>().Create()
                ?? throw new Exception("Invalid conf detected"))

            // // OneBot Misc Service
            .AddSingleton<MusicSigner>()
            .AddSingleton<NotifyService>()
            .AddSingleton<MessageService>()
            .AddSingleton<OperationService>();

        return builder;
    }
}