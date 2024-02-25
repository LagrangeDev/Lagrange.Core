namespace Lagrange.Audio;

public enum AudioFormat
{
    /// <summary>
    /// Signed 16 bit
    /// </summary>
    Signed16Bit,

    /// <summary>
    /// Signed 32 bit
    /// </summary>
    Signed32Bit,

    /// <summary>
    /// Unsigned 8 bit
    /// </summary>
    UnSigned8Bit,

    /// <summary>
    /// Float 32 bit
    /// </summary>
    Float32Bit,

    /// <summary>
    /// Float 64 bit
    /// </summary>
    Float64Bit
}

/// <summary>
/// Audio channel
/// </summary>
public enum AudioChannel : int
{
    /// <summary>
    /// Mono
    /// </summary>
    Mono = 1,

    /// <summary>
    /// Stereo
    /// </summary>
    Stereo = 2,
}

/// <summary>
/// Audio info
/// </summary>
public readonly struct AudioInfo
{
    /// <summary>
    /// Audio format
    /// </summary>
    public AudioFormat Format { get; }

    /// <summary>
    /// Audio channel number
    /// </summary>
    public AudioChannel Channels { get; }

    /// <summary>
    /// Audio sample rate
    /// </summary>
    public int SampleRate { get; }

    /// <summary>
    /// Audio info
    /// </summary>
    /// <param name="format"></param>
    /// <param name="channels"></param>
    /// <param name="sampleRate"></param>
    public AudioInfo(AudioFormat format, AudioChannel channels, int sampleRate)
    {
        Format = format;
        Channels = channels;
        SampleRate = sampleRate;
    }

    /// <summary>
    /// Default audio presets
    /// </summary>
    /// <returns></returns>
    public static AudioInfo Default() => new(AudioFormat.Signed16Bit, AudioChannel.Stereo, 44100);
        
    /// <summary>
    /// Default mp3 presets
    /// </summary>
    /// <returns></returns>
    public static AudioInfo Mp3() => new(AudioFormat.Signed16Bit, AudioChannel.Stereo, 44100);
        
    /// <summary>
    /// Default silk presets
    /// </summary>
    /// <returns></returns>
    public static AudioInfo SilkV3() => new(AudioFormat.Signed16Bit, AudioChannel.Mono, 24000);
}