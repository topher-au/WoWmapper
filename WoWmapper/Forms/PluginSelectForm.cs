using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public PluginSelectForm()
        {
            InitializeComponent();
        }

        private void RefreshInputPlugins()
        {
            var currentPlugin = Properties.Settings.Default.InputPlugin;

            var path = "plugins";

            string[] inputDllFiles = null;

            if (Directory.Exists(path))
            {
                // Get all input assemblies in folder
                inputDllFiles = Directory.GetFiles(path, "input_*.dll");
            }
            else
            {
                return;
            }

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
                plugins.Add(plugin);
            }

            foreach (var plugin in plugins)
            {
                var pluginFile = Assembly.GetAssembly(plugin.GetType()).GetName().Name;
                //listInputPlugins.Items.Add(plugin.Name);

                if (currentPlugin != string.Empty && currentPlugin.Contains(pluginFile))
                {
                    //listInputPlugins.SelectedIndex = listInputPlugins.Items.Count - 1;
                }

                plugin.Dispose();
            }
        }
    }
}
