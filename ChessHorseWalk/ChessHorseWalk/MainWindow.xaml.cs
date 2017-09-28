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
        private Stack<Cell> _tempHistory;
        private int _side;

        public MainWindow()
        {
            InitializeComponent();

            _butReset.Click += (s, e) =>
            {
                _startCell = _finishCell = null;
                _field?.Clear();
                Reset();
            };

            _butFindAllShortest.Click += async (s, e) => await RunProcess(RunFindAllShortest);
            _butFindOneShortest.Click += async (s, e) => await RunProcess(RunFindOneShortest);
            _butFindSinglestepped.Click += async (s, e) => await RunProcess(RunFindSinglestepped);

            Reset();
        }



        private void Reset()
        {
            _histories = new List<Stack<Cell>>();
            _tempHistory = new Stack<Cell>();
            _lblTime.Content = null;
            _cells = _field.Cells;
            _side = _field.Side;
            _butReset.IsEnabled = _butFindAllShortest.IsEnabled = _butFindOneShortest.IsEnabled = _butFindSinglestepped.IsEnabled = false;

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
                        _butFindAllShortest.IsEnabled = _butFindOneShortest.IsEnabled = _butFindSinglestepped.IsEnabled = true;
                    }
                };
            }

            _butReset.IsEnabled = true;
        }




        private async Task RunProcess(Func<Task> task)
        {
            _lblTime.Content = null;
            _butReset.IsEnabled = _butFindAllShortest.IsEnabled = _butFindOneShortest.IsEnabled = _butFindSinglestepped.IsEnabled = false;
            var sw = new Stopwatch();
            sw.Start();
            await task();
            sw.Stop();
            _lblTime.Content = sw.Elapsed;
            _butReset.IsEnabled = true;
        }




        private async Task RunFindAllShortest()
        {
            void RunFindAllShortestRecursive(Cell cell, int steps)
            {
                if (cell.Value < steps)
                    return;

                _tempHistory.Push(cell);

                if (cell == _finishCell)
                {
                    if (cell.Value != steps)
                        _histories.Clear();
                    cell.Value = steps;
                    var history = new Stack<Cell>();
                    foreach (var c in _tempHistory)
                        history.Push(new Cell(c));
                    _histories.Add(history);
                    _tempHistory.Pop();
                    return;
                }

                cell.Value = steps;

                var y = cell.Place.Y;
                var x = cell.Place.X;

                if (y - 2 >= 0 && x - 1 >= 0 && y - 2 < _side && x - 1 < _side)
                    RunFindAllShortestRecursive(_cells[y - 2, x - 1], steps + 1);
                if (y - 2 >= 0 && x + 1 >= 0 && y - 2 < _side && x + 1 < _side)
                    RunFindAllShortestRecursive(_cells[y - 2, x + 1], steps + 1);
                if (y - 1 >= 0 && x + 2 >= 0 && y - 1 < _side && x + 2 < _side)
                    RunFindAllShortestRecursive(_cells[y - 1, x + 2], steps + 1);
                if (y + 1 >= 0 && x + 2 >= 0 && y + 1 < _side && x + 2 < _side)
                    RunFindAllShortestRecursive(_cells[y + 1, x + 2], steps + 1);
                if (y + 2 >= 0 && x + 1 >= 0 && y + 2 < _side && x + 1 < _side)
                    RunFindAllShortestRecursive(_cells[y + 2, x + 1], steps + 1);
                if (y + 2 >= 0 && x - 1 >= 0 && y + 2 < _side && x - 1 < _side)
                    RunFindAllShortestRecursive(_cells[y + 2, x - 1], steps + 1);
                if (y + 1 >= 0 && x - 2 >= 0 && y + 1 < _side && x - 2 < _side)
                    RunFindAllShortestRecursive(_cells[y + 1, x - 2], steps + 1);
                if (y - 1 >= 0 && x - 2 >= 0 && y - 1 < _side && x - 2 < _side)
                    RunFindAllShortestRecursive(_cells[y - 1, x - 2], steps + 1);

                _tempHistory.Pop();
            }

            await Task.Run(() => RunFindAllShortestRecursive(_startCell, 0));

            _tbSide.Text = (_histories.First().Count - 1).ToString();

            foreach (var history in _histories)
            {
                foreach (var cell in history)
                {
                    cell.Label.Content = cell.Value;
                }
            }
        }







        private async Task RunFindOneShortest()
        {
            void RunFindOneShortestRecursive(Cell cell, int steps)
            {
                if (cell.Value <= steps)
                    return;

                _tempHistory.Push(cell);

                if (cell == _finishCell)
                {
                    _histories.Clear();
                    cell.Value = steps;
                    var history = new Stack<Cell>();
                    foreach (var c in _tempHistory)
                        history.Push(new Cell(c));
                    _histories.Add(history);
                    _tempHistory.Pop();
                    return;
                }

                cell.Value = steps;

                var y = cell.Place.Y;
                var x = cell.Place.X;

                if (y - 2 >= 0 && x - 1 >= 0 && y - 2 < _side && x - 1 < _side)
                    RunFindOneShortestRecursive(_cells[y - 2, x - 1], steps + 1);
                if (y - 2 >= 0 && x + 1 >= 0 && y - 2 < _side && x + 1 < _side)
                    RunFindOneShortestRecursive(_cells[y - 2, x + 1], steps + 1);
                if (y - 1 >= 0 && x + 2 >= 0 && y - 1 < _side && x + 2 < _side)
                    RunFindOneShortestRecursive(_cells[y - 1, x + 2], steps + 1);
                if (y + 1 >= 0 && x + 2 >= 0 && y + 1 < _side && x + 2 < _side)
                    RunFindOneShortestRecursive(_cells[y + 1, x + 2], steps + 1);
                if (y + 2 >= 0 && x + 1 >= 0 && y + 2 < _side && x + 1 < _side)
                    RunFindOneShortestRecursive(_cells[y + 2, x + 1], steps + 1);
                if (y + 2 >= 0 && x - 1 >= 0 && y + 2 < _side && x - 1 < _side)
                    RunFindOneShortestRecursive(_cells[y + 2, x - 1], steps + 1);
                if (y + 1 >= 0 && x - 2 >= 0 && y + 1 < _side && x - 2 < _side)
                    RunFindOneShortestRecursive(_cells[y + 1, x - 2], steps + 1);
                if (y - 1 >= 0 && x - 2 >= 0 && y - 1 < _side && x - 2 < _side)
                    RunFindOneShortestRecursive(_cells[y - 1, x - 2], steps + 1);

                _tempHistory.Pop();
            }

            await Task.Run(() => RunFindOneShortestRecursive(_startCell, 0));

            _tbSide.Text = (_histories.First().Count - 1).ToString();

            foreach (var cell in _histories.First())
            {
                cell.Label.Content = cell.Value;
            }
        }






        private async Task RunFindSinglestepped()
        {
            var lim = _side * _side - 1;
            var found = false;

            void RunFindSinglesteppedRecursive(Cell cell, int steps)
            {
                if (found || steps > lim || cell.Value != int.MaxValue)
                    return;

                if (cell == _finishCell)
                {
                    if (steps != lim)
                        return;

                    found = true;
                    _tempHistory.Push(cell);
                    cell.Value = steps;
                    var history = new Stack<Cell>();
                    foreach (var c in _tempHistory)
                        history.Push(new Cell(c));
                    _histories.Add(history);
                    _tempHistory.Pop();
                    return;
                }

                _tempHistory.Push(cell);
                cell.Value = steps;

                var y = cell.Place.Y;
                var x = cell.Place.X;

                if (!found && y - 2 >= 0 && x - 1 >= 0 && y - 2 < _side && x - 1 < _side)
                    RunFindSinglesteppedRecursive(_cells[y - 2, x - 1], steps + 1);
                if (!found && y - 2 >= 0 && x + 1 >= 0 && y - 2 < _side && x + 1 < _side)
                    RunFindSinglesteppedRecursive(_cells[y - 2, x + 1], steps + 1);
                if (!found && y - 1 >= 0 && x + 2 >= 0 && y - 1 < _side && x + 2 < _side)
                    RunFindSinglesteppedRecursive(_cells[y - 1, x + 2], steps + 1);
                if (!found && y + 1 >= 0 && x + 2 >= 0 && y + 1 < _side && x + 2 < _side)
                    RunFindSinglesteppedRecursive(_cells[y + 1, x + 2], steps + 1);
                if (!found && y + 2 >= 0 && x + 1 >= 0 && y + 2 < _side && x + 1 < _side)
                    RunFindSinglesteppedRecursive(_cells[y + 2, x + 1], steps + 1);
                if (!found && y + 2 >= 0 && x - 1 >= 0 && y + 2 < _side && x - 1 < _side)
                    RunFindSinglesteppedRecursive(_cells[y + 2, x - 1], steps + 1);
                if (!found && y + 1 >= 0 && x - 2 >= 0 && y + 1 < _side && x - 2 < _side)
                    RunFindSinglesteppedRecursive(_cells[y + 1, x - 2], steps + 1);
                if (!found && y - 1 >= 0 && x - 2 >= 0 && y - 1 < _side && x - 2 < _side)
                    RunFindSinglesteppedRecursive(_cells[y - 1, x - 2], steps + 1);

                if (found)
                    return;

                _tempHistory.Pop();
                cell.Value = int.MaxValue;
            }

            await Task.Run(() => RunFindSinglesteppedRecursive(_startCell, 0));

            if (_histories.Any())
            {
                _tbSide.Text = (_histories.First().Count - 1).ToString();

                foreach (var cell in _histories.First())
                {
                    cell.Label.Content = cell.Value;
                }
            }
            else
                _tbSide.Text = @"Path not found";
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
