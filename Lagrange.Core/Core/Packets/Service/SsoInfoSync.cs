using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service;

#pragma warning disable CS8618

/// <summary>
/// trpc.msg.register_proxy.RegisterProxy.SsoInfoSync
/// </summary>
[ProtoContract]
internal class SsoInfoSync
{
    [ProtoMember(1)] public uint SyncType { get; set; } // 15 for partial, 143 for full
    
    [ProtoMember(2)] public uint RandomSeq { get; set; } // req_random
    
    [ProtoMember(4)] public uint Type { get; set; } // 2
    
    [ProtoMember(5)] public uint GroupLastMsgTime { get; set; } // 0 or req_group_last_msg_time
    
    [ProtoMember(6)] public SsoInfoSyncC2C C2C { get; set; } // 0 or req_c2c_msg_last_time
}

[ProtoContract]
internal class SsoInfoSyncC2C
{
    [ProtoMember(1)] public byte[] Empty { get; set; } // empty
    
    [ProtoMember(2)] public uint C2CMsgLastTime { get; set; } // 0 or req_c2c_msg_last_time
}