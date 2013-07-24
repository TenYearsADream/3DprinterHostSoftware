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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using RepetierHost.view.utils;
using RepetierHost.model;

namespace RepetierHost.view
{
    public partial class Skeinforge : Form
    {
        public RegistryKey repetierKey;
        Process procSkein = null;
        Process procConvert = null;
        string slicefile = null;
        SkeinConfig profileConfig = null;
        SkeinConfig exportConfig = null;
        SkeinConfig extrusionConfig = null;
        SkeinConfig multiplyConfig = null;
        SkeinConfig raftAndSupportConfig = null;
        string name = "Skeinforge";

        public Skeinforge()
        {
            InitializeComponent();
            RegMemory.RestoreWindowPos("skeinforgeWindow", this);
            repetierKey = Custom.BaseKey; // Registry.CurrentUser.CreateSubKey("SOFTWARE\\Repetier");
            regToForm();
            translate();
            if (BasicConfiguration.basicConf.SkeinforgeProfileDir.IndexOf("sfact") >= 0)
                name = "SFACT";
            else
                name = "Skeinforge";
            Main.main.languageChanged += translate;
        }
        private void translate()
        {
            Text = Trans.T("W_SKEIN_SETTINGS");
            labelApplication.Text = Trans.T("L_SKEIN_APPLICARTION");
            labelCraft.Text = Trans.T("L_SKEIN_CRAFT");
            labelProfdirInfo.Text = Trans.T("L_SKEIN_PROFDIR_INFO");
            labelProfilesDirectory.Text = Trans.T("L_SKEIN_PROFILES_DIRECTORY");
            labelPypy.Text = Trans.T("L_SKEIN_PYPY");
            labelPypyInfo.Text = Trans.T("L_SKEIN_PYPY_INFO");
            labelPython.Text = Trans.T("L_SKEIN_PYTHON");
            labelWorkdirInfo.Text = Trans.T("L_SKEIN_WORKDIR_INFO");
            labelWorkingDirectory.Text = Trans.T("L_SKEIN_WORKING_DIRECTORY");
            openFile.Title = Trans.T("L_SKEIN_OPEN_FILE");
            openPython.Title = Trans.T("L_SKEIN_OPEN_PYTHON");
            buttonAbort.Text = Trans.T("B_CANCEL");
            buttonOK.Text = Trans.T("B_OK");
            buttonSearchCraft.Text = Trans.T("B_BROWSE");
            buttonSerach.Text = Trans.T("B_BROWSE");
            buttonSerachPy.Text = Trans.T("B_BROWSE");
            buttonBrosePyPy.Text = Trans.T("B_BROWSE");
            buttonBrowseProfilesDir.Text = Trans.T("B_BROWSE");
            buttonBrowseWorkingDirectory.Text = Trans.T("B_BROWSE");
            
        }
        public string wrapQuotes(string text)
        {
            if (text.StartsWith("\"") && text.EndsWith("\"")) return text;
            return "\"" + text.Replace("\"", "\\\"") + "\"";
        }
        public void RestoreConfigs()
        {
            if (profileConfig != null)
                profileConfig.writeOriginal();
            if (exportConfig != null)
                exportConfig.writeOriginal();
            if (extrusionConfig != null)
                extrusionConfig.writeOriginal();
            if (multiplyConfig != null)
                multiplyConfig.writeOriginal();
            profileConfig = null;
            exportConfig = null;
            extrusionConfig = null;
            multiplyConfig = null;
        }
        public void RunSkeinforge()
        {
            if (procSkein != null)
            {
                return;
            }
            string python = findPythonw();
            if (python == null)
            {
                MessageBox.Show(Trans.T("L_PYTHON_NOT_FOUND"), Trans.T("L_ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string sk = findSkeinforge();
            if (sk == null)
            {
                MessageBox.Show(Trans.T("L_SKEINFORGE_NOT_FOUND"), Trans.T("L_ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            procSkein = new Process();
            try
            {
                procSkein.EnableRaisingEvents = true;
                procSkein.Exited += new EventHandler(SkeinExited);
                procSkein.StartInfo.FileName = Main.IsMono ? python : wrapQuotes(python);
                procSkein.StartInfo.Arguments = wrapQuotes(sk);
                procSkein.StartInfo.WorkingDirectory = textWorkingDirectory.Text;
                procSkein.StartInfo.UseShellExecute = false;
                procSkein.StartInfo.RedirectStandardOutput = true;
                procSkein.OutputDataReceived += new DataReceivedEventHandler(OutputDataHandler);
                procSkein.StartInfo.RedirectStandardError = true;
                procSkein.ErrorDataReceived += new DataReceivedEventHandler(OutputDataHandler);
                procSkein.Start();
                // Start the asynchronous read of the standard output stream.
                procSkein.BeginOutputReadLine();
                procSkein.BeginErrorReadLine();
            }
            catch (Exception e)
            {
                Main.connection.log(e.ToString(), false, 2);
            }
        }
        public void KillSlice()
        {
            if (procConvert != null)
            {
                procConvert.Kill();
                procConvert = null;
                Main.connection.log(Trans.T1("L_SKEIN_KILLED",name),false,2); //"Skeinforge slicing process killed on user request.", false, 2);
                RestoreConfigs();
            }
        }
        public string findSkeinforgeProfiles()
        {
            if (Directory.Exists(textProfilesDir.Text))
                return textProfilesDir.Text;
            string test = ((Environment.OSVersion.Platform == PlatformID.Unix ||
                   Environment.OSVersion.Platform == PlatformID.MacOSX)
    ? Environment.GetEnvironmentVariable("HOME")
    : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%")) + Path.DirectorySeparatorChar + ".skeinforge"+Path.DirectorySeparatorChar+"profiles";
            if (Directory.Exists(test)) return test;
            return null;
        }
        public string findPypy()
        {
            if (File.Exists(textPypy.Text)) // use preconfigured
                return textPypy.Text;
            string[] possibleNames = { "pypy.exe", "pypy"};
            if (textPypy.Text.Length > 1)
            {
                if(File.Exists(textPypy.Text))
                    return textPypy.Text;
            }
            // Search in PATH environment var
            foreach (string test in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(Path.PathSeparator))
            {
                string path = test.Trim();
                foreach (string exname in possibleNames) // Search bundled version
                {
                    if (!String.IsNullOrEmpty(path) && File.Exists(Path.Combine(path, exname)))
                        return Path.GetFullPath(Path.Combine(path, exname));
                }
            }
            string[] possibleNames2 = { "python.exe", "python2","python"};
            foreach (string test in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(Path.PathSeparator))
            {
                string path = test.Trim();
                foreach (string exname in possibleNames2) // Search bundled version
                {
                    if (!String.IsNullOrEmpty(path) && File.Exists(Path.Combine(path, exname)))
                        return Path.GetFullPath(Path.Combine(path, exname));
                }
            }

            return findPythonw();
        }
        public string findPythonw()
        {
            if (File.Exists(textPython.Text)) // use preconfigured
                return textPython.Text;
            string[] possibleNames = { "pythonw.exe", "python2","python" };
            if (textPypy.Text.Length > 1)
            {
                if (File.Exists(textPypy.Text))
                    return textPypy.Text;
            }
            // Search in PATH environment var
            foreach (string test in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(Path.PathSeparator))
            {
                string path = test.Trim();
                foreach (string exname in possibleNames) // Search bundled version
                {
                    if (!String.IsNullOrEmpty(path) && File.Exists(Path.Combine(path, exname)))
                        return Path.GetFullPath(Path.Combine(path, exname));
                }
            }
            return null;
        }
        public string findCraft()
        {
            if(textSkeinforgeCraft.Text.Length>1 && File.Exists(textSkeinforgeCraft.Text)) return textSkeinforgeCraft.Text;
            if (File.Exists("/usr/lib/python2.7/site-packages/skeinforge/skeinforge_application/skeinforge_utilities/skeinforge_craft.py"))
                return "/usr/lib/python2.7/site-packages/skeinforge/skeinforge_application/skeinforge_utilities/skeinforge_craft.py";
            return null;
        }
        public string findSkeinforge()
        {
            if (textSkeinforge.Text.Length > 1 && File.Exists(textSkeinforge.Text)) return textSkeinforge.Text;
            if (File.Exists("/usr/lib/python2.7/site-packages/skeinforge/skeinforge_application/skeinforge.py"))
                return "/usr/lib/python2.7/site-packages/skeinforge/skeinforge_application/skeinforge.py";
            return null;
        }
        public string PyPy
        {
            get
            {
                return findPypy();
            }
        }
        public void RunSlice(string file, string profile)
        {
            if (procConvert != null)
            {
                MessageBox.Show(Trans.T("L_SKEIN_STILL_RUNNING") /*"Last slice job still running. Slicing of new job is canceled."*/,Trans.T("L_ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string py = PyPy;
            if (py == null)
            {
                MessageBox.Show(Trans.T("L_PYPY_NOT_FOUND"), Trans.T("L_ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string craft = findCraft();
            if (craft == null)
            {
                MessageBox.Show(Trans.T("L_SKEINCRAFT_NOT_FOUND"), Trans.T("L_ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string profdir = findSkeinforgeProfiles();
            if (profdir == null)
            {
                MessageBox.Show(Trans.T("L_SKEINCRAFT_PROFILES_NOT_FOUND"), Trans.T("L_ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            profileConfig = new SkeinConfig(Path.Combine(profdir,"skeinforge_profile.csv"));
            extrusionConfig = new SkeinConfig(Path.Combine(profdir,"extrusion.csv"));
            exportConfig = new SkeinConfig(Path.Combine(profdir,"extrusion" +   Path.DirectorySeparatorChar + profile + Path.DirectorySeparatorChar + "export.csv"));
            multiplyConfig = new SkeinConfig(Path.Combine(profdir,"extrusion" +
                Path.DirectorySeparatorChar + profile + Path.DirectorySeparatorChar + "multiply.csv"));
            raftAndSupportConfig = new SkeinConfig(Path.Combine(profdir, "extrusion" +
                Path.DirectorySeparatorChar + profile + Path.DirectorySeparatorChar + "raft.csv"));
            // Set profile to extrusion
            /* cutting	False
extrusion	True
milling	False
winding	False
*/
            profileConfig.setValue("cutting", "False");
            profileConfig.setValue("milling", "False");
            profileConfig.setValue("extrusion", "True");
            profileConfig.setValue("winding", "False");
                profileConfig.writeModified();
            // Set used profile
            extrusionConfig.setValue("Profile Selection:", profile);
            extrusionConfig.writeModified();
            // Set export to correct values
            exportConfig.setValue("Activate Export", "True");
            exportConfig.setValue("Add Profile Extension", "False");
            exportConfig.setValue("Add Profile Name to Filename", "False");
            exportConfig.setValue("Add Timestamp Extension", "False");
            exportConfig.setValue("Add Timestamp to Filename", "False");
            exportConfig.setValue("Add Description to Filename", "False");
            exportConfig.setValue("Add Descriptive Extension", "False");
            exportConfig.writeModified();

            multiplyConfig.setValue("Activate Multiply:", "False");
            multiplyConfig.setValue("Activate Multiply: ", "False");
            multiplyConfig.setValue("Activate Multiply", "False");
            multiplyConfig.writeModified();

            string target = StlToGCode(file);
            if (File.Exists(target))
            {
                File.Delete(target);
            }

            // Modify Start Code, Raft, and Support settings to reflect the current user settings
            CalibrateHeightStartGcode(profdir);
            AddRaftConfiguration(raftAndSupportConfig);
            AddSupportConfiguration(raftAndSupportConfig);
            raftAndSupportConfig.writeModified(); // write the modified raft.csv 

            procConvert = new Process();
            try
            {
                SlicingInfo.Start(name);
                SlicingInfo.SetAction(Trans.T("L_SLICING_STL_FILE...")); //"Slicing STL file ...");
                slicefile = file;
                procConvert.EnableRaisingEvents = true;
                procConvert.Exited += new EventHandler(ConversionExited);

                procConvert.StartInfo.FileName = Main.IsMono ? py : wrapQuotes(py);
                procConvert.StartInfo.Arguments = wrapQuotes(craft) + " " + wrapQuotes(file);
                procConvert.StartInfo.UseShellExecute = false;
                procConvert.StartInfo.WorkingDirectory = textWorkingDirectory.Text;
                procConvert.StartInfo.RedirectStandardOutput = true;
                procConvert.OutputDataReceived += new DataReceivedEventHandler(OutputDataHandler);
                procConvert.StartInfo.RedirectStandardError = true;
                procConvert.ErrorDataReceived += new DataReceivedEventHandler(OutputDataHandler);
                procConvert.Start();
                // Start the asynchronous read of the standard output stream.
                procConvert.BeginOutputReadLine();
                procConvert.BeginErrorReadLine();
                //Main.main.tab.SelectedTab = Main.main.tabPrint;
            }
            catch (Exception e)
            {
                Main.connection.log(e.ToString(), false, 2);
                RestoreConfigs();
            }
        }

        /// <summary>
        /// Adds the support settings to true of false to the configRaftSuppor SkeinConfig object. 
        /// </summary>
        /// <param name="configRaftSupport"></param>
        private void AddSupportConfiguration(SkeinConfig configRaftSupport)
        {
            if (Main.main.slicerPanel.generateSupportCheckbox.Checked == true)
            {
                configRaftSupport.setValue("None", "False");
                configRaftSupport.setValue("Empty Layers Only", "False");
                configRaftSupport.setValue("Exterior Only", "False");
                configRaftSupport.setValue("Everywhere", "True");
                
            }
            else
            {
                configRaftSupport.setValue("None", "True");
                configRaftSupport.setValue("Everywhere", "False");
                configRaftSupport.setValue("Empty Layers Only", "False");
                configRaftSupport.setValue("Exterior Only", "False");

            }
            
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the Raft configuration to true or false based on the main printer settings. Modifies the SkienConfig object. 
        /// </summary>
        /// <param name="configRaftSupport"></param>
        private void AddRaftConfiguration( SkeinConfig configRaftSupport)
        {
            if (Main.main.slicerPanel.generateRaftCheckbox.Checked == true)
            {
                configRaftSupport.setValue("Activate Raft", "True");
                configRaftSupport.setValue("Add Raft, Elevate Nozzle, Orbit:", "True");
                configRaftSupport.setValue("Base Layers (integer):", "5");
                configRaftSupport.setValue("Interface Layers (integer):", "3");    

            }
            else
            {
                // These must be true, to allow support generation, but we will set the raft to 0 to remove the raft. 
                configRaftSupport.setValue("Activate Raft", "True");
                configRaftSupport.setValue("Add Raft, Elevate Nozzle, Orbit:", "True");
                configRaftSupport.setValue("Base Layers (integer):", "0");
                configRaftSupport.setValue("Interface Layers (integer):", "0");    
                ////configRaftSupport.setValue("Activate Raft", "False");
                ////configRaftSupport.setValue("Add Raft, Elevate Nozzle, Orbit:", "False"); 

            }

           // throw new NotImplementedException();
        }

        /// <summary>
        /// Totally replaces the C:\Users\USERNAME\.skeinforge\alterations\start.gcode with the correct start G-code or Baoyan Automation
        /// The important change it he custom height that was set during calibration. 
        /// </summary>
        private void CalibrateHeightStartGcode(string profileDirectory)
        {
            string startGcode = @"  M92 E92

M109 S230
M190 S60
G21           ;set units to mm
G90           ;set to absolute positioning
;M80
M107
G92 E0		;reset extruder 
G28 X0 Y0 Z0
G92 Z{0}
G1 Z0.2 F400
G1 X20 E4 F100
G1 Y3 E6 F100
G1 X0 E10
G92 E0  
M140 S90";

            // Make the substitution
            string newGocdeSTart = String.Format(startGcode, Main.printerSettings.textPrintAreaHeight.Text);

            // Find the path to the start.gocde
            string path = profileDirectory + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "alterations" + Path.DirectorySeparatorChar + "start.gcode";
           
            // Write the new text document start.gcode
            System.IO.File.WriteAllText(path, newGocdeSTart);

       
           // throw new NotImplementedException();
        }


        public delegate void LoadGCode(String myString);
        private void ConversionExited(object sender, System.EventArgs e)
        {
            if (procConvert == null) return;
            try
            {
                procConvert.Close();
                procConvert = null;
                string gcodefile = StlToGCode(slicefile);
                Main.slicer.Postprocess(gcodefile);
                RestoreConfigs();
            }
            catch { }
        }
        private void SkeinExited(object sender, System.EventArgs e)
        {
            procSkein.Close();
            procSkein = null;
            //Main.main.Invoke(Main.main.slicerPanel.UpdateSelectionInvoker);
        }
        private static void OutputDataHandler(object sendingProcess,
            DataReceivedEventArgs outLine)
        {
            // Collect the net view command output.
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                string[] lines = outLine.Data.Split((char)0x0d);
                foreach (string l in lines)
                    Main.connection.log("<"+Main.main.skeinforge.name+"> " + l, false, 4);
            }
        }

        public string StlToGCode(string stl)
        {
            int p = stl.LastIndexOf('.');
            if (p > 0) stl = stl.Substring(0, p);
            string extension = exportConfig.getValue("File Extension:");
            if (extension == null)
                extension = exportConfig.getValue("File Extension (gcode):");
            string export = exportConfig.getValue("Add Export Suffix");
            if (export == null)
                export = exportConfig.getValue("Add _export to filename (filename_export)");
            if (export == null || export != "True") export = ""; else export = "_export";
            return stl + export + "." + extension;
        }
        private void regToForm()
        {

            textSkeinforge.Text = (string)repetierKey.GetValue("SkeinforgePath", textSkeinforge.Text);
            textSkeinforgeCraft.Text = (string)repetierKey.GetValue("SkeinforgeCraftPath", textSkeinforgeCraft.Text);
            textPython.Text = (string)repetierKey.GetValue("SkeinforgePython", textPython.Text);
            textPypy.Text = (string)repetierKey.GetValue("SkeinforgePypy", textPypy.Text);
            //textExtension.Text = (string)repetierKey.GetValue("SkeinforgeExtension", textExtension.Text);
            //textPostfix.Text = (string)repetierKey.GetValue("SkeinforgePostfix", textPostfix.Text);
            textWorkingDirectory.Text = (string)repetierKey.GetValue("SkeinforgeWorkdir", textWorkingDirectory.Text);
            textProfilesDir.Text = BasicConfiguration.basicConf.SkeinforgeProfileDir;
        }
        private void FormToReg()
        {
            BasicConfiguration.basicConf.SkeinforgeProfileDir = textProfilesDir.Text;
            repetierKey.SetValue("SkeinforgePath", textSkeinforge.Text);
            repetierKey.SetValue("SkeinforgeCraftPath", textSkeinforgeCraft.Text);
            repetierKey.SetValue("SkeinforgePython", textPython.Text);
            repetierKey.SetValue("SkeinforgePypy", textPypy.Text);
            //repetierKey.SetValue("SkeinforgeExtension", textExtension.Text);
            //repetierKey.SetValue("SkeinforgePostfix", textPostfix.Text);
            repetierKey.SetValue("SkeinforgeWorkdir", textWorkingDirectory.Text);
        }
        private void buttonAbort_Click(object sender, EventArgs e)
        {
            regToForm();
            Hide();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            FormToReg();
            Hide();
            if (BasicConfiguration.basicConf.SkeinforgeProfileDir.IndexOf("sfact") >= 0)
                name = "SFACT";
            else
                name = "Skeinforge";
            Main.main.languageChanged += translate;
            Main.slicer.Update();
            //Main.main.slicerPanel.UpdateSelection();
        }

        private void buttonSerach_Click(object sender, EventArgs e)
        {
            openFile.Title = Trans.T("L_SKEIN_OPEN_FILE");
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                textSkeinforge.Text = openFile.FileName;
            }
        }

        private void buttonSearchCraft_Click(object sender, EventArgs e)
        {
            openFile.Title = Trans.T("L_SKEIN_OPEN_CRAFT");
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                textSkeinforgeCraft.Text = openFile.FileName;
            }
        }

        private void buttonSerachPy_Click(object sender, EventArgs e)
        {
            openPython.Title = Trans.T("L_SKEIN_OPEN_PYTHON");
            if (openPython.ShowDialog() == DialogResult.OK)
                textPython.Text = openPython.FileName;
        }

        private void Skeinforge_FormClosing(object sender, FormClosingEventArgs e)
        {
            RegMemory.StoreWindowPos("skeinforgeWindow", this, false, false);
        }

        private void buttonBrowseWorkingDirectory_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.Description = Trans.T("L_SKEIN_SELECT_WORKING_FOLDER");
            folderBrowserDialog.SelectedPath = textWorkingDirectory.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textWorkingDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void buttonBrowseProfilesDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.Description = Trans.T("L_SKEIN_SELECT_PROFILE_FOLDER");
            folderBrowserDialog.SelectedPath = textProfilesDir.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textProfilesDir.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void buttonBrosePyPy_Click(object sender, EventArgs e)
        {
            openPython.Title = Trans.T("L_SKEIN_OPEN_PYPY");
            if (openPython.ShowDialog() == DialogResult.OK)
                textPypy.Text = openPython.FileName;
        }
    }
}
