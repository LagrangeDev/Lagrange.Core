using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Action.HttpConn;

[ProtoContract]
internal class HttpConn0x6ff_501Response
{
    [ProtoMember(0x501)] public HttpConnResponse HttpConn { get; set; }
}