using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace WoWmapper.Options
{
    /// <summary>
    /// Interaction logic for AboutWoWmapper.xaml
    /// </summary>
    public partial class AboutWoWmapper : UserControl
    {
        public AboutWoWmapper()
        {
            InitializeComponent();
        }

        private void Ds4Windows_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/Jays2Kings/DS4Windows");
        }

        private void Xinput_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/jjoyner/xinput-wrapper");
        }
    }
}
