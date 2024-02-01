using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// Resharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
[OidbSvcTrpcTcp(0x126D, 200)]
internal class OidbSvcTrpcTcp0x126D_200
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x126D_200Field1 Field1 { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x126D_200Field3 Field3 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x126D_200Field1
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x126D_200Field1Field1 Field1 { get; set; }
    
    [ProtoMember(2)] public OidbSvcTrpcTcp0x126D_200Field1Field2 Field2 { get; set; }

    [ProtoMember(3)] public OidbSvcTrpcTcp0x126D_200Field1Field3 Field3 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x126D_200Field1Field1
{
    [ProtoMember(1)] public uint Field1 { get; set; } // 1
    
    [ProtoMember(2)] public uint Field2 { get; set; } // 200
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x126D_200Field1Field2
{
    [ProtoMember(101)] public uint Field1 { get; set; } // 1
    
    [ProtoMember(102)] public uint Field2 { get; set; } // 3
    
    [ProtoMember(200)] public uint Field3 { get; set; } // 1
    
    [ProtoMember(201)] public OidbSvcTrpcTcp0x126D_200Field1Field2Field201 Field201 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x126D_200Field1Field2Field201
{
    [ProtoMember(1)] public uint Field1 { get; set; } // 2
    
    [ProtoMember(2)] public string SelfUid { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x126D_200Field1Field3
{
    [ProtoMember(1)] public uint Field1 { get; set; } // 2
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x126D_200Field3
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x126D_200Field3Field1 Field1 { get; set; }
    
    [ProtoMember(2)] public OidbSvcTrpcTcp0x126D_200Field3Field2 Field2 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x126D_200Field3Field1
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x126D_200Field3Field1Field1 Field1 { get; set; }
    
    [ProtoMember(2)] public string FileUuid { get; set; }
    
    [ProtoMember(3)] public uint Field3 { get; set; } // 0
    
    [ProtoMember(4)] public uint Field4 { get; set; } // 0

    [ProtoMember(5)] public uint Field5 { get; set; } // 0

    [ProtoMember(6)] public uint Field6 { get; set; } // 0
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x126D_200Field3Field1Field1
{
    [ProtoMember(1)] public uint Field1 { get; set; } // 0
    
    [ProtoMember(2)] public string FileHash { get; set; }
    
    [ProtoMember(3)] public string FileSha1 { get; set; } // ""
    
    [ProtoMember(4)] public string FileName { get; set; }
    
    [ProtoMember(5)] public OidbSvcTrpcTcp0x126D_200Field3Field1Field1Field5 Field5 { get; set; }
    
    [ProtoMember(6)] public uint Field6 { get; set; } // 0
    
    [ProtoMember(7)] public uint Field7 { get; set; } // 0
    
    [ProtoMember(8)] public uint Field8 { get; set; } // 2
    
    [ProtoMember(9)] public uint Field9 { get; set; } // 0
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x126D_200Field3Field1Field1Field5
{
    [ProtoMember(1)] public uint Field1 { get; set; } // 2
    
    [ProtoMember(2)] public uint Field2 { get; set; } // 0
    
    [ProtoMember(3)] public uint Field3 { get; set; } // 0
    
    [ProtoMember(4)] public uint Field4 { get; set; } // 1
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x126D_200Field3Field2
{
    public OidbSvcTrpcTcp0x126D_200Field3Field2Field2 Field2 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x126D_200Field3Field2Field2
{
    [ProtoMember(1)] public uint Field1 { get; set; } // 0
    
    [ProtoMember(2)] public uint Field2 { get; set; } // 0
}