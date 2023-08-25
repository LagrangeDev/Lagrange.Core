using Lagrange.Core.Core.Packets.Service.Oidb.Generics;
using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

[ProtoContract]
[OidbSvcTrpcTcp(0x11C5, 100)]
internal class OidbSvcTrpcTcp0x11C5_100
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x11C5_100Body1 Body1 { get; set; }
    
    [ProtoMember(2)] public OidbSvcTrpcTcp0x11C5_100Body2 Body2 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100Body1
{
    [ProtoMember(1)] public OidbTwoNumber Field1 { get; set; } // 1, 100
    
    [ProtoMember(2)] public OidbSvcTrpcTcp0x11C5_100Body1Field2 Field2 { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x11C5_100Body1Field3 Field3 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100Body1Field2
{
    [ProtoMember(101)] public uint Field101 { get; set; } // 2
    
    [ProtoMember(102)] public uint Field102 { get; set; } // 1
    
    [ProtoMember(200)] public uint Field200 { get; set; } // 1
    
    [ProtoMember(201)] public OidbSvcTrpcTcp0x11C5_100Body1Field2Field201 Field201 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100Body1Field2Field201
{
    [ProtoMember(1)] public uint Type { get; set; } // 2
    
    [ProtoMember(2)] public string TargetUid { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100Body1Field3
{
    [ProtoMember(1)] public uint Type { get; set; } // 2
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100Body2
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x11C5_100Body2Field1 Field1 { get; set; }
    
    [ProtoMember(2)] public uint Field2 { get; set; } // 1
    
    [ProtoMember(3)] public uint Field3 { get; set; } // 0

    [ProtoMember(4)] public uint Random { get; set; }
    
    [ProtoMember(5)] public uint Field5 { get; set; } // 0
    
    [ProtoMember(6)] public OidbSvcTrpcTcp0x11C5_100Body2Field1Field6 Field6 { get; set; }
    
    [ProtoMember(7)] public uint Sequence { get; set; } // 0
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100Body2Field1
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x11C5_100Body2Field1Field1 Field1 { get; set; }
    
    [ProtoMember(2)] public uint Field2 { get; set; } // 0
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100Body2Field1Field1
{
    [ProtoMember(1)] public uint FileSize { get; set; }
    
    [ProtoMember(2)] public string FileMd5 { get; set; } // lowercase hex
    
    [ProtoMember(3)] public string FileSha1 { get; set; } // lowercase hex
    
    [ProtoMember(4)] public string FileName { get; set; }

    [ProtoMember(5)] public OidbSvcTrpcTcp0x11C5_100Body2Field1Field1Field5 Field5 { get; set; }
    
    [ProtoMember(6)] public uint ImageWidth { get; set; }
    
    [ProtoMember(7)] public uint ImageHeight { get; set; }
    
    [ProtoMember(8)] public uint Field8 { get; set; } // 0
    
    [ProtoMember(9)] public uint Field9 { get; set; } // 1
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100Body2Field1Field1Field5
{
    [ProtoMember(1)] public uint Type { get; set; } // 1
    
    [ProtoMember(2)] public uint Field2 { get; set; } // 1001
    
    [ProtoMember(3)] public uint Field3 { get; set; } // 0
    
    [ProtoMember(4)] public uint Field4 { get; set; } // 0
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100Body2Field1Field6
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x11C5_100Body2Field1Field6Field1 Field1 { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x11C5_100Body2Field1Field6Field3 Field3 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100Body2Field1Field6Field1
{
    [ProtoMember(1)] public uint Field1 { get; set; } // 0
    
    [ProtoMember(2)] public string Field2 { get; set; } // ""
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_100Body2Field1Field6Field3
{
    [ProtoMember(3)] public uint Field3 { get; set; } // 0

    [ProtoMember(4)] public uint Field4 { get; set; } // 0
    
    [ProtoMember(5)] public string Field5 { get; set; } // ""
}
