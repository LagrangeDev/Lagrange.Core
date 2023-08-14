using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.System;

[ProtoContract]
internal class NTSysEventSub
{
    [ProtoMember(2)] public long State { get; set; }
    
    [ProtoMember(3)] public int Field3 { get; set; }
    
    [ProtoMember(4)] public long Field4 { get; set; }
    
    [ProtoMember(5)] public long Uin { get; set; }
    
    [ProtoMember(6)] public int Flag { get; set; }
    
    [ProtoMember(7)] public int On { get; set; }
    
    [ProtoMember(8)] public uint GroupUin { get; set; }
}