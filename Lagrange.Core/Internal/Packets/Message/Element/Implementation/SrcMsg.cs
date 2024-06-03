using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

[ProtoContract]
internal partial class SrcMsg
{
	[ProtoMember(1)] public List<uint>? OrigSeqs { get; set; }
    
    [ProtoMember(2)] public ulong SenderUin { get; set; }
    
    [ProtoMember(3)] public int? Time { get; set; }
    
    [ProtoMember(4)] public int? Flag { get; set; }
    
    [ProtoMember(5)] public List<Elem>? Elems { get; set; }
    
    [ProtoMember(6)] public int? Type { get; set; }
    
	[ProtoMember(7)] public byte[]? RichMsg { get; set; }

	[ProtoMember(8)] public byte[]? PbReserve { get; set; }

	[ProtoMember(9)] public byte[]? SourceMsg { get; set; } // Metadata

	[ProtoMember(10)] public ulong? ToUin { get; set; }

	[ProtoMember(11)] public byte[]? TroopName { get; set; }
}