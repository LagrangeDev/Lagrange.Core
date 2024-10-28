#pragma warning disable CS8618

using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;

[ProtoContract]
internal class GroupReactionExtra
{
    [ProtoMember(1)] public GroupReactionExtraBody Body { get; set; }
}

[ProtoContract]
internal class GroupReactionExtraBody
{
    [ProtoMember(1)] public GroupReactionExtraBodyField1 Field1 { get; set; }

    [ProtoMember(2)] public GroupReactionExtraFaceInfo[] FaceInfos { get; set; }
}

[ProtoContract]
internal class GroupReactionExtraBodyField1
{
    [ProtoMember(1)] public uint Field1 { get; set; }

    [ProtoMember(2)] public uint Field2 { get; set; }
}

[ProtoContract]
internal class GroupReactionExtraFaceInfo
{
    // See Lagrange.Core/Internal/Service/System/FetchFullSysFacesService.cs
    [ProtoMember(1)] public string FaceId { get; set; }

    // See Lagrange.Core/Internal/Service/System/FetchFullSysFacesService.cs
    [ProtoMember(2)] public uint Type { get; set; }

    [ProtoMember(3)] public uint Count { get; set; }

    [ProtoMember(4)] public uint IsAdded { get; set; }
}