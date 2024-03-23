using NVorbis;

namespace Lagrange.Audio.Codec;

public class OggCodec
{
    public class Decoder : AudioStream
    {
        private readonly VorbisReader _stream;
        private readonly AudioInfo _output;

        /// <summary>
        /// Wav decoder
        /// </summary>
        public Decoder(string file)
        {
            _stream = new VorbisReader(file);
            _output = GetProfile(_stream);
        }
        
        public Decoder(Stream file)
        {
            _stream = new VorbisReader(file);
            _output = GetProfile(_stream);
        }
        
        internal override AudioInfo? GetAdaptiveOutput()
            => _output;
        

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            var pcmData = new float[count / sizeof(float)];
            int samplesRead = _stream.ReadSamples(pcmData, 0, pcmData.Length);

            if (samplesRead <= 0) return 0;
            var data = ConvertOggToSigned32Bit(pcmData);
            
            Buffer.BlockCopy(data, 0, buffer, offset, data.Length);
            return samplesRead * sizeof(float);
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            _stream.SeekTo(offset, origin);
            return _stream.SamplePosition;
        }

        /// <inheritdoc />
        public override long Position
        {
            get => _stream.SamplePosition;
            set => _stream.SamplePosition = value;
        }

        private static AudioInfo GetProfile(VorbisReader reader)
        {
            return new AudioInfo(AudioFormat.Signed32Bit, (AudioChannel)reader.Channels, reader.SampleRate);
        }
        
        private static int Float32BitToSigned32Bit(float value)
        {
            const float maxValue = 1.0f;
            const float minValue = -1.0f;
            const int maxIntValue = int.MaxValue;
            const int minIntValue = int.MinValue;

            return value switch
            {
                >= maxValue => maxIntValue,
                <= minValue => minIntValue,
                _ => (int)(value * maxIntValue)
            };
        }
        
        private static byte[] ConvertOggToSigned32Bit(float[] floatPcm)
        {
            int[] signed32BitPcm = new int[floatPcm.Length];
            for (int i = 0; i < floatPcm.Length; i++)
            {
                float floatSample = floatPcm[i];
                signed32BitPcm[i] = Float32BitToSigned32Bit(floatSample);
            }

            byte[] signed32BitData = new byte[signed32BitPcm.Length * 4];
            Buffer.BlockCopy(signed32BitPcm, 0, signed32BitData, 0, signed32BitData.Length);

            return signed32BitData;
        }
    }
}