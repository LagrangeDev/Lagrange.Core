using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Action.Pb;

[ProtoContract]
internal class PbMultiMsgItem
{
    [ProtoMember(1)] public string FileName { get; set; }
    
    [ProtoMember(2)] public PbMultiMsgNew Buffer { get; set; }
}