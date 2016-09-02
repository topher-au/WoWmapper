using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls.Dialogs;
using Octokit;
using WoWmapper.Controllers;
using WoWmapper.Keybindings;
using WoWmapper.Overlay;
using WoWmapper.WorldOfWarcraft;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using Timer = System.Timers.Timer;

namespace WoWmapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public delegate void ShowKeybindDialogHandler(GamepadButton button);
        public static event ShowKeybindDialogHandler ShowKeybindDialogEvent;
        
        private bool _settingsOpen = false;

        private readonly DoubleAnimation FadeInAnimation;
        private readonly DoubleAnimation FadeOutAnimation;
        private readonly DoubleAnimation MoveWindowAnimation;

        private readonly Storyboard ExpandStoryboard;
        private readonly Storyboard ShrinkStoryboard;

        private readonly Timer _uiTimer = new Timer() {AutoReset = true, Interval = 250};
        private double _finalLeft = 0;

        public delegate void ButtonStyleChangedHandler();
        public static event ButtonStyleChangedHandler ButtonStyleChanged;

        private Release _latest;

        public static void UpdateButtonStyle()
        {
            if(ButtonStyleChanged != null)
                Application.Current.Dispatcher.Invoke(ButtonStyleChanged);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Properties.Settings.Default.RunInBackground)
            {
                e.Cancel = true;
                Hide();
            }
            _uiTimer.Stop();
            if(App.Overlay != null)
                App.Overlay.CloseOverlay();
            base.OnClosing(e);
        }

        public async void CheckUpdates()
        {
            try
            {
                var octoApi = new Octokit.ApiConnection(new Connection(new ProductHeaderValue("WoWmapper")));
                var octoRelease = new Octokit.ReleasesClient(octoApi);
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
                    ImageUpdateIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/update-available.png"));
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

        public MainWindow()
        {
            InitializeComponent();
            if(Properties.Settings.Default.DisableDonationButton) DonateButton.Visibility=Visibility.Collapsed;
            // Set up UI timer
            _uiTimer.Elapsed += UiTimer_Elapsed;
            _uiTimer.Start();

            ExpandStoryboard = (Storyboard) Resources["ExpandWindow"];
           

            // Initialize animations
            ExpandStoryboard = (Storyboard) Resources["ExpandWindow"];

            ExpandStoryboard.Completed += (sender, args) =>
            {
                BeginAnimation(WidthProperty, null);
                BeginAnimation(LeftProperty, null);
                Width = 500;
                Left = _finalLeft;
                StackContent.Visibility = Visibility.Collapsed;
            };

            ShrinkStoryboard = (Storyboard) Resources["ShrinkWindow"];

            ShrinkStoryboard.Completed += (sender, args) =>
            {
                BeginAnimation(WidthProperty, null);
                BeginAnimation(LeftProperty, null);
                Width = 300;
                Left = _finalLeft;
                PanelSettings.Visibility = Visibility.Collapsed;
            };

            FadeOutAnimation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                BeginTime = TimeSpan.FromMilliseconds(0),
                Duration = TimeSpan.FromMilliseconds(250)
            };

            FadeInAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                BeginTime = TimeSpan.FromMilliseconds(250),
                Duration = TimeSpan.FromMilliseconds(500),
            };


            if (Properties.Settings.Default.HideAtStartup && Properties.Settings.Default.RunInBackground) Hide();

            CheckUpdates();

            ShowKeybindDialogEvent += OnShowKeybindDialog;
            Show();
            Focus();

        }

        public static void ShowKeybindDialog(GamepadButton button)
        {
            ShowKeybindDialogEvent?.Invoke(button);
        }

        private ProgressDialogController _keybindDialogController;
        private Key _keybindKey = Key.None;
        private GamepadButton _keybindButton;

        private async void OnShowKeybindDialog(GamepadButton button)
        {
            _keybindButton = button;

            var buttonName = ControllerManager.GetButtonName(button);

            KeyDown += OnKeyDown;
            _keybindDialogController =
                await
                    this.ShowProgressAsync($"Keybinding", $"Press a button on your keyboard to send for {buttonName}",
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
            else
            {
                // Keybind dialog was canceled
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
                _keybindKey = keyEventArgs.Key;
            }
            
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
            PanelDonate.Visibility = Properties.Settings.Default.DisableDonationButton
                ? Visibility.Collapsed
                : Visibility.Visible;

            var activeDevice = ControllerManager.ActiveController;

            if (activeDevice != null) // Controller connected
            {
                var battery = activeDevice.BatteryLevel > 100 ? 100 : activeDevice.BatteryLevel;

                if (activeDevice.Type == GamepadType.PlayStation)
                {
                    TextControllerStatus1.Text = $"DualShock 4 connected";
                }
                else
                {
                    TextControllerStatus1.Text = $"Xinput controller connected";
                }

                TextControllerStatus2.Text = $"Battery is at {battery}%";
            }
            else
            {
                TextControllerStatus1.Text = "No active controller";
                TextControllerStatus2.Text = "No information available";
            }

            TextWoWStatus1.Text = ProcessManager.GameRunning
                ? "World of Warcraft is running"
                : "World of Warcraft is not running";

            if (Properties.Settings.Default.EnableMemoryReading)
                TextWoWStatus2.Text = MemoryManager.IsAttached
                    ? "Memory reading is enabled"
                    : "Memory reading is unavailable";
            else
                TextWoWStatus2.Text = "Memory reading is disabled";
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
            if (Keyboard.IsKeyDown(Key.RightShift) && Keyboard.IsKeyDown(Key.RightAlt) && e.ChangedButton == MouseButton.Right)
            {
                Properties.Settings.Default.DisableDonationButton = true;
                DonateButton.Visibility = Visibility.Collapsed;
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
    }
}