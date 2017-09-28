using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ChessHorseWalk
{
    public partial class MainWindow
    {
        private Cell _startCell;
        private Cell _finishCell;
        private Cell[,] _cells;
        private List<List<Cell>> _histories;
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
            var sideEntered = int.TryParse(_tbSide.Text, out var side);
            if (sideEntered && side > 0 && side < 101)
                _field.FillNewField(side);
            _histories = new List<List<Cell>>();
            _tempHistory = new Stack<Cell>();
            _lblTime.Content = _lblResult.Content = null;
            _cells = _field.Cells;
            _side = _field.Side;
            _butReset.IsEnabled = _butFindAllShortest.IsEnabled = _butFindOneShortest.IsEnabled = _butFindSinglestepped.IsEnabled = false;
            _tbSide.IsReadOnly = true;
            foreach (var cell in _cells)
            {
                cell.Value = int.MaxValue;
                cell.Label.MouseUp += (s, e) =>
                {
                    if (_startCell == null)
                    {
                        cell.Label.Content = "S";
                        _startCell = cell;
                        _butFindAllShortest.IsEnabled = _butFindOneShortest.IsEnabled = _butFindSinglestepped.IsEnabled = true;
                    }
                    else if (_finishCell == null)
                    {
                        cell.Label.Content = "F";
                        _finishCell = cell;
                    }
                };
            }
            _butReset.IsEnabled = true;
            _tbSide.IsReadOnly = false;
        }


        private async Task RunProcess(Func<Task> task)
        {
            _lblTime.Content = null;
            _butReset.IsEnabled = _butFindAllShortest.IsEnabled = _butFindOneShortest.IsEnabled = _butFindSinglestepped.IsEnabled = false;
            _tbSide.IsReadOnly = true;
            var sw = new Stopwatch();
            sw.Start();
            await task();
            sw.Stop();
            _lblTime.Content = sw.Elapsed;
            ShowResults();
            _butReset.IsEnabled = true;
            _tbSide.IsReadOnly = false;
        }


        private void RecursiveStep(Cell cell, int steps, Action<Cell, int> act)
        {
            var y = cell.Place.Y;
            var x = cell.Place.X;
            if (y - 2 >= 0 && x - 1 >= 0 && y - 2 < _side && x - 1 < _side)
                act(_cells[y - 2, x - 1], steps + 1);
            if (y - 2 >= 0 && x + 1 >= 0 && y - 2 < _side && x + 1 < _side)
                act(_cells[y - 2, x + 1], steps + 1);
            if (y - 1 >= 0 && x + 2 >= 0 && y - 1 < _side && x + 2 < _side)
                act(_cells[y - 1, x + 2], steps + 1);
            if (y + 1 >= 0 && x + 2 >= 0 && y + 1 < _side && x + 2 < _side)
                act(_cells[y + 1, x + 2], steps + 1);
            if (y + 2 >= 0 && x + 1 >= 0 && y + 2 < _side && x + 1 < _side)
                act(_cells[y + 2, x + 1], steps + 1);
            if (y + 2 >= 0 && x - 1 >= 0 && y + 2 < _side && x - 1 < _side)
                act(_cells[y + 2, x - 1], steps + 1);
            if (y + 1 >= 0 && x - 2 >= 0 && y + 1 < _side && x - 2 < _side)
                act(_cells[y + 1, x - 2], steps + 1);
            if (y - 1 >= 0 && x - 2 >= 0 && y - 1 < _side && x - 2 < _side)
                act(_cells[y - 1, x - 2], steps + 1);
        }


        private void HandleFinish(Cell cell, int steps)
        {
            cell.Value = steps;
            var history = new List<Cell>();
            foreach (var c in _tempHistory)
                history.Add(new Cell(c));
            _histories.Add(history);
            _tempHistory.Pop();
        }


        private void ShowResults()
        {
            if (_histories.Any())
            {
                _lblResult.Content = (_histories.First().Count - 1).ToString();
                foreach (var history in _histories)
                {
                    foreach (var cell in history)
                        cell.Label.Content = cell.Value;
                }
            }
            else
                _lblResult.Content = @"No way";
        }


        private async Task RunFindAllShortest() => await FindShortest(true);


        private async Task RunFindOneShortest() => await FindShortest(false);


        private async Task FindShortest(bool lookForAll)
        {
            void RunFindShortestRecursive(Cell cell, int steps)
            {
                if (cell.Value < steps || !lookForAll && cell.Value == steps)
                    return;
                _tempHistory.Push(cell);
                if (cell == _finishCell)
                {
                    if (cell.Value != steps)
                        _histories.Clear();
                    HandleFinish(cell, steps);
                    return;
                }
                cell.Value = steps;
                RecursiveStep(cell, steps, RunFindShortestRecursive);
                _tempHistory.Pop();
            }

            if (_finishCell == null)
                _finishCell = _startCell;
            await Task.Run(() => RunFindShortestRecursive(_startCell, 0));
        }


        private async Task RunFindSinglestepped()
        {
            if (_finishCell == _startCell)
            {
                _startCell.Value = 0;
                _histories.Add(new List<Cell> { _startCell });
                return;
            }
            var lim = _side * _side - 1;
            var found = false;
            var free = _finishCell == null;

            void RunFindSinglesteppedRecursive(Cell cell, int steps)
            {
                if (found || steps > lim || cell.Value != int.MaxValue)
                    return;
                if (steps == lim && (free || cell == _finishCell))
                {
                    found = true;
                    _tempHistory.Push(cell);
                    HandleFinish(cell, steps);
                    return;
                }
                _tempHistory.Push(cell);
                cell.Value = steps;
                RecursiveStep(cell, steps, RunFindSinglesteppedRecursive);
                _tempHistory.Pop();
                cell.Value = int.MaxValue;
            }

            await Task.Run(() => RunFindSinglesteppedRecursive(_startCell, 0));
        }



    }
}
