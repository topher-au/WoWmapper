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

namespace WoWmapper.Options
{
    /// <summary>
    /// Interaction logic for HapticsColors.xaml
    /// </summary>
    public partial class HapticsColors : UserControl
    {
        public HapticsColors()
        {
            InitializeComponent();
        }

        private void CheckboxChanged(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
