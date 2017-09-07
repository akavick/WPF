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
        private Button _current = null;
        private readonly Button[,] _cells = null;


        public ChessField()
        {
            InitializeComponent();
            _cells = new Button[Side, Side];
            FillField();
        }


        public void Clear()
        {
            _current.Content = null;
            _current.Foreground = Brushes.Black;
            _current = null;
            foreach (var b in _cells)
            {
                b.IsEnabled = true;
                b.Content = null;
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
                    var b = new Button
                    {
                        FontSize = 50,
                        BorderBrush = Brushes.Black,
                        Background = isWhite ? Brushes.White : Brushes.Gray
                    };
                    _cells[i, j] = b;
                    b.SetValue(Grid.ColumnProperty, j);
                    b.SetValue(Grid.RowProperty, i);
                    var y = i;
                    var x = j;
                    b.Click += (s, e) =>
                    {
                        b.Content = (int?)b.Content + 1 ?? 1;
                        b.Foreground = Brushes.Red;
                        if (_current != null)
                            _current.Foreground = Brushes.Black;
                        _current = b;
                        HandlePosition(y, x);
                    };
                    _field.Children.Add(b);
                    isWhite = !isWhite;
                }
            }
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

            foreach (var button in _cells)
                button.IsEnabled = false;
            foreach (var pp in possiblePositions)
                _cells[pp.y, pp.x].IsEnabled = true;
        }

    }
}
