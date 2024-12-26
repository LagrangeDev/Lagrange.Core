using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;

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

        bot.Invoker.OnBotLogEvent += (_, @event) =>
        {
            Utility.Console.ChangeColorByTitle(@event.Level);
            Console.WriteLine(@event.ToString());
        };

        bot.Invoker.OnBotOnlineEvent += (_, @event) =>
        {
            Console.WriteLine(@event.ToString());
            WtLoginTest.SaveKeystore(bot.UpdateKeystore());
        };

        bot.Invoker.OnBotCaptchaEvent += (_, @event) =>
        {
            Console.WriteLine(@event.ToString());
            var captcha = Console.ReadLine();
            var randStr = Console.ReadLine();
            if (captcha != null && randStr != null) bot.SubmitCaptcha(captcha, randStr);
        };

        bot.Invoker.OnGroupInvitationReceived += (_, @event) =>
        {
            Console.WriteLine(@event.ToString());
        };

        await bot.LoginByPassword();

        var friendChain = MessageBuilder.Group(411240674)
                .Text("This is the friend message sent by Lagrange.Core")
                .Mention(1925648680);
        await bot.SendMessage(friendChain.Build());

        await Task.Delay(1000);
    }
}