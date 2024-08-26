using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Action.HttpConn;

[ProtoContract]
internal class HttpConn
{
    [ProtoMember(1)] public int Field1 { get; set; } // 0

    [ProtoMember(2)] public int Field2 { get; set; } // 0

    [ProtoMember(3)] public int Field3 { get; set; } // 16

    [ProtoMember(4)] public int Field4 { get; set; } // 1

    [ProtoMember(5)] public string Tgt { get; set; } // tgt lower hex

    [ProtoMember(6)] public int Field6 { get; set; } // 3

    [ProtoMember(7)] public List<int> ServiceTypes { get; set; } // 1, 5, 10, 21 for image

    [ProtoMember(9)] public int Field9 { get; set; } // 2

    [ProtoMember(10)] public int Field10 { get; set; } // 9

    [ProtoMember(11)] public int Field11 { get; set; } // 8

    [ProtoMember(15)] public string Ver { get; set; } // 1.0.1
}