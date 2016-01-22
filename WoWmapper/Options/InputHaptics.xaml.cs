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
    /// Interaction logic for InputHaptics.xaml
    /// </summary>
    public partial class InputHaptics : UserControl
    {
        public InputHaptics()
        {
            InitializeComponent();


        }
        public void UpdateAdvancedFeedback()
        {
            switch (Settings.Default.EnableAdvancedFeatures)
            {
                case true:
                    CheckVibrateDamage.Visibility = Visibility.Visible;
                    CheckVibrateTarget.Visibility = Visibility.Visible;
                    CheckLightbarClass.Visibility = Visibility.Visible;
                    CheckLightbarHealth.Visibility = Visibility.Visible;
                    TextEnableAdvancedFeedback.Visibility = Visibility.Collapsed;
                    break;
                case false:
                    CheckVibrateDamage.Visibility = Visibility.Collapsed;
                    CheckVibrateTarget.Visibility = Visibility.Collapsed;
                    CheckLightbarClass.Visibility = Visibility.Collapsed;
                    CheckLightbarHealth.Visibility = Visibility.Collapsed;
                    TextEnableAdvancedFeedback.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void ColorPropertyChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            SaveSettings();
        }

        private void CheckboxChanged(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            Settings.Default.Save();
        }

        private void ButtonConfigureColors_Click(object sender, RoutedEventArgs e)
        {
            ColorsFlyout.IsOpen = true;
        }
    }
}
