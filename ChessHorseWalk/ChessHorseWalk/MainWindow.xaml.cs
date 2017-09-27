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

            _butStart.Click += async (s, e) => await RunProcess();

            Reset();
        }



        private void Reset()
        {
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





        private int minLen = int.MaxValue;
        private async Task RunProcess()
        {
            minLen = int.MaxValue;

            _lblTime.Content = null;
            _butReset.IsEnabled = _butStart.IsEnabled = false;

            List<Stack<Label>> histories = new List<Stack<Label>>();
            Stack<Label> history = new Stack<Label>();

            var sw = new Stopwatch();
            sw.Start();
            await Task.Run(() => RunProcessRecursive(_startCell, 0));
            sw.Stop();

            _tbSide.Text = minLen.ToString();

            _lblTime.Content = sw.Elapsed;
            _butReset.IsEnabled = _butStart.IsEnabled = true;

        }


        
        private void RunProcessRecursive(Cell cell, int steps)
        {
            if (cell.Value <= steps)
                return;

            cell.Value = steps;

            if (cell == _finishCell)
            {
                if (steps < minLen)
                    minLen = steps;
                //todo history
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

        }











        //void OnCellOnMouseUp(object s, MouseButtonEventArgs e)
        //{
        //    cell.Content = (int?)cell.Content + 1 ?? 1;
        //    cell.Foreground = Brushes.Red;
        //    if (_current != null)
        //        _current.Foreground = Brushes.Black;
        //    _current = cell;
        //    //HandlePosition(y, x);
        //}

        //cell.MouseUp += OnCellOnMouseUp;


        //http://impotencija.net/prostatit/tabletki-ot-prostatita/

        //private void HandlePosition(int i, int j)
        //{
        //    var possiblePositions = new[]
        //    {
        //        (y:i - 2, x:j - 1),
        //        (y:i - 2, x:j + 1),
        //        (y:i - 1, x:j + 2),
        //        (y:i + 1, x:j + 2),
        //        (y:i + 2, x:j + 1),
        //        (y:i + 2, x:j - 1),
        //        (y:i + 1, x:j - 2),
        //        (y:i - 1, x:j - 2)
        //    }
        //    .Where(pp => pp.x > -1 && pp.x < Side && pp.y > -1 && pp.y < Side)
        //    .ToArray();

        //    foreach (var cell in _cells)
        //    {
        //        cell.IsEnabled = false;
        //        cell.BorderBrush = Brushes.Black;
        //        cell.Opacity = 0.8;
        //    }

        //    foreach (var pp in possiblePositions)
        //    {
        //        var cell = _cells[pp.y, pp.x];
        //        cell.IsEnabled = true;
        //        cell.BorderBrush = Brushes.LimeGreen;
        //        cell.Opacity = 1.0;
        //    }

        //    _current.BorderBrush = Brushes.Red;
        //    _current.Opacity = 1.0;
        //}
    }
}
