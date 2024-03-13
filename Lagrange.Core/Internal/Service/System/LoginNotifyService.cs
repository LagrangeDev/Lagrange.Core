using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Utility.Binary.JceStruct;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(LoginNotifyEvent))]
[Service("StatSvc.SvcReqMSFLoginNotify")]
internal class LoginNotifyService : BaseService<LoginNotifyEvent>
{
    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out LoginNotifyEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var jce = new JceReader(input).Deserialize(true);
        var wrapper = (JceStruct)((JceStruct)((Dictionary<object, object>)((Dictionary<object, object>)((JceStruct)jce[7])[0])["SvcReqMSFLoginNotify"])["QQService.SvcReqMSFLoginNotify"])[0];

        output = LoginNotifyEvent.Result((byte)wrapper[1] == 1, (uint)(int)wrapper[0], (string)wrapper[4], (string)wrapper[5]);
        extraEvents = null;
        return true;
    }
}