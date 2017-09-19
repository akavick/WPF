namespace ProcessDispatcherMVVM
{
    public class ProcessInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Threads { get; set; }
        public int Descriptors { get; set; }
        public string Priority { get; set; }
        public long WorkingSet { get; set; }
        public long VirtualMemorySize { get; set; }
        public long PrivateMemorySize { get; set; }
        public long PeakWorkingSet { get; set; }
        public long PeakVirtualMemorySize { get; set; }
        public long PeakPagedMemorySize { get; set; }
        public long PagedSystemMemorySize { get; set; }
        public long PagedMemorySize { get; set; }
        public long NonpagedSystemMemorySize { get; set; }
    }
}
