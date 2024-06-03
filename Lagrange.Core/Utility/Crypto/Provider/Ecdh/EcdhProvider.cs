using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Lagrange.Core.Utility.Crypto.Provider.Ecdh;

/// <summary>
/// EC Diffie-Hellman key exchange provider, implemented with unsafe code to enhance performance.
/// </summary>
internal sealed unsafe class EcdhProvider
{
    public EllipticCurve Curve { get; }
    
    private BigInteger Secret { get; set; }
    
    private EllipticPoint Public { get; set; }

    public EcdhProvider(EllipticCurve curve)
    {
        Curve = curve;
        Secret = CreateSecret();
        Public = CreatePublic(Secret);
    }

    public void SetSecret(BigInteger secret)
    {
        Secret = secret;
        Public = CreatePublic(Secret);
    }

    /// <summary>
    /// Key exchange with bob
    /// </summary>
    /// <param name="ecPub"></param>
    /// <param name="isHash"></param>
    public byte[] KeyExchange(EllipticPoint ecPub, bool isHash)
    {
        var shared = CreateShared(Secret, ecPub);
        return PackShared(shared, isHash);
    }
    
    public EllipticPoint UnpackPublic(byte[] publicKey)
    {
        int length = publicKey.Length;
        if (length != Curve.Size * 2 + 1 && length != Curve.Size + 1) throw new Exception("Length does not match.");

        var x = new byte[Curve.Size]; // Teardown x
        Buffer.BlockCopy(publicKey, 1, x, 0, Curve.Size);
        Array.Reverse(x); // To LE
        Array.Resize(ref x, x.Length + 1); // Append 0x00

        // Not compressed
        if (publicKey[0] == 0x04)
        {
            var y = new byte[Curve.Size];  // Teardown y
            Buffer.BlockCopy(publicKey, Curve.Size + 1, y, 0, Curve.Size);
            Array.Reverse(y); // To LE
            Array.Resize(ref y, y.Length + 1); // Append 0x00

            return new EllipticPoint(new BigInteger(x), new BigInteger(y));
        }
        else
        {
            var px = new BigInteger(x);
            var x3 = px * px * px;
            var ax = px * Curve.A;
            var right = (x3 + ax + Curve.B) % Curve.P;

            var tmp = (Curve.P + 1) >> 2;
            var py = BigInteger.ModPow(right, tmp, Curve.P);

            if (!(py.IsEven && publicKey[0] == 0x02 || !py.IsEven && publicKey[0] == 0x03))
            {
                py = Curve.P - py;
            }

            return new EllipticPoint(px, py);
        }
    }

    
    private byte[] PackShared(EllipticPoint ecShared, bool isHash)
    {
        var x = TakeReverse(ecShared.X.ToByteArray(), Curve.Size);
        if (!isHash) return x;
        
        using var md5 = MD5.Create();
        return md5.ComputeHash(x[..Curve.PackSize]);
    }

    /// <summary>
    /// Unpack secret
    /// </summary>
    private BigInteger UnpackSecret(byte[] ecSec)
    {
        var length = ecSec.Length - 4;
        if (length != ecSec[3]) throw new Exception("Length does not match.");

        var temp = new byte[length];
        Buffer.BlockCopy(ecSec, 4, temp, 0, length);

        return new BigInteger(temp);
    }
    
    private byte[] PackSecret(BigInteger ecSec)
    {
        var result = ecSec.ToByteArray();
        var length = result.Length;
        Array.Resize(ref result, length + 4);
        Array.Reverse(result);

        result[3] = (byte)length;

        return result;
    }
    
    private BigInteger GenerateSecret(EllipticPoint point)
    {
        BigInteger result;
        var array = new byte[Curve.Size + 1];

        do
        {
            RandomNumberGenerator.Fill(array);
            array[Curve.Size] = 0;
            result = new BigInteger(array);
        } while (result < 1 || result >= Curve.N);

        return result;
    }

