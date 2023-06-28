using ProtoBuf;

// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Core.Packets.Action;

[ProtoContract]
internal class FirstViewReq
{
	[ProtoMember(1)] public int LastMsgTime { get; set; }
	
	[ProtoMember(3)] public int Seq { get; set; }
	
	[ProtoMember(4)] public int DirectMessageFlag { get; set; }
	
	[ProtoMember(5)] public int UdcFlag { get; set; }
}