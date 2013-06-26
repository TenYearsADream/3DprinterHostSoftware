using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using RepetierHost.model;
using RepetierHost.view;
using RepetierHost.view.utils;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;

namespace RepetierHost
{
    delegate void UpdateAll();

    /// <summary>
    /// Designed to help remove some of the code from the Main class
    /// </summary>
    public class MainHelper
    {
        Main main;
        public MainHelper(Main _main)
        {
            this.main = _main;

        }

        /// <summary>
        /// Updates the current view and buttons
        /// Should call this any time something signficant changes. 
        /// </summary>
        public void UpdateEverythingInMain()
        //public MethodInvoker UpdateEverythingInMain = delegate
       {
           updateSelectionBoxSize();
           //Main.main.Invoke(UpdateJobButtons);
           UpdateJobButtons();
           update3DViewselection();
           UpdateConnections();
           Update3D();
           updatePositionControlLocation();
            


       }

        /// <summary>
        /// Updates the current 3D view
        /// </summary>
        private void update3DViewselection()
        {
            switch (main.current3Dview)
            {

                case RepetierHost.Main.ThreeDViewOptions.loadAFile:

                    main.toolStripStatusLabel1.Text = "Load a file";
                    main.threedview.SetView(main.fileAddOrRemove.stleditorView);
                    break;
                case RepetierHost.Main.ThreeDViewOptions.STLeditor:
                    main.toolStripStatusLabel1.Text = "stl editor";
                    main.threedview.SetView(main.fileAddOrRemove.stleditorView);
                    break;
                case RepetierHost.Main.ThreeDViewOptions.gcode:
                    main.toolStripStatusLabel1.Text = "gcode editor";
                    main.listSTLObjects.Visible = false;
                    main.threedview.SetView(main.jobPreview);
                    break;
                case RepetierHost.Main.ThreeDViewOptions.printing:
                    main.toolStripStatusLabel1.Text = "printPreview";
                    main.listSTLObjects.Visible = false;
                    main.threedview.SetView(main.printPreview);
                    break;
            }
        }

        /// <summary>
        /// Updates the SplitButton Dropdown menu to include the recent printers that were used. 
        /// </summary>
        public void UpdateConnections()
        {
            main.connectToolStripSplitButton.DropDownItems.Clear();
            foreach (string s in RepetierHost.Main.printerSettings.printerKey.GetSubKeyNames())
            // main.printerSettings.printerKey.GetSubKeyNames())
            {
                main.connectToolStripSplitButton.DropDownItems.Add(s, null, main.ConnectHandler);
            }
            foreach (ToolStripItem it in main.connectToolStripSplitButton.DropDownItems)
                it.Enabled = !Main.conn.connected;// main.conn.connected;
        }

        /// <summary>
        /// Call the 3d view to update
        /// </summary>
        public void Update3D()
        {
            if (main.threedview != null)
                main.threedview.UpdateChanges();
        }

        /// <summary>
        /// Changes the size and visibility of the .stl model selection box in the top right corner
        /// </summary>
        public void updateSelectionBoxSize()
        {
            // We must be in .stleditor mode and have some models to show. 
            if (main.listSTLObjects.Items.Count > 0 && main.current3Dview == Main.ThreeDViewOptions.STLeditor)
            {
                main.listSTLObjects.Visible = true;
                main.listSTLObjects.Height = main.listSTLObjects.PreferredHeight;
            }
            else
            {
                main.listSTLObjects.Visible = false;
            }
        }


        /// <summary>
        /// Updates the avaible buttons to click. 
        /// </summary>
        //public MethodInvoker UpdateJobButtons = delegate
        public void UpdateJobButtons()
        {
          

            switch (Main.main.current3Dview)
            {
                case RepetierHost.Main.ThreeDViewOptions.loadAFile:
                    Main.main.positionToolSplitButton2.Enabled = false;
                    Main.main.sliceToolSplitButton3.Enabled = false;
                    Main.main.printStripSplitButton4.Enabled = false;
                    break;

                case RepetierHost.Main.ThreeDViewOptions.STLeditor:
                    Main.main.positionToolSplitButton2.Enabled = true;
                    Main.main.sliceToolSplitButton3.Enabled = true;
                    Main.main.printStripSplitButton4.Enabled = false;
                    break;
                case RepetierHost.Main.ThreeDViewOptions.gcode:
                    Main.main.positionToolSplitButton2.Enabled = false;
                    Main.main.sliceToolSplitButton3.Enabled = false;
                    if (Main.conn.connected == true)
                        Main.main.printStripSplitButton4.Enabled = true;
                    break;
                case RepetierHost.Main.ThreeDViewOptions.printing:
                    Main.main.positionToolSplitButton2.Enabled = false;
                    Main.main.sliceToolSplitButton3.Enabled = false;
                    Main.main.printStripSplitButton4.Enabled = Main.conn.connected;
                    break;
            }

            if (Main.conn.job.mode != 1) // if it is not printing. 1 = printing
            {
                Main.main.killJobToolStripMenuItem.Enabled = false;
                //Main.main.printStripSplitButton4.Enabled = Main.conn.connected;
                Main.main.printStripSplitButton4.ToolTipText = Trans.T("M_RUN_JOB"); // "Run job";
                Main.main.printStripSplitButton4.Text = Trans.T("M_RUN_JOB"); //"Run job";
                Main.main.printStripSplitButton4.Image = Main.main.imageList.Images[2];
            }
            else
            {
                Main.main.printStripSplitButton4.Enabled = true;
                Main.main.killJobToolStripMenuItem.Enabled = true;
                Main.main.printStripSplitButton4.Image = Main.main.imageList.Images[3];
                Main.main.printStripSplitButton4.ToolTipText = Trans.T("M_PAUSE_JOB"); //"Pause job";
                Main.main.printStripSplitButton4.Text = Trans.T("M_PAUSE_JOB"); //"Pause job";
                Main.main.printVisual.Clear();
            }

        }//;


        public void updatePositionControlLocation()
        {
            if (Main.main.current3Dview != Main.ThreeDViewOptions.STLeditor)
                this.main.postionGUI.Visible = false;

            this.main.postionGUI.Left = this.main.Width - this.main.postionGUI.Width;
            this.main.postionGUI.Top = (this.main.Height - this.main.postionGUI.Height) / 2;

        }
    }
}