    public byte[] PackPublic(bool compress = true) => PackPublic(Public, compress);
    
    public byte[] PackPublic(EllipticPoint ecPub, bool compress = true)
    {
        if (compress)
        {
            var result = ecPub.X.ToByteArray();
            if (result.Length == Curve.Size) Array.Resize(ref result, Curve.Size + 1);

            Array.Reverse(result);
            result[0] = (byte) (ecPub.Y.IsEven ^ ecPub.Y.Sign < 0 ? 0x02 : 0x03);
            return result;
        }

        var x = TakeReverse(ecPub.X.ToByteArray(), Curve.Size);
        var y = TakeReverse(ecPub.Y.ToByteArray(), Curve.Size);
        var buffer = new byte [Curve.Size * 2 + 1];
        
        buffer[0] = 0x04;
        Buffer.BlockCopy(x, 0, buffer, 1, x.Length);
        Buffer.BlockCopy(y, 0, buffer, y.Length + 1, x.Length);

        return buffer;
    }
    
    private EllipticPoint CreatePublic(BigInteger ecSec) => CreateShared(ecSec, Curve.G);
    
    private BigInteger CreateSecret()
    {
        BigInteger result;
        var array = new byte[Curve.Size + 1];

        do
        {
            RandomNumberGenerator.Fill(array);
            array[Curve.Size] = 0;
            result = new BigInteger(array);
        } while (result < 1 || result >= Curve.N);

        return result;
    }
    
    private EllipticPoint CreateShared(BigInteger ecSec, EllipticPoint ecPub)
    {
        if (ecSec % Curve.N == 0 || ecPub.IsDefault) return default;
        if (ecSec < 0) return CreateShared(-ecSec, -ecPub);

        if (!Curve.CheckOn(ecPub)) throw new Exception("Public key does not correct.");

        var pr = new EllipticPoint();
        var pa = ecPub;
        while (ecSec > 0)
        {
            if ((ecSec & 1) > 0) pr = PointAdd(Curve, pr, pa);

            pa = PointAdd(Curve, pa, pa);
            ecSec >>= 1;
        }

        if (!Curve.CheckOn(pr)) throw new Exception("Unknown error.");

        return pr;
    }
    
    private static EllipticPoint PointAdd(EllipticCurve curve, EllipticPoint p1, EllipticPoint p2)
    {
        if (p1.IsDefault) return p2;
        if (p2.IsDefault) return p1;
        if (!curve.CheckOn(p1) || !curve.CheckOn(p2)) throw new Exception();

        var x1 = p1.X;
        var x2 = p2.X;
        var y1 = p1.Y;
        var y2 = p2.Y;
        BigInteger m;

        if (x1 == x2)
        {
            if (y1 == y2) m = (3 * x1 * x1 + curve.A) * ModInverse(y1 << 1, curve.P);
            else return default;
        }
        else
        {
            m = (y1 - y2) * ModInverse(x1 - x2, curve.P);
        }

        var xr = Mod(m * m - x1 - x2, curve.P);
        var yr = Mod(m * (x1 - xr) - y1, curve.P);
        var pr = new EllipticPoint(xr, yr);
        
        if (!curve.CheckOn(pr)) throw new Exception();
        return pr;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte[] TakeReverse(byte[] array, int length)
    {
        var result = new byte[length];
        fixed (byte* resultPtr = result, arrayPtr = array)
        {
            for (int i = 0, j = length - 1; i < length; ++i, --j) resultPtr[i] = arrayPtr[j];
        }

        return result;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static BigInteger ModInverse(BigInteger a, BigInteger p)
    {
        if (a < 0) return p - ModInverse(-a, p);

        var g = BigInteger.GreatestCommonDivisor(a, p);
        if (g != 1) throw new Exception("Inverse does not exist.");

        return BigInteger.ModPow(a, p - 2, p);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static BigInteger Mod(BigInteger a, BigInteger b)
    {
        var result = a % b;
        if (result < 0) result += b;
        return result;
    }
}
