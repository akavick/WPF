using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChessHorseWalk
{
    public partial class ChessField
    {
        private const int ClassicSideSize = 8;
        private int _side;
        private Label[,] _cells;
        public int Side => _side;
        public Label[,] Cells => _cells;



        public ChessField() : this(ClassicSideSize) { }

        public ChessField(int side)
        {
            InitializeComponent();
            FillNewField(side);
        }




        public void FillNewField(int side = ClassicSideSize)
        {
            _field = new Grid();
            _chessField.Content = _field;
            _side = side;
            _cells = new Label[_side, _side];
            FillField();
        }



        public void Clear()
        {
            FillNewField(_side);
        }



        private void FillField()
        {
            for (var i = 0; i < _side; i++)
            {
                _field.ColumnDefinitions.Add(new ColumnDefinition());
                _field.RowDefinitions.Add(new RowDefinition());
            }
            FillCells();
        }



        private void FillCells()
        {
            for (var i = 0; i < _side; i++)
            {
                var isWhite = i % 2 == 0;
                for (var j = 0; j < _side; j++)
                {
                    var cell = CreateLabel(isWhite);
                    _cells[i, j] = cell;
                    cell.SetValue(Grid.ColumnProperty, j);
                    cell.SetValue(Grid.RowProperty, i);
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


    }
}
