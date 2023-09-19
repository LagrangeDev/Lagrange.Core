using System.Numerics;
using BitConverter = Lagrange.Core.Utility.Binary.BitConverter;

namespace Lagrange.Core.Utility;

public static class ImageResolver
{
    public static ImageFormat Resolve(byte[] image, out Vector2 size)
    {
        if (image[..6].SequenceEqual(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }) ||
            image[..6].SequenceEqual(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 })) // GIF89a / GIF87a
        {
            size = new Vector2(BitConverter.ToUInt16(image.AsSpan()[6..8]), BitConverter.ToUInt16(image.AsSpan()[8..10]));
            return ImageFormat.Gif;
        }
        
        if (image[..2].SequenceEqual(new byte[] { 0xFF, 0xD8 })) // JPEG
        {
            size = Vector2.Zero;

            if (image[2..4].SequenceEqual(new byte[] { 0xFF, 0xE0 })) // JFIF
            {
                for (int i = 4; i < image.Length - 1; i++)
                {
                    if (image[i] == 0xFF && image[i + 1] >= 0xC0 && image[i + 1] <= 0xC3) // SOF0 ~ SOF3
                    {
                        size = new Vector2(BitConverter.ToUInt16(image.AsSpan()[(i + 7)..(i + 9)], false),BitConverter.ToUInt16(image.AsSpan()[(i + 5)..(i + 7)], false));
                        break;
                    }
                }
            }
            return ImageFormat.Jpeg;
        }
        
        if (image[..8].SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A })) // PNG
        {
            size = new Vector2(BitConverter.ToUInt32(image.AsSpan()[16..20], false), BitConverter.ToUInt32(image.AsSpan()[20..24], false));
            return ImageFormat.Png;
        }

        if (image[..4].SequenceEqual(new byte[] { 0x52, 0x49, 0x46, 0x46 }) && image[8..12].SequenceEqual(new byte[] { 0x57, 0x45, 0x42, 0x50 })) // RIFF WEBP
        {
            if (image[12..16].SequenceEqual(new byte[] { 0x56, 0x50, 0x38, 0x58 })) // VP8X
                size = new Vector2(BitConverter.ToUInt16(image.AsSpan()[24..27]) + 1, BitConverter.ToUInt16(image.AsSpan()[27..30]) + 1);
            else if (image[12..16].SequenceEqual(new byte[] { 0x56, 0x50, 0x38, 0x4C })) // VP8L
                size = new Vector2((BitConverter.ToInt32(image.AsSpan()[21..25]) & 0x3FFF) + 1, ((BitConverter.ToInt32(image.AsSpan()[21..25]) & 0xFFFC000) >> 0x0E) + 1);
            else // VP8 
                size = new Vector2(BitConverter.ToUInt16(image.AsSpan()[26..28]), BitConverter.ToUInt16(image.AsSpan()[28..30]));
            return ImageFormat.Webp;
        }
        
        if (image[..2].SequenceEqual(new byte[] { 0x42, 0x4D })) // BMP
        {
            size = new Vector2(BitConverter.ToUInt16(image.AsSpan()[18..20]), BitConverter.ToUInt16(image.AsSpan()[22..24]));
            return ImageFormat.Bmp;
        }
        
        if (image[..2].SequenceEqual(new byte[] { 0x49, 0x49 }) || image[..2].SequenceEqual(new byte[] { 0x4D, 0x4D })) // TIFF
        {
            size = new Vector2(BitConverter.ToUInt16(image.AsSpan()[18..20]), BitConverter.ToUInt16(image.AsSpan()[30..32]));
            return ImageFormat.Tiff;
        }

        size = Vector2.Zero;
        return ImageFormat.Unknown;
    }
}

public enum ImageFormat
{
    Unknown,
    Png,
    Jpeg,
    Gif,
    Webp,
    Bmp,
    Tiff
}