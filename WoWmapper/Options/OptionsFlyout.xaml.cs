using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace WoWmapper.Options
{
    /// <summary>
    /// Interaction logic for OptionsFlyout.xaml
    /// </summary>
    public partial class OptionsFlyout : Flyout
    {
        private PreferencesWoWmapper pWoWmapper = new PreferencesWoWmapper();
        private PreferencesWoW pWoW = new PreferencesWoW();
        private PreferencesAppearance pAppearance = new PreferencesAppearance();
        private InputControllers pControllers = new InputControllers();
        private InputCursor pCursor = new InputCursor();
        private InputTriggers pTriggers = new InputTriggers();
        private InputHaptics pHaptics = new InputHaptics();
        private InputKeybinds pKeybinds = new InputKeybinds();
        private AdvancedSettings pAdvanced = new AdvancedSettings();
        private AboutWoWmapper pAbout = new AboutWoWmapper();
        
        public OptionsFlyout()
        {
            InitializeComponent();


            // Expand options tree
            foreach (TreeViewItem treeItem in treeSettings.Items)
            {
                if(treeItem.Items.Count > 0)
                    treeItem.IsExpanded = true;
                if (treeSettings.SelectedItem == null)
                    (treeItem.Items[0] as TreeViewItem).IsSelected = true;
            }
        }



        public void OpenKeybinds()
        {
            TreeViewItemInputKeybinds.IsSelected = true;
        }

        public void OpenControllers()
        {
            TreeViewItemInputControllers.IsSelected = true;
        }

        public void OpenWoW()
        {
            TreeViewItemPreferencesWoW.IsSelected = true;
        }

        private void TreeViewItemPreferencesWoWmapper_Selected(object sender, RoutedEventArgs e)
        {
            SettingsContent.Content = pWoWmapper;
        }

        private void TreeViewItemPreferencesAppearance_Selected(object sender, RoutedEventArgs e)
        {
            SettingsContent.Content = pAppearance;
        }

        private void treeSettings_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //TreeViewItem selectedItem = treeSettings.SelectedItem as TreeViewItem;
            //if (selectedItem != null && selectedItem.Items.Count > 0) (selectedItem.Items[0] as TreeViewItem).IsSelected = true;
        }

        private void TreeViewItemInputControllers_Selected(object sender, RoutedEventArgs e)
        {
            SettingsContent.Content = pControllers;
        }

        private void TreeViewItemInputHapticFeedback_Selected(object sender, RoutedEventArgs e)
        {
            SettingsContent.Content = pHaptics;
            pHaptics.UpdateAdvancedFeedback();
        }

        private void TreeViewItemInputKeybinds_Selected(object sender, RoutedEventArgs e)
        {
            SettingsContent.Content = pKeybinds;
        }

        private void TreeViewItemAdvancedSettings_Selected(object sender, RoutedEventArgs e)
        {
            SettingsContent.Content = pAdvanced;
        }

        private void TreeViewItemPreferencesWoW_Selected(object sender, RoutedEventArgs e)
        {
            SettingsContent.Content = pWoW;
        }

        private void AboutWoWmapper_Click(object sender, RoutedEventArgs e)
        {
            SettingsContent.Content = pAbout;
            TreeViewItem selectedItem = treeSettings.SelectedItem as TreeViewItem;
            if (selectedItem != null) selectedItem.IsSelected = false;
        }

        private void TreeViewItemInputTriggers_Selected(object sender, RoutedEventArgs e)
        {
            SettingsContent.Content = pTriggers;
        }

        private void TreeViewItemInputCursor_Selected(object sender, RoutedEventArgs e)
        {
            SettingsContent.Content = pCursor;
        }
    }
}
