using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class D5D6ReqBody
{
    [ProtoMember(1)] public uint Uint32Seq { get; set; }

    [ProtoMember(2)] public List<SnsUpdateBuffer> RptMsgUpdateBuffer { get; set; }

    [ProtoMember(3)] public uint Uint32Domain { get; set; }
}

[ProtoPackable]
internal partial class D5D6RspBody
{
    [ProtoMember(1)] public uint Uint32Seq { get; set; }

    [ProtoMember(2)] public string StrWording { get; set; }
}

[ProtoPackable]
internal partial class SnsUpdateBuffer
{
    [ProtoMember(1)] public string Uid { get; set; }

    [ProtoMember(2)] public ulong GroupUin { get; set; }

    [ProtoMember(400)] public List<SnsUpdateItem> RptMsgSnsUpdateItem { get; set; }

    [ProtoMember(401)] public List<uint> RptUint32IdList { get; set; }
}

[ProtoPackable]
internal partial class SnsUpdateItem
{
    [ProtoMember(1)] public uint Uint32UpdateSnsType { get; set; }

    [ProtoMember(2)] public byte[] BytesValue { get; set; }

    [ProtoMember(3)] public uint Uint32ValueOffset { get; set; }
}
