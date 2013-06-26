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
    public class FileAddOrRemove
    {
        Main main;
        private bool writeSTLBinary = true;
        public ThreeDView stleditorView;
        private bool autosizeFailed = false;
        private CopyObjectsDialog copyDialog = new CopyObjectsDialog();

        public  FileAddOrRemove(Main main1)
        {
            this.main = main1;
            try
            {
                //main.threedview
                stleditorView = new ThreeDView();
            }
            catch { }


             if (Main.main != null)
                {
                    Main.main.languageChanged += translate;
                    translate();
                }
        }

        /// <summary>
        /// Allows either G-code or .stl file to be added. 
        /// </summary>
        /// <param name="file"></param>
        public void LoadGCodeOrSTL(string file)
        {
            if (!File.Exists(file)) return;
            FileInfo f = new FileInfo(file);
            this.main.Title = f.Name;
            this.main.fileHistory.Save(file);
            this.UpdateHistory();
            if (file.ToLower().EndsWith(".stl"))
            {
                 openAndAddObject(file);
                 this.main.current3Dview = Main.ThreeDViewOptions.STLeditor;
            }
            else
            {  

                this.main.current3Dview = RepetierHost.Main.ThreeDViewOptions.gcode;
                //this.main.update3DviewSelection();
                //tab.SelectTab(tabGCode);
                this.main.editor.selectContent(0);
                this.main.editor.setContent(0, System.IO.File.ReadAllText(file));
            }
            //changeSelectionBoxSize();
            Main.main.mainHelp.UpdateEverythingInMain();
        }

        public void LoadGCode(string file)
        {
            try
            {
                 this.main.current3Dview = RepetierHost.Main.ThreeDViewOptions.gcode;
                
                //tab.SelectTab(tabGCode);
                this.main.editor.selectContent(0);
                this.main.editor.setContent(0, System.IO.File.ReadAllText(file));
                this.main.mainHelp.UpdateEverythingInMain();
                //this.main.update3DviewSelection();
                
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

        public void LoadGCodeText(string text)
        {
            try
            {
                this.main.current3Dview = Main.ThreeDViewOptions.gcode;
                this.main.editor.setContent(0, text);
                //tab.SelectTab(tabGCode);
                this.main.editor.selectContent(0);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), Trans.T("L_ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void AddAFile()
        {
            if (this.main.openFileSTLorGcode.ShowDialog() == DialogResult.OK)
            {
                foreach (string fname in this.main.openFileSTLorGcode.FileNames)
                    openAndAddObject(fname);
            }
            //changeSelectionBoxSize();
            main.mainHelp.UpdateEverythingInMain();
        }

       

        public void openAndAddObject(string file)
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
            //else
            //{
            //    main.listSTLObjects.Visible = false;
            //}
               
                   
        }

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
                if (stl.outside) itemsOutide = true;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistoryHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            RegMemory.HistoryFile f = (RegMemory.HistoryFile)clickedItem.Tag;
            this.LoadGCodeOrSTL(f.file);
            // Take some action based on the data in clickedItem
        }

        public void removeObject()
        {

            LinkedList<STL> list = new LinkedList<STL>();
            foreach (STL stl in this.main.listSTLObjects.SelectedItems)
                list.AddLast(stl);
            foreach (STL stl in list)
            {
                stleditorView.models.Remove(stl);
                this.main.listSTLObjects.Items.Remove(stl);
                autosizeFailed = false; // Reset autoposition
            }
            list.Clear();
            if (this.main.listSTLObjects.Items.Count > 0)
                this.main.listSTLObjects.SelectedIndex = 0;
           // Update is called from the parent caller. 
            Main.main.mainHelp.UpdateEverythingInMain();

        }

       

        //public void LoadGCodeOrSTL(string file)
        //{
        //    //if (!File.Exists(file)) return;
        //    //FileInfo f = new FileInfo(file);
        //    //Title = f.Name;
        //    //fileHistory.Save(file);
        //    //UpdateHistory();
        //    //if (file.ToLower().EndsWith(".stl"))
        //    //{
        //    //    /*  if (MessageBox.Show("Do you want to slice the STL-File? No adds it to the object grid.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //    //      {
        //    //          slicer.RunSlice(file); // Slice it and load
        //    //      }
        //    //      else
        //    //      {*/
        //    //    //tab.SelectTab(tabModel);
        //    //    stlComposer1.openAndAddObject(file);
        //    //    //}
        //    //}
        //    //else
        //    //{
        //    //    //tab.SelectTab(tabGCode);
        //    //    editor.selectContent(0);
        //    //    editor.setContent(0, System.IO.File.ReadAllText(file));
        //    //}
        //}


        
        //////////////////////////
        //*****************************************
        //*****************************************
        //---------------------------------------

 
       /// <summary>
       /// Translate the tooltip text and the text of the objects. 
       /// </summary>
        public void translate()
        {
            // TODO: Fix this translateion
            //labelTranslation.Text = Trans.T("L_TRANSLATION:");
            //labelScale.Text = Trans.T("L_SCALE:");
            //labelRotate.Text = Trans.T("L_ROTATE:");
            //labelSTLObjects.Text = Trans.T("L_STL_OBJECTS");
            //buttonSave.Text = Trans.T("B_SAVE_AS_STL");
            //buttonRemoveSTL.Text = Trans.T("B_REMOVE_STL_OBJECT");
            //buttonAddSTL.Text = Trans.T("B_ADD_STL_OBJECT");
            //buttonAutoplace.Text = Trans.T("B_AUTOPOSITION");
            //buttonLand.Text = Trans.T("B_DROP_OBJECT");
            //buttonCopyObjects.Text = Trans.T("B_COPY_OBJECTS");
            //buttonCenter.Text = Trans.T("B_CENTER_OBJECT");
            //checkScaleAll.Text = Trans.T("L_LOCK_ASPECT_RATIO");
            //if (Main.slicer != null)
            //    buttonSlice.Text = Trans.T1("L_SLICE_WITH", Main.slicer.SlicerName);
        }

  
        /// <summary>
        /// Removes an STL file. 
        /// TODO: Link to the method in the main. 
        /// </summary>
        public void buttonRemoveSTL_Click()
        {
            //STL stl = (STL)listSTLObjects.SelectedItem;
            //if (stl == null) return;
            LinkedList<STL> list = new LinkedList<STL>();
            foreach (STL stl in this.main.listSTLObjects.SelectedItems)
                list.AddLast(stl);
            foreach (STL stl in list)
            {
                stleditorView.models.Remove(stl);
                this.main.listSTLObjects.Items.Remove(stl);
                autosizeFailed = false; // Reset autoposition
            }
            list.Clear();
            if (this.main.listSTLObjects.Items.Count > 0)
                this.main.listSTLObjects.SelectedIndex = 0;
            Main.main.threedview.UpdateChanges();
        }

        /// <summary>
        /// Save the combined or modified .stl file
        /// TODO: Link to the method in the main 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void buttonSave_Click(object sender, EventArgs e)
        //{
        //    if (saveSTL.ShowDialog() == DialogResult.OK)
        //    {
        //        saveComposition(saveSTL.FileName);
        //    }
        //}
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
        /// <param name="fname"></param>
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

    

      
      


        static bool inRecheckFiles = false;
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
    

        
    
    public class EnglishStreamWriter : StreamWriter
    {
        public EnglishStreamWriter(Stream path)
            : base(path, Encoding.ASCII)
        {
        }
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



    


