using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WoWmapper.Properties;

namespace WoWmapper.Options
{
    /// <summary>
    /// Interaction logic for InputTriggers.xaml
    /// </summary>
    public partial class InputTriggers : UserControl
    {
        private readonly bool _componentsLoaded = false;

        public InputTriggers()
        {
            InitializeComponent();
            _componentsLoaded = true;
            UpdateTriggerClickPanel();
            UpdateSliders();
        }

        private void UpdateSliders()
        {
            if (!_componentsLoaded || !Settings.Default.EnableTriggerGrip) return;
            SliderLeftClick.Minimum = Settings.Default.ThresholdLeft + 1;
            SliderRightClick.Minimum = Settings.Default.ThresholdRight + 1;
            SliderLeft.Maximum = Settings.Default.ThresholdLeftClick - 1;
            SliderRight.Maximum = Settings.Default.ThresholdRightClick - 1;
            UpdateTriggerClickPanel();
        }

        private void checkEnableTriggerClick_Checked(object sender, RoutedEventArgs e)
        {
            if (!_componentsLoaded) return;
            UpdateTriggerClickPanel();
            UpdateSliders();
            Settings.Default.Save();
        }

        private void UpdateTriggerClickPanel()
        {
            if (Settings.Default.EnableTriggerGrip)
            {
                PanelTriggerClickSliders.Visibility = Visibility.Visible;

                if (Settings.Default.ThresholdLeftClick <= Settings.Default.ThresholdLeft) Settings.Default.ThresholdLeftClick = Settings.Default.ThresholdLeft + 1;
                if (Settings.Default.ThresholdRightClick <= Settings.Default.ThresholdRight) Settings.Default.ThresholdRightClick = Settings.Default.ThresholdRight + 1;

                Settings.Default.Save();
            }
            else
            {
                PanelTriggerClickSliders.Visibility = Visibility.Collapsed;
                SliderLeft.Maximum = 254;
                SliderRight.Maximum = 254;
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.ShowMessageBox(Properties.Resources.DIALOG_HELP_TRIGGER_GRIP_TITLE, Properties.Resources.DIALOG_HELP_TRIGGER_GRIP_TEXT);
        }

        private void SliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Settings.Default.Save();
            UpdateSliders();
        }
    }
}