using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Create Folder
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x6D7, 9)]
internal class OidbSvcTrpcTcp0x6D7_0
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x6D7_0Folder Folder { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D7_0Folder
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(3)] public string RootDirectory { get; set; }
    
    [ProtoMember(4)] public string FolderName { get; set; }
}