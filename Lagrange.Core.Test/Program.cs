using Lagrange.Core.Common.Interface;
using Lagrange.Core.Common;
using Lagrange.Core.Test.Tests;
using System.Text.Json.Serialization;
using System.Text.Json;
using Lagrange.Core.Common.Interface.Api;

namespace Lagrange.Core.Test;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        // PowTest.Test();
        // BenchmarkRunner.Run<ProtoBufTest>(new DebugBuildConfig());


        var deviceInfo = GetDeviceInfo();
        var keyStore = LoadKeystore();

        /*
        if (keyStore == null)
        {
            Console.WriteLine("Keystore is not found");

            uint uin = 0;
            string password = "";
            uin = uint.Parse(Console.ReadLine() ?? "0");
            password = Console.ReadLine() ?? "";
            keyStore = new(uin, password);
        }

        var bot = BotFactory.Create(new BotConfig
        {
            UseIPv6Network = false,
            GetOptimumServer = true,
            AutoReconnect = true,
            Protocol = Protocols.AndroidPhone
        }, deviceInfo, keyStore);

        bot.Invoker.OnBotLogEvent += (_, @event) =>
        {
            Utility.Console.ChangeColorByTitle(@event.Level);
            Console.WriteLine(@event.ToString());
        };

        bot.Invoker.OnBotOnlineEvent += (context, @event) =>
        {
            Console.WriteLine(@event.ToString());
            SaveDeviceInfo(bot.UpdateDeviceInfo());
            SaveKeystore(bot.UpdateKeystore());
        };

        await new WtLoginTest().LoginByPassword(bot);
        */



        if (keyStore == null)
        {
            Console.WriteLine("KeyStore is not found");

            keyStore = new();
        }

        var bot = BotFactory.Create(new BotConfig
        {
            UseIPv6Network = false,
            GetOptimumServer = true,
            AutoReconnect = true,
            Protocol = Protocols.Linux
        }, deviceInfo, keyStore);

        bot.Invoker.OnBotLogEvent += (_, @event) =>
        {
            Utility.Console.ChangeColorByTitle(@event.Level);
            Console.WriteLine(@event.ToString());
        };

        bot.Invoker.OnBotOnlineEvent += (context, @event) =>
        {
            Console.WriteLine(@event.ToString());
            SaveDeviceInfo(bot.UpdateDeviceInfo());
            SaveKeystore(bot.UpdateKeystore());
        };

        if (keyStore.Session.TempPassword == null)
        {
            await new WtLoginTest().LoginByQrCode(bot);
            Console.WriteLine("Please login by QrCode first");
            return;
        }

        await new NTLoginTest().LoginByPassword(bot);


        /*
        bot.Invoker.OnGroupInvitationReceived += (_, @event) =>
        {
            Console.WriteLine(@event.ToString());
        };

        var friendChain = MessageBuilder.Group(411240674)
                .Text("This is the friend message sent by Lagrange.Core")
                .Mention(1925648680);
        await bot.SendMessage(friendChain.Build());

        await Task.Delay(1000);

        await bot.ContextCollection.Business.SendEvent(FetchAvatarEvent.Create("u_jwgijMpG1BnknQNubcy1bQ"));
        await Task.Delay(1000);
        */
    }


    public static void SaveDeviceInfo(BotDeviceInfo info) =>
    File.WriteAllText("Test/DeviceInfo.json", JsonSerializer.Serialize(info));

    public static BotDeviceInfo GetDeviceInfo()
    {
        try
        {
            var text = File.ReadAllText("Test/DeviceInfo.json");
            return JsonSerializer.Deserialize<BotDeviceInfo>(text) ?? BotDeviceInfo.GenerateInfo(Protocols.Linux);
        }
        catch
        {
            return BotDeviceInfo.GenerateInfo(Protocols.Linux);
        }
    }

    public static void SaveKeystore(BotKeystore keystore) =>
        File.WriteAllText("Test/Keystore.json", JsonSerializer.Serialize(keystore));

    public static BotKeystore? LoadKeystore()
    {
        try
        {
            var text = File.ReadAllText("Test/Keystore.json");
            return JsonSerializer.Deserialize<BotKeystore>(text, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
        }
        catch
        {
            return null;
        }
    }
}

