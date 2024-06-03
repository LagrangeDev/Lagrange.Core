using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Component;

[ProtoContract]
internal class Attr
{
	[ProtoMember(1)] public int CodePage { get; set; }
    
    [ProtoMember(2)] public int Time { get; set; }
    
    [ProtoMember(3)] public int Random { get; set; }
    
    [ProtoMember(4)] public int Color { get; set; }
    
    [ProtoMember(5)] public int Size { get; set; }
    
    [ProtoMember(6)] public int Effect { get; set; }
    
    [ProtoMember(7)] public int CharSet { get; set; }
    
	[ProtoMember(8)] public int PitchAndFamily { get; set; }
	
	[ProtoMember(9)] public string FontName { get; set; }
	
	[ProtoMember(10)] public byte[] ReserveData { get; set; }
}