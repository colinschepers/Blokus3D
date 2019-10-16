using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Blokus3D
{
    public class Grid3D
    {
        private static readonly double _delta = 0.001;
        private static readonly int[] _baseIndices = new [] 
        {
            0, 2, 3, 3, 1, 0, 4, 5, 7, 7, 6, 4, 0, 1, 5, 5, 4, 0, 0, 4, 6, 6, 2, 0, 2, 6, 7, 7, 3, 2, 1, 3, 7, 7, 5, 1
        };

        public GeometryModel3D Model { get; }

        public Grid3D()
        {
            Model = CreateModel();
        }

        private GeometryModel3D CreateModel()
        {
            var allPositions = Point3DContainer.GetAllPoints();

            var indicesBase = 0;
            var lenX = Configuration.BoardSizeX + 1;
            var lenY = Configuration.BoardSizeY + 1;
            var lenZ = Configuration.BoardSizeZ + 1;

            var mesh = new MeshGeometry3D();
            for (int z = 0; z < lenZ; z++)
            {
                for (int y = 0; y < lenY; y++)
                {
                    var p1i = z * lenX * lenY + y * lenX;
                    var p1 = allPositions[p1i];
                    var p2 = allPositions[p1i + (lenX - 1)];
                    AddLine(p1, p2, mesh.Positions, mesh.TriangleIndices, ref indicesBase);
                }
            }
            for (int z = 0; z < lenZ; z++)
            {
                for (int x = 0; x < lenX; x++)
                {
                    int p1i = z * lenX * lenY + x;
                    Point3D p1 = allPositions[p1i];
                    Point3D p2 = allPositions[p1i + (lenY - 1) * lenX];
                    AddLine(p1, p2, mesh.Positions, mesh.TriangleIndices, ref indicesBase);
                }
            }
            for (int y = 0; y < lenY; y++)
            {
                for (int x = 0; x < lenX; x++)
                {
                    var p1i = y * lenX + x;
                    var p1 = allPositions[y * lenX + x];
                    var p2 = allPositions[p1i + (lenZ - 1) * lenY * lenX];
                    AddLine(p1, p2, mesh.Positions, mesh.TriangleIndices, ref indicesBase);
                }
            }

            var material = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)));
            return new GeometryModel3D(mesh, material);
        }

        private void AddLine(Point3D pt1, Point3D pt2, Point3DCollection points, Int32Collection indices, ref int indicesBase)
        {
            var pList = new List<Point3D>
            {
                new Point3D(pt1.X - _delta, pt1.Y - _delta, pt1.Z - _delta),
                new Point3D(pt1.X - _delta, pt1.Y + _delta, pt1.Z - _delta),
                new Point3D(pt1.X - _delta, pt1.Y - _delta, pt1.Z + _delta),
                new Point3D(pt1.X - _delta, pt1.Y + _delta, pt1.Z + _delta),
                new Point3D(pt1.X + _delta, pt1.Y - _delta, pt1.Z - _delta),
                new Point3D(pt1.X + _delta, pt1.Y + _delta, pt1.Z - _delta),
                new Point3D(pt1.X + _delta, pt1.Y - _delta, pt1.Z + _delta),
                new Point3D(pt1.X + _delta, pt1.Y + _delta, pt1.Z + _delta),

                new Point3D(pt2.X - _delta, pt2.Y - _delta, pt2.Z - _delta),
                new Point3D(pt2.X - _delta, pt2.Y + _delta, pt2.Z - _delta),
                new Point3D(pt2.X - _delta, pt2.Y - _delta, pt2.Z + _delta),
                new Point3D(pt2.X - _delta, pt2.Y + _delta, pt2.Z + _delta),
                new Point3D(pt2.X + _delta, pt2.Y - _delta, pt2.Z - _delta),
                new Point3D(pt2.X + _delta, pt2.Y + _delta, pt2.Z - _delta),
                new Point3D(pt2.X + _delta, pt2.Y - _delta, pt2.Z + _delta),
                new Point3D(pt2.X + _delta, pt2.Y + _delta, pt2.Z + _delta)
            };

            pList.RemoveAll(p => !(((p.X < pt1.X && p.X < pt2.X) || (p.X > pt1.X && p.X > pt2.X)) &&
                                   ((p.Y < pt1.Y && p.Y < pt2.Y) || (p.Y > pt1.Y && p.Y > pt2.Y)) &&
                                   ((p.Z < pt1.Z && p.Z < pt2.Z) || (p.Z > pt1.Z && p.Z > pt2.Z))));

            foreach (var point in pList)
            {
                points.Add(point);
            }

            foreach (int baseIndex in _baseIndices)
            {
                indices.Add(indicesBase + baseIndex);
            }

            indicesBase += 8;
        }
    }
}
