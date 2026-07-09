using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<InfoSyncEventReq>(Protocols.PC)]
[Service("trpc.msg.register_proxy.RegisterProxy.SsoInfoSync")]
internal class InfoSyncService : BaseService<InfoSyncEventReq, InfoSyncEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(InfoSyncEventReq input, BotContext context)
    {
        var packet = new SsoInfoSyncRequest
        {
            SyncFlag = 735,
            ReqRandom = (uint)Random.Shared.Next(),
            CurActiveStatus = 2,
            GroupLastMsgTime = 0,
            C2CSyncInfo = new SsoC2CSyncInfo
            {
                C2CMsgCookie = new SsoC2CMsgCookie { C2CLastMsgTime = 0 },
                C2CLastMsgTime = 0,
                LastC2CMsgCookie = new SsoC2CMsgCookie { C2CLastMsgTime = 0 }
            },
            NormalConfig = new NormalConfig { IntCfg = new Dictionary<uint, int>() },
            RegisterInfo = new RegisterInfo
            {
                Guid = Convert.ToHexString(context.Keystore.Guid),
                KickPC = 0,
                BuildVer = context.AppInfo.CurrentVersion,
                IsFirstRegisterProxyOnline = 1,
                LocaleId = 2052,
                DeviceInfo = new DeviceInfo
                {
                    DevName = context.Keystore.DeviceName,
                    DevType = context.AppInfo.Kernel,
                    OsVer = "",
                    Brand = "",
                    VendorOsName = context.AppInfo.VendorOs,
                },
                SetMute = 0,
                RegisterVendorType = 6,
                RegType = 0,
                BusinessInfo = new OnlineBusinessInfo { NotifySwitch = 1, BindUinNotifySwitch = 1 },
                BatteryStatus = 0,
                Field12 = 1
            },
            Unknown = new Dictionary<uint, uint> { { 0, 2 } },
            AppState = new CurAppState { IsDelayRequest = 0, AppStatus = 0, SilenceStatus = 0 }
        };

        return new ValueTask<ReadOnlyMemory<byte>>(ProtoHelper.Serialize(packet));
    }

    protected override ValueTask<InfoSyncEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var packet = ProtoHelper.Deserialize<SsoSyncInfoResponse>(input.Span);

        return new ValueTask<InfoSyncEventResp>(new InfoSyncEventResp(packet.RegisterResponse?.Msg ?? "failed"));
    }
}

[EventSubscribe<InfoSyncEventReq>(Protocols.Android)]
[Service("trpc.msg.register_proxy.RegisterProxy.SsoInfoSync")]
internal class InfoSyncServiceAndroid : BaseService<InfoSyncEventReq, InfoSyncEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(InfoSyncEventReq input, BotContext context)
    {
        var packet = new SsoInfoSyncRequest
        {
            SyncFlag = 735,
            ReqRandom = (uint)Random.Shared.Next(),
            CurActiveStatus = 2,
            GroupLastMsgTime = 0,
            C2CSyncInfo = new SsoC2CSyncInfo
            {
                C2CMsgCookie = new SsoC2CMsgCookie { C2CLastMsgTime = 0 },
                C2CLastMsgTime = 0,
                LastC2CMsgCookie = new SsoC2CMsgCookie { C2CLastMsgTime = 0 }
            },
            NormalConfig = new NormalConfig
            {
                IntCfg = new Dictionary<uint, int>
                {
                    { 46, 0 },
                    { 283, 0 }
                }
            },
            RegisterInfo = new RegisterInfo
            {
                Guid = Convert.ToHexString(context.Keystore.Guid),
                KickPC = 0,
                BuildVer = context.AppInfo.CurrentVersion,
                IsFirstRegisterProxyOnline = 0,
                LocaleId = 2052,
                DeviceInfo = new DeviceInfo
                {
                    DevName = context.Keystore.DeviceName,
                    DevType = "alioth",
                    OsVer = "13",
                    Brand = "MIUI",
                    VendorOsName = "V816",
                },
                SetMute = 0,
                RegisterVendorType = 6,
                RegType = 1,
                BusinessInfo = new OnlineBusinessInfo { NotifySwitch = 1, BindUinNotifySwitch = 1 },
                BatteryStatus = 0
            },
            Unknown = new Dictionary<uint, uint> { { 0, 1 } },
            AppState = new CurAppState { IsDelayRequest = 0, AppStatus = 1, SilenceStatus = 0 }
        };

        return new ValueTask<ReadOnlyMemory<byte>>(ProtoHelper.Serialize(packet));
    }

    protected override ValueTask<InfoSyncEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var packet = ProtoHelper.Deserialize<SsoSyncInfoResponse>(input.Span);

        return new ValueTask<InfoSyncEventResp>(new InfoSyncEventResp(packet.RegisterResponse?.Msg ?? "failed"));
    }
}