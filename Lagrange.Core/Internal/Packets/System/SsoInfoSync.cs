using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.System;

/// <summary>
/// trpc.msg.register_proxy.RegisterProxy.SsoInfoSync
/// </summary>
[ProtoContract]
internal class SsoInfoSyncRequest
{
    [ProtoMember(1)] public uint SyncFlag { get; set; }
    
    [ProtoMember(2)] public uint? ReqRandom { get; set; }
    
    [ProtoMember(4)] public uint? CurActiveStatus { get; set; }
    
    [ProtoMember(5)] public ulong? GroupLastMsgTime { get; set; }
    
    [ProtoMember(6)] public SsoC2CInfoSync? C2CInfoSync { get; set; }
    
    [ProtoMember(8)] public NormalConfig? NormalConfig { get; set; }
    
    [ProtoMember(9)] public RegisterInfo? RegisterInfo { get; set; }
    
    [ProtoMember(10)] public UnknownStructure? UnknownStructure { get; set; }
    
    [ProtoMember(11)] public CurAppState? AppState { get; set; }
}

[ProtoContract]
internal class SsoC2CMsgCookie
{
    [ProtoMember(1)] public ulong C2CLastMsgTime { get; set; }
}

[ProtoContract]
internal class SsoC2CInfoSync
{
    [ProtoMember(1)] public SsoC2CMsgCookie? C2CMsgCookie { get; set; }
    
    [ProtoMember(2)] public ulong C2CLastMsgTime { get; set; }
    
    [ProtoMember(3)] public SsoC2CMsgCookie? LastC2CMsgCookie { get; set; }
}

[ProtoContract]
internal class NormalConfig
{
    [ProtoMember(1)] public Dictionary<uint, int>? IntCfg { get; set; }
}

[ProtoContract]
internal class CurAppState
{
    [ProtoMember(1)] public uint? IsDelayRequest { get; set; }
    
    [ProtoMember(2)] public uint? AppStatus { get; set; }
    
    [ProtoMember(3)] public uint? SilenceStatus { get; set; }
}

[ProtoContract]
internal class UnknownStructure
{
    [ProtoMember(1)] public uint? GroupCode { get; set; }
    
    [ProtoMember(2)] public uint Flag2 { get; set; }
}