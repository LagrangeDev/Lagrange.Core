using Lagrange.Core.Utility.Crypto.Provider.Ecdh;

namespace Lagrange.Core.Utility.Crypto;

internal partial class EcdhImpl
{
    public enum CryptMethod : uint
    {
        Secp192K1 = 0x0101 << 16 | 0x0102,
        Prime256V1 = 0x0201 << 16 | 0x0131
    }

    public enum CryptId
    {
        Ecdh135 = 0x87,
        Ecdh7 = 0x07
    }

    private static readonly Dictionary<CryptMethod, EcdhInfo> CurveTable = new()
    {
        {
            CryptMethod.Prime256V1, new EcdhInfo
            {
                Curve = EllipticCurve.Prime256V1,
                Id = CryptId.Ecdh135
            }
        },
        {
            CryptMethod.Secp192K1, new EcdhInfo
            {
                Curve = EllipticCurve.Secp192K1,
                Id = CryptId.Ecdh7
            }
        }
    };

    private readonly record struct EcdhInfo(EllipticCurve Curve, CryptId Id);
}