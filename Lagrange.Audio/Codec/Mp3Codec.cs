using MP3Sharp;

namespace Lagrange.Audio.Codec;

/// <summary>
/// Mp3 codec
/// </summary>
public static class Mp3Codec
{
    /// <summary>
    /// Decoder
    /// </summary>
    public class Decoder : AudioStream
    {
        private readonly MP3Stream _stream;
        private readonly AudioInfo _output;

        /// <summary>
        /// Mp3Codec decoder
        /// </summary>
        public Decoder(string file)
        {
            _stream = new MP3Stream(file);
            _output = new AudioInfo(AudioFormat.Signed16Bit, (AudioChannel)(_stream.Format + 1), _stream.Frequency);
        }

        /// <summary>
        /// Mp3Codec decoder
        /// </summary>
        public Decoder(Stream stream)
        {
            _stream = new MP3Stream(stream);
            _output = new AudioInfo(AudioFormat.Signed16Bit, (AudioChannel)(_stream.Format + 1), _stream.Frequency);
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