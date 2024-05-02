using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Ranshen_s_Flash
{
    internal static class Program
    {
        [DllImport("SHCore.dll", SetLastError = true)]
        private static extern bool SetProcessDpiAwareness(int awareness);

        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDpiAwareness(1); 
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
