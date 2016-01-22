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
using WoWmapper.Controllers;
using WoWmapper.Properties;

namespace WoWmapper.Options
{
    /// <summary>
    /// Interaction logic for InputControllers.xaml
    /// </summary>
    public partial class InputControllers : UserControl
    {
        public class ControllerListItem
        {
            public BitmapImage DisplayImage { get; set; }

            public string ControllerName { get; set; }
            public string ControllerNumber { get; set; }
            public string DisplayID { get; set; }
            public Color ForeColor { get; set; }

            public IController Controller { get; set; }
        }

        public InputControllers()
        {
            InitializeComponent();
            MainWindow.UpdateContent += RefreshControllerList;
            ControllerManager.ControllersUpdated += ControllersUpdated;
        }

        private void ControllersUpdated()
        {
            Dispatcher.Invoke(RefreshControllerList);
        }

        private void RefreshControllerList()
        {
            listControllers.Items.Clear();

            var currentControllers = ControllerManager.CurrentControllers;
            var activeController = ControllerManager.GetActiveController();
            int activeIndex = -1;

            if(activeController != null)
                activeIndex = currentControllers.IndexOf(activeController);

            for (int i = 0; i < currentControllers.Count; i++)
            {
                BitmapImage displayImage = new BitmapImage();
                displayImage.BeginInit();
                switch (currentControllers[i].Type)
                {
                    case ControllerType.DualShock4:
                        displayImage.UriSource = new Uri(@"pack://application:,,,/Resources/playstation-24.png");
                        break;
                    case ControllerType.Xbox:
                        displayImage.UriSource = new Uri(@"pack://application:,,,/Resources/xbox-24.png");
                        break;
                    default:
                        displayImage = null;
                        break;
                }
                displayImage.EndInit();
                string Name = currentControllers[i].Type.ToString();
                listControllers.Items.Add(new ControllerListItem()
                {
                    DisplayImage = displayImage,
                    ControllerName = Name,
                    ControllerNumber = string.Format(Properties.Resources.CONTROLLER_NUMBER, i+1),
                    DisplayID = currentControllers[i].ControllerID,
                    ForeColor = Color.FromArgb(255,123,45,142)
                });
            }

            listControllers.SelectedIndex = activeIndex;

            buttonActivateController.IsEnabled = listControllers.SelectedItems.Count == 1 ? true : false;
        }

        private void UpdateSettings(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
        }

        private void ScanForControllers_Click(object sender, RoutedEventArgs e)
        {
            RefreshControllerList();
        }

        private void ActivateController_Click(object sender, RoutedEventArgs e)
        {
            ControllerManager.SetActiveController(ControllerManager.CurrentControllers[listControllers.SelectedIndex]);
            RefreshControllerList();
        }

        private void listControllers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonActivateController.IsEnabled = listControllers.SelectedItems.Count == 1 ? true : false;
        }
    }
}
