using Blokus3D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Blokus3D
{
    public class Solver
    {
        public enum SolverStatus { Stopped, Running, Paused };

        private Board _board;
        private List<Piece> _pieceSet;

        public int ColorCount { get; }
        public SolverStatus Status { get; set; }

        public event SolutionEventHandler SolutionFound;
        public event FinishedEventHandler Finished;

        public delegate void SolutionEventHandler(Board board);
        public delegate void FinishedEventHandler();

        public Solver(Board board, List<Piece> pieceSet)
        {
            _board = board;
            _pieceSet = pieceSet;
            ColorCount = pieceSet.Select(p => p.Color).Distinct().Count();
        }

        public void Solve()
        {
            Status = SolverStatus.Running;

            new Thread(() => 
            {
                Solve(new Coordinate(0, 0, 0));
                Status = SolverStatus.Stopped;
                Finished();
            }) { IsBackground = true, Priority = ThreadPriority.Lowest }.Start();
        }

        private void Solve(Coordinate coordinate)
        {
            if (BoardHasIsolatedRegion(coordinate))
            {
                return;
            }

            for (int p = _pieceSet.Count - 1; p >= 0; p--)
            {
                var piece = _pieceSet[p];
                piece.MoveTo(coordinate);

                for (int r = 0; r < piece.PermutationCount; r++)
                {
                    if (_board.CanPlace(piece))
                    {
                        while (Status == SolverStatus.Paused)
                        {
                            Thread.Sleep(100);
                        }

                        if (Status == SolverStatus.Stopped)
                        {
                            Thread.CurrentThread.Abort();
                        }

                        _board.Place(piece);
                        _pieceSet.RemoveAt(p);
                        
                        Thread.Sleep(Configuration.Delay);

                        var solutionFound = _board.FreeSquares == 0;

                        if (solutionFound)
                        {
                            SolutionFound((Board)_board.Clone());
                        }
                        else
                        {
                            var nextCoordinate = (Coordinate)coordinate.Clone();
                            do
                            {
                                NextCoordinate(nextCoordinate);
                                if (_board.OutOfBounds(nextCoordinate))
                                {
                                    return;
                                }
                            }
                            while (!_board.IsEmpty(nextCoordinate));

                            Solve(nextCoordinate);
                        }
                        
                        _board.Remove(piece);
                        _pieceSet.Insert(p, piece);

                        if (solutionFound)
                        {
                            return;
                        }
                    }

                    piece.NextPermutation();
                }
            }
        }

        private Coordinate NextCoordinate(Coordinate coordinate)
        {
            coordinate.X = (coordinate.X + 1) % Configuration.BoardSizeX;
            coordinate.Y = coordinate.X == 0 ? (coordinate.Y + 1) % Configuration.BoardSizeY : coordinate.Y;
            coordinate.Z = coordinate.X == 0 && coordinate.Y == 0 ? coordinate.Z + 1 : coordinate.Z;
            return coordinate;
        }

        private bool BoardHasIsolatedRegion(Coordinate coordinate)
        {
            var c = (Coordinate)coordinate.Clone();

            do
            {
                if (_board.IsEmpty(c))
                {
                    Coordinate neighbor;
                    if ((_board.OutOfBounds(neighbor = new Coordinate(c.X + 1, c.Y, c.Z)) || !_board.IsEmpty(neighbor)) &&
                        (_board.OutOfBounds(neighbor = new Coordinate(c.X - 1, c.Y, c.Z)) || !_board.IsEmpty(neighbor)) &&
                        (_board.OutOfBounds(neighbor = new Coordinate(c.X, c.Y + 1, c.Z)) || !_board.IsEmpty(neighbor)) &&
                        (_board.OutOfBounds(neighbor = new Coordinate(c.X, c.Y - 1, c.Z)) || !_board.IsEmpty(neighbor)) &&
                        (_board.OutOfBounds(neighbor = new Coordinate(c.X, c.Y, c.Z + 1)) || !_board.IsEmpty(neighbor)) &&
                        (_board.OutOfBounds(neighbor = new Coordinate(c.X, c.Y, c.Z - 1)) || !_board.IsEmpty(neighbor)))
                    {
                        return true;
                    }
                }
                NextCoordinate(c);
            }
            while (!_board.OutOfBounds(c));

            return false;
        }
    }
}
