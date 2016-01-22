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
using Xceed.Wpf.Toolkit;
using WoWmapper;
using WoWmapper.Controllers;

namespace WoWmapper.Options
{
    /// <summary>
    /// Interaction logic for InputCursor.xaml
    /// </summary>
    public partial class InputCursor : UserControl
    {
        public InputCursor()
        {
            InitializeComponent();
            MainWindow.UpdateContent += MainWindowUpdateContent;
        }

        private void MainWindowUpdateContent()
        {
            switch (Properties.Settings.Default.EnableAdvancedFeatures)
            {
                case true:
                    PanelAdvanced.Visibility = Visibility.Visible;
                    AdvancedNote.Visibility = Visibility.Collapsed;
                    break;
                case false:
                    PanelAdvanced.Visibility = Visibility.Collapsed;
                    AdvancedNote.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
