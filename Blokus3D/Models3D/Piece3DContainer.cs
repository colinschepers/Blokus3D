using Blokus3D.Logic;
using Blokus3D.Models;
using System;
using System.Linq;

namespace Blokus3D.Models3D
{
    public class Piece3DContainer
    {
        private static readonly Piece3DContainer _instance = new Piece3DContainer();
        private static int _boardSizeX, _boardSizeY, _boardSizeZ;
        private static Piece3D[,,,,,] _pieces;

        public static Piece3DContainer Instance { get { Load(); return _instance; } }

        private Piece3DContainer()
        {
        }

        public Piece3D GetPiece3D(Piece piece)
        {
            var firstCoord = piece.Coordinates[0];
            return _pieces[(int)piece.Color, piece.ShapeNr, piece.PermutationNr, firstCoord.X, firstCoord.Y, firstCoord.Z];
        }
        
        private static void Load()
        {
            if (_boardSizeX != Configuration.BoardSizeX || _boardSizeY != Configuration.BoardSizeY || _boardSizeZ != Configuration.BoardSizeZ)
            {
                _boardSizeX = Configuration.BoardSizeX;
                _boardSizeY = Configuration.BoardSizeY;
                _boardSizeZ = Configuration.BoardSizeZ;

                var board = new Board(_boardSizeX, _boardSizeY, _boardSizeZ);

                _pieces = new Piece3D[Configuration.ColorCount, Configuration.ShapeCount, Configuration.PermutationCount,
                    Configuration.BoardSizeX, Configuration.BoardSizeY, Configuration.BoardSizeZ];

                for (int shapeNr = 0; shapeNr < Configuration.ShapeCount; shapeNr++)
                {
                    for (int permNr = 0; permNr < Permutations.GetPermutationCount(shapeNr); permNr++)
                    {
                        for (int x = 0; x < _boardSizeX; x++)
                        {
                            for (int y = 0; y < _boardSizeY; y++)
                            {
                                for (int z = 0; z < _boardSizeZ; z++)
                                {
                                    foreach (var pieceColor in Enum.GetValues(typeof(PieceColors)).Cast<PieceColors>())
                                    {
                                        Piece piece = new Piece(pieceColor, shapeNr, permNr);
                                        piece.MoveTo(new Coordinate(x, y, z));
                                        if (board.CanPlace(piece))
                                        {
                                            _pieces[(int)pieceColor, shapeNr, permNr, x, y, z] = new Piece3D(piece, ColorPicker.GetMediaColor(piece.Color));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
