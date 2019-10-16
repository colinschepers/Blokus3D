using Blokus3D.Logic;
using Blokus3D.Models;
using Blokus3D.Models3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Blokus3D
{
    public partial class BlokusWindow : Window
    {
        private bool _mouseDown;
        private Point _mouseLastPosition;
        private Transform3D _transform;
        private Solver _solver;
        private List<Board> _solutions;
        private bool _isWatchingSolution;
        private Board _board;
        private int _solutionNr, _pieceNr;

        delegate void BoardUpdateDelegate(Board board);
        delegate void DrawTextDelegate();

        public BlokusWindow()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            InitializeComponent();
            WindowState = WindowState.Maximized;
            Show();
            Initialize();
        }

        private void Initialize()
        {
            _transform = new Transform3DGroup();
            _solutions = new List<Board>();
            _board = new Board(Configuration.BoardSizeX, Configuration.BoardSizeY, Configuration.BoardSizeZ);
            var warmup1 = Point3DContainer.Instance;
            var warmup2 = Piece3DContainer.Instance;
            ResetSolver();
            DrawText();
            DrawGrid();
        }

        private void ResetSolver()
        {
            var board = new Board(Configuration.BoardSizeX, Configuration.BoardSizeY, Configuration.BoardSizeZ);
            board.Changed += new Board.ChangedEvent(BoardChanged);
            _solver = new Solver(board, Configuration.PieceSet.Select(x => (Piece)x.Clone()).ToList());
            _solver.SolutionFound += new Solver.SolutionEventHandler(SolutionFound);
            _solver.Finished += new Solver.FinishedEventHandler(Finished);
        }

        private void DrawText()
        {
            solutionText.Text = _isWatchingSolution
                ? "Solution: " + (_solutions.Any() ? _solutionNr + 1 + "/" + _solutions.Count : "0/0")
                : _solver.Status.ToString();
            pieceText.Text = _isWatchingSolution
                ? "     Piece: " + (_solutions.Any() ? _pieceNr + 1 + "/" + _solutions[_solutionNr].Pieces.Count : "0/0")
                : "Solutions found: " + _solutions.Count;

            if (Configuration.Delay <= 5)
            {
                solutionText.Text += " (drawing disabled)";
            }
        }

        private void DrawGrid()
        {
            var grid3D = new Grid3D();
            grid3D.Model.Transform = _transform;
            group.Children.Add(grid3D.Model);
        }

        private void DrawBoard(Board board)
        {
            for (int i = group.Children.Count - 1; i >= 3; i--)
            {
                group.Children.RemoveAt(i);
            }

            var pieces = board.Pieces.ToList();
            for (int i = 0; i < pieces.Count; i++)
            {
                var piece3D = Piece3DContainer.Instance.GetPiece3D(pieces[i]);
                piece3D.Model.Material = new DiffuseMaterial(new SolidColorBrush(ColorPicker.GetMediaColor(i)));
                piece3D.Model.Transform = _transform;
                group.Children.Add(piece3D.Model);
            }
        }
        
        private void ResetClick(object sender, RoutedEventArgs e)
        {
            _camera.Position = new Point3D(_camera.Position.X, _camera.Position.Y, 7);
            (_transform as Transform3DGroup).Children.Clear();
        }

        private void OptionsClick(object sender, RoutedEventArgs e)
        {
            var optionsWindow = new OptionsWindow();
            if (optionsWindow.ShowDialog() == true)
            {
                if (optionsWindow.BoardChanged)
                {
                    _solver.Status = Solver.SolverStatus.Stopped;
                    for (int i = group.Children.Count - 1; i >= 2; i--)
                    {
                        group.Children.RemoveAt(i);
                    }
                    Initialize();
                }
                else if (_isWatchingSolution)
                {
                    DrawBoard(_board);
                }
            }
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            StopClick(sender, e);
            _solver.Solve();
            DrawText();
        }

        private void PauseClick(object sender, RoutedEventArgs e)
        {
            if (_solver.Status != Solver.SolverStatus.Stopped)
            {
                if (_isWatchingSolution || _solver.Status == Solver.SolverStatus.Paused)
                {
                    _isWatchingSolution = false;
                    _solver.Status = Solver.SolverStatus.Running;
                }
                else _solver.Status = Solver.SolverStatus.Paused;
                DrawText();
            }
        }

        private void StopClick(object sender, RoutedEventArgs e)
        {
            _solver.Status = Solver.SolverStatus.Stopped;
            _solutions.Clear();
            _solutionNr = 0;
            _pieceNr = 0;
            ResetSolver();
            DrawBoard(new Board(Configuration.BoardSizeX, Configuration.BoardSizeY, Configuration.BoardSizeZ));
            DrawText();
            _isWatchingSolution = false;
        }

        private void PreviousSolutionClick(object sender, RoutedEventArgs e)
        {
            if (_solutions.Count > 0)
            {
                _solutionNr = Math.Max(0, _solutionNr - 1);
                _board = (Board)_solutions[_solutionNr].Clone();
                _pieceNr = _board.Pieces.Count - 1;
                DrawBoard(_board);
                _isWatchingSolution = true;
                DrawText();
            }
        }

        private void FirstBlockClick(object sender, RoutedEventArgs e)
        {
            _pieceNr = 0;
            if (_solutions.Count > 0)
            {
                Board board = _solutions[_solutionNr];
                _board = new Board(board.Data.GetLength(0), board.Data.GetLength(1), board.Data.GetLength(2));
                _board.Place(board.Pieces[0]);
                DrawBoard(_board);
                _isWatchingSolution = true;
                DrawText();
            }
        }

        private void PreviousBlockClick(object sender, RoutedEventArgs e)
        {
            if (_solutions.Count > 0)
            {
                if (_pieceNr > 0)
                {
                    _pieceNr--;
                    _board.Remove(_board.Pieces[_board.Pieces.Count - 1]);
                    DrawBoard(_board);
                    DrawText();
                }
                _isWatchingSolution = true;
            }
        }

        private void NextBlockClick(object sender, RoutedEventArgs e)
        {
            if (_solutions.Count > 0)
            {
                Board board = _solutions[_solutionNr];
                _pieceNr = Math.Min(board.Pieces.Count - 1, _pieceNr + 1);
                _board.Place(board.Pieces[_pieceNr]);
                DrawBoard(_board);
                _isWatchingSolution = true;
                DrawText();
            }
        }

        private void LastBlockClick(object sender, RoutedEventArgs e)
        {
            if (_solutions.Count > 0)
            {
                _board = (Board)_solutions[_solutionNr].Clone();
                _pieceNr = _board.Pieces.Count - 1;
                DrawBoard(_board);
                _isWatchingSolution = true;
                DrawText();
            }
        }

        private void NextSolutionClick(object sender, RoutedEventArgs e)
        {
            if (_solutions.Count > 0)
            {
                _solutionNr = Math.Min(_solutions.Count - 1, _solutionNr + 1);
                _board = (Board)_solutions[_solutionNr].Clone();
                _pieceNr = _board.Pieces.Count - 1;
                DrawBoard(_board);
                _isWatchingSolution = true;
                DrawText();
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                var pos = Mouse.GetPosition(viewport);
                var mousePosition = new Point(pos.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - pos.Y);
                var dx = mousePosition.X - _mouseLastPosition.X;
                var dy = mousePosition.Y - _mouseLastPosition.Y;
                var mouseAngle = 0D;

                if (dx != 0 && dy != 0)
                {
                    mouseAngle = Math.Asin(Math.Abs(dy) / Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)));

                    if (dx < 0 && dy > 0)
                    {
                        mouseAngle += Math.PI / 2;
                    }
                    else if (dx < 0 && dy < 0)
                    {
                        mouseAngle += Math.PI;
                    }
                    else if (dx > 0 && dy < 0)
                    {
                        mouseAngle += Math.PI * 1.5;
                    }
                }
                else if (dx == 0 && dy != 0)
                {
                    mouseAngle = Math.Sign(dy) > 0 ? Math.PI / 2 : Math.PI * 1.5;
                }
                else if (dx != 0 && dy == 0)
                {
                    mouseAngle = Math.Sign(dx) > 0 ? 0 : Math.PI;
                }

                var axisAngle = mouseAngle + Math.PI / 2;
                var lastAx = new Vector3D(Math.Cos(axisAngle) * 4, Math.Sin(axisAngle) * 4, 0);
                var lastRot = 0.01 * Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

                var rotation = new QuaternionRotation3D(new Quaternion(lastAx, lastRot * 180 / Math.PI));
                (_transform as Transform3DGroup).Children.Add(new RotateTransform3D(rotation));

                _mouseLastPosition = mousePosition;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            var pos = Mouse.GetPosition(viewport);
            _mouseLastPosition = new Point(pos.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - pos.Y);

            _mouseDown = true;
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mouseDown = false;
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _camera.Position = new Point3D(_camera.Position.X, _camera.Position.Y, _camera.Position.Z - e.Delta / 250D);
        }

        private void DelaySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var newItem = (ComboBoxItem)e.AddedItems[0];
            switch ((string)newItem.Content)
            {
                case ("Slowest"):
                    Configuration.Delay = 5000;
                    break;
                case ("Slow"):
                    Configuration.Delay = 1000;
                    break;
                case ("Normal"):
                    Configuration.Delay = 100;
                    break;
                case ("Fast"):
                    Configuration.Delay = 10;
                    break;
                case ("Fastest"):
                    Configuration.Delay = 0;
                    break;
                default:
                    break;
            }
            if (_solutions != null && _solver != null)
            {
                DrawText();
            }
        }

        public void BoardChanged(Board board)
        {
            Dispatcher.Invoke(new BoardUpdateDelegate(BoardUpdate), board);
        }

        public void BoardUpdate(Board board)
        {
            if (!_isWatchingSolution && _solver.Status == Solver.SolverStatus.Running && Configuration.Delay > 5)
            {
                DrawBoard(board);
            }
        }

        public void SolutionFound(Board board)
        {
            _solutions.Add(board);
            Dispatcher.Invoke(new DrawTextDelegate(DrawText));
        }

        public void Finished()
        {
            _isWatchingSolution = true;
            Dispatcher.Invoke(new DrawTextDelegate(DrawText));
            if(_solutionNr < _solutions.Count)
            {
                Dispatcher.Invoke(new BoardUpdateDelegate(BoardUpdate), _solutions[_solutionNr]);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}
