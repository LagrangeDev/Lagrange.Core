using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x511)]
internal class Tlv511 : TlvBody
{
    public Tlv511(string[] domainList)
    {
        DomainCount = (ushort)domainList.Length;
        var packet = new BinaryPacket();
        foreach (var domain in domainList)
        {
            packet.WriteByte(1).WriteString(domain, Prefix.Uint16 | Prefix.LengthOnly);
        }
        DomainBody = packet.ToArray();
    }

    [BinaryProperty] public ushort DomainCount { get; set; }

    [BinaryProperty(Prefix.None)] public byte[] DomainBody { get; set; }
}