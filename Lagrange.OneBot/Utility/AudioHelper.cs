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
    }

    /// <summary>
    /// Detect audio type
    /// </summary>
    /// <param name="data"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool DetectAudio(byte[] data, out AudioFormat type)
    {
        // Read first 16 bytes
        var buffer = new BinaryPacket(data[..16]); 
        uint value = buffer.ReadUint(false);

        // SILKV3
        //  #  !  S  I  L  K
        // +--+--+--+--+--+--+
        // |23|21|53|49|4C|4B|
        // +--+--+--+--+--+--+
        // 0           4     6  
        if (value == 0x23215349)
        {
            type = AudioFormat.SilkV3;
            return true;
        }

        // TENSILKV3
        //     #  !  S  I  L  K
        // +--+--+--+--+--+--+--+
        // |02|23|21|53|49|4C|4B|
        // +--+--+--+--+--+--+--+
        // 0           4        7  
        if (value == 0x02232153)
        {
            type = AudioFormat.TenSilkV3;
            return true;
        }

        // AMR
        //     #  !  A  M  R
        // +--+--+--+--+--+--+
        // |02|23|21|41|4D|52|
        // +--+--+--+--+--+--+
        // 0           4     6
        if (value == 0x2321414D)
        {
            type = AudioFormat.Amr;
            return true;
        }

        // MP3 ID3
        //  I  D  3
        // +--+--+--+
        // |49|44|33|
        // +--+--+--+
        // 0        3
        if (value >> 8 == 0x494433)
        {
            type = AudioFormat.Mp3;
            return true;
        }

        // MP3 no ID3
        //  ÿ  û
        // +--+--+
        // |FF|FB|
        // +--+--+
        // 0     2
        if (value >> 16 == 0xFFFB ||
        //  ÿ  ó
        // +--+--+
        // |FF|F3|
        // +--+--+
        // 0     2
        value >> 16 == 0xFFF3 ||
        //  ÿ  ò
        // +--+--+
        // |FF|F2|
        // +--+--+
        // 0     2
        value >> 16 == 0xFFF2)
        {
            type = AudioFormat.Mp3;
            return true;
        }
        
        // WAV
        //  R  I  F  F  .  .  .  .  W  A  V  E  f  m  t
        // +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        // |52|49|46|46|00|00|00|00|57|41|56|45|66|6D|74|
        // +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        // 0           4           8           12       15
        buffer.Skip(4);
        if (value == 0x52494646 && buffer.ReadUint(false) == 0x57415645)
        {
            type = AudioFormat.Wav;
            return true;
        }

        type = AudioFormat.Unknown;
        return false;
    }

    /// <summary>
    /// Get silk total time
    /// </summary>
    /// <param name="data"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static double GetSilkTime(byte[] data, int offset = 0)
    {
        int blocks = 0;
        int position = 9 + offset;

        // Find all blocks
        while (position + 2 < data.Length)
        {
            // Teardown block size
            int len = BitConverter.ToUInt16(data[position..(position + 2)].ToArray());

            // End with 0xFFFFFF
            // for standard silk files
            if (len == 0xFFFF) break;
            
            // Teardown block
            ++blocks;

            // Move to next block
            position += len + 2;
        }

        // Because the silk encoder encodes each 20ms sample as a block,
        // So that we can calculate the total time easily.
        return blocks * 0.02;
    }
}
