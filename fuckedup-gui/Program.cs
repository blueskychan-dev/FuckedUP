using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fuckedup_gui
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
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Console.WriteLine("!! IF DO YOU USE IN TERMINAL, PLEASE USE RELEASE FOR CLI !!");
        }
    }
}
