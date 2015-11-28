using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsolePort
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if(!Environment.Is64BitProcess)
            {
                // Incompatibility with x86
                MessageBox.Show("Sorry, this application can only be run in 64-bit mode.", "ConsolePort", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
