using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.C2C;

[ProtoContract]
internal class C2CMsgInfo
{
	[ProtoMember(1)] public ulong FromUin { get; set; }
    
	[ProtoMember(2)] public ulong ToUin { get; set; }
	
	[ProtoMember(3)] public int MsgSeq { get; set; }
	
	[ProtoMember(4)] public long MsgUid { get; set; }
	
	[ProtoMember(5)] public long MsgTime { get; set; }
	
	[ProtoMember(6)] public int MsgRandom { get; set; }
	
	[ProtoMember(7)] public int PkgNum { get; set; }
	
	[ProtoMember(8)] public int PkgIndex { get; set; }
	
	[ProtoMember(9)] public int DivSeq { get; set; }
	
	[ProtoMember(10)] public int MsgType { get; set; }
	
	[ProtoMember(20)] public RoutingHead RoutingHead { get; set; }
}