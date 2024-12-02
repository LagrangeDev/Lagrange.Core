using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
[OidbSvcTrpcTcp(0x126B, 0)]
internal class OidbSvcTrpcTcp0x126B_0
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x126B_0_Field1 Field1 { get; set; } = new();
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x126B_0_Field1
{
    [ProtoMember(1)] public string? TargetUid { get; set; }

    [ProtoMember(2)] public OidbSvcTrpcTcp0x126B_0_Field1_2 Field2 { get; set; } = new();

    [ProtoMember(3)] public bool Block { get; set; }

    [ProtoMember(4)] public bool Field4 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x126B_0_Field1_2
{
    [ProtoMember(1)] public uint Field1 { get; set; } = 130;

    [ProtoMember(2)] public uint Field2 { get; set; } = 109;

    [ProtoMember(3)] public OidbSvcTrpcTcp0x126B_0_Field1_2_3 Field3 { get; set; } = new();
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x126B_0_Field1_2_3
{
    [ProtoMember(1)] public uint Field1 { get; set; } = 8;

    [ProtoMember(2)] public uint Field2 { get; set; } = 8;

    [ProtoMember(3)] public uint Field3 { get; set; } = 50;
}