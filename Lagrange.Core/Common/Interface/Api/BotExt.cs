namespace Lagrange.Core.Common.Interface.Api;

public static class BotExt
{
    public static async Task<byte[]?> FetchQrCode(this BotContext bot)
        => await bot.ContextCollection.Business.WtExchangeLogic.FetchQrCode();
    
    /// <summary>
    /// Use this method to login by QrCode, you should call <see cref="FetchQrCode"/> first
    /// </summary>
    public static async Task LoginByQrCode(this BotContext bot)
        => await bot.ContextCollection.Business.WtExchangeLogic.LoginByQrCode();
    
    /// <summary>
    /// Use this method to login by password, EasyLogin may be preformed if there is sig in <see cref="BotKeystore"/>
    /// </summary>
    public static async Task<bool> LoginByPassword(this BotContext bot)
        => await bot.ContextCollection.Business.WtExchangeLogic.LoginByPassword();
    
    public static BotKeystore UpdateKeystore(this BotContext bot)
        => bot.ContextCollection.Keystore;
}