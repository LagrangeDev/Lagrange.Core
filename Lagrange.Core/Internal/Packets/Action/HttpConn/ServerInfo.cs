using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Action.HttpConn;

#pragma warning disable CS8618

[ProtoContract]
internal class ServerInfo
{
    [ProtoMember(1)] public uint ServiceType { get; set; }

    [ProtoMember(2)] public List<ServerAddr> ServerAddrs { get; set; }
}