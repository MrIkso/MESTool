using System;
using System.IO;
using static MESTool.MesToolProcessor;
using System.Windows.Forms;

namespace MESTool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainForm());

        }
    }

    /*
    public class Program
    {
        private static MesToolProcessor processor;

        static void Main(string[] args)
        {
            processor = new MesToolProcessor(Properties.Resources.CharTable);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Bayonetta *.mes Text Dumper/Creator by gdkchan");
            Console.WriteLine("Version 0.5.1");
            Console.WriteLine(string.Empty);
            Console.ResetColor();

            if (args.Length == 0)
                PrintUsage();
            else
            {
                string Operation = args[0];
                string FileName = null;

                if (args.Length == 2)
                {
                    FileName = args[1];
                }
                else if (args.Length == 3)
                {
                    switch (args[1])
                    {
                        case "-ps3":
                            processor.Plat = Platform.PS3;
                            break;
                        case "-x360":
                            processor.Plat = Platform.X360;
                            break;
                        case "-pc":
                            processor.Plat = Platform.PC;
                            break;
                        case "-wiiu":
                            processor.Plat = Platform.WiiU;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error: Invalid platform specified!");
                            Console.WriteLine(string.Empty);
                            PrintUsage();
                            return;
                    }
                    FileName = args[2];
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Invalid number of arguments!");
                    Console.WriteLine(string.Empty);
                    PrintUsage();
                    return;
                }

                switch (Operation)
                {
                    case "-d":
                        if (FileName == "-all")
                        {
                            string[] Files = Directory.GetFiles(Environment.CurrentDirectory);
                            foreach (string File in Files) if (Path.GetExtension(File).ToLower() == ".mes")
                            {
                               processor.Dump(File);
                            }
                        }
                        else
                        {
                            processor.Dump(FileName);
                        }

                        break;
                    case "-c":
                        if (FileName == "-all")
                        {
                            string[] Folders = Directory.GetDirectories(Environment.CurrentDirectory);
                            foreach (string Folder in Folders)
                            {
                                processor.Create(Folder);
                            }
                        }
                        else
                        {
                            processor.Create(FileName);
                        }

                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Invalid operation specified!");
                        Console.WriteLine(string.Empty);
                        PrintUsage();
                        break;
                }
            }
        }

        static void PrintUsage()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Usage:");
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("MESTool.exe [operation] [platform] [file|-all]");
            Console.WriteLine(string.Empty);

            Console.WriteLine("[operation]");
            Console.WriteLine("-d  Dumps a *.mes file to a folder");
            Console.WriteLine("-c  Creates a *.mes file from a folder");
            Console.WriteLine(string.Empty);

            Console.WriteLine("[platform]");
            Console.WriteLine("-ps3  For the PS3 version of the game (default)");
            Console.WriteLine("-x360  For the Xbox 360 version of the game");
            Console.WriteLine("-pc  For the PC version of the game");
            Console.WriteLine("-wiiu  For the Wii U version (partial)");
            Console.WriteLine(string.Empty);

            Console.WriteLine("-all  Manipulate all the files on the work directory");
            Console.WriteLine(string.Empty);

            Console.WriteLine("Example:");
            Console.WriteLine("MESTool -d file.mes");
            Console.WriteLine("MESTool -d -all");
            Console.WriteLine("MESTool -c folder");
            Console.WriteLine("MESTool -c -all");
            Console.ResetColor();
        }

    }
    */
}
