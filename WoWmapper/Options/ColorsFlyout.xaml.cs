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
using MahApps.Metro.Controls;
using WoWmapper.Properties;

namespace WoWmapper.Options
{
    /// <summary>
    /// Interaction logic for ColorsFlyout.xaml
    /// </summary>
    public partial class ColorsFlyout : Flyout
    {
        public ColorsFlyout()
        {
            InitializeComponent();
        }

        private void SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Settings.Default.Save();
        }

        private void SliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Settings.Default.Save();
        }

        private void CloseFlyout(object sender, RoutedEventArgs e)
        {
            this.IsOpen = false;
        }
    }
}
