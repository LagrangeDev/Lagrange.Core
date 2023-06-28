using Lagrange.Core.Core.Packets.Message.Element;
using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Message.Component;

[ProtoContract]
internal class RichText
{
    [ProtoMember(1)] public Attr? Attr { get; set; }
    
    [ProtoMember(2)] public List<Elem> Elems { get; set; }
    
    [ProtoMember(3)] public NotOnlineFile? NotOnlineFile { get; set; }
    
    [ProtoMember(4)] public Ptt? Ptt { get; set; }
}