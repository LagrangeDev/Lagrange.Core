namespace Lagrange.Core.Utils.Tencent;

public static partial class Algorithm
{
    private static uint TencentCrc32(uint[] a, byte[] b)
    {
        uint crc = 0;
        crc = ~crc;
        foreach (byte v in b) crc = a[(crc & 0xff) ^ v] ^ (crc >> 8);
        return ~crc;
    }
    
    private static void TencentEncryptionA(byte[] input, byte[] key, byte[] data)
    {
        var s = new State(new uint[16], new uint[16], 0, 0);
        s.Init(key, data, new uint[] { 0, 0 }, 20);
        s.Encrypt(input);
    }

    private static byte[] TencentEncryptB(byte[] c, byte[] m)
    {
        byte[] buf = new byte[16];
        uint[] w = SubF(TableE, TableF, TableB);
        byte[] result = new byte[21];
        
        for (int i = 0; i < 21; i++)
        {
            if ((i & 0xf) == 0)
            {
                c.CopyTo(buf, 0);
                _tencentEncryptB(buf, w);
                for (int j = 15; j >= 0; j--)
                {
                    c[j]++;
                    if (c[j] != 0)
                        break;
                }
            }
            result[i] = SubAa(i, TableA, buf, m);
        }
        return result;
    }

    private static void _tencentEncryptB(byte[] p1, uint[] p2)
    {
        const int c = 10;
        for (int r = 0; r < 9; r++)
        {
            SubD(TableD, p1);
            SubB(p1, p2[(r * 4)..((r + 1) * 4)]);
            SubC(TableB, p1);
            SubE(TableC, p1);
        }
        SubD(TableD, p1);
        SubB(p1, p2[((c - 1) * 4)..(c * 4)]);
        SubC(TableB, p1);
        SubA(p1, p2[(c * 4)..((c + 1) * 4)]);
    }
}