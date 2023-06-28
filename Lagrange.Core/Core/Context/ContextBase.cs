using Lagrange.Core.Common;

namespace Lagrange.Core.Core.Context;

internal abstract class ContextBase
{
    protected readonly ContextCollection Collection;
    
    protected readonly BotKeystore Keystore;
    
    protected readonly BotAppInfo AppInfo;
    
    protected readonly BotDeviceInfo DeviceInfo;

    protected ContextBase(ContextCollection collection, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device)
    {
        Collection = collection;
        Keystore = keystore;
        AppInfo = appInfo;
        DeviceInfo = device;
    }
}