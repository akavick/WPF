using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ProcessDispatcher
{
    public partial class MainView
    {
        private DispatcherTimer _timer;

        private static ObservableCollection<T> GetAnonymousObservableCollection<T>(IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }

        public MainView()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(3000) };
            _timer.Tick += async (s, ea) => await Refresh();
            await Refresh();
        }


        private async Task Refresh()
        {
            _timer.Stop();

            var collection = await Task.Run(() =>
            {
                var processes = Process.GetProcesses().Select(p =>
                {
                    string priority;

                    try
                    {
                        priority = p.PriorityClass.ToString();
                    }
                    catch
                    {
                        priority = "Access denied";
                    }

                    return new
                    {
                        p.Id,
                        Name = p.ProcessName,
                        Threads = p.Threads.Count,
                        Descriptors = p.HandleCount,
                        Priority = priority,
                        //p.WorkingSet64,
                        //p.VirtualMemorySize64,
                        //p.PrivateMemorySize64,
                        //p.PeakWorkingSet64,
                        //p.PeakVirtualMemorySize64,
                        //p.PeakPagedMemorySize64,
                        //p.PagedSystemMemorySize64,
                        //p.PagedMemorySize64,
                        //p.NonpagedSystemMemorySize64,
                    };
                });

                return GetAnonymousObservableCollection(processes);
            });

            _dataGrid.ItemsSource = collection;
            _timer.Start();
        }

    }
}
