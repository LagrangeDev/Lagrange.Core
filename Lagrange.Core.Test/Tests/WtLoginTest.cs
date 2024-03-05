using Lagrange.Core.Common.Interface.Api;

namespace Lagrange.Core.Test.Tests;

public class WtLoginTest
{
    public async Task LoginByPassword(BotContext bot)
    {
        bot.Invoker.OnBotCaptchaEvent += (context, @event) =>
        {
            Console.WriteLine(@event.Url);
            var ticket = Console.ReadLine();
            if (ticket != null)
                bot.SubmitCaptcha(ticket);
        };

        bot.Invoker.OnBotNewDeviceVerify += async (context, @event) =>
        {
            Console.WriteLine(@event.PhoneNumber);
            if (await bot.SendSmsCode())
            {
                var smsCode = Console.ReadLine();
                if (smsCode != null)
                    bot.SubmitSmsCode(smsCode);
            }
            else
            {
                Console.WriteLine("Send SmsCode Fail");
            }
        };

        _ = bot.LoginByPassword();
    }

    public async Task LoginByQrCode(BotContext bot)
    {
        var qrCode = await bot.FetchQrCode();
        if (qrCode != null)
        {
            await File.WriteAllBytesAsync("qr.png", qrCode.Value.QrCode);
            await bot.LoginByQrCode();
        }
    }
}