using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blokus3D
{
    public class Coordinate : IComparable, ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Coordinate(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Coordinate operator +(Coordinate c1, Coordinate c2)
        {
            return new Coordinate(c1.X + c2.X, c1.Y + c2.Y, c1.Z + c2.Z);
        }

        public static Coordinate operator -(Coordinate c1, Coordinate c2)
        {
            return new Coordinate(c1.X - c2.X, c1.Y - c2.Y, c1.Z - c2.Z);
        }

        public static bool operator ==(Coordinate c1, Coordinate c2)
        {
            return c1.X == c2.X && c1.Y == c2.Y && c1.Z == c2.Z;
        }

        public static bool operator !=(Coordinate c1, Coordinate c2)
        {
            return c1.X != c2.X || c1.Y != c2.Y || c1.Z != c2.Z;
        } 

        public static Coordinate[] ParseIntArray(int[] intCoordinates)
        {
            var coordinates = new Coordinate[intCoordinates.Length / 3];
            for (int i = 0; i < coordinates.Length; i++)
            {
                coordinates[i] = new Coordinate(intCoordinates[i * 3], intCoordinates[i * 3 + 1], intCoordinates[i * 3 + 2]);
            }
            Array.Sort(coordinates);
            return coordinates;
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinate c && this == c;
        }

        public override int GetHashCode()
        {
            return 43 * X + 53 * Y + 67 * Z;
        }

        public int CompareTo(object obj)
        {
            if (obj is Coordinate other)
            {
                if (Z == other.Z)
                {
                    if (Y == other.Y)
                    {
                        return X.CompareTo(other.X);
                    }
                    return Y.CompareTo(other.Y);
                }
                return Z.CompareTo(other.Z);
            }
            return 0;
        }

        public object Clone()
        {
            return new Coordinate(X, Y, Z);
        }
    }
}
