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
    public delegate void executeHostCommandDelegate(GCode code);
    public delegate void languageChangedEvent();

    public partial class Main : Form
    {
        public event languageChangedEvent languageChanged;
        private const int InfoPanel2MinSize = 440;
        public static PrinterConnection conn;
        public static Main main;
        public static FormPrinterSettings printerSettings;
        public static PrinterModel printerModel;
        public static ThreeDSettings threeDSettings;
        public static GlobalSettings globalSettings = null;
        public static GCodeGenerator generator = null;
        public string basicTitle = "";
        public static bool IsMono = Type.GetType("Mono.Runtime") != null;
        public static Slicer slicer = null;
        public static Slic3r slic3r = null;
        public static bool IsMac = false;

        public PositionSTLGUI postionGUI = null;
        //public positionModelGUIInterface positionModelGUI = null;
        public Form extraForm = null;
        public Form logform = null;
        public MainHelper mainHelp = null;
        public Skeinforge skeinforge = null;
        public EEPROMRepetier eepromSettings = null;
        public EEPROMMarlin eepromSettingsm = null;
        public LogView logView = null;
        public PrintPanel printPanel = null;
        public RegistryKey repetierKey;
        public ThreeDControlOld threedview = null;
        public ThreeDView gcodePreviewView = null;
        public ThreeDView printPreview = null;
        public GCodeVisual jobVisual = new GCodeVisual();
        public GCodeVisual printVisual = null;
        // public STLComposer stlComposer1 = null;
        public volatile GCodeVisual newVisual = null;
        public volatile bool jobPreviewThreadFinished = true;
        public volatile Thread previewThread = null;
        public RegMemory.FilesHistory fileHistory = new RegMemory.FilesHistory("fileHistory", 8);
        public int refreshCounter = 0;
        public executeHostCommandDelegate executeHostCall;
        bool recalcJobPreview = false;
        List<GCodeShort> previewArray0, previewArray1, previewArray2;
        public TemperatureHistory history = null;
        public TemperatureView tempView = null;
        public Trans trans = null;
        public RepetierHost.view.RepetierEditor editor;
        public double gcodePrintingTime = 0;
        public FileAddOrRemove fileAddOrRemove = null;

        public enum ThreeDViewOptions { loadAFile, STLeditor, gcode, printing };
        public ThreeDViewOptions current3Dview = ThreeDViewOptions.loadAFile;

        public bool DeveloperMode = false;

        public class JobUpdater
        {
            GCodeVisual visual = null;
            // This method will be called when the thread is started.
            public void DoWork()
            {
                RepetierEditor ed = Main.main.editor;

                Stopwatch sw = new Stopwatch();
                sw.Start();
                visual = new GCodeVisual();
                visual.showSelection = true;
                switch (ed.ShowMode)
                {
                    case 0:
                        visual.minLayer = 0;
                        visual.maxLayer = 999999;
                        break;
                    case 1:
                        visual.minLayer = visual.maxLayer = ed.ShowMinLayer;
                        break;
                    case 2:
                        visual.minLayer = ed.ShowMinLayer;
                        visual.maxLayer = ed.ShowMaxLayer;
                        break;
                }
                visual.parseGCodeShortArray(Main.main.previewArray0, true, 0);
                visual.parseGCodeShortArray(Main.main.previewArray1, false, 1);
                visual.parseGCodeShortArray(Main.main.previewArray2, false, 2);
                Main.main.previewArray0 = Main.main.previewArray1 = Main.main.previewArray2 = null;
                visual.Reduce();
                Main.main.gcodePrintingTime = visual.ana.printingTime;
                //visual.stats();
                Main.main.newVisual = visual;
                Main.main.jobPreviewThreadFinished = true;
                Main.main.previewThread = null;
                sw.Stop();
                //Main.conn.log("Update time:" + sw.ElapsedMilliseconds, false, 3);
            }
        }
        //From Managed.Windows.Forms/XplatUI
        static bool IsRunningOnMac()
        {
            IntPtr buf = IntPtr.Zero;
            try
            {
                buf = System.Runtime.InteropServices.Marshal.AllocHGlobal(8192);
                // This is a hacktastic way of getting sysname from uname ()
                if (uname(buf) == 0)
                {
                    string os = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(buf);
                    if (os == "Darwin")
                        return true;
                }
            }
            catch
            {
            }
            finally
            {
                if (buf != IntPtr.Zero)
                    System.Runtime.InteropServices.Marshal.FreeHGlobal(buf);
            }
            return false;
        }
        private void Main_Load(object sender, EventArgs e)
        {
            /*    RegMemory.RestoreWindowPos("mainWindow", this);
               // if (WindowState == FormWindowState.Maximized)
               //     Application.DoEvents(); // This crashes mono if run here
                splitLog.SplitterDistance = RegMemory.GetInt("logSplitterDistance", splitLog.SplitterDistance);
                splitInfoEdit.SplitterDistance = RegMemory.GetInt("infoEditSplitterDistance", Width - 470);
                //A bug causes the splitter to throw an exception if the PanelMinSize is set too soon.
                splitInfoEdit.Panel2MinSize = Main.InfoPanel2MinSize;
                //splitInfoEdit.SplitterDistance = (splitInfoEdit.Width - splitInfoEdit.Panel2MinSize);
             * */
            mainHelp.UpdateEverythingInMain();
            //Main.main.Invoke(UpdateJobButtons);
        }
        [System.Runtime.InteropServices.DllImport("libc")]
        static extern int uname(IntPtr buf);
        public Main()
        {
            executeHostCall = new executeHostCommandDelegate(this.executeHostCommand);
            repetierKey = Custom.BaseKey; // Registry.CurrentUser.CreateSubKey("SOFTWARE\\Repetier");
            repetierKey.SetValue("installPath", Application.StartupPath);
            if (Path.DirectorySeparatorChar != '\\' && IsRunningOnMac())
                IsMac = true;
            /*String[] parms = Environment.GetCommandLineArgs();
            string lastcom = "";
            foreach (string s in parms)
            {
                if (lastcom == "-home")
                {
                    repetierKey.SetValue("installPath",s);
                    lastcom = "";
                    continue;
                }
                if (s == "-macosx") IsMac = true;
                lastcom = s;
            }*/
            main = this;
          
            SplashScreen.run();
            trans = new Trans(Application.StartupPath + Path.DirectorySeparatorChar + "data" + Path.DirectorySeparatorChar + "translations");
            SwitchButton.imageOffset = RegMemory.GetInt("onOffImageOffset", 0);
            generator = new GCodeGenerator();
            globalSettings = new GlobalSettings();
            conn = new PrinterConnection();
            printerSettings = new FormPrinterSettings();
            printerModel = new PrinterModel();
            conn.analyzer.start();
            threeDSettings = new ThreeDSettings();
            InitializeComponent();

            editor = new RepetierEditor();
            this.Controls.Add(editor);
            editor.Visible = false;
            //editor.Left = this.Width - editor.Width;
            //editor.Top = (this.Height - editor.Height) / 2;
            
            //panel1.Controls.Add(editor);
            updateShowFilament();
            RegMemory.RestoreWindowPos("mainWindow", this);


            extraForm = new Form();
            postionGUI = new PositionSTLGUI();
            postionGUI.Left = this.Width - postionGUI.Width;
            postionGUI.Top = (this.Height - postionGUI.Height) / 2;
            postionGUI.Visible = false;
            Main.main.Controls.Add(postionGUI);



            fileAddOrRemove = new FileAddOrRemove(this);
            main.listSTLObjects.Visible = false;
            

            // Anthony, Maximize the window
            WindowState = FormWindowState.Maximized;

            if (WindowState == FormWindowState.Maximized)
                Application.DoEvents();
            //splitLog.SplitterDistance = RegMemory.GetInt("logSplitterDistance", splitLog.SplitterDistance);
            // splitInfoEdit.SplitterDistance = RegMemory.GetInt("infoEditSplitterDistance", Width-470);
            if (IsMono)
            {
                if (!IsMac)
                {
                    foreach (ToolStripItem m in menu.Items)
                    {
                        m.Text = m.Text.Replace("&", null);
                    }
                }
                if (IsMac)
                {
                    /*Application.Events.Quit += delegate (object sender, ApplicationEventArgs e) {
                        Application.Quit ();
                        e.Handled = true;
                    };
 
                    ApplicationEvents.Reopen += delegate (object sender, ApplicationEventArgs e) {
                        WindowState = FormWindowState.Normal;
                        e.Handled = true;
                    };*/

                    MinimumSize = new Size(500, 640);
                    //tab.MinimumSize = new Size(500, 500);
                    //splitLog.Panel1MinSize = 520;
                    //splitLog.Panel2MinSize = 100;
                    //splitLog.IsSplitterFixed = true;
                    ////splitContainerPrinterGraphic.SplitterDistance -= 52;
                    //splitLog.SplitterDistance = splitLog.Height - 100;
                }
            }
            //slicerToolStripMenuItem.Visible = false;
            //splitLog.Panel2Collapsed = !RegMemory.GetBool("logShow", true);
            conn.eventConnectionChange += OnPrinterConnectionChange;
            conn.eventPrinterAction += OnPrinterAction;
            conn.eventJobProgress += OnJobProgress;


            //stlComposer1 = new STLComposer();
            //stlComposer1.Dock = DockStyle.Fill;
            //tabModel.Controls.Add(stlComposer1);
            //panel1.Controls.Add(stlComposer1);

            printPanel = new PrintPanel();
            //printPanel.Dock = DockStyle.Fill;
            this.Controls.Add(printPanel);
            printPanel.Visible = false;
            printerSettings.formToCon();

            extraForm = new Form();
            extraForm.Dock = DockStyle.Fill;


            logView = new LogView();
            logform = new Form();
            logView.Dock = DockStyle.Fill;
          
            
            logform.StartPosition = FormStartPosition.WindowsDefaultBounds;

            logform.Controls.Add(logView);
            //logView.Dock = DockStyle.Fill;
            //splitLog.Panel2.Controls.Add(logView);

            // TODO: Remomve this. 
            skeinforge = new Skeinforge();

            PrinterChanged(printerSettings.currentPrinterKey, true);
            printerSettings.eventPrinterChanged += PrinterChanged;



            /// Threedview is the part that shows either the .stl files, g-code analysis results, or the print preview (showing the travel path of the print head). 
            /// See the Method assign3DView() to get an adea of how it works. 
            threedview = new ThreeDControlOld();
            threedview.Dock = DockStyle.Fill;
            panel2.Controls.Add(threedview); // Add the OpenGL panel to the panel2 from the Windows Form
            //tabPage3DView.Controls.Add(threedview);

            // 
            gcodePreviewView = new ThreeDView();
            gcodePreviewView.SetEditor(false);
            gcodePreviewView.models.AddLast(jobVisual); // Add a g-code visualzation model to the view
            editor.contentChangedEvent += JobPreview;
            editor.commands = new Commands();
            editor.commands.Read("default", "en");
            this.fileAddOrRemove.UpdateHistory();

            // Make a new View. 
            printPreview = new ThreeDView();
            printPreview.SetEditor(false);
            printPreview.autoupdateable = true;

            // Print visualzation is dependent on the connection to the printer being active. (ie the printer is connected)
            printVisual = new GCodeVisual(conn.analyzer);
            printVisual.liveView = true;
            printPreview.models.AddLast(printVisual); // Add a new model to the view
            basicTitle = Text;


          
            //UpdateConnections();
            //mainHelp.UpdateEverythingInMain();

           
            // TODO: One is called slic3r and the other slicer. Why two??
            Main.slic3r = new Slic3r();
            Main.slicer = new Slicer();

            //toolShowLog_CheckedChanged(null, null);
            updateShowFilament();
            //update3DviewSelection();
            mainHelp = new MainHelper(this);
            // mainHelp.UpdateEverythingInMain(); called later. 

            // TODO: Add temperature controls
            //history = new TemperatureHistory();
            //tempView = new TemperatureView();
            //tempView.Dock = DockStyle.Fill;
            //tabPageTemp.Controls.Add(tempView);
            //if (IsMono)
            //{
            //    showWorkdirectoryToolStripMenuItem.Visible = false;
            //    toolStrip.Height = 56;
            //}
            new SoundConfig();
            //stlComposer1.buttonSlice.Text = Trans.T1("L_SLICE_WITH", slicer.SlicerName);

            // Customizations

            if (Custom.GetBool("removeTestgenerator", false))
            {
                internalSlicingParameterToolStripMenuItem.Visible = false;
                testCaseGeneratorToolStripMenuItem.Visible = false;
            }
            string titleAdd = Custom.GetString("titleAddition", "");
            if (titleAdd.Length > 0)
            {
                int p = basicTitle.IndexOf(' ');
                basicTitle = basicTitle.Substring(0, p) + titleAdd + basicTitle.Substring(p);
                Text = basicTitle;
            }
            mainHelp.UpdateEverythingInMain();
            //update3DviewSelection();
            //slicerPanel.UpdateSelection();

            // Determine whether to check for updates or not based on the users check preferences in advances settings. 
            if (Custom.GetBool("removeUpdates", false))
                checkForUpdatesToolStripMenuItem.Visible = false;
            else
                RHUpdater.checkForUpdates(true);
            UpdateToolbarSize();

            // Determine which languages should appear in the languages menu. 
            // Add languages
            foreach (Translation t in trans.translations.Values)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(t.language, null, languageSelected);
                item.Tag = t;
                languageToolStripMenuItem.DropDownItems.Add(item);
            }
            languageChanged += translate;
            translate();


            // Set which slicer to use. Remeber the users's preference before. 
            if (Custom.GetBool("removeSkeinforge", false))
            {
                Main.slicer.ActiveSlicer = Slicer.SlicerID.Slic3r; // set it to slic3r
            }

            // Sets whether to show the button for support (i.e Help) in the help menough. 
            if (Custom.GetBool("extraSupportButton", false))
            {
                supportToolStripMenuItem.Text = Custom.GetString("extraSupportText", "Support");
            }
            else supportToolStripMenuItem.Visible = false;
            string supportImage = Custom.GetString("extraSupportToolbarImage", "");
            if (supportImage.Length > 0 && File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + supportImage))
            {
                helpSplitButton3.Image = Image.FromFile(Application.StartupPath + Path.DirectorySeparatorChar + Custom.GetString("extraSupportToolbarImage", ""));
                helpSplitButton3.Text = Custom.GetString("extraSupportText", "Support");
            }
            else
            {
                helpSplitButton3.Visible = false;
            }

            // Tool tip text
            toolAction.Text = Trans.T("L_IDLE");
            toolConnection.Text = Trans.T("L_DISCONNECTED");
            updateTravelMoves();

            // Allow for Drag and drop
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

            //Main.main.Invoke(UpdateJobButtons);

        } // End Main()

        /// <summary>
        /// Control the Drag Enter actions.
        /// Basically copy the file to the clip board??
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        /// Control the dragdrop drop action. Try loading the file. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files) this.fileAddOrRemove.LoadGCodeOrSTL(file);
        }

        /// <summary>
        /// Translate all the menus
        /// TODO: Update this. 
        /// </summary>
        public void translate()
        {
            fileToolStripMenuItem.Text = Trans.T("M_FILE");
            settingsToolStripMenuItem.Text = Trans.T("M_CONFIG");
            //slicerToolStripMenuItem.Text = Trans.T("M_SLICER");
            printerToolStripMenuItem.Text = Trans.T("M_PRINTER");
            temperatureToolStripMenuItem.Text = Trans.T("M_TEMPERATURE");
            helpToolStripMenuItem.Text = Trans.T("M_HELP");
            loadGCodeToolStripMenuItem.Text = Trans.T("M_LOAD_GCODE");
            showWorkdirectoryToolStripMenuItem.Text = Trans.T("M_SHOW_WORKDIRECTORY");
            languageToolStripMenuItem.Text = Trans.T("M_LANGUAGE");
            printerSettingsToolStripMenuItem.Text = Trans.T("M_PRINTER_SETTINGS");
            //eeprom.Text = Trans.T("M_EEPROM_SETTINGS");
            advancedViewConfigurationToolStripMenuItem.Text = Trans.T("M_3D_VIEWER_CONFIGURATION");
            repetierSettingsToolStripMenuItem.Text = Trans.T("M_REPETIER_SETTINGS");
            internalSlicingParameterToolStripMenuItem.Text = Trans.T("M_TESTCASE_SETTINGS");
            soundConfigurationToolStripMenuItem.Text = Trans.T("M_SOUND_CONFIGURATION");
            showExtruderTemperaturesMenuItem.Text = Trans.T("M_SHOW_EXTRUDER_TEMPERATURES");
            showHeatedBedTemperaturesMenuItem.Text = Trans.T("M_SHOW_HEATED_BED_TEMPERATURES");
            showTargetTemperaturesMenuItem.Text = Trans.T("M_SHOW_TARGET_TEMPERATURES");
            showAverageTemperaturesMenuItem.Text = Trans.T("M_SHOW_AVERAGE_TEMPERATURES");
            showHeaterPowerMenuItem.Text = Trans.T("M_SHOW_HEATER_POWER");
            autoscrollTemperatureViewMenuItem.Text = Trans.T("M_AUTOSCROLL_TEMPERATURE_VIEW");
            timeperiodMenuItem.Text = Trans.T("M_TIMEPERIOD");
            temperatureZoomMenuItem.Text = Trans.T("M_TEMPERATURE_ZOOM");
            buildAverageOverMenuItem.Text = Trans.T("M_BUILD_AVERAGE_OVER");
            secondsToolStripMenuItem.Text = Trans.T("M_30_SECONDS");
            minuteToolStripMenuItem.Text = Trans.T("M_1_MINUTE");
            minuteToolStripMenuItem1.Text = Trans.T("M_1_MINUTE");
            minutesToolStripMenuItem.Text = Trans.T("M_2_MINUTES");
            minutesToolStripMenuItem1.Text = Trans.T("M_5_MINUTES");
            minutes5ToolStripMenuItem.Text = Trans.T("M_5_MINUTES");
            minutes10ToolStripMenuItem.Text = Trans.T("M_10_MINUTES");
            minutes15ToolStripMenuItem.Text = Trans.T("M_15_MINUTES");
            minutes30ToolStripMenuItem.Text = Trans.T("M_30_MINUTES");
            minutes60ToolStripMenuItem.Text = Trans.T("M_60_MINUTES");
            continuousMonitoringMenuItem.Text = Trans.T("M_CONTINUOUS_MONITORING");
            disableToolStripMenuItem.Text = Trans.T("M_DISABLE");
            extruder1ToolStripMenuItem.Text = Trans.T("M_EXTRUDER_1");
            extruder2ToolStripMenuItem.Text = Trans.T("M_EXTRUDER_2");
            heatedBedToolStripMenuItem.Text = Trans.T("M_HEATED_BED");
            printerInformationsToolStripMenuItem.Text = Trans.T("M_PRINTER_INFORMATION");
            jobStatusToolStripMenuItem.Text = Trans.T("M_JOB_STATUS");
            menuSDCardManager.Text = Trans.T("M_SD_CARD_MANAGER");
            testCaseGeneratorToolStripMenuItem.Text = Trans.T("M_TEST_CASE_GENERATOR");
            sendScript1ToolStripMenuItem.Text = Trans.T("M_SEND_SCRIPT_1");
            sendScript2ToolStripMenuItem.Text = Trans.T("M_SEND_SCRIPT_2");
            sendScript3ToolStripMenuItem.Text = Trans.T("M_SEND_SCRIPT_3");
            sendScript4ToolStripMenuItem.Text = Trans.T("M_SEND_SCRIPT_4");
            sendScript5ToolStripMenuItem.Text = Trans.T("M_SEND_SCRIPT_5");
            repetierHostHomepageToolStripMenuItem.Text = Trans.T("M_REPETIER_HOST_HOMEPAGE");
            repetierHostDownloadPageToolStripMenuItem.Text = Trans.T("M_REPETIER_HOST_DOWNLOAD_PAGE");
            manualToolStripMenuItem.Text = Trans.T("M_MANUAL");
            slic3rHomepageToolStripMenuItem.Text = Trans.T("M_SLIC3R_HOMEPAGE");
            skeinforgeHomepageToolStripMenuItem.Text = Trans.T("M_SKEINFORGE_HOMEPAGE");
            repRapWebsiteToolStripMenuItem.Text = Trans.T("M_REPRAP_WEBSITE");
            repRapForumToolStripMenuItem.Text = Trans.T("M_REPRAP_FORUM");
            thingiverseNewestToolStripMenuItem.Text = Trans.T("M_THINGIVERSE_NEWEST");
            thingiversePopularToolStripMenuItem.Text = Trans.T("M_THINGIVERSE_POPULAR");
            aboutRepetierHostToolStripMenuItem.Text = Trans.T("M_ABOUT_REPETIER_HOST");
            checkForUpdatesToolStripMenuItem.Text = Trans.T("M_CHECK_FOR_UPDATES");
            quitToolStripMenuItem.Text = Trans.T("M_QUIT");
            donateToolStripMenuItem.Text = Trans.T("M_DONATE");
            //tabPage3DView.Text = Trans.T("TAB_3D_VIEW");
            //tabPageTemp.Text = Trans.T("TAB_TEMPERATURE_CURVE");
            //tabModel.Text = Trans.T("TAB_OBJECT_PLACEMENT");
            //tabSlicer.Text = Trans.T("TAB_SLICER");
            //tabGCode.Text = Trans.T("TAB_GCODE_EDITOR");
            //tabPrint.Text = Trans.T("TAB_MANUAL_CONTROL");
            printerOptionsToolStripMenuItem.Text = Trans.T("M_PRINTER_SETTINGS");
            printerOptionsToolStripMenuItem.ToolTipText = Trans.T("M_PRINTER_SETTINGS");
            emergencyStopStripButton6.Text = Trans.T("M_EMERGENCY_STOP");
            sDCardToolStripMenuItem.Text = Trans.T("M_SD_CARD");
            sDCardToolStripMenuItem.ToolTipText = Trans.T("L_SD_CARD_MANAGEMENT");
            emergencyStopStripButton6.ToolTipText = Trans.T("M_EMERGENCY_STOP");

            // TODO: Add the translation information back in for my new tools. 
            importSTLToolSplitButton1.Text = Trans.T("M_LOAD");
            //toolStripSaveJob.Text = Trans.T("M_SAVE_JOB");
            killJobToolStripMenuItem.Text = Trans.T("M_KILL_JOB");
            sDCardToolStripMenuItem.Text = Trans.T("M_SD_CARD");
            //toolShowLog.Text = toolShowLog.ToolTipText = Trans.T("M_TOGGLE_LOG");

            // TODO: Update this to be something other than "show filament"
            //viewSlicedObjectToolStripMenuItem1.Text = Trans.T("M_SHOW_FILAMENT");


            if (conn.connected)
            {
                connectToolStripSplitButton.ToolTipText = Trans.T("L_DISCONNECT_PRINTER"); // "Disconnect printer";
                connectToolStripSplitButton.Text = Trans.T("M_DISCONNECT"); // "Disconnect";
            }
            else
            {
                connectToolStripSplitButton.ToolTipText = Trans.T("L_CONNECT_PRINTER"); // "Connect printer";
                connectToolStripSplitButton.Text = Trans.T("M_CONNECT"); // "Connect";
            }
            if (threeDSettings.checkDisableFilamentVisualization.Checked)
            {
                // TODO: Update this to be something other than "show filament"
                //viewSlicedObjectToolStripMenuItem1.ToolTipText = Trans.T("L_FILAMENT_VISUALIZATION_DISABLED"); // "Filament visualization disabled";
                //viewSlicedObjectToolStripMenuItem1.Text = Trans.T("M_SHOW_FILAMENT"); // "Show filament";
            }
            else
            {
                // TODO: Update this to be something other than "show filament"
                //viewSlicedObjectToolStripMenuItem1.ToolTipText = Trans.T("L_FILAMENT_VISUALIZATION_ENABLED"); // "Filament visualization enabled";
                //viewSlicedObjectToolStripMenuItem1.Text = Trans.T("M_HIDE_FILAMENT"); // "Hide filament";
            }
            if (conn.job.mode != 1)
            {
                Main.main.printStripSplitButton4.ToolTipText = Trans.T("M_RUN_JOB"); // "Run job";
                Main.main.printStripSplitButton4.Text = Trans.T("M_RUN_JOB"); //"Run job";
            }
            else
            {
                Main.main.printStripSplitButton4.ToolTipText = Trans.T("M_PAUSE_JOB"); //"Pause job";
                Main.main.printStripSplitButton4.Text = Trans.T("M_PAUSE_JOB"); //"Pause job";
            }
            importSTLToolSplitButton1.ToolTipText = Trans.T("L_LOAD_FILE"); // Load file
            //toolStripSaveJob.ToolTipText = Trans.T("M_SAVE_JOB");

            // TODO change to open g-code or .stl translation
            openFileSTLorGcode.Title = Trans.T("L_IMPORT_G_CODE"); // Import G-Code
            saveJobDialog.Title = Trans.T("L_SAVE_G_CODE"); //Save G-Code
            updateTravelMoves();
            updateShowFilament();
            foreach (ToolStripMenuItem item in languageToolStripMenuItem.DropDownItems)
            {
                item.Checked = item.Tag == trans.active;
            }


            // Not sure if this goes here, but it needs to go somewhere. , Anthony Garland
            //toolMove.ToolTipText = Trans.T("L_MOVE_CAMERA");
            //toolMoveObject.ToolTipText = Trans.T("L_MOVE_OBJECT");
            //toolMoveViewpoint.ToolTipText = Trans.T("L_MOVE_VIEWPOINT");
            //toolResetView.ToolTipText = Trans.T("L_RESET_VIEW");
            //toolRotate.ToolTipText = Trans.T("L_ROTATE");
            //toolTopView.ToolTipText = Trans.T("L_TOP_VIEW");
            //toolZoom.ToolTipText = Trans.T("T_ZOOM_VIEW");
            //toolStripClear.ToolTipText = Trans.T("T_CLEAR_OBJECTS");
            //toolParallelProjection.ToolTipText = Trans.T("L_USE_PARALLEL_PROJECTION");

        }

        /// <summary>
        /// If we need to make everything smaller, than in the menus remove the text and only show the image. 
        /// </summary>
        public void UpdateToolbarSize()
        {
            if (globalSettings == null) return;
            bool mini = globalSettings.ReduceToolbarSize;
            foreach (ToolStripItem it in this.toolStrip1.Items)
            {
                if (mini)
                    it.DisplayStyle = ToolStripItemDisplayStyle.Image;
                else
                    it.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            }
        }

        /// <summary>
        /// Handle for the change language event. Basically if it is a different language than the current, than you should change languages. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void languageSelected(object sender, EventArgs e)
        {
            ToolStripItem it = (ToolStripItem)sender;
            trans.selectLanguage((Translation)it.Tag);
            if (languageChanged != null)
                languageChanged();
        }





        /// <summary>
        /// When clicking on recent printers/printer settings update the printer settings objects. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ConnectHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            printerSettings.load(clickedItem.Text);
            printerSettings.formToCon();
            //slicerPanel.UpdateSelection();
            printerSettings.UpdateDimensions();
            //main.Update3D();
            Main.main.mainHelp.UpdateEverythingInMain();
            conn.open();
        }
        public void LoadGCode(string fileName)
        {
            main.fileAddOrRemove.LoadGCode(fileName);
        }

        public void PrinterChanged(RegistryKey pkey, bool printerChanged)
        {
            if (printerChanged && editor != null)
            {
                editor.UpdatePrependAppend();
            }
        }

        /// <summary>
        /// Gets and sets the Title for the application that shows in the top left corner near the icon. It will always say
        /// the basic title plus the name of the current models. 
        /// </summary>
        public string Title
        {
            set { Text = basicTitle + " - " + value; }
            get { return Text; }
        }

        /// <summary>
        /// Bring to the front a particular form. 
        /// </summary>
        /// <param name="f"></param>
        private void FormToFront(Form f)
        {
            f.BringToFront();
        }

        /// <summary>
        /// Exit the application on click "quit"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// If the connection to the prnter changes then update what is available to click. 
        /// </summary>
        /// <param name="msg"></param>
        private void OnPrinterConnectionChange(string msg)
        {
            toolConnection.Text = msg;
            sendScript1ToolStripMenuItem.Enabled = conn.connected;
            sendScript2ToolStripMenuItem.Enabled = conn.connected;
            sendScript3ToolStripMenuItem.Enabled = conn.connected;
            sendScript4ToolStripMenuItem.Enabled = conn.connected;
            sendScript5ToolStripMenuItem.Enabled = conn.connected;
            if (conn.connected)
            {
                connectToolStripSplitButton.Image = imageList.Images[0];
                connectToolStripSplitButton.ToolTipText = Trans.T("L_DISCONNECT_PRINTER"); // "Disconnect printer";
                connectToolStripSplitButton.Text = Trans.T("M_DISCONNECT"); // "Disconnect";
                foreach (ToolStripItem it in connectToolStripSplitButton.DropDownItems)
                    it.Enabled = false;
                //eeprom.Enabled = true;
                emergencyStopStripButton6.Enabled = true;
            }
            else
            {
                connectToolStripSplitButton.Image = imageList.Images[1];
                connectToolStripSplitButton.ToolTipText = Trans.T("L_CONNECT_PRINTER"); // "Connect printer";
                connectToolStripSplitButton.Text = Trans.T("M_CONNECT"); // "Connect";
                //eeprom.Enabled = false;
                continuousMonitoringMenuItem.Enabled = false;
                if (eepromSettings != null && eepromSettings.Visible)
                {
                    eepromSettings.Close();
                    eepromSettings.Dispose();
                    eepromSettings = null;
                }
                if (eepromSettingsm != null && eepromSettingsm.Visible)
                {
                    eepromSettingsm.Close();
                    eepromSettingsm.Dispose();
                    eepromSettingsm = null;
                }
                foreach (ToolStripItem it in connectToolStripSplitButton.DropDownItems)
                    it.Enabled = true;
                emergencyStopStripButton6.Enabled = false;
                SDCard.Disconnected();
            }
        }

        /// <summary>
        /// Update the toolAction text based on some msg
        /// </summary>
        /// <param name="msg"></param>
        private void OnPrinterAction(string msg)
        {
            toolAction.Text = msg;
        }

        /// <summary>
        /// Update the progress bar. 
        /// </summary>
        /// <param name="per"></param>
        private void OnJobProgress(float per)
        {
            toolProgress.Value = (int)per;
        }

        /// <summary>
        /// Configure what is available to click based on the printer connection type. Related ot the eeprom on the printer. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (conn.isRepetier)
            {
                if (eepromSettings != null)
                {
                    if (eepromSettings.Visible)
                    {
                        eepromSettings.BringToFront();
                        return;
                    }
                    else
                    {
                        eepromSettings.Dispose();
                        eepromSettings = null;
                    }
                }
                if (eepromSettings == null)
                    eepromSettings = new EEPROMRepetier();
                eepromSettings.Show2();
            }
            if (conn.isMarlin)
            {
                if (eepromSettingsm != null)
                {
                    if (eepromSettingsm.Visible)
                    {
                        eepromSettingsm.BringToFront();
                        return;
                    }
                    else
                    {
                        eepromSettingsm.Dispose();
                        eepromSettingsm = null;
                    }
                }
                if (eepromSettingsm == null)
                    eepromSettingsm = new EEPROMMarlin();
                eepromSettingsm.Show2();
            }
        }


        /// <summary>
        /// Load g-code or .stl when you click file, load. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolGCodeLoad_Click(object sender, EventArgs e)
        {
            if (openFileSTLorGcode.ShowDialog() == DialogResult.OK)
            {
                this.fileAddOrRemove.LoadGCodeOrSTL(openFileSTLorGcode.FileName);
                // LoadGCodeOrSTL(openGCode.FileName);
            }
        }


        public MethodInvoker StartJob = delegate
        {
            Main.main.toolPrintJob_Click(null, null);
        };
        private void toolPrintJob_Click(object sender, EventArgs e)
        {
            Printjob job = conn.job;
            if (job.dataComplete)
            {
                conn.pause(Trans.T("L_PAUSE_MSG")); //"Press OK to continue.\n\nYou can add pauses in your code with\n@pause Some text like this");
            }
            else
            {
                //tab.SelectedTab = tabPrint;
                current3Dview = ThreeDViewOptions.printing;
                //conn.analyzer.StartJob();
                printStripSplitButton4.Image = imageList.Images[3];
                job.BeginJob();
                job.PushGCodeShortArray(editor.getContentArray(1));
                job.PushGCodeShortArray(editor.getContentArray(0));
                job.PushGCodeShortArray(editor.getContentArray(2));
                job.EndJob();
            }
        }



        private void toolKillJob_Click(object sender, EventArgs e)
        {
            conn.job.KillJob();
        }

        private void printerSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printerSettings.Show(this);
            printerSettings.UpdatePorts();
            FormToFront(printerSettings);
        }

        private void skeinforgeSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skeinforge.Show();
            skeinforge.BringToFront();
        }

        private void skeinforgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skeinforge.RunSkeinforge();
        }

        private void threeDSettingsMenu_Click(object sender, EventArgs e)
        {

        }
        private PrinterInfo printerInfo = null;
        private void printerInformationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (printerInfo == null)
                printerInfo = new PrinterInfo();
            printerInfo.Show();
            printerInfo.BringToFront();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conn.job.mode == 1)
            {
                if (MessageBox.Show(Trans.T("L_REALLY_QUIT"), Trans.T("L_SECURITY_QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
            if (!conn.close())
            {
                e.Cancel = true;
                return;
            }
            RegMemory.StoreWindowPos("mainWindow", this, true, true);
            //RegMemory.SetInt("logSplitterDistance", splitLog.SplitterDistance);
            //RegMemory.SetInt("infoEditSplitterDistance", splitInfoEdit.SplitterDistance);

            // RegMemory.SetBool("logShow", !splitLog.Panel2Collapsed);

            if (previewThread != null)
                previewThread.Join();
            conn.Destroy();
        }

        private void repetierSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            globalSettings.Show(this);
            globalSettings.BringToFront();
        }
        public About about = null;
        private void aboutRepetierHostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (about == null) about = new About();
            about.Show(this);
            about.BringToFront();
        }

        private void jobStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JobStatus.ShowStatus();
        }
        public void openLink(string link)
        {
            try
            {
                System.Diagnostics.Process.Start(link);
            }
            catch
            (
            System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }
        private void repetierHostHomepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Change this. 
            openLink("http://www.repetier.com");
        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openLink("http://www.repetier.com/documentation/repetier-host/");
        }
        public MethodInvoker FirmwareDetected = delegate
        {
            Main.main.printPanel.UpdateConStatus(true);
            if (conn.isRepetier)
            {
                Main.main.continuousMonitoringMenuItem.Enabled = true;
            }
        };


        public MethodInvoker UpdateEEPROM = delegate
        {
            if (conn.isMarlin || conn.isRepetier) // Activate special menus and function
            {
                //main.eeprom.Enabled = true;
            }
            //else main.eeprom.Enabled = false;

        };
        /*  private void toolStripSaveGCode_Click(object sender, EventArgs e)
          {
              if (saveJobDialog.ShowDialog() == DialogResult.OK)
              {
                  System.IO.File.WriteAllText(saveJobDialog.FileName, textGCode.Text, Encoding.Default);
              }
          }

          private void toolStripSavePrepend_Click(object sender, EventArgs e)
          {
              printerSettings.currentPrinterKey.SetValue("gcodePrepend", textGCodePrepend.Text);
          }

          private void toolStripSaveAppend_Click(object sender, EventArgs e)
          {
              printerSettings.currentPrinterKey.SetValue("gcodeAppend", textGCodeAppend.Text);
          }*/
        private void JobPreview()
        {
            if (editor.autopreview == false) return;
            /*       if (splitJob.Panel2Collapsed)
                   {
                       splitJob.Panel2Collapsed = false;
                       splitJob.SplitterDistance = 300;
                       jobPreview = new ThreeDControl();
                       jobPreview.Dock = DockStyle.Fill;
                       splitJob.Panel2.Controls.Add(jobPreview);
                       jobPreview.SetEditor(false);
                       jobPreview.models.AddLast(jobVisual);
                       //jobPreview.SetObjectSelected(false);
                   }*/
            /* Read the initial time. */
            recalcJobPreview = true;
            /*DateTime startTime = DateTime.Now;
            jobVisual.ParseText(editor.getContent(1), true);
            jobVisual.ParseText(editor.getContent(0), false);
            jobVisual.ParseText(editor.getContent(2), false);
            DateTime stopTime = DateTime.Now;
            TimeSpan duration = stopTime - startTime;
            Main.conn.log(duration.ToString(), false, 3);
            jobPreview.UpdateChanges();*/
        }


        private void testCaseGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestGenerator.Execute();
        }

        private void internalSlicingParameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SlicingParameter.Execute();
        }

        private void toolStripSDCard_Click(object sender, EventArgs e)
        {
            SDCard.Execute();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (newVisual != null)
            {
                gcodePreviewView.models.RemoveLast();
                jobVisual.Clear();
                jobVisual = newVisual;
                gcodePreviewView.models.AddLast(jobVisual);
                threedview.UpdateChanges();
                newVisual = null;
                editor.toolUpdating.Text = "";
                if (Main.main.gcodePrintingTime > 0)
                {
                    Main.main.editor.printingTime = Main.main.gcodePrintingTime;
                    int sec = (int)(Main.main.editor.printingTime * (1 + 0.01 * Main.conn.addPrintingTime));
                    int hours = sec / 3600;
                    sec -= 3600 * hours;
                    int min = sec / 60;
                    sec -= min * 60;
                    StringBuilder s = new StringBuilder();
                    if (hours > 0)
                        s.Append(Trans.T1("L_TIME_H:", hours.ToString())); //"h:");
                    if (min > 0)
                        s.Append(Trans.T1("L_TIME_M:", min.ToString()));
                    s.Append(Trans.T1("L_TIME_S", sec.ToString()));
                    Main.main.editor.toolPrintingTime.Text = Trans.T1("L_PRINTING_TIME:", s.ToString());
                }

                editor.UpdateLayerInfo();
                editor.MaxLayer = editor.getContentArray(0).Last<GCodeShort>().layer;
            }
            if (recalcJobPreview && jobPreviewThreadFinished)
            {
                previewArray0 = new List<GCodeShort>();
                previewArray1 = new List<GCodeShort>();
                previewArray2 = new List<GCodeShort>();
                previewArray0.AddRange(((RepetierEditor.Content)editor.toolFile.Items[1]).textArray);
                previewArray1.AddRange(((RepetierEditor.Content)editor.toolFile.Items[0]).textArray);
                previewArray2.AddRange(((RepetierEditor.Content)editor.toolFile.Items[2]).textArray);
                recalcJobPreview = false;
                jobPreviewThreadFinished = false;
                JobUpdater workerObject = new JobUpdater();
                editor.toolUpdating.Text = Trans.T("L_UPDATING..."); // "Updating ...";
                previewThread = new Thread(workerObject.DoWork);
                previewThread.Start();

            }
            if (refreshCounter > 0)
            {
                if (--refreshCounter == 0)
                {
                    Invalidate();
                }
            }
        }

        private void toolConnect_Click(object sender, EventArgs e)
        {
            if (conn.connected)
            {
                conn.close();
            }
            else
            {
                conn.open();
            }
        }

        //private void toolShowLog_Click(object sender, EventArgs e)
        //{
        //    //if (splitLog.Panel2Collapsed == true)
        //    //{
        //    //    splitLog.Panel2Collapsed = false;
        //    //}
        //    //else
        //    //{
        //    //    splitLog.Panel2Collapsed = true;
        //    //}            
        //    //toolShowLog.Checked = !toolShowLog.Checked;
        //}

        private void toolShowLog_CheckedChanged(object sender, EventArgs e)
        {
            //if (splitLog.Panel2Collapsed == true)
            //{
            //    splitLog.Panel2Collapsed = false;
            //}
            //else
            //{
            //    splitLog.Panel2Collapsed = true;
            //}
        }

        private void repRapWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openLink("http://www.reprap.org");
        }

        private void repRapForumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openLink("http://forum.reprap.org");
        }

        private void slic3rHomepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openLink("http://www.slic3r.org");

        }

        private void skeinforgeHomepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openLink("http://fabmetheus.crsndoo.com/");

        }

        private void thingiverseNewestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openLink("http://www.thingiverse.com/newest");

        }

        private void thingiversePopularToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openLink("http://www.thingiverse.com/popular");

        }

        private void slic3rToolStripMenuItem_Click(object sender, EventArgs e)
        {
            slicer.ActiveSlicer = Slicer.SlicerID.Slic3r;
            // TODO: Add this translation back
            //stlComposer1.buttonSlice.Text = Trans.T1("L_SLICE_WITH", slicer.SlicerName);
        }

        private void skeinforgeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            slicer.ActiveSlicer = Slicer.SlicerID.Skeinforge;
            //TODO: add this translation back
            //stlComposer1.buttonSlice.Text = Trans.T1("L_SLICE_WITH", slicer.SlicerName);
        }

        private void slic3rConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            slic3r.Show();
            slic3r.BringToFront();
        }





        //private void tab_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //Console.WriteLine("index changed " + Environment.OSVersion.Platform + " Mac=" + PlatformID.MacOSX);
        //    //if (Environment.OSVersion.Platform == PlatformID.MacOSX )
        //    if (IsMac)
        //    {
        //        // In MacOSX the OpenGL windows shine through the
        //        // tabs, so we need to disable all GL windows except the active.
        //        //if (tab.SelectedTab != tabModel)
        //        //{
        //        //    if (tabModel.Controls.Contains(stlComposer1))
        //        //    {
        //        //        tabModel.Controls.Remove(stlComposer1);
        //        //    }
        //        //}
        //        //if (tab.SelectedTab == tabModel)
        //        //{
        //        //    if (!tabModel.Controls.Contains(stlComposer1))
        //        //        tabModel.Controls.Add(stlComposer1);
        //        //}
        //        refreshCounter = 6;
        //    }
        //    //if (tab.SelectedTab == tabModel || tab.SelectedTab == tabSlicer)
        //    //{
        //    //    tabControlView.SelectedIndex = 0;
        //    //}
        //    update3DviewSelection();
        //}

        private void Main_Resize(object sender, EventArgs e)
        {
            if (IsMac)
            {
                if (Height < 740) Height = 740;
                refreshCounter = 8;
                Application.DoEvents();
                /*             Invalidate();
                             Application.DoEvents();
                             tab.SelectedTab.Invalidate();*/
            }
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            if (!globalSettings.WorkdirOK())
                globalSettings.Show();
            if (Custom.GetBool("showGCodeExample", false) && RegMemory.GetBool("gcodeExampleShown", false) == false)
            {
                string file = Application.StartupPath + Path.DirectorySeparatorChar + Custom.GetString("GCodeExample", "");
                if (File.Exists(file))
                    // TODO: Fix this
                    //LoadGCodeOrSTL(file);
                    RegMemory.SetBool("gcodeExampleShown", true);
            }
        }
        public void executeHostCommand(GCode code)
        {
            string com = code.getHostCommand();
            string param = code.getHostParameter();
            if (com.Equals("@info"))
            {
                conn.log(param, false, 3);
            }
            else if (com.Equals("@pause"))
            {
                SoundConfig.PlayPrintPaused(false);
                conn.pause(param);
            }
            else if (com.Equals("@sound"))
            {
                SoundConfig.PlaySoundCommand(false);
            }
            else if (com.Equals("@execute"))
            {
                CommandExecutioner ce = new CommandExecutioner();
                ce.setExeArgs(code.getHostParameter());
                ce.run();
            }
        }

        /// <summary>
        /// Toggles whether the show filament visualization is on or off. 
        /// TODO: Update this with the new pictures in teh image list. 
        /// </summary>
        public void updateShowFilament()
        {
            // TODO: Don't need this any more. Maybe we could put an icon at the bottom that indicates the current configuration of the filament. 
            //if (threeDSettings.checkDisableFilamentVisualization.Checked)
            //{
            //    toolShowFilament.Image = imageList.Images[5];
            //    toolShowFilament.ToolTipText = Trans.T("L_FILAMENT_VISUALIZATION_DISABLED"); // "Filament visualization disabled";
            //    toolShowFilament.Text = Trans.T("M_HIDE_FILAMENT"); // "Show filament";
            //}
            //else
            //{
            //    toolShowFilament.Image = imageList.Images[4];
            //    toolShowFilament.ToolTipText = Trans.T("L_FILAMENT_VISUALIZATION_ENABLED"); // "Filament visualization enabled";
            //    toolShowFilament.Text = Trans.T("M_SHOW_FILAMENT"); // "Hide filament";
            //}
        }

        /// <summary>
        /// Toggles the Update Travel on or off. 
        /// TODO: Update the images with custom images. 
        /// </summary>
        public void updateTravelMoves()
        {
            if (threeDSettings == null) return;
            if (threeDSettings.checkDisableTravelMoves.Checked)
            {
                // TODO: Might need to fix this
                //toolShowTravel.Image = imageList.Images[5];
                //toolShowTravel.ToolTipText = Trans.T("L_TRAVEL_VISUALIZATION_DISABLED"); // "Travel visualization disabled";
                //toolShowTravel.Text = Trans.T("M_HIDE_TRAVEL"); // "Hide Travel";
            }
            else
            {
                // TODO might need to fix this. 
                //toolShowTravel.Image = imageList.Images[4];
                //toolShowTravel.ToolTipText = Trans.T("L_TRAVEL_VISUALIZATION_ENABLED"); // "Travel visualization enabled";
                //toolShowTravel.Text = Trans.T("M_SHOW_TRAVEL"); // "Show Travel";
            }

            // TODO might need to update this. Might be imprtant related to how it draws things. 
            // toolShowTravel.Visible = threeDSettings.drawMethod == 2;
        }
        private void toolShowFilament_Click(object sender, EventArgs e)
        {
            threeDSettings.checkDisableFilamentVisualization.Checked = !threeDSettings.checkDisableFilamentVisualization.Checked;
            // updateShowFilament();
        }

        private void toolStripEmergencyButton_Click(object sender, EventArgs e)
        {
            if (!conn.connected) return;
            conn.injectManualCommandFirst("M112");
            conn.job.KillJob();
            conn.serial.DtrEnable = false;
            //conn.serial.RtsEnable = true;
            Thread.Sleep(200);
            //conn.serial.RtsEnable = false;
            conn.serial.DtrEnable = true;
            Thread.Sleep(200);
            conn.serial.DtrEnable = false;
            conn.log(Trans.T("L_EMERGENCY_STOP_MSG"), false, 3);
            while (conn.hasInjectedMCommand(112))
            {
                Application.DoEvents();
            }
            //conn.close();
        }

        private void killSlicingProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skeinforge.KillSlice();
            slic3r.KillSlice();
            SlicingInfo.Stop();
        }

        private void externalSlic3rSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Slic3rSetup.Execute();
        }

        private void externalSlic3rToolStripMenuItem_Click(object sender, EventArgs e)
        {
            slicer.ActiveSlicer = Slicer.SlicerID.Slic3rExternal;
            // TODO: add this translation back
            //stlComposer1.buttonSlice.Text = Trans.T1("L_SLICE_WITH", slicer.SlicerName);
        }

        private void externalSlic3rConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            slic3r.RunExternalConfig();
        }

        private void Main_Activated(object sender, EventArgs e)
        {
            fileAddOrRemove.recheckChangedFiles();

            //slicerPanel.UpdateSelection();
        }
        public void selectTimePeriod(object sender, EventArgs e)
        {
            history.CurrentPos = (int)((ToolStripMenuItem)sender).Tag;
        }
        public void selectAverage(object sender, EventArgs e)
        {
            history.AvgPeriod = int.Parse(((ToolStripMenuItem)sender).Tag.ToString());
        }
        public void selectZoom(object sender, EventArgs e)
        {
            history.CurrentZoomLevel = int.Parse(((ToolStripMenuItem)sender).Tag.ToString());
        }

        private void showExtruderTemperaturesMenuItem_Click(object sender, EventArgs e)
        {
            history.ShowExtruder = !history.ShowExtruder;
        }

        private void showHeatedBedTemperaturesMenuItem_Click(object sender, EventArgs e)
        {
            history.ShowBed = !history.ShowBed;
        }

        private void showTargetTemperaturesMenuItem_Click(object sender, EventArgs e)
        {
            history.ShowTarget = !history.ShowTarget;
        }

        private void showAverageTemperaturesMenuItem_Click(object sender, EventArgs e)
        {
            history.ShowAverage = !history.ShowAverage;
        }

        private void showHeaterPowerMenuItem_Click(object sender, EventArgs e)
        {
            history.ShowOutput = !history.ShowOutput;
        }

        private void autoscrollTemperatureViewMenuItem_Click(object sender, EventArgs e)
        {
            history.Autoscroll = !history.Autoscroll;
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conn.injectManualCommand("M203 S255");
        }

        private void extruder1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conn.injectManualCommand("M203 S0");
        }

        private void extruder2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conn.injectManualCommand("M203 S1");
        }

        private void heatedBedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conn.injectManualCommand("M203 S100");
        }

        private void showWorkdirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Main.globalSettings.Workdir))
                Process.Start("explorer.exe", Main.globalSettings.Workdir);
        }

        private void soundConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoundConfig.config.ShowDialog();
        }

        private void toolStripSaveJob_Click(object sender, EventArgs e)
        {
            StoreCode.Execute();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            //if (tabControlView.SelectedIndex == 0)
            //{
            threedview.ThreeDControl_KeyDown(sender, e);
            this.postionGUI.listSTLObjects_SelectedIndexChanged(sender, e);           
            //}
        }

        public void repetierHostDownloadPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openLink(Custom.GetString("downloadUrl", "http://www.repetier.com/download/"));
        }

        private void sendScript1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GCodeShort code in editor.getContentArray(5))
            {
                conn.injectManualCommand(code.text);
            }
        }

        private void sendScript2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GCodeShort code in editor.getContentArray(6))
            {
                conn.injectManualCommand(code.text);
            }
        }

        private void sendScript3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GCodeShort code in editor.getContentArray(7))
            {
                conn.injectManualCommand(code.text);
            }
        }

        private void sendScript4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GCodeShort code in editor.getContentArray(8))
            {
                conn.injectManualCommand(code.text);
            }
        }

        private void sendScript5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GCodeShort code in editor.getContentArray(9))
            {
                conn.injectManualCommand(code.text);
            }
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RHUpdater.checkForUpdates(false);
        }


        static bool firstSizeCall = true;
        private void Main_SizeChanged(object sender, EventArgs e)
        {
            if (firstSizeCall)
            {
                firstSizeCall = false;
                //splitLog.SplitterDistance = RegMemory.GetInt("logSplitterDistance", splitLog.SplitterDistance);
                //splitInfoEdit.SplitterDistance = RegMemory.GetInt("infoEditSplitterDistance", Width - 470);
                ////A bug causes the splitter to throw an exception if the PanelMinSize is set too soon.
                //splitInfoEdit.Panel2MinSize = Main.InfoPanel2MinSize;
            }
        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openLink("http://www.repetier.com/donate-or-support/");
        }

        private void toolShowTravel_Click(object sender, EventArgs e)
        {
            threeDSettings.checkDisableTravelMoves.Checked = !threeDSettings.checkDisableTravelMoves.Checked;
            threeDSettings.FormToRegistry();
        }

        private void slicerPanel_Load(object sender, EventArgs e)
        {

        }

        private void toolAction_Click(object sender, EventArgs e)
        {
            conn.job.etaModeNormal = !conn.job.etaModeNormal;
        }

        private void supportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openLink(Custom.GetString("extraSupportURL", "http://www.repetier.com"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;



            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    //stlComposer1.openAndAddObject(openFileDialog1.FileName);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }


        }

        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set the Control to RorateMode
            this.threedview.SetMode(0);

            // string message = sender.ToString();        
            //const string caption = "Test";
            //    var result = MessageBox.Show(message, caption,
            //                     MessageBoxButtons.YesNo,
            //                     MessageBoxIcon.Question);

        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        private void perspectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }





        private void importSTLToolSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            if (openFileSTLorGcode.ShowDialog() == DialogResult.OK)
            {
                this.fileAddOrRemove.LoadGCodeOrSTL(openFileSTLorGcode.FileName);
                // LoadGCodeOrSTL(openGCode.FileName);
            }

            //if (openGCode.ShowDialog() == DialogResult.OK)
            //{
            //    this.fileAddOrRemove.LoadGCodeOrSTL(openGCode.FileName);
            //}            

            //this.fileAddOrRemove.AddAFile();
        }



        private void listSTLObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.postionGUI.listSTLObjects_SelectedIndexChanged(sender, e);           
        }

        public void listSTLObjects_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                for (int i = 0; i < listSTLObjects.Items.Count; i++)
                    listSTLObjects.SetSelected(i, true);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                this.fileAddOrRemove.removeObject();
                //buttonRemoveSTL_Click(sender, null);
                //fileAddOrRemove.removeObject();
                e.Handled = true;
            }
        }

        

        private void moveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //TODO: Create a move dialog box similar to the stlComposer that lets you specifiy how much you want to move the object. 
        }

        private void topViewStripButton1_Click(object sender, EventArgs e)
        {
            this.threedview.GoToTopView();
        }

        private void frontStripButton1_Click(object sender, EventArgs e)
        {
            this.threedview.GoToFrontView();
        }

        private void sideViewStripButton1_Click(object sender, EventArgs e)
        {
            this.threedview.GoToSideView();
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StoreCode.Execute();
        }

        private void emergencyStopStripButton6_Click(object sender, EventArgs e)
        {
            if (!conn.connected) return;
            conn.injectManualCommandFirst("M112");
            conn.job.KillJob();
            conn.serial.DtrEnable = false;
            //conn.serial.RtsEnable = true;
            Thread.Sleep(200);
            //conn.serial.RtsEnable = false;
            conn.serial.DtrEnable = true;
            Thread.Sleep(200);
            conn.serial.DtrEnable = false;
            conn.log(Trans.T("L_EMERGENCY_STOP_MSG"), false, 3);
            while (conn.hasInjectedMCommand(112))
            {
                Application.DoEvents();
            }
            //conn.close();
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            this.threedview.SetMode(0);
        }

        private void moveStripMenuItem12_Click(object sender, EventArgs e)
        {
            // Set the Control mode to move Mode
            this.threedview.SetMode(2);
        }

        private void zoomStripMenuItem13_Click(object sender, EventArgs e)
        {
            this.threedview.SetMode(3);
        }

        private void perspectiveStripMenuItem14_Click(object sender, EventArgs e)
        {
            this.threedview.ChangeProspectiveMode();
        }

        private void resetStripMenuItem15_Click(object sender, EventArgs e)
        {
            this.threedview.ResetView();
        }

        private void advancedViewConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            threeDSettings.Show();
            threeDSettings.BringToFront();
        }

        private void stopSlicerToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void viewSlicedObjectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // TODO: This isn't working. 
            // We want to show the sliced results by default when the slicer finishes. 
            threeDSettings.checkDisableFilamentVisualization.Checked = !threeDSettings.checkDisableFilamentVisualization.Checked;
        }

        private void viewPrintPathToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            threeDSettings.checkDisableTravelMoves.Checked = !threeDSettings.checkDisableTravelMoves.Checked;
            threeDSettings.FormToRegistry();
        }

        private void sliceToolSplitButton3_ButtonClick(object sender, EventArgs e)
        {
            this.fileAddOrRemove.Slice();
        }

        private void removeObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Remove this button. Make it so if you select an objet and then hit delete, that it removes it. 
            // Maybe also pop up with a text box asking if you really want to remove it. 
            this.fileAddOrRemove.removeObject();
            //fileAddOrRemove.changeSelectionBoxSize();
            main.mainHelp.UpdateEverythingInMain();
        }

        //private void copyObjectToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    this.postionGUI.CopyObjects();
        //}

        //private void autopositionToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    this.postionGUI.Autoposition();
        //}

        //private void centerObjectToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    // TODO: Error when moving to a new location. The box doesn't follow it. 
        //    this.fileAddOrRemove.CenterObject();
        //}

        //private void scaleToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    //TODO: Make something here about scaling
        //}

        //private void rotateToolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    //TODO: Make something here about rotating
        //}

        private void setPathToSlicerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Slic3rSetup.Execute();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            current3Dview = ThreeDViewOptions.gcode;
            //update3DviewSelection();
            mainHelp.UpdateEverythingInMain();
            //threedview.SetView(jobPreview);
            //JobPreview();
            //threedview.SetView(fileAddOrRemove.cont);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            threedview.SetView(fileAddOrRemove.stleditorView);
        }

        /// <summary>
        /// What to do when the print button is clicked. 
        /// TODO: Change the availablity of being able to click this and the image based on whether g-code that an be printed is avaible. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printStripSplitButton4_ButtonClick(object sender, EventArgs e)
        {
            Printjob job = conn.job;
            if (job.dataComplete)
            {
                conn.pause(Trans.T("L_PAUSE_MSG")); //"Press OK to continue.\n\nYou can add pauses in your code with\n@pause Some text like this");
            }
            else
            {
                //tab.SelectedTab = tabPrint;
                conn.analyzer.StartJob();

                // TODO: Uncomment this section. The button should not be pushed unless a job is ready. 
                printStripSplitButton4.Image = imageList.Images[3];
                job.BeginJob();
                job.PushGCodeShortArray(editor.getContentArray(1));
                job.PushGCodeShortArray(editor.getContentArray(0));
                job.PushGCodeShortArray(editor.getContentArray(2));
                job.EndJob();
            }
        }

        private void killJobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conn.job.KillJob();
        }

        private void positionToolSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            //positionModelGUI.Dock = DockStyle.Right;
            //positionModelGUIInterface.
            //positionModelGUI.
            //positionModelGUI.Size = new System.Drawing.Size(200, 200);
            //positionModelGUI.Location = new Point(200, 200);
            //System.Drawing.Size s =  positionModelGUI.Size;

            //mainHelp.updatePositionControlLocation();
            //positionModelGUI.Visible = !positionModelGUI.Visible;
            //positionModelGUI.BringToFront();
            
            //positionModelGUI.Show();
            // TODO: When the position tab is selected, the Slicer-options are not avaible, When the Slicer mode is selected, the position options are not avaible. 
            // The print tab is not avaible when in the position mode and only after you slice the model. Also the print tab is not available unless connected. 
            //TODO: Remove the option to click this when in G-code mode. 
        }

        private void importSTLToolSplitButton1_ButtonClick_1(object sender, EventArgs e)
        {
            if (openFileSTLorGcode.ShowDialog() == DialogResult.OK)
            {
                this.fileAddOrRemove.LoadGCodeOrSTL(openFileSTLorGcode.FileName);
            }
        }

        private void loggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (logform.IsDisposed == true)
            {
                logView = new LogView();
                logform = new Form();
                logView.Dock = DockStyle.Fill;
                logform.Controls.Add(logView);
                logform.Visible = true;

            }
            else
                logform.Visible = !logform.Visible;
            //logform.Show();
            //logView.Show();
        }

        private void sDCardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SDCard.Execute();
        }

        private void printerOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printerSettings.Show(this);
            printerSettings.UpdatePorts();
            FormToFront(printerSettings);
        }

        private void helpSplitButton3_ButtonClick(object sender, EventArgs e)
        {
            openLink(Custom.GetString("extraSupportURL", "http://www.by3dp.com  "));
        }

        private void advancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            extraForm.Controls.Add(editor);
            extraForm.Size = new Size(640, 530);
            extraForm.Visible = !extraForm.Visible;
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            if (current3Dview == ThreeDViewOptions.STLeditor)
                current3Dview = ThreeDViewOptions.gcode;
            else if (current3Dview == ThreeDViewOptions.gcode)
                current3Dview = ThreeDViewOptions.STLeditor;

            
            //update3DviewSelection();
            mainHelp.UpdateEverythingInMain();
        }

        private void connectToolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            // TODO: Select COM port
            if (conn.connected)
            {
                conn.close();
            }
            else
            {
                conn.open();
            }
        }

        private void languageToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void manualControlToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //extraForm.Controls.Clear();
            //extraForm.Controls.Add(printPanel);
            //extraForm.Size = new Size(800, 600);
            //extraForm.Visible = !extraForm.Visible;
            printPanel.Left = (this.Width - printPanel.Width) / 2;
            printPanel.Top = (this.Height - printPanel.Height) / 2;
            printPanel.Visible = !printPanel.Visible;

           // if(printPanel.Visible==true)
                printPanel.BringToFront();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void positionToolSplitButton2_Click(object sender, EventArgs e)
        {

            mainHelp.updatePositionControlLocation();

            postionGUI.Visible = !postionGUI.Visible;
            postionGUI.BringToFront();
        }

        private void toolStripMenuItem11_Click_1(object sender, EventArgs e)
        {
            if (saveSTL.ShowDialog() == DialogResult.OK)
            {
                this.fileAddOrRemove.saveComposition(saveSTL.FileName);
            }
        }

        private void advancedConfigStripSplitButton3_ButtonClick(object sender, EventArgs e)
        {

        }

        private void slicerSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Slic3rSetup.Execute();
        }

        private void gcodeEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.Left = (this.Width - editor.Width) / 2;
            editor.Top = (this.Height - editor.Height) / 2;
            editor.Visible = !editor.Visible;
            editor.BringToFront();
            

        }

        private void stopSlicingProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skeinforge.KillSlice();
            slic3r.KillSlice();
            SlicingInfo.Stop();
        }

        private void loadAFileMenuModeMenuOption_Click(object sender, EventArgs e)
        {
            this.current3Dview = ThreeDViewOptions.loadAFile;
            this.mainHelp.UpdateEverythingInMain();
        }

        private void STLEditorMenuOption_Click(object sender, EventArgs e)
        {
            this.current3Dview = ThreeDViewOptions.STLeditor;
            this.mainHelp.UpdateEverythingInMain();
        }

        private void gCodeVisualizationMenuOption_Click(object sender, EventArgs e)
        {
            this.current3Dview = ThreeDViewOptions.gcode;
            this.mainHelp.UpdateEverythingInMain();
        }

        private void livePrintingMenuOption_Click(object sender, EventArgs e)
        {
            this.current3Dview = ThreeDViewOptions.printing;
            this.mainHelp.UpdateEverythingInMain();
        }

     
    }
}
