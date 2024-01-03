using Net.Codecrete.QrCodeGenerator;

namespace Lagrange.OneBot.Utility;

internal static class QrCodeHelper
{
    // This part of the code is from "https://github.com/eric2788/Lagrange.Core/blob/fd20a5aec81cacd56d60f3130cf057461300fd3f/Lagrange.OneBot/Utility/QrCodeHelper.cs#L30C52-L30C52"
    // Thanks to "https://github.com/eric2788"
    internal static void Output(string text)
    {
        var segments = QrSegment.MakeSegments(text);
        var qrCode = QrCode.EncodeSegments(segments, QrCode.Ecc.Low);

        for (var y = 0; y < qrCode.Size + 2; y += 2)
        {
            for (var x = 0; x < qrCode.Size + 2; ++x)
            {
                var foregroundBlack = qrCode.GetModule(x - 1, y - 1);
                var backgroundBlack = qrCode.GetModule(x - 1, y) || y > qrCode.Size;

                if (foregroundBlack && !backgroundBlack)
                {
                    Console.Write("▄");
                } else if (!foregroundBlack && backgroundBlack)
                {
                    Console.Write("▀");
                } else if (foregroundBlack && backgroundBlack)
                {
                    Console.Write(" ");

                } else if (!foregroundBlack && !backgroundBlack)
                {
                    Console.Write("█");
                }
            }
            Console.Write("\n");
        }
    }
}
