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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WoWmapper.SettingsPanels
{
    /// <summary>
    /// Interaction logic for SettingsPanel.xaml
    /// </summary>
    public partial class SettingsPanel : UserControl
    {
        private static readonly SettingsWoWmapper SettingsWoWmapper = new SettingsWoWmapper();
        private static readonly SettingsDevices SettingsDevices = new SettingsDevices();
        private static readonly SettingsKeybindings SettingsKeybindings = new SettingsKeybindings();

        private static readonly SettingsAnalog SettingsAnalog = new SettingsAnalog();

        private static readonly SettingsOverlay SettingsOverlay = new SettingsOverlay();
        private string _panelHeader;
        private object _panelContent;

        public SettingsPanel()
        {
            InitializeComponent();

            try
            {
                var defaultTreeViewItem = TreeSettings.Items[0] as TreeViewItem;

                if (defaultTreeViewItem != null)
                    defaultTreeViewItem.IsSelected = true;
            } catch { }
            
        }
        private void TreeSettings_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (GroupSettings.Content == _panelContent) return; // No animation if panel doesn't change

            if (e.OldValue == null) // Set up initial panel
            {
                _panelContent = SettingsWoWmapper;
                GroupSettings.Content = _panelContent;
                return;
            }

            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200));
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));
            fadeOut.Completed += (o, args) =>
            {
                GroupSettings.Content = _panelContent;
                GroupSettings.Header = _panelHeader;
                (GroupSettings.Content as UIElement).BeginAnimation(OpacityProperty, fadeIn);
            };
            (GroupSettings.Content as UIElement).BeginAnimation(OpacityProperty, fadeOut);
        }

        private void WoWmapper_OnSelected(object sender, RoutedEventArgs e)
        {
            _panelHeader = "WoWmapper";
            _panelContent = SettingsWoWmapper;
        }

        private void Devices_Selected(object sender, RoutedEventArgs e)
        {
            _panelHeader = "Devices";
            _panelContent = SettingsDevices;
        }

        private void Keybindings_Selected(object sender, RoutedEventArgs e)
        {
            _panelHeader = "Keybindings";
            _panelContent = SettingsKeybindings;
        }

        private void Camera_Selected(object sender, RoutedEventArgs e)
        {
            _panelHeader = "Analog Inputs";
            _panelContent = SettingsAnalog;
        }

        private void Overlay_OnSelected(object sender, RoutedEventArgs e)
        {
            _panelHeader = "Overlay";
            _panelContent = SettingsOverlay;
        }
    }
}