using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Common.Interface.Api;

namespace Lagrange.Core.Test.Tests;

// ReSharper disable once InconsistentNaming

public class NTLoginTest
{
    public async Task LoginByPassword()
    {
        var deviceInfo = WtLoginTest.GetDeviceInfo();
        var keyStore = WtLoginTest.LoadKeystore();
        
        if (keyStore == null)
        {
            Console.WriteLine("Please login by QrCode first");
            return;
        }

        var bot = BotFactory.Create(new BotConfig() 
        {
            UseIPv6Network = false,
            GetOptimumServer = true,
            AutoReconnect = true,
            Protocol = Protocols.Linux
        }, deviceInfo, keyStore);
        
        bot.Invoker.OnBotLogEvent += (context, @event) =>
        {
            Console.WriteLine(@event.ToString());
        };
        
        bot.Invoker.OnBotOnlineEvent += (context, @event) =>
        {
            Console.WriteLine(@event.ToString());
            WtLoginTest.SaveKeystore(bot.UpdateKeystore());
        };

        await bot.LoginByPassword();
    }
}