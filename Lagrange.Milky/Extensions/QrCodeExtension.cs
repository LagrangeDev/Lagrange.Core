using System;
using System.Text;
using Net.Codecrete.QrCodeGenerator;

namespace Lagrange.Milky.Extensions;

public static class QrCodeExtension
{
    public static string ToAscii(this QrCode qrCode, bool compatible)
    {
        StringBuilder result = new();
        for (int y = 0; y < qrCode.Size; y += 2)
        {
            for (int x = 0; x < qrCode.Size; x++)
            {
                bool top = qrCode.GetModule(x, y);
                bool bottom = qrCode.GetModule(x, y + 1);

                result.Append((top, bottom) switch
                {
                    (true, true) => compatible ? '@' : '█',
                    (true, false) => compatible ? '^' : '▀',
                    (false, true) => compatible ? '.' : '▄',
                    (false, false) => ' ',
                });
            }
            if (y < qrCode.Size) result.Append(Environment.NewLine);
        }
        return result.ToString();
    }
}