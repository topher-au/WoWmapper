using WoWmapper.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using FolderSelect;
using System.Windows.Interop;
using WoWmapper.WorldOfWarcraft;

namespace WoWmapper.Options
{
    /// <summary>
    /// Interaction logic for PreferencesWoW.xaml
    /// </summary>
    public partial class PreferencesWoW : UserControl
    {
        public PreferencesWoW()
        {
            InitializeComponent();
            MainWindow.UpdateContent += MainWindowUpdateContent;
        }

        private void MainWindowUpdateContent()
        {
            textWoWPath.Text = Properties.Settings.Default.WoWFolder;

            switch (Properties.Settings.Default.ForceArchitecture)
            {
                case 64:
                    comboGameArchitecture.SelectedIndex = 1;
                    break;

                case 32:
                    comboGameArchitecture.SelectedIndex = 2;
                    break;

                default:
                    comboGameArchitecture.SelectedIndex = 0;
                    break;
            }
        }

        private void comboGameArchitecture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (comboGameArchitecture.SelectedIndex)
            {
                case 0: // Auto-select
                    Properties.Settings.Default.ForceArchitecture = -1;
                    break;

                case 1: // 64-bit
                    Properties.Settings.Default.ForceArchitecture = 64;
                    break;

                case 2: // 32-bit
                    Properties.Settings.Default.ForceArchitecture = 32;
                    break;
            }
            Properties.Settings.Default.Save();
        }

        private void buttonLocateWoW_Click(object sender, RoutedEventArgs e)
        {
            FolderSelectDialog wowFolderDialog = new FolderSelectDialog();
            wowFolderDialog.InitialDirectory = "C:\\";
            wowFolderDialog.Title = Properties.Resources.DialogWoWFolderBrowserTitle;

            // Get main window handle so the form is locked while dialog is open
            IntPtr mainWindowHandle = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            var folderResult = wowFolderDialog.ShowDialog(mainWindowHandle);

            if (!folderResult) return;

            if(!Functions.ValidateWoWPath(wowFolderDialog.FileName))
            {
                // Not a valid path
                MessageBox.Show(Properties.Resources.ErrorDialogInvalidFolderText, Properties.Resources.ErrorDialogInvalidFolderTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                // Valid path, save and update
                Properties.Settings.Default.WoWFolder = wowFolderDialog.FileName;
                Properties.Settings.Default.Save();
                MainWindowUpdateContent();
            }
        
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}