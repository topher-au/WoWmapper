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
using WoWmapper.Overlay;

namespace WoWmapper.SettingsPanels
{
    /// <summary>
    /// Interaction logic for SettingsOverlay.xaml
    /// </summary>
    public partial class SettingsOverlay : UserControl
    {
        public SettingsOverlay()
        {
            InitializeComponent();
        }

        private void CheckEnableOverlay_OnChecked(object sender, RoutedEventArgs e)
        {
            App.Overlay.Start();
        }

        private void CheckEnableOverlay_OnUnchecked(object sender, RoutedEventArgs e)
        {
            App.Overlay.Stop();
        }

        private void Alignment_Changed(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                App.Overlay.SetAlignment((HorizontalAlignment)ComboHorizontal.SelectedIndex, (VerticalAlignment)ComboVertical.SelectedIndex);
            } catch { }
        }

        private void TestNotification_Click(object sender, RoutedEventArgs e)
        {
            App.Overlay.PopupNotification(new OverlayNotification()
            {
                Header="This is a test",
                Content="This is a test notification. You clicked a button, and this is the notification that appeared. That's it.",
                Duration=5000
            });
        }
    }
}
