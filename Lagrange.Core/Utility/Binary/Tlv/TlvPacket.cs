using System.Reflection;
using Lagrange.Core.Utility.Crypto;
using ProtoBuf;

namespace Lagrange.Core.Utility.Binary.Tlv;

/// <summary>
/// Tlv is the structure that composed of T(TAG) L(LENGTH) V(VALUE)
/// </summary>
internal class TlvPacket : BinaryPacket
{
    [BinaryProperty] public ushort TlvCommand { get; }
    [BinaryProperty] public ushort TlvBodyLength { get; }
    
    [BinaryProperty] public TlvBody TlvBody { get; }

    public TlvPacket(ushort tlvCommand, TlvBody tlvBody, (TeaImpl tea, byte[] key)? encrypt = null)
    {
        TlvCommand = tlvCommand;
        TlvBodyLength = (ushort) tlvBody.Length;
        TlvBody = tlvBody;
        
        Serialize(tlvBody, encrypt);
    }

    private void Serialize(TlvBody tlvBody, (TeaImpl tea, byte[] key)? encrypt)
    {
        BinaryPacket packet;
        if (tlvBody.GetType().GetCustomAttribute<ProtoContractAttribute>() != null)
        {
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, tlvBody);

            packet = new BinaryPacket(stream.ToArray()); // Write V(VALUE)
        }
        else
        {
            packet = BinarySerializer.Serialize(tlvBody); // Write V(VALUE)
        }

        Func<BinaryPacket> func;
        if (encrypt != null)
        {
            var (tea, key) = encrypt.Value;
            func = () => new BinaryPacket(tea.Encrypt(packet.ToArray(), key));
        }
        else
        {
            func = () => packet;
        }

        WriteUshort(TlvCommand, false); // Write T(TAG)
        Barrier(typeof(ushort), func, false);
    }
}