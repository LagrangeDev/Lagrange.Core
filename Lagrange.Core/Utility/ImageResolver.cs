using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using BitConverter = Lagrange.Core.Utility.Binary.BitConverter;

namespace Lagrange.Core.Utility;

internal static class ImageResolver
{
    // GIF
    private static readonly byte[] GIF87A_0_6 = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 };
    private static readonly byte[] GIF89A_0_6 = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };
    // JPEG
    private static readonly byte[] JPEG_0_2 = new byte[] { 0xFF, 0xD8 };
    // PNG
    private static readonly byte[] PNG_0_8 = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
    // WEBP
    private static readonly byte[] RIFF_0_4 = new byte[] { 0x52, 0x49, 0x46, 0x46 };
    private static readonly byte[] WEBP_8_12 = new byte[] { 0x57, 0x45, 0x42, 0x50 };
    private static readonly byte[] VP8_12_16 = new byte[] { 0x56, 0x50, 0x38, 0x20 };
    private static readonly byte[] VP8L_12_16 = new byte[] { 0x56, 0x50, 0x38, 0x4C };
    private static readonly byte[] VP8X_12_16 = new byte[] { 0x56, 0x50, 0x38, 0x58 };
    // BMP
    private static readonly byte[] BMP_0_2 = new byte[] { 0x42, 0x4D };
    // TIFF
    private static readonly byte[] LLTIFF_0_2 = new byte[] { 0x49, 0x49 };
    private static readonly byte[] MMTIFF_0_2 = new byte[] { 0x4D, 0x4D };

    public static ImageFormat Resolve(Stream image, out Vector2 size)
    {
        Span<byte> buffer = stackalloc byte[1024];
        int length = image.Read(buffer);

        { // JPEG
            if (buffer[..2].SequenceEqual(JPEG_0_2))
            {
                int read = 2;
                while (true)
                {
                    int remaining = length - read;
                    if (remaining < 9)
                    {
                        buffer[read..length].CopyTo(buffer[..remaining]);
                        if ((length = image.Read(buffer[remaining..])) == 0)
                        {
                            size = Vector2.Zero;
                            return ImageFormat.Unknown;
                        }
                        read = 0;

                        continue;
                    }

                    if ((buffer[read + 1] & 0xFC) == 0xC0) // SOF0 - SOF3
                    {
                        size = new Vector2(BitConverter.ToUInt16(buffer[(read + 7)..(read + 9)], false), BitConverter.ToUInt16(buffer[(read + 5)..(read + 7)], false));
                        return ImageFormat.Jpeg;
                    }

                    int len = 2 + BitConverter.ToUInt16(buffer[(read + 2)..(read + 4)], false);
                    if (len > remaining)
                    {
                        if (image.CanSeek)
                        {
                            image.Seek(len - remaining, SeekOrigin.Current);
                            if ((length = image.Read(buffer)) == 0)
                            {
                                size = Vector2.Zero;
                                return ImageFormat.Unknown;
                            }
                        }
                        else
                        {
                            int skipped = remaining;
                            while (skipped + 1024 > len)
                            {
                                if ((length = image.Read(buffer)) == 0)
                                {
                                    size = Vector2.Zero;
                                    return ImageFormat.Unknown;
                                }
                                skipped += length;
                            }

                            if (image.Read(buffer[..(len - skipped)]) == 0)
                            {
                                size = Vector2.Zero;
                                return ImageFormat.Unknown;
                            }

                            if ((length = image.Read(buffer)) == 0)
                            {
                                size = Vector2.Zero;
                                return ImageFormat.Unknown;
                            }
                        }
                        read = 0;

                        continue;
                    }
                    read += len;
                }
            }
        }

        { // GIF
            Span<byte> header = buffer[..6];
            if (header.SequenceEqual(GIF87A_0_6) || header.SequenceEqual(GIF89A_0_6))
            {
                size = new Vector2(BitConverter.ToUInt16(buffer[6..8]), BitConverter.ToUInt16(buffer[8..10]));
                return ImageFormat.Gif;
            }
        }

        { // PNG
            if (buffer[..8].SequenceEqual(PNG_0_8))
            {
                size = new Vector2(BitConverter.ToUInt32(buffer[16..20], false), BitConverter.ToUInt32(buffer[20..24], false));
                return ImageFormat.Png;
            }
        }

        { // BMP
            if (buffer[..2].SequenceEqual(BMP_0_2))
            {
                size = new Vector2(BitConverter.ToUInt16(buffer[18..20]), BitConverter.ToUInt16(buffer[22..24]));
                return ImageFormat.Bmp;
            }
        }

        { // WEBP
            if (length < 30)
            {
                size = Vector2.Zero;
                return ImageFormat.Unknown;
            }

            if (buffer[..4].SequenceEqual(RIFF_0_4) && buffer[8..12].SequenceEqual(WEBP_8_12)) // RIFF WEBP
            {
                Span<byte> fourCC = buffer[12..16];

                // VP8
                if (fourCC.SequenceEqual(VP8_12_16))
                {
                    size = new Vector2(BitConverter.ToUInt16(buffer[26..28]), BitConverter.ToUInt16(buffer[28..30]));
                    return ImageFormat.Webp;
                }

                // VP8L
                if (fourCC.SequenceEqual(VP8L_12_16))
                {
                    size = new Vector2((BitConverter.ToInt32(buffer[21..25]) & 0x3FFF) + 1, ((BitConverter.ToInt32(buffer[21..25]) & 0xFFFC000) >> 0x0E) + 1);
                    return ImageFormat.Webp;
                }

                // VP8X
                if (fourCC.SequenceEqual(VP8X_12_16))
                {
                    size = new Vector2(BitConverter.ToUInt16(buffer[24..27]), BitConverter.ToUInt16(buffer[27..30]));
                    return ImageFormat.Webp;
                }
            }
        }

        // { // TODO TIFF
        //     bool ll = false;
        //     if (ll = buffer[..2].SequenceEqual(LLTIFF_0_2) || buffer[..2].SequenceEqual(MMTIFF_0_2))
        //     {
        //         uint offset = BitConverter.ToUInt32(buffer[4..8], ll);
        //         int read = (int)offset;
        //         while (true)
        //         {
        //             int remaining = length - read;
        //             if (remaining < 2)
        //             {
        //                 buffer[read..length].CopyTo(buffer[..remaining]);
        //                 if ((length = image.Read(buffer[remaining..])) == 0)
        //                 {
        //                     size = Vector2.Zero;
        //                     return ImageFormat.Unknown;
        //                 }
        //                 read = 0;

        //                 continue;
        //             }

        //             int count = BitConverter.ToUInt16(buffer[read..2]);
        //             for (int i = 0; i < count; i++)
        //             {
        //                 // TODO: read fd
        //             }
        //         }
        //     }
        // }

        size = Vector2.Zero;
        return ImageFormat.Unknown;
    }
}

internal enum ImageFormat : uint
{
    Unknown,  // regard as jpg
    Png = 1001,
    Jpeg = 1000,
    Gif = 2000,
    Webp = 1002,
    Bmp = 1005
    // Tiff
}