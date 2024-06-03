using Lagrange.Core.Message.Entity;
using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Component.Extra;

#pragma warning disable CS8618

[ProtoContract]
internal class ButtonExtra
{
    [ProtoMember(1)] public KeyboardData Data { get; set; }
}