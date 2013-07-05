/*
   Copyright 2011 repetier repetierdev@gmail.com

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using RepetierHost.model;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Forms;

namespace RepetierHost.model
{
    /// <summary>
    /// Allows for customization of the software if it reads a custom.ini while starting up. 
    /// Sets the Main Registry key value to "Baoyan"
    /// Most of this class is not used. 
    /// </summary>
    public class Custom
    {
        private static RegistryKey baseKey;
        private static Dictionary<string,string> dic;
        public static void Initialize()
        {
            dic = new Dictionary<string, string>();
            dic["registryFolder"] = "Baoyan";
            string customfile = Application.StartupPath + Path.DirectorySeparatorChar+"data"+Path.DirectorySeparatorChar+"custom.ini";
            if(File.Exists(customfile))
                ReadFile(customfile);
            // Creates a registry key in the CurrentUser\\SOFTWARE\\Baoyan Folder. Creates or reads the key. 
            baseKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\"+dic["registryFolder"]);

        }
        public static void ReadFile(string fname)
        {
            string line;
            System.IO.StreamReader file =
               new System.IO.StreamReader(fname);
            while ((line = file.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.StartsWith(";")) continue;
                int s = line.IndexOf('=');
                if (s < 0) continue;
                string key = line.Substring(0, s).Trim();
                string value = line.Substring(s + 1).Trim();
                if (value.Length >= 2 && value.StartsWith("\"") && value.EndsWith("\""))
                    value = value.Substring(1, value.Length - 2);
                dic[key] = value;
            }

            file.Close();
        }

        /// <summary>
        /// Returns the requested value that is designed by the "string name"
        /// </summary>
        /// <param name="name">Value to get</param>
        /// <param name="def">If fail to get value, then this is the default</param>
        /// <returns></returns>
        public static bool GetBool(string name, bool def)
        {
            if (!dic.ContainsKey(name)) return def;
            string val = dic[name];
            if (val == "1" || val == "yes" || val == "true") return true;
            if (val == "0" || val == "no" || val == "false") return false;
            return def;
        }
        public static int GetInteger(string name, int def)
        {
            if (!dic.ContainsKey(name)) return def;
            string val = dic[name];
            int ival = def;
            int.TryParse(val, out ival);
            return ival;
        }
        public static string GetString(string name, string def)
        {
            if (!dic.ContainsKey(name)) return def;
            return dic[name];
        }
        public static RegistryKey BaseKey
        {
            get { return baseKey; }
        }
    }
}
