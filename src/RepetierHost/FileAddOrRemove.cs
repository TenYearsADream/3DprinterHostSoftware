///-----------------------------------------------------------------------
/// <copyright file="FileAddOrRemove.cs" company="Baoyan">
///   Some parts of this file were derived from Repetier Host which can be found at
/// https://github.com/repetier/Repetier-Host Which is licensed using the Apache 2.0 license. 
/// Other parts of the file are property of Baoyan Automation LTC, Nanjing Jiangsu China
/// </copyright>
///-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using RepetierHost.model;
using RepetierHost.view;
using RepetierHost.view.utils;

namespace RepetierHost
{
    /// <summary>
    /// This class aids in adding and removing files. 
    /// </summary>
    public class FileAddOrRemove
    {
        /// <summary>
        /// Reference back to Main. 
        /// </summary>
        Main main;

        /// <summary>
        /// Determines whether we should write the ".stl" file as a binary or not. 
        /// </summary>
        private bool writeSTLBinary = true;

        /// <summary>
        /// The 3D view for the ".stl" editor and manipulator. TODO: This should probably be in the Main. 
        /// </summary>
        public ThreeDView stleditorView;

        // private CopyObjectsDialog copyDialog = new CopyObjectsDialog();

        /// <summary>
        /// Initializes a new instance of the FileAddOrRemove class which helps with adding and removing stl and gcode files. 
        /// </summary>
        /// <param name="main1">References the Main. </param>
        public FileAddOrRemove(Main main1)
        {
            this.main = main1;
            try
            {
                //main.threedview
                stleditorView = new ThreeDView();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Allows either G-code or .stl file to be added. 
        /// </summary>
        /// <param name="file">The File to Load</param>
        public void LoadGCodeOrSTL(string file)
        {
            if (!File.Exists(file))
            {
                return;
            }

            FileInfo f = new FileInfo(file);
            this.main.Title = f.Name;
            this.main.fileHistory.Save(file);
            this.UpdateHistory();
            if (file.ToLower().EndsWith(".stl"))
            {
                OpenAndAddSTLFile(file);
                this.main.current3Dview = Main.ThreeDViewOptions.STLeditor;
            }
            else
            {
                this.main.current3Dview = RepetierHost.Main.ThreeDViewOptions.gcode;
                this.main.editor.selectContent(0);
                this.main.editor.setContent(0, System.IO.File.ReadAllText(file));
            }

            //// changeSelectionBoxSize(); TODO: Think this referes to the box around the .stl object. This is still an error. 
            Main.main.mainHelp.UpdateEverythingInMain();
        }

        /// <summary>
        /// Loads a Gcode file and sets the mode to g-code visualization. 
        /// </summary>
        /// <param name="file">File Path to Load</param>
        public void LoadGCode(string file)
        {
            try
            {
                this.main.editor.setContent(0, System.IO.File.ReadAllText(file));
                this.main.current3Dview = RepetierHost.Main.ThreeDViewOptions.gcode;
                this.main.editor.selectContent(0);
                this.main.mainHelp.UpdateEverythingInMain();
            }
            catch (System.IO.FileNotFoundException)
            {
                GCodeNotFound.execute(file);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), Trans.T("L_ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads a G-code text and sets the mode to g-code visualization. 
        /// </summary>
        /// <param name="text">The actual text of the g-code. </param>
        public void LoadGCodeText(string text)
        {
            try
            {
                this.main.current3Dview = Main.ThreeDViewOptions.gcode;
                this.main.editor.setContent(0, text);
               this.main.editor.selectContent(0);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), Trans.T("L_ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Allows the user to select the stl or gcode file to add to the program. 
        /// </summary>
        public void AddAFile()
        {
            if (this.main.openFileSTLorGcode.ShowDialog() == DialogResult.OK)
            {
                foreach (string fname in this.main.openFileSTLorGcode.FileNames)
                {
                    OpenAndAddSTLFile(fname);
                }
            }
            //// changeSelectionBoxSize(); Update the box around the object?? TODO: might need to fix this. 
            main.mainHelp.UpdateEverythingInMain();
        }

        /// <summary>
        /// Opens the .stl file, sets the view to stl editor, and causes the model to animate a fall onto the print platform. 
        /// </summary>
        /// <param name="file">File Path to the stl file.</param>
        public void OpenAndAddSTLFile(string file)
        {
            STL stl = new STL();
            stl.Load(file);
            stl.Center(Main.printerSettings.PrintAreaWidth / 2, Main.printerSettings.PrintAreaDepth / 2);
            stl.Land();
            if (stl.list.Count > 0)
            {
                this.main.listSTLObjects.Items.Add(stl);
                stleditorView.models.AddLast(stl);
                this.main.listSTLObjects.SelectedItem = stl;
                main.postionGUI.Autoposition();
                stl.addAnimation(new DropAnimation("drop"));
                main.postionGUI.updateSTLState(stl);
            }
            else
            {
                main.listSTLObjects.Visible = false;
            }
        }

        /// <summary>
        /// Causes the current stl models to be sliced using the active slicer. A High level Method. 
        /// </summary>
        public void Slice()
        {
            string dir = Main.globalSettings.Workdir;
            if (!Directory.Exists(dir))
            {
                MessageBox.Show(Trans.T("L_EXISTING_WORKDIR_REQUIRED"), Trans.T("L_ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                Main.globalSettings.Show();
                return;
            }

            if (main.listSTLObjects.Items.Count == 0) return;
            bool itemsOutide = false;
            foreach (STL stl in main.listSTLObjects.Items)
            {
                if (stl.outside) 
                    itemsOutide = true;
            }

            if (itemsOutide)
            {
                if (MessageBox.Show(Trans.T("L_OBJECTS_OUTSIDE_SLICE_QUEST"), Trans.T("L_WARNING"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    return;
            }

            string t = main.listSTLObjects.Items[0].ToString();
            if (main.listSTLObjects.Items.Count > 1)
                t += " + " + (main.listSTLObjects.Items.Count - 1).ToString();
            Main.main.Title = t;
            dir += Path.DirectorySeparatorChar + "composition.stl";
            saveComposition(dir);
            Main.slicer.RunSlice(dir); // Slice it and load
        }

        /// <summary>
        /// Updates the recent file history in the fileToolStripMenu to include the 
        /// </summary>
        public void UpdateHistory()
        {
            bool delFlag = false;
            LinkedList<ToolStripItem> delArray = new LinkedList<ToolStripItem>();
            int pos = 0;
            foreach (ToolStripItem c in main.fileToolStripMenuItem.DropDownItems)
            {
                if (c == main.toolStripEndHistory) break;
                if (!delFlag) pos++;
                if (c == main.toolStripStartHistory)
                {
                    delFlag = true;
                    continue;
                }

                if (delFlag)
                    delArray.AddLast(c);
            }

            foreach (ToolStripItem i in delArray)
                main.fileToolStripMenuItem.DropDownItems.Remove(i);
            main.importSTLToolSplitButton1.DropDownItems.Clear();
            foreach (RegMemory.HistoryFile f in main.fileHistory.list)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(); // You would obviously calculate this value at runtime
                item = new ToolStripMenuItem();
                item.Name = "file" + pos;
                item.Tag = f;
                item.Text = f.ToString();
                item.Click += new EventHandler(HistoryHandler);
                this.main.fileToolStripMenuItem.DropDownItems.Insert(pos++, item);
                item = new ToolStripMenuItem();
                item.Name = "filet" + pos;
                item.Tag = f;
                item.Text = f.ToString();
                item.Click += new EventHandler(HistoryHandler);
                this.main.importSTLToolSplitButton1.DropDownItems.Add(item);
            }
        }

        /// <summary>
        /// When clicked on a history item in the file menu, load that file. First we must recall which item it actually is from the history. 
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Args</param>
        private void HistoryHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            RegMemory.HistoryFile f = (RegMemory.HistoryFile)clickedItem.Tag;
            this.LoadGCodeOrSTL(f.file);
         }

        /// <summary>
        /// Removes the selected stl object(s) from the printing platform. 
        /// </summary>
        public void RemoveSTLObject()
        {
            LinkedList<STL> list = new LinkedList<STL>();
            foreach (STL stl in this.main.listSTLObjects.SelectedItems)
                list.AddLast(stl);
            foreach (STL stl in list)
            {
                stleditorView.models.Remove(stl);
                this.main.listSTLObjects.Items.Remove(stl);
                Main.main.postionGUI.autosizeFailed = false; // Reset autoposition
            }

            list.Clear();
            if (this.main.listSTLObjects.Items.Count > 0)
                this.main.listSTLObjects.SelectedIndex = 0;
            else
                this.main.current3Dview = Main.ThreeDViewOptions.loadAFile; // if there are no more in the list, then go to load a file mode. 

            Main.main.mainHelp.UpdateEverythingInMain();
        }

        /// <summary>
        /// Removes an STL file. 
        /// TODO: Link to the method in the main. 
        /// </summary>
        public void buttonRemoveSTL_Click()
        {
            LinkedList<STL> list = new LinkedList<STL>();
            foreach (STL stl in this.main.listSTLObjects.SelectedItems)
                list.AddLast(stl);
            foreach (STL stl in list)
            {
                stleditorView.models.Remove(stl);
                this.main.listSTLObjects.Items.Remove(stl);
                Main.main.postionGUI.autosizeFailed = false; // Reset autoposition
            }
            list.Clear();
            if (this.main.listSTLObjects.Items.Count > 0)
                this.main.listSTLObjects.SelectedIndex = 0;
            Main.main.threedview.UpdateChanges();
        }

        /// <summary>
        /// Determines whether the given vector coordinate values are valid. (Not 0 and not infinity)
        /// </summary>
        /// <param name="v">Vector whose values to check</param>
        /// <returns>true if the vector is ok. Otherwise false</returns>
        private bool AssertVector3NotNaN(Vector3 v)
        {
            if (float.IsNaN(v.X) || float.IsNaN(v.Y) || float.IsNaN(v.Z))
            {
                // Main.conn.log("NaN value in STL file export", false, 2);
                return false;
            }
            if (float.IsInfinity(v.X) || float.IsInfinity(v.Y) || float.IsInfinity(v.Z))
            {
                // Main.conn.log("Infinity value in STL file export", false, 2);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Determines if the two vectors have a meaningful distance from each other. 
        /// (Checks to see if they are so close that they might be the same value)
        /// </summary>
        /// <param name="a">First Vector</param>
        /// <param name="b">Second Vector</param>
        /// <returns>true if they are not too close. False if they are too close</returns>
        private bool AssertMinDistance(Vector3 a, Vector3 b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            double dz = a.Z - b.Z;
            return dx * dx + dy * dy + dz * dz > 1e-8;
        }

        /// <summary>
        /// Create a combined .stl file from all the .stl files that are on the current printer platform. 
        /// </summary>
        /// <param name="fname">File path and name of the combined composition to save</param>
        public void saveComposition(string fname)
        {
            int n = 0;
            foreach (STL stl in this.main.listSTLObjects.Items)
            {
                n += stl.list.Count;
            }

            STLTriangle[] triList2 = new STLTriangle[n];
            int p = 0;
            foreach (STL stl in this.main.listSTLObjects.Items)
            {
                stl.UpdateMatrix();
                foreach (STLTriangle t2 in stl.list)
                {
                    STLTriangle t = new STLTriangle();
                    t.p1 = new Vector3();
                    t.p2 = new Vector3();
                    t.p3 = new Vector3();
                    t.normal = new Vector3();
                    stl.TransformPoint(ref t2.p1, out t.p1.X, out t.p1.Y, out t.p1.Z);
                    stl.TransformPoint(ref t2.p2, out t.p2.X, out t.p2.Y, out t.p2.Z);
                    stl.TransformPoint(ref t2.p3, out t.p3.X, out t.p3.Y, out t.p3.Z);

                    // Compute normal from p1-p3
                    float ax = t.p2.X - t.p1.X;
                    float ay = t.p2.Y - t.p1.Y;
                    float az = t.p2.Z - t.p1.Z;
                    float bx = t.p3.X - t.p1.X;
                    float by = t.p3.Y - t.p1.Y;
                    float bz = t.p3.Z - t.p1.Z;
                    t.normal.X = ay * bz - az * by;
                    t.normal.Y = az * bx - ax * bz;
                    t.normal.Z = ax * by - ay * bx;
                    Vector3.Normalize(ref t.normal, out t.normal);
                    if (AssertVector3NotNaN(t.normal) && AssertVector3NotNaN(t.p1) && AssertVector3NotNaN(t.p2) &&
                        AssertVector3NotNaN(t.p3) &&
                        AssertMinDistance(t.p1, t.p2) && AssertMinDistance(t.p1, t.p3) && AssertMinDistance(t.p2, t.p3))
                    {
                        triList2[p++] = t;
                    }
                }
            }

            n = p;
            STLTriangle[] triList = new STLTriangle[n];
            for (int i = 0; i < n; i++)
                triList[i] = triList2[i];

            // STL should have increasing z for faster slicing
            Array.Sort<STLTriangle>(triList, triList[0]);

            // Write file in binary STL format
            FileStream fs = File.Open(fname, FileMode.Create);
            if (writeSTLBinary)
            {
                BinaryWriter w = new BinaryWriter(fs);
                int i;
                for (i = 0; i < 20; i++) w.Write((int)0);
                w.Write(n);
                for (i = 0; i < n; i++)
                {
                    STLTriangle t = triList[i];
                    w.Write(t.normal.X);
                    w.Write(t.normal.Y);
                    w.Write(t.normal.Z);
                    w.Write(t.p1.X);
                    w.Write(t.p1.Y);
                    w.Write(t.p1.Z);
                    w.Write(t.p2.X);
                    w.Write(t.p2.Y);
                    w.Write(t.p2.Z);
                    w.Write(t.p3.X);
                    w.Write(t.p3.Y);
                    w.Write(t.p3.Z);
                    w.Write((short)0);
                }

                w.Close();
            }
            else
            {
                TextWriter w = new EnglishStreamWriter(fs);
                w.WriteLine("solid RepetierHost");
                for (int i = 0; i < n; i++)
                {
                    STLTriangle t = triList[i];
                    w.Write("  facet normal ");
                    w.Write(t.normal.X);
                    w.Write(" ");
                    w.Write(t.normal.Y);
                    w.Write(" ");
                    w.WriteLine(t.normal.Z);
                    w.WriteLine("    outer loop");
                    w.Write("      vertex ");
                    w.Write(t.p1.X);
                    w.Write(" ");
                    w.Write(t.p1.Y);
                    w.Write(" ");
                    w.WriteLine(t.p1.Z);
                    w.Write("      vertex ");
                    w.Write(t.p2.X);
                    w.Write(" ");
                    w.Write(t.p2.Y);
                    w.Write(" ");
                    w.WriteLine(t.p2.Z);
                    w.Write("      vertex ");
                    w.Write(t.p3.X);
                    w.Write(" ");
                    w.Write(t.p3.Y);
                    w.Write(" ");
                    w.WriteLine(t.p3.Z);
                    w.WriteLine("    endloop");
                    w.WriteLine("  endfacet");
                }

                w.WriteLine("endsolid RepetierHost");
                w.Close();
            }

            fs.Close();
        }



        /// <summary>
        /// Tells if the files have already been checked. 
        /// </summary>
        static bool inRecheckFiles = false;

        /// <summary>
        /// Checks if the stl files on the printing platform have changed. Maybe they have been edited by an outside program. 
        /// </summary>
        public void recheckChangedFiles()
        {
            if (inRecheckFiles) return;
            inRecheckFiles = true;
            bool changed = false;
            foreach (STL stl in this.main.listSTLObjects.Items)
            {
                if (stl.changedOnDisk())
                {
                    changed = true;
                    break;
                }
            }
            if (changed)
            {
                if (MessageBox.Show("One or more objects files are changed.\r\nReload objects?", "Files changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (STL stl in this.main.listSTLObjects.Items)
                    {
                        if (stl.changedOnDisk())
                            stl.reload();
                    }
                    Main.main.threedview.UpdateChanges();
                }
                else
                {
                    foreach (STL stl in this.main.listSTLObjects.Items)
                    {
                        if (stl.changedOnDisk())
                            stl.resetModifiedDate();
                    }
                }
            }
            inRecheckFiles = false;
        }

        /// <summary>
        /// Saves the current composition on the print platform if the "Crtl S" combination is pressed
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        public void saveAFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                if (Main.main.current3Dview == Main.ThreeDViewOptions.gcode)
                    StoreCode.Execute();

                if (Main.main.current3Dview == Main.ThreeDViewOptions.STLeditor)
                {
                    if (Main.main.saveSTL.ShowDialog() == DialogResult.OK)
                    {
                        this.saveComposition(Main.main.saveSTL.FileName);
                    }
                }

                e.Handled = true;
            }

        }

        /// <summary>
        /// Class related to reading Files in the country specific format?? Not sure this is used. 
        /// </summary>
        public class EnglishStreamWriter : StreamWriter
        {
            /// <summary>
            /// Initializes a new instance of the EnglishStreamWriter class
            /// </summary>
            /// <param name="path">The path to the stream to read??</param>
            public EnglishStreamWriter(Stream path)
                : base(path, Encoding.ASCII)
            {
            }

            /// <summary>
            /// Overrides the default format field with the country or language specific field. 
            /// </summary>
            public override IFormatProvider FormatProvider
            {
                get
                {
                    return System.Globalization.CultureInfo.InvariantCulture;
                }
            }
        }
    }
}



    


