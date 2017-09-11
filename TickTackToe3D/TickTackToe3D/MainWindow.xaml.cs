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

namespace TickTackToe3D
{
    public partial class MainWindow
    {

        public MainWindow()
        {
            InitializeComponent();

        }


        private void Method001()
        {
            /*
                         <Button 
                            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                            Background="{x:Null}"
                            Height="{x:Static SystemParameters.IconHeight}"
                            Content="{Binding Path=Height, RelativeSource={RelativeSource Self}}"/>
            */

            /*

                                 <Button 
                                    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
                                     <Button.Background>
                                        <x:Null/>
                                     </Button.Background>
                                     <Button.Height>
                                        <x:Static Member="SystemParameters.IconHeight"/>
                                     </Button.Height>
                                     <Button.Content>
                                         <Binding Path="Height">
                                             <Binding.RelativeSource>
                                                 <RelativeSource Mode="Self"/>
                                             </Binding.RelativeSource>
                                         </Binding>
                                     </Button.Content>
                                 </Button>

            */


            /* Content
             
             <Button xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
                 <Rectangle Height ="40" Width="40" Fill="Black"/>
             </Button >
             
             */


            var b = new Button
            {
                Background = null,
                Height = SystemParameters.IconHeight
            };

            var binding = new Binding
            {
                Path = new PropertyPath("Height"),
                RelativeSource = RelativeSource.Self
            };

            b.SetBinding(ContentProperty, binding);
        }



        private void Method002()
        {
            /*
             
            <ListBox xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
                <ListBox.Items>
                    <ListBoxItem Content="Item 1"/>
                    <ListBoxItem Content="Item 2"/>
                </ListBox.Items>
            </ListBox>
             
             */

            var listbox = new ListBox();
            listbox.Items.Add(new ListBoxItem { Content = "Item 1" });
            listbox.Items.Add(new ListBoxItem { Content = "Item 2" });
        }

    }
}
