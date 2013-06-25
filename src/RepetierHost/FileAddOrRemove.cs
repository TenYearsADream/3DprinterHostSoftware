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
            this.main.UpdateHistory();
            if (file.ToLower().EndsWith(".stl"))
            {
                 openAndAddObject(file);                
            }
            else
            {  

                this.main.current3Dview = RepetierHost.Main.ThreeDViewOptions.gcode;
                //this.main.update3DviewSelection();
                //tab.SelectTab(tabGCode);
                this.main.editor.selectContent(0);
                this.main.editor.setContent(0, System.IO.File.ReadAllText(file));
            }
            changeSelectionBoxSize();
        }

        public void LoadGCode(string file)
        {
            try
            {
                 this.main.current3Dview = RepetierHost.Main.ThreeDViewOptions.gcode;
                
                //tab.SelectTab(tabGCode);
                this.main.editor.selectContent(0);
                this.main.editor.setContent(0, System.IO.File.ReadAllText(file));
                this.main.update3DviewSelection();
                
                
                
                //this.main.editor.setContent(0, System.IO.File.ReadAllText(file));
                ////tab.SelectTab(tabGCode);

                //// TODO: The file update history should be in the fileAddOrRemove file and not the main. 
                //this.main.editor.selectContent(0);
                //this.main.fileHistory.Save(file);
                //this.main.UpdateHistory();
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
            changeSelectionBoxSize();
        }

        public void changeSelectionBoxSize()
        {
            if (main.listSTLObjects.Items.Count > 0 && main.current3Dview == Main.ThreeDViewOptions.STLeditor)
            {
                main.listSTLObjects.Visible = true;
                // = this.main.listSTLObjects.ItemHeight * (this.main.listSTLObjects.Items.Count+1);
                main.listSTLObjects.Height = main.listSTLObjects.PreferredHeight;
            }
            else
                main.listSTLObjects.Visible = false;
           // main.listSTLObjects.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);

            //main.listSTLObjects.Invalidate();
           main.listSTLObjects.Update();
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
                Autoposition();
                stl.addAnimation(new DropAnimation("drop"));
                updateSTLState(stl);
            }
            else
            {
                main.listSTLObjects.Visible = false;
            }
               
                   
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
            Main.main.threedview.UpdateChanges();
            changeSelectionBoxSize();

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
        /// Updates the openGl box. Calls a function in the main, so I'm not sure this is needed
        /// </summary>
        public void Update3D()
        {
            Main.main.threedview.UpdateChanges();
        }

        // TODO : reuse this function for validation of add validation to the move, and rotate numbers
        //private void float_Validating(object sender, CancelEventArgs e)
        //{
        //    TextBox box = (TextBox)sender;
        //    try
        //    {
        //        float.Parse(box.Text);
        //        errorProvider.SetError(box, "");
        //    }
        //    catch
        //    {
        //        errorProvider.SetError(box, "Not a number.");
        //    }
        //}


        // TODO: Only allow for moving and rotating of .stl objects when a object is selected. Reuse this function. 
        private void updateEnabled()
        {
        //    int n = listSTLObjects.SelectedItems.Count;
        //    if (n != 1)
        //    {
        //        textRotX.Enabled = false;
        //        textRotY.Enabled = false;
        //        textRotZ.Enabled = false;
        //        textScaleX.Enabled = false;
        //        textScaleY.Enabled = false;
        //        textScaleZ.Enabled = false;
        //        checkScaleAll.Enabled = false;
        //        textTransX.Enabled = false;
        //        textTransY.Enabled = false;
        //        textTransZ.Enabled = false;
        //        buttonCenter.Enabled = false;
        //        buttonAutoplace.Enabled = listSTLObjects.Items.Count > 1;
        //        buttonLand.Enabled = n > 0;
        //        if (Main.main.threedview != null)
        //            Main.main.threedview.SetObjectSelected(n > 0);
        //        buttonCopyObjects.Enabled = n > 0;
        //    }
        //    else
        //    {
        //        buttonAutoplace.Enabled = listSTLObjects.Items.Count > 1;
        //        buttonCopyObjects.Enabled = true;
        //        textRotX.Enabled = true;
        //        textRotY.Enabled = true;
        //        textRotZ.Enabled = true;
        //        textScaleX.Enabled = true;
        //        textScaleY.Enabled = !checkScaleAll.Checked;
        //        textScaleZ.Enabled = !checkScaleAll.Checked;
        //        checkScaleAll.Enabled = true;
        //        textTransX.Enabled = true;
        //        textTransY.Enabled = true;
        //        textTransZ.Enabled = true;
        //        buttonCenter.Enabled = true;
        //        buttonLand.Enabled = true;
        //        if (Main.main.threedview != null)
        //            Main.main.threedview.SetObjectSelected(true);
        //    }
        //    buttonRemoveSTL.Enabled = n != 0;
        //    buttonSlice.Enabled = listSTLObjects.Items.Count > 0;
        //    buttonSave.Enabled = listSTLObjects.Items.Count > 0;
        }

       
        
        /// <summary>
        /// Checks the state of the object.
        /// If it is outside print are it starts pulsing
        /// </summary>
        public void updateSTLState(STL stl)
        {
            FormPrinterSettings ps = Main.printerSettings;
            stl.UpdateBoundingBox();
            if (!ps.PointInside(stl.xMin, stl.yMin, stl.zMin) ||
                !ps.PointInside(stl.xMax, stl.yMin, stl.zMin) ||
                !ps.PointInside(stl.xMin, stl.yMax, stl.zMin) ||
                !ps.PointInside(stl.xMax, stl.yMax, stl.zMin) ||
                !ps.PointInside(stl.xMin, stl.yMin, stl.zMax) ||
                !ps.PointInside(stl.xMax, stl.yMin, stl.zMax) ||
                !ps.PointInside(stl.xMin, stl.yMax, stl.zMax) ||
                !ps.PointInside(stl.xMax, stl.yMax, stl.zMax))
            {
                stl.outside = true;
                if (Main.threeDSettings.pulseOutside.Checked && !stl.hasAnimationWithName("pulse"))
                    stl.addAnimation(new PulseAnimation("pulse", 0.03, 0.03, 0.03, 0.3));
            }
            else
            {
                stl.outside = false;
                stl.removeAnimationWithName("pulse");
            }
        }


        public void listSTLObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateEnabled();
            STL stl = (STL)this.main.listSTLObjects.SelectedItem;
            foreach (STL s in stleditorView.models)
            {
                s.Selected = this.main.listSTLObjects.SelectedItems.Contains(s);
            }
            if (this.main.listSTLObjects.SelectedItems.Count > 1) stl = null;
            if (stl != null)
            {
                // Todo, Update this with the new methods
                //textRotX.Text = stl.Rotation.x.ToString(GCode.format);
                //textRotY.Text = stl.Rotation.y.ToString(GCode.format);
                //textRotZ.Text = stl.Rotation.z.ToString(GCode.format);
                //textScaleX.Text = stl.Scale.x.ToString(GCode.format);
                //textScaleY.Text = stl.Scale.y.ToString(GCode.format);
                //textScaleZ.Text = stl.Scale.z.ToString(GCode.format);
                //textTransX.Text = stl.Position.x.ToString(GCode.format);
                //textTransY.Text = stl.Position.y.ToString(GCode.format);
                //textTransZ.Text = stl.Position.z.ToString(GCode.format);
                //checkScaleAll.Checked = (stl.Scale.x == stl.Scale.y && stl.Scale.x == stl.Scale.z);
            }
            Main.main.threedview.UpdateChanges();
        }

        // TODO: Update these with the new methods of rotating and moving
        //private void textTransX_TextChanged(object sender, EventArgs e)
        //{
        //    STL stl = (STL)listSTLObjects.SelectedItem;
        //    if (stl == null) return;
        //    float.TryParse(textTransX.Text, NumberStyles.Float, GCode.format, out stl.Position.x);
        //    updateSTLState(stl);
        //    Main.main.threedview.UpdateChanges();
        //}

        //private void textTransY_TextChanged(object sender, EventArgs e)
        //{
        //    STL stl = (STL)listSTLObjects.SelectedItem;
        //    if (stl == null) return;
        //    float.TryParse(textTransY.Text, NumberStyles.Float, GCode.format, out stl.Position.y);
        //    updateSTLState(stl);
        //    Main.main.threedview.UpdateChanges();
        //}

        //private void textTransZ_TextChanged(object sender, EventArgs e)
        //{
        //    STL stl = (STL)listSTLObjects.SelectedItem;
        //    if (stl == null) return;
        //    float.TryParse(textTransZ.Text, NumberStyles.Float, GCode.format, out stl.Position.z);
        //    updateSTLState(stl);
        //    Main.main.threedview.UpdateChanges();
        //}

        private void objectMoved(float dx, float dy)
        {
            //STL stl = (STL)listSTLObjects.SelectedItem;
            //if (stl == null) return;
            foreach (STL stl in this.main.listSTLObjects.SelectedItems)
            {
                stl.Position.x += dx;
                stl.Position.y += dy;
                if (this.main.listSTLObjects.SelectedItems.Count == 1)
                {
                    // TODO: Update this
                    //textTransX.Text = stl.Position.x.ToString(GCode.format);
                    //textTransY.Text = stl.Position.y.ToString(GCode.format);
                }
                updateSTLState(stl);
            }
            Main.main.threedview.UpdateChanges();
        }
        private void objectSelected(ThreeDModel sel)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                if (!sel.Selected)
                    this.main.listSTLObjects.SelectedItems.Add(sel);
            }
            else
                if (Control.ModifierKeys == Keys.Control)
                {
                    if (sel.Selected)
                        this.main.listSTLObjects.SelectedItems.Remove(sel);
                    else
                        this.main.listSTLObjects.SelectedItems.Add(sel);
                }
                else
                {
                    this.main.listSTLObjects.SelectedItems.Clear();
                    this.main.listSTLObjects.SelectedItem = sel;
                }
        }

        // TODO: Update these with the new methods. 
        //private void textScaleX_TextChanged(object sender, EventArgs e)
        //{
        //    STL stl = (STL)this.main.listSTLObjects.SelectedItem;
        //    if (stl == null) return;
        //    float.TryParse(textScaleX.Text, NumberStyles.Float, GCode.format, out stl.Scale.x);
        //    if (checkScaleAll.Checked)
        //    {
        //        stl.Scale.y = stl.Scale.z = stl.Scale.x;
        //        textScaleY.Text = stl.Scale.y.ToString(GCode.format);
        //        textScaleZ.Text = stl.Scale.z.ToString(GCode.format);
        //    }
        //    updateSTLState(stl);
        //    Main.main.threedview.UpdateChanges();
        //}

        //private void textScaleY_TextChanged(object sender, EventArgs e)
        //{
        //    STL stl = (STL)this.main.listSTLObjects.SelectedItem;
        //    if (stl == null) return;
        //    float.TryParse(textScaleY.Text, NumberStyles.Float, GCode.format, out stl.Scale.y);
        //    updateSTLState(stl);
        //    Main.main.threedview.UpdateChanges();
        //}

        //private void textScaleZ_TextChanged(object sender, EventArgs e)
        //{
        //    STL stl = (STL)this.main.listSTLObjects.SelectedItem;
        //    if (stl == null) return;
        //    float.TryParse(textScaleZ.Text, NumberStyles.Float, GCode.format, out stl.Scale.z);
        //    updateSTLState(stl);
        //    Main.main.threedview.UpdateChanges();
        //}

        //private void textRotX_TextChanged(object sender, EventArgs e)
        //{
        //    STL stl = (STL)this.main.listSTLObjects.SelectedItem;
        //    if (stl == null) return;
        //    float.TryParse(textRotX.Text, NumberStyles.Float, GCode.format, out stl.Rotation.x);
        //    updateSTLState(stl);
        //    Main.main.threedview.UpdateChanges();
        //}

        //private void textRotY_TextChanged(object sender, EventArgs e)
        //{
        //    STL stl = (STL)this.main.listSTLObjects.SelectedItem;
        //    if (stl == null) return;
        //    float.TryParse(textRotY.Text, NumberStyles.Float, GCode.format, out stl.Rotation.y);
        //    updateSTLState(stl);
        //    Main.main.threedview.UpdateChanges();
        //}

        //private void textRotZ_TextChanged(object sender, EventArgs e)
        //{
        //    STL stl = (STL)this.main.listSTLObjects.SelectedItem;
        //    if (stl == null) return;
        //    float.TryParse(textRotZ.Text, NumberStyles.Float, GCode.format, out stl.Rotation.z);
        //    updateSTLState(stl);
        //    Main.main.threedview.UpdateChanges();
        //}

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

        private void buttonLand_Click(object sender, EventArgs e)
        {
            //STL stl = (STL)listSTLObjects.SelectedItem;
            //if (stl == null) return;
            foreach (STL stl in this.main.listSTLObjects.SelectedItems)
            {
                stl.Land();
                listSTLObjects_SelectedIndexChanged(null, null);
            }
            Main.main.threedview.UpdateChanges();
        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
            //STL stl = (STL)listSTLObjects.SelectedItem;
            //if (stl == null) return;
            foreach (STL stl in this.main.listSTLObjects.SelectedItems)
            {
                stl.Center(Main.printerSettings.BedLeft + Main.printerSettings.PrintAreaWidth / 2, Main.printerSettings.BedFront + Main.printerSettings.PrintAreaDepth / 2);
                listSTLObjects_SelectedIndexChanged(null, null);

            }
            Main.main.threedview.UpdateChanges();
        }

        public void buttonSlice_Click(object sender, EventArgs e)
        {
            string dir = Main.globalSettings.Workdir;
            if (!Directory.Exists(dir))
            {
                MessageBox.Show(Trans.T("L_EXISTING_WORKDIR_REQUIRED"), Trans.T("L_ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                Main.globalSettings.Show();
                return;
            }
            if (this.main.listSTLObjects.Items.Count == 0) return;
            bool itemsOutide = false;
            foreach (STL stl in this.main.listSTLObjects.Items)
            {
                if (stl.outside) itemsOutide = true;
            }
            if (itemsOutide)
            {
                if (MessageBox.Show(Trans.T("L_OBJECTS_OUTSIDE_SLICE_QUEST"), Trans.T("L_WARNING"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    return;
            }
            string t = this.main.listSTLObjects.Items[0].ToString();
            if (this.main.listSTLObjects.Items.Count > 1)
                t += " + " + (this.main.listSTLObjects.Items.Count - 1).ToString();
            Main.main.Title = t;

            string newStlFile = dir + Path.DirectorySeparatorChar + "composition.stl";
            //dir += Path.DirectorySeparatorChar + "composition.stl";
            saveComposition(newStlFile);
            Main.slicer.RunSlice(newStlFile); // Slice it and load
        }

        // TODO: Not sure what this is
        //private void checkScaleAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    textScaleY.Enabled = !checkScaleAll.Checked;
        //    textScaleZ.Enabled = !checkScaleAll.Checked;
        //}

        /// <summary>
        /// Autopositions .stl files on the print plane. 
        /// </summary>
        public void Autoposition()
        {
            if (autosizeFailed) return;
            RectPacker packer = new RectPacker(1, 1);
            int border = 3;
            FormPrinterSettings ps = Main.printerSettings;
            float maxW = ps.PrintAreaWidth;
            float maxH = ps.PrintAreaDepth;
            float xOff = ps.BedLeft, yOff = ps.BedFront;
            if (ps.printerType == 1)
            {
                if (ps.DumpAreaFront <= 0)
                {
                    yOff = ps.BedFront + ps.DumpAreaDepth - ps.DumpAreaFront;
                    maxH -= yOff;
                }
                else if (ps.DumpAreaDepth + ps.DumpAreaFront >= maxH)
                {
                    yOff = ps.BedFront + -(maxH - ps.DumpAreaFront);
                    maxH += yOff;
                }
                else if (ps.DumpAreaLeft <= 0)
                {
                    xOff = ps.BedLeft + ps.DumpAreaWidth - ps.DumpAreaLeft;
                    maxW -= xOff;
                }
                else if (ps.DumpAreaWidth + ps.DumpAreaLeft >= maxW)
                {
                    xOff = ps.BedLeft + maxW - ps.DumpAreaLeft;
                    maxW += xOff;
                }
            }
            foreach (STL stl in this.main.listSTLObjects.Items)
            {
                int w = 2 * border + (int)Math.Ceiling(stl.xMax - stl.xMin);
                int h = 2 * border + (int)Math.Ceiling(stl.yMax - stl.yMin);
                if (!packer.addAtEmptySpotAutoGrow(new PackerRect(0, 0, w, h, stl), (int)maxW, (int)maxH))
                {
                    autosizeFailed = true;
                }
            }
            if (autosizeFailed)
            {
                MessageBox.Show("Too many objects on printer bed for automatic packing.\r\nPacking disabled until elements are removed.",
                "Printer bed full", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            float xAdd = (maxW - packer.w) / 2.0f;
            float yAdd = (maxH - packer.h) / 2.0f;
            foreach (PackerRect rect in packer.vRects)
            {
                STL s = (STL)rect.obj;
                float xPos = xOff + xAdd + rect.x + border;
                float yPos = yOff + yAdd + rect.y + border;
                s.Position.x += xPos - s.xMin;
                s.Position.y += yPos - s.yMin;
                s.UpdateBoundingBox();
            }
            Main.main.threedview.UpdateChanges();
        }
        private void buttonAutoplace_Click(object sender, EventArgs e)
        {
            Autoposition();
            foreach (STL stl in this.main.listSTLObjects.Items)
            {
                stl.UpdateBoundingBox();
            }
        }

        /// <summary>
        /// Copies the selected .stl model to make a duplicate
        /// </summary>
        public void CopyObjects()
        {
            if (copyDialog.ShowDialog(Main.main) == DialogResult.Cancel) return;
            int numberOfCopies = (int)copyDialog.numericCopies.Value;

            List<STL> newSTL = new List<STL>();
            foreach (STL act in this.main.listSTLObjects.SelectedItems)
            {
                STL last = act;
                for (int i = 0; i < numberOfCopies; i++)
                {
                    STL stl = last.copySTL();
                    last = stl;
                    newSTL.Add(stl);
                }
            }
            foreach (STL stl in newSTL)
            {
                this.main.listSTLObjects.Items.Add(stl);
                stleditorView.models.AddLast(stl);
            }
            if (copyDialog.checkAutoposition.Checked)
            {
                Autoposition();
            }
            Main.main.threedview.UpdateChanges();
        }

        /// <summary>
        /// Centers the currently selected object
        /// </summary>
        public void CenterObject()
        {
            //STL stl = (STL)listSTLObjects.SelectedItem;
            //if (stl == null) return;
            

            // TODO: The frame around a selected object doesn't move with the object. Not sure why and not sure where the bug is. 
            // Try moving an object and then selecting it again. 
            // I noticed the error while it was using the center position. 
            foreach (STL stl in this.main.listSTLObjects.SelectedItems)
            {
                stl.Center(Main.printerSettings.BedLeft + Main.printerSettings.PrintAreaWidth / 2, Main.printerSettings.BedFront + Main.printerSettings.PrintAreaDepth / 2);
                stl.UpdateBoundingBox();
                listSTLObjects_SelectedIndexChanged(null, null);

            }
            Main.main.threedview.UpdateChanges();
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



    


