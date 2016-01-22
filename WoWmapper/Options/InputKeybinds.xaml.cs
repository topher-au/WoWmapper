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
using UserControl = System.Windows.Controls.UserControl;

namespace WoWmapper.Options
{
    /// <summary>
    /// Interaction logic for InputKeybinds.xaml
    /// </summary>
    public partial class InputKeybinds : UserControl
    {
        public InputKeybinds()
        {
            InitializeComponent();

            BindingManager.LoadProfile(new KeybindProfile());
            BindingManager.BindingsUpdated += UpdateBindList;
            ControllerManager.ControllerChanged += ControllerManagerOnControllerChanged;
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

        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog();
            var openResult = openDialog.ShowDialog();
            if (openResult != DialogResult.OK || !File.Exists(openDialog.FileName)) return;

            var newBinds = BindingManager.ReadFile(openDialog.FileName);

            if (newBinds == null) return;

            BindingManager.CurrentBinds = newBinds.Keybinds;
            BindingManager.SaveKeybinds();
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            var saveResult = saveDialog.ShowDialog();
            if (saveResult != DialogResult.OK) return;

            BindingManager.WriteFile(new KeybindProfile() {Keybinds = BindingManager.CurrentBinds}, saveDialog.FileName);
        }
    }
}
