using System;
using System.Collections;
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
        #region Поля, Константы, Свойства

        private const int DefaultNumber = 1;
        private string EnterNumber => $@"Введите номер простого числа (дефолт: {DefaultNumber}):";
        private string EnterThreads => $@"Введите число потоков (дефолт: {Environment.ProcessorCount})";

        #endregion


        #region Конструктор

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
            _calculateD.Click += CalculateClick;
            _calculateE.Click += CalculateClick;
        }

        #endregion


        #region Обработчики событий

        private void _textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox))
                return;
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Left || e.Key == Key.Right)
                return;
            var str = new KeyConverter().ConvertToString(e.Key)?.Replace("NumPad", "");
            e.Handled = !int.TryParse(str, out _);
        }



        private void _textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox tb) || int.TryParse(tb.Text, out _))
                return;
            tb.Text = Equals(tb, _number) ? EnterNumber : EnterThreads;
            tb.Foreground = Brushes.LightGray;
        }



        private void _textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox tb) || int.TryParse(tb.Text, out _))
                return;
            tb.Text = "";
            tb.Foreground = Brushes.Black;
        }



        private async void CalculateClick(object sender, RoutedEventArgs e)
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

            if (Equals(sender as Button, _calculateD))
            {
                if (trs > 1)
                    results = await CalculateD(num, trs);
                else
                    results = await CalculateD(num);
            }
            else
            {
                if (trs > 1)
                    results = await CalculateE(num, trs);
                else
                    results = await CalculateE(num);
            }

            _result.Content = results.prime;
            _time.Content = results.time;
            _bar.Visibility = Visibility.Collapsed;
            _fog.Background = null;
        }

        #endregion


        #region Решето Эратосфена

        /// <summary>
        /// Однопоточный эталон
        /// </summary>
        /// <param name="sNum">Номер искомого числа</param>
        /// <returns></returns>
        //private Task<(int prime, TimeSpan time)> CalculateE(int sNum)
        //{
        //    return Task.Run(() =>
        //    {
        //        var sw = new Stopwatch();
        //        sw.Start();

        //        var size = (int)(sNum * Math.Log(sNum) + sNum * Math.Log(Math.Log(sNum)));
        //        if (size < 12)
        //            size = 12;

        //        #region woOptimization

        //        //var bools = new BitArray(size);
        //        ////var bools = new bool[size];

        //        //for (var i = 2; i < size; i++)
        //        //    bools[i] = true;

        //        //for (var number = 2; number * number < size; ++number)
        //        //{
        //        //    if (!bools[number])
        //        //        continue;
        //        //    for (var j = number * number; j < size; j += number)
        //        //        bools[j] = false;
        //        //}

        //        //var prime = 0;

        //        //for (int number = 2, counter = 1; number < size; number++)
        //        //{
        //        //    if (bools[number] && counter++ == sNum)
        //        //    {
        //        //        prime = number;
        //        //        break;
        //        //    }
        //        //}

        //        #endregion


        //        #region Optimization

        //        var bools = new BitArray(size);
        //        //var bools = new bool[size];

        //        for (var i = 1; i < size; i++)
        //            bools[i] = true;


        //        #region while

        //        //var number = 1;
        //        //while (true)
        //        //{
        //        //    var step = 2 * number + 1;
        //        //    if (step * step > 2 * size)
        //        //        break;
        //        //    // если 2k+1 - простое (т. е. k не вычеркнуто)
        //        //    if (!bools[number])
        //        //    {
        //        //        ++number;
        //        //        continue;
        //        //    }
        //        //    // то вычеркнем кратные 2k+1
        //        //    for (var j = 3 * number + 1/*number * number*/; j < size; j += step)
        //        //        bools[j] = false;
        //        //    ++number;
        //        //}// теперь S[k]=1 тогда и только тогда, когда 2k+1 - простое

        //        #endregion

        //        #region for

        //        for (var number = 1; (2 * number + 1) * (2 * number + 1) < 2 * size + 1; ++number)
        //        {
        //            if (!bools[number])
        //                continue;
        //            for (var j = 3 * number + 1/*number * number*/; j < size; j += 2 * number + 1)
        //                bools[j] = false;
        //        }

        //        #endregion

        //        var prime = 2;


        //        for (int num = 1, counter = 1; num < size; num++)
        //        {
        //            if (bools[num] && counter++ == sNum - 1)
        //            {
        //                prime = (2 * num + 1);
        //                break;
        //            }
        //        }

        //        #endregion


        //        sw.Stop();

        //        return (prime, sw.Elapsed);
        //    });
        //}



        private Task<(int prime, TimeSpan time)> CalculateE(int sNum)
        {
            return Task.Run(() =>
            {
                var sw = new Stopwatch();
                sw.Start();

                var size = (int)(sNum * Math.Log(sNum) + sNum * Math.Log(Math.Log(sNum)));
                if (size < 12)
                    size = 12;








                #region Optimization

                var numbers = new HashSet<int>();

                var bools = new BitArray(size);
                //var bools = new bool[size];

                for (var i = 1; i < size; i++)
                    bools[i] = true;


                #region while

                //var number = 1;
                //while (true)
                //{
                //    var step = 2 * number + 1;
                //    if (step * step > 2 * size)
                //        break;
                //    // если 2k+1 - простое (т. е. k не вычеркнуто)
                //    if (!bools[number])
                //    {
                //        ++number;
                //        continue;
                //    }
                //    // то вычеркнем кратные 2k+1
                //    for (var j = 3 * number + 1/*number * number*/; j < size; j += step)
                //        bools[j] = false;
                //    ++number;
                //}// теперь S[k]=1 тогда и только тогда, когда 2k+1 - простое

                #endregion

                #region for

                for (var number = 1; (2 * number + 1) * (2 * number + 1) < 2 * size + 1; ++number)
                {
                    if (!bools[number])
                        continue;
                    for (var j = 3 * number + 1/*number * number*/; j < size; j += 2 * number + 1)
                        bools[j] = false;
                }

                #endregion

                var prime = 2;


                for (int num = 1, counter = 1; num < size; num++)
                {
                    if (bools[num] && counter++ == sNum - 1)
                    {
                        prime = (2 * num + 1);
                        break;
                    }
                }

                #endregion


                sw.Stop();

                return (prime, sw.Elapsed);
            });
        }


        /// <summary>
        /// Многопоточное решение
        /// </summary>
        /// <param name="sNum">Номер искомого числа</param>
        /// <param name="threadsCount">Количество потоков</param>
        /// <returns></returns>
        private Task<(int prime, TimeSpan time)> CalculateE(int sNum, int threadsCount)
        {
            return CalculateE(sNum);

            //return Task.Run(() =>
            //{
            //    var sw = new Stopwatch();
            //    sw.Start();

            //    var array = Enumerable
            //        .Range(0, threadsCount)
            //        .AsParallel()
            //        .SelectMany(x =>
            //        {
            //            return new int[1];
            //        })
            //        .ToArray();

            //    Array.Sort(array);

            //    sw.Stop();

            //    return (array[0], sw.Elapsed);
            //});
        }

        #endregion


        #region Перебором делителей

        /// <summary>
        /// Возвращает массив простых чисел-делителей
        /// </summary>
        /// <param name="searchingNumber">Номер искомого числа</param>
        /// <param name="prime">Если на этой стадии будет найдено искомое число - его значение будет задесь</param>
        /// <returns></returns>
        private int[] GetDivisorPrimesD(int searchingNumber, out int? prime)
        {
            // Максимальный делитель
            var maxDivisorPrime = (int)Math.Sqrt(int.MaxValue);
            var currentDivisorPrime = 3;
            var divisorPrimes = new List<int> { 2, currentDivisorPrime };

            // Заполняем массив делителей
            var count = divisorPrimes.Count;
            while ((currentDivisorPrime += 2) < maxDivisorPrime)
            {
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
        private Task<(int prime, TimeSpan time)> CalculateD(int searchingNumber)
        {
            return Task.Run(() =>
            {
                var sw = new Stopwatch();
                sw.Start();

                var divisorPrimes = GetDivisorPrimesD(searchingNumber, out var prime);

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
        /// <param name="threadsCount">Количество потоков</param>
        /// <returns></returns>
        private Task<(int prime, TimeSpan time)> CalculateD(int searchingNumber, int threadsCount)
        {
            return Task.Run(() =>
            {
                var sw = new Stopwatch();
                sw.Start();

                var divisorPrimes = GetDivisorPrimesD(searchingNumber, out var prime);

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

        #endregion

    }
}