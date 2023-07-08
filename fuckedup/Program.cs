using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace fuckedup
{
    class Program
    {

        unsafe static uint stopCode = 0xc000021a;

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

        static unsafe void Crash()
        {
            Console.WriteLine("Fucked up....");
            bool t1;
            uint t2;
            RtlAdjustPrivilege(19, true, false, out t1);
            NtRaiseHardError(stopCode, 0, 0, IntPtr.Zero, 6, out t2);
        }
        static bool IsValidStopCode(uint stopCode)
        {
            // Add your validation logic here
            // Return true if the stop code is valid, false otherwise
            // You can define a list of valid stop codes and check against it, or use any other validation criteria
            // For example, if you want to validate that the stop code starts with "0xC", you can do:
            return stopCode.ToString("X").StartsWith("C");
        }

        static unsafe void Main(string[] args)
        {
            Console.Title = "FuckedUP (CLI) - Best way to fuck up windows without UAC (.NET way)";
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            // Config
            bool now = false;
            bool custom = false;
            bool spam = false;
            bool help = false;
            int count = 0;
            foreach (var arg in args)
            {
                if (arg == "-now")
                {
                    now = true;
                }
                if (arg == "-stop")
                {
                    custom = true;
                    if (uint.TryParse(args[count + 1].Replace("0x", ""), System.Globalization.NumberStyles.HexNumber, null, out stopCode))
                    {
                        // Check if the stop code is valid
                        if (IsValidStopCode(stopCode))
                        {
                            // Valid stop code, continue with your logic
                        }
                        else
                        {
                            Console.WriteLine("Invalid stop code specified.");
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid stop code format.");
                        Environment.Exit(0);
                    }
                }
                if (arg == "-fuckedup")
                {
                    spam = true;
                }
                if (arg == "-help")
                {
                    help = true;
                    now = false;
                }
            }

            if (now == true) { /* skip */ }
            else
            {
                Console.WriteLine("---Few information of your software---");
                Console.WriteLine($"Windows {GetWindowsOSVersion()}");
                Console.WriteLine($".NET Version: {GetDotNetVersion()}");
                Console.WriteLine($"Current login computer\\user: {GetCurrentUsername()}");
                Console.WriteLine($"Running with Admin: {IsUserAdmin()}");
                Console.WriteLine("FuckedUP Version: " + version);
                Console.WriteLine("---End of Debugging---");
                if (custom == false)
                {
                    Console.WriteLine("\n!! Default StopCode: 0xc000021a (Fatal System Error) !!");
                }
                Console.WriteLine("\nWhat is FuckedUP?\nFuckedUP is software that allows you to BSOD windows without admin\nwhich is very helpful for Windows YTPMVs/Crazy Error/Development/Testing\nWarning before pressing any key: Please save your data/everything before pressing it, \nYOUR temporary data will be unsafe and lost after BSOD.\n\n!!TRY WITH YOUR OWN RISK!!");
                if (help == true)
                {
                    Console.WriteLine("Available commands:");
                    Console.WriteLine("-stop <stop code>     : Customize the stop code for NtRaiseHardError");
                    Console.WriteLine("                        (Example: 0xc000021a, start with 0xc ONLY!) *Experiment");
                    Console.WriteLine("-now                  : Immediately trigger the FuckedUP action without confirmation");
                    Console.WriteLine("-fuckedup             : Accelerate the FuckedUP process by spamming NtRaiseHardError");
                    Console.WriteLine("-help                 : Show available commands and examples");
                    Console.WriteLine("Copyright to blueskychan-dev 2023 (https://fusemeow.me)");
                    Environment.Exit(0);
                }
            }
            if (now == true)
            {
                if (custom == true)
                {
                    Console.WriteLine($"Custom stop code: 0x{stopCode:X8}");
                    if (spam == true)
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
                        Crash();
                }
                else
                    if (spam == true)
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
                    Crash();
            }
            else if (custom == true)
            {
                if (now == true)
                {
                    Console.WriteLine($"Custom stop code: 0x{stopCode:X8}");
                    if (spam == true)
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
                        Crash();
                }
                else
                {
                    Console.WriteLine($"Custom stop code: 0x{stopCode:X8}");
                    Console.WriteLine("Press any key to proceed...");
                    Console.ReadLine();
                    if (spam == true)
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
                        Crash();
                }
            }
            else
            Console.WriteLine("Press any key to proceed...");
            Console.ReadLine();
            Crash();
            
            }
        }
    }

