using System.Numerics;
using System.Security.Cryptography;

namespace Lagrange.Core.Utility.Tencent;

internal class ClientPow
{
    public static byte[] GetPow(PowValue powValue)
    {
        var start = DateTime.Now;

        switch (powValue.CheckType)
        {
            case 1:
                CheckOrigin(powValue);
                break;
            case 2:
                CalcCostCount(powValue);
                break;
            default:
                throw new Exception();
        };

        powValue.Cost = (int)(DateTime.Now - start).TotalMilliseconds;
        powValue.HasHashResult = 1;

        return powValue.ToArray();
    }

    private static int CalcCostCount(PowValue pow)
    {
        var hashTemp = new byte[32];
        var copy = new byte[pow.Origin.Length];
        Buffer.BlockCopy(pow.Origin, 0, copy, 0, pow.Origin.Length);

        var bigInt = new BigInteger(copy, true, true);
        if (pow.AlgorithmType == 1)
        {
            while (true)
            {
                SHA256.HashData(copy.AsSpan(), hashTemp.AsSpan());
                if (hashTemp.SequenceEqual(pow.Cp))
                {
                    var result = (byte[])copy.Clone();
                    pow.HashResult = result;
                    pow.HashResultSize = (ushort)result.Length;
                    return pow.Count;
                }

                pow.Count++;
                bigInt++;

                var array = bigInt.ToByteArray(true, true);
                if (array.Length > copy.Length) return -1;
                Buffer.BlockCopy(array, 0, copy, 0, array.Length);
            }
        }
        else
        {
            return -1;
        }
    }

    private static int Check(byte[] inData, int count)
    {
        if (count > 32) return 1;

        int iv = 255;
        int i = 0;
        while (iv >= 0 && i < count)
        {
            if ((inData[iv / 8] & (1 << (iv % 8))) != 0)
            {
                return 2;
            }
            iv--;
            i++;
        }
        return 0;
    }

    private static int CheckOrigin(PowValue pow)
    {
        var hashTemp = new byte[32];
        var copy = new byte[pow.Origin.Length];
        Buffer.BlockCopy(pow.Origin, 0, copy, 0, pow.Origin.Length);

        var bigInt = new BigInteger(copy, true, true);
        if (pow.AlgorithmType == 1)
        {
            while (true)
            {
                SHA256.HashData(copy.AsSpan(), hashTemp.AsSpan());
                if (Check(hashTemp, pow.BaseCount) == 0)
                {
                    var result = (byte[])copy.Clone();
                    pow.HashResult = result;
                    pow.HashResultSize = (ushort)result.Length;
                    return pow.Count;
                }

                pow.Count++;
                bigInt++;

                var array = bigInt.ToByteArray(true, true);
                if (array.Length > copy.Length) return -1;
                Buffer.BlockCopy(array, 0, copy, 0, array.Length);
            }
        }
        else
        {
            return -1;
        }
    }
}