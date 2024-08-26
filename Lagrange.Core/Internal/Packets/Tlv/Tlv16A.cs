using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x16A)]
internal class Tlv16A : TlvBody
{
    /// <summary>
    /// Tlv0x16A <see cref="TlvQrCode19"/>
    /// </summary>
    public Tlv16A(BotKeystore keystore)
    {
        NoPicSig = keystore.Session.NoPicSig ?? Array.Empty<byte>();
    }

    [BinaryProperty(Prefix.None)] public byte[] NoPicSig { get; set; }
}

[Tlv(0x16A, true)]
internal class Tlv16AResponse : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] NoPicSig { get; set; }
}