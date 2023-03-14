using System.IO;
using System.Numerics;
using System.Reflection;
using System.Text;
using Hermes.Logic;
using Hermes.Utility;

namespace Hermes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---------------------------------------------------------------------------------------------------");
            Console.WriteLine(" Hermes, a drag and drop tool for BO3 to automatically support all languages for localized strings. ");
            Console.WriteLine(" Version: 1.2.0");
            Console.WriteLine("---------------------------------------------------------------------------------------------------");
            Console.WriteLine("");

            if (args.Length == 0)
            {
                Hermes.Utility.CLI.ErrorMessage("* No files were specified.");
                Hermes.Utility.CLI.WaitForUserConfirmation();
                return;
            }

            StrHandler handler = new();
            foreach (var arg in args)
            {
                string file = Path.GetFileName(arg);
                
                if (!File.Exists(file))
                {
                    Hermes.Utility.CLI.ErrorMessage("* File does not exist");
                    continue;
                }

                if (Path.GetExtension(file).ToLower() != ".str")
                {
                    Hermes.Utility.CLI.ErrorMessage($"* {file} has the wrong extension.");
                    continue;
                }

                string? path = Path.GetDirectoryName(arg);
                if (path == null)
                {
                    Hermes.Utility.CLI.ErrorMessage("* Path is null");
                    continue;
                }

                Hermes.Utility.CLI.WaitMessage($"> Copying {file}");

                handler.AddLocalizedText(file);
                StrHandler.CopyLanguagesToPaths(arg, path, file);

                Hermes.Utility.CLI.SuccessMessage($"! Completed copying {file}");
            }

            handler.MakePrecacheTxt();
            Hermes.Utility.CLI.NeutralMessage("\nAll files copied, please check out precaches.txt located in your exe directory.");
            Hermes.Utility.CLI.WaitForUserConfirmation();
        }
    }

}