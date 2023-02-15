using System.Reflection;
using System.Text;

namespace Hermes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---------------------------------------------------------------------------------------------------");
            Console.WriteLine("Hermes, a drag and drop tool for BO3 to automatically support all languages for localized strings. ");
            Console.WriteLine("Version: 1.1.0");
            Console.WriteLine("---------------------------------------------------------------------------------------------------");
            Console.WriteLine("");

            if (args.Length == 0)
            {
                Console.WriteLine("No files were specified. \nPress enter to exit.");
                Console.ReadLine();
                return;
            }

            StrHandler handler = new StrHandler();
            foreach (var arg in args)
            {
                string file = Path.GetFileName(arg);
                
                if (!File.Exists(file))
                {
                    Console.WriteLine("File does not exist.");
                    continue;
                }

                if (Path.GetExtension(file).ToLower() != ".str")
                {
                    Console.WriteLine($"{file} has the wrong extension.");
                    continue;
                }

                string path = Path.GetDirectoryName(arg);
                if (path == null)
                {
                    Console.WriteLine("Path is null.");
                    continue;
                }

                Console.WriteLine($"> Copying {file}");

                handler.AddLocalizedText(file);
                handler.CopyLanguagesToPaths(arg, path, file);

                Console.WriteLine($"! Completed copying {file}");
            }

            handler.MakePrecacheTxt();

            Console.WriteLine("\nAll files copied, please check out precaches.txt located in your exe directory. \nPress enter to exit.");
            Console.ReadLine();
        }
    }

}