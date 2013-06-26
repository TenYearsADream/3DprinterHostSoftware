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
    public partial class positionModelGUIInterface : UserControl
    {
        FormPrinterSettings printerSettings = null;
        public positionModelGUIInterface()
        {
            InitializeComponent();
            //trackBar1.Minimum
        }

        /// <summary>
        /// Set the printer Settings and update all the controls. 
        /// </summary>
        /// <param name="settings"></param>
        public void setPrinterSettings(FormPrinterSettings settings)
        {
            printerSettings = settings;
            updateControls();
        }

        public void updateControls()
        {
           // printerSettings.XMax;

        }

        /// <summary>
        /// Action to preform when user leaves the control. Shou
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void positionModelGUIInterface_Leave(object sender, EventArgs e)
        {
            Main.main.postionGUI.Visible = false;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void XAxisMoveNumberControl_ValueChanged(object sender, EventArgs e)
        {
            STL stl = (STL)Main.main.listSTLObjects.SelectedItem;          
            if (stl == null) return;
            float.TryParse(XAxisMoveNumberControl.Text, NumberStyles.Float, GCode.format, out stl.Position.x);
            Main.main.mainHelp.UpdateEverythingInMain();
            //updateSTLState(stl);
            Main.main.threedview.UpdateChanges();
        }

        private void positionModelGUIInterface_Enter(object sender, EventArgs e)
        {
            // TODO, Update everything. 
        }

        public void updateAllFields()
        {
            // Update on selection change, and when 
            // If no .stl is selected then don't enable anything 
            // Also if not in the .stleditor mode, then don't bother. 
            // Add code to the update everything so that if we are switching to .stleditor it shold update this. 


            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Main.main.fileAddOrRemove.CopyObjects();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main.main.fileAddOrRemove.CenterObject();
        }

        private void autoPositionButton_Click(object sender, EventArgs e)
        {
            Main.main.fileAddOrRemove.Autoposition();
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
                XAxisRotateNumberControl.Value = (decimal)stl.Rotation.x; //textRotX.Text = stl.Rotation.x.ToString(GCode.format);
                YAxisRotateNumberControl.Value = (decimal)stl.Rotation.y; //textRotY.Text = stl.Rotation.y.ToString(GCode.format);
                ZAxisRotateNumberControl.Value = (decimal)stl.Rotation.z; //textRotZ.Text = stl.Rotation.z.ToString(GCode.format);
                XAxisScaleNumberControl.Value = (decimal)stl.Scale.x; //textScaleX.Text = stl.Scale.x.ToString(GCode.format);
                YAxisScaleNumberControl.Value = (decimal)stl.Scale.y; //textScaleY.Text = stl.Scale.y.ToString(GCode.format);
                ZAxisScaleNumberControl.Value = (decimal)stl.Scale.z ; //textScaleZ.Text = stl.Scale.z.ToString(GCode.format);
                XAxisMoveNumberControl.Value = (decimal)stl.Position.x; //textTransX.Text = stl.Position.x.ToString(GCode.format);
                YAxisMoveNumberControl.Value = (decimal)stl.Position.y; //textTransY.Text = stl.Position.y.ToString(GCode.format);
                ZAxisMoveNumberControl.Value = (decimal)stl.Position.z; //textTransZ.Text = stl.Position.z.ToString(GCode.format);
                scaleTogethercheckBox.Checked =  (stl.Scale.x == stl.Scale.y && stl.Scale.x == stl.Scale.z); //checkScaleAll.Checked = (stl.Scale.x == stl.Scale.y && stl.Scale.x == stl.Scale.z);
            }
            Main.main.threedview.UpdateChanges();
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

        // TODO: Only allow for moving and rotating of .stl objects when a object is selected. Reuse this function. 
        private void updateEnabled()
        {
            int n = Main.main.listSTLObjects.SelectedItems.Count;
            if (n != 1)
            {
                // TODO: Add sliders
                XAxisRotateNumberControl.Enabled = false;
                YAxisRotateNumberControl.Enabled = false;
                ZAxisRotateNumberControl.Enabled = false;
                XAxisScaleNumberControl.Enabled = false;
                YAxisScaleNumberControl.Enabled = false;
                ZAxisScaleNumberControl.Enabled = false;
                scaleTogethercheckBox.Enabled = false;
                XAxisMoveNumberControl.Enabled = false;
                YAxisMoveNumberControl.Enabled = false;
                ZAxisMoveNumberControl.Enabled = false;
                centerObjectButton.Enabled = false;
                autoPositionButton.Enabled = (Main.main.listSTLObjects.Items.Count > 1);
                //buttonLand.Enabled = n > 0;
                if (Main.main.threedview != null)
                    Main.main.threedview.SetObjectSelected(n > 0);
                tempCopyButton.Enabled = n > 0;
            }
            else
            {
                autoPositionButton.Enabled = Main.main.listSTLObjects.Items.Count > 1;
                tempCopyButton.Enabled = true;
                XAxisRotateNumberControl.Enabled = true;
                YAxisRotateNumberControl.Enabled = true;
                ZAxisRotateNumberControl.Enabled = true;
                XAxisScaleNumberControl.Enabled = true;
                YAxisScaleNumberControl.Enabled = !scaleTogethercheckBox.Checked;
                ZAxisScaleNumberControl.Enabled = !scaleTogethercheckBox.Checked;
                scaleTogethercheckBox.Enabled = true;
                XAxisMoveNumberControl.Enabled = true;
                YAxisMoveNumberControl.Enabled = true;
                ZAxisMoveNumberControl.Enabled = true;
                centerObjectButton.Enabled = true;
                //buttonLand.Enabled = true;
                if (Main.main.threedview != null)
                    Main.main.threedview.SetObjectSelected(true);
            }
            // buttonRemoveSTL.Enabled = n != 0;// TODO: just hit the delete button
            //buttonSlice.Enabled = listSTLObjects.Items.Count > 0;
            //buttonSave.Enabled = listSTLObjects.Items.Count > 0;
        }
    }
}
