using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Lagrange.Core.Utils.Tencent;

/// <summary>
/// Tlv544 Signer, credit to OICQ and MiraiGo for the original implementation
/// </summary>
public static partial class Algorithm
{
	private static readonly Random Random = new();

	public static byte[] Sign(uint curr, byte[] input)
    {
	    var crcData = new byte[21];
	    curr %= 1000000;
	    PutUint32(crcData, 0, curr & 0xffffffff, true);
	    
	    input = input.Concat(crcData.Take(4)).ToArray();
	    var kt = new byte[40];
	    for (var i = 0; i < 2; i++) kt[i] = (byte)(KeyTable[Random.Next(0, 50)] + 50);
	    
	    kt[2] = (byte)(kt[1] + 20);
	    kt[3] = (byte)(kt[2] + 20);
	    
	    Key1.CopyTo(kt, 6, 0, 4);
	    for (var i = 0; i < 4; i++) kt[10 + i] = (byte) (Key2[i] ^ kt[i]);
	    kt[4] = kt[12];
	    kt[5] = kt[13];
	    kt[12] = kt[13] = 0;
	    
	    var key = kt.Skip(4).Take(8).ToArray();
	    var encryptedKey = Rc4.Apply(key, key);
	    encryptedKey.CopyTo(kt, 4, 0, encryptedKey.Length);
	    PutUint64(kt, 4, Magic); // LittleEndian

	    TencentEncryptionA(input, kt[4..(4 + 32)], crcData[4..(4 + 8)]);
	    var result = Md5(input); // md5.Sum(input);
	    
	    crcData[2] = crcData[4] = 1;
	    kt.CopyTo(crcData, 5, 0, 4);
	    PutUint32(kt, 9, curr, true);
	    result.CopyTo(crcData, 13, 0, 8);
	    var calcCrc = TencentCrc32(Tab, crcData[2..]);
	    PutUint32(kt, 36, calcCrc, true); // BigEndian

	    crcData[0] = kt[36];
	    crcData[1] = kt[39];
	    uint nonce = (uint)Random.Next(0, int.MaxValue);
	    PutUint32(kt, 0, nonce, true);
	    kt.CopyTo(kt, 4, 0, 4);
	    kt.CopyTo(kt, 8, 0, 8);
	    TransformInner(crcData, TransformEncode);
	    var encrypted = TencentEncryptB(kt[0..16], crcData);
	    TransformInner(encrypted, TransformDecode);
	    
	    kt[0] = 0x0C;
	    kt[1] = 0x05;
	    PutUint32(kt, 2, nonce, true);
	    encrypted.CopyTo(kt, 6, 0, encrypted.Length);
	    
	    kt[27] = kt[28] = kt[29] = kt[30] = 0;
	    kt[31] = (byte)KeyTable2[Random.Next(0, 50)];
	    kt[32] = (byte)KeyTable2[Random.Next(0, 50)];
	    
	    var addition = Random.Next(0, 9);
	    while ((addition & 1) == 0) addition = Random.Next(0, 9);
	    
	    kt[33] = (byte) (kt[31] + addition & 0xff);
	    kt[34] = (byte) (kt[32] + ((9 - addition) & 0xff) + 1);
	    kt[35] = kt[36] = kt[37] = kt[38] = 0;
	    return kt[..39];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyTo(this byte[] source, byte[] destination, int targetStart, int sourceStart, int sourceEnd)
	{
	    for (var i = sourceStart; i < sourceEnd; i++) destination[targetStart + i] = source[i];
	}
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte[] Md5(byte[] input)
	{
	    using var md5 = MD5.Create();
	    return md5.ComputeHash(input);
	}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void PutUint32(byte[] b, int offset, uint v, bool bigEndian = false)
    {
	    if (bigEndian)
	    {
		    for (var i = 0; i < 4; i++) b[offset + i] = (byte) (v >> (24 - i * 8));
	    }
	    else
	    {
		    for (var i = 0; i < 4; i++) b[offset + i] = (byte) (v >> (i * 8));
	    }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void PutUint64(byte[] b, int offset, ulong v, bool bigEndian = false)
	{
	    if (bigEndian)
	    {
		    for (var i = 0; i < 8; i++) b[offset + i] = (byte) (v >> (56 - i * 8));
	    }
	    else
	    {
		    for (var i = 0; i < 8; i++) b[offset + i] = (byte) (v >> (i * 8));
	    }
	}
}