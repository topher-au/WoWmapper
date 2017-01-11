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
using MahApps.Metro;
using WoWmapper.Controllers;
using WoWmapper.Keybindings;
using WoWmapper.Properties;
using WoWmapper.WoWInfoReader;

namespace WoWmapper.SettingsPanels
{
    /// <summary>
    /// Interaction logic for SettingsVibration.xaml
    /// </summary>
    public partial class SettingsMemoryReading : UserControl
    {
        private double _warningHeight = 132;
        private bool _ready;
        private int confirmIndex, cancelIndex;
        private List<GamepadButton> _buttons = new List<GamepadButton>();

        public SettingsMemoryReading()
        {
            InitializeComponent();
            MainWindow.ButtonStyleChanged += ButtonStyleChanged;
            ThemeManager.IsThemeChanged += (sender, args) =>
            {
                ButtonStyleChanged();
            };
            ButtonStyleChanged();
            _ready = true;
        }

        private void ButtonStyleChanged()
        {
            _ready = false;
            ImageMenuConfirm.Source = ControllerManager.GetButtonIcon(GamepadButton.RFaceDown);
            ImageMenuUp.Source = ControllerManager.GetButtonIcon(GamepadButton.LFaceUp);
            ImageMenuDown.Source = ControllerManager.GetButtonIcon(GamepadButton.LFaceDown);

            var _binds = BindManager.CurrentKeybinds.ToList();
            ComboAoeCancel.Items.Clear();
            ComboAoeConfirm.Items.Clear();
            _buttons.Clear();

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
            _binds.RemoveAll(bind => bind.BindType.ToString().StartsWith("LeftStick"));
            var foreColor = (Brush)ThemeManager.DetectAppStyle().Item1.Resources["BlackColorBrush"];
            foreach (var bind in _binds)
            {
                foreach (ComboBox box in new[] {ComboAoeConfirm, ComboAoeCancel})
                {
                    var buttonItem = new ComboBoxItem
                    {
                        Content = new DockPanel
                        {
                            Children =
                            {
                                new Image
                                {
                                    Source = ControllerManager.GetButtonIcon(bind.BindType),
                                    Width = 24,
                                    Height = 24,
                                    Stretch = Stretch.Uniform
                                },
                                new TextBlock
                                {
                                    Text = ControllerManager.GetButtonName(bind.BindType),
                                    VerticalAlignment = VerticalAlignment.Center,
                                    Margin=new Thickness(5,0,0,0), Foreground=foreColor
                                }
                            }
                        }
                    };
                    if (Settings.Default.MemoryAoeConfirm == bind.BindType && box.Name=="ComboAoeConfirm") buttonItem.IsSelected = true;
                    if (Settings.Default.MemoryAoeCancel == bind.BindType && box.Name == "ComboAoeCancel") buttonItem.IsSelected = true;
                    box.Items.Add(buttonItem);
                }

                
                _buttons.Add(bind.BindType);
            }
            _ready = true;
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

        private void ButtonRefreshValues_Click(object sender, RoutedEventArgs e)
        {
            ListDebug.Items.Clear();
            var health = WoWReader.PlayerHealth;
            ListDebug.Items.Add(new DebugItem()
            {
                Name = "PlayerBase",
                Address = WoWReader.offsetPlayerBase.ToString("X2"),
                Value = $"{health.Item1}/{health.Item2}"
            });
            ListDebug.Items.Add(new DebugItem()
            {
                Name = "GameState",
                Address = WoWReader.offsetGameState.ToString("X2"),
                Value = WoWReader.GameState.ToString()
            });
            ListDebug.Items.Add(new DebugItem()
            {
                Name = "MouselookState",
                Address = WoWReader.offsetMouselookState.ToString("X2"),
                Value = WoWReader.MouselookState.ToString()
            });
            ListDebug.Items.Add(new DebugItem()
            {
                Name = "WalkState",
                Address = WoWReader.offsetWalkState.ToString("X2"),
                Value = WoWReader.MovementState.ToString()
            });
            ListDebug.Items.Add(new DebugItem()
            {
                Name = "AoeState",
                Address = WoWReader.offsetAoeState.ToString("X2"),
                Value = WoWReader.AoeState.ToString()
            });
        }

        private void AoeOverride_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (!_ready) return;
            Settings.Default.MemoryAoeCancel = _buttons[ComboAoeCancel.SelectedIndex];
            Settings.Default.MemoryAoeConfirm = _buttons[ComboAoeConfirm.SelectedIndex];
        }

        public class DebugItem
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string Value { get; set; }
        }
    }

   
}
