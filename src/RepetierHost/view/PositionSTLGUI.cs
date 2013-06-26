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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using OpenTK;
using RepetierHost.model;

namespace RepetierHost.view
{
    public partial class PositionSTLGUI : UserControl
    {
        // TODO: Add increment buttons so you can easily change the positon. Also for the rotation.
        // TODO: Enable the sliders and get them to work. 
        private bool writeSTLBinary = true;
        //public ThreeDView cont;
        private bool autosizeFailed = false;
        private CopyObjectsDialog copyDialog = new CopyObjectsDialog();
        public PositionSTLGUI()
        {
            InitializeComponent();
            try
            {
                //cont = new ThreeDView();
                //  cont.Dock = DockStyle.None;
                //  cont.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                //  cont.Width = Width - panelControls.Width;
                //  cont.Height = Height;
                //  Controls.Add(cont);
                //cont.SetEditor(true);
                //cont.objectsSelected = false;
                //cont.eventObjectMoved += objectMoved;
                //cont.eventObjectSelected += objectSelected;
                //cont.autoupdateable = true;
                updateEnabled();
                if (Main.main != null)
                {
                    Main.main.languageChanged += translate;
                    translate();
                }
            }
            catch { }
        }
        public void translate()
        {
            // TODO: Update these translations
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


        private void CopyObjects()
        {
            if (copyDialog.ShowDialog(Main.main) == DialogResult.Cancel) return;
            int numberOfCopies = (int)copyDialog.numericCopies.Value;

            List<STL> newSTL = new List<STL>();
            foreach (STL act in Main.main.listSTLObjects.SelectedItems)
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
                Main.main.listSTLObjects.Items.Add(stl);
                Main.main.fileAddOrRemove.stleditorView.models.AddLast(stl);
            }
            if (copyDialog.checkAutoposition.Checked)
            {
                Autoposition();
            }
            Main.main.threedview.UpdateChanges();
        }


        /// <summary>
        /// Calls the update to the 3d model using opentTK
        /// </summary>
        public void Update3D()
        {
            Main.main.threedview.UpdateChanges();
        }
        private void float_Validating(object sender, CancelEventArgs e)
        {
            TextBox box = (TextBox)sender;
            try
            {
                float.Parse(box.Text);
                errorProvider.SetError(box, "");
            }
            catch
            {
                errorProvider.SetError(box, "Not a number.");
            }
        }
        private void updateEnabled()
        {
            int n = Main.main.listSTLObjects.SelectedItems.Count;
            if (n != 1)
            {
                textRotX.Enabled = false;
                textRotY.Enabled = false;
                textRotZ.Enabled = false;
                textScaleX.Enabled = false;
                textScaleY.Enabled = false;
                textScaleZ.Enabled = false;
                checkScaleAll.Enabled = false;
                textTransX.Enabled = false;
                textTransY.Enabled = false;
                textTransZ.Enabled = false;
                buttonCenter.Enabled = false;
                buttonAutoplace.Enabled = Main.main.listSTLObjects.Items.Count > 1;
                //buttonLand.Enabled = n > 0;
                if (Main.main.threedview != null)
                    Main.main.threedview.SetObjectSelected(n > 0);
                buttonCopy.Enabled = n > 0;
            }
            else
            {
                buttonAutoplace.Enabled = Main.main.listSTLObjects.Items.Count > 1;
                buttonCopy.Enabled = true;
                textRotX.Enabled = true;
                textRotY.Enabled = true;
                textRotZ.Enabled = true;
                textScaleX.Enabled = true;
                textScaleY.Enabled = !checkScaleAll.Checked;
                textScaleZ.Enabled = !checkScaleAll.Checked;
                checkScaleAll.Enabled = true;
                textTransX.Enabled = true;
                textTransY.Enabled = true;
                textTransZ.Enabled = true;
                buttonCenter.Enabled = true;
                //buttonLand.Enabled = true;
                if (Main.main.threedview != null)
                    Main.main.threedview.SetObjectSelected(true);
            }
            //buttonRemoveSTL.Enabled = n != 0;
            //buttonSlice.Enabled = listSTLObjects.Items.Count > 0;
            //buttonSave.Enabled = listSTLObjects.Items.Count > 0; TODO: Add a save button somewhere. 
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
            STL stl = (STL)Main.main.listSTLObjects.SelectedItem;
            foreach (STL s in Main.main.fileAddOrRemove.stleditorView.models)
            {
                s.Selected = Main.main.listSTLObjects.SelectedItems.Contains(s);
            }
            if (Main.main.listSTLObjects.SelectedItems.Count > 1) stl = null;
            if (stl != null)
            {
                textRotX.Text = stl.Rotation.x.ToString(GCode.format);
                textRotY.Text = stl.Rotation.y.ToString(GCode.format);
                textRotZ.Text = stl.Rotation.z.ToString(GCode.format);
                textScaleX.Text = stl.Scale.x.ToString(GCode.format);
                textScaleY.Text = stl.Scale.y.ToString(GCode.format);
                textScaleZ.Text = stl.Scale.z.ToString(GCode.format);
                textTransX.Text = stl.Position.x.ToString(GCode.format);
                textTransY.Text = stl.Position.y.ToString(GCode.format);
                textTransZ.Text = stl.Position.z.ToString(GCode.format);
                checkScaleAll.Checked = (stl.Scale.x == stl.Scale.y && stl.Scale.x == stl.Scale.z);
            }
            //Main.main.threedview.UpdateChanges();
            Main.main.mainHelp.UpdateEverythingInMain();
        }

