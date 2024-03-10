using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D8
{
    [ProtoMember(2)] public OidbSvcTrpcTcp0x6D8List? List { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x6D8Count? Count { get; set; }
    
    [ProtoMember(4)] public OidbSvcTrpcTcp0x6D8Space? Space { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D8List
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint AppId { get; set; } // 7
    
    [ProtoMember(3)] public string TargetDirectory { get; set; } // /
    
    [ProtoMember(5)] public uint FileCount { get; set; } // 20
    
    [ProtoMember(9)] public uint SortBy { get; set; } // 1
    
    [ProtoMember(13)] public uint StartIndex { get; set; } // default 0
    
    [ProtoMember(17)] public uint Field17 { get; set; } // 2
    
    [ProtoMember(18)] public uint Field18 { get; set; } // 0
}


[ProtoContract]
internal class OidbSvcTrpcTcp0x6D8Count
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint AppId { get; set; } // 7
    
    [ProtoMember(3)] public uint BusId { get; set; } // 6
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D8Space
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint AppId { get; set; } // 7
}