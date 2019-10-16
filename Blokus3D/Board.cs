using System;
using System.Collections.Generic;
using System.Linq;

namespace Blokus3D
{
    public class Board : ICloneable
    {
        public Piece[,,] Data { get; }
        public List<Piece> Pieces { get; }
        public int FreeSquares { get; private set; }

        public event ChangedEvent Changed;
        public delegate void ChangedEvent(Board board);

        public Board(int sizeX, int sizeY, int sizeZ)
        {
            Data = new Piece[sizeX, sizeY, sizeZ];
            Pieces = new List<Piece>();
            FreeSquares = Data.Length;
        }

        public bool Place(Piece piece)
        {
            if (CanPlace(piece))
            {
                Pieces.Add(piece);
                foreach (var coordinate in piece.Coordinates)
                {
                    Data[coordinate.X, coordinate.Y, coordinate.Z] = piece;
                    FreeSquares--;
                }
                Changed?.Invoke(this);
                return true;
            }
            return false;
        }

        public bool Remove(Piece piece)
        {
            if (Pieces.Remove(piece))
            {
                foreach (var coordinate in piece.Coordinates)
                {
                    Data[coordinate.X, coordinate.Y, coordinate.Z] = null;
                    FreeSquares++;
                }
                Changed?.Invoke(this);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            if (Pieces.Count > 0)
            {
                Pieces.Clear();
                Array.Clear(Data, 0, Data.Length);
                FreeSquares = Data.Length;
                Changed?.Invoke(this);
            }
        }

        public bool CanPlace(Piece piece)
        {
            return piece.Coordinates.All(x => !OutOfBounds(x) && IsEmpty(x));
        }

        public bool OutOfBounds(Coordinate coordinate)
        {
            return coordinate.X < 0 || coordinate.X >= Data.GetLength(0) ||
                coordinate.Y < 0 || coordinate.Y >= Data.GetLength(1) ||
                coordinate.Z < 0 || coordinate.Z >= Data.GetLength(2);
        }

        public bool IsEmpty(Coordinate coordinate)
        {
            return Data[coordinate.X, coordinate.Y, coordinate.Z] == null;
        }

        public object Clone()
        {
            var copy = new Board(Data.GetLength(0), Data.GetLength(1), Data.GetLength(2))
            {
                FreeSquares = FreeSquares
            };

            foreach (var piece in Pieces)
            {
                var pieceCopy = (Piece)piece.Clone();
                copy.Pieces.Add(pieceCopy);

                foreach (var coordinate in pieceCopy.Coordinates)
                {
                    copy.Data[coordinate.X, coordinate.Y, coordinate.Z] = pieceCopy;
                }
            }

            return copy;
        }
    }
}
