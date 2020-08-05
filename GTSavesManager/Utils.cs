using GTSavesManager.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GTSavesManager
{
    class Utils
    {
        public static string getOrigSavePath()
        {
            return Path.Combine(Settings.Default.savesFolder, "savedGame.gt");
        }
        public static string getOrigGlobalSavePath()
        {
            return Path.Combine(Settings.Default.savesFolder, "globalSave.gt");
        }

        public static bool isGTRunning()
        {
            foreach(Process process in Process.GetProcesses())
            {
                if (process.ProcessName.Equals("Golden Treasure - The Great Green")) return true;
            }
            return false;
        }
        public static string EmptyToNull(string a)
        {
            if(string.IsNullOrWhiteSpace(a)) return null;
            else return a;
        }
    }
}
