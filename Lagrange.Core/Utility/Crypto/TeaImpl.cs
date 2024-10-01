using Lagrange.Core.Utility.Crypto.Provider.Tea;

namespace Lagrange.Core.Utility.Crypto;

internal class TeaImpl
{
    public byte[] Encrypt(byte[] data, byte[] key)
    {
        return TeaProvider.Encrypt(data, key);
    }
    
    public Span<byte> Decrypt(Span<byte> data, Span<byte> key)
    {
        var decipher = TeaProvider.Decrypt(data, key);
        return decipher;
    }
}