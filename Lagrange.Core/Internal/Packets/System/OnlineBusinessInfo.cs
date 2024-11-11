using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.System;

[ProtoContract]
internal class OnlineBusinessInfo
{
    [ProtoMember(1)] public uint NotifySwitch { get; set; }
    
    [ProtoMember(2)] public uint BindUinNotifySwitch { get; set; }
}