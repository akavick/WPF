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
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

namespace TestWpfApp
{
    public partial class MainWindow
    {
        private BusyWindow _busyWindow;

        public MainWindow()
        {
            InitializeComponent();
            _but.Click += _but_Click;
            Closed += (s, e) => Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
        }



        #region TestRealization

        //private void _but_Click(object sender, RoutedEventArgs e)
        //{
        //    ShowBusy();
        //    Thread.Sleep(3000);
        //    HideBusy();
        //}

        //private void ShowBusy()
        //{
        //    if (_busyWindow != null)
        //        return;

        //    var p = new Point { X = Left + Width / 2, Y = Top + Height / 2 };

        //    var newWindowThread = new Thread(() =>
        //    {
        //        _busyWindow = new BusyWindow
        //        {
        //            Left = p.X,
        //            Top = p.Y
        //        };
        //        _busyWindow.Show();

        //        Dispatcher.Run();
        //    })
        //    { IsBackground = true };

        //    newWindowThread.SetApartmentState(ApartmentState.STA);
        //    newWindowThread.Start();
        //}

        //private void HideBusy()
        //{
        //    _busyWindow?.Dispatcher.BeginInvoke((Action)(() =>
        //    {
        //        _busyWindow.Close();
        //        _busyWindow = null;
        //    }));
        //}

        #endregion




        #region MyRealization

        private async void _but_Click(object sender, RoutedEventArgs e)
        {
            //var sc = SynchronizationContext.Current;
            //var num = int.Parse(_text.Text) + 1;

            //Task.Run(() =>
            //{
            //    sc?.Post(x =>
            //    {
            //        _but.Content = !bool.Parse(_but.Content.ToString());
            //        _text.Text = x.ToString();
            //    }, num);
            //});

            //Application.Current.Dispatcher.UnhandledException;

            await ShowBusy();
            Thread.Sleep(2000);
            HideBusy();
        }



        private async Task ShowBusy()
        {
            var p = new Point { X = Left + Width / 2, Y = Top + Height / 2 };
            var tcs = new TaskCompletionSource<object>();

            Task.Run(async () =>
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    _busyWindow = new BusyWindow
                    {
                        Left = p.X,
                        Top = p.Y
                    };
                    _busyWindow.Show();
                });

                tcs.SetResult(null);
            });

            await tcs.Task;

            //Dispatcher.Run();

            //SynchronizationContext.SetSynchronizationContext(
            //    new DispatcherSynchronizationContext(
            //        Dispatcher.CurrentDispatcher));

            //_busyWindow.Closed += (s, e) =>
            //{
            //    _busyWindow.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
            //};
        }

        private void HideBusy()
        {
            _busyWindow.Close();
            _busyWindow = null;
        }

        #endregion



        #region MyRealization2

        //private async void _but_Click(object sender, RoutedEventArgs e)
        //{
        //    await ShowBusy();
        //    Thread.Sleep(3000);
        //    HideBusy();
        //}

        //private async Task ShowBusy()
        //{
        //    var p = new Point { X = Left + Width / 2, Y = Top + Height / 2 };

        //    await Dispatcher.InvokeAsync(() =>
        //    {
        //        _busyWindow = new BusyWindow
        //        {
        //            Left = p.X,
        //            Top = p.Y
        //        };
        //        _busyWindow.Show();

        //        Dispatcher.Run();
        //    });
        //}

        //private void HideBusy()
        //{
        //    _busyWindow.Close();
        //    _busyWindow = null;
        //}

        #endregion




        #region WorkingRealization

        //private void _but_Click(object sender, RoutedEventArgs e)
        //{
        //    ShowBusy();
        //    Thread.Sleep(3000);
        //    HideBusy();
        //}

        //private readonly object _busyWindowSync = new object();

        //private void ShowBusy()
        //{
        //    lock (_busyWindowSync)
        //    {
        //        if (_busyWindow == null)
        //        {
        //            var newWindowThread = new Thread(AnimationThreadStartingPoint) { IsBackground = true };
        //            newWindowThread.SetApartmentState(ApartmentState.STA);
        //            newWindowThread.Start(new Point { X = Left + Width / 2, Y = Top + Height / 2 });
        //        }
        //    }
        //}

        //private void AnimationThreadStartingPoint(object position)
        //{
        //    lock (_busyWindowSync)
        //    {
        //        if (_busyWindow == null)
        //        {
        //            _busyWindow = new BusyWindow
        //            {
        //                Left = ((Point)position).X,
        //                Top = ((Point)position).Y
        //            };
        //            _busyWindow.Show();
        //        }
        //    }
        //    Dispatcher.Run();
        //}

        //private void HideBusy()
        //{
        //    lock (_busyWindowSync)
        //    {
        //        _busyWindow?.Dispatcher.BeginInvoke((Action)(() =>
        //        {
        //            _busyWindow.Close();
        //            _busyWindow = null;
        //        }));
        //    }
        //}

        #endregion



    }
}
