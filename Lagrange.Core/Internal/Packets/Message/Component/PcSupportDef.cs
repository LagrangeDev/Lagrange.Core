using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Component;

[ProtoContract]
internal class PcSupportDef
{
    [ProtoMember(1)] public uint PcPtlBegin { get; set; }

    [ProtoMember(2)] public uint PcPtlEnd { get; set; }

    [ProtoMember(3)] public uint MacPtlBegin { get; set; }

    [ProtoMember(4)] public uint MacPtlEnd { get; set; }

    [ProtoMember(5)] public List<uint> PtlsSupport { get; set; }

    [ProtoMember(6)] public List<uint> PtlsNotSupport { get; set; }
}