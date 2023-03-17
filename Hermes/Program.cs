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
            Console.WriteLine(" Version: 1.3.1");
            Console.WriteLine("---------------------------------------------------------------------------------------------------");
            Console.WriteLine("");

            if (args.Length == 0)
            {
                CLI.ErrorMessage("* No files were specified.");
                CLI.WaitForUserConfirmation();
                return;
            }

            StrHandler handler = new();
            foreach (var arg in args)
            {
                string file = Path.GetFileName(arg);
                
                if (!File.Exists(file))
                {
                    CLI.ErrorMessage("* File does not exist");
                    continue;
                }

                if (Path.GetExtension(file).ToLower() != ".str")
                {
                    CLI.ErrorMessage($"* {file} has the wrong extension.");
                    continue;
                }

                string? path = Path.GetDirectoryName(arg);
                if (path == null)
                {
                    CLI.ErrorMessage("* Path is null");
                    continue;
                }

                CLI.WaitMessage($"> Copying {file}");

                handler.AddLocalizedText(file);
                StrHandler.CopyLanguagesToPaths(arg, path, file);

                CLI.SuccessMessage($"! Completed copying {file}");
            }

            handler.MakePrecacheTxt();
            CLI.NeutralMessage("\nAll files copied, please check out precaches.txt located in your exe directory.");
            CLI.WaitForUserConfirmation();
        }
    }

}