using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Notify;

#pragma warning disable CS8618

[ProtoContract]
internal class CommonTips
{
    // [ProtoMember(1)] public uint Field1 { get; set; }

    [ProtoMember(2)] public uint Type { get; set; }

    // [ProtoMember(3)] public uint Field3 { get; set; }

    // [ProtoMember(6)] public uint Field6 { get; set; }

    [ProtoMember(7)] public Dictionary<string, string> Params { get; set; }

    // [ProtoMember(8)] public string Xml { get; set; }

    // [ProtoMember(10)] public uint Field10 { get; set; }
}