using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

[ProtoContract]
internal class OnlineImage
{
    [ProtoMember(1)] public byte[] Guid { get; set; }

    [ProtoMember(2)] public byte[] FilePath { get; set; }

    [ProtoMember(3)] public byte[] OldVerSendFile { get; set; }
}