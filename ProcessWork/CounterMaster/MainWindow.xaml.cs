using System.Collections.ObjectModel;
using System.Windows;

namespace CounterMaster
{
    public partial class MainWindow
    {
        private readonly ObservableCollection<CounterInfo> _counters = new ObservableCollection<CounterInfo>();
        
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _butNew.Click += _butNew_Click;
            _butKill.Click += _butKill_Click;
            _butPlus.Click += _butPlus_Click;
            _butMinus.Click += _butMinus_Click;
            _dataGrid.ItemsSource = _counters;
        }

        private void _butMinus_Click(object sender, RoutedEventArgs e)
        {
            var p = (CounterInfo)_dataGrid.SelectedItem;
            p.TryDecreasePriority();
        }

        private void _butPlus_Click(object sender, RoutedEventArgs e)
        {
            var p = (CounterInfo)_dataGrid.SelectedItem;
            p.TryIncreasePriority();
        }

        private void _butNew_Click(object sender, RoutedEventArgs e)
        {
            var p = new CounterInfo();
            p.Exited += () => Dispatcher.Invoke(() => _counters.Remove(p));
            _counters.Add(p);
        }

        private void _butKill_Click(object sender, RoutedEventArgs e)
        {
            var p = (CounterInfo)_dataGrid.SelectedItem;
            _counters.Remove(p);
            p.Kill();
        }
    }
}
