using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.Utility
{
    internal static class Location
    {
        /// <summary>
        /// Gets the exe directory of the program
        /// </summary>
        /// <returns> Returns exe directory </returns>
        public static string GetExeDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
        }

        /// <summary>
        /// Returns the correct path for localization languages
        /// </summary>
        /// <param name="path"> Current path to replace </param>
        /// <param name="languages"> Array of all available languages </param>
        /// <param name="currentLang"> What to replace the language in the path with </param>
        /// <returns></returns>
        public static string GetLocalizationPath(string path, string[] languages, string currentLang)
        {
            foreach (string lang in languages)
            {
                if (path.Contains(lang)) 
                {
                    return path.Replace(lang, currentLang);
                }
            }

            return path;
        }
    }
}