        private void textTransX_TextChanged(object sender, EventArgs e)
        {
            STL stl = (STL)Main.main.listSTLObjects.SelectedItem;
            if (stl == null) return;
            float.TryParse(textTransX.Text, NumberStyles.Float, GCode.format, out stl.Position.x);
            updateSTLState(stl);
            Main.main.threedview.UpdateChanges();
        }

        private void textTransY_TextChanged(object sender, EventArgs e)
        {
            STL stl = (STL)Main.main.listSTLObjects.SelectedItem;
            if (stl == null) return;
            float.TryParse(textTransY.Text, NumberStyles.Float, GCode.format, out stl.Position.y);
            updateSTLState(stl);
            Main.main.threedview.UpdateChanges();
        }

        private void textTransZ_TextChanged(object sender, EventArgs e)
        {
            STL stl = (STL)Main.main.listSTLObjects.SelectedItem;
            if (stl == null) return;
            float.TryParse(textTransZ.Text, NumberStyles.Float, GCode.format, out stl.Position.z);
            updateSTLState(stl);
            Main.main.threedview.UpdateChanges();
        }


        private void objectMoved(float dx, float dy)
        {
            //STL stl = (STL)listSTLObjects.SelectedItem;
            //if (stl == null) return;
            foreach (STL stl in Main.main.listSTLObjects.SelectedItems)
            {
                stl.Position.x += dx;
                stl.Position.y += dy;
                if (Main.main.listSTLObjects.SelectedItems.Count == 1)
                {
                    textTransX.Text = stl.Position.x.ToString(GCode.format);
                    textTransY.Text = stl.Position.y.ToString(GCode.format);
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
                    Main.main.listSTLObjects.SelectedItems.Add(sel);
            }
            else
                if (Control.ModifierKeys == Keys.Control)
                {
                    if (sel.Selected)
                        Main.main.listSTLObjects.SelectedItems.Remove(sel);
                    else
                        Main.main.listSTLObjects.SelectedItems.Add(sel);
                }
                else
                {
                    Main.main.listSTLObjects.SelectedItems.Clear();
                    Main.main.listSTLObjects.SelectedItem = sel;
                }
        }
        private void textScaleX_TextChanged(object sender, EventArgs e)
        {
            STL stl = (STL)Main.main.listSTLObjects.SelectedItem;
            if (stl == null) return;
            float.TryParse(textScaleX.Text, NumberStyles.Float, GCode.format, out stl.Scale.x);
            if (checkScaleAll.Checked)
            {
                stl.Scale.y = stl.Scale.z = stl.Scale.x;
                textScaleY.Text = stl.Scale.y.ToString(GCode.format);
                textScaleZ.Text = stl.Scale.z.ToString(GCode.format);
            }
            updateSTLState(stl);
            Main.main.threedview.UpdateChanges();
        }

        private void textScaleY_TextChanged(object sender, EventArgs e)
        {
            STL stl = (STL)Main.main.listSTLObjects.SelectedItem;
            if (stl == null) return;
            float.TryParse(textScaleY.Text, NumberStyles.Float, GCode.format, out stl.Scale.y);
            updateSTLState(stl);
            Main.main.threedview.UpdateChanges();
        }

        private void textScaleZ_TextChanged(object sender, EventArgs e)
        {
            STL stl = (STL)Main.main.listSTLObjects.SelectedItem;
            if (stl == null) return;
            float.TryParse(textScaleZ.Text, NumberStyles.Float, GCode.format, out stl.Scale.z);
            updateSTLState(stl);
            Main.main.threedview.UpdateChanges();
        }

        private void textRotX_TextChanged(object sender, EventArgs e)
        {
            STL stl = (STL)Main.main.listSTLObjects.SelectedItem;
            if (stl == null) return;
            float.TryParse(textRotX.Text, NumberStyles.Float, GCode.format, out stl.Rotation.x);
            updateSTLState(stl);
            Main.main.threedview.UpdateChanges();
        }

        private void textRotY_TextChanged(object sender, EventArgs e)
        {
            STL stl = (STL)Main.main.listSTLObjects.SelectedItem;
            if (stl == null) return;
            float.TryParse(textRotY.Text, NumberStyles.Float, GCode.format, out stl.Rotation.y);
            updateSTLState(stl);
            Main.main.threedview.UpdateChanges();
        }

        private void textRotZ_TextChanged(object sender, EventArgs e)
        {
            STL stl = (STL)Main.main.listSTLObjects.SelectedItem;
            if (stl == null) return;
            float.TryParse(textRotZ.Text, NumberStyles.Float, GCode.format, out stl.Rotation.z);
            updateSTLState(stl);
            Main.main.threedview.UpdateChanges();
        }

        public void buttonRemoveSTL_Click(object sender, EventArgs e)
        {
            //STL stl = (STL)listSTLObjects.SelectedItem;
            //if (stl == null) return;
            LinkedList<STL> list = new LinkedList<STL>();
            foreach (STL stl in Main.main.listSTLObjects.SelectedItems)
                list.AddLast(stl);
            foreach (STL stl in list)
            {
                Main.main.fileAddOrRemove.stleditorView.models.Remove(stl);
                Main.main.listSTLObjects.Items.Remove(stl);
                autosizeFailed = false; // Reset autoposition
            }
            list.Clear();
            if (Main.main.listSTLObjects.Items.Count > 0)
                Main.main.listSTLObjects.SelectedIndex = 0;
            Main.main.threedview.UpdateChanges();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (saveSTL.ShowDialog() == DialogResult.OK)
            {
                Main.main.fileAddOrRemove.saveComposition(saveSTL.FileName);
            }
        }
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
      

        private void buttonLand_Click(object sender, EventArgs e)
        {
            //STL stl = (STL)listSTLObjects.SelectedItem;
            //if (stl == null) return;
            foreach (STL stl in Main.main.listSTLObjects.SelectedItems)
            {
                stl.Land();
                listSTLObjects_SelectedIndexChanged(null, null);
            }
            Main.main.threedview.UpdateChanges();
        }

       

        

        private void checkScaleAll_CheckedChanged(object sender, EventArgs e)
        {
            textScaleY.Enabled = !checkScaleAll.Checked;
            textScaleZ.Enabled = !checkScaleAll.Checked;
        }

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
            foreach (STL stl in Main.main.listSTLObjects.Items)
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
        }

