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
    public class IniSection
    {
        public string name;
        public Dictionary<string, string> entries;
        public IniSection(string _name)
        {
            entries = new Dictionary<string, string>();
            name = _name;
        }
        public void addLine(string l)
        {
            int p = l.IndexOf('=');
            string name = l.Substring(0, p).Trim();
            if (!entries.ContainsKey(name))
                entries.Add(name, l);
        }
        public void merge(IniSection s)
        {
            foreach (String name in s.entries.Keys)
            {
                if (name == "extrusion_multiplier" || name == "filament_diameter" || name=="first_layer_temperature"
                    || name =="temperature")
                {
                    if (entries.ContainsKey(name))
                    {
                        string full = s.entries[name];
                        int p = full.IndexOf('=');
                        if (p >= 0)
                            full = full.Substring(p + 1).Trim();
                        entries[name] += "," + full;
                    }
                    else
                    {
                        entries.Add(name, s.entries[name]);
                    }
                }
            }
        }

        public void replaceValue(string targetName, string replacementString)
        {
            if (entries.ContainsKey(targetName))
                this.entries[targetName] = replacementString;
            else
                addLine(replacementString);
        }
    }
    public class IniFile
    {
        public string path = "";
        public Dictionary<string, IniSection> sections = new Dictionary<string, IniSection>();
        public void read(string _path)
        {
            if (_path != null)
                path = _path;

            IniSection actSect = null;
            actSect = new IniSection("");
            sections.Add("", actSect);
            if (!File.Exists(path)) return;
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            foreach (string line in lines)
            {
                string tl = line.Trim();
                if (tl.StartsWith("#")) continue; // comment
                if (tl.StartsWith("[") && tl.EndsWith("]"))
                {
                    string secname = tl.Substring(1, tl.Length - 2);
                    actSect = sections[secname];
                    if (actSect == null)
                    {
                        actSect = new IniSection(secname);
                        sections.Add(secname, actSect);
                    }
                    continue;
                }
                int p = tl.IndexOf('=');
                if (p < 0) continue;
                actSect.addLine(line);
            }
        }
        public void add(IniFile f)
        {
            foreach (IniSection s in f.sections.Values)
            {
                if (!sections.ContainsKey(s.name))
                {
                    sections.Add(s.name, new IniSection(s.name));
                }
                IniSection ms = sections[s.name];
                foreach (string ent in s.entries.Values)
                    ms.addLine(ent);
            }
        }
        /// <summary>
        /// Merges the values of both ini files by seperating values by a ,
        /// </summary>
        /// <param name="f"></param>
        public void merge(IniFile f)
        {
            foreach (IniSection s in f.sections.Values)
            {
                if (!sections.ContainsKey(s.name))
                {
                    sections.Add(s.name, new IniSection(s.name));
                }
                else
                {
                    sections[s.name].merge(s);
                }
                /*IniSection ms = sections[s.name];
                foreach (string ent in s.entries.Values)
                    ms.addLine(ent);*/
            }
        }
        public void flatten()
        {
            IniSection flat = sections[""];
            LinkedList<IniSection> dellist = new LinkedList<IniSection>();
            foreach (IniSection s in sections.Values)
            {
                if (s.name == "") continue;
                foreach (string line in s.entries.Values)
                    flat.addLine(line);
                dellist.AddLast(s);
            }
            foreach (IniSection s in dellist)
                sections.Remove(s.name);
        }
        public void write(string path)
        {
            LinkedList<string> lines = new LinkedList<string>();
            foreach (IniSection s in sections.Values)
            {
                if (s.name != "")
                    lines.AddLast("[" + s.name + "]");
                foreach (string line in s.entries.Values)
                    lines.AddLast(line);
            }
            File.WriteAllLines(path, lines.ToArray());
        }

        internal void AddSupportandRaft()
        {
            if (Main.main.slicerPanel.generateSupportCheckbox.Checked == true)
                addSupportMaterial();

            if (Main.main.slicerPanel.generateRaftCheckbox.Checked == true)
                addRaft();
            // throw new NotImplementedException();
        }

        private void addRaft()
        {
            foreach (IniSection s in sections.Values)
            {
                s.replaceValue("raft_layers", "raft_layers = 5");              
            }

        }
        
        private void addSupportMaterial()
        {

            foreach (IniSection s in sections.Values)
            {
                s.replaceValue("support_material",                  "support_material = 1");
                s.replaceValue("support_material_angle",            "support_material_angle = 0");
                s.replaceValue("support_material_enforce_layers",   "support_material_enforce_layers = 2");
                s.replaceValue("support_material_extruder",         "support_material_extruder = 1");
                s.replaceValue("support_material_extrusion_width",  "support_material_extrusion_width = 0");
                s.replaceValue("support_material_interface_layers", "support_material_interface_layers = 2");
                s.replaceValue("support_material_interface_spacing","support_material_interface_spacing = 0");
                s.replaceValue("support_material_pattern",          "support_material_pattern = rectilinear-grid");
                s.replaceValue("support_material_spacing",          "support_material_spacing = 2.5");
                s.replaceValue("support_material_speed",            "support_material_speed = 60");
                s.replaceValue("support_material_threshold",        "support_material_threshold = 0");
            }

        }

        /// <summary>
        /// Inserts the custom Baoyan start G-code with the correctly calibrated Z height which is abtained form the printer settings. 
        /// </summary>
        internal void CalibrateHeight()
        {
            foreach (IniSection s in sections.Values)
            {
                string replace = String.Format(
                     @"start_gcode = M92 E380\nM140 S110\nM109 S230\nM190 S80\nG21 ;set units to mm\nG90 ;set to absolute positioning\n;M80\nM107\nG92 E0 ;reset extruder \nG28 X0 Y0 Z0\nG92 Z{0}\nG1 Z0.2 F400\nG1 X20 E4 F100\nG1 Y3 E6 F100\nG1 X0 E10\nM140 S110\nG92 E0",
                       Main.printerSettings.textPrintAreaHeight.Text);

                s.replaceValue("start_gcode", replace);               
            }
        }
    }
}
