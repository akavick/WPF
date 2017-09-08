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
    public partial class ChessField
    {
        private const int Side = 8;
        private Label _current = null;
        private readonly Label[,] _cells = null;


        public ChessField()
        {
            InitializeComponent();
            _cells = new Label[Side, Side];
            FillField();
        }


        public void Clear()
        {
            if (_current != null)
            {
                _current.Content = null;
                _current.Foreground = Brushes.Black;
                _current = null;
            }
            foreach (var cell in _cells)
            {
                cell.IsEnabled = true;
                cell.Content = null;
                cell.BorderBrush = Brushes.Black;
                cell.Opacity = 1.0;
            }
        }


        private void FillField()
        {
            for (var i = 0;i < Side;i++)
            {
                _field.ColumnDefinitions.Add(new ColumnDefinition());
                _field.RowDefinitions.Add(new RowDefinition());
            }
            FillCells();
        }


        private void FillCells()
        {
            for (var i = 0;i < Side;i++)
            {
                var isWhite = i % 2 == 0;
                for (var j = 0;j < Side;j++)
                {
                    var cell = CreateLabel(isWhite);
                    _cells[i, j] = cell;
                    cell.SetValue(Grid.ColumnProperty, j);
                    cell.SetValue(Grid.RowProperty, i);
                    var y = i;
                    var x = j;

                    void OnCellOnMouseUp(object s, MouseButtonEventArgs e)
                    {
                        cell.Content = (int?) cell.Content + 1 ?? 1;
                        cell.Foreground = Brushes.Red;
                        if (_current != null)
                            _current.Foreground = Brushes.Black;
                        _current = cell;
                        HandlePosition(y, x);
                    }

                    cell.MouseUp += OnCellOnMouseUp;
                    _field.Children.Add(cell);
                    isWhite = !isWhite;
                }
            }
        }


        private Label CreateLabel(bool isWhite)
        {
            var cell = new Label
            {
                FontSize = 40,
                FontWeight = FontWeights.Bold,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2),
                Background = isWhite ? Brushes.White : Brushes.Gray,
                ClipToBounds = true,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = new Thickness(0),
                Margin = new Thickness(0)
            };
            return cell;
        }


        private void HandlePosition(int i, int j)
        {
            var possiblePositions = new[]
            {
                (y:i - 2, x:j - 1),
                (y:i - 2, x:j + 1),
                (y:i - 1, x:j + 2),
                (y:i + 1, x:j + 2),
                (y:i + 2, x:j + 1),
                (y:i + 2, x:j - 1),
                (y:i + 1, x:j - 2),
                (y:i - 1, x:j - 2)
            }
            .Where(pp => pp.x > -1 && pp.x < Side && pp.y > -1 && pp.y < Side)
            .ToArray();

            foreach (var cell in _cells)
            {
                cell.IsEnabled = false;
                cell.BorderBrush = Brushes.Black;
                cell.Opacity = 0.8;
            }

            foreach (var pp in possiblePositions)
            {
                var cell = _cells[pp.y, pp.x];
                cell.IsEnabled = true;
                cell.BorderBrush = Brushes.LimeGreen;
                cell.Opacity = 1.0;
            }

            _current.BorderBrush = Brushes.Red;
            _current.Opacity = 1.0;
        }

    }
}
