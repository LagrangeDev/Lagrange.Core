using NAudio.Wave;

namespace Lagrange.Audio.Codec;

/// <summary>
/// Wav codec
/// </summary>
public static class WavCodec
{
    public class Decoder : AudioStream
    {
        private readonly WaveStream _stream;
        private readonly AudioInfo _output;
        
        /// <summary>
        /// WavCodec decoder
        /// </summary>
        public Decoder(string file)
        {
            _stream = new WaveFileReader(file);
            _output = new AudioInfo(AudioFormat.Signed16Bit, (AudioChannel)_stream.WaveFormat.Channels, _stream.WaveFormat.SampleRate);
        }
        
        /// <summary>
        /// WavCodec decoder
        /// </summary>
        public Decoder(Stream file)
        {
            _stream = new WaveFileReader(file);
            _output = new AudioInfo(AudioFormat.Signed16Bit, (AudioChannel)_stream.WaveFormat.Channels, _stream.WaveFormat.SampleRate);
        }
        
        /// <summary>
        /// Get adaptive output
        /// </summary>
        internal override AudioInfo? GetAdaptiveOutput() => _output;

        /// <inheritdoc />
        public override void Flush() => _stream.Flush();

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);

        /// <inheritdoc />
        public override void SetLength(long value) => _stream.SetLength(value);

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);

        /// <inheritdoc />
        public override bool CanRead => _stream.CanRead;

        /// <inheritdoc />
        public override bool CanSeek => _stream.CanSeek;

        /// <inheritdoc />
        public override bool CanWrite => _stream.CanWrite;

        /// <inheritdoc />
        public override long Length => _stream.Length;

        /// <inheritdoc />
        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }
    }
}