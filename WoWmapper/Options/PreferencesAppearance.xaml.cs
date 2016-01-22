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
using MahApps.Metro;
using WoWmapper.Properties;

namespace WoWmapper.Options
{
    /// <summary>
    /// Interaction logic for PreferencesAppearance.xaml
    /// </summary>
    public partial class PreferencesAppearance : UserControl
    {
        public PreferencesAppearance()
        {
            InitializeComponent();

            MainWindow.UpdateContent += MainWindowUpdateContent;
        }

        private void MainWindowUpdateContent()
        {
            comboTheme.Items.Clear();
            comboAccent.Items.Clear();

            // Load Themes
            foreach (var Theme in ThemeManager.AppThemes)
            {
                
                comboTheme.Items.Add(Theme);
                if (Theme.Name == Settings.Default.AppTheme) comboTheme.SelectedIndex = comboTheme.Items.Count - 1;
            }

            // Load Accents
            foreach (var Accent in ThemeManager.Accents)
            {
                comboAccent.Items.Add(Accent);
                if (Accent.Name == Settings.Default.AppAccent) comboAccent.SelectedIndex = comboAccent.Items.Count - 1;
            }

            switch (Settings.Default.EnableAdvancedFeatures)
            {
                case true:
                    PanelAdvanced.Visibility = Visibility.Visible;
                    AdvancedNote.Visibility = Visibility.Collapsed;
                    break;
                case false:
                    PanelAdvanced.Visibility = Visibility.Collapsed;
                    AdvancedNote.Visibility = Visibility.Visible;
                    break;
            }

            // Load other settings
            checkDropShadow.IsChecked = Settings.Default.AppDropShadow;
        }

        private void comboTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ThemeManager.ChangeAppStyle(Application.Current, (Accent)comboAccent.SelectedItem, (AppTheme)comboTheme.SelectedItem);
                Settings.Default.AppTheme = ((AppTheme)comboTheme.SelectedItem).Name;
                Settings.Default.Save();
            } catch { }
            
        }

        private void comboAccent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ThemeManager.ChangeAppStyle(Application.Current, (Accent)comboAccent.SelectedItem, (AppTheme)comboTheme.SelectedItem);
                Settings.Default.AppAccent = ((Accent)comboAccent.SelectedItem).Name;
                Settings.Default.Save();
            }
            catch { }
        }

        private void checkDropShadow_Checked(object sender, RoutedEventArgs e)
        {
            if(Settings.Default.AppDropShadow != checkDropShadow.IsChecked.Value)
            {
                Settings.Default.AppDropShadow = checkDropShadow.IsChecked.Value;
                Settings.Default.Save();
                textNextStartup.Visibility = Visibility.Visible;
            }
        }

        private void comboLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}
