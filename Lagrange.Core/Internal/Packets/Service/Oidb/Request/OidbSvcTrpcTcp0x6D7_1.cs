using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Delete Folder
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x6D7, 1)]
internal class OidbSvcTrpcTcp0x6D7_1
{
    [ProtoMember(2)] public OidbSvcTrpcTcp0x6D7_1Delete Delete { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D7_1Delete
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(3)] public string FolderId { get; set; }
}