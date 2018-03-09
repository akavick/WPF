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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NormalD
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Title = @"Normal";

            var width = _grid.ActualWidth;
            var heigth = _grid.ActualHeight;
            const int points = 1000000;
            const int throws = 100;
            const double threshold = 0.5;
            const double elR = 2.5;
            var wCoef = width / throws;
            var hCoef = heigth / throws;

            Task.Run(() =>
            {
                var r = new Random();

                Enumerable.Range(0, points)
                          .Select
                          (
                              i => Enumerable.Range(0, throws)
                                             .Select(j => r.NextDouble() > threshold)
                                             .Count(b => b)
                          )
                          .ToList()
                          .ForEach(async res => await Dispatcher.InvokeAsync(() =>
                          {
                              var element = new Ellipse
                              {
                                  Width = elR * 2.0,
                                  Height = elR * 2.0,
                                  HorizontalAlignment = HorizontalAlignment.Left,
                                  VerticalAlignment = VerticalAlignment.Bottom,
                                  Fill = Brushes.Green,
                                  Margin = new Thickness(res * wCoef + elR, 0.0, 0.0, res * hCoef + elR)
                              };

                              _grid.Children.Add(element);
                          }));
            });
        }
    }
}
