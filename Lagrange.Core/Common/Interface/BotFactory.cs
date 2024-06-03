namespace Lagrange.Core.Common.Interface;

public static class BotFactory
{
    /// <summary>
    /// Create new Bot from existing <see cref="BotKeystore"/> and <see cref="BotDeviceInfo"/>
    /// </summary>
    /// <param name="config">The config for Bot</param>
    /// <param name="deviceInfo">Existing DeviceInfo from deserialization</param>
    /// <param name="keystore">Existing Keystore from deserialization</param>
    /// <returns>Created BotContext Instance</returns>
    public static BotContext Create(BotConfig config, BotDeviceInfo deviceInfo, BotKeystore keystore) => 
        new(config, deviceInfo, keystore);
    
    /// <summary>
    /// Create new Bot from Password and uin
    /// </summary>
    /// <param name="config">The config for Bot</param>
    /// <param name="uin">Uin, if QrCode login is used, ensure the account that scans QrCode is consistent with this Uin</param>
    /// <param name="password">The password of account, for Password Login</param>
    /// <param name="device">Created device, should be serialized to files for next use</param>
    /// <returns>Created BotContext Instance</returns>
    public static BotContext Create(BotConfig config, uint uin, string password, out BotDeviceInfo device) => 
        new(config, device = BotDeviceInfo.GenerateInfo(), new BotKeystore(uin, password));
}