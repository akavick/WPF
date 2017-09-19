using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessDispatcher
{
    public partial class MainView
    {
        private static ObservableCollection<T> GetAnonymousObservableCollection<T>(IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }

        public MainView()
        {
            InitializeComponent();
            _refresh.Click += async (s, e) => await Refresh();
            Loaded += async (s, e) => await Refresh();
        }

        private async Task Refresh()
        {
            _refresh.IsEnabled = false;
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
                        ID = p.Id,
                        Name = p.ProcessName,
                        Threads = p.Threads.Count,
                        Descriptors = p.HandleCount,
                        Priority = priority,
                        WorkingSet = p.WorkingSet64,
                        VirtualMemorySize = p.VirtualMemorySize64,
                        PrivateMemorySize = p.PrivateMemorySize64,
                        PeakWorkingSet = p.PeakWorkingSet64,
                        PeakVirtualMemorySize = p.PeakVirtualMemorySize64,
                        PeakPagedMemorySize = p.PeakPagedMemorySize64,
                        PagedSystemMemorySize = p.PagedSystemMemorySize64,
                        PagedMemorySize = p.PagedMemorySize64,
                        NonpagedSystemMemorySize = p.NonpagedSystemMemorySize64
                    };
                });

                return GetAnonymousObservableCollection(processes);
            });

            _dataGrid.ItemsSource = collection;
            _refresh.IsEnabled = true;
        }

    }
}
