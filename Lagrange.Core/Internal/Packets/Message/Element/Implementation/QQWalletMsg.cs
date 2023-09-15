using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

[ProtoContract]
internal partial class QQWalletMsg
{
    [ProtoMember(1)] public QQWalletAioBody Type { get; set; }
}