using System.Windows.Controls;

namespace ChessHorseWalk
{
    public class Cell
    {
        public IntPoint Place;
        public int Value;
        public Label Label;

        public Cell(){}

        public Cell(Cell other)
        {
            Place = new IntPoint
            {
                X = other.Place.X,
                Y = other.Place.Y
            };
            Value = other.Value;
            Label = other.Label;
        }

        public void Reset()
        {
            Place = new IntPoint();
            Value = 0;
            Label = null;
        }
    }
}
