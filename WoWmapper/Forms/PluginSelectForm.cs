using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WoWmapper.Input;

namespace WoWmapper
{
    public partial class PluginSelectForm : Form
    {
        private List<IInputPlugin> plugins;
        string[] inputDllFiles;

        public PluginSelectForm()
        {
            InitializeComponent();
            RefreshInputPlugins();
            this.Text = Properties.Resources.STRING_PLUGIN_TITLE;
            buttonLoad.Text = Properties.Resources.STRING_PLUGIN_LOAD;
            labelSelectPlugin.Text = Properties.Resources.STRING_PLUGIN_SELECT;
        }

        private void RefreshInputPlugins()
        {
            var currentPlugin = Properties.Settings.Default.InputPlugin;

            var path = "plugins";

            inputDllFiles = null;

            if (!Directory.Exists(path)) return;

            inputDllFiles = Directory.GetFiles(path, "input_*.dll");
            
            if (inputDllFiles == null) return;
            if (inputDllFiles.Length == 0) return;

            // Load all asemblies in folder
            ICollection<Assembly> assemblies = new List<Assembly>(inputDllFiles.Length);
            foreach (string dllFile in inputDllFiles)
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                Assembly assembly = Assembly.Load(an);
                assemblies.Add(assembly);
            }

            // Find all assemblies that implement IInputPlugin
            Type pluginType = typeof(IInputPlugin);
            ICollection<Type> pluginTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface(pluginType.FullName) != null)
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                }
            }

            plugins = new List<IInputPlugin>(pluginTypes.Count);
            foreach (Type type in pluginTypes)
            {
                IInputPlugin plugin = (IInputPlugin)Activator.CreateInstance(type);
                var pluginFile = Assembly.GetAssembly(plugin.GetType()).GetName().Name;
                listInputPlugins.Items.Add(plugin.Name);

                if (currentPlugin != string.Empty && currentPlugin.Contains(pluginFile))
                {
                    listInputPlugins.SelectedIndex = listInputPlugins.Items.Count - 1;
                }
                plugin.Kill();
                plugin = null;
            }

        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (listInputPlugins.SelectedItems.Count != 1) return;
            Properties.Settings.Default.InputPlugin = Path.GetFileName(inputDllFiles[listInputPlugins.SelectedIndex]);
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }

        private void buttonOpenPlugins_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", $"{Application.StartupPath}\\plugins");
        }
    }
}
