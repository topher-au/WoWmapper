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
using WoWmapper.Controllers;

namespace WoWmapper.SettingsPanels
{
    /// <summary>
    /// Interaction logic for SettingsDevices.xaml
    /// </summary>
    public partial class SettingsDevices : UserControl
    {
        private ImageSource _ds4Image = new BitmapImage(new Uri("pack://application:,,,/Resources/ds4.png")) {DecodePixelWidth = 24, DecodePixelHeight = 24};
        private ImageSource _xinputImage = new BitmapImage(new Uri("pack://application:,,,/Resources/xinput.png"));
        private List<IController> _controllers;

        public SettingsDevices()
        {
            InitializeComponent();
            ControllerManager.ControllersChanged += UpdateControllerList;
            ControllerManager.ActiveControllerChanged += UpdateSelectedController;
            UpdateControllerList();
            UpdateSelectedController(ControllerManager.ActiveController);
        }

        private void UpdateSelectedController(IController controller)
        {
            if (controller == null)
            {
                ListSelectedDevice.Visibility=Visibility.Hidden;
                TextNoController.Visibility=Visibility.Visible;
                return;
            }

            ListSelectedDevice.Visibility = Visibility.Visible;
            TextNoController.Visibility = Visibility.Hidden;

            ListSelectedDevice.Content = GetListItem(controller);
        }

        public ControllerListItem GetListItem(IController controller)
        {
            if (controller.Type == GamepadType.PlayStation)
            {
                return new ControllerListItem()
                {
                    Image = _ds4Image,
                    Type = controller.Type.ToString(),
                    Name = controller.Name
                };
            }
            else if (controller.Type == GamepadType.Xbox)
            {
                return new ControllerListItem()
                {
                    Image = _xinputImage,
                    Type = "Xinput",
                    Name = controller.Name
                };
            }
            else return null;
        }

        public void UpdateControllerList()
        {
            _controllers = ControllerManager.Controllers;

            var controllerList = new List<ControllerListItem>();

            foreach (var controller in _controllers)
            {
                if (controller.Type == GamepadType.PlayStation)
                {
                    controllerList.Add(GetListItem(controller));
                }
                else if (controller.Type == GamepadType.Xbox)
                {
                    controllerList.Add(GetListItem(controller));
                }
            }

            ListAvailableDevices.ItemsSource = null;
            ListAvailableDevices.ItemsSource = controllerList;
        }

        private void ListAvailableDevices_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SetActiveController();
        }

        private void ButtonUseController_Click(object sender, RoutedEventArgs e)
        {
            SetActiveController();
        }

        private void SetActiveController()
        {
            if (ListAvailableDevices.SelectedItem == null) return;

            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200));
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));
            fadeOut.Completed += (sender, args) =>
            {
                ControllerManager.SetController(_controllers[ListAvailableDevices.SelectedIndex]);
                ListSelectedDevice.BeginAnimation(OpacityProperty, fadeIn);
            };
            ListSelectedDevice.BeginAnimation(OpacityProperty, fadeOut);
        }
    }

    public class ControllerListItem
    {
        public ImageSource Image { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}