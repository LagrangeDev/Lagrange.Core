using Lagrange.Core.Utility.Crypto.Provider.Ecdh;

namespace Lagrange.Core.Utility.Crypto;

internal partial class EcdhImpl
{
    private readonly EcdhProvider _ecdhProvider;
    
    private readonly TeaImpl _teaImpl;
    
    public CryptMethod Method { get; }
    
    public CryptId MethodId { get; }
    
    public byte[] ShareKey { get; private set; }

    public EcdhImpl(CryptMethod method, bool isHash = true)
    {
        Method = method;
        _teaImpl = new TeaImpl();

        var crypt = CurveTable[method]; // Select the curve

        _ecdhProvider = new EcdhProvider(crypt.Curve);
        MethodId = crypt.Id;
        ShareKey = GenerateShared(crypt.PubKey, isHash);
    }
    
    public byte[] GenerateShared(byte[] bobPublic, bool isHash = true)
    {
        var unpack = _ecdhProvider.UnpackPublic(bobPublic);
        ShareKey = _ecdhProvider.KeyExchange(unpack, isHash);

        return ShareKey;
    }
    
    public byte[] GetPublicKey(bool compress = true) => _ecdhProvider.PackPublic(compress);
    
    public byte[] Encrypt(byte[] data, byte[] key)
    {
        throw new NotImplementedException();
    }

    public byte[] Encrypt(byte[] data) => _teaImpl.Encrypt(data, ShareKey);

    public Span<byte> Decrypt(Span<byte> data) => _teaImpl.Decrypt(data, ShareKey);

    public Span<byte> Decrypt(byte[] data, byte[] key) => Decrypt(data);
}