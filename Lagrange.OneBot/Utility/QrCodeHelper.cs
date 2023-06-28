using ZXing;
using ZXing.QrCode;

namespace Lagrange.OneBot.Utility;

public static class QrCodeHelper
{
    public static void Output(string text)
    {
        const int threshold = 180;

        var writer = new ZXing.SkiaSharp.BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Width = 33,
                Height = 33,
                Margin = 1
            }
        };
        var image = writer.Write(text);
        var points = new int[image.Width, image.Height];

        for (var i = 0; i < image.Width; i++)
        {
            for (var j = 0; j < image.Height; j++)
            {
                var color = image.GetPixel(i, j);
                points[i, j] = color.Blue <= threshold ? 1 : 0;
            }
        }

            
        for (var i = 0; i < image.Width; i++)
        {
            for (var j = 0; j < image.Height; j++)
            {
                if (points[i, j] == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.Write("  ");
                Console.ResetColor();
            }
            Console.Write("\n");
        }
    }

}