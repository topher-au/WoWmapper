using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WoWmapper.WorldOfWarcraft;
using WoWmapper.Properties;

namespace WoWmapper.Options
{
    /// <summary>
    /// Interaction logic for AdvancedSettings.xaml
    /// </summary>
    public partial class AdvancedSettings : UserControl
    {
        public static event EventHandler ShowFeedbackWarning;
        public static event EventHandler DoResetAll;

        public AdvancedSettings()
        {
            InitializeComponent();
            CheckEnableAdvancedFeatures.IsChecked = Settings.Default.EnableAdvancedFeatures;
            MainWindow.UpdateContent += AdvancedSettings_UpdateFeedbackCheckbox;
        }

        private void AdvancedSettings_UpdateFeedbackCheckbox()
        {
            CheckEnableAdvancedFeatures.IsChecked = Settings.Default.EnableAdvancedFeatures;
        }

        private void CheckboxChanged(object sender, RoutedEventArgs e)
        {
            if (CheckEnableAdvancedFeatures.IsChecked.Value != Settings.Default.EnableAdvancedFeatures)
            {
                if(Settings.Default.EnableAdvancedFeatures == false)
                {
                    ShowFeedbackWarning?.Invoke(null,null);
                } else
                {
                    Settings.Default.EnableAdvancedFeatures = false;
                    Settings.Default.Save();
                    ProcessWatcher.Restart();
                    MainWindow.UpdateChildren();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoResetAll(this, e);
        }

        private void ButtonMoreInfo_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/topher-au/WoWmapper/wiki/Advanced-Features");
        }
    }
}
