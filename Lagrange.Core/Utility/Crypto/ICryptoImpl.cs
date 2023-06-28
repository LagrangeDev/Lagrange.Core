namespace Lagrange.Core.Utility.Crypto;

internal interface ICryptoImpl
{
    public byte[] Encrypt(byte[] data, byte[] key);
    
    public byte[] Encrypt(byte[] data);
    
    public byte[] Decrypt(byte[] data, byte[] key);
    
    public byte[] Decrypt(byte[] data);
}