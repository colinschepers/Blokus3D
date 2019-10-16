using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Blokus3D
{
    public class HelperClass
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

        public static Color GetMediaColor(int shapeNr)
        {
            const byte alpha = 255;
            switch (shapeNr)
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
                    return Color.FromArgb(alpha, 100, 0, 0);
                case 7:
                    return Color.FromArgb(alpha, 0, 100, 0);
                case 8:
                    return Color.FromArgb(alpha, 0, 0, 100);
                case 9:
                    return Color.FromArgb(alpha, 100, 100, 0);
                case 10:
                    return Color.FromArgb(alpha, 100, 0, 100);
                case 11:
                    return Color.FromArgb(alpha, 0, 100, 100);
                default:
                    break;
            }
            return Color.FromArgb(0, 0, 0, 0);
        }

        public static Coordinate[] Copy(Coordinate[] coordinateArray)
        {
            var copy = new Coordinate[coordinateArray.Length];
            for (int i = 0; i < coordinateArray.Length; i++)
            {
                copy[i] = (Coordinate)coordinateArray[i].Clone();
            }
            return copy;
        }
    }
}
