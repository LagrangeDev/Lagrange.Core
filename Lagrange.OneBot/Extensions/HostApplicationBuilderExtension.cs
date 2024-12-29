using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.OneBot.Core.Login;
using Lagrange.OneBot.Core.Network;
using Lagrange.OneBot.Core.Network.Service;
using Lagrange.OneBot.Core.Notify;
using Lagrange.OneBot.Core.Operation;
using Lagrange.OneBot.Database;
using Lagrange.OneBot.Message;
using Lagrange.OneBot.Utility;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            .AddSingleton(services => // Database
            {
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<LiteDatabase>>();

                BsonMapper.Global.TrimWhitespace = false;
                BsonMapper.Global.EmptyStringToNull = false;

                // Specify ctor for some classes
                BsonMapper.Global.RegisterType(
                    LiteDbUtility.IMessageEntitySerialize,
                    LiteDbUtility.IMessageEntityDeserialize
                );

                string path = configuration["ConfigPath:Database"] ?? $"lagrange-{configuration["Account:Uin"]}.db";

                bool isFirstCreate = false;
                if (!File.Exists(path)) isFirstCreate = true;

                var db = new LiteDatabase(path)
                {
                    CheckpointSize = 50
                };

                string[] expressions = ["$.Sequence", "$.MessageId", "$.FriendUin", "$.GroupUin"];

                bool hasFirstIndex = false;
                var indexes = db.GetCollection("$indexes");
                foreach (var expression in expressions)
                {
                    if (indexes.Exists(Query.EQ("expression", expression))) continue;

                    logger.LogWarning("In the database index");
                    logger.LogWarning("Depending on the size of the database will consume some time and memory");
                    logger.LogWarning("Not yet finished, please wait...");

                    hasFirstIndex = true;
                    break;
                }

                var records = db.GetCollection<MessageRecord>();
                foreach (var expression in expressions)
                {
                    records.EnsureIndex(BsonExpression.Create(expression));
                }

                // Skipping the first database creation is a restart after indexing
                if (!isFirstCreate && hasFirstIndex)
                {
                    db.Dispose(); // Ensure that the database is written correctly
                    logger.LogInformation("Indexing Complete! Press any key to close and restart the program manually!");
                    Console.ReadKey(true);
                    Environment.Exit(0);
                }
                return db;
            })

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