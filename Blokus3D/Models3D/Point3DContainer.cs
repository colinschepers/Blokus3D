using Blokus3D.Models;
using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Blokus3D.Models3D
{
    public class Point3DContainer
    {
        private static readonly Point3DContainer _instance = new Point3DContainer();
        private static int _boardSizeX, _boardSizeY, _boardSizeZ;
        private static List<Point3D> _allPoints;

        public static Point3DContainer Instance { get { Load(); return _instance; } }

        private Point3DContainer()
        {
        }

        public List<Point3D> GetAllPoints()
        {
            return _allPoints;
        }

        public Point3D[] GetPositions(Coordinate coordinate)
        {
            var lenX = _boardSizeX + 1;
            var lenY = _boardSizeY + 1;
            var lenZ = _boardSizeZ + 1;
            var baseIndex = (coordinate.Z * lenX * lenY) + (coordinate.Y * lenX) + coordinate.X;

            var positions = new Point3D[8];
            positions[0] = _allPoints[baseIndex];
            positions[1] = _allPoints[baseIndex + 1];
            positions[2] = _allPoints[baseIndex + lenX];
            positions[3] = _allPoints[baseIndex + lenX + 1];
            positions[4] = _allPoints[baseIndex + lenX * lenY];
            positions[5] = _allPoints[baseIndex + lenX * lenY + 1];
            positions[6] = _allPoints[baseIndex + lenX * lenY + lenX];
            positions[7] = _allPoints[baseIndex + lenX * lenY + lenX + 1];

            return positions;
        }

        private static void Load()
        {
            if (_boardSizeX != Configuration.BoardSizeX || _boardSizeY != Configuration.BoardSizeY || _boardSizeZ != Configuration.BoardSizeZ)
            {
                _allPoints = new List<Point3D>();
                _boardSizeX = Configuration.BoardSizeX;
                _boardSizeY = Configuration.BoardSizeY;
                _boardSizeZ = Configuration.BoardSizeZ;

                var lenMax = Math.Max(Math.Max(_boardSizeX, _boardSizeY), _boardSizeZ);

                for (int z = 0; z <= _boardSizeZ; z++)
                {
                    for (int y = 0; y <= _boardSizeY; y++)
                    {
                        for (int x = 0; x <= _boardSizeX; x++)
                        {
                            _allPoints.Add(new Point3D(
                                (-_boardSizeX / 2.0f * (2.0F / lenMax)) + (x * (2.0F / lenMax)),
                                (-_boardSizeY / 2.0f * (2.0F / lenMax)) + (y * (2.0F / lenMax)),
                                (-_boardSizeZ / 2.0f * (2.0F / lenMax)) + (z * (2.0F / lenMax))));
                        }
                    }
                }
            }
        }
    }
}
