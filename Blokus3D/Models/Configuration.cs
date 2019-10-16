using Blokus3D.Models;
using System.Collections.Generic;

namespace Blokus3D.Models
{
    public class Configuration
    {
        public static int BoardSizeX { get; set; } = 3;
        public static int BoardSizeY { get; set; } = 3;
        public static int BoardSizeZ { get; set; } = 3;
        public static int ColorCount { get; set; } = 4;
        public static int ShapeCount { get; set; } = 11;
        public static int PermutationCount { get; set; } = 24;
        public static int Delay { get; set; } = 0;
        public static List<Piece> PieceSet { get; set; } = CreatePieceSet();

        private static List<Piece> CreatePieceSet()
        {
            var pieceSet = new List<Piece>();
            for (int shapeNr = 0; shapeNr < ShapeCount; shapeNr++)
            {
                pieceSet.Add(new Piece(PieceColors.Red, shapeNr));
            }
            return pieceSet;
        }
    }
}
