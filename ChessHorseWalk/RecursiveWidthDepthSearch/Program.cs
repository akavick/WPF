using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RecursiveWidthDepthSearch
{
    class Program
    {
        private static void Main()
        {
            try
            {
                new Program().Run().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        private async Task Run()
        {
            const int side = 5;
            var found = false;
            var locker = new object();
            var rand = new Random();
            var range = Enumerable.Range(0, side).ToArray();

            var sw = new Stopwatch();
            sw.Start();

            var cells = new int[side, side];
            var start = (y:0, x:0);
            cells[start.y, start.x] = int.MaxValue;
            var finish = (y:4, x:4);


            void PathFind((int y, int x) point, int shortestRun)
            {
                if (cells[point.y, point.x] > shortestRun)
                    return;
                cells[point.y, point.x] = shortestRun;

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
                .Where(pp => pp.x > -1 && pp.x < side && pp.y > -1 && pp.y < side)
                .ToArray();
            }

            PathFind(start, 0);




            Console.WriteLine(sw.Elapsed);
        }










        //private async Task Run()
        //{
        //    const int side = 7;
        //    var found = false;
        //    var locker = new object();
        //    var rand = new Random();
        //    var range = Enumerable.Range(0, side).ToArray();

        //    var sw = new Stopwatch();
        //    sw.Start();

        //    var tasks = range
        //        .SelectMany(i => range.Select(j => (i, j)))
        //        .OrderBy(t => rand.Next())
        //        .Take(Environment.ProcessorCount)
        //        .Select(async p => await Task.Run(() =>
        //        {
        //            Console.WriteLine($"{p} {Task.CurrentId} {Thread.CurrentThread.ManagedThreadId} started{Environment.NewLine}");

        //            var cells = new bool[side, side];
        //            var history = new Stack<(int y, int x)>();
        //            var pathFound = false;

        //            void PathFind((int y, int x) point)
        //            {
        //                if (found)
        //                    return;

        //                cells[point.y, point.x] = true;
        //                history.Push(point);

        //                var possiblePositions = new[]
        //                {
        //                    (y:point.y - 2, x:point.x - 1),
        //                    (y:point.y - 2, x:point.x + 1),
        //                    (y:point.y - 1, x:point.x + 2),
        //                    (y:point.y + 1, x:point.x + 2),
        //                    (y:point.y + 2, x:point.x + 1),
        //                    (y:point.y + 2, x:point.x - 1),
        //                    (y:point.y + 1, x:point.x - 2),
        //                    (y:point.y - 1, x:point.x - 2)
        //                }
        //                .Where(pp => pp.x > -1 && pp.x < side && pp.y > -1 && pp.y < side && !cells[pp.y, pp.x])
        //                .ToArray();

        //                foreach (var pp in possiblePositions)
        //                    PathFind(pp);

        //                if (history.Count == cells.Length)
        //                {
        //                    lock (locker)
        //                        if (!found)
        //                            pathFound = found = true;
        //                    return;
        //                }

        //                cells[point.y, point.x] = false;
        //                history.Pop();
        //            }

        //            PathFind(p);

        //            Console.WriteLine($"{p} {Task.CurrentId} {Thread.CurrentThread.ManagedThreadId} finished{Environment.NewLine}");

        //            if (!pathFound)
        //                return;

        //            lock (locker)
        //            {
        //                foreach (var point in history.Reverse())
        //                    Console.WriteLine(point);
        //                Console.WriteLine();
        //            }
        //        }))
        //        .ToList();

        //    await Task.WhenAll(tasks);

        //    if (!found)
        //        Console.WriteLine("Path not Found");

        //    Console.WriteLine(sw.Elapsed);
        //}










    }
}
