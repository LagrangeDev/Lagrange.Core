using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Message.Element.Implementation;

internal partial class NotOnlineImage
{
    [ProtoContract]
    internal class PbReserve
    {
        [ProtoMember(1)] public int Field1 { get; set; } // Zero Constant
        
        [ProtoMember(8)] public string Field8 { get; set; } // Empty String
        
        [ProtoMember(30)] public string Url { get; set; }
    }
}