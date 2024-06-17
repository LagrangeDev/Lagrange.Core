using Lagrange.Core.Event;

namespace Lagrange.Core.Common.Interface.Api;

public static class BotExt
{
    /// <summary>
    /// Fetch the qrcode for QRCode Login
    /// </summary>
    /// <returns>return url and qrcode image in PNG format</returns>
    public static Task<(string Url, byte[] QrCode)?> FetchQrCode(this BotContext bot)
        => bot.ContextCollection.Business.WtExchangeLogic.FetchQrCode();
    
    /// <summary>
    /// Use this method to login by QrCode, you should call <see cref="FetchQrCode"/> first
    /// </summary>
    public static Task LoginByQrCode(this BotContext bot)
        => bot.ContextCollection.Business.WtExchangeLogic.LoginByQrCode();
    
    /// <summary>
    /// Use this method to login by password, EasyLogin may be preformed if there is sig in <see cref="BotKeystore"/>
    /// </summary>
    public static Task<bool> LoginByPassword(this BotContext bot)
        => bot.ContextCollection.Business.WtExchangeLogic.LoginByPassword();
    
    /// <summary>
    /// Submit the captcha of the url given by the <see cref="EventInvoker.OnBotCaptchaEvent"/>
    /// </summary>
    /// <returns>Whether the captcha is submitted successfully</returns>
    public static bool SubmitCaptcha(this BotContext bot, string ticket, string randStr)
        => bot.ContextCollection.Business.WtExchangeLogic.SubmitCaptcha(ticket, randStr);
    
    public static Task<bool> SetNeedToConfirmSwitch(this BotContext bot, bool needToConfirm) 
        => bot.ContextCollection.Business.OperationLogic.SetNeedToConfirmSwitch(needToConfirm);
    
    /// <summary>
    /// Use this method to update keystore, so EasyLogin may be preformed next time by using this keystore
    /// </summary>
    /// <returns>BotKeystore instance</returns>
    public static BotKeystore UpdateKeystore(this BotContext bot)
        => bot.ContextCollection.Keystore;

    /// <summary>
    /// Use this method to update device info
    /// </summary>
    /// <param name="bot"></param>
    public static BotDeviceInfo UpdateDeviceInfo(this BotContext bot)
        => bot.ContextCollection.Device;
}