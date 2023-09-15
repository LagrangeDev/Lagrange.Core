using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Action;

[ProtoContract]
internal class SendMessageResponse
{
	[ProtoMember(1)] public int Result { get; set; }
	
	[ProtoMember(2)] public string? ErrMsg { get; set; }
	
	[ProtoMember(3)] public uint Timestamp1 { get; set; }
	
	[ProtoMember(10)] public uint Field10 { get; set; }
	
	[ProtoMember(11)] public uint? Sequence { get; set; }
	
	[ProtoMember(12)] public uint Timestamp2 { get; set; }
	
	[ProtoMember(14)] public uint Field14 { get; set; }
}