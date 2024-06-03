// Resharper disable InconsistentNaming

using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

[ProtoContract]
[OidbSvcTrpcTcp(0xFE5, 2)]
internal class OidbSvcTrpcTcp0xFE5_2
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0xFE5_2Config Config { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE5_2Config
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0xFE5_2Config1 Config1 { get; set; }
    
    [ProtoMember(2)] public OidbSvcTrpcTcp0xFE5_2Config2 Config2 { get; set; }

    [ProtoMember(3)] public OidbSvcTrpcTcp0xFE5_2Config3 Config3 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE5_2Config1
{
    [ProtoMember(1)] public bool GroupOwner { get; set; } = true;
    
    [ProtoMember(2)] public bool Field2 { get; set; } = true;
    
    [ProtoMember(3)] public bool MemberMax { get; set; } = true;
    
    [ProtoMember(4)] public bool MemberCount { get; set; } = true;
    
    [ProtoMember(5)] public bool GroupName { get; set; } = true;
    
    [ProtoMember(8)] public bool Field8 { get; set; } = true;
    
    [ProtoMember(9)] public bool Field9 { get; set; } = true;
    
    [ProtoMember(10)] public bool Field10 { get; set; } = true;
    
    [ProtoMember(11)] public bool Field11 { get; set; } = true;
    
    [ProtoMember(12)] public bool Field12 { get; set; } = true;
    
    [ProtoMember(13)] public bool Field13 { get; set; } = true;
    
    [ProtoMember(14)] public bool Field14 { get; set; } = true;
    
    [ProtoMember(15)] public bool Field15 { get; set; } = true;
    
    [ProtoMember(16)] public bool Field16 { get; set; } = true;
    
    [ProtoMember(17)] public bool Field17 { get; set; } = true;
    
    [ProtoMember(18)] public bool Field18 { get; set; } = true;
    
    [ProtoMember(19)] public bool Question { get; set; } = true;
    
    [ProtoMember(20)] public bool Field20 { get; set; } = true;
    
    [ProtoMember(22)] public bool Field22 { get; set; } = true;
    
    [ProtoMember(23)] public bool Field23 { get; set; } = true;
    
    [ProtoMember(24)] public bool Field24 { get; set; } = true;
    
    [ProtoMember(25)] public bool Field25 { get; set; } = true;
    
    [ProtoMember(26)] public bool Field26 { get; set; } = true;
    
    [ProtoMember(27)] public bool Field27 { get; set; } = true;
    
    [ProtoMember(28)] public bool Field28 { get; set; } = true;
    
    [ProtoMember(29)] public bool Field29 { get; set; } = true;
    
    [ProtoMember(30)] public bool Field30 { get; set; } = true;
    
    [ProtoMember(31)] public bool Field31 { get; set; } = true;
    
    [ProtoMember(32)] public bool Field32 { get; set; } = true;
    
    [ProtoMember(5001)] public bool Field5001 { get; set; } = true;
    
    [ProtoMember(5002)] public bool Field5002 { get; set; } = true;
    
    [ProtoMember(5003)] public bool Field5003 { get; set; } = true;
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE5_2Config2
{
    [ProtoMember(1)] public bool Field1 { get; set; } = true;
    
    [ProtoMember(2)] public bool Field2 { get; set; } = true;
    
    [ProtoMember(3)] public bool Field3 { get; set; } = true;
    
    [ProtoMember(4)] public bool Field4 { get; set; } = true;
    
    [ProtoMember(5)] public bool Field5 { get; set; } = true;
    
    [ProtoMember(6)] public bool Field6 { get; set; } = true;
    
    [ProtoMember(7)] public bool Field7 { get; set; } = true;
    
    [ProtoMember(8)] public bool Field8 { get; set; } = true;
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE5_2Config3
{
    [ProtoMember(5)] public bool Field5 { get; set; } = true;

    [ProtoMember(6)] public bool Field6 { get; set; } = true;
}