using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WoWmapper.Controllers;
using WoWmapper.Input;
using WoWmapper.Properties;
using WoWmapper.WorldOfWarcraft.AddOns;
using UserControl = System.Windows.Controls.UserControl;

namespace WoWmapper.Options
{
    /// <summary>
    /// Interaction logic for InputKeybinds.xaml
    /// </summary>
    public partial class InputKeybinds : UserControl
    {
        private bool _ready;
        public InputKeybinds()
        {
            InitializeComponent();

            BindingManager.LoadProfile(new KeybindProfile());
            BindingManager.BindingsUpdated += UpdateBindList;
            ControllerManager.ControllerChanged += ControllerManagerOnControllerChanged;
            _ready = true;
        }

        private void ControllerManagerOnControllerChanged()
        {
            UpdateBindList();
        }

        public void UpdateBindList()
        {
            Dispatcher.Invoke(() =>
            {
                var selected = listKeybinds.SelectedIndex;

                listKeybinds.Items.Clear();
                var activeController = ControllerManager.GetActiveController();
                if (activeController != null)
                {
                    var binds = BindingManager.GetKeybindList(activeController.Type);
                    foreach (var bind in binds) listKeybinds.Items.Add(bind);

                    if (selected < listKeybinds.Items.Count) listKeybinds.SelectedIndex = selected;
                }
                
            });
        }

        private void listKeybinds_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = (ScrollViewer)listKeybinds.Parent;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void ListKeybinds_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listKeybinds.SelectedItems.Count != 1) return;
            KeybindListItem selectedBind = (KeybindListItem)listKeybinds.SelectedItems[0];
            MainWindow.ShowKeybindDialog(selectedBind.BindingKey);
        }

        private void ButtonResetDefaults_Click(object sender, RoutedEventArgs e)
        {
            BindingManager.ResetBinds();
            UpdateBindList();
        }

        private void CheckEnableExport_OnChecked(object sender, RoutedEventArgs e)
        {
            if (!_ready) return;
            BindingManager.SaveKeybinds();
            Settings.Default.Save();
        }
    }
}
