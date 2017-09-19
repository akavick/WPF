using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ProcessDispatcherMVVM
{
    public class MainViewModel : ViewModelBase
    {
        private ICommand _refresh;
        private ObservableCollection<ProcessInfo> _processInfos;


        public ICommand Refresh => _refresh ?? (_refresh = new RelayCommand(Renew));

        public ObservableCollection<ProcessInfo> ProcessInfos
        {
            get => _processInfos;
            private set
            {
                _processInfos = value;
                RaisePropertyChanged(() => ProcessInfos);
            }
        }


        public MainViewModel()
        {
            Renew();
        }


        private void Renew()
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

                return new ProcessInfo
                {
                    Id = p.Id,
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

            ProcessInfos = new ObservableCollection<ProcessInfo>(processes);
        }



    }
}
