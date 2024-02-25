namespace Lagrange.Audio;

/// <summary>
/// Audio stream
/// </summary>
public abstract class AudioStream : MemoryStream
{
    /// <summary>
    /// Set adaptive input
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    internal virtual void SetAdaptiveInput(AudioInfo info) { }

    /// <summary>
    /// Get adaptive output
    /// </summary>
    /// <returns></returns>
    internal virtual AudioInfo? GetAdaptiveOutput() => null;

    /// <summary>
    /// Get output audio time
    /// </summary>
    /// <returns></returns>
    internal virtual double GetOutputTime() => 0;
}