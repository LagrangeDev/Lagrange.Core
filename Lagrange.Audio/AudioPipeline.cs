namespace Lagrange.Audio;

public class AudioPipeline : List<Stream>, IDisposable
{
    private double _audioTime;
    private AudioInfo? _audioFormat;

    /// <summary>
    /// Start pipeline
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Start() => await Task.Run(() =>
    {
        // Process each stream
        for (var i = 0; i < Count - 1; ++i)
        {
            // Adaptive audio format
            if (this[i] is AudioStream x) _audioFormat = x.GetAdaptiveOutput() ?? _audioFormat;

            // Set audio format
            if (this[i + 1] is AudioStream y && _audioFormat != null) y.SetAdaptiveInput(_audioFormat.Value);

            // Pass data to next stream
            this[i].CopyTo(this[i + 1]);

            // Get audio time
            if (this[i] is AudioResampler z) _audioTime = z.GetOutputTime();
        }

        return true;
    });

    /// <summary>
    /// Get audio format
    /// </summary>
    /// <returns></returns>
    public AudioInfo? GetAudioFormat() => _audioFormat;

    /// <summary>
    /// Get audio time
    /// </summary>
    /// <returns></returns>
    public double GetAudioTime() => _audioTime;

    /// <inheritdoc />
    public void Dispose()
    {
        foreach (var i in this) i.Dispose();
    }
}