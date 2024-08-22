using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D7_0Response
{
    [ProtoMember(1)] public int Retcode { get; set; }
    
    [ProtoMember(2)] public string RetMsg { get; set; }
    
    [ProtoMember(3)] public string ClientWording { get; set; }
    
    [ProtoMember(4)] public OidbSvcTrpcTcp0x6D7_0ResponseFolderInfo FolderInfo { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D7_0ResponseFolderInfo
{
    [ProtoMember(1)] public string FolderId { get; set; }
    
    [ProtoMember(2)] public string FolderPath { get; set; }
    
    [ProtoMember(3)] public string FolderName { get; set; }
    
    [ProtoMember(4)] public uint Timestamp4 { get; set; }
    
    [ProtoMember(5)] public uint Timestamp5 { get; set; }
    
    [ProtoMember(6)] public uint OperatorUin6 { get; set; }
    
    [ProtoMember(7)] public uint OperatorUin9 { get; set; }
}