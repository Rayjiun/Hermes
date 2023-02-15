using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hermes
{
    internal class StrHandler
    {
        List<string> localizedText = new List<string>();

        // Puts all the text in a list that we can later write out in a .txt file
        public void AddLocalizedText(string file)
        {
            string[] lines = File.ReadAllLines(file);

            for(int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].ToLower().Trim(); // Trim it here, to remove any whitespace before the word reference
                if(line.Contains("reference"))
                {
                    line = line[9..].Trim(); // Trim again for safety
                    localizedText.Add(Path.GetFileNameWithoutExtension(file).ToUpper() + "_" + line.ToUpper()); // This is how 3arc formats their localized strings
                }
            }
        }

        // Copies all the files to the correct language file path
        public void CopyLanguagesToPaths(string fullPath, string path, string file)
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

            foreach (string lang in languages)
            {
                // TODO: Add it so if the path doesn't end with localizedstrings to not go back 2 directories and instead use the exe directory
                string langPath = Path.GetFullPath(Path.Combine(path, "..", "..", lang, "localizedstrings")); // Hardcoding the path isn't ideal, but subdirs are (mostly) pointless in localizedstrings regardless.
                Directory.CreateDirectory(langPath);

                string strFile = Path.Combine(langPath, file);
                if (File.Exists(strFile))
                {
                    continue;
                }

                File.Copy(fullPath, Path.Combine(langPath, file));
            }
        }

        // Ensures the text file is created for the precache text
        public void MakePrecacheTxt()
        {
            string file = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "precaches.txt"));

            using (FileStream txt = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
            using (StreamWriter streamTxt = new StreamWriter(txt))
            {
                streamTxt.Write("Please be reminded to change \"string\" to \"triggerstring\" if it's for (uni)triggers. \n\n");

                // Write out all the necessary precaches
                foreach (string localized in localizedText)
                {
                    streamTxt.WriteLine($"#precache(\"string\", \"{localized}\");");
                }
            }
        }

    }
}
