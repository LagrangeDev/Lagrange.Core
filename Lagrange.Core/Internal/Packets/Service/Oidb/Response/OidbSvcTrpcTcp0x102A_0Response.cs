using Lagrange.Core.Internal.Packets.Service.Oidb.Generics;
using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class OidbSvcTrpcTcp0x102A_0Response
{
    [ProtoMember(1)] public List<OidbProperty> Urls { get; set; }
}