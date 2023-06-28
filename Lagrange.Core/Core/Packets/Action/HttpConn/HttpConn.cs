using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Action.HttpConn;

internal class HttpConn
{
    [ProtoMember(1)] public int Field1 { get; set; }
    
    [ProtoMember(2)] public int Field2 { get; set; }

    [ProtoMember(3)] public int Field3 { get; set; }

    [ProtoMember(4)] public int Field4 { get; set; }

    [ProtoMember(5)] public string Tgt { get; set; }

    [ProtoMember(6)] public int Field6 { get; set; }

    [ProtoMember(7)] public List<int> Field7 { get; set; }

    [ProtoMember(9)] public int Field9 { get; set; }

    [ProtoMember(10)] public int Field10 { get; set; }

    [ProtoMember(11)] public int Field11 { get; set; }

    [ProtoMember(15)] public string Ver { get; set; }
}