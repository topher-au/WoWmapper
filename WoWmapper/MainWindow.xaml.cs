using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Octokit;
using WoWmapper.Controllers;
using WoWmapper.Keybindings;
using WoWmapper.Properties;
using WoWmapper.WorldOfWarcraft;
using Application = System.Windows.Application;

namespace WoWmapper
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public delegate void ButtonStyleChangedHandler();

        public delegate void ShowKeybindDialogHandler(GamepadButton button);

        private readonly List<Key> _ignoreKeys = new List<Key>
        {
            Key.Escape,
            Key.LeftAlt,
            Key.RightAlt,
            Key.LeftCtrl,
            Key.RightCtrl,
            Key.LeftShift,
            Key.RightShift,
            Key.LWin,
            Key.RWin
        };

        private readonly Timer _uiTimer = new Timer {AutoReset = true, Interval = 1000};
        private readonly Storyboard ExpandStoryboard;

        private readonly DoubleAnimation FadeInAnimation;
        private readonly DoubleAnimation FadeOutAnimation;
        private readonly Storyboard ShrinkStoryboard;
        private double _finalLeft;

        private GamepadButton _keybindButton;

        private ProgressDialogController _keybindDialogController;
        private Key _keybindKey = Key.None;

        private Release _latest;

        private bool _settingsOpen;

        public MainWindow()
        {
            InitializeComponent();

            // Begin interface update timer
            _uiTimer.Elapsed += UiTimer_Elapsed;
            _uiTimer.Start();

            // Initialize window animations
            ExpandStoryboard = (Storyboard) Resources["ExpandWindow"];
            ShrinkStoryboard = (Storyboard) Resources["ShrinkWindow"];

            ExpandStoryboard.Completed += (sender, args) =>
            {
                BeginAnimation(WidthProperty, null);
                BeginAnimation(LeftProperty, null);
                Width = 500;
                Left = _finalLeft;
                StackContent.Visibility = Visibility.Collapsed;
            };

            ShrinkStoryboard.Completed += (sender, args) =>
            {
                BeginAnimation(WidthProperty, null);
                BeginAnimation(LeftProperty, null);
                Width = 300;
                Left = _finalLeft;
                PanelSettings.Visibility = Visibility.Collapsed;
            };

            FadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                BeginTime = TimeSpan.FromMilliseconds(0),
                Duration = TimeSpan.FromMilliseconds(230)
            };

            FadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                BeginTime = TimeSpan.FromMilliseconds(270),
                Duration = TimeSpan.FromMilliseconds(250)
            };


            CheckUpdates();


            if (Settings.Default.HideAtStartup && Settings.Default.RunInBackground)
                Hide();
            else
                Show();

            ShowKeybindDialogEvent += OnShowKeybindDialog;
            Focus();
        }

        public static event ShowKeybindDialogHandler ShowKeybindDialogEvent;

        public static event ButtonStyleChangedHandler ButtonStyleChanged;

        public static void UpdateButtonStyle()
        {
            if (ButtonStyleChanged != null)
                Application.Current.Dispatcher.Invoke(ButtonStyleChanged);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Settings.Default.RunInBackground)
            {
                e.Cancel = true;
                Hide();
            }
            _uiTimer.Stop();
            base.OnClosing(e);
            Application.Current.Shutdown(0);
        }

        public async void CheckUpdates()
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                var octoApi = new ApiConnection(new Connection(new ProductHeaderValue("WoWmapper")));
                var octoRelease = new ReleasesClient(octoApi);
                var wmReleases = await octoRelease.GetAll("topher-au", "wowmapper");
                var latestStable = wmReleases.First(release => release.Prerelease == false);
                var latestVersion = new Version(latestStable.TagName);
                var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                if (latestVersion > currentVersion)
                {
                    _latest = latestStable;
                    TextUpdateStatus1.Text = $"Version {latestVersion} is available now!";
                    TextUpdateStatus1.Cursor = Cursors.Hand;
                    TextUpdateStatus1.TextDecorations.Add(TextDecorations.Underline);
                    ImageUpdateIcon.Source =
                        new BitmapImage(new Uri("pack://application:,,,/Resources/update-available.png"));
                }
                else
                {
                    TextUpdateStatus1.Text = "You have the latest version.";
                    ImageUpdateIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/update-ok.png"));
                }
            }
            catch (Exception ex)
            {
                TextUpdateStatus1.Text = "Error checking for updates!";
                ImageUpdateIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/update-failed.png"));
                TextUpdateStatus1.ToolTip = ex.Message;
            }

            ImageUpdateIcon.RenderTransform = null;
            ImageUpdateIcon.Triggers.Clear();
        }

        public static void ShowKeybindDialog(GamepadButton button)
        {
            ShowKeybindDialogEvent?.Invoke(button);
        }

        private async void OnShowKeybindDialog(GamepadButton button)
        {
            _keybindButton = button;

            var buttonName = ControllerManager.GetButtonName(button);

            KeyDown += OnKeyDown;
            _keybindDialogController =
                await
                    this.ShowProgressAsync($"Rebind {buttonName}",
                        $"Press a button on your keyboard. Some special keys may not be recognized by the WoW client.",
                        true);

            _keybindDialogController.Canceled += KeybindDialogControllerOnCanceled;
            _keybindDialogController.Closed += KeybindDialogControllerOnClosed;
        }

        private async void KeybindDialogControllerOnCanceled(object sender, EventArgs eventArgs)
        {
            _keybindKey = Key.None;
            await _keybindDialogController.CloseAsync();
        }

        private void KeybindDialogControllerOnClosed(object sender, EventArgs eventArgs)
        {
            if (_keybindKey != Key.None)
            {
                // User pressed a key, save keybinding
                BindManager.SetKey(_keybindButton, _keybindKey);
                Console.WriteLine(_keybindKey.ToString());
            }

            _keybindDialogController.Canceled -= KeybindDialogControllerOnCanceled;
            _keybindDialogController.Closed -= KeybindDialogControllerOnClosed;
        }

        private async void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (_keybindDialogController == null) return;

            // Handle keyboard input for keybinding dialog
            if (keyEventArgs.Key == Key.Escape)
            {
                _keybindKey = Key.None;
            }
            else
            {
                if (keyEventArgs.SystemKey != Key.None)
                    _keybindKey = keyEventArgs.SystemKey;
                else
                    _keybindKey = keyEventArgs.Key;
            }

            if (_ignoreKeys.Contains(_keybindKey)) return;

            if (_keybindDialogController.IsOpen)
                await _keybindDialogController.CloseAsync();
        }

        private void UiTimer_Elapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            // Update UI elements
            try
            {
                Dispatcher.Invoke(UpdateUi);
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception ex)
            {
                Log.WriteLine($"Exception when updating interface:\n        {ex.Message}");
            }
        }

        private void UpdateUi()
        {
            var activeDevice = ControllerManager.ActiveController;

            if (activeDevice != null) // Controller connected
            {
                var battery = activeDevice.BatteryLevel > 100 ? 100 : activeDevice.BatteryLevel;

                if (activeDevice.Type == GamepadType.PlayStation)
                    TextControllerStatus1.Text = $"DualShock 4 connected";
                else
                    TextControllerStatus1.Text = $"Xinput controller connected";

                TextControllerStatus2.Text = $"Battery is at {battery}%";

                if ((activeDevice.Type == GamepadType.Xbox) && ControllerManager.IsXInput9)
                {
                    TextControllerStatus3.Text =
                        "Your system is using the DirectX 9 Xinput library. You will not be able to use the Xbox Guide button.";
                    TextControllerStatus3.Visibility = Visibility.Visible;
                }
                else
                {
                    TextControllerStatus3.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                TextControllerStatus1.Text = "No active controller";
                TextControllerStatus2.Text = "No information available";
            }

            TextWoWStatus1.Text = ProcessManager.GameRunning
                ? "World of Warcraft is running"
                : "World of Warcraft is not running";
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            if (_settingsOpen)
            {
                // Hide settings
                StackContent.Visibility = Visibility.Visible;
                _finalLeft = Left + 100;
                var expand = (Storyboard) Resources["ShrinkWindow"];
                (expand.Children[0] as DoubleAnimation).From = Left;
                (expand.Children[0] as DoubleAnimation).To = _finalLeft;
                BeginStoryboard(expand);

                StackContent.BeginAnimation(OpacityProperty, FadeInAnimation);
                PanelSettings.BeginAnimation(OpacityProperty, FadeOutAnimation);
                _settingsOpen = false;
            }
            else
            {
                // Show settings
                PanelSettings.Visibility = Visibility.Visible;
                _finalLeft = Left - 100;
                var expand = (Storyboard) Resources["ExpandWindow"];
                (expand.Children[0] as DoubleAnimation).From = Left;
                (expand.Children[0] as DoubleAnimation).To = _finalLeft;
                BeginStoryboard(expand);

                StackContent.BeginAnimation(OpacityProperty, FadeOutAnimation);
                PanelSettings.BeginAnimation(OpacityProperty, FadeInAnimation);
                _settingsOpen = true;
            }
        }

        private void DonateButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.RightShift) && Keyboard.IsKeyDown(Key.RightAlt) &&
                (e.ChangedButton == MouseButton.Right))
            {
                return;
            }

            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=PKPJ97Q429QJC");
        }

        private void TextUpdateStatus1_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_latest == null) return;
            var updateForm = new UpdateWindow(_latest) {Owner = this};
            updateForm.ShowDialog();
        }

        private void DiscordLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://discord.gg/8cyyCgc");
        }
    }
}