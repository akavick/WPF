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

            if (number == 0 && !Equals(_number.Foreground, Brushes.LightGray) || threadsCount == 0 && !Equals(_threads.Foreground, Brushes.LightGray))
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

            var num = numVal ? number : DefaultNumber;
            var trs = trsVal ? threadsCount : Environment.ProcessorCount;

            (int prime, TimeSpan time) results;

            if(trs > 1)
                results = await Calculate(num, trs);
            else
                results = await Calculate(num);

            _result.Content = results.prime;
            _time.Content = results.time;
            _bar.Visibility = Visibility.Collapsed;
            _fog.Background = null;
        }



        /// <summary>
        /// Возвращает массив простых чисел-делителей
        /// </summary>
        /// <param name="searchingNumber">Номер искомого числа</param>
        /// <param name="prime">Если на этой стадии будет найдено искомое число - его значение будет задесь</param>
        /// <returns></returns>
        private int[] GetDivisorPrimes(int searchingNumber, out int? prime)
        {
            // Максимальный делитель
            var maxDivisorPrime = (int)Math.Sqrt(int.MaxValue);
            var currentDivisorPrime = 3;
            var divisorPrimes = new List<int> { 2, currentDivisorPrime };

            // Заполняем массив делителей
            var count = divisorPrimes.Count;
            while (true)
            {
                if ((currentDivisorPrime += 2) > maxDivisorPrime)
                    break;

                var i = 0;

                while (count < searchingNumber)
                {
                    var divisorPrime = divisorPrimes[++i];

                    if (currentDivisorPrime % divisorPrime == 0)
                        break;

                    if (divisorPrime * divisorPrime < currentDivisorPrime)
                        continue;

                    divisorPrimes.Add(currentDivisorPrime);
                    ++count;
                    break;
                }
            }

            if (count >= searchingNumber)
            {
                prime = divisorPrimes[searchingNumber - 1];
                return null;
            }

            prime = null;
            return divisorPrimes.ToArray();
        }



        /// <summary>
        /// Однопоточный эталон
        /// </summary>
        /// <param name="searchingNumber">Номер искомого числа</param>
        /// <returns></returns>
        private Task<(int prime, TimeSpan time)> Calculate(int searchingNumber)
        {
            return Task.Run(() =>
            {
                var sw = new Stopwatch();
                sw.Start();

                var divisorPrimes = GetDivisorPrimes(searchingNumber, out var prime);

                if (prime != null)
                {
                    sw.Stop();
                    return (prime.Value, sw.Elapsed);
                }

                var number = divisorPrimes.Length;
                var currentPrime = divisorPrimes.Last();

                while (number < searchingNumber)
                {
                    currentPrime += 2;
                    var i = 0;

                    while (true)
                    {
                        var divisorPrime = divisorPrimes[++i];

                        if (currentPrime % divisorPrime == 0)
                            break;

                        if (divisorPrime * divisorPrime < currentPrime)
                            continue;

                        number++;
                        break;
                    }
                }

                sw.Stop();

                return (currentPrime, sw.Elapsed);
            });
        }



        /// <summary>
        /// Многопоточное решение
        /// </summary>
        /// <param name="searchingNumber">Номер искомого числа</param>
        /// <param name="threadsCount">Количество потоков (не факт, что CLR даст именно столько)</param>
        /// <returns></returns>
        private Task<(int prime, TimeSpan time)> Calculate(int searchingNumber, int threadsCount)
        {
            return Task.Run(() =>
            {
                var sw = new Stopwatch();
                sw.Start();

                var divisorPrimes = GetDivisorPrimes(searchingNumber, out var prime);

                if (prime != null)
                {
                    sw.Stop();
                    return (prime.Value, sw.Elapsed);
                }

                var number = divisorPrimes.Length;
                var potentialPrime = divisorPrimes.Last();

                var array = Enumerable
                    .Range(0, threadsCount)
                    .AsParallel()
                    .SelectMany(x =>
                    {
                        var primes = new List<int>();

                        while (number < searchingNumber)
                        {
                            var pp = Interlocked.Add(ref potentialPrime, 2);
                            var i = 0;

                            while (true)
                            {
                                var divisorPrime = divisorPrimes[++i];
                                if (pp % divisorPrime == 0)
                                    break;
                                if (divisorPrime * divisorPrime < pp)
                                    continue;
                                primes.Add(pp);
                                Interlocked.Increment(ref number);
                                break;
                            }
                        }

                        return primes;
                    })
                    .ToArray();

                Array.Sort(array);

                sw.Stop();

                return (array[searchingNumber - divisorPrimes.Length - 1], sw.Elapsed);
            });
        }























    }
}