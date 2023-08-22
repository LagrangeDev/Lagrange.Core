namespace Lagrange.Core.Common.Interface.Api;

internal static class TestExt
{
    public static async Task<bool> GetHighwayAddress(this BotContext bot) 
        => await bot.ContextCollection.Business.OperationLogic.GetHighwayAddress();
}