using System;

namespace Blokus3D
{
    public class Piece3DContainer
    {
        private static int _boardSizeX, _boardSizeY, _boardSizeZ;
        private static Piece3D[,,,,,] _pieces;
        private static Piece3D[,,,,] _singleSetPieces;

        public static Piece3D GetPiece3D(Piece piece)
        {
            CalculateAllPiece3Ds();
            var firstCoord = piece.Coordinates[0];
            return _pieces[(int)piece.Color, piece.ShapeNr, piece.PermutationNr, firstCoord.X, firstCoord.Y, firstCoord.Z];
        }

        public static Piece3D GetSingleSetPiece3D(Piece piece)
        {
            CalculateAllPiece3Ds();
            var firstCoord = piece.Coordinates[0];
            return _singleSetPieces[piece.ShapeNr, piece.PermutationNr, firstCoord.X, firstCoord.Y, firstCoord.Z];
        }

        private static void CalculateAllPiece3Ds()
        {
            if (_boardSizeX != Configuration.BoardSizeX || _boardSizeY != Configuration.BoardSizeY || _boardSizeZ != Configuration.BoardSizeZ)
            {
                _boardSizeX = Configuration.BoardSizeX;
                _boardSizeY = Configuration.BoardSizeY;
                _boardSizeZ = Configuration.BoardSizeZ;

                Board board = new Board(_boardSizeX, _boardSizeY, _boardSizeZ);

                _pieces = new Piece3D[Configuration.ColorCount, Configuration.ShapeCount, Configuration.PermutationCount,
                    Configuration.BoardSizeX, Configuration.BoardSizeY, Configuration.BoardSizeZ];
                _singleSetPieces = new Piece3D[Configuration.ShapeCount, Configuration.PermutationCount,
                    Configuration.BoardSizeX, Configuration.BoardSizeY, Configuration.BoardSizeZ];

                for (int shapeNr = 0; shapeNr < Configuration.ShapeCount; shapeNr++)
                {
                    for (int permNr = 0; permNr < Shapes.GetNumberOfPermutations(shapeNr); permNr++)
                    {
                        for (int x = 0; x < _boardSizeX; x++)
                        {
                            for (int y = 0; y < _boardSizeY; y++)
                            {
                                for (int z = 0; z < _boardSizeZ; z++)
                                {
                                    foreach (PieceColors pieceColor in Enum.GetValues(typeof(PieceColors)))
                                    {
                                        Piece piece = new Piece(pieceColor, shapeNr, permNr);
                                        piece.MoveTo(new Coordinate(x, y, z));
                                        if (board.CanPlace(piece))
                                        {
                                            _pieces[(int)pieceColor, shapeNr, permNr, x, y, z] = new Piece3D(piece, HelperClass.GetMediaColor(piece.Color));
                                            if (_singleSetPieces[shapeNr, permNr, x, y, z] == null)
                                            {
                                                _singleSetPieces[shapeNr, permNr, x, y, z] = new Piece3D(piece, HelperClass.GetMediaColor(piece.ShapeNr));
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
}
