using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message;

[ProtoContract]
internal class C2CRecallMsg
{
    [ProtoMember(1)] public uint Type { get; set; } // 1
    
    [ProtoMember(3)] public string TargetUid { get; set; } // PeerUid in the binary
    
    [ProtoMember(4)] public C2CRecallMsgInfo Info { get; set; }
    
    [ProtoMember(5)] public C2CRecallMsgSettings Settings { get; set; }
    
    [ProtoMember(6)] public bool Field6 { get; set; } // 1
}

[ProtoContract]
internal class C2CRecallMsgInfo
{
    [ProtoMember(1)] public uint ClientSequence { get; set; }
    
    [ProtoMember(2)] public uint Random { get; set; }
    
    [ProtoMember(3)] public ulong MessageId { get; set; } // 0x1000000 << 32 | Random
    
    [ProtoMember(4)] public uint Timestamp { get; set; }
    
    [ProtoMember(5)] public uint Field5 { get; set; } // 0
    
    [ProtoMember(6)] public uint MessageSequence { get; set; } // 700
}

[ProtoContract]
internal class C2CRecallMsgSettings
{
    [ProtoMember(1)] public bool Field1 { get; set; } // 0
    
    [ProtoMember(2)] public bool Field2 { get; set; } // 0
}