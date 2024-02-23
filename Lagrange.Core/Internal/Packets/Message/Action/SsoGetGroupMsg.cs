using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Action;

#pragma warning disable CS8618

/// <summary>
/// trpc.msg.register_proxy.RegisterProxy.SsoGetGroupMsg
/// </summary>
[ProtoContract]
internal class SsoGetGroupMsg
{
    [ProtoMember(1)] public SsoGetGroupMsgInfo Info { get; set; }
    
    [ProtoMember(2)] public bool Direction { get; set; }  // true
}

[ProtoContract]
internal class SsoGetGroupMsgInfo
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint StartSequence { get; set; }
    
    [ProtoMember(3)] public uint EndSequence { get; set; }
}