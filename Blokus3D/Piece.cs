using System;

namespace Blokus3D
{
    public class Piece : ICloneable
    { 
        public PieceColors Color { get; private set; }
        public int ShapeNr { get; private set; }
        public int PermutationNr { get; private set; }
        public Coordinate[] Coordinates { get; private set; }

        public Piece(PieceColors color, int shapeNr) 
            : this(color, shapeNr, 0)
        {
        }

        public Piece(PieceColors color, int shapeNr, int permutationNr) 
            : this(color, shapeNr, permutationNr, HelperClass.Copy(Shapes.GetPermutation(shapeNr, permutationNr)))
        {
        }

        private Piece(PieceColors color, int shapeNr, int permutationNr, Coordinate[] coordinates)
        {
            Color = color;
            ShapeNr = shapeNr;
            PermutationNr = permutationNr;
            Coordinates = coordinates;
        }

        public void MoveTo(Coordinate coordinate)
        {
            int dx = coordinate.X - Coordinates[0].X;
            int dy = coordinate.Y - Coordinates[0].Y;
            int dz = coordinate.Z - Coordinates[0].Z;
            Move(dx, dy, dz);
        }

        public void Move(int dx, int dy, int dz)
        {
            foreach (var coordinate in Coordinates)
            {
                coordinate.X += dx;
                coordinate.Y += dy;
                coordinate.Z += dz;
            }
        }

        public void NextPermutation()
        {
            var baseCoordinate = (Coordinate)Coordinates[0].Clone();
            PermutationNr = (PermutationNr + 1) % Shapes.GetNumberOfPermutations(ShapeNr);
            Coordinates = HelperClass.Copy(Shapes.GetPermutation(ShapeNr, PermutationNr));
            MoveTo(baseCoordinate);
        }

        public int GetNrOfPermutations()
        {
            return Shapes.GetNumberOfPermutations(ShapeNr);
        }

        public int Size()
        {
            return Coordinates.Length;
        }

        public override bool Equals(object obj)
        {
            if (obj is Piece piece)
            {
                return Color == piece.Color && ShapeNr == piece.ShapeNr && PermutationNr == piece.PermutationNr
                    && Coordinates[0] == piece.Coordinates[0];
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 41 * (int)Color + 53 * ShapeNr + 61 * PermutationNr + 71 * Coordinates[0].GetHashCode();
        }

        public object Clone()
        {
            return new Piece(Color, ShapeNr, PermutationNr, HelperClass.Copy(Coordinates));
        }
    }
}
