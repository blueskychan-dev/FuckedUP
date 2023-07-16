using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace fuckedup_gui
{
    public partial class Form1 : Form
    {
        unsafe static uint stopCode = 0xc000021a;
        static uint[] StopCodeList = {
    0xc000021a,
    0xc0000022,
    0xc0000005,
    0xc0000221,
    0xc000026C,
    0xc0000374,
    0xc0000142,
    0xc0000225,
    0xc0000234,
    0xc000026D,
    0xc000027B,
    0xc0000350,
    0xc0000415,
    0xc0000428,
    0xc0000602,
    0xc0000000
};

        [DllImport("ntdll.dll")]
        public static extern uint RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

        [DllImport("ntdll.dll")]
        public static extern uint NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ValidResponseOption, out uint Response);

        public static string GetWindowsOSVersion()
        {
            OperatingSystem os = Environment.OSVersion;
            Version version = os.Version;
            string platform = os.Platform.ToString();
            string servicePack = os.ServicePack;

            return $"Version: {version}, Platform: {platform}, Service Pack: {servicePack}";
        }

        public static string GetDotNetVersion()
        {
            Version dotNetVersion = Environment.Version;
            return dotNetVersion.ToString();
        }

        public static string GetCurrentUsername()
        {
            WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
            string username = currentIdentity.Name;

            return username;
        }

        public static bool IsUserAdmin()
        {
            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

            return isAdmin;
        }
        static bool IsValidStopCode(uint stopCode)
        {
            // Add your validation logic here
            // Return true if the stop code is valid, false otherwise
            // You can define a list of valid stop codes and check against it, or use any other validation criteria
            // For example, if you want to validate that the stop code starts with "0xC", you can do:
            return stopCode.ToString("X").StartsWith("C");
        }

        static unsafe void Crash()
        {
            Console.WriteLine("Fucked up....");
            bool t1;
            uint t2;
            RtlAdjustPrivilege(19, true, false, out t1);
            NtRaiseHardError(stopCode, 0, 0, IntPtr.Zero, 6, out t2);
        }
        public Form1()
        {
            try{
                InitializeComponent();
            }
            catch
            {
                InitializeComponent_noicon();
                MessageBox.Show("Due to compatibility issues between your .NET version and operating system, this program will be run in safe mode to ensure compatibility with your current software. Please note that some features may not work properly or may experience glitches, but the main features will function correctly.", "Compatibility Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            comboBox1.SelectedIndex = 0;
            Console.WriteLine("!! IF DO YOU USE IN TERMINAL, PLEASE USE RELEASE FOR CLI !!");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                var msgbox = MessageBox.Show("WARNING: Use this feature only if you have knowledge about stop codes. Custom stop codes will only work for errors starting with '0xC' Only!", "WARNING!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (msgbox == DialogResult.Yes)
                {
                    comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
                }
                else
                {
                    checkBox1.Checked = false;
                }
            }
            else
            {
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bool now = checkBox2.Checked;
            bool custom = checkBox1.Checked;
            bool spam = checkBox3.Checked;

            if (custom)
            {
                if (uint.TryParse(comboBox1.Text.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber, null, out stopCode))
                {
                    // Check if the stop code is valid
                    if (!IsValidStopCode(stopCode))
                    {
                        MessageBox.Show("Invalid stop code specified.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid stop code format.");
                    return;
                }
            }
            else
            {
                stopCode = StopCodeList[comboBox1.SelectedIndex];
            }

            if (now)
            {
                Crash();
            }
            else
            {
                var result = MessageBox.Show("Warning: Continuing will initiate a process that may cause a Blue Screen of Death. Are you sure you want to proceed?", "Caution", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (spam)
                    {
                        for (;;)
                        {
                            new Thread(() =>
                            {
                                Crash();
                            }).Start();
                        }
                    }
                    else
                    {
                        Crash();
                    }
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            MessageBox.Show($"---Few information of your software---\nWindows {GetWindowsOSVersion()}\n.NET Version: {GetDotNetVersion()}\nCurrent login computer\\user: {GetCurrentUsername()}\nRunning with Admin: {IsUserAdmin()}\nFuckedUP Version: {version}\n---End of Debugging---");
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.ShowDialog();
        }
    }
}

    