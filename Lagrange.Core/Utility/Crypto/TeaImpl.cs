using Lagrange.Core.Utility.Crypto.Provider.Tea;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Utility.Crypto;

internal class TeaImpl : ICryptoImpl
{
    public byte[] Encrypt(byte[] data, byte[] key)
    {
        // Console.WriteLine("Data: " + data.Hex() + $" Key: {key.Hex()}");
        return TeaProvider.Encrypt(data, key);
    }

    public byte[] Encrypt(byte[] data) => Encrypt(data, new byte[16]);
    
    public byte[] Decrypt(byte[] data, byte[] key)
    {
        var decipher = TeaProvider.Decrypt(data, key);
        // Console.WriteLine("Decrypted Data: " + decipher.Hex() + $" Key: {key.Hex()}");
        return decipher;
    }

    public byte[] Decrypt(byte[] data) => Decrypt(data, new byte[16]);
}