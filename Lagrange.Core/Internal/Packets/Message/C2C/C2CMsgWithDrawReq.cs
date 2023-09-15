using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.C2C;

[ProtoContract]
internal class C2CMsgWithDrawReq
{
	[ProtoMember(1)] public C2CMsgInfo[] MsgInfo { get; set; }
	
	[ProtoMember(2)] public int LongMessageFlag { get; set; }
	
	[ProtoMember(3)] public byte[] Reserved { get; set; }
	
	[ProtoMember(4)] public int SubCmd { get; set; }
}