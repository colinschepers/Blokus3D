using System;
using System.Collections.Generic;

namespace Blokus3D
{
    public class PermutationCalculator
    {
        public List<Coordinate[]> Calculate(Coordinate[] coordinates)
        {
            List<Coordinate[]> permutations = new List<Coordinate[]>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        TryAdd(coordinates, permutations);
                        coordinates = RotateXYPlane(coordinates);
                    }
                    coordinates = RotateXZPlane(coordinates);
                }
                coordinates = RotateYZPlane(coordinates);
            }
            return permutations;
        }

        private static Coordinate[] RotateXYPlane(Coordinate[] coordinates)
        {
            Coordinate[] newCoordinates = new Coordinate[coordinates.Length];
            for (int i = 0; i < coordinates.Length; i++)
            {
                Coordinate oldCoordinate = coordinates[i];
                int newX = oldCoordinate.Y;
                int newY = oldCoordinate.X * -1;
                int newZ = oldCoordinate.Z;
                newCoordinates[i] = new Coordinate(newX, newY, newZ);
            }
            Array.Sort(newCoordinates);
            Normalize(ref newCoordinates);
            return newCoordinates;
        }

        private static Coordinate[] RotateXZPlane(Coordinate[] coordinates)
        {
            Coordinate[] newCoordinates = new Coordinate[coordinates.Length];
            for (int i = 0; i < coordinates.Length; i++)
            {
                Coordinate oldCoordinate = coordinates[i];
                int newX = oldCoordinate.Z;
                int newY = oldCoordinate.Y;
                int newZ = oldCoordinate.X * -1;
                newCoordinates[i] = new Coordinate(newX, newY, newZ);
            }
            Array.Sort(newCoordinates);
            Normalize(ref newCoordinates);
            return newCoordinates;
        }

        private static Coordinate[] RotateYZPlane(Coordinate[] coordinates)
        { 
            Coordinate[] newCoordinates = new Coordinate[coordinates.Length];
            for (int i = 0; i < coordinates.Length; i++)
            {
                Coordinate oldCoordinate = coordinates[i];
                int newX = oldCoordinate.X;
                int newY = oldCoordinate.Z;
                int newZ = oldCoordinate.Y * -1;
                newCoordinates[i] = new Coordinate(newX, newY, newZ); 
            }
            Array.Sort(newCoordinates);
            Normalize(ref newCoordinates);
            return newCoordinates;
        }

        private static void Normalize(ref Coordinate[] coordinates)
        {
            for (int i = coordinates.Length - 1; i >= 0; i--)
            {
                coordinates[i] -= coordinates[0];
            }
        }

        private void TryAdd(Coordinate[] coordinates, List<Coordinate[]> permutations)
        {
            foreach (var permutation in permutations)
            {
                bool samePerm = true;
                for (int i = 0; i < permutation.Length; i++)
			    {
                    if (permutation[i] != coordinates[i])
                    {
                        samePerm = false;
                        break;
                    }
			    }
                if (samePerm)
                {
                    return;
                }
            }
            permutations.Add(coordinates);
        }
    }
}
