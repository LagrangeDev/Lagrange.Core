using Lagrange.Core.Core.Packets.Service.Oidb.Generics;
using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
[OidbSvcTrpcTcp(0x11C5, 101)]
internal class OidbSvcTrpcTcp0x11C5_101
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x11C5_101Body1 Body1 { get; set; }

    [ProtoMember(6)] public byte[] CommandAdditional { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_101Body1
{
    [ProtoMember(1)] public OidbTwoNumber Command { get; set; } // 2, 101
    
    [ProtoMember(2)] public OidbSvcTrpcTcp0x11C5_100Body1Field2 Field2 { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x11C5_100Body1Field3 Field3 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_101Body1Field2
{
    [ProtoMember(101)] public uint Field101 { get; set; } // 2
    
    [ProtoMember(102)] public uint Field102 { get; set; } // 1
    
    [ProtoMember(200)] public uint Field200 { get; set; } // 1
    
    [ProtoMember(201)] public OidbSvcTrpcTcp0x11C5_100Body1Field2Field201 Field201 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x11C5_101Body1Field2Field201
{
    [ProtoMember(1)] public uint Type { get; set; } // 2
    
    [ProtoMember(2)] public string TargetUid { get; set; }
}