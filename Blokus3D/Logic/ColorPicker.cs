using Blokus3D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Blokus3D.Logic
{
    public class ColorPicker
    {
        public static Color GetMediaColor(PieceColors pieceColor)
        {
            const byte value = 255;
            switch (pieceColor)
            {
                case PieceColors.Red:
                    return Color.FromArgb(255, value, 0, 0);
                case PieceColors.Green:
                    return Color.FromArgb(255, 0, value, 0);
                case PieceColors.Blue:
                    return Color.FromArgb(255, 0, 0, value);
                case PieceColors.Yellow:
                    return Color.FromArgb(255, value, value, 0);
                default:
                    break;
            }
            return Color.FromArgb(0, 0, 0, 0);
        }

        public static Color GetMediaColor(int x)
        {
            const byte alpha = 255;
            switch (x % 24)
            {
                case 0:
                    return Color.FromArgb(alpha, 255, 0, 0);
                case 1:
                    return Color.FromArgb(alpha, 0, 255, 0);
                case 2:
                    return Color.FromArgb(alpha, 0, 0, 255);
                case 3:
                    return Color.FromArgb(alpha, 255, 255, 0);
                case 4:
                    return Color.FromArgb(alpha, 255, 0, 255);
                case 5:
                    return Color.FromArgb(alpha, 0, 255, 255);
                case 6:
                    return Color.FromArgb(alpha, 128, 0, 0);
                case 7:
                    return Color.FromArgb(alpha, 0, 128, 0);
                case 8:
                    return Color.FromArgb(alpha, 0, 0, 128);
                case 9:
                    return Color.FromArgb(alpha, 128, 128, 0);
                case 10:
                    return Color.FromArgb(alpha, 128, 0, 128);
                case 11:
                    return Color.FromArgb(alpha, 0, 128, 128);
                case 12:
                    return Color.FromArgb(alpha, 64, 0, 0);
                case 13:
                    return Color.FromArgb(alpha, 0, 64, 0);
                case 14:
                    return Color.FromArgb(alpha, 0, 0, 64);
                case 15:
                    return Color.FromArgb(alpha, 64, 64, 0);
                case 16:
                    return Color.FromArgb(alpha, 64, 0, 64);
                case 17:
                    return Color.FromArgb(alpha, 0, 64, 64);
                case 18:
                    return Color.FromArgb(alpha, 192, 0, 0);
                case 19:
                    return Color.FromArgb(alpha, 0, 192, 0);
                case 20:
                    return Color.FromArgb(alpha, 0, 0, 192);
                case 21:
                    return Color.FromArgb(alpha, 192, 192, 0);
                case 22:
                    return Color.FromArgb(alpha, 192, 0, 192);
                case 23:
                    return Color.FromArgb(alpha, 0, 192, 192);
                default:
                    break;
            }
            return Color.FromArgb(0, 0, 0, 0);
        }
    }
}
