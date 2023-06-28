using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using Lagrange.Core.Core.Packets.Login.NTLogin.Plain.Body;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Tlv;

[Tlv(0x106)]
internal class Tlv106 : TlvBody
{
    /// <summary>
    /// <para>manually construct Tlv106 by tempPassword, from TlvQrCode18, not through dependency injection</para>
    /// <para>This field does not only use as the request, but also response</para>
    /// <para>Response should be referred to <see cref="SsoNTLoginEasyLogin"/></para>

    /// <para>Password is depreciated, so such field is now injected through dependency injection</para>
    /// </summary>
    public Tlv106(BotKeystore keystore) => Temp = keystore.Session.TempPassword ?? throw new ArgumentNullException(nameof(keystore.Session.TempPassword));

    [BinaryProperty(Prefix.None)] public byte[] Temp { get; set; }
}

[Tlv(0x106, true)]
internal class Tlv106Response : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] Temp { get; set; }
}