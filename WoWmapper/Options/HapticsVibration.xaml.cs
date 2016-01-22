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
using WoWmapper.Properties;

namespace WoWmapper.Options
{
    /// <summary>
    /// Interaction logic for HapticsVibration.xaml
    /// </summary>
    public partial class HapticsVibration : UserControl
    {
        public HapticsVibration()
        {
            InitializeComponent();
        }

        private void CheckboxChanged(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}
