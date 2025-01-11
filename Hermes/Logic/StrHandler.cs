using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hermes.Utility;

namespace Hermes.Logic
{
    internal class StrHandler
    {
        readonly List<string> localizedText = new();

        /// <summary>
        /// Puts all the text in a list that we can later write out in a .txt file
        /// </summary>
        /// <param name="file"> File to read from </param>
        public void AddLocalizedText(string file)
        {
            string[] lines = File.ReadAllLines(file);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].ToLower().Trim(); // Trim it here, to remove any whitespace before the word reference
                if (line.Contains("reference"))
                {
                    line = line[9..].Trim(); // Trim again for safety
                    localizedText.Add(Path.GetFileNameWithoutExtension(file).ToUpper() + "_" + line.ToUpper()); // This is how 3arc formats their localized strings
                }
            }
        }

        /// <summary>
        /// Writes a new string file with the correct language key
        /// </summary>
        /// <param name="file"> File to replace </param>
        /// <param name="path"> Directory to put the file </param>
        /// <param name="language"> Which language we need to support </param>
        public static void WriteNewStrFile(string file, string path, string language)
        {
            string[] lines = File.ReadAllLines(file);
            List<string> newLines = new();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                // Replace the keyword of the language to the correct one. This is technically not necessary, but it doesn't hurt for the sake of consistensy
                if (line.Contains("LANG_"))
                {
                    string[] currentLanguage = line.Split('"');
                    line = line.Replace(currentLanguage[0].Trim(), "LANG_" + language.ToUpper()); // Trim it here, so we don't replace the whitespaces
                }

                newLines.Add(line);
            }

            using FileStream txt = new(Path.Combine(path, file), FileMode.OpenOrCreate, FileAccess.Write);
            using StreamWriter streamTxt = new(txt);
            foreach (string newLine in newLines)
            {
                streamTxt.WriteLine(newLine);
            }
        }

        /// <summary>
        /// Copies all the files to the correct language file path
        /// </summary>
        /// <param name="fullPath"> The full path of the file (including the file) </param>
        /// <param name="path"> The directory of the file </param>
        /// <param name="file"> The file to copy </param>
        public static void CopyLanguagesToPaths(string fullPath, string path, string file)
        {
            string[] languages =
            {
                "english",
                "englisharabic",
                "french",
                "german",
                "italian",
                "japanese",
                "polish",
                "portuguese",
                "russian",
                "simplifiedchinese",
                "spanish",
                "traditionalchinese"
            };

            bool shouldOverrideFile = false;

            foreach (string lang in languages)
            {
                string langPath, combinedPath;

                if (path.Contains("localizedstrings")) // path.ToLower().EndsWith("localizedstrings")
                {
                    combinedPath = Location.GetLocalizationPath(path, languages, lang);
                    // combinedPath = Path.Combine(path, "..", "..", lang, "localizedstrings"); // Hardcoding the path isn't ideal, but subdirs are (mostly) pointless in localizedstrings regardless. Move to recursive method to fix this?
                }
                else
                {
                    combinedPath = Path.Combine(Location.GetExeDirectory(), "Output", lang, "localizedstrings"); // If the str file is not coming from localizedstrings, create an output folder in the exe dir
                }

                langPath = Path.GetFullPath(combinedPath);
                Directory.CreateDirectory(langPath);
                string strFile = Path.Combine(langPath, file);

                if (!shouldOverrideFile && File.Exists(strFile))
                {
                    shouldOverrideFile = true;
                    CLI.WaitForUserConfirmation("File already exists. Press any key to continue. This will override any existing .str file");
                }

                WriteNewStrFile(file, langPath, lang); // Potentially always write a new file anyways to replace it with the proper language
            }
        }

        /// <summary>
        /// Ensures the text file is created for the precache text
        /// </summary>
        public void MakePrecacheTxt()
        {
            string file = Path.GetFullPath(Path.Combine(Location.GetExeDirectory(), "precaches.txt"));

            using FileStream txt = new(file, FileMode.OpenOrCreate, FileAccess.Write);
            using StreamWriter streamTxt = new(txt);
            streamTxt.Write("Please be reminded to change \"string\" to \"triggerstring\" if it's for (uni)triggers. \n\n");

            // Write out all the necessary precaches
            foreach (string localized in localizedText)
            {
                streamTxt.WriteLine($"#precache(\"string\", \"{localized}\");");
            }
        }

    }
}
