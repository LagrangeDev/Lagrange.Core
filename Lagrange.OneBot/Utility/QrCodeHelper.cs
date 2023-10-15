using Net.Codecrete.QrCodeGenerator;

namespace Lagrange.OneBot.Utility;

internal static class QrCodeHelper
{
    internal static void Output(string text)
    {
        var qrCode = QrCode.EncodeText(text, QrCode.Ecc.Low);
        
        for (var y = 0; y < qrCode.Size + 2; y += 2)
        {
            for (var x = 0; x < qrCode.Size + 2; ++x)
            {
                Console.ForegroundColor = qrCode.GetModule(x - 1, y - 1)
                    ? ConsoleColor.Black
                    : ConsoleColor.White;

                Console.BackgroundColor = qrCode.GetModule(x - 1, y) || y > qrCode.Size
                    ? ConsoleColor.Black
                    : ConsoleColor.White;

                Console.Write("▀");
            }
            Console.Write("\n");
        }
        
        Console.ResetColor();
    }
}
