using ZXing;
using ZXing.QrCode;

namespace Lagrange.OneBot.Utility;

internal static class QrCodeHelper
{
    internal static void Output(string text)
    {
        var writer = new BarcodeWriterGeneric
                     {
                         Format = BarcodeFormat.QR_CODE, Options = new QrCodeEncodingOptions
                         {
                             Margin = 1,
                             QrCompact = true,
                             Hints = {  }
                         },
                     };
        
        var points = writer.Encode(text);

        for (var i = 0; i < points.Width; i++)
        {
            for (var j = 0; j < points.Height; j++)
            {
                var color = points[i, j] ? ConsoleColor.Black : ConsoleColor.White;

                Console.BackgroundColor = color;
                Console.ForegroundColor = color;
                Console.Write("  ");
                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }
}
