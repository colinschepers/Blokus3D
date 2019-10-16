using System.Collections.Generic;

namespace Blokus3D
{
    public class Shapes
    {
        private static List<Coordinate[]>[] _shapes;

        private static int[][] _baseCoordinates = new int[][] {
            new int[] { 0, 0, 0, 1, 0, 0 },
            new int[] { 0, 0, 0, 1, 0, 0, 2, 0, 0 },
            new int[] { 0, 0, 0, 1, 0, 0, 0, 1, 0 },
            new int[] { 0, 0, 0, 1, 0, 0, 2, 0, 0, 3, 0, 0 },
            new int[] { 0, 0, 0, 1, 0, 0, 2, 0, 0, 1, 1, 0 },
            new int[] { 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 1, 0 },
            new int[] { 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 0 },
            new int[] { 0, 0, 0, 1, 0, 0, 1, 1, 0, 2, 1, 0 },
            new int[] { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1 },
            new int[] { 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1 },
            new int[] { 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 1 }
        };

        static Shapes()
        {
            _shapes = new List<Coordinate[]>[_baseCoordinates.Length];
            var permutationCalculator = new PermutationCalculator();
            for (int shapeNr = 0; shapeNr < _baseCoordinates.Length; shapeNr++)
            {
                var coordinates = Coordinate.ParseIntArray(_baseCoordinates[shapeNr]);
                _shapes[shapeNr] = permutationCalculator.Calculate(coordinates);
            }
        }

        public static Coordinate[] GetShape(int shapeNr)
        {
            return _shapes[shapeNr][0];
        }

        public static Coordinate[] GetPermutation(int shapeNr, int permutationNr)
        {
            return _shapes[shapeNr][permutationNr];
        }

        public static int GetNumberOfShapes()
        {
            return _shapes.Length;
        } 

        public static int GetNumberOfPermutations(int shapeNr)
        {
            return _shapes[shapeNr].Count;
        }
    }
}
