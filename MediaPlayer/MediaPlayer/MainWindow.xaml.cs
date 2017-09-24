using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace MyMediaPlayer
{
    public partial class MainWindow
    {


        public MainWindow()
        {
            InitializeComponent();
            _open.Click += _open_Click;
            _playPause.Click += _playPause_Click;
            _media.MediaOpened += _media_MediaOpened;
        }

        private void _media_MediaOpened(object sender, RoutedEventArgs e)
        {
            
        }

        private void _playPause_Click(object sender, RoutedEventArgs e)
        {

        }

        private void _open_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = "*.*",
                Filter = "Media(*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == false)
                return;


            _media.Source = new Uri(openFileDialog.FileName);
        }
    }
}
