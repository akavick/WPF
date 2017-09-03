using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestWpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Button _button = new Button {Content = "Go"};
        private readonly TextBlock _results = new TextBlock();

        public MainWindow()
        {
            InitializeComponent();
            var panel = new StackPanel();
            panel.Children.Add(_button);
            panel.Children.Add(_results);
            Content = panel;
            _button.Click += (sender, args) => Go();
        }

        private async void Go()
        {
            _button.IsEnabled = false;
            var urls = "www.albahari.com www.oreilly.com www.linqpad.net".Split();
            var totalLength = 0;
            try
            {
                foreach (var url in urls)
                {
                    var uri = new Uri("http://" + url);
                    var data = await new WebClient().DownloadDataTaskAsync(uri);
                    _results.Text += $"Length of {url} is {data.Length}{Environment.NewLine}";
                    totalLength += data.Length;
                }
                _results.Text += $"Total length: {totalLength}{Environment.NewLine}";
            }
            catch (WebException ex)
            {
                _results.Text += "Error: " + ex.Message;
            }
            finally
            {
                _button.IsEnabled = true;
            }

        }
    }
}