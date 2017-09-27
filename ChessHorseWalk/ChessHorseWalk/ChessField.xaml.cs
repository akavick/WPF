using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ChessHorseWalk
{
    public partial class ChessField
    {
        public const int ClassicSideSize = 8;
        private int _side;
        private Cell[,] _cells;
        public int Side => _side;
        public Cell[,] Cells => _cells;



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
            _cells = new Cell[_side, _side];
            FillField();
        }



        public void Clear() => FillNewField(_side);


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
            for (var y = 0; y < _side; y++)
            {
                var isWhite = y % 2 == 0;
                for (var x = 0; x < _side; x++)
                {
                    var cell = _cells[y, x] = new Cell
                    {
                        Place = new IntPoint
                        {
                            Y = y,
                            X = x
                        },
                        Label = new Label
                        {
                            Background = isWhite ? Brushes.White : Brushes.Gray,
                            Style = FindResource("_cellStyle") as Style
                        }
                    };
                    cell.Label.SetValue(Grid.ColumnProperty, x);
                    cell.Label.SetValue(Grid.RowProperty, y);
                    _field.Children.Add(cell.Label);
                    isWhite = !isWhite;
                }
            }
        }





    }
}
