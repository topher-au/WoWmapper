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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WoWmapper.Controllers;

namespace WoWmapper.SettingsPanels
{
    /// <summary>
    /// Interaction logic for SettingsVibration.xaml
    /// </summary>
    public partial class SettingsMemoryReading : UserControl
    {
        private double _warningHeight = 132;

        public SettingsMemoryReading()
        {
            InitializeComponent();
            MainWindow.ButtonStyleChanged += ButtonStyleChanged;
        }

        private void ButtonStyleChanged()
        {
            ImageMenuConfirm.Source = ControllerManager.GetButtonIcon(GamepadButton.RFaceDown);
            ImageMenuUp.Source = ControllerManager.GetButtonIcon(GamepadButton.LFaceUp);
            ImageMenuDown.Source = ControllerManager.GetButtonIcon(GamepadButton.LFaceDown);
            ImageAoeConfirm.Source = ControllerManager.GetButtonIcon(GamepadButton.RFaceDown);
            ImageAoeCancel.Source = ControllerManager.GetButtonIcon(GamepadButton.RFaceRight);
        }

        private void TextMoreInfo_Click(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/topher-au/WoWmapper/wiki/Advanced-Features");
        }

        private void HideWarning(object sender, RoutedEventArgs e)
        {

            var shrinkStack = new DoubleAnimation(_warningHeight, 0, TimeSpan.FromMilliseconds(200));
            StackWarning.BeginAnimation(HeightProperty, shrinkStack);
        }

        private void ShowWarning(object sender, RoutedEventArgs e)
        {
            var expandStack = new DoubleAnimation(0, _warningHeight, TimeSpan.FromMilliseconds(200));
            StackWarning.BeginAnimation(HeightProperty, expandStack);
        }
    }
}
