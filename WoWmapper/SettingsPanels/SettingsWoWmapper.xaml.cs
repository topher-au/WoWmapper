using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using MahApps.Metro;
using MahApps.Metro.Controls;
using WoWmapper.ConsolePort;

namespace WoWmapper.SettingsPanels
{
    /// <summary>
    /// Interaction logic for SettingsWoWmapper.xaml
    /// </summary>
    public partial class SettingsWoWmapper : UserControl
    {
        private bool _ready;

        public SettingsWoWmapper()
        {
            InitializeComponent();
            ComboAccent.ItemsSource = ThemeManager.Accents;
            ComboTheme.ItemsSource = ThemeManager.AppThemes;

            var currentTheme = ThemeManager.DetectAppStyle(Application.Current);

            ComboAccent.SelectedIndex = ComboAccent.Items.IndexOf(currentTheme.Item2);
            ComboTheme.SelectedIndex = ComboTheme.Items.IndexOf(currentTheme.Item1);

            TextVersion.Text = $"WoWmapper Version {Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";

            _ready = true;
        }

        private void ButtonResetAll_Click(object sender, RoutedEventArgs e)
        {
            var warn = MessageBox.Show("Your settings will be reset and WoWmapper will exit.\n\nContinue?", "Warning",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (warn == MessageBoxResult.No) return;

            Properties.Settings.Default.Reset();
            Properties.Settings.Default.Save();
            Application.Current.Shutdown();
        }

        private void ComboTheme_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_ready) return;

            Properties.Settings.Default.AppTheme = (ComboTheme.SelectedItem as AppTheme).Name;
            Properties.Settings.Default.AppAccent = (ComboAccent.SelectedItem as Accent).Name;
            App.SetTheme();
        }

        private void CheckExportBindings_OnChecked(object sender, RoutedEventArgs e)
        {
            BindWriter.WriteBinds();
        }

        private void CheckOverrideXinput_OnChecked(object sender, RoutedEventArgs e)
        {
            if (!_ready) return;
            MessageBox.Show("Please restart WoWmapper for changes to this setting to take effect.", "Notice",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
