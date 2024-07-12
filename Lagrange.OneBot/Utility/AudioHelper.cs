using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Lagrange.Core.Utility.Binary;
using BitConverter = System.BitConverter;

namespace Lagrange.OneBot.Utility;

public static class AudioHelper
{
    public enum AudioFormat
    {
        Unknown,
        Wav,
        Mp3,
        SilkV3,
        TenSilkV3,
        Amr,
        Ogg,
    }

    /// <summary>
    /// Detect audio type
    /// </summary>
    /// <param name="data"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    [Obsolete("public static AudioFormat DetectAudio(byte[] data)")]
    public static bool DetectAudio(byte[] data, out AudioFormat type)
    {
        return (type = DetectAudio(data)) != AudioFormat.Unknown;
    }

    // WAV
    //  R  I  F  F              W  A  V  E  f  m  t
    // +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    // |52|49|46|46|  |  |  |  |57|41|56|45|66|6D|74|
    // +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    // 0           4           8           12       15
    private const ulong WAV_HEAD_LOWER = 0x5249464600000000UL;
    private const ulong WAV_HEAD_UPPER = 0x57415645666D7400UL;

    // TENSILKV3
    //     #  !  S  I  L  K
    // +--+--+--+--+--+--+--+
    // |02|23|21|53|49|4C|4B|
    // +--+--+--+--+--+--+--+
    // 0           4        7
    private const ulong TENSILKV3_HEAD_LOWER = 0x02232153494C4B00UL;

    // AMR
    //  #  !  A  M  R
    // +--+--+--+--+--+
    // |23|21|41|4D|52|
    // +--+--+--+--+--+
    // 0           4  5
    private const ulong AMR_HEAD_LOWER = 0x2321414D52000000UL;

    // SILKV3
    //  #  !  S  I  L  K
    // +--+--+--+--+--+--+
    // |23|21|53|49|4C|4B|
    // +--+--+--+--+--+--+
    // 0           4     6
    private const ulong SILKV3_HEAD_LOWER = 0x232153494C4B0000UL;

    // Ogg
    //  O  g  g  S
    // +--+--+--+--+
    // |4F|67|67|53|
    // +--+--+--+--+
    // 0           4
    private const ulong OGG_HEAD_LOWER = 0x4F67675300000000UL;

    // MP3 ID3
    //  I  D  3
    // +--+--+--+
    // |49|44|33|
    // +--+--+--+
    // 0        3
    private const ulong MP3ID3_HEAD_LOWER = 0x4944330000000000UL;

    // MP3 no ID3
    //  ÿ  û
    // +--+--+
    // |FF|F2|
    // +--+--+
    // 0     2
    private const ulong MP3FFF2_HEAD_LOWER = 0xFFF2000000000000UL;

    // MP3 no ID3
    //  ÿ  û
    // +--+--+
    // |FF|F3|
    // +--+--+
    // 0     2
    private const ulong MP3FFF3_HEAD_LOWER = 0xFFF3000000000000UL;

    // MP3 no ID3
    //  ÿ  û
    // +--+--+
    // |FF|FB|
    // +--+--+
    // 0     2
    private const ulong MP3FFFB_HEAD_LOWER = 0xFFFB000000000000UL;

    /// <summary>
    /// Detect audio type
    /// </summary>
    /// <param name="data"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static AudioFormat DetectAudio(byte[] data)
    {
        ulong lower = BinaryPrimitives.ReadUInt64BigEndian(data.AsSpan(0, sizeof(ulong)));
        ulong upper = BinaryPrimitives.ReadUInt64BigEndian(data.AsSpan(8, sizeof(ulong)));

        if ((lower & WAV_HEAD_LOWER) == WAV_HEAD_LOWER &&
            (upper & WAV_HEAD_UPPER) == WAV_HEAD_UPPER
        ) return AudioFormat.Wav;

        if ((lower & TENSILKV3_HEAD_LOWER) == TENSILKV3_HEAD_LOWER) return AudioFormat.TenSilkV3;

        if ((lower & AMR_HEAD_LOWER) == AMR_HEAD_LOWER) return AudioFormat.Amr;

        if ((lower & SILKV3_HEAD_LOWER) == SILKV3_HEAD_LOWER) return AudioFormat.SilkV3;

        if ((lower & OGG_HEAD_LOWER) == OGG_HEAD_LOWER) return AudioFormat.Ogg;

        if ((lower & MP3ID3_HEAD_LOWER) == MP3ID3_HEAD_LOWER) return AudioFormat.Mp3;

        if ((lower & MP3FFF2_HEAD_LOWER) == MP3FFF2_HEAD_LOWER) return AudioFormat.Mp3;

        if ((lower & MP3FFF3_HEAD_LOWER) == MP3FFF3_HEAD_LOWER) return AudioFormat.Mp3;

        if ((lower & MP3FFFB_HEAD_LOWER) == MP3FFFB_HEAD_LOWER) return AudioFormat.Mp3;

        return AudioFormat.Unknown;
    }

    private static readonly byte[] _silk_end = [0xFF, 0xFF];

    public static double GetSilkTime(byte[] data, int offset = 0)
    {
        int count = 0;
        for (int i = 9 + offset; i < data.Length && !data.AsSpan(i, 2).SequenceEqual(_silk_end); i += 2 + BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(i, 2)))
        {
            count++;
        }
        // Because the silk encoder encodes each 20ms sample as a block,
        // So that we can calculate the total time easily.
        return count * 0.02;
    }

    /// <summary>
    /// Get silk total time
    /// </summary>
    /// <param name="data"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static double GetTenSilkTime(byte[] data)
    {
        int count = 0;
        for (int i = 10; i < data.Length; i += 2 + BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(i, 2)))
        {
            count++;
        }
        // Because the silk encoder encodes each 20ms sample as a block,
        // So that we can calculate the total time easily.
        return count * 0.02;
    }
}
