///-----------------------------------------------------------------------
/// <copyright file="FileAddOrRemove.cs" company="Baoyan">
///   Some parts of this file were derived from Repetier Host which can be found at
/// https://github.com/repetier/Repetier-Host Which is licensed using the Apache 2.0 license. 
/// All other parts of the file are property of Baoyan Automation LTC, Nanjing Jiangsu China
/// http://www.by3dp.com
/// </copyright>
///-----------------------------------------------------------------------

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
        // static RepetierHost.Main.ThreeDViewOptions oldviewMode;
        
        public MainHelper(Main _main)
        {
            this.main = _main;
            // oldviewMode = Main.ThreeDViewOptions.STLeditor;
            oldview = main.fileAddOrRemove.stleditorView;

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
           SyncViews();
           UpdateConnections();
           UpdateProgressBar();
           
           Update3D();
           updatePositionControlLocation();
       }

        private void UpdateProgressBar()
        {
            if(RepetierHost.Main.conn.job.mode !=1 )
                Main.main.toolProgress.Enabled = false;
            else
                Main.main.toolProgress.Enabled = true;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the current 3D view
        /// </summary>
        private void update3DViewselection()
        {
            switch (main.current3Dview)
            {

                case RepetierHost.Main.ThreeDViewOptions.loadAFile:
                    main.toolStripStatusLabel1.Text = Trans.T("M_LOAD_A_FILE");
                    main.threedview.SetView(main.fileAddOrRemove.stleditorView);
                    break;
                case RepetierHost.Main.ThreeDViewOptions.STLeditor:
                    main.toolStripStatusLabel1.Text = Trans.T("M_STL_EDITOR");
                    main.threedview.SetView(main.fileAddOrRemove.stleditorView);
                    break;
                case RepetierHost.Main.ThreeDViewOptions.gcode:
                    main.toolStripStatusLabel1.Text = Trans.T("M_GCODE_VIEW");
                    main.listSTLObjects.Visible = false;
                    main.threedview.SetView(main.gcodePreviewView);
                    break;
                case RepetierHost.Main.ThreeDViewOptions.livePrinting:
                    main.toolStripStatusLabel1.Text = Trans.T("M_LIVE_PRINTING");
                    main.listSTLObjects.Visible = false;
                    main.threedview.SetView(main.printPreview);
                    break;
            }
            
        }


        static ThreeDView oldview;
        /// <summary>
        /// Syncs the .stleditor, g-code, and printviews only on a change. 
        /// TODO: NOt working when going from g-code to .stl for some reason. Not sure why. 
        /// </summary>
        private void SyncViews()
        {
           
            //if (main.current3Dview != oldviewMode)
            //{

            //    switch (oldviewMode)
            //    {

            //        case RepetierHost.Main.ThreeDViewOptions.loadAFile:
            //            oldview = Main.main.threedview.view;
            //            break;
            //        case RepetierHost.Main.ThreeDViewOptions.STLeditor:
            //            oldview = Main.main.threedview.view;
            //            break;
            //        case RepetierHost.Main.ThreeDViewOptions.gcode:
            //            oldview = Main.main.gcodePreviewView;
            //            break;
            //        case RepetierHost.Main.ThreeDViewOptions.printing:
            //            oldview = Main.main.printPreview;
            //            break;
            //    }

            //    switch (main.current3Dview)
            //    {

            //        case RepetierHost.Main.ThreeDViewOptions.loadAFile:
            //            newThreedview = Main.main.threedview.view;                    
            //            break;
            //        case RepetierHost.Main.ThreeDViewOptions.STLeditor:
            //            newThreedview = Main.main.threedview.view;
            //            break;
            //        case RepetierHost.Main.ThreeDViewOptions.gcode:
            //            newThreedview = Main.main.gcodePreviewView;
            //            break;
            //        case RepetierHost.Main.ThreeDViewOptions.printing:
            //            newThreedview = Main.main.printPreview;
            //            break;
            //    }
            //        //    FormPrinterSettings ps = Main.printerSettings;
            //    //public onObjectMoved eventObjectMoved;
            //    //public onObjectSelected eventObjectSelected;
            //    newThreedview.zoom = oldview.zoom;
            //    newThreedview.viewCenter = oldview.viewCenter;
            //    newThreedview.userPosition = oldview.userPosition;    //public Vector3 userPosition;
            //    newThreedview.lookAt = oldview.lookAt;
            //    newThreedview.persp = oldview.persp;
            //    newThreedview.modelView = oldview.modelView; //public Matrix4 lookAt, persp, modelView;
            //    newThreedview.normX = oldview.normX;
            //    newThreedview.normY = oldview.normY; //public double normX = 0, normY = 0;
            //    newThreedview.nearDist = oldview.nearDist;
            //    newThreedview.farDist = oldview.farDist;
            //    newThreedview.aspectRatio = oldview.aspectRatio;
            //    newThreedview.nearHeight = oldview.nearHeight; //public float nearDist, farDist, aspectRatio, nearHeight;
            //    newThreedview.rotZ = oldview.rotZ;
            //    newThreedview.rotX = oldview.rotX; //public float rotZ = 0, rotX = 0;
            //    //public int mode = 0;
            //    //public bool editor = false;
            //    //public bool autoupdateable = false;
            //    //public int slowCounter = 0; // Indicates slow framerates
            //    //public uint timeCall = 0;
            //}
            ////oldview = newThreedview;
            //oldviewMode = main.current3Dview;

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
                    Main.main.saveGCodeToolStripMenuItem.Enabled = false;
                    Main.main.saveNewSTLMenuItem11.Enabled = false;

                    if(Main.main.listSTLObjects.Items.Count >0)
                     Main.main.STLEditorMenuOption.Enabled = true;
                    else
                        Main.main.STLEditorMenuOption.Enabled = false;

                   
                    if(Main.main.printPreview.models.Count >1)
                        Main.main.gCodeVisualizationMenuOption.Enabled = true;
                    else
                         Main.main.gCodeVisualizationMenuOption.Enabled = false;

                    Main.main.livePrintingMenuOption.Enabled = false;
                    Main.main.emergencyStopStripButton6.Enabled = false;


                    break;

                   

                case RepetierHost.Main.ThreeDViewOptions.STLeditor:
                    Main.main.positionToolSplitButton2.Enabled = true;
                    Main.main.sliceToolSplitButton3.Enabled = true;
                    Main.main.printStripSplitButton4.Enabled = false;

                    Main.main.saveGCodeToolStripMenuItem.Enabled = false;
                    Main.main.saveNewSTLMenuItem11.Enabled = true;

                    Main.main.loadAFileMenuModeMenuOption.Enabled = false;
                    Main.main.STLEditorMenuOption.Enabled = true;

                    if (Main.main.editor.getContentArray(0).Count >1)
                        Main.main.gCodeVisualizationMenuOption.Enabled = true;
                    else
                         Main.main.gCodeVisualizationMenuOption.Enabled = false;

                     Main.main.livePrintingMenuOption.Enabled = false;
                     Main.main.emergencyStopStripButton6.Enabled = false;
                    break;

                case RepetierHost.Main.ThreeDViewOptions.gcode:
                    Main.main.positionToolSplitButton2.Enabled = false;
                    Main.main.sliceToolSplitButton3.Enabled = false;
                    Main.main.gCodeVisualizationMenuOption.Enabled = true;
                    if (Main.conn.connected == true)
                    {
                        Main.main.printStripSplitButton4.Enabled = true;
                        Main.main.printStripSplitButton4.Image = Main.main.imageList.Images[2];
                        Main.main.livePrintingMenuOption.Enabled = true;
                        Main.main.emergencyStopStripButton6.Enabled = true;
                    }
                    else
                        Main.main.emergencyStopStripButton6.Enabled = false;

                    Main.main.saveGCodeToolStripMenuItem.Enabled = true;
                    Main.main.loadAFileMenuModeMenuOption.Enabled = true;
                    if (Main.main.listSTLObjects.Items.Count > 0)
                    {
                        Main.main.STLEditorMenuOption.Enabled = true;
                        Main.main.saveNewSTLMenuItem11.Enabled = true;
                    }

                    break;
                case RepetierHost.Main.ThreeDViewOptions.livePrinting:
                    Main.main.positionToolSplitButton2.Enabled = false;
                    Main.main.sliceToolSplitButton3.Enabled = false;
                    Main.main.printStripSplitButton4.Enabled = Main.conn.connected;
                    Main.main.saveGCodeToolStripMenuItem.Enabled = true;
                    Main.main.saveNewSTLMenuItem11.Enabled = false;

                    Main.main.loadAFileMenuModeMenuOption.Enabled = true;
                    if (Main.main.editor.getContentArray(0).Count > 1)
                        Main.main.gCodeVisualizationMenuOption.Enabled = true;
                    else
                        Main.main.gCodeVisualizationMenuOption.Enabled = false;

                    if (Main.main.listSTLObjects.Items.Count > 0)
                    {
                        Main.main.STLEditorMenuOption.Enabled = true;
                        Main.main.saveNewSTLMenuItem11.Enabled = true;
                    }
                    Main.main.emergencyStopStripButton6.Enabled = true;

                    break;
            }

            if (Main.conn.job.mode != 1) // if it is not printing. 1 = printing
            {
                Main.main.killJobToolStripMenuItem.Enabled = false;
                //Main.main.printStripSplitButton4.Enabled = Main.conn.connected;
                Main.main.printStripSplitButton4.ToolTipText = Trans.T("M_RUN_JOB"); // "Run job";
                Main.main.printStripSplitButton4.Text = Trans.T("M_RUN_JOB"); //"Run job";

                
                Main.main.printStripSplitButton4.Image = Main.main.imageList.Images[2]; // image "Play"
            }
            else
            {
                Main.main.printStripSplitButton4.Enabled = true;
                Main.main.killJobToolStripMenuItem.Enabled = true;
                Main.main.printStripSplitButton4.Image = Main.main.imageList.Images[1]; // Image "pause"
                Main.main.printStripSplitButton4.ToolTipText = Trans.T("M_PAUSE_JOB"); //"Pause job";
                Main.main.printStripSplitButton4.Text = Trans.T("M_PAUSE_JOB"); //"Pause job";
                Main.main.printVisual.Clear();
            }

        }//;


        public void updatePositionControlLocation()
        {
            //if (Main.main.current3Dview != Main.ThreeDViewOptions.STLeditor)
            //    this.main.postionGUI.Visible = false;

            //if (this.main.postionGUI.Visible == true)
            //{
            //    this.main.postionGUI.Left = this.main.Width - this.main.postionGUI.Width;
            //    this.main.postionGUI.Top = (this.main.Height - this.main.postionGUI.Height) / 2;
            //}
        }
    }
}

