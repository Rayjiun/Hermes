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
    }
}
