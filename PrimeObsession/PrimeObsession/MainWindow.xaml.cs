using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PrimeObsession
{
    public partial class MainWindow
    {
        private const int DefaultNumber = 1;
        private string EnterNumber => $@"Введите номер простого числа (дефолт: {DefaultNumber}):";
        private string EnterThreads => $@"Введите число потоков (дефолт: {Environment.ProcessorCount})";

        /// <summary>
        /// Простые числа-делители
        /// </summary>
        private int[] _divisorPrimes; // int. long и даже uint всё равно никто ждать не будет =)



        public MainWindow()
        {
            InitializeComponent();
            _number.Text = EnterNumber;
            _threads.Text = EnterThreads;
            _number.GotFocus += _textBox_GotFocus;
            _threads.GotFocus += _textBox_GotFocus;
            _number.LostFocus += _textBox_LostFocus;
            _threads.LostFocus += _textBox_LostFocus;
            _number.PreviewKeyDown += _textBox_PreviewKeyDown;
            _threads.PreviewKeyDown += _textBox_PreviewKeyDown;
            _calculate.Click += _calculate_Click;
            FillDivisorPrimes();
        }



        private void FillDivisorPrimes()
        {
            // Максимальный делитель
            var maxDivisorPrime = (int)Math.Sqrt(int.MaxValue);
            var currentDivisorPrime = 3;
            var divisorPrimes = new List<int> { 2, currentDivisorPrime };

            // Заполняем массив делителей
            while (true)
            {
                if ((currentDivisorPrime += 2) > maxDivisorPrime)
                    break;

                var i = 0;
                while (true)
                {
                    var divisorPrime = divisorPrimes[++i];

                    if (currentDivisorPrime % divisorPrime == 0)
                        break;

                    if (divisorPrime * divisorPrime < currentDivisorPrime)
                        continue;

                    divisorPrimes.Add(currentDivisorPrime);
                    break;
                }
            }

            _divisorPrimes = divisorPrimes.ToArray();
        }



        private void _textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox))
                return;
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Left || e.Key == Key.Right)
                return;
            var str = new KeyConverter().ConvertToString(e.Key)?.Replace("NumPad", "");
            e.Handled = !int.TryParse(str, out var _);
        }



        private void _textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox tb) || int.TryParse(tb.Text, out var _))
                return;
            tb.Text = Equals(tb, _number) ? EnterNumber : EnterThreads;
            tb.Foreground = Brushes.LightGray;
        }



        private void _textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox tb) || int.TryParse(tb.Text, out var _))
                return;
            tb.Text = "";
            tb.Foreground = Brushes.Black;
        }



        private async void _calculate_Click(object sender, RoutedEventArgs e)
        {
            var numVal = int.TryParse(_number.Text, out var number);
            var trsVal = int.TryParse(_threads.Text, out var threadsCount);

            if (number == 0 && !Equals(_number.Foreground, Brushes.LightGray)
                || threadsCount == 0 && !Equals(_threads.Foreground, Brushes.LightGray))
            {
                if (number == 0)
                    _result.Content = "!";
                if (threadsCount == 0)
                    _time.Content = "!";
                return;
            }

            _result.Content = _time.Content = null;
            _fog.Background = Brushes.Transparent;
            _bar.Visibility = Visibility.Visible;

            var results = await Calculate(numVal ? number : DefaultNumber,
                trsVal ? threadsCount : Environment.ProcessorCount);

            _result.Content = results.prime;
            _time.Content = results.time;
            _bar.Visibility = Visibility.Collapsed;
            _fog.Background = null;
        }



        private async Task<(int prime, TimeSpan time)> Calculate(int searchingNumber, int threadsCount)
        {
            var sw = new Stopwatch();
            sw.Start();

            if (searchingNumber <= _divisorPrimes.Length)
            {
                sw.Stop();
                return (_divisorPrimes[searchingNumber - 1], sw.Elapsed);
            }

            var number = _divisorPrimes.Length;
            var currentPrime = _divisorPrimes.Last();
            var step = threadsCount * 2;
            var tasks = new List<Task<int[]>>();
            var length = (searchingNumber - number + (searchingNumber - number) % threadsCount) / threadsCount;

            for (var x = 1;x <= threadsCount;x++)
            {
                var cp = currentPrime + x * 2;
                var task = Task.Run(() =>
                {
                    //var primes = new List<int>();

                    var primes = new int[length];
                    var index = 0;

                    while (/*number < searchingNumber*/ index < length)
                    {
                        var i = 0;
                        while (true)
                        {
                            var divisorPrime = _divisorPrimes[++i];

                            if (cp % divisorPrime == 0)
                                break;

                            if (divisorPrime * divisorPrime < cp)
                                continue;

                            Interlocked.Increment(ref number);

                            primes[index++] = cp;
                            //primes.Add(cp);
                            break;
                        }
                        cp += step;
                    }

                    return primes.ToArray();
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            var array = tasks
                .SelectMany(t => t.Result)
                .OrderBy(n => n)
                .ToArray();

            sw.Stop();

            return (array[searchingNumber - _divisorPrimes.Length - 1], sw.Elapsed);
        }



        //private Task<(int prime, TimeSpan time)> Calculate(int searchingNumber, int threadsCount)
        //{
        //    return Task.Run(() =>
        //    {
        //        var sw = new Stopwatch();
        //        sw.Start();

        //        if (searchingNumber <= _divisorPrimes.Length)
        //        {
        //            sw.Stop();
        //            return (_divisorPrimes[searchingNumber - 1], sw.Elapsed);
        //        }

        //        var number = _divisorPrimes.Length;
        //        var currentPrime = _divisorPrimes.Last();
        //        //var step = threadsCount * 2;
        //        //var primes = new List<int>(_divisorPrimes);

        //        while (number < searchingNumber)
        //        {
        //            currentPrime += 2;
        //            var i = 0;
        //            while (true)
        //            {
        //                var divisorPrime = _divisorPrimes[++i];

        //                if (currentPrime % divisorPrime == 0)
        //                    break;

        //                if (divisorPrime * divisorPrime < currentPrime)
        //                    continue;

        //                number++;
        //                //primes.Add(currentPrime);
        //                break;
        //            }
        //        }

        //        sw.Stop();

        //        return (currentPrime, sw.Elapsed);
        //    });
        //}
    }
}












#region select

//var resultPrime = await Task.Run(() =>
//{
//    sw.Start();
//    if (searchingNumber == 1)
//        return 2;

//    var primes = new List<int> { 2, 3 };
//    var collection = Enumerable
//        .Range(2, searchingNumber - 2)
//        .Select(n =>
//        {
//            var prime = primes.Last();
//            while (true)
//            {
//                prime += 2;
//                for (var i = 1;;i++)
//                {
//                    var currentDivisor = primes[i];

//                    if (prime % currentDivisor == 0)
//                        break;

//                    if (currentDivisor * currentDivisor < prime)
//                        continue;

//                    primes.Add(prime);
//                    return prime;
//                }
//            }
//        });
//    var res = searchingNumber == 2 ? 3 : collection.Last();
//    sw.Stop();
//    return res;
//});

#endregion

#region aggr

//var resultPrime = await Task.Run(() =>
//{
//    sw.Start();
//    if (searchingNumber == 1)
//        return 2;
//    var res = Enumerable
//        .Range(2, searchingNumber - 2)
//        //.AsParallel()
//        .Aggregate(new List<int> { 2, 3 }, (primes, n) =>
//        {
//            var prime = primes.Last();
//            while (true)
//            {
//                prime += 2;
//                for (var i = 1;;i++)
//                {
//                    var currentDivisor = primes[i];

//                    if (prime % currentDivisor == 0)
//                        break;

//                    if (currentDivisor * currentDivisor < prime)
//                        continue;

//                    primes.Add(prime);
//                    return primes;
//                }
//            }
//        }, primes => primes.Last());
//    sw.Stop();
//    return res;
//});

#endregion