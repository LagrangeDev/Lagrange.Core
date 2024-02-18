using System.Numerics;
using System.Runtime.CompilerServices;
using BitConverter = Lagrange.Core.Utility.Binary.BitConverter;

namespace Lagrange.Core.Utility;

internal static class ImageResolver
{
    public static ImageFormat Resolve(byte[] image, out Vector2 size)
    {
        ReadOnlySpan<byte> readOnlySpan = image.AsSpan();
        if (readOnlySpan[..6].SequenceEqual(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }) ||
            readOnlySpan[..6].SequenceEqual(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 })) // GIF89a / GIF87a
        {
            size = new Vector2(BitConverter.ToUInt16(readOnlySpan[6..8]), BitConverter.ToUInt16(readOnlySpan[8..10]));
            return ImageFormat.Gif;
        }

        if (readOnlySpan[..2].SequenceEqual(new byte[] { 0xFF, 0xD8 })) // JPEG
        {
            size = Vector2.Zero;
            for (int i = 2; i < readOnlySpan.Length - 10; i++)
            {
                if ((Unsafe.ReadUnaligned<ushort>(ref image[i]) & 0xFCFF) == 0xC0FF) // SOF0 ~ SOF3
                {
                    size = new Vector2(BitConverter.ToUInt16(readOnlySpan[(i + 7)..(i + 9)], false), BitConverter.ToUInt16(readOnlySpan[(i + 5)..(i + 7)], false));
                    break;
                }
            }
            return ImageFormat.Jpeg;
        }

        if (readOnlySpan[..8].SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A })) // PNG
        {
            size = new Vector2(BitConverter.ToUInt32(readOnlySpan[16..20], false), BitConverter.ToUInt32(readOnlySpan[20..24], false));
            return ImageFormat.Png;
        }

        if (readOnlySpan[..4].SequenceEqual(new byte[] { 0x52, 0x49, 0x46, 0x46 }) && readOnlySpan[8..12].SequenceEqual(new byte[] { 0x57, 0x45, 0x42, 0x50 })) // RIFF WEBP
        {
            if (readOnlySpan[12..16].SequenceEqual(new byte[] { 0x56, 0x50, 0x38, 0x58 })) // VP8X
                size = new Vector2(BitConverter.ToUInt16(readOnlySpan[24..27]) + 1, BitConverter.ToUInt16(readOnlySpan[27..30]) + 1);
            else if (readOnlySpan[12..16].SequenceEqual(new byte[] { 0x56, 0x50, 0x38, 0x4C })) // VP8L
                size = new Vector2((BitConverter.ToInt32(readOnlySpan[21..25]) & 0x3FFF) + 1, ((BitConverter.ToInt32(readOnlySpan[21..25]) & 0xFFFC000) >> 0x0E) + 1);
            else // VP8 
                size = new Vector2(BitConverter.ToUInt16(readOnlySpan[26..28]), BitConverter.ToUInt16(readOnlySpan[28..30]));
            return ImageFormat.Webp;
        }

        if (readOnlySpan[..2].SequenceEqual(new byte[] { 0x42, 0x4D })) // BMP
        {
            size = new Vector2(BitConverter.ToUInt16(readOnlySpan[18..20]), BitConverter.ToUInt16(readOnlySpan[22..24]));
            return ImageFormat.Bmp;
        }

        if (readOnlySpan[..2].SequenceEqual(new byte[] { 0x49, 0x49 }) || readOnlySpan[..2].SequenceEqual(new byte[] { 0x4D, 0x4D })) // TIFF
        {
            size = new Vector2(BitConverter.ToUInt16(readOnlySpan[18..20]), BitConverter.ToUInt16(readOnlySpan[30..32]));
            return ImageFormat.Tiff;
        }

        size = Vector2.Zero;
        return ImageFormat.Unknown;
    }
}

internal enum ImageFormat
{
    Unknown,
    Png,
    Jpeg,
    Gif,
    Webp,
    Bmp,
    Tiff
}