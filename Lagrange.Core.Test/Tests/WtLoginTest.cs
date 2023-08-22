using System.Text.Json;
using System.Text.Json.Serialization;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Common.Interface.Api;

namespace Lagrange.Core.Test.Tests;

public class WtLoginTest
{
    public async Task FetchQrCode()
    {
        var deviceInfo = GetDeviceInfo();
        var keyStore = LoadKeystore() ?? new BotKeystore();
        
        var bot = BotFactory.Create(new BotConfig 
        {
            UseIPv6Network = false,
            GetOptimumServer = true,
            AutoReconnect = true,
            Protocol = Protocols.Linux
        }, deviceInfo, keyStore);

        bot.Invoker.OnBotLogEvent += (context, @event) =>
        {
            Utility.Console.ChangeColorByTitle(@event.Level);
            Console.WriteLine(@event.ToString());
        };
        
        bot.Invoker.OnBotOnlineEvent += (context, @event) =>
        {
            Console.WriteLine(@event.ToString());
            SaveKeystore(bot.UpdateKeystore());
        };

        var qrCode = await bot.FetchQrCode();
        if (qrCode != null)
        {
            await File.WriteAllBytesAsync("qr.png", qrCode);
            await bot.LoginByQrCode();
        }
    }

    public static BotDeviceInfo GetDeviceInfo()
    {
        if (File.Exists("Test/DeviceInfo.json"))
        {
            var info = JsonSerializer.Deserialize<BotDeviceInfo>(File.ReadAllText("Test/DeviceInfo.json"));
            if (info != null) return info;

            info = BotDeviceInfo.GenerateInfo();
            File.WriteAllText("Test/DeviceInfo.json", JsonSerializer.Serialize(info));
            return info;
        }
        
        var deviceInfo = BotDeviceInfo.GenerateInfo();
        File.WriteAllText("Test/DeviceInfo.json", JsonSerializer.Serialize(deviceInfo));
        return deviceInfo;
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