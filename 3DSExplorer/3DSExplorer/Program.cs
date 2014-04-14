using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DSDecmp.Formats.Nitro;

namespace _3DSExplorer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
                Application.Run(new frmExplorer(args[1]));
            else
                Application.Run(new frmExplorer());
            
        }
    }
}
