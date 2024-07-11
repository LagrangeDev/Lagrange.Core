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
    public static AudioFormat DetectAudio(byte[] data)
    {
        uint value = BinaryPrimitives.ReadUInt32BigEndian(data.AsSpan(0, sizeof(uint)));

        // SILKV3
        //  #  !  S  I  L  K
        // +--+--+--+--+--+--+
        // |23|21|53|49|4C|4B|
        // +--+--+--+--+--+--+
        // 0           4     6  
        if (value == 0x23215349) return AudioFormat.SilkV3;

        // TENSILKV3
        //     #  !  S  I  L  K
        // +--+--+--+--+--+--+--+
        // |02|23|21|53|49|4C|4B|
        // +--+--+--+--+--+--+--+
        // 0           4        7  
        if (value == 0x02232153) return AudioFormat.TenSilkV3;

        // AMR
        //     #  !  A  M  R
        // +--+--+--+--+--+--+
        // |02|23|21|41|4D|52|
        // +--+--+--+--+--+--+
        // 0           4     6
        if (value == 0x2321414D) return AudioFormat.Amr;

        // MP3 ID3
        //  I  D  3
        // +--+--+--+
        // |49|44|33|
        // +--+--+--+
        // 0        3
        if (value >> 8 == 0x494433) return AudioFormat.Mp3;


        // Ogg
        //  O  g  g  S
        // +--+--+--+--+
        // |4F|67|67|53
        // +--+--+--+--+
        if (value == 0x4F676753) return AudioFormat.Ogg;

        uint mp3NoId3Value = value >> 16;
        // MP3 no ID3
        //  ÿ  û
        // +--+--+
        // |FF|FB|
        // +--+--+
        // 0     2
        if (mp3NoId3Value == 0xFFFB) return AudioFormat.Mp3;
        //  ÿ  ó
        // +--+--+
        // |FF|F3|
        // +--+--+
        // 0     2
        if (mp3NoId3Value == 0xFFF3) return AudioFormat.Mp3;
        //  ÿ  ò
        // +--+--+
        // |FF|F2|
        // +--+--+
        // 0     2
        if (mp3NoId3Value == 0xFFF2) return AudioFormat.Mp3;

        // WAV
        //  R  I  F  F  .  .  .  .  W  A  V  E  f  m  t
        // +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        // |52|49|46|46|00|00|00|00|57|41|56|45|66|6D|74|
        // +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        // 0           4           8           12       15
        if (value == 0x52494646 && BinaryPrimitives.ReadUInt32BigEndian(data.AsSpan(8, sizeof(uint))) == 0x57415645)
        {
            return AudioFormat.Wav;
        }

        return AudioFormat.Unknown;
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
