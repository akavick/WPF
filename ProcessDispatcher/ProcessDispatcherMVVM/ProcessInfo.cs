using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDispatcherMVVM
{
    public class ProcessInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Threads { get; set; }
        public int Descriptors { get; set; }
        public string Priority { get; set; }
        //p.WorkingSet64,
        //p.VirtualMemorySize64,
        //p.PrivateMemorySize64,
        //p.PeakWorkingSet64,
        //p.PeakVirtualMemorySize64,
        //p.PeakPagedMemorySize64,
        //p.PagedSystemMemorySize64,
        //p.PagedMemorySize64,
        //p.NonpagedSystemMemorySize64,
    }
}
