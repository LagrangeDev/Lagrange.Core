using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Message.Element.Implementation;

[ProtoContract]
internal class RichMsg
{
	[ProtoMember(1)] public byte[]? Template1 { get; set; }
	
	[ProtoMember(2)] public int? ServiceId { get; set; }
	
	[ProtoMember(3)] public byte[]? MsgResId { get; set; }
	
	[ProtoMember(4)] public int? Rand { get; set; }
	
	[ProtoMember(5)] public int? Seq { get; set; }
}