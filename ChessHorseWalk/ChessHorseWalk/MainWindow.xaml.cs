using System;
using System.Collections.Generic;
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
        private struct IntPoint
        {
            public int X;
            public int Y;

            public static bool operator ==(IntPoint left, IntPoint right) => left.X == right.X && left.Y == right.Y;
            public static bool operator !=(IntPoint left, IntPoint right) => !(left == right);
        }

        private struct Cell
        {
            public IntPoint Place;
            public int BestSteps;
            public Label Label;

            public void Reset()
            {
                Place.X = Place.Y = int.MinValue;
                BestSteps = 0;
                Label = null;
            }
        }

        private Cell _startCell;
        private Cell _finishCell;
        private Cell[,] _cells;
        private int _side;

        public MainWindow()
        {
            InitializeComponent();

            _butReset.Click += (s, e) =>
            {
                _startCell.Reset();
                _finishCell.Reset();
                _field.Clear();
                Reset();
            };

            _butStart.Click += async (s, e) => await RunProcess();

            _butCancel.Click += (s, e) =>
            {

            };

            Reset();
        }



        private void Reset()
        {
            _side = _field.Side;
            _butReset.IsEnabled = _butCancel.IsEnabled = _butStart.IsEnabled = false;
            _cells = new Cell[_side, _side];

            for (var y = 0; y < _side; y++)
            {
                for (var x = 0; x < _side; x++)
                {
                    _cells[y, x].Place.Y = y;
                    _cells[y, x].Place.X = x;
                    _cells[y, x].Label = _field.Cells[y, x];

                    var c = _cells[y, x];
                    _cells[y, x].Label.MouseUp += (s, e) =>
                    {
                        if (_startCell.Label == null)
                        {
                            _startCell = c;
                            _startCell.Label.Content = "S";
                        }
                        else if (_finishCell.Label == null)
                        {
                            _finishCell = c;
                            _finishCell.Label.Content = "F";
                        }
                    };
                }
            }

            _butReset.IsEnabled = _butCancel.IsEnabled = _butStart.IsEnabled = true;
        }






        private async Task RunProcess()
        {
            await Dispatcher.InvokeAsync(() => _butReset.IsEnabled = _butStart.IsEnabled = false);
            RunProcessRecursive(_startCell.Place, 0);
            await Dispatcher.InvokeAsync(() => _butReset.IsEnabled = _butStart.IsEnabled = true);
        }



        private void RunProcessRecursive(IntPoint startFrom, int steps)
        {
            ++steps;

            if()

            if (startFrom == _finishCell.Place)
            {
                
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
