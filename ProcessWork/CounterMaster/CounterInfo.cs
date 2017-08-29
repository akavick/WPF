using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CounterMaster
{
    internal class CounterInfo : INotifyPropertyChanged
    {
        private static readonly List<ProcessPriorityClass> Priorities = new List<ProcessPriorityClass>
        {
            ProcessPriorityClass.Idle,
            ProcessPriorityClass.BelowNormal,
            ProcessPriorityClass.Normal,
            ProcessPriorityClass.AboveNormal,
            ProcessPriorityClass.High,
            ProcessPriorityClass.RealTime
        };



        public event Action Exited;
        public event PropertyChangedEventHandler PropertyChanged;
        private Process _process;


        public int Pid => _process.Id;
        public ProcessPriorityClass Priority
        {
            get => _process.PriorityClass;
            set
            {
                if (_process.PriorityClass == value)
                    return;
                _process.PriorityClass = value;
                NotifyPropertyChanged();
            }
        }


        public CounterInfo()
        {
            _process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = { FileName = @"..\..\..\Counter\bin\Debug\Counter.exe" }
            };
            _process.Exited += (s, e) => Exited?.Invoke();
            _process.Start();
        }





        public bool TryIncreasePriority()
        {
            if (Priority == ProcessPriorityClass.RealTime)
                return false;
            var index = Priorities.IndexOf(Priority) + 1;
            Priority = Priorities[index];
            return true;
        }

        public bool TryDecreasePriority()
        {
            if (Priority == ProcessPriorityClass.Idle)
                return false;
            var index = Priorities.IndexOf(Priority) - 1;
            Priority = Priorities[index];
            return true;
        }

        public void Kill()
        {
            _process.Close();
            _process.Kill();
            _process = null;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
