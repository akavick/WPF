using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessHorseWalk
{
    public partial class MainWindow
    {
        private Cell _startCell;
        private Cell _finishCell;
        private Cell[,] _cells;
        private List<Stack<Cell>> _histories;
        private int _shortestPath;
        private Stack<Cell> _tempHistory;
        private int _side;
        private Random _random = new Random();

        public MainWindow()
        {
            InitializeComponent();

            _butReset.Click += (s, e) =>
            {
                _startCell = _finishCell = null;
                _field?.Clear();
                Reset();
            };

            _butStart.Click += async (s, e) => await RunProcess();

            Reset();
        }



        private void Reset()
        {
            _histories = new List<Stack<Cell>>();
            _tempHistory = new Stack<Cell>();
            _shortestPath = 1;
            _lblTime.Content = null;
            _cells = _field.Cells;
            _side = _field.Side;
            _butReset.IsEnabled = _butStart.IsEnabled = false;

            foreach (var cell in _cells)
            {
                cell.Value = int.MaxValue;
                cell.Label.MouseUp += (s, e) =>
                {
                    if (_startCell == null)
                    {
                        cell.Label.Content = "S";
                        _startCell = cell;
                    }
                    else if (_finishCell == null)
                    {
                        cell.Label.Content = "F";
                        _finishCell = cell;
                        _butStart.IsEnabled = true;
                    }
                };
            }

            _butReset.IsEnabled = true;
        }





        private async Task RunProcess()
        {
            _lblTime.Content = null;
            _butReset.IsEnabled = _butStart.IsEnabled = false;

            var sw = new Stopwatch();
            sw.Start();
            await Task.Run(() => RunProcessRecursive(_startCell, 0));
            //await RunProcessRecursive(_startCell, 0);
            sw.Stop();

            _tbSide.Text = _shortestPath.ToString();

            var r = _random;
            foreach (var history in _histories)
            {
                var brush = new SolidColorBrush(Color.FromRgb((byte)r.Next(256), (byte)r.Next(256), (byte)r.Next(256)));
                foreach (var cell in history)
                {
                    cell.Label.Background = brush;
                    cell.Label.Content = cell.Value;
                }
            }

            _lblTime.Content = sw.Elapsed;
            _butReset.IsEnabled = _butStart.IsEnabled = true;
        }



        private void RunProcessRecursive(Cell cell, int steps)
        {
            if (cell.Value < steps)
                return;

            if (cell.Value == steps)
            {
                if (cell == _finishCell)
                {
                    var history = new Stack<Cell>();
                    foreach (var c in _tempHistory)
                        history.Push(new Cell(c));
                    _histories.Add(history);
                }
                else
                    return;
            }

            cell.Value = steps;
            _tempHistory.Push(cell);

            if (cell == _finishCell)
            {
                if (_histories.Count == 0 || _tempHistory.Count < _shortestPath)
                {
                    _shortestPath = _tempHistory.Count;
                    var history = new Stack<Cell>();
                    foreach (var c in _tempHistory)
                        history.Push(new Cell(c));
                    _histories.Clear();
                    _histories.Add(history);
                }
            }

            var y = cell.Place.Y;
            var x = cell.Place.X;

            if (y - 2 >= 0 && x - 1 >= 0 && y - 2 < _side && x - 1 < _side)
            {
                RunProcessRecursive(_cells[y - 2, x - 1], steps + 1);
            }
            if (y - 2 >= 0 && x + 1 >= 0 && y - 2 < _side && x + 1 < _side)
            {
                RunProcessRecursive(_cells[y - 2, x + 1], steps + 1);
            }
            if (y - 1 >= 0 && x + 2 >= 0 && y - 1 < _side && x + 2 < _side)
            {
                RunProcessRecursive(_cells[y - 1, x + 2], steps + 1);
            }
            if (y + 1 >= 0 && x + 2 >= 0 && y + 1 < _side && x + 2 < _side)
            {
                RunProcessRecursive(_cells[y + 1, x + 2], steps + 1);
            }
            if (y + 2 >= 0 && x + 1 >= 0 && y + 2 < _side && x + 1 < _side)
            {
                RunProcessRecursive(_cells[y + 2, x + 1], steps + 1);
            }
            if (y + 2 >= 0 && x - 1 >= 0 && y + 2 < _side && x - 1 < _side)
            {
                RunProcessRecursive(_cells[y + 2, x - 1], steps + 1);
            }
            if (y + 1 >= 0 && x - 2 >= 0 && y + 1 < _side && x - 2 < _side)
            {
                RunProcessRecursive(_cells[y + 1, x - 2], steps + 1);
            }
            if (y - 1 >= 0 && x - 2 >= 0 && y - 1 < _side && x - 2 < _side)
            {
                RunProcessRecursive(_cells[y - 1, x - 2], steps + 1);
            }

            _tempHistory.Pop();
        }
























        //private async Task RunProcessRecursive(Cell cell, int steps)
        //{
        //    await Task.Delay(1);

        //    if (cell.Value <= steps)
        //        return;

        //    cell.Value = steps;
        //    await Dispatcher.InvokeAsync(() =>
        //    {
        //        cell.Label.Content = steps;
        //    });

        //    if (cell == _finishCell)
        //    {
        //        if (steps < minLen)
        //            minLen = steps;
        //        //todo history
        //    }

        //    var y = cell.Place.Y;
        //    var x = cell.Place.X;

        //    var possiblePositions = new[]
        //    {
        //        (y:y - 2, x:x - 1),
        //        (y:y - 2, x:x + 1),
        //        (y:y - 1, x:x + 2),
        //        (y:y + 1, x:x + 2),
        //        (y:y + 2, x:x + 1),
        //        (y:y + 2, x:x - 1),
        //        (y:y + 1, x:x - 2),
        //        (y:y - 1, x:x - 2)
        //    };

        //    foreach (var p in possiblePositions)
        //    {
        //        if (p.x > -1 && p.x < _side && p.y > -1 && p.y < _side)
        //            await RunProcessRecursive(_cells[p.y, p.x], steps + 1);
        //    }
        //}






    }
}
