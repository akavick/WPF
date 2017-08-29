using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ProcessDispatcherMVVM
{
    public class MainViewModel : ViewModelBase
    {
        private ICommand _refresh;
        private ObservableCollection<ProcessInfo> _processInfos;
        private readonly DispatcherTimer _timer;


        public ICommand Refresh => _refresh ?? (_refresh = new RelayCommand(Renew));

        public ObservableCollection<ProcessInfo> ProcessInfos
        {
            get => _processInfos;
            private set
            {
                _processInfos = value;
                RaisePropertyChanged(/*() => ProcessInfos*/);
            }
        }


        public MainViewModel()
        {
            Renew();
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(3000) };
            _timer.Tick += (s, e) => Renew();
            _timer.Start();
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
                    Priority = priority
                };
            });

            ProcessInfos = new ObservableCollection<ProcessInfo>(processes);
        }



    }
}
