using System;

namespace Lagrange.Core.Utils.Tencent
{
    public static class Rc4
    {
        public static byte[] Apply(byte[] data, byte[] key)
        {
            int[] s = new int[256];
            for (int _ = 0; _ < 256; _++) s[_] = _;
            int[] T = new int[256];
            if (key.Length == 256)
            {
                Buffer.BlockCopy(key, 0, T, 0, key.Length);
            }
            else
            {
                for (int _ = 0; _ < 256; _++) T[_] = key[_ % key.Length];
            }
            int i = 0;
            int j = 0;
            for (i = 0; i < 256; i++)
            {
                j = (j + s[i] + T[i]) % 256;
                (s[i], s[j]) = (s[j], s[i]);
            }

            
            i = j = 0;
            byte[] result = new byte[data.Length];
            for (int iteration = 0; iteration < data.Length; iteration++)
            {
                i = (i + 1) % 256;
                j = (j + s[i]) % 256;
                (s[i], s[j]) = (s[j], s[i]);
                int k = s[(s[i] + s[j]) % 256];
                result[iteration] = Convert.ToByte(data[iteration] ^ k);
            }

            return result;
        }
    }
}