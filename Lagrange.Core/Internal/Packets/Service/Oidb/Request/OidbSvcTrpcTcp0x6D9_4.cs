using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Group Send File
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x6D9, 4)]
internal class OidbSvcTrpcTcp0x6D9_4
{
    [ProtoMember(5)] public OidbSvcTrpcTcp0x6D9_4Body Body { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D9_4Body
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint Type { get; set; } // 2
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x6D9_4Info Info { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D9_4Info
{
    [ProtoMember(1)] public uint BusiType { get; set; } // 102
    
    [ProtoMember(2)] public string FileId { get; set; }
    
    [ProtoMember(3)] public uint Field3 { get; set; } // random
    
    [ProtoMember(4)] public string? Field4 { get; set; } // null
    
    [ProtoMember(5)] public bool Field5 { get; set; } // true
}