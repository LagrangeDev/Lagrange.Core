using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Delete Folder
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x6D7, 2)]
internal class OidbSvcTrpcTcp0x6D7_2
{
    [ProtoMember(3)] public OidbSvcTrpcTcp0x6D7_2Rename Rename { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D7_2Rename
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(3)] public string FolderId { get; set; }
    
    [ProtoMember(4)] public string NewFolderName { get; set; }
}