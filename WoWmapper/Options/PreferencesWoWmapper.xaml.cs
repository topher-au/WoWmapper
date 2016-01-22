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
    /// Interaction logic for PreferencesWoWmapper.xaml
    /// </summary>
    public partial class PreferencesWoWmapper : UserControl
    {
        public PreferencesWoWmapper()
        {
            InitializeComponent();
            MainWindow.UpdateContent += MainWindowUpdateContent;
        }

        private void MainWindowUpdateContent()
        {
            checkCloseTray.IsChecked = Settings.Default.CloseToTray;
            checkMinimizeTray.IsChecked = Settings.Default.MinimizeToTray;
            checkStartMinimized.IsChecked = Settings.Default.StartMinimized;
        }

        private void checkStartMinimized_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.StartMinimized = checkStartMinimized.IsChecked.Value;
            Settings.Default.Save();
        }

        private void checkMinimizeTray_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.MinimizeToTray = checkMinimizeTray.IsChecked.Value;
            Settings.Default.Save();
        }

        private void checkCloseTray_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.CloseToTray = checkCloseTray.IsChecked.Value;
            Settings.Default.Save();
        }
    }
}
