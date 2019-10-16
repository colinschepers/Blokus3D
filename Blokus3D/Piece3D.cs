using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Blokus3D
{
    public class Piece3D
    {
        public static int[,] CubeIndices { get; } =
        {
            { 0, 2, 3, 3, 1, 0 },   // Bottom
            { 4, 5, 7, 7, 6, 4 },   // Top
            { 0, 1, 5, 5, 4, 0 },   // Front
            { 0, 4, 6, 6, 2, 0 },   // Left
            { 2, 6, 7, 7, 3, 2 },   // Back
            { 1, 3, 7, 7, 5, 1 }    // Right
        };

        public GeometryModel3D Model { get; }

        public Piece3D(Piece piece, Color color)
        {
            Model = CreateModel(piece, color);
        }

        private static GeometryModel3D CreateModel(Piece piece, Color color)
        {
            var mesh = new MeshGeometry3D();

            var indicesBase = 0;
            foreach (var coordinate in piece.Coordinates)
            {
                var positions = Point3DContainer.Instance.GetPositions(coordinate);

                foreach (var position in positions)
                {
                    mesh.Positions.Add(position);
                }

                if (!piece.Coordinates.Contains(new Coordinate(coordinate.X, coordinate.Y, coordinate.Z - 1))) // Bottom
                {
                    for (int i = 0; i < CubeIndices.GetLength(1); i++) mesh.TriangleIndices.Add(indicesBase + CubeIndices[0, i]);
                }
                if (!piece.Coordinates.Contains(new Coordinate(coordinate.X, coordinate.Y, coordinate.Z + 1))) // Top
                {
                    for (int i = 0; i < CubeIndices.GetLength(1); i++) mesh.TriangleIndices.Add(indicesBase + CubeIndices[1, i]);
                }
                if (!piece.Coordinates.Contains(new Coordinate(coordinate.X, coordinate.Y - 1, coordinate.Z))) // Front
                {
                    for (int i = 0; i < CubeIndices.GetLength(1); i++) mesh.TriangleIndices.Add(indicesBase + CubeIndices[2, i]);
                }
                if (!piece.Coordinates.Contains(new Coordinate(coordinate.X - 1, coordinate.Y, coordinate.Z))) // Left
                {
                    for (int i = 0; i < CubeIndices.GetLength(1); i++) mesh.TriangleIndices.Add(indicesBase + CubeIndices[3, i]);
                }
                if (!piece.Coordinates.Contains(new Coordinate(coordinate.X, coordinate.Y + 1, coordinate.Z))) // Back
                {
                    for (int i = 0; i < CubeIndices.GetLength(1); i++) mesh.TriangleIndices.Add(indicesBase + CubeIndices[4, i]);
                }
                if (!piece.Coordinates.Contains(new Coordinate(coordinate.X + 1, coordinate.Y, coordinate.Z))) // Right
                {
                    for (int i = 0; i < CubeIndices.GetLength(1); i++) mesh.TriangleIndices.Add(indicesBase + CubeIndices[5, i]);
                }

                indicesBase += 8;
            }

            return new GeometryModel3D(mesh, new DiffuseMaterial(new SolidColorBrush(color)));
        }
    }
}
