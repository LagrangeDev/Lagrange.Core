using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Response;

// ReSharper disable InconsistentNaming

[ProtoContract]
public class OidbSvcTrpcTcp0x8FC_3Response
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
}