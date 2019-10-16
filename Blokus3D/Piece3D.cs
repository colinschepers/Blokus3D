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
        private static readonly float _textDelta = 0.01F;
        private static readonly SolidColorBrush _textBrush = Brushes.Black;

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
        public Model3DGroup TextModel { get; }

        public Piece3D(Piece piece, Color color)
        {
            Model = CreateModel(piece, color);
            TextModel = CreateTextModel(piece);
        }

        private static GeometryModel3D CreateModel(Piece piece, Color color)
        {
            var mesh = new MeshGeometry3D();

            var indicesBase = 0;
            foreach (var coordinate in piece.Coordinates)
            {
                var positions = Point3DContainer.GetPositions(coordinate);

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

        private static Model3DGroup CreateTextModel(Piece piece)
        {
            var textModel = new Model3DGroup();

            var text = piece.ShapeNr.ToString();
            var textSize = GetTextSize();
            int indicesBase = 0;

            foreach (var coordinate in piece.Coordinates)
            {
                var positions = Point3DContainer.GetPositions(coordinate);
                if (!piece.Coordinates.Contains(new Coordinate(coordinate.X, coordinate.Y, coordinate.Z - 1))) // Bottom
                {
                    var center = new Point3D((positions[0].X + positions[1].X) / 2, (positions[0].Y + positions[2].Y) / 2, positions[0].Z - _textDelta);
                    var label = new TextLabel3D(text, _textBrush, true, textSize, center, positions[0] - positions[1], positions[0] - positions[2]);
                    textModel.Children.Add(label.Model);
                }
                if (!piece.Coordinates.Contains(new Coordinate(coordinate.X, coordinate.Y, coordinate.Z + 1))) // Top
                {
                    var center = new Point3D((positions[4].X + positions[5].X) / 2, (positions[4].Y + positions[6].Y) / 2, positions[4].Z + _textDelta);
                    var label = new TextLabel3D(text, _textBrush, true, textSize, center, positions[5] - positions[4], positions[6] - positions[4]);
                    textModel.Children.Add(label.Model);
                }
                if (!piece.Coordinates.Contains(new Coordinate(coordinate.X, coordinate.Y - 1, coordinate.Z))) // Front
                {
                    var center = new Point3D((positions[0].X + positions[1].X) / 2, positions[0].Y - _textDelta, (positions[0].Z + positions[4].Z) / 2);
                    var label = new TextLabel3D(text, _textBrush, true, textSize, center, positions[1] - positions[0], positions[4] - positions[0]);
                    textModel.Children.Add(label.Model);
                }
                if (!piece.Coordinates.Contains(new Coordinate(coordinate.X - 1, coordinate.Y, coordinate.Z))) // Left
                {
                    var center = new Point3D(positions[0].X - _textDelta, (positions[0].Y + positions[2].Y) / 2, (positions[0].Z + positions[4].Z) / 2);
                    var label = new TextLabel3D(text, _textBrush, true, textSize, center, positions[0] - positions[2], positions[4] - positions[0]);
                    textModel.Children.Add(label.Model);
                }
                if (!piece.Coordinates.Contains(new Coordinate(coordinate.X, coordinate.Y + 1, coordinate.Z))) // Back
                {
                    var center = new Point3D((positions[2].X + positions[3].X) / 2, positions[2].Y + _textDelta, (positions[2].Z + positions[6].Z) / 2);
                    var label = new TextLabel3D(text, _textBrush, true, textSize, center, positions[3] - positions[2], positions[6] - positions[2]);
                    textModel.Children.Add(label.Model);
                }
                if (!piece.Coordinates.Contains(new Coordinate(coordinate.X + 1, coordinate.Y, coordinate.Z))) // Right
                {
                    var center = new Point3D(positions[1].X + _textDelta, (positions[1].Y + positions[3].Y) / 2, (positions[1].Z + positions[5].Z) / 2);
                    var label = new TextLabel3D(text, _textBrush, true, textSize, center, positions[1] - positions[3], positions[5] - positions[1]);
                    textModel.Children.Add(label.Model);
                }

                indicesBase += 8;
            }

            return textModel;
        }

        private static double GetTextSize()
        {
            var allPositions = Point3DContainer.GetAllPoints();
            var idx = (Configuration.BoardSizeX + 1) * (Configuration.BoardSizeY + 1) + Configuration.BoardSizeX + 2;
            var blockSizeVector = allPositions[idx] - allPositions[0];
            return Math.Min(Math.Min(blockSizeVector.X, blockSizeVector.Y), blockSizeVector.Z) / 5;
        }
    }
}
