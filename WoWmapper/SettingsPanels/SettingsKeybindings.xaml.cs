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
using WoWmapper.ConsolePort;
using WoWmapper.Controllers;
using WoWmapper.Keybindings;
using WoWmapper.Properties;
using WoWmapper.WorldOfWarcraft;

namespace WoWmapper.SettingsPanels
{
    /// <summary>
    /// Interaction logic for SettingsKeybindings.xaml
    /// </summary>
    public partial class SettingsKeybindings : UserControl
    {
        private bool ready;
        private DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200));

        private DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));

        public SettingsKeybindings()
        {
            InitializeComponent();

            MainWindow.ButtonStyleChanged += RefreshBinds;
            
            fadeOut.Completed += (o, args) =>
            {
                ListKeybinds.Visibility = Visibility.Collapsed;
                ButtonResetBinds.Visibility = Visibility.Collapsed;
            };

            BindManager.BindingsChanged += (sender, args) =>
            {
                RefreshBinds();
            };
            
            DoTransition();

            ready = true;

            TextSyncMessage.Text = Settings.Default.ExportBindings
                        ? "ConsolePort sync is enabled. Your settings will be automatically synced when changed. Restart WoW or type /reload in game to load changes."
                        : "ConsolePort sync is disabled. You will need to manually configure the controller calibration within World of Warcraft before you can play.";

            Settings.Default.SettingChanging += (sender, args) =>
            {
                if (args.SettingName == "ExportBindings")
                {
                    TextSyncMessage.Text = (bool) args.NewValue
                        ? "ConsolePort sync is enabled. Your settings will be automatically synced when changed. Restart WoW or type /reload in game to load changes."
                        : "ConsolePort sync is disabled. You will need to manually configure the controller calibration within World of Warcraft before you can play.";
                }
            };
        }

        private void CheckCustomBinding_Changed(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;

            BindManager.ResetDefaults(Settings.Default.ModifierStyle);
            Settings.Default.BindingsModified = DateTime.Now;
            BindWriter.WriteBinds();
            RefreshBinds();

            DoTransition();
        }

        private void DoTransition()
        {
            if (Properties.Settings.Default.CustomBindings)
            {
                // Show custom bindings panel
                ListKeybinds.BeginAnimation(OpacityProperty, fadeIn);
                ButtonResetBinds.BeginAnimation(OpacityProperty, fadeIn);
                ListKeybinds.Visibility = Visibility.Visible;
                ButtonResetBinds.Visibility = Visibility.Visible;
            }
            else if (!Properties.Settings.Default.CustomBindings)
            {
                // Hide custom bindings panel
                ListKeybinds.BeginAnimation(OpacityProperty, fadeOut);
                ButtonResetBinds.BeginAnimation(OpacityProperty, fadeOut);
            }

            RefreshBinds();
        }
        
        List<Keybind> _binds = new List<Keybind>();

        private void RefreshBinds()
        {
            _binds = BindManager.CurrentKeybinds.ToList();
            ListKeybinds.Items.Clear();

            var shoulders = new List<GamepadButton>
            {
                GamepadButton.ShoulderLeft,
                GamepadButton.ShoulderRight,
                GamepadButton.TriggerLeft,
                GamepadButton.TriggerRight,
            };

            switch (Properties.Settings.Default.ModifierStyle)
            {
                case 0:
                    shoulders.Remove(GamepadButton.ShoulderRight);
                    shoulders.Remove(GamepadButton.TriggerRight);
                    break;
                case 1:
                    shoulders.Remove(GamepadButton.ShoulderLeft);
                    shoulders.Remove(GamepadButton.ShoulderRight);
                    break;
                case 2:
                    shoulders.Remove(GamepadButton.ShoulderLeft);
                    shoulders.Remove(GamepadButton.TriggerLeft);
                    break;
                case 3:
                    shoulders.Remove(GamepadButton.TriggerLeft);
                    shoulders.Remove(GamepadButton.TriggerRight);
                    break;
            }

            _binds.RemoveAll(bind => shoulders.Contains(bind.BindType));

            foreach (var bind in _binds)
            {
                var buttonBind = new TextBlock
                {
                    Text = bind.Key.ToString(),
                    VerticalAlignment = VerticalAlignment.Center
                };
                var buttonText = new TextBlock
                {
                    Text = ControllerManager.GetButtonName(bind.BindType),
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 100,
                };
                var buttonIcon = new Image
                {
                    Source = ControllerManager.GetButtonIcon(bind.BindType),
                    Width = 32,
                    Height = 32,
                    Stretch = Stretch.Uniform
                };
                var buttonPanel = new DockPanel {Children = {buttonIcon, buttonText, buttonBind}};
                var buttonItem = new ListViewItem {Content = buttonPanel};
                ListKeybinds.Items.Add(buttonItem);
            }
        }

        private void ButtonResetBinds_OnClick(object sender, RoutedEventArgs e)
        {
            BindManager.ResetDefaults(Settings.Default.ModifierStyle);
            Settings.Default.BindingsModified = DateTime.Now;
            BindWriter.WriteBinds();
            RefreshBinds();
        }

        private void ComboButtonIcons_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            DoTransition();
            Settings.Default.BindingsModified = DateTime.Now;
            BindWriter.WriteBinds();
            MainWindow.UpdateButtonStyle();
        }

        private void ComboCurrentStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            Settings.Default.BindingsModified = DateTime.Now;
            BindManager.ResetDefaults(ComboCurrentStyle.SelectedIndex);
            BindWriter.WriteBinds();
            DoTransition();
        }

        private void ListKeybinds_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListKeybinds.SelectedItem == null) return;

            MainWindow.ShowKeybindDialog(_binds[ListKeybinds.SelectedIndex].BindType);
        }
    }
}