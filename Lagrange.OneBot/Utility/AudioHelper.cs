using System.Buffers.Binary;
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

    private static readonly KeyValuePair<UInt128, AudioFormat>[] _mapper =
    [
        // WAV
        //  R  I  F  F              W  A  V  E  f  m  t
        // +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        // |52|49|46|46|  |  |  |  |57|41|56|45|66|6D|74|
        // +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        // 0           4           8           12       15
        new(new(0x5249464600000000UL, 0x57415645666D7400UL), AudioFormat.Wav),
        // TENSILKV3
        //     #  !  S  I  L  K
        // +--+--+--+--+--+--+--+
        // |02|23|21|53|49|4C|4B|
        // +--+--+--+--+--+--+--+
        // 0           4        7
        new(new(0x02232153494C4B00UL, 0x0000000000000000UL), AudioFormat.TenSilkV3),
        // AMR
        //  #  !  A  M  R
        // +--+--+--+--+--+
        // |23|21|41|4D|52|
        // +--+--+--+--+--+
        // 0           4  5
        new(new(0x2321414D52000000UL, 0x0000000000000000UL), AudioFormat.Amr),
        // SILKV3
        //  #  !  S  I  L  K
        // +--+--+--+--+--+--+
        // |23|21|53|49|4C|4B|
        // +--+--+--+--+--+--+
        // 0           4     6
        new(new(0x232153494C4B0000UL, 0x0000000000000000UL), AudioFormat.SilkV3),
        // Ogg
        //  O  g  g  S
        // +--+--+--+--+
        // |4F|67|67|53|
        // +--+--+--+--+
        // 0           4
        new(new(0x4F67675300000000UL, 0x0000000000000000UL), AudioFormat.Ogg),
        // MP3 ID3
        //  I  D  3
        // +--+--+--+
        // |49|44|33|
        // +--+--+--+
        // 0        3
        new(new(0x4944330000000000UL, 0x0000000000000000UL), AudioFormat.Mp3),
        // MP3 no ID3
        //  ÿ  û
        // +--+--+
        // |FF|F2|
        // +--+--+
        // 0     2
        new(new(0xFFF2000000000000UL,0x0000000000000000UL), AudioFormat.Mp3),
        // MP3 no ID3
        //  ÿ  û
        // +--+--+
        // |FF|F3|
        // +--+--+
        // 0     2
        new(new(0xFFF3000000000000UL,0x0000000000000000UL), AudioFormat.Mp3),
        // MP3 no ID3
        //  ÿ  û
        // +--+--+
        // |FF|FB|
        // +--+--+
        // 0     2
        new(new(0xFFFB000000000000UL,0x0000000000000000UL), AudioFormat.Mp3),
    ];

    /// <summary>
    /// Detect audio type
    /// </summary>
    /// <param name="data"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static AudioFormat DetectAudio(byte[] data)
    {
        UInt128 value = BinaryPrimitives.ReadUInt128BigEndian(data.AsSpan(0, 16));
        foreach ((UInt128 head, AudioFormat format) in _mapper)
        {
            if ((value & head) == head) return format;
        }
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
