using Lagrange.Core.Utility.Crypto.Provider.Tea;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Utility.Crypto;

internal class TeaImpl
{
    public byte[] Encrypt(byte[] data, byte[] key)
    {
        // Console.WriteLine("Data: " + data.Hex() + $" Key: {key.Hex()}");
        return TeaProvider.Encrypt(data, key);
    }
    
    public Span<byte> Decrypt(Span<byte> data, Span<byte> key)
    {
        var decipher = TeaProvider.Decrypt(data, key);
        // Console.WriteLine("Decrypted Data: " + decipher.Hex() + $" Key: {key.Hex()}");
        return decipher;
    }
}