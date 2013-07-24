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
using System.IO;

namespace RepetierHost.model
{
    /// <summary>
    /// Helps manipulate the configuration files for skeinforge by reading, modifiiying, and writing them. 
    /// TODO: Currently it uses a array of strings in the "line" field. However a dictionary would work better. It kind of reinvents the wheel right now. 
    /// </summary>
    public class SkeinConfig
    {
        /// <summary>
        /// All the lines that make up the configuration file
        /// </summary>
        string[] lines;

        /// <summary>
        /// A copy of "lines[]" that holds the unmodified orignal
        /// </summary>
        string[] orig;

        /// <summary>
        /// Path to the configuration file (including the file name)
        /// </summary>
        string path;

        /// <summary>
        /// Shows if the file exists. True if it exists, otherwise false. 
        /// </summary>
        bool exists;

        /// <summary>
        /// Initializes a new instance of the SkeinConfig class. Check if the file exists and readsa all the lines. 
        /// </summary>
        /// <param name="_path"></param>
        public SkeinConfig(string _path)
        {
            path = _path;
            exists = File.Exists(path);
            if (!exists)
            {
                return;
            }

            lines = File.ReadAllLines(path);
            orig = (string[])lines.Clone();
        }

        /// <summary>
        /// Writes all of the lines to the file. We assume that the lines have modified, but they may have not been modified
        /// Uses the same file path/name used during initialization
        /// </summary>
        public void writeModified()
        {
            if (!exists)
            {
                return;
            }

            File.WriteAllLines(path, lines);
        }

        /// <summary>
        /// Writes the original file back to its original location. 
        /// Uses the same file path/name used during initialization
        /// </summary>
        public void writeOriginal()
        {
            if (!exists)
            {
                return;
            }

            File.WriteAllLines(path, orig);
        }

        /// <summary>
        /// Finds the line number on which a string "key" exists. If the key is not found then return -1
        /// The format should be : key (TAB) value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int lineForKey(string key)
        {
            key += "\t";
            for (int i = 0; i < lines.Count(); i++)
            {
                if(lines[i].StartsWith(key)) return i;
            }

            return -1;
        }

        /// <summary>
        /// Gets the value associated with a key. REturn Null if key is now found
        /// The format should be : key (TAB) value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string getValue(string key)
        {
            if (!exists)
            {
                return null;
            }

            int idx = lineForKey(key);
            if (idx < 0)
            {
                return null;
            }

            return lines[idx].Substring(key.Length + 1);
        }

        /// <summary>
        /// Sets the value associated with a key.  REturn Null if key is now found
        ///  The format should be : key (TAB) value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void setValue(string key, string val)
        {
            if (!exists)
            {
                return;
            }

            int idx = lineForKey(key);
            if (idx < 0)
            {
                return;
            }

            lines[idx] = key + "\t" + val;
        }
    }
}
