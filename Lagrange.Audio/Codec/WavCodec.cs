using System.Text;
using Lagrange.Audio.Exceptions;
using Lagrange.Audio.Utility;

namespace Lagrange.Audio.Codec;

/// <summary>
/// Wav codec
/// </summary>
public static class WavCodec
{
    /// <summary>
    /// Wav encoder
    /// </summary>
    public class Encoder : AudioStream
    {
        private AudioInfo _output;
        private bool _firstRead;
        private bool _firstWrite;

        /// <summary>
        /// Wav encoder
        /// </summary>
        public Encoder()
        {
            _output = AudioInfo.Default();
            _firstRead = true;
            _firstWrite = true;
        }

        /// <summary>
        /// Wav encoder
        /// </summary>
        public Encoder(AudioInfo input) : this()
        {
            _output = input;
        }

        /// <summary>
        /// Set adaptive output
        /// </summary>
        internal override void SetAdaptiveInput(AudioInfo info)
            => _output = info;

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            // Write wav header
            if (_firstWrite)
            {
                _firstWrite = false;

                using var writer = new BinaryWriter
                    (this, Encoding.Default, true);
                {
                    writer.Write(1179011410U);
                    writer.Write((uint)(Length + 44 - 8));

                    writer.Write(1163280727U);
                    writer.Write(7630182U);

                    writer.Write(16);
                    writer.Write((ushort) 0x01);
                    writer.Write((ushort) _output.Channels);

                    writer.Write(_output.SampleRate);
                    writer.Write((uint) ((double) _output.SampleRate
                        * Sample.GetSampleLen(_output.Format) * (int) _output.Channels / 8));

                    writer.Write((ushort) Sample.GetSampleLen(_output.Format));

                    writer.Write(1635017060U);
                    writer.Write((uint)Length);
                }
            }

            base.Write(buffer, offset, count);
        }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_firstRead)
            {
                Position = 0;
                _firstRead = false;
            }

            return base.Read(buffer, offset, count);
        }
    }

    public class Decoder : AudioStream
    {
        private readonly Stream _stream;
        private readonly AudioInfo _output;

        /// <summary>
        /// Wav decoder
        /// </summary>
        public Decoder(string file)
        {
            _stream = File.OpenRead(file);
            _output = GetProfileAndMove(_stream);
        }
        
        public Decoder(Stream file)
        {
            _stream = file;
            _output = GetProfileAndMove(_stream);
        }

        /// <summary>
        /// Get adaptive output
        /// </summary>
        internal override AudioInfo? GetAdaptiveOutput()
            => _output;

        public override void Flush()
            => _stream.Flush();

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
            => _stream.Read(buffer, offset, count);

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
            => _stream.Seek(offset, origin);

        /// <inheritdoc />
        public override void SetLength(long value)
            => _stream.SetLength(value);

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
            => _stream.Write(buffer, offset, count);

        /// <inheritdoc />
        public override bool CanRead
            => _stream.CanRead;

        /// <inheritdoc />
        public override bool CanSeek
            => _stream.CanSeek;

        /// <inheritdoc />
        public override bool CanWrite
            => _stream.CanWrite;

        /// <inheritdoc />
        public override long Length
            => _stream.Length;

        /// <inheritdoc />
        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }

        /// <summary>
        /// Get wav file information and move stream pointer to 44
        /// </summary>
        /// <returns></returns>
        private static AudioInfo GetProfileAndMove(Stream s)
        {
            var buffer = new byte[44];
            _ = s.Read(buffer, 0, 44);
            
            short fmt = BitConverter.ToInt16(buffer, 20);
            short channel = BitConverter.ToInt16(buffer, 22);
            int sampleRate = BitConverter.ToInt32(buffer, 24);
            byte align = buffer[34];

            // Not supported compressed wav
            if (fmt != 1)
            {
                throw new DecodeException("Wav decoder doesn't support compressed wav files.");
            }

            return new AudioInfo(align switch
            {
                8 => AudioFormat.UnSigned8Bit,
                16 => AudioFormat.Signed16Bit,
                _ => throw new DecodeException("Not supported wav file format.")
            }, (AudioChannel) channel, sampleRate);
        }
    }
}