        private void buttonCopyObjects_Click(object sender, EventArgs e)
        {
            //if (copyDialog.ShowDialog(Main.main) == DialogResult.Cancel) return;
            int numberOfCopies = (int)this.numericCopies.Value; // (int)copyDialog.numericCopies.Value;

            List<STL> newSTL = new List<STL>();
            foreach (STL act in Main.main.listSTLObjects.SelectedItems)
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
                Main.main.listSTLObjects.Items.Add(stl);
                Main.main.fileAddOrRemove.stleditorView.models.AddLast(stl);
            }
            if (copyDialog.checkAutoposition.Checked)
            {
                Autoposition();
            }
            Main.main.mainHelp.UpdateEverythingInMain();
            Main.main.threedview.UpdateChanges();
        }
        static bool inRecheckFiles = false;
       

        public void listSTLObjects_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                for (int i = 0; i < Main.main.listSTLObjects.Items.Count; i++)
                    Main.main.listSTLObjects.SetSelected(i, true);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                buttonRemoveSTL_Click(sender, null);
                e.Handled = true;
            }
        }

        private void xAxisScaleSliderControl_Scroll(object sender, EventArgs e)
        {

        }

        private void buttonCenter_Click_1(object sender, EventArgs e)
        {
           
            STL stl1 = (STL)Main.main.listSTLObjects.SelectedItem;
            if (stl1 == null) return;
            foreach (STL stl in Main.main.listSTLObjects.SelectedItems)
            {
                stl.Center(Main.printerSettings.BedLeft + Main.printerSettings.PrintAreaWidth / 2, Main.printerSettings.BedFront + Main.printerSettings.PrintAreaDepth / 2);
                listSTLObjects_SelectedIndexChanged(null, null);

            }
            Main.main.threedview.UpdateChanges();
        

        }

        private void PositionSTLGUI_Leave(object sender, EventArgs e)
        {
            this.Visible = false;
        }

       
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
