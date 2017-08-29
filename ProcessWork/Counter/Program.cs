using System;

namespace Counter
{
    internal class Program
    {
        private static long _maxCount = 10000000000;

        private static void Main()
        {
            for (long i = 0; i < _maxCount; i++)
            {
                if (i % (_maxCount / 10000) == 0)
                    Console.Write($"{i * 100.0 / _maxCount}% complete               \r");
            }
            Console.WriteLine("Done                            ");
        }
    }
}
