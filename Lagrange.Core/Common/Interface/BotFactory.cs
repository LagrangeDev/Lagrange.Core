namespace Lagrange.Core.Common.Interface;

public static class BotFactory
{
    public static BotContext Create(BotConfig config, BotDeviceInfo deviceInfo, BotKeystore keystore) => 
        new(config, deviceInfo, keystore);
}