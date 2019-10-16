using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Blokus3D
{
    public partial class OptionsPiece : Border
    {
        private readonly Piece _piece;
        private readonly GeometryModel3D _pieceModel;
        private bool _enabled;

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    ChangeState();
                }
            }
        }

        public event ClickedEvent Clicked;
        public delegate void ClickedEvent(Piece piece, bool enabled);

        public OptionsPiece(Piece piece)
        {
            _piece = piece;
            InitializeComponent();
            ChangeState();
            _pieceModel = CreateModel(piece);
            group.Children.Add(_pieceModel);
            StartRotating();
        }

        private GeometryModel3D CreateModel(Piece piece)
        {
            var mesh = new MeshGeometry3D();
            var cubeIndices = Piece3D.CubeIndices;

            for (var c = 0; c < piece.Coordinates.Length; c++)
            {
                var coordinate = piece.Coordinates[c];
                for (int z = 0; z < 2; z++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int x = 0; x < 2; x++)
                        {
                            mesh.Positions.Add(new Point3D(coordinate.X + x, coordinate.Y + y, coordinate.Z + z));
                        }
                    }
                }
                for (int i = 0; i < cubeIndices.Length; i++)
                {
                    mesh.TriangleIndices.Add(c * 8 + cubeIndices[i / 6, i % 6]);
                }
            }

            return new GeometryModel3D(mesh, new DiffuseMaterial(new SolidColorBrush(HelperClass.GetMediaColor(piece.Color))))
            {
                Transform = new Transform3DGroup()
            };
        }

        private void StartRotating()
        {
            var rotate = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(10)) { RepeatBehavior = RepeatBehavior.Forever };
            rotate3D.BeginAnimation(AxisAngleRotation3D.AngleProperty, rotate);
        }

        private void HandleMouseDown(object sender, MouseButtonEventArgs e)
        {
            Enabled = !Enabled;
            Clicked(_piece, Enabled);
        }

        private void ChangeState()
        {
            if (Enabled)
            {
                ambientLight.Color = Color.FromRgb(150, 150, 150);
                directionalLight.Color = Color.FromRgb(150, 150, 150);
                camera.Position = new Point3D(5, 5, 5);
            }
            else
            {
                ambientLight.Color = Color.FromRgb(30, 30, 30);
                directionalLight.Color = Color.FromRgb(10, 10, 10);
                camera.Position = new Point3D(6, 6, 6);
            }
        }
    }
}
