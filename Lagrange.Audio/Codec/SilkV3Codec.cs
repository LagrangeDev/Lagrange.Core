using System.Text;
using System.Runtime.InteropServices;
using Lagrange.Audio.Exceptions;

namespace Lagrange.Audio.Codec;

/// <summary>
/// Silk codec
/// </summary>
public static partial class SilkV3Codec
{
    [return: MarshalAs(UnmanagedType.Bool)]
    [LibraryImport("SilkCodec", EntryPoint = "silkEncode")]
    private static partial bool SilkEncode(IntPtr pcmData, int dataLen, int sampleRate, CodecCallback cb, IntPtr userData);

    [return: MarshalAs(UnmanagedType.Bool)]
    [LibraryImport("SilkCodec", EntryPoint = "silkDecode")]
    private static partial bool SilkDecode(IntPtr silkData, int dataLen, int sampleRate, CodecCallback cb, IntPtr userData);

    private delegate void CodecCallback(IntPtr userData, IntPtr p, int len);

    /// <summary>
    /// SilkCodec encoder
    /// </summary>
    public class Encoder : AudioStream
    {
        private bool _firstRead;

        /// <summary>
        /// SilkCodec encoder
        /// </summary>
        public Encoder() => _firstRead = true;

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_firstRead)  // Encode
            {
                _firstRead = false;
                
                var pcmData = ToArray();   // Duplicate the data
                var lpPcmData = Marshal.AllocHGlobal(pcmData.Length);

                Marshal.Copy(pcmData, 0, lpPcmData, pcmData.Length);  // Copy the data

                SetLength(0);  // Cleanup the stream
                Position = 0;

                try
                {
                    // Prepare the stream
                    using (var binaryWriter = new BinaryWriter(this, Encoding.Default, true))
                    {
                        // Encode the pcm data
                        bool result = SilkEncode(lpPcmData, pcmData.Length, 24000, (_, data, length) =>
                        {
                            var outbuf = new byte[length];  // Copy the part
                            Marshal.Copy(data, outbuf, 0, length);

                            binaryWriter.Write(outbuf);  // Write to stream
                        }, IntPtr.Zero);
                        
                        if (!result) throw new EncodeException(""); // Failed
                    }
                    
                    Position = 0;  // Move position
                }
                catch  // Catch native exceptions
                {
                    throw new EncodeException("Thrown an exception while encoding silk.");
                }
                finally  // Cleanup
                {
                    Marshal.FreeHGlobal(lpPcmData);
                }
            }

            return base.Read(buffer, offset, count);
        }
    }

    /// <summary>
    /// SilkCodec decoder
    /// </summary>
    public class Decoder : AudioStream
    {
        private bool _firstRead;

        /// <summary>
        /// SilkCodec decoder
        /// </summary>
        public Decoder() => _firstRead = true;

        /// <summary>
        /// Get adaptive output
        /// </summary>
        internal override AudioInfo? GetAdaptiveOutput() => AudioInfo.SilkV3();

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_firstRead)  // decode
            {
                _firstRead = false;

                var silkData = ToArray();  // Duplicate the data
                var lpSilkData = Marshal.AllocHGlobal(silkData.Length);

                Marshal.Copy(silkData, 0, lpSilkData, silkData.Length);  // Copy the data

                SetLength(0);  // Cleanup the stream
                Position = 0;

                try
                {
                    // Prepare the stream
                    using (var binaryWriter = new BinaryWriter(this, Encoding.Default, true))
                    {
                        // Decode the silk data
                        bool result = SilkDecode(lpSilkData, silkData.Length, 24000, (_, data, length) =>
                        {
                            var outbuf = new byte[length];  // Copy the part
                            Marshal.Copy(data, outbuf, 0, length);

                            binaryWriter.Write(outbuf); // Write to stream
                        }, IntPtr.Zero);

                        if (!result) throw new EncodeException(""); // Failed

                        Position = 0; // Move position
                    }
                }
                catch  // Catch native exceptions
                {
                    throw new EncodeException("Thrown an exception while encoding silk.");
                }
                finally  // Cleanup
                {
                    Marshal.FreeHGlobal(lpSilkData);
                }
            }

            return base.Read(buffer, offset, count);
        }
    }
}