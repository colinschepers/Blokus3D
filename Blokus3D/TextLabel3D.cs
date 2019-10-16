using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Blokus3D
{
    public class TextLabel3D
    {
        public GeometryModel3D Model { get; }

        public TextLabel3D(string text, Brush textColor, bool isDoubleSided, double height, 
            Point3D center, Vector3D over, Vector3D up) : base()
        {
            Model = CreateModel(text, textColor, isDoubleSided, height, center, over, up);
        }

        private static GeometryModel3D CreateModel(string text, Brush textColor, bool isDoubleSided, double height, 
            Point3D center, Vector3D over, Vector3D up)
        {
            var width = text.Length * height;

            var p0 = center - width / 2 * over - height / 2 * up;
            var p1 = p0 + up * 1 * height;
            var p2 = p0 + over * width;
            var p3 = p0 + up * 1 * height + over * width;

            var geometry = new MeshGeometry3D { Positions = new Point3DCollection { p0, p1, p2, p3 } };

            if (isDoubleSided)
            {
                geometry.Positions.Add(p0);
                geometry.Positions.Add(p1);
                geometry.Positions.Add(p2);
                geometry.Positions.Add(p3);
            }

            geometry.TriangleIndices.Add(0);
            geometry.TriangleIndices.Add(3);
            geometry.TriangleIndices.Add(1);
            geometry.TriangleIndices.Add(0);
            geometry.TriangleIndices.Add(2);
            geometry.TriangleIndices.Add(3);

            if (isDoubleSided)
            {
                geometry.TriangleIndices.Add(4);
                geometry.TriangleIndices.Add(5);
                geometry.TriangleIndices.Add(7);
                geometry.TriangleIndices.Add(4);
                geometry.TriangleIndices.Add(7);
                geometry.TriangleIndices.Add(6);
            }

            geometry.TextureCoordinates.Add(new Point(0, 1));
            geometry.TextureCoordinates.Add(new Point(0, 0));
            geometry.TextureCoordinates.Add(new Point(1, 1));
            geometry.TextureCoordinates.Add(new Point(1, 0));

            if (isDoubleSided)
            {
                geometry.TextureCoordinates.Add(new Point(1, 1));
                geometry.TextureCoordinates.Add(new Point(1, 0));
                geometry.TextureCoordinates.Add(new Point(0, 1));
                geometry.TextureCoordinates.Add(new Point(0, 0));
            }

            var textBlock = new TextBlock(new Run(text)) { Foreground = textColor, FontFamily = new FontFamily("Arial") };
            var material = new DiffuseMaterial { Brush = new VisualBrush(textBlock) };
            return new GeometryModel3D(geometry, material);
        }
    }
}
