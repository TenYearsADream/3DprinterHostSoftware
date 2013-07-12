//-----------------------------------------------------------------------
// <copyright file="Main.cs" company="Baoyan">
//   Some parts of this file were derived from Repetier Host which can be found at
// https://github.com/repetier/Repetier-Host Which is licensed using the Apache 2.0 license. 
// 
// Other parts of the file are property of Baoyan Automation LTC, Nanjing Jiangsu China.// 
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-----------------------------------------------------------------------
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
    /// <summary>
    /// Helps with the printer connection. Allows the connection thread to call something in the main thread. 
    /// Not totally sure what it does??
    /// </summary>
    /// <param name="code">The g-code to use??</param>
    public delegate void executeHostCommandDelegate(GCode code);

    /// <summary>
    /// Event when the language is changed. 
    /// </summary>
    public delegate void languageChangedEvent();


    /// <summary>
    /// Main is the form that shows the main user GUI. As members that are the other important user forms and user controls and 
    /// other helpful objects. 
    /// </summary>
    public partial class Main : Form
    {
        /// <summary>
        /// Defines the event when the user changes the language. 
        /// </summary>
        public event languageChangedEvent languageChanged;

        /// <summary>
        /// The main printer connection object. 
        /// </summary>
        public static PrinterConnection connection;

        /// <summary>
        /// A reference to itself. The main is the primary user interface. 
        /// </summary>
        public static Main main;

        /// <summary>
        /// Form that is created in the main object that points to the form used to show printer Settings. 
        /// </summary>
        public static FormPrinterSettings printerSettings;

        /// <summary>
        /// Helps with saving information about the printer setup to the registry. 
        /// </summary>
        public static PrinterModel printerModel;

        /// <summary>
        /// Shows the settings related to how to show the 3D model. 
        /// </summary>
        public static ThreeDSettings threeDSettings;

        /// <summary>
        /// Form which shows and allows the user to set information related to global settings. Such as the current working directory. 
        /// </summary>
        public static GlobalSettings globalSettings = null;

        /// <summary>
        /// Code related to generating g-code from .stl files. 
        /// </summary>
        public static GCodeGenerator generator = null;

        /// <summary>
        /// A string that is related to the text in the top left corner of the windows form. It changes based on what the name
        /// of the current .stl file is. 
        /// </summary>
        public string basicTitle = "";

        /// <summary>
        /// Not really sure what this is. Maybe monochrome (black and white) on some basic linux computers. 
        /// Not important for Windows development. 
        /// </summary>
        public static bool IsMono = Type.GetType("Mono.Runtime") != null;

        /// <summary>
        /// Class that helps manage the non-slicer specific information. 
        /// </summary>
        public static Slicer slicer = null;

        /// <summary>
        /// Code that helps setup slic3r. 
        /// </summary>
        public static Slic3r slic3r = null;

        /// <summary>
        /// Used to indicated if running on a Mac (Apple) computer. 
        /// </summary>
        public static bool IsMac = false;

        /// <summary>
        /// Form used to show slic3r configuration information. 
        /// </summary>
        public Form slicerPanaelForm = null;

        /// <summary>
        /// The User control that lets the user pick the slic3r settings that are saved as .ini files 
        /// </summary>
        public SlicerPanel slicerPanel = null;

        /// <summary>
        /// A User control that lets the user move, rotate, and scale the selected .stl object. 
        /// </summary>
        public PositionSTLGUI postionGUI = null;

        /// <summary>
        /// The form that allows the user to see and edit the gcode. 
        /// </summary>
        public Form gCodeEditorForm = null;

        /// <summary>
        /// The form that lets the user see the log information. 
        /// </summary>
        public Form logform = null;

        /// <summary>
        /// A class related to updating things in the program. Often these methods are called after events. 
        /// </summary>
        public MainUpdaterHelper mainUpdaterHelper = null;

        /// <summary>
        /// Object that helps manage skeinforge. I'm not using skeinforge right now. 
        /// </summary>
        public Skeinforge skeinforge = null;

        /// <summary>
        /// Form related to modifying the eeprom of the particular printer. 
        /// </summary>
        public EEPROMRepetier eepromSettings = null;

        /// <summary>
        /// Form related to configuring the EEPROM on the Marlin 3d printer. 
        /// </summary>
        public EEPROMMarlin eepromSettingsm = null;

        /// <summary>
        /// User control that shows the scrolling log. The controller for the log form object.
        /// </summary>
        public LogView logView = null;

        /// <summary>
        /// The user form that lets the user manually control the printer. 
        /// </summary>
        public ManualPrinterControl manulControl = null;

        /// <summary>
        /// The registry Key that is used. 
        /// </summary>
        public RegistryKey repetierKey;

        /// <summary>
        /// OpenGL wrapper that handles the openGL code. Graphics handler. 
        /// </summary>
        public ThreeDControl threedview = null;

        /// <summary>
        /// 3D view saved for the G-code print preview. 
        /// </summary>
        public ThreeDView gcodePreviewView = null;

        /// <summary>
        /// The 3D view saved of the live printing. 
        /// </summary>
        public ThreeDView livePrintingView = null;

        /// <summary>
        /// G-code graphics handler that helps show the result of some G-code. Shows what the "job" will look like. 
        /// </summary>
        public GCodeVisual jobVisual = new GCodeVisual();

        /// <summary>
        /// G-code live visualization handler. 
        /// </summary>
        public GCodeVisual printVisual = null;

        /// <summary>
        /// Another G-code live visualization handler. Maybe accessed by several threads. 
        /// </summary>
        public volatile GCodeVisual newVisual = null;

        /// <summary>
        /// Not sure what this does?? something related to updating the live printing g-code. Can be accessed by more than one thread. 
        /// </summary>
        public volatile bool jobPreviewThreadFinished = true;

        /// <summary>
        /// Thread for updating the live printing view. 
        /// </summary>
        public volatile Thread previewThread = null;

        /// <summary>
        /// Object that helps mange the file history information that is in the registry. 
        /// </summary>
        public RegMemory.FilesHistory fileHistory = new RegMemory.FilesHistory("fileHistory", 8);

        /// <summary>
        /// Not sure what this does.  TODO
        /// </summary>
        public int refreshCounter = 0;

        /// <summary>
        /// Pointer to some code. TODO
        /// </summary>
        public executeHostCommandDelegate executeHostCall;

        /// <summary>
        /// Not user what this does. TODO. 
        /// </summary>
        bool recalcJobPreview = false;

        /// <summary>
        /// Not sure what these do. Something related to updating the live view. TODO:
        /// </summary>
        List<GCodeShort> previewArray0, previewArray1, previewArray2;

        /// <summary>
        /// Temperature History manager. Not really using this right now. 
        /// </summary>
        public TemperatureHistory history = null;

        /// <summary>
        /// Temperature History user Control. 
        /// </summary>
        public TemperatureView tempView = null;

        /// <summary>
        /// Translation object. 
        /// </summary>
        public Trans trans = null;

        /// <summary>
        /// G-code editor. User Control.  
        /// </summary>
        public RepetierHost.view.RepetierEditor editor;

        /// <summary>
        /// Not sure what this is. 
        /// </summary>
        public double gcodePrintingTime = 0;

        /// <summary>
        /// Object that helps with adding and removing of files both .stl and gcode. 
        /// </summary>
        public FileAddOrRemove fileAddOrRemove = null;

        /// <summary>
        /// Windows Form that guides the user through setting up the Z height of their printer. 
        /// </summary>
        private Calibration calibrationZ;

        /// <summary>
        /// Options for the current view mode. 
        /// </summary>
        public enum ThreeDViewOptions
        { 
            /// <summary>
            /// Loads a file. Only the load a file button should be enabled
            /// </summary>
            loadAFile,

            /// <summary>
            /// Allows the user to manipulate, add, copy, scale, move .stl files on the printer platform
            /// </summary>
            STLeditor, 

            /// <summary>
            /// Shows gcode preview of how the software thinks the printer will move and place material. 
            /// </summary>
            gcode, 

            /// <summary>
            /// Show the live printing of what the printer is doing. 
            /// </summary>
            livePrinting 
        };

        /// <summary>
        /// The current 3D view mode. 
        /// </summary>
        public ThreeDViewOptions current3Dview = ThreeDViewOptions.loadAFile;

        /// <summary>
        /// Allows a developer to access advanced options. Not in use right now. 
        /// </summary>
        public bool DeveloperMode = false;


        /// <summary>
        /// Updates the live printing model while the printer is printing. 
        /// </summary>
        public class JobUpdater
        {
            /// <summary>
            /// A G-code visualization object. 
            /// </summary>
            GCodeVisual visual = null;

            /// <summary>
            /// This method will be called when the thread is started.
            /// </summary>
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
                Main.main.newVisual = visual;
                Main.main.jobPreviewThreadFinished = true;
                Main.main.previewThread = null;
                sw.Stop();
                //Main.conn.log("Update time:" + sw.ElapsedMilliseconds, false, 3);
            }
        }


        /// <summary>
        /// Changes some of the settings to work better if it is running on a MAC. I don't have a MAC so I haven't been
        /// keeping customizing the code to work on MAC. 
        /// </summary>
        /// <returns>True if running on Mac</returns>
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

        /// <summary>
        /// Called when the Main form is loaded. Doesn't really do much. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments</param>
        private void Main_Load(object sender, EventArgs e)
        {
           mainUpdaterHelper.UpdateEverythingInMain();
        }

        [System.Runtime.InteropServices.DllImport("libc")]
        static extern int uname(IntPtr buf);

        /// <summary>
        /// Initializes a new instance of the Main class. Part of the Windows form setup. Everything should be initialized in this function. 
        /// </summary>
        public Main()
        {
            executeHostCall = new executeHostCommandDelegate(this.executeHostCommand); // Define a delegate
            repetierKey = Custom.BaseKey; // Get the Regestry Key
            repetierKey.SetValue("installPath", Application.StartupPath);
            if (Path.DirectorySeparatorChar != '\\' && IsRunningOnMac())
                IsMac = true;

            main = this;

            SplashScreen.run();

            // Temp change the directory which looks for translations. 
            trans = new Trans(Application.StartupPath + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "Translation");
            //            trans = new Trans(Application.StartupPath + Path.DirectorySeparatorChar + "data" + Path.DirectorySeparatorChar + "translations");
            SwitchButton.imageOffset = RegMemory.GetInt("onOffImageOffset", 0);
            generator = new GCodeGenerator();
            globalSettings = new GlobalSettings();
            connection = new PrinterConnection();
            printerSettings = new FormPrinterSettings();
            printerModel = new PrinterModel();
            connection.analyzer.start();
            threeDSettings = new ThreeDSettings();
            InitializeComponent();

            editor = new RepetierEditor();
            this.Controls.Add(editor);
            editor.Visible = false;

            //updateShowFilament();
            RegMemory.RestoreWindowPos("mainWindow", this);

            gCodeEditorForm = new Form();
            postionGUI = new PositionSTLGUI();
            postionGUI.Left = this.Width - postionGUI.Width;
            postionGUI.Top = (this.Height - postionGUI.Height) / 2;
            postionGUI.Visible = false;
            Main.main.Controls.Add(postionGUI);

            logView = new LogView();
            logform = new Form();

            logform.Controls.Add(logView);

            fileAddOrRemove = new FileAddOrRemove(this);
            main.listSTLObjects.Visible = false;

            WindowState = FormWindowState.Maximized;

            //if (WindowState == FormWindowState.Maximized)
            //    Application.DoEvents();
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
                    /////*Application.Events.Quit += delegate (object sender, ApplicationEventArgs e) {
                    ////    Application.Quit ();
                    ////    e.Handled = true;
                    ////};

                    ////ApplicationEvents.Reopen += delegate (object sender, ApplicationEventArgs e) {
                    ////    WindowState = FormWindowState.Normal;
                    ////    e.Handled = true;
                    ////};*/

                    MinimumSize = new Size(500, 640);
                    ////tab.MinimumSize = new Size(500, 500);
                    ////splitLog.Panel1MinSize = 520;
                    ////splitLog.Panel2MinSize = 100;
                    ////splitLog.IsSplitterFixed = true;
                    //////splitContainerPrinterGraphic.SplitterDistance -= 52;
                    ////splitLog.SplitterDistance = splitLog.Height - 100;
                }
            }
            connection.eventConnectionChange += OnPrinterConnectionChange;
            connection.eventPrinterAction += OnPrinterAction;
            connection.eventJobProgress += OnJobProgress;

            manulControl = new ManualPrinterControl();
            this.Controls.Add(manulControl);
            manulControl.Visible = false;
            printerSettings.formToCon();
            gCodeEditorForm = new Form();
            gCodeEditorForm.Dock = DockStyle.Fill;

            logView = new LogView();
            logform = new Form();
            logform.ControlBox = false;
            logView.Dock = DockStyle.Fill;
            logform.StartPosition = FormStartPosition.WindowsDefaultBounds;
            logform.Controls.Add(logView);

            // TODO: Remomve this. 
            skeinforge = new Skeinforge();

            slicerPanel = new SlicerPanel();
            slicerPanaelForm = new Form();
            slicerPanaelForm.Visible = false;
            slicerPanaelForm.Width = 650;
            slicerPanaelForm.Height = 500;
            slicerPanaelForm.Controls.Add(slicerPanel);
            slicerPanaelForm.ControlBox = false;

            PrinterChanged(printerSettings.currentPrinterKey, true);
            printerSettings.eventPrinterChanged += PrinterChanged;

            // Threedview is the part that shows either the .stl files, g-code analysis results, or the print preview (showing the travel path of the print head). 
            // See the Method assign3DView() to get an adea of how it works. 
            threedview = new ThreeDControl();
            threedview.Dock = DockStyle.Fill;
            panel2.Controls.Add(threedview); // Add the OpenGL panel to the panel2 from the Windows Form
            //tabPage3DView.Controls.Add(threedview);

            // 
            gcodePreviewView = new ThreeDView();          
            gcodePreviewView.models.AddLast(jobVisual); // Add a g-code visualzation model to the view
            editor.contentChangedEvent += JobPreview;
            editor.commands = new Commands();
            editor.commands.Read("default", "en");
            this.fileAddOrRemove.UpdateHistory();

            // Make a new View. 
            livePrintingView = new ThreeDView();         
            livePrintingView.autoupdateable = true;

            // Print visualzation is dependent on the connection to the printer being active. (ie the printer is connected)
            printVisual = new GCodeVisual(connection.analyzer);
            printVisual.liveView = true;
            livePrintingView.models.AddLast(printVisual); // Add a new model to the view
            basicTitle = Text;

            // TODO: One is called slic3r and the other slicer. Why two??
            Main.slic3r = new Slic3r();
            Main.slicer = new Slicer();

            mainUpdaterHelper = new MainUpdaterHelper(this);

            //// TODO: Add temperature controls
            ////history = new TemperatureHistory();
            ////tempView = new TemperatureView();
            ////tempView.Dock = DockStyle.Fill;
            ////tabPageTemp.Controls.Add(tempView);
            ////if (IsMono)
            ////{
            ////    showWorkdirectoryToolStripMenuItem.Visible = false;
            ////    toolStrip.Height = 56;
            ////}

            new SoundConfig();

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

            // Don't check for updates because we want our own custom software to be sent to customers. 
            RHUpdater.checkForUpdates(true);

            // Update everything. 
            UpdateToolbarSize();
            mainUpdaterHelper.UpdateEverythingInMain();

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
            else
            {
                supportToolStripMenuItem.Visible = false;
            }

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

            calibrationZ = new Calibration();
            calibrationZ.Visible = false;
            calibrationZ.ControlBox = false;

            // Allow for Drag and drop
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

        } // End Main()

        /// <summary>
        /// Control the Drag Enter actions. Basically copy the file to the clip board??
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments</param>
        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        /// Control the drag drop drop action. Try loading the file. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files) this.fileAddOrRemove.LoadGCodeOrSTL(file);
        }

        /// <summary>
        /// Translate all the menus
        /// </summary>
        public void translate()
        {
            this.calibrateHeightToolStripMenuItem.Text = Trans.T("M_CALIBRATE_HIEGHT");
            fileToolStripMenuItem.Text = Trans.T("M_FILE");
            settingsToolStripMenuItem.Text = Trans.T("M_CONFIG");
            this.repetierSettingsToolStripMenuItem.Text = Trans.T("M_SOFTWARE_GENERAL_SETTINGS");
            this.slicerToolStripMenuItem1.Text = Trans.T("M_SLICER");
            //// this.slicerSelectionToolStripMenuItem.Text = Trans.T("M_SLICER_DIRECTORY_SETUP");
            this.slicerConfigurationToolStripMenuItem.Text = Trans.T("M_SLICER_CONFIGURATION");
            this.gcodeEditorToolStripMenuItem.Text = Trans.T("M_GCODE_EDITOR");
            this.stopSlicingProcessToolStripMenuItem.Text = Trans.T("M_STOP_SLICER");
            this.dViewSettingsToolStripMenuItem.Text = Trans.T("M_3D_VIEW_OPTIONS");
            this.userManual2ToolStripMenuItem.Text = Trans.T("M_MANUAL");
            this.manualControlToolStripMenuItem.Text = Trans.T("M_IMANUAL");
            this.topViewStripButton1.Text = Trans.T("M_TOP");
            this.sideViewStripButton1.Text = Trans.T("M_SIDE");
            this.frontStripButton1.Text = Trans.T("M_FRONT");
            this.viewsStripSplitButton6.Text = Trans.T("M_VIEW");
            this.rotateStripMenuItem11.Text = Trans.T("M_Rotate");
            this.moveStripMenuItem12.Text = Trans.T("M_MOVE");
            this.zoomStripMenuItem13.Text = Trans.T("M_ZOOM");
            this.perspectiveStripMenuItem14.Text = Trans.T("M_PERSPECTIVE");
            this.resetStripMenuItem15.Text = Trans.T("M_RESET_VIEW");
            this.positionToolSplitButton2.Text = Trans.T("M_POSITION");
            this.sliceToolSplitButton3.Text = Trans.T("M_SLICE");
            this.withRaftToolStripMenuItem1.Text = Trans.T("M_WITH_RAFT");
            this.withSupportsToolStripMenuItem1.Text = Trans.T("M_WITH_SUPPORT");
            this.modeToolStripSplitButton1.Text = Trans.T("M_MODE");
            this.loadAFileMenuModeMenuOption.Text = Trans.T("M_LOAD_A_FILE");
            this.STLEditorMenuOption.Text = Trans.T("M_STL_EDITOR");
            this.gCodeVisualizationMenuOption.Text = Trans.T("M_GCODE_VIEW");
            this.livePrintingMenuOption.Text = Trans.T("M_LIVE_PRINTING");
            this.saveGCodeToolStripMenuItem.Text = Trans.T("M_SAVE_GCODE");
            this.saveNewSTLMenuItem11.Text = Trans.T("M_SAVE_NEW_STL");
            this.loggingToolStripMenuItem.Text = Trans.T("M_VIEW_LOGGING");
            this.manualControlToolStripMenuItem.Text = Trans.T("M_MANUAL_CONTROL");
            this.advancedConfigStripSplitButton3.Text = Trans.T("M_ADVANCED");
            this.printerToolStripMenuItem.Text = Trans.T("M_PRINTER");
            this.temperatureToolStripMenuItem.Text = Trans.T("M_TEMPERATURE");
            this.helpToolStripMenuItem.Text = Trans.T("M_HELP");
            this.loadGCodeToolStripMenuItem.Text = Trans.T("M_LOAD_GCODE_OR_STL_FILE");
            this.showWorkdirectoryToolStripMenuItem.Text = Trans.T("M_SHOW_WORKDIRECTORY");
            this.languageToolStripMenuItem.Text = Trans.T("M_LANGUAGE");
            this.printerSettingsToolStripMenuItem.Text = Trans.T("M_PRINTER_SETTINGS");
            ////eeprom.Text = Trans.T("M_EEPROM_SETTINGS");
            ////advancedViewConfigurationToolStripMenuItem.Text = Trans.T("M_3D_VIEWER_CONFIGURATION");
            ////repetierSettingsToolStripMenuItem.Text = Trans.T("M_REPETIER_SETTINGS");
            this.internalSlicingParameterToolStripMenuItem.Text = Trans.T("M_TESTCASE_SETTINGS");
            this.soundConfigurationToolStripMenuItem.Text = Trans.T("M_SOUND_CONFIGURATION");
            this.showExtruderTemperaturesMenuItem.Text = Trans.T("M_SHOW_EXTRUDER_TEMPERATURES");
            this.showHeatedBedTemperaturesMenuItem.Text = Trans.T("M_SHOW_HEATED_BED_TEMPERATURES");
            this.showTargetTemperaturesMenuItem.Text = Trans.T("M_SHOW_TARGET_TEMPERATURES");
            this.showAverageTemperaturesMenuItem.Text = Trans.T("M_SHOW_AVERAGE_TEMPERATURES");
            this.showHeaterPowerMenuItem.Text = Trans.T("M_SHOW_HEATER_POWER");
            this.autoscrollTemperatureViewMenuItem.Text = Trans.T("M_AUTOSCROLL_TEMPERATURE_VIEW");
            this.timeperiodMenuItem.Text = Trans.T("M_TIMEPERIOD");
            this.temperatureZoomMenuItem.Text = Trans.T("M_TEMPERATURE_ZOOM");
            this.buildAverageOverMenuItem.Text = Trans.T("M_BUILD_AVERAGE_OVER");
            this.secondsToolStripMenuItem.Text = Trans.T("M_30_SECONDS");
            this.minuteToolStripMenuItem.Text = Trans.T("M_1_MINUTE");
            this.minuteToolStripMenuItem1.Text = Trans.T("M_1_MINUTE");
            this.minutesToolStripMenuItem.Text = Trans.T("M_2_MINUTES");
            this.minutesToolStripMenuItem1.Text = Trans.T("M_5_MINUTES");
            this.minutes5ToolStripMenuItem.Text = Trans.T("M_5_MINUTES");
            this.minutes10ToolStripMenuItem.Text = Trans.T("M_10_MINUTES");
            this.minutes15ToolStripMenuItem.Text = Trans.T("M_15_MINUTES");
            this.minutes30ToolStripMenuItem.Text = Trans.T("M_30_MINUTES");
            this.minutes60ToolStripMenuItem.Text = Trans.T("M_60_MINUTES");
            this.continuousMonitoringMenuItem.Text = Trans.T("M_CONTINUOUS_MONITORING");
            this.disableToolStripMenuItem.Text = Trans.T("M_DISABLE");
            this.extruder1ToolStripMenuItem.Text = Trans.T("M_EXTRUDER_1");
            this.extruder2ToolStripMenuItem.Text = Trans.T("M_EXTRUDER_2");
            this.heatedBedToolStripMenuItem.Text = Trans.T("M_HEATED_BED");
            this.printerInformationsToolStripMenuItem.Text = Trans.T("M_PRINTER_INFORMATION");
            this.jobStatusToolStripMenuItem.Text = Trans.T("M_JOB_STATUS");
            this.menuSDCardManager.Text = Trans.T("M_SD_CARD_MANAGER");
            this.testCaseGeneratorToolStripMenuItem.Text = Trans.T("M_TEST_CASE_GENERATOR");
            this.sendScript1ToolStripMenuItem.Text = Trans.T("M_SEND_SCRIPT_1");
            this.sendScript2ToolStripMenuItem.Text = Trans.T("M_SEND_SCRIPT_2");
            this.sendScript3ToolStripMenuItem.Text = Trans.T("M_SEND_SCRIPT_3");
            this.sendScript4ToolStripMenuItem.Text = Trans.T("M_SEND_SCRIPT_4");
            this.sendScript5ToolStripMenuItem.Text = Trans.T("M_SEND_SCRIPT_5");
            //// repetierHostHomepageToolStripMenuItem.Text = Trans.T("M_REPETIER_HOST_HOMEPAGE");
            ////// repetierHostDownloadPageToolStripMenuItem.Text = Trans.T("M_REPETIER_HOST_DOWNLOAD_PAGE");
            //// manualToolStripMenuItem.Text = Trans.T("M_MANUAL");
            ////slic3rHomepageToolStripMenuItem.Text = Trans.T("M_SLIC3R_HOMEPAGE");
            ////skeinforgeHomepageToolStripMenuItem.Text = Trans.T("M_SKEINFORGE_HOMEPAGE");
            ////repRapWebsiteToolStripMenuItem.Text = Trans.T("M_REPRAP_WEBSITE");
            ////repRapForumToolStripMenuItem.Text = Trans.T("M_REPRAP_FORUM");
            ////thingiverseNewestToolStripMenuItem.Text = Trans.T("M_THINGIVERSE_NEWEST");
            ////thingiversePopularToolStripMenuItem.Text = Trans.T("M_THINGIVERSE_POPULAR");
            ////aboutRepetierHostToolStripMenuItem.Text = Trans.T("M_ABOUT_REPETIER_HOST");
            checkForUpdatesToolStripMenuItem.Text = Trans.T("M_CHECK_FOR_UPDATES");
            quitToolStripMenuItem.Text = Trans.T("M_QUIT");
            ////donateToolStripMenuItem.Text = Trans.T("M_DONATE");
            ////tabPage3DView.Text = Trans.T("TAB_3D_VIEW");
            ////tabPageTemp.Text = Trans.T("TAB_TEMPERATURE_CURVE");
            ////tabModel    .Text = Trans.T("TAB_OBJECT_PLACEMENT");
            ////tabSlicer.Text = Trans.T("TAB_SLICER");
            ////tabGCode.Text = Trans.T("TAB_GCODE_EDITOR");
            ////tabPrint.Text = Trans.T("TAB_MANUAL_CONTROL");
            ////printerOptionsToolStripMenuItem.Text = Trans.T("M_PRINTER_SETTINGS");
            ////printerOptionsToolStripMenuItem.ToolTipText = Trans.T("M_PRINTER_SETTINGS");
            this.emergencyStopStripButton6.Text = Trans.T("M_EMERGENCY_STOP");
            this.sDCardToolStripMenuItem.Text = Trans.T("M_SD_CARD");
            this.sDCardToolStripMenuItem.ToolTipText = Trans.T("L_SD_CARD_MANAGEMENT");
            this.emergencyStopStripButton6.ToolTipText = Trans.T("M_EMERGENCY_STOP");
            this.importSTLToolSplitButton1.Text = Trans.T("M_LOAD");
            ////toolStripSaveJob.Text = Trans.T("M_SAVE_JOB");
            this.killJobToolStripMenuItem.Text = Trans.T("M_KILL_JOB");
            this.sDCardToolStripMenuItem.Text = Trans.T("M_SD_CARD");
            ////toolShowLog.Text = toolShowLog.ToolTipText = Trans.T("M_TOGGLE_LOG");            
            ////viewSlicedObjectToolStripMenuItem1.Text = Trans.T("M_SHOW_FILAMENT");

            if (connection.connected)
            {
                connectToolStripSplitButton.ToolTipText = Trans.T("L_DISCONNECT_PRINTER"); // "Disconnect printer";
                connectToolStripSplitButton.Text = Trans.T("M_DISCONNECT"); // "Disconnect";
            }
            else
            {
                connectToolStripSplitButton.ToolTipText = Trans.T("L_CONNECT_PRINTER"); // "Connect printer";
                connectToolStripSplitButton.Text = Trans.T("M_CONNECT"); // "Connect";
            }

            ////if (threeDSettings.checkDisableFilamentVisualization.Checked)
            ////{
            ////    // TODO: Update this to be something other than "show filament"
            ////    //viewSlicedObjectToolStripMenuItem1.ToolTipText = Trans.T("L_FILAMENT_VISUALIZATION_DISABLED"); // "Filament visualization disabled";
            ////    //viewSlicedObjectToolStripMenuItem1.Text = Trans.T("M_SHOW_FILAMENT"); // "Show filament";
            ////}
            ////else
            ////{
            ////    // TODO: Update this to be something other than "show filament"
            ////    //viewSlicedObjectToolStripMenuItem1.ToolTipText = Trans.T("L_FILAMENT_VISUALIZATION_ENABLED"); // "Filament visualization enabled";
            ////    //viewSlicedObjectToolStripMenuItem1.Text = Trans.T("M_HIDE_FILAMENT"); // "Hide filament";
            ////}

            if (connection.job.mode != Printjob.jobMode.printingJob)
            {
                Main.main.printStripSplitButton4.ToolTipText = Trans.T("M_RUN_JOB"); // "Run job";
                Main.main.printStripSplitButton4.Text = Trans.T("M_RUN_JOB"); //"Run job";
            }
            else
            {
                Main.main.printStripSplitButton4.ToolTipText = Trans.T("M_PAUSE_JOB"); //"Pause job";
                Main.main.printStripSplitButton4.Text = Trans.T("M_PAUSE_JOB"); //"Pause job";
            }

            this.importSTLToolSplitButton1.ToolTipText = Trans.T("L_LOAD_FILE"); // Load file
            ////toolStripSaveJob.ToolTipText = Trans.T("M_SAVE_JOB");

            this.openFileSTLorGcode.Title = Trans.T("L_IMPORT_G_CODE"); // Import G-Code
            this.saveJobDialog.Title = Trans.T("L_SAVE_G_CODE"); //Save G-Code
            this.updateTravelMoves();

            foreach (ToolStripMenuItem item in languageToolStripMenuItem.DropDownItems)
            {
                item.Checked = item.Tag == trans.active;
            }
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
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void languageSelected(object sender, EventArgs e)
        {
            ToolStripItem it = (ToolStripItem)sender;
            trans.selectLanguage((Translation)it.Tag);
            if (languageChanged != null)
                languageChanged();
        }

        /// <summary>
        /// When clicking on recent printers/printer settings update the printer settings objects. Provides an action for clicking on recent printers. 
        /// </summary>
        /// <param name="sender">Sending Button</param>
        /// <param name="e">Event arguments</param>
        public void ConnectHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            connection.port = clickedItem.Text;
            printerSettings.formToCon(); // so that we save the most the port name. 
           
            this.mainUpdaterHelper.UpdateEverythingInMain();
            connection.open();
        }       

        /// <summary>
        /// Actions to take when changing printers. Assigned to point to the printer form settings members 
        /// </summary>
        /// <param name="pkey">The registry key for the particular printer.</param>
        /// <param name="printerChanged">True if printer is to change.</param>
        public void PrinterChanged(RegistryKey pkey, bool printerChanged)
        {
            if (printerChanged && editor != null)
            {
                editor.UpdatePrependAppend();
            }
        }

        /// <summary>
        /// Delegated way of invoking the print button. 
        /// </summary>
        public MethodInvoker StartJob = delegate
        {
            Main.main.printStripSplitButton4_ButtonClick(null, null);
        };

        /// <summary>
        /// Gets or sets the Title for the application that shows in the top left corner near the icon. It will always say
        /// the basic title plus the name of the current models. 
        /// </summary>
        public string Title
        {
            set { Text = basicTitle + " - " + value; }
            get { return Text; }
        }

        /// <summary>
        /// Exit the application on click "quit"
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// If the connection to the printer changes then update what is available to click. 
        /// </summary>
        /// <param name="msg">Message to put in the connection status</param>
        private void OnPrinterConnectionChange(string msg)
        {
            this.toolConnection.Text = msg;
            this.sendScript1ToolStripMenuItem.Enabled = connection.connected;
            this.sendScript2ToolStripMenuItem.Enabled = connection.connected;
            this.sendScript3ToolStripMenuItem.Enabled = connection.connected;
            this.sendScript4ToolStripMenuItem.Enabled = connection.connected;
            this.sendScript5ToolStripMenuItem.Enabled = connection.connected;
            if (connection.connected)
            {
                this.connectToolStripSplitButton.Image = imageList.Images[4];
                this.connectToolStripSplitButton.ToolTipText = Trans.T("L_DISCONNECT_PRINTER"); // "Disconnect printer";
                this.connectToolStripSplitButton.Text = Trans.T("M_DISCONNECT"); // "Disconnect";
                foreach (ToolStripItem it in connectToolStripSplitButton.DropDownItems)
                {
                    it.Enabled = false;
                }

                emergencyStopStripButton6.Enabled = true;
            }
            else
            {
                this.connectToolStripSplitButton.Image = imageList.Images[3];
                this.connectToolStripSplitButton.ToolTipText = Trans.T("L_CONNECT_PRINTER"); // "Connect printer";
                this.connectToolStripSplitButton.Text = Trans.T("M_CONNECT"); // "Connect";
                this.continuousMonitoringMenuItem.Enabled = false;
                if (this.eepromSettings != null && this.eepromSettings.Visible)
                {
                    this.eepromSettings.Close();
                    this.eepromSettings.Dispose();
                    this.eepromSettings = null;
                }

                if (this.eepromSettingsm != null && this.eepromSettingsm.Visible)
                {
                    this.eepromSettingsm.Close();
                    this.eepromSettingsm.Dispose();
                    this.eepromSettingsm = null;
                }

                foreach (ToolStripItem it in this.connectToolStripSplitButton.DropDownItems)
                {
                    it.Enabled = true;
                }

                this.emergencyStopStripButton6.Enabled = false;
                SDCard.Disconnected();
            }
        }

        /// <summary>
        /// Update the toolAction text based on some message
        /// </summary>
        /// <param name="msg">Message to put as text</param>
        private void OnPrinterAction(string msg)
        {
            this.toolAction.Text = msg;
        }

        /// <summary>
        /// Update the progress bar. 
        /// </summary>
        /// <param name="per">Value indicating progress</param>
        private void OnJobProgress(float per)
        {
            toolProgress.Value = (int)per;
        }

        /// <summary>
        /// Configure what is available to click based on the printer connection type. Related to the eeprom on the printer. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (connection.isRepetier)
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
                {
                    eepromSettings = new EEPROMRepetier();
                }

                eepromSettings.Show2();
            }

            if (connection.isMarlin)
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
                {
                    eepromSettingsm = new EEPROMMarlin();
                }

                eepromSettingsm.Show2();
            }
        }


        /// <summary>
        /// Load g-code or .stl when you click file, load. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments </param>
        private void toolGCodeLoad_Click(object sender, EventArgs e)
        {
            if (openFileSTLorGcode.ShowDialog() == DialogResult.OK)
            {
                this.fileAddOrRemove.LoadGCodeOrSTL(openFileSTLorGcode.FileName);
            }
        }       
       
        /// <summary>
        /// Brings up the printer settings form. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        private void printerSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printerSettings.Show(this);
            printerSettings.UpdatePorts();
            printerSettings.BringToFront();            
        }

        ////private void skeinforgeSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    skeinforge.Show();
        ////    skeinforge.BringToFront();
        ////}
        ////private void skeinforgeToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    skeinforge.RunSkeinforge();
        ////}

        /// <summary>
        /// Contains information about the current printer. 
        /// </summary>
        private PrinterInfo printerInfo = null;

        /// <summary>
        /// Displays information about the current printer. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments</param>
        private void printerInformationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.printerInfo == null)
                this.printerInfo = new PrinterInfo();
            this.printerInfo.Show();
            this.printerInfo.BringToFront();
        }

        /// <summary>
        /// Run when the user tries to close the program. Asks if they are sure. If a job is running than it gives more messages about if they want to turn off the heaters. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments</param>
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            string message = Trans.T("M_SURE_TO_EXIT");
            string caption = Trans.T("B_EXIT");
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);
            // If the no button was pressed ... 
            if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            if (connection.job.mode == Printjob.jobMode.printingJob)
            {
                if (MessageBox.Show(Trans.T("L_REALLY_QUIT"), Trans.T("L_SECURITY_QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (!connection.close())
            {
                e.Cancel = true;
                return;
            }

            RegMemory.StoreWindowPos("mainWindow", this, true, true);

            if (previewThread != null)
            {
                previewThread.Join();
            }

            connection.Destroy();
        }

        /// <summary>
        /// Displays a form related to general software settings. Like the working directory. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        private void repetierSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            globalSettings.Show(this);
            globalSettings.BringToFront();
        }

        ////public About about = null;
        ////private void aboutRepetierHostToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    if (about == null) about = new About();
        ////    about.Show(this);
        ////    about.BringToFront();
        ////}

        ////private void jobStatusToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    JobStatus.ShowStatus();
        ////}

        /// <summary>
        /// Used to try to open a web address. Catches if there are problems. 
        /// </summary>
        /// <param name="link">Link to open.</param>
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

        ////private void repetierHostHomepageToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    // TODO: Change this. 
        ////    openLink("http://www.repetier.com");
        ////}

        /// <summary>
        /// Opens a link to the instructions manual. Right now it just redirects to the Baoyan website. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments</param>
        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openLink("http://www.by3dp.com");
        }

        /// <summary>
        /// Delegated function that is called by the printer connection if it can determine the firmware??
        /// </summary>
        public MethodInvoker FirmwareDetected = delegate
        {
            Main.main.manulControl.UpdateConStatus(true);
            if (connection.isRepetier)
            {
                Main.main.continuousMonitoringMenuItem.Enabled = true;
            }
        };

        /// <summary>
        /// Updates the eeprom menus. 
        /// </summary>
        public MethodInvoker UpdateEEPROM = delegate
        {
            ////if (connection.isMarlin || connection.isRepetier) // Activate special menus and function
            ////{
            ////    //main.eeprom.Enabled = true;
            ////}
            //////else main.eeprom.Enabled = false;

        };
       
        /// <summary>
        /// Called by the gcode editor when something changes. 
        /// </summary>
        private void JobPreview()
        {
            if (editor.autopreview == false)
            {
                return;
            }
           
            recalcJobPreview = true;           
        }

        /// <summary>
        /// Starts generating a test case object. Not working right now. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event argument</param>
        private void testCaseGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestGenerator.Execute();
        }

        ////private void internalSlicingParameterToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    SlicingParameter.Execute();
        ////}

        ////private void toolStripSDCard_Click(object sender, EventArgs e)
        ////{
        ////    SDCard.Execute();
        ////}

        /// <summary>
        /// When the timer ticks it updates some of the live printing view view information. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
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
                    int sec = (int)(Main.main.editor.printingTime * (1 + 0.01 * Main.connection.addPrintingTime));
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

        ////private void toolShowLog_Click(object sender, EventArgs e)
        ////{
        ////    //if (splitLog.Panel2Collapsed == true)
        ////    //{
        ////    //    splitLog.Panel2Collapsed = false;
        ////    //}
        ////    //else
        ////    //{
        ////    //    splitLog.Panel2Collapsed = true;
        ////    //}            
        ////    //toolShowLog.Checked = !toolShowLog.Checked;
        ////}
        ////private void toolShowLog_CheckedChanged(object sender, EventArgs e)
        ////{
        ////    //if (splitLog.Panel2Collapsed == true)
        ////    //{
        ////    //    splitLog.Panel2Collapsed = false;
        ////    //}
        ////    //else
        ////    //{
        ////    //    splitLog.Panel2Collapsed = true;
        ////    //}
        ////}
        ////private void repRapWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    openLink("http://www.reprap.org");
        ////}
        ////private void repRapForumToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    openLink("http://forum.reprap.org");
        ////}
        ////private void slic3rHomepageToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    openLink("http://www.slic3r.org");
         ////}
        ////private void skeinforgeHomepageToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    openLink("http://fabmetheus.crsndoo.com/");
        ////}
        ////private void thingiverseNewestToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    openLink("http://www.thingiverse.com/newest");
        ////}
        ////private void thingiversePopularToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    openLink("http://www.thingiverse.com/popular");
        ////}
        ////private void slic3rToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    slicer.ActiveSlicer = Slicer.SlicerID.Slic3r;
        ////    // TODO: Add this translation back
        ////    //stlComposer1.buttonSlice.Text = Trans.T1("L_SLICE_WITH", slicer.SlicerName);
        ////}
        ////private void skeinforgeToolStripMenuItem1_Click(object sender, EventArgs e)
        ////{
        ////    slicer.ActiveSlicer = Slicer.SlicerID.Skeinforge;
        ////    //TODO: add this translation back
        ////    //stlComposer1.buttonSlice.Text = Trans.T1("L_SLICE_WITH", slicer.SlicerName);
        ////}
        ////private void slic3rConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        ////{
        ////    slic3r.Show();
        ////    slic3r.BringToFront();
        ////}
        ////private void tab_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    //Console.WriteLine("index changed " + Environment.OSVersion.Platform + " Mac=" + PlatformID.MacOSX);
        ////    //if (Environment.OSVersion.Platform == PlatformID.MacOSX )
        ////    if (IsMac)
        ////    {
        ////        // In MacOSX the OpenGL windows shine through the
        ////        // tabs, so we need to disable all GL windows except the active.
        ////        //if (tab.SelectedTab != tabModel)
        ////        //{
        ////        //    if (tabModel.Controls.Contains(stlComposer1))
        ////        //    {
        ////        //        tabModel.Controls.Remove(stlComposer1);
        ////        //    }
        ////        //}
        ////        //if (tab.SelectedTab == tabModel)
        ////        //{
        ////        //    if (!tabModel.Controls.Contains(stlComposer1))
        ////        //        tabModel.Controls.Add(stlComposer1);
        ////        //}
        ////        refreshCounter = 6;
        ////    }
        ////    //if (tab.SelectedTab == tabModel || tab.SelectedTab == tabSlicer)
        ////    //{
        ////    //    tabControlView.SelectedIndex = 0;
        ////    //}
        ////    update3DviewSelection();
        ////}

        /// <summary>
        /// Event on resize. Basically update everything. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments</param>
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

        /// <summary>
        /// What to do when showing a the main again. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e"></param>
        private void Main_Shown(object sender, EventArgs e)
        {
            if (!globalSettings.WorkdirOK())
            {
                globalSettings.Show();
            }
        }

        /// <summary>
        /// Related to the printer connection and running. Not totally sure what it does. 
        /// </summary>
        /// <param name="code">Gcode command. </param>
        public void executeHostCommand(GCode code)
        {
            string com = code.getHostCommand();
            string param = code.getHostParameter();
            if (com.Equals("@info"))
            {
                connection.log(param, false, 3);
            }
            else if (com.Equals("@pause"))
            {
                SoundConfig.PlayPrintPaused(false);
                connection.pause(param);
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

        ///// <summary>
        ///// Toggles whether the show filament visualization is on or off. 
        ///// 
        ///// </summary>
        ////public void updateShowFilament()
        ////{
        ////    // TODO: Don't need this any more. Maybe we could put an icon at the bottom that indicates the current configuration of the filament. 
        ////    //if (threeDSettings.checkDisableFilamentVisualization.Checked)
        ////    //{
        ////    //    toolShowFilament.Image = imageList.Images[5];
        ////    //    toolShowFilament.ToolTipText = Trans.T("L_FILAMENT_VISUALIZATION_DISABLED"); // "Filament visualization disabled";
        ////    //    toolShowFilament.Text = Trans.T("M_HIDE_FILAMENT"); // "Show filament";
        ////    //}
        ////    //else
        ////    //{
        ////    //    toolShowFilament.Image = imageList.Images[4];
        ////    //    toolShowFilament.ToolTipText = Trans.T("L_FILAMENT_VISUALIZATION_ENABLED"); // "Filament visualization enabled";
        ////    //    toolShowFilament.Text = Trans.T("M_SHOW_FILAMENT"); // "Hide filament";
        ////    //}
        ////}

        /// <summary>
        /// Toggles the Update Travel on or off. 
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

      
        /// <summary>
        /// Runs when the user comes back to the Main application after using other programs. Checks to make sure the .stl files are the same. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Activated(object sender, EventArgs e)
        {
            fileAddOrRemove.RecheckChangedFiles();
        }

        /// <summary>
        /// Selects the temperature time period to show. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">E</param>
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
            connection.injectManualCommand("M203 S255");
        }

        private void extruder1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connection.injectManualCommand("M203 S0");
        }

        private void extruder2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connection.injectManualCommand("M203 S1");
        }

        private void heatedBedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connection.injectManualCommand("M203 S100");
        }

        private void showWorkdirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Main.globalSettings.Workdir))
                Process.Start("explorer.exe", Main.globalSettings.Workdir);
        }

        //private void soundConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    SoundConfig.config.ShowDialog();
        //}

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
                connection.injectManualCommand(code.text);
            }
        }

        private void sendScript2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GCodeShort code in editor.getContentArray(6))
            {
                connection.injectManualCommand(code.text);
            }
        }

        private void sendScript3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GCodeShort code in editor.getContentArray(7))
            {
                connection.injectManualCommand(code.text);
            }
        }

        private void sendScript4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GCodeShort code in editor.getContentArray(8))
            {
                connection.injectManualCommand(code.text);
            }
        }

        private void sendScript5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GCodeShort code in editor.getContentArray(9))
            {
                connection.injectManualCommand(code.text);
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
            connection.job.etaModeNormal = !connection.job.etaModeNormal;
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
           // this.threedview.SetMode(0);
            ThreeDControl.CurrentMode = ThreeDControl.modeOptions.Rotation;

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
                this.fileAddOrRemove.RemoveSTLObject();
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
            if (!connection.connected) return;
            connection.injectManualCommandFirst("M112");
            connection.job.KillJob();
            connection.serial.DtrEnable = false;
            //conn.serial.RtsEnable = true;
            Thread.Sleep(200);
            //conn.serial.RtsEnable = false;
            connection.serial.DtrEnable = true;
            Thread.Sleep(200);
            connection.serial.DtrEnable = false;
            connection.log(Trans.T("L_EMERGENCY_STOP_MSG"), false, 3);
            while (connection.hasInjectedMCommand(112))
            {
                Application.DoEvents();
            }
            //conn.close();
        }

        /// <summary>
        /// Set the mode to rotate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            //// this.threedview.SetMode(0);
            ThreeDControl.CurrentMode = ThreeDControl.modeOptions.Rotation;
        }

        /// <summary>
        /// Set the 3D mode to move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moveStripMenuItem12_Click(object sender, EventArgs e)
        {
            // Set the Control mode to move Mode
            //// this.threedview.SetMode(ThreeDControl.modeOptions.MoveViewpoint);
            ThreeDControl.CurrentMode = ThreeDControl.modeOptions.MoveViewpoint;
        }

        /// <summary>
        /// Set the 3D mode to zoom.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoomStripMenuItem13_Click(object sender, EventArgs e)
        {
            ThreeDControl.CurrentMode = ThreeDControl.modeOptions.Zoom;
            //// this.threedview.SetMode(ThreeDControl.modeOptions.Zoom);
        }

        /// <summary>
        ///Change the perspective mode of the 3D view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void perspectiveStripMenuItem14_Click(object sender, EventArgs e)
        {
            this.threedview.ChangeProspectiveMode();
        }

        /// <summary>
        /// Reset the 3D view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetStripMenuItem15_Click(object sender, EventArgs e)
        {
            this.threedview.ResetView();
        }

        private void advancedViewConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
            this.fileAddOrRemove.RemoveSTLObject();
            //fileAddOrRemove.changeSelectionBoxSize();
            main.mainUpdaterHelper.UpdateEverythingInMain();
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
            mainUpdaterHelper.UpdateEverythingInMain();
            //threedview.SetView(jobPreview);
            //JobPreview();
            //threedview.SetView(fileAddOrRemove.cont);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            threedview.SetView(fileAddOrRemove.StleditorView);
        }

        /// <summary>
        /// What to do when the print button is clicked. 
        /// TODO: Change the availablity of being able to click this and the image based on whether g-code that an be printed is avaible. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printStripSplitButton4_ButtonClick(object sender, EventArgs e)
        {
            Printjob job = connection.job;
            if (job.dataComplete)
            {
                connection.pause(Trans.T("L_PAUSE_MSG")); //"Press OK to continue.\n\nYou can add pauses in your code with\n@pause Some text like this");
            }
            else
            {
                //tab.SelectedTab = tabPrint;
                current3Dview = ThreeDViewOptions.livePrinting;
                connection.analyzer.StartJob();

                // TODO: Uncomment this section. The button should not be pushed unless a job is ready. 
                //printStripSplitButton4.Image = imageList.Images[3];
                job.BeginJob();
                job.PushGCodeShortArray(editor.getContentArray(1));
                job.PushGCodeShortArray(editor.getContentArray(0));
                job.PushGCodeShortArray(editor.getContentArray(2));
                job.EndJob();
            }
        }

        /// <summary>
        /// Begins the kill job set of functions to end a printing job. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">arguments</param>
        private void killJobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connection.job.KillJob();
        }
        
        /// <summary>
        /// Starts the set of events to import .stl or gcode. 
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">arguments</param>
        private void importSTLToolSplitButton1_ButtonClick_1(object sender, EventArgs e)
        {
            if (openFileSTLorGcode.ShowDialog() == DialogResult.OK)
            {
                this.fileAddOrRemove.LoadGCodeOrSTL(openFileSTLorGcode.FileName);
            }
        }

        private void loggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (logform.IsDisposed == true)
            //{

            //    logform.Visible = true;

            //}
            //else

            logform.Visible = !logform.Visible;
            logform.BringToFront();
            //
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
            printerSettings.BringToFront();
        }

        private void helpSplitButton3_ButtonClick(object sender, EventArgs e)
        {
            openLink(Custom.GetString("extraSupportURL", "http://www.by3dp.com  "));
        }


        private void advancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gCodeEditorForm.Controls.Add(editor);
            gCodeEditorForm.Size = new Size(640, 530);
            gCodeEditorForm.Visible = !gCodeEditorForm.Visible;
        }

       

        /// <summary>
        /// If connected than disconnect, if not connected then connect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectToolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            // TODO: Select COM port
            if (connection.connected)
            {
                connection.close();
            }
            else
            {
                connection.open();
            }
        }

        private void languageToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Displays the manul control panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void manualControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manulControl.Left = (this.Width - manulControl.Width) / 2;
            manulControl.Top = (this.Height - manulControl.Height) / 2;
            manulControl.Visible = !manulControl.Visible;
            manulControl.BringToFront();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Shows the Position stl GUI control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void positionToolSplitButton2_Click(object sender, EventArgs e)
        {

            this.postionGUI.Left = 100;
            this.postionGUI.Top = 24 + 54;
            postionGUI.Visible = !postionGUI.Visible;
            postionGUI.BringToFront();
        }

        private void toolStripMenuItem11_Click_1(object sender, EventArgs e)
        {
            if (saveSTL.ShowDialog() == DialogResult.OK)
            {
                this.fileAddOrRemove.SaveComposition(saveSTL.FileName);
            }
        }

        Stopwatch developerModeWatch = new Stopwatch();
        int developerModeClickCount = 0;


        /// <summary>
        /// Runs actions related to going clicking the davanced button. Toggles developer mode if clicked 6 times in 5 seconds. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advancedConfigStripSplitButton3_ButtonClick(object sender, EventArgs e)
        {
            // Remember what mode we start in so that we can see if it changes. 
            bool beginMode = DeveloperMode;

            // Increment the counter. 
            developerModeClickCount++;

            // if clicked 6 times then toggle developer mode. 
            if ( developerModeClickCount>6)
            {
                DeveloperMode = !DeveloperMode;
                developerModeWatch.Stop();
                developerModeWatch.Reset();
                developerModeClickCount = 0;
            }

            // if the time elapsed is greater than 5 seconds since the start of the timer, then reset the stop watch and click count to zero
            TimeSpan timeElapsed = developerModeWatch.Elapsed;
            if(timeElapsed.Seconds >= 5)
            {
                developerModeWatch.Stop();
                developerModeWatch.Reset();
                developerModeClickCount = 0;
            }

            // If the stop watch is not running, then start the stop watch
            if (developerModeWatch.IsRunning == false)
            {
                developerModeWatch.Start();
            }

            if (DeveloperMode != beginMode)
                mainUpdaterHelper.UpdateEverythingInMain();
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
            this.mainUpdaterHelper.UpdateEverythingInMain();
        }

        private void STLEditorMenuOption_Click(object sender, EventArgs e)
        {
            this.current3Dview = ThreeDViewOptions.STLeditor;
            this.mainUpdaterHelper.UpdateEverythingInMain();
        }

        private void gCodeVisualizationMenuOption_Click(object sender, EventArgs e)
        {
            this.current3Dview = ThreeDViewOptions.gcode;
            this.mainUpdaterHelper.UpdateEverythingInMain();
        }

        private void livePrintingMenuOption_Click(object sender, EventArgs e)
        {
            this.current3Dview = ThreeDViewOptions.livePrinting;
            this.mainUpdaterHelper.UpdateEverythingInMain();
        }

        private void slicerConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Main.slic3r.RunConfig();
            slicerPanel.UpdateSelection();
            slicerPanaelForm.Visible = !slicerPanaelForm.Visible;
            //slicerPanel.Width = 600;
            //slicerPanel.Height = 400;

            if (slicerPanaelForm.Visible == true)
                slicerPanaelForm.BringToFront();
        }

        private void withRaftToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            withRaftToolStripMenuItem1.Checked = !withRaftToolStripMenuItem1.Checked;
            slicerPanel.generateRaftCheckbox.Checked = withRaftToolStripMenuItem1.Checked;
        }

        private void withSupportsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            withSupportsToolStripMenuItem1.Checked = !withSupportsToolStripMenuItem1.Checked;
            slicerPanel.generateSupportCheckbox.Checked = withSupportsToolStripMenuItem1.Checked;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dViewSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            threeDSettings.Show();
            threeDSettings.BringToFront();
        }

        private void centerOnObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.threedview.CenterViewOnObjects();
        }

        private void printerSettings2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printerSettings.ShowSimpleForm();
        }

        private void connectToolStripSplitButton_DropDownOpened(object sender, EventArgs e)
        {
            //this.mainUpdaterHelper.UpdateConnections();
        }

        private void connectToolStripSplitButton_MouseEnter(object sender, EventArgs e)
        {
            if (this.connectToolStripSplitButton.DropDownButtonPressed)
            {
                return; // do nothing
            }

            this.mainUpdaterHelper.UpdateConnections(); // update the ports
        }




        /// <summary>
        /// Updates the visibility or availablity of menu itmes based on the current developer mode.
        /// </summary>
        internal void UpdateMenuItemsForDeveloper()
        {
            this.printerSettingsToolStripMenuItem.Visible = DeveloperMode;
            this.repetierSettingsToolStripMenuItem.Visible = DeveloperMode;
            this.internalSlicingParameterToolStripMenuItem.Visible = DeveloperMode;
            this.dViewSettingsToolStripMenuItem.Visible = DeveloperMode;
            this.printerSettings2ToolStripMenuItem.Visible = DeveloperMode;
            this.temperatureToolStripMenuItem.Visible = DeveloperMode;
            this.printerToolStripMenuItem.Visible = DeveloperMode;
            this.gcodeEditorToolStripMenuItem.Visible = DeveloperMode;
            this.loggingToolStripMenuItem.Visible = DeveloperMode;
            this.soundConfigurationToolStripMenuItem.Visible = DeveloperMode;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Action to take on clicking sound menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void soundConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoundConfig.config.ShowDialog();
        }

        

        /// <summary>
        /// Action to take on clicking the calibration menu item. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calibrateHeightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calibrationZ.Visible = !calibrationZ.Visible;
        }
    }
}
