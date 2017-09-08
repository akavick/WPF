using System;
using System.Collections.Generic;
using System.Linq;

namespace RecursiveWidthDepthSearch
{



    class Program
    {
        private static void Main() => new Program().Run2();

        private const int Side = 8;

        private void Run2()
        {
            var cells = new bool[Side, Side];
            var history = new Stack<(int y, int x)>();
            var startPoint = (3, 5);

            void PathFind((int y, int x) point)
            {
                cells[point.y, point.x] = true;
                history.Push(point);

                if (history.Count == cells.Length)
                    return;

                var possiblePositions = new[]
                {
                    (y:point.y - 2, x:point.x - 1),
                    (y:point.y - 2, x:point.x + 1),
                    (y:point.y - 1, x:point.x + 2),
                    (y:point.y + 1, x:point.x + 2),
                    (y:point.y + 2, x:point.x + 1),
                    (y:point.y + 2, x:point.x - 1),
                    (y:point.y + 1, x:point.x - 2),
                    (y:point.y - 1, x:point.x - 2)
                }
                .Where(pp => pp.x > -1 && pp.x < Side && pp.y > -1 && pp.y < Side && !cells[pp.y, pp.x])
                .ToArray();

                foreach (var pp in possiblePositions)
                    PathFind(pp);

                if (history.Count == cells.Length)
                    return;

                cells[point.y, point.x] = false;
                history.Pop();
            }

            PathFind(startPoint);

            if(history.Count == 0)
                Console.WriteLine("Path not found");

            foreach (var point in history.Reverse())
                Console.WriteLine(point);
        }











        class Cell
        {
            public readonly int Y;
            public readonly int X;
            public int Steps = 0;

            public Cell(int y, int x)
            {
                X = x;
                Y = y;
            }
        }

        private void Run1()
        {
            var board = new Cell[Side, Side];
            var history = new Stack<Cell>();

            for (var y = 0;y < Side;y++)
            {
                for (var x = 0;x < Side;x++)
                {
                    board[y, x] = new Cell(y, x);
                }
            }

            var current = board[0, 0];



        }

    }
}
