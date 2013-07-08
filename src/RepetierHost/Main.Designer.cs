namespace RepetierHost
{
    partial class Main
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadGCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showWorkdirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStartHistory = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripEndHistory = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printerSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.repetierSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.internalSlicingParameterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soundConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slicerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.slicerSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slicerConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gcodeEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopSlicingProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dViewSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.temperatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showExtruderTemperaturesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHeatedBedTemperaturesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTargetTemperaturesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAverageTemperaturesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHeaterPowerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoscrollTemperatureViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.timeperiodMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.temperatureZoomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minutes60ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minutes30ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minutes15ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minutes10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minutes5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minuteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.buildAverageOverMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secondsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minuteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minutesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minutesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.continuousMonitoringMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extruder1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extruder2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.heatedBedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printerInformationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSDCardManager = new System.Windows.Forms.ToolStripMenuItem();
            this.testCaseGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.sendScript1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendScript2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendScript3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendScript4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendScript5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userManual2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.supportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.status = new System.Windows.Forms.StatusStrip();
            this.toolConnection = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTempReading = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolAction = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.fpsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.saveJobDialog = new System.Windows.Forms.SaveFileDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.listSTLObjects = new System.Windows.Forms.ListBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.topViewStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.sideViewStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.frontStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.viewsStripSplitButton6 = new System.Windows.Forms.ToolStripSplitButton();
            this.rotateStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.moveStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomStripMenuItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.perspectiveStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.resetStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.connectToolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.importSTLToolSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.positionToolSplitButton2 = new System.Windows.Forms.ToolStripButton();
            this.sliceToolSplitButton3 = new System.Windows.Forms.ToolStripSplitButton();
            this.withRaftToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.withSupportsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.printStripSplitButton4 = new System.Windows.Forms.ToolStripSplitButton();
            this.killJobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emergencyStopStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.helpSplitButton3 = new System.Windows.Forms.ToolStripSplitButton();
            this.baoyanAutomationWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutSoftwareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedConfigStripSplitButton3 = new System.Windows.Forms.ToolStripSplitButton();
            this.saveGCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveNewSTLMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.sDCardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modeToolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.loadAFileMenuModeMenuOption = new System.Windows.Forms.ToolStripMenuItem();
            this.STLEditorMenuOption = new System.Windows.Forms.ToolStripMenuItem();
            this.gCodeVisualizationMenuOption = new System.Windows.Forms.ToolStripMenuItem();
            this.livePrintingMenuOption = new System.Windows.Forms.ToolStripMenuItem();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.openFileSTLorGcode = new System.Windows.Forms.OpenFileDialog();
            this.saveSTL = new System.Windows.Forms.SaveFileDialog();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.menu.SuspendLayout();
            this.status.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.temperatureToolStripMenuItem,
            this.printerToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(1017, 24);
            this.menu.TabIndex = 0;
            this.menu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadGCodeToolStripMenuItem,
            this.showWorkdirectoryToolStripMenuItem,
            this.toolStripStartHistory,
            this.toolStripEndHistory,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // loadGCodeToolStripMenuItem
            // 
            this.loadGCodeToolStripMenuItem.Name = "loadGCodeToolStripMenuItem";
            this.loadGCodeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.loadGCodeToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.loadGCodeToolStripMenuItem.Text = "&Load STL file or G-code";
            this.loadGCodeToolStripMenuItem.Click += new System.EventHandler(this.toolGCodeLoad_Click);
            // 
            // showWorkdirectoryToolStripMenuItem
            // 
            this.showWorkdirectoryToolStripMenuItem.Name = "showWorkdirectoryToolStripMenuItem";
            this.showWorkdirectoryToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.W)));
            this.showWorkdirectoryToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.showWorkdirectoryToolStripMenuItem.Text = "Show workdirectory";
            this.showWorkdirectoryToolStripMenuItem.Click += new System.EventHandler(this.showWorkdirectoryToolStripMenuItem_Click);
            // 
            // toolStripStartHistory
            // 
            this.toolStripStartHistory.Name = "toolStripStartHistory";
            this.toolStripStartHistory.Size = new System.Drawing.Size(217, 6);
            // 
            // toolStripEndHistory
            // 
            this.toolStripEndHistory.Name = "toolStripEndHistory";
            this.toolStripEndHistory.Size = new System.Drawing.Size(217, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.quitToolStripMenuItem.Text = "&Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.languageToolStripMenuItem,
            this.printerSettingsToolStripMenuItem,
            this.repetierSettingsToolStripMenuItem,
            this.internalSlicingParameterToolStripMenuItem,
            this.soundConfigurationToolStripMenuItem,
            this.slicerToolStripMenuItem1,
            this.dViewSettingsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.settingsToolStripMenuItem.Text = "&Configuration";
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.languageToolStripMenuItem.Text = "Language";
            this.languageToolStripMenuItem.Click += new System.EventHandler(this.languageToolStripMenuItem_Click);
            // 
            // printerSettingsToolStripMenuItem
            // 
            this.printerSettingsToolStripMenuItem.Name = "printerSettingsToolStripMenuItem";
            this.printerSettingsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printerSettingsToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.printerSettingsToolStripMenuItem.Text = "&Printer settings";
            this.printerSettingsToolStripMenuItem.Click += new System.EventHandler(this.printerSettingsToolStripMenuItem_Click);
            // 
            // repetierSettingsToolStripMenuItem
            // 
            this.repetierSettingsToolStripMenuItem.Name = "repetierSettingsToolStripMenuItem";
            this.repetierSettingsToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.repetierSettingsToolStripMenuItem.Text = "Software general configuration";
            this.repetierSettingsToolStripMenuItem.Click += new System.EventHandler(this.repetierSettingsToolStripMenuItem_Click);
            // 
            // internalSlicingParameterToolStripMenuItem
            // 
            this.internalSlicingParameterToolStripMenuItem.Name = "internalSlicingParameterToolStripMenuItem";
            this.internalSlicingParameterToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.internalSlicingParameterToolStripMenuItem.Text = "Test case slicing parameter";
            // 
            // soundConfigurationToolStripMenuItem
            // 
            this.soundConfigurationToolStripMenuItem.Name = "soundConfigurationToolStripMenuItem";
            this.soundConfigurationToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.soundConfigurationToolStripMenuItem.Text = "Sound configuration";
            //this.soundConfigurationToolStripMenuItem.Click += new System.EventHandler(this.soundConfigurationToolStripMenuItem_Click);
            // 
            // slicerToolStripMenuItem1
            // 
            this.slicerToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slicerSelectionToolStripMenuItem,
            this.slicerConfigurationToolStripMenuItem,
            this.gcodeEditorToolStripMenuItem,
            this.stopSlicingProcessToolStripMenuItem});
            this.slicerToolStripMenuItem1.Name = "slicerToolStripMenuItem1";
            this.slicerToolStripMenuItem1.Size = new System.Drawing.Size(237, 22);
            this.slicerToolStripMenuItem1.Text = "Slicer";
            // 
            // slicerSelectionToolStripMenuItem
            // 
            this.slicerSelectionToolStripMenuItem.Name = "slicerSelectionToolStripMenuItem";
            this.slicerSelectionToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.slicerSelectionToolStripMenuItem.Text = "Slicer Directory Setup";
            this.slicerSelectionToolStripMenuItem.Click += new System.EventHandler(this.slicerSelectionToolStripMenuItem_Click);
            // 
            // slicerConfigurationToolStripMenuItem
            // 
            this.slicerConfigurationToolStripMenuItem.Name = "slicerConfigurationToolStripMenuItem";
            this.slicerConfigurationToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.slicerConfigurationToolStripMenuItem.Text = "Slicer Configuration";
            this.slicerConfigurationToolStripMenuItem.Click += new System.EventHandler(this.slicerConfigurationToolStripMenuItem_Click);
            // 
            // gcodeEditorToolStripMenuItem
            // 
            this.gcodeEditorToolStripMenuItem.Name = "gcodeEditorToolStripMenuItem";
            this.gcodeEditorToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.gcodeEditorToolStripMenuItem.Text = "G-code Editor";
            this.gcodeEditorToolStripMenuItem.Click += new System.EventHandler(this.gcodeEditorToolStripMenuItem_Click);
            // 
            // stopSlicingProcessToolStripMenuItem
            // 
            this.stopSlicingProcessToolStripMenuItem.Name = "stopSlicingProcessToolStripMenuItem";
            this.stopSlicingProcessToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.stopSlicingProcessToolStripMenuItem.Text = "Stop Slicing Process";
            this.stopSlicingProcessToolStripMenuItem.Click += new System.EventHandler(this.stopSlicingProcessToolStripMenuItem_Click);
            // 
            // dViewSettingsToolStripMenuItem
            // 
            this.dViewSettingsToolStripMenuItem.Name = "dViewSettingsToolStripMenuItem";
            this.dViewSettingsToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.dViewSettingsToolStripMenuItem.Text = "3D View Settings";
            this.dViewSettingsToolStripMenuItem.Click += new System.EventHandler(this.dViewSettingsToolStripMenuItem_Click);
            // 
            // temperatureToolStripMenuItem
            // 
            this.temperatureToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showExtruderTemperaturesMenuItem,
            this.showHeatedBedTemperaturesMenuItem,
            this.showTargetTemperaturesMenuItem,
            this.showAverageTemperaturesMenuItem,
            this.showHeaterPowerMenuItem,
            this.autoscrollTemperatureViewMenuItem,
            this.toolStripMenuItem5,
            this.timeperiodMenuItem,
            this.temperatureZoomMenuItem,
            this.buildAverageOverMenuItem,
            this.continuousMonitoringMenuItem});
            this.temperatureToolStripMenuItem.Name = "temperatureToolStripMenuItem";
            this.temperatureToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
            this.temperatureToolStripMenuItem.Text = "&Temperature";
            this.temperatureToolStripMenuItem.Visible = false;
            // 
            // showExtruderTemperaturesMenuItem
            // 
            this.showExtruderTemperaturesMenuItem.Name = "showExtruderTemperaturesMenuItem";
            this.showExtruderTemperaturesMenuItem.Size = new System.Drawing.Size(238, 22);
            this.showExtruderTemperaturesMenuItem.Text = "Show extruder temperatures";
            this.showExtruderTemperaturesMenuItem.Click += new System.EventHandler(this.showExtruderTemperaturesMenuItem_Click);
            // 
            // showHeatedBedTemperaturesMenuItem
            // 
            this.showHeatedBedTemperaturesMenuItem.Name = "showHeatedBedTemperaturesMenuItem";
            this.showHeatedBedTemperaturesMenuItem.Size = new System.Drawing.Size(238, 22);
            this.showHeatedBedTemperaturesMenuItem.Text = "Show heated bed temperatures";
            this.showHeatedBedTemperaturesMenuItem.Click += new System.EventHandler(this.showHeatedBedTemperaturesMenuItem_Click);
            // 
            // showTargetTemperaturesMenuItem
            // 
            this.showTargetTemperaturesMenuItem.Name = "showTargetTemperaturesMenuItem";
            this.showTargetTemperaturesMenuItem.Size = new System.Drawing.Size(238, 22);
            this.showTargetTemperaturesMenuItem.Text = "Show target temperatures";
            this.showTargetTemperaturesMenuItem.Click += new System.EventHandler(this.showTargetTemperaturesMenuItem_Click);
            // 
            // showAverageTemperaturesMenuItem
            // 
            this.showAverageTemperaturesMenuItem.Name = "showAverageTemperaturesMenuItem";
            this.showAverageTemperaturesMenuItem.Size = new System.Drawing.Size(238, 22);
            this.showAverageTemperaturesMenuItem.Text = "Show average temperatures";
            this.showAverageTemperaturesMenuItem.Click += new System.EventHandler(this.showAverageTemperaturesMenuItem_Click);
            // 
            // showHeaterPowerMenuItem
            // 
            this.showHeaterPowerMenuItem.Name = "showHeaterPowerMenuItem";
            this.showHeaterPowerMenuItem.Size = new System.Drawing.Size(238, 22);
            this.showHeaterPowerMenuItem.Text = "Show heater power";
            this.showHeaterPowerMenuItem.Click += new System.EventHandler(this.showHeaterPowerMenuItem_Click);
            // 
            // autoscrollTemperatureViewMenuItem
            // 
            this.autoscrollTemperatureViewMenuItem.Name = "autoscrollTemperatureViewMenuItem";
            this.autoscrollTemperatureViewMenuItem.Size = new System.Drawing.Size(238, 22);
            this.autoscrollTemperatureViewMenuItem.Text = "Autoscroll temperature view";
            this.autoscrollTemperatureViewMenuItem.Click += new System.EventHandler(this.autoscrollTemperatureViewMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(235, 6);
            // 
            // timeperiodMenuItem
            // 
            this.timeperiodMenuItem.Name = "timeperiodMenuItem";
            this.timeperiodMenuItem.Size = new System.Drawing.Size(238, 22);
            this.timeperiodMenuItem.Text = "Timeperiod";
            // 
            // temperatureZoomMenuItem
            // 
            this.temperatureZoomMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.minutes60ToolStripMenuItem,
            this.minutes30ToolStripMenuItem,
            this.minutes15ToolStripMenuItem,
            this.minutes10ToolStripMenuItem,
            this.minutes5ToolStripMenuItem,
            this.minuteToolStripMenuItem1});
            this.temperatureZoomMenuItem.Name = "temperatureZoomMenuItem";
            this.temperatureZoomMenuItem.Size = new System.Drawing.Size(238, 22);
            this.temperatureZoomMenuItem.Text = "Zoom";
            // 
            // minutes60ToolStripMenuItem
            // 
            this.minutes60ToolStripMenuItem.Name = "minutes60ToolStripMenuItem";
            this.minutes60ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.minutes60ToolStripMenuItem.Tag = "60";
            this.minutes60ToolStripMenuItem.Text = "60 Minues";
            this.minutes60ToolStripMenuItem.Click += new System.EventHandler(this.selectZoom);
            // 
            // minutes30ToolStripMenuItem
            // 
            this.minutes30ToolStripMenuItem.Name = "minutes30ToolStripMenuItem";
            this.minutes30ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.minutes30ToolStripMenuItem.Tag = "30";
            this.minutes30ToolStripMenuItem.Text = "30 Minutes";
            this.minutes30ToolStripMenuItem.Click += new System.EventHandler(this.selectZoom);
            // 
            // minutes15ToolStripMenuItem
            // 
            this.minutes15ToolStripMenuItem.Name = "minutes15ToolStripMenuItem";
            this.minutes15ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.minutes15ToolStripMenuItem.Tag = "15";
            this.minutes15ToolStripMenuItem.Text = "15 Minutes";
            this.minutes15ToolStripMenuItem.Click += new System.EventHandler(this.selectZoom);
            // 
            // minutes10ToolStripMenuItem
            // 
            this.minutes10ToolStripMenuItem.Name = "minutes10ToolStripMenuItem";
            this.minutes10ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.minutes10ToolStripMenuItem.Tag = "10";
            this.minutes10ToolStripMenuItem.Text = "10 Minutes";
            this.minutes10ToolStripMenuItem.Click += new System.EventHandler(this.selectZoom);
            // 
            // minutes5ToolStripMenuItem
            // 
            this.minutes5ToolStripMenuItem.Name = "minutes5ToolStripMenuItem";
            this.minutes5ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.minutes5ToolStripMenuItem.Tag = "5";
            this.minutes5ToolStripMenuItem.Text = "5 Minutes";
            this.minutes5ToolStripMenuItem.Click += new System.EventHandler(this.selectZoom);
            // 
            // minuteToolStripMenuItem1
            // 
            this.minuteToolStripMenuItem1.Name = "minuteToolStripMenuItem1";
            this.minuteToolStripMenuItem1.Size = new System.Drawing.Size(132, 22);
            this.minuteToolStripMenuItem1.Tag = "1";
            this.minuteToolStripMenuItem1.Text = "1 Minute";
            this.minuteToolStripMenuItem1.Click += new System.EventHandler(this.selectZoom);
            // 
            // buildAverageOverMenuItem
            // 
            this.buildAverageOverMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.secondsToolStripMenuItem,
            this.minuteToolStripMenuItem,
            this.minutesToolStripMenuItem,
            this.minutesToolStripMenuItem1});
            this.buildAverageOverMenuItem.Name = "buildAverageOverMenuItem";
            this.buildAverageOverMenuItem.Size = new System.Drawing.Size(238, 22);
            this.buildAverageOverMenuItem.Text = "Build average over ...";
            // 
            // secondsToolStripMenuItem
            // 
            this.secondsToolStripMenuItem.Name = "secondsToolStripMenuItem";
            this.secondsToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.secondsToolStripMenuItem.Tag = "30";
            this.secondsToolStripMenuItem.Text = "30 seconds";
            this.secondsToolStripMenuItem.Click += new System.EventHandler(this.selectAverage);
            // 
            // minuteToolStripMenuItem
            // 
            this.minuteToolStripMenuItem.Name = "minuteToolStripMenuItem";
            this.minuteToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.minuteToolStripMenuItem.Tag = "60";
            this.minuteToolStripMenuItem.Text = "1 Minute";
            this.minuteToolStripMenuItem.Click += new System.EventHandler(this.selectAverage);
            // 
            // minutesToolStripMenuItem
            // 
            this.minutesToolStripMenuItem.Name = "minutesToolStripMenuItem";
            this.minutesToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.minutesToolStripMenuItem.Tag = "120";
            this.minutesToolStripMenuItem.Text = "2 Minutes";
            this.minutesToolStripMenuItem.Click += new System.EventHandler(this.selectAverage);
            // 
            // minutesToolStripMenuItem1
            // 
            this.minutesToolStripMenuItem1.Name = "minutesToolStripMenuItem1";
            this.minutesToolStripMenuItem1.Size = new System.Drawing.Size(132, 22);
            this.minutesToolStripMenuItem1.Tag = "300";
            this.minutesToolStripMenuItem1.Text = "5 Minutes";
            this.minutesToolStripMenuItem1.Click += new System.EventHandler(this.selectAverage);
            // 
            // continuousMonitoringMenuItem
            // 
            this.continuousMonitoringMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disableToolStripMenuItem,
            this.extruder1ToolStripMenuItem,
            this.extruder2ToolStripMenuItem,
            this.heatedBedToolStripMenuItem});
            this.continuousMonitoringMenuItem.Enabled = false;
            this.continuousMonitoringMenuItem.Name = "continuousMonitoringMenuItem";
            this.continuousMonitoringMenuItem.Size = new System.Drawing.Size(238, 22);
            this.continuousMonitoringMenuItem.Text = "Continuous monitoring";
            // 
            // disableToolStripMenuItem
            // 
            this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            this.disableToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.disableToolStripMenuItem.Text = "Disable";
            this.disableToolStripMenuItem.Click += new System.EventHandler(this.disableToolStripMenuItem_Click);
            // 
            // extruder1ToolStripMenuItem
            // 
            this.extruder1ToolStripMenuItem.Name = "extruder1ToolStripMenuItem";
            this.extruder1ToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.extruder1ToolStripMenuItem.Text = "Extruder 1";
            this.extruder1ToolStripMenuItem.Click += new System.EventHandler(this.extruder1ToolStripMenuItem_Click);
            // 
            // extruder2ToolStripMenuItem
            // 
            this.extruder2ToolStripMenuItem.Name = "extruder2ToolStripMenuItem";
            this.extruder2ToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.extruder2ToolStripMenuItem.Text = "Extruder 2";
            this.extruder2ToolStripMenuItem.Click += new System.EventHandler(this.extruder2ToolStripMenuItem_Click);
            // 
            // heatedBedToolStripMenuItem
            // 
            this.heatedBedToolStripMenuItem.Name = "heatedBedToolStripMenuItem";
            this.heatedBedToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.heatedBedToolStripMenuItem.Text = "Heated Bed";
            this.heatedBedToolStripMenuItem.Click += new System.EventHandler(this.heatedBedToolStripMenuItem_Click);
            // 
            // printerToolStripMenuItem
            // 
            this.printerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printerInformationsToolStripMenuItem,
            this.jobStatusToolStripMenuItem,
            this.menuSDCardManager,
            this.testCaseGeneratorToolStripMenuItem,
            this.toolStripMenuItem6,
            this.sendScript1ToolStripMenuItem,
            this.sendScript2ToolStripMenuItem,
            this.sendScript3ToolStripMenuItem,
            this.sendScript4ToolStripMenuItem,
            this.sendScript5ToolStripMenuItem});
            this.printerToolStripMenuItem.Name = "printerToolStripMenuItem";
            this.printerToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.printerToolStripMenuItem.Text = "&Printer";
            this.printerToolStripMenuItem.Visible = false;
            // 
            // printerInformationsToolStripMenuItem
            // 
            this.printerInformationsToolStripMenuItem.Name = "printerInformationsToolStripMenuItem";
            this.printerInformationsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.P)));
            this.printerInformationsToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.printerInformationsToolStripMenuItem.Text = "Printer information";
            this.printerInformationsToolStripMenuItem.Click += new System.EventHandler(this.printerInformationsToolStripMenuItem_Click);
            // 
            // jobStatusToolStripMenuItem
            // 
            this.jobStatusToolStripMenuItem.Name = "jobStatusToolStripMenuItem";
            this.jobStatusToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.J)));
            this.jobStatusToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.jobStatusToolStripMenuItem.Text = "Job status";
            // 
            // menuSDCardManager
            // 
            this.menuSDCardManager.Name = "menuSDCardManager";
            this.menuSDCardManager.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C)));
            this.menuSDCardManager.Size = new System.Drawing.Size(212, 22);
            this.menuSDCardManager.Text = "SD card manager";
            // 
            // testCaseGeneratorToolStripMenuItem
            // 
            this.testCaseGeneratorToolStripMenuItem.Name = "testCaseGeneratorToolStripMenuItem";
            this.testCaseGeneratorToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.testCaseGeneratorToolStripMenuItem.Text = "Test case generator";
            this.testCaseGeneratorToolStripMenuItem.Click += new System.EventHandler(this.testCaseGeneratorToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(209, 6);
            // 
            // sendScript1ToolStripMenuItem
            // 
            this.sendScript1ToolStripMenuItem.Name = "sendScript1ToolStripMenuItem";
            this.sendScript1ToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.sendScript1ToolStripMenuItem.Text = "Send script 1";
            this.sendScript1ToolStripMenuItem.Click += new System.EventHandler(this.sendScript1ToolStripMenuItem_Click);
            // 
            // sendScript2ToolStripMenuItem
            // 
            this.sendScript2ToolStripMenuItem.Name = "sendScript2ToolStripMenuItem";
            this.sendScript2ToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.sendScript2ToolStripMenuItem.Text = "Send script 2";
            this.sendScript2ToolStripMenuItem.Click += new System.EventHandler(this.sendScript2ToolStripMenuItem_Click);
            // 
            // sendScript3ToolStripMenuItem
            // 
            this.sendScript3ToolStripMenuItem.Name = "sendScript3ToolStripMenuItem";
            this.sendScript3ToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.sendScript3ToolStripMenuItem.Text = "Send script 3";
            this.sendScript3ToolStripMenuItem.Click += new System.EventHandler(this.sendScript3ToolStripMenuItem_Click);
            // 
            // sendScript4ToolStripMenuItem
            // 
            this.sendScript4ToolStripMenuItem.Name = "sendScript4ToolStripMenuItem";
            this.sendScript4ToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.sendScript4ToolStripMenuItem.Text = "Send script 4";
            this.sendScript4ToolStripMenuItem.Click += new System.EventHandler(this.sendScript4ToolStripMenuItem_Click);
            // 
            // sendScript5ToolStripMenuItem
            // 
            this.sendScript5ToolStripMenuItem.Name = "sendScript5ToolStripMenuItem";
            this.sendScript5ToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.sendScript5ToolStripMenuItem.Text = "Send script 5";
            this.sendScript5ToolStripMenuItem.Click += new System.EventHandler(this.sendScript5ToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userManual2ToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem,
            this.supportToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // userManual2ToolStripMenuItem
            // 
            this.userManual2ToolStripMenuItem.Name = "userManual2ToolStripMenuItem";
            this.userManual2ToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.userManual2ToolStripMenuItem.Text = "Manual";
            this.userManual2ToolStripMenuItem.Click += new System.EventHandler(this.manualToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // supportToolStripMenuItem
            // 
            this.supportToolStripMenuItem.Name = "supportToolStripMenuItem";
            this.supportToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.supportToolStripMenuItem.Text = "Support";
            this.supportToolStripMenuItem.Click += new System.EventHandler(this.supportToolStripMenuItem_Click);
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolConnection,
            this.toolTempReading,
            this.toolAction,
            this.toolProgress,
            this.fpsLabel,
            this.toolStripStatusLabel1});
            this.status.Location = new System.Drawing.Point(0, 527);
            this.status.Name = "status";
            this.status.Padding = new System.Windows.Forms.Padding(1, 0, 15, 0);
            this.status.Size = new System.Drawing.Size(1017, 22);
            this.status.TabIndex = 1;
            this.status.Text = "statusStrip1";
            // 
            // toolConnection
            // 
            this.toolConnection.BackColor = System.Drawing.Color.Transparent;
            this.toolConnection.Name = "toolConnection";
            this.toolConnection.Size = new System.Drawing.Size(86, 17);
            this.toolConnection.Text = "Not connected";
            // 
            // toolTempReading
            // 
            this.toolTempReading.BackColor = System.Drawing.Color.Transparent;
            this.toolTempReading.Margin = new System.Windows.Forms.Padding(0, 3, 8, 2);
            this.toolTempReading.Name = "toolTempReading";
            this.toolTempReading.Size = new System.Drawing.Size(12, 17);
            this.toolTempReading.Text = "-";
            // 
            // toolAction
            // 
            this.toolAction.BackColor = System.Drawing.Color.Transparent;
            this.toolAction.Name = "toolAction";
            this.toolAction.Size = new System.Drawing.Size(26, 17);
            this.toolAction.Text = "Idle";
            this.toolAction.Click += new System.EventHandler(this.toolAction_Click);
            // 
            // toolProgress
            // 
            this.toolProgress.Name = "toolProgress";
            this.toolProgress.Size = new System.Drawing.Size(214, 16);
            // 
            // fpsLabel
            // 
            this.fpsLabel.BackColor = System.Drawing.Color.Transparent;
            this.fpsLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(34, 17);
            this.fpsLabel.Text = "- FPS";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "7Stop.png");
            this.imageList.Images.SetKeyName(1, "8Pause.PNG");
            this.imageList.Images.SetKeyName(2, "9Resume.png");
            this.imageList.Images.SetKeyName(3, "disconnect32.png");
            this.imageList.Images.SetKeyName(4, "connect32.png");
            // 
            // saveJobDialog
            // 
            this.saveJobDialog.DefaultExt = "gcode";
            this.saveJobDialog.Filter = "GCode|*.gcode|All files|*.*";
            this.saveJobDialog.Title = "Save G-Code";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.listSTLObjects);
            this.panel2.Controls.Add(this.toolStrip2);
            this.panel2.Controls.Add(this.toolStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1017, 503);
            this.panel2.TabIndex = 6;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // listSTLObjects
            // 
            this.listSTLObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listSTLObjects.BackColor = System.Drawing.Color.Gray;
            this.listSTLObjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listSTLObjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listSTLObjects.ForeColor = System.Drawing.Color.Transparent;
            this.listSTLObjects.FormattingEnabled = true;
            this.listSTLObjects.ItemHeight = 16;
            this.listSTLObjects.Location = new System.Drawing.Point(716, 57);
            this.listSTLObjects.Name = "listSTLObjects";
            this.listSTLObjects.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listSTLObjects.Size = new System.Drawing.Size(301, 16);
            this.listSTLObjects.TabIndex = 13;
            this.listSTLObjects.TabStop = false;
            this.listSTLObjects.SelectedIndexChanged += new System.EventHandler(this.listSTLObjects_SelectedIndexChanged);
            this.listSTLObjects.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listSTLObjects_KeyDown);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.topViewStripButton1,
            this.sideViewStripButton1,
            this.frontStripButton1,
            this.viewsStripSplitButton6});
            this.toolStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStrip2.Location = new System.Drawing.Point(0, 184);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(49, 227);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // topViewStripButton1
            // 
            this.topViewStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("topViewStripButton1.Image")));
            this.topViewStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.topViewStripButton1.Name = "topViewStripButton1";
            this.topViewStripButton1.Size = new System.Drawing.Size(47, 51);
            this.topViewStripButton1.Text = "Top";
            this.topViewStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.topViewStripButton1.Click += new System.EventHandler(this.topViewStripButton1_Click);
            // 
            // sideViewStripButton1
            // 
            this.sideViewStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("sideViewStripButton1.Image")));
            this.sideViewStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sideViewStripButton1.Name = "sideViewStripButton1";
            this.sideViewStripButton1.Size = new System.Drawing.Size(47, 51);
            this.sideViewStripButton1.Text = "Side";
            this.sideViewStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.sideViewStripButton1.Click += new System.EventHandler(this.sideViewStripButton1_Click);
            // 
            // frontStripButton1
            // 
            this.frontStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("frontStripButton1.Image")));
            this.frontStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.frontStripButton1.Name = "frontStripButton1";
            this.frontStripButton1.Size = new System.Drawing.Size(47, 51);
            this.frontStripButton1.Text = "Front";
            this.frontStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.frontStripButton1.Click += new System.EventHandler(this.frontStripButton1_Click);
            // 
            // viewsStripSplitButton6
            // 
            this.viewsStripSplitButton6.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rotateStripMenuItem11,
            this.moveStripMenuItem12,
            this.zoomStripMenuItem13,
            this.perspectiveStripMenuItem14,
            this.resetStripMenuItem15});
            this.viewsStripSplitButton6.Image = ((System.Drawing.Image)(resources.GetObject("viewsStripSplitButton6.Image")));
            this.viewsStripSplitButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.viewsStripSplitButton6.Name = "viewsStripSplitButton6";
            this.viewsStripSplitButton6.Size = new System.Drawing.Size(47, 51);
            this.viewsStripSplitButton6.Text = "View";
            this.viewsStripSplitButton6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // rotateStripMenuItem11
            // 
            this.rotateStripMenuItem11.Name = "rotateStripMenuItem11";
            this.rotateStripMenuItem11.Size = new System.Drawing.Size(134, 22);
            this.rotateStripMenuItem11.Text = "Rotate";
            this.rotateStripMenuItem11.Click += new System.EventHandler(this.toolStripMenuItem11_Click);
            // 
            // moveStripMenuItem12
            // 
            this.moveStripMenuItem12.Name = "moveStripMenuItem12";
            this.moveStripMenuItem12.Size = new System.Drawing.Size(134, 22);
            this.moveStripMenuItem12.Text = "Move";
            this.moveStripMenuItem12.Click += new System.EventHandler(this.moveStripMenuItem12_Click);
            // 
            // zoomStripMenuItem13
            // 
            this.zoomStripMenuItem13.Name = "zoomStripMenuItem13";
            this.zoomStripMenuItem13.Size = new System.Drawing.Size(134, 22);
            this.zoomStripMenuItem13.Text = "Zoom";
            this.zoomStripMenuItem13.Click += new System.EventHandler(this.zoomStripMenuItem13_Click);
            // 
            // perspectiveStripMenuItem14
            // 
            this.perspectiveStripMenuItem14.Name = "perspectiveStripMenuItem14";
            this.perspectiveStripMenuItem14.Size = new System.Drawing.Size(134, 22);
            this.perspectiveStripMenuItem14.Text = "Perspective";
            this.perspectiveStripMenuItem14.Click += new System.EventHandler(this.perspectiveStripMenuItem14_Click);
            // 
            // resetStripMenuItem15
            // 
            this.resetStripMenuItem15.Name = "resetStripMenuItem15";
            this.resetStripMenuItem15.Size = new System.Drawing.Size(134, 22);
            this.resetStripMenuItem15.Text = "Reset";
            this.resetStripMenuItem15.Click += new System.EventHandler(this.resetStripMenuItem15_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripSplitButton,
            this.importSTLToolSplitButton1,
            this.positionToolSplitButton2,
            this.sliceToolSplitButton3,
            this.printStripSplitButton4,
            this.emergencyStopStripButton6,
            this.helpSplitButton3,
            this.advancedConfigStripSplitButton3,
            this.modeToolStripSplitButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1017, 54);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // connectToolStripSplitButton
            // 
            this.connectToolStripSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("connectToolStripSplitButton.Image")));
            this.connectToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectToolStripSplitButton.Name = "connectToolStripSplitButton";
            this.connectToolStripSplitButton.Size = new System.Drawing.Size(68, 51);
            this.connectToolStripSplitButton.Text = "Connect";
            this.connectToolStripSplitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.connectToolStripSplitButton.ToolTipText = "Connect printer";
            this.connectToolStripSplitButton.ButtonClick += new System.EventHandler(this.connectToolStripSplitButton_ButtonClick);
            // 
            // importSTLToolSplitButton1
            // 
            this.importSTLToolSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("importSTLToolSplitButton1.Image")));
            this.importSTLToolSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importSTLToolSplitButton1.Name = "importSTLToolSplitButton1";
            this.importSTLToolSplitButton1.Size = new System.Drawing.Size(59, 51);
            this.importSTLToolSplitButton1.Text = "Import";
            this.importSTLToolSplitButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.importSTLToolSplitButton1.ButtonClick += new System.EventHandler(this.importSTLToolSplitButton1_ButtonClick_1);
            // 
            // positionToolSplitButton2
            // 
            this.positionToolSplitButton2.Image = ((System.Drawing.Image)(resources.GetObject("positionToolSplitButton2.Image")));
            this.positionToolSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.positionToolSplitButton2.Name = "positionToolSplitButton2";
            this.positionToolSplitButton2.Size = new System.Drawing.Size(54, 51);
            this.positionToolSplitButton2.Text = "Position";
            this.positionToolSplitButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.positionToolSplitButton2.Click += new System.EventHandler(this.positionToolSplitButton2_Click);
            // 
            // sliceToolSplitButton3
            // 
            this.sliceToolSplitButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.withRaftToolStripMenuItem1,
            this.withSupportsToolStripMenuItem1});
            this.sliceToolSplitButton3.Image = ((System.Drawing.Image)(resources.GetObject("sliceToolSplitButton3.Image")));
            this.sliceToolSplitButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sliceToolSplitButton3.Name = "sliceToolSplitButton3";
            this.sliceToolSplitButton3.Size = new System.Drawing.Size(48, 51);
            this.sliceToolSplitButton3.Text = "Slice";
            this.sliceToolSplitButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.sliceToolSplitButton3.ButtonClick += new System.EventHandler(this.sliceToolSplitButton3_ButtonClick);
            // 
            // withRaftToolStripMenuItem1
            // 
            this.withRaftToolStripMenuItem1.Name = "withRaftToolStripMenuItem1";
            this.withRaftToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.withRaftToolStripMenuItem1.Text = "With Raft";
            this.withRaftToolStripMenuItem1.Click += new System.EventHandler(this.withRaftToolStripMenuItem1_Click);
            // 
            // withSupportsToolStripMenuItem1
            // 
            this.withSupportsToolStripMenuItem1.Name = "withSupportsToolStripMenuItem1";
            this.withSupportsToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.withSupportsToolStripMenuItem1.Text = "With Supports";
            this.withSupportsToolStripMenuItem1.Click += new System.EventHandler(this.withSupportsToolStripMenuItem1_Click);
            // 
            // printStripSplitButton4
            // 
            this.printStripSplitButton4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.killJobToolStripMenuItem});
            this.printStripSplitButton4.Image = ((System.Drawing.Image)(resources.GetObject("printStripSplitButton4.Image")));
            this.printStripSplitButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printStripSplitButton4.Name = "printStripSplitButton4";
            this.printStripSplitButton4.Size = new System.Drawing.Size(48, 51);
            this.printStripSplitButton4.Text = "Print";
            this.printStripSplitButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.printStripSplitButton4.ButtonClick += new System.EventHandler(this.printStripSplitButton4_ButtonClick);
            // 
            // killJobToolStripMenuItem
            // 
            this.killJobToolStripMenuItem.Name = "killJobToolStripMenuItem";
            this.killJobToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.killJobToolStripMenuItem.Text = "Kill Job";
            this.killJobToolStripMenuItem.Click += new System.EventHandler(this.killJobToolStripMenuItem_Click);
            // 
            // emergencyStopStripButton6
            // 
            this.emergencyStopStripButton6.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.emergencyStopStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("emergencyStopStripButton6.Image")));
            this.emergencyStopStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.emergencyStopStripButton6.Name = "emergencyStopStripButton6";
            this.emergencyStopStripButton6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.emergencyStopStripButton6.Size = new System.Drawing.Size(97, 51);
            this.emergencyStopStripButton6.Text = "Emergency Stop";
            this.emergencyStopStripButton6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.emergencyStopStripButton6.Click += new System.EventHandler(this.emergencyStopStripButton6_Click);
            // 
            // helpSplitButton3
            // 
            this.helpSplitButton3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.helpSplitButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.baoyanAutomationWebsiteToolStripMenuItem,
            this.userManualToolStripMenuItem,
            this.aboutSoftwareToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem1});
            this.helpSplitButton3.Image = ((System.Drawing.Image)(resources.GetObject("helpSplitButton3.Image")));
            this.helpSplitButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpSplitButton3.Name = "helpSplitButton3";
            this.helpSplitButton3.Size = new System.Drawing.Size(48, 51);
            this.helpSplitButton3.Text = "Help";
            this.helpSplitButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.helpSplitButton3.ButtonClick += new System.EventHandler(this.helpSplitButton3_ButtonClick);
            // 
            // baoyanAutomationWebsiteToolStripMenuItem
            // 
            this.baoyanAutomationWebsiteToolStripMenuItem.Name = "baoyanAutomationWebsiteToolStripMenuItem";
            this.baoyanAutomationWebsiteToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.baoyanAutomationWebsiteToolStripMenuItem.Text = "Baoyan Automation Website";
            // 
            // userManualToolStripMenuItem
            // 
            this.userManualToolStripMenuItem.Name = "userManualToolStripMenuItem";
            this.userManualToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.userManualToolStripMenuItem.Text = "User Manual";
            // 
            // aboutSoftwareToolStripMenuItem
            // 
            this.aboutSoftwareToolStripMenuItem.Name = "aboutSoftwareToolStripMenuItem";
            this.aboutSoftwareToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.aboutSoftwareToolStripMenuItem.Text = "About Software";
            // 
            // checkForUpdatesToolStripMenuItem1
            // 
            this.checkForUpdatesToolStripMenuItem1.Name = "checkForUpdatesToolStripMenuItem1";
            this.checkForUpdatesToolStripMenuItem1.Size = new System.Drawing.Size(225, 22);
            this.checkForUpdatesToolStripMenuItem1.Text = "Check for Updates";
            // 
            // advancedConfigStripSplitButton3
            // 
            this.advancedConfigStripSplitButton3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.advancedConfigStripSplitButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveGCodeToolStripMenuItem,
            this.saveNewSTLMenuItem11,
            this.sDCardToolStripMenuItem,
            this.loggingToolStripMenuItem,
            this.manualControlToolStripMenuItem});
            this.advancedConfigStripSplitButton3.Image = ((System.Drawing.Image)(resources.GetObject("advancedConfigStripSplitButton3.Image")));
            this.advancedConfigStripSplitButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.advancedConfigStripSplitButton3.Name = "advancedConfigStripSplitButton3";
            this.advancedConfigStripSplitButton3.Size = new System.Drawing.Size(76, 51);
            this.advancedConfigStripSplitButton3.Text = "Advanced";
            this.advancedConfigStripSplitButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.advancedConfigStripSplitButton3.ButtonClick += new System.EventHandler(this.advancedConfigStripSplitButton3_ButtonClick);
            // 
            // saveGCodeToolStripMenuItem
            // 
            this.saveGCodeToolStripMenuItem.Name = "saveGCodeToolStripMenuItem";
            this.saveGCodeToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.saveGCodeToolStripMenuItem.Text = "Save G-code";
            this.saveGCodeToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
            // 
            // saveNewSTLMenuItem11
            // 
            this.saveNewSTLMenuItem11.Name = "saveNewSTLMenuItem11";
            this.saveNewSTLMenuItem11.Size = new System.Drawing.Size(189, 22);
            this.saveNewSTLMenuItem11.Text = "Save as new stl model";
            this.saveNewSTLMenuItem11.Click += new System.EventHandler(this.toolStripMenuItem11_Click_1);
            // 
            // sDCardToolStripMenuItem
            // 
            this.sDCardToolStripMenuItem.Name = "sDCardToolStripMenuItem";
            this.sDCardToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.sDCardToolStripMenuItem.Text = "SD Card";
            this.sDCardToolStripMenuItem.Visible = false;
            this.sDCardToolStripMenuItem.Click += new System.EventHandler(this.sDCardToolStripMenuItem_Click);
            // 
            // loggingToolStripMenuItem
            // 
            this.loggingToolStripMenuItem.Name = "loggingToolStripMenuItem";
            this.loggingToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.loggingToolStripMenuItem.Text = "Logging";
            this.loggingToolStripMenuItem.Click += new System.EventHandler(this.loggingToolStripMenuItem_Click);
            // 
            // manualControlToolStripMenuItem
            // 
            this.manualControlToolStripMenuItem.Name = "manualControlToolStripMenuItem";
            this.manualControlToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.manualControlToolStripMenuItem.Text = "Manual Control";
            this.manualControlToolStripMenuItem.Click += new System.EventHandler(this.manualControlToolStripMenuItem_Click);
            // 
            // modeToolStripSplitButton1
            // 
            this.modeToolStripSplitButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.modeToolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadAFileMenuModeMenuOption,
            this.STLEditorMenuOption,
            this.gCodeVisualizationMenuOption,
            this.livePrintingMenuOption});
            this.modeToolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("modeToolStripSplitButton1.Image")));
            this.modeToolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.modeToolStripSplitButton1.Name = "modeToolStripSplitButton1";
            this.modeToolStripSplitButton1.Size = new System.Drawing.Size(54, 51);
            this.modeToolStripSplitButton1.Text = "Mode";
            this.modeToolStripSplitButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // loadAFileMenuModeMenuOption
            // 
            this.loadAFileMenuModeMenuOption.Name = "loadAFileMenuModeMenuOption";
            this.loadAFileMenuModeMenuOption.Size = new System.Drawing.Size(177, 22);
            this.loadAFileMenuModeMenuOption.Text = "Load a File";
            this.loadAFileMenuModeMenuOption.Click += new System.EventHandler(this.loadAFileMenuModeMenuOption_Click);
            // 
            // STLEditorMenuOption
            // 
            this.STLEditorMenuOption.Name = "STLEditorMenuOption";
            this.STLEditorMenuOption.Size = new System.Drawing.Size(177, 22);
            this.STLEditorMenuOption.Text = "stlEditor";
            this.STLEditorMenuOption.Click += new System.EventHandler(this.STLEditorMenuOption_Click);
            // 
            // gCodeVisualizationMenuOption
            // 
            this.gCodeVisualizationMenuOption.Name = "gCodeVisualizationMenuOption";
            this.gCodeVisualizationMenuOption.Size = new System.Drawing.Size(177, 22);
            this.gCodeVisualizationMenuOption.Text = "Gcode Visualization";
            this.gCodeVisualizationMenuOption.Click += new System.EventHandler(this.gCodeVisualizationMenuOption_Click);
            // 
            // livePrintingMenuOption
            // 
            this.livePrintingMenuOption.Name = "livePrintingMenuOption";
            this.livePrintingMenuOption.Size = new System.Drawing.Size(177, 22);
            this.livePrintingMenuOption.Text = "Printing ";
            this.livePrintingMenuOption.Click += new System.EventHandler(this.livePrintingMenuOption_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // openFileSTLorGcode
            // 
            this.openFileSTLorGcode.DefaultExt = "stl";
            this.openFileSTLorGcode.Filter = "STL-Files/Gcode|*.stl;*.STL;*.gcode;*GCODE;*.gco;*GCO;|All files|*.*";
            this.openFileSTLorGcode.Multiselect = true;
            this.openFileSTLorGcode.Title = "Add STL or Gcode file";
            // 
            // saveSTL
            // 
            this.saveSTL.DefaultExt = "stl";
            this.saveSTL.Filter = "STL-Files|*.stl;*.STL";
            this.saveSTL.Title = "Save composition";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1017, 549);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.status);
            this.Controls.Add(this.menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.Name = "Main";
            this.Text = "Baoyan Automation v0.2 beta";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.Main_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.SizeChanged += new System.EventHandler(this.Main_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolConnection;
        private System.Windows.Forms.ToolStripStatusLabel toolAction;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printerSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printerInformationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem repetierSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jobStatusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userManual2ToolStripMenuItem;
        public System.Windows.Forms.ToolStripStatusLabel fpsLabel;
        private System.Windows.Forms.ToolStripMenuItem testCaseGeneratorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem internalSlicingParameterToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem menuSDCardManager;
        public System.Windows.Forms.SaveFileDialog saveJobDialog;
        private System.Windows.Forms.Timer timer;
        public System.Windows.Forms.ToolStripStatusLabel toolTempReading;
        private System.Windows.Forms.ToolStripMenuItem temperatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem minutes60ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minutes30ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minuteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minutesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minutesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem minutes15ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minutes10ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minutes5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minuteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem disableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extruder1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extruder2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem heatedBedToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem showExtruderTemperaturesMenuItem;
        public System.Windows.Forms.ToolStripMenuItem showHeatedBedTemperaturesMenuItem;
        public System.Windows.Forms.ToolStripMenuItem showTargetTemperaturesMenuItem;
        public System.Windows.Forms.ToolStripMenuItem showAverageTemperaturesMenuItem;
        public System.Windows.Forms.ToolStripMenuItem showHeaterPowerMenuItem;
        public System.Windows.Forms.ToolStripMenuItem autoscrollTemperatureViewMenuItem;
        public System.Windows.Forms.ToolStripMenuItem timeperiodMenuItem;
        public System.Windows.Forms.ToolStripMenuItem temperatureZoomMenuItem;
        public System.Windows.Forms.ToolStripMenuItem buildAverageOverMenuItem;
        public System.Windows.Forms.ToolStripMenuItem continuousMonitoringMenuItem;
        private System.Windows.Forms.ToolStripMenuItem soundConfigurationToolStripMenuItem;
        public System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem sendScript1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendScript2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendScript3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendScript4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendScript5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem supportToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSplitButton viewsStripSplitButton6;
        private System.Windows.Forms.ToolStripMenuItem rotateStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem moveStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem zoomStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem perspectiveStripMenuItem14;
        private System.Windows.Forms.ToolStripMenuItem resetStripMenuItem15;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ToolStripSplitButton helpSplitButton3;
        private System.Windows.Forms.ToolStripMenuItem baoyanAutomationWebsiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userManualToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton topViewStripButton1;
        private System.Windows.Forms.ToolStripButton sideViewStripButton1;
        private System.Windows.Forms.ToolStripButton frontStripButton1;
        private System.Windows.Forms.ToolStripMenuItem aboutSoftwareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSplitButton advancedConfigStripSplitButton3;
        private System.Windows.Forms.ToolStripMenuItem loggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualControlToolStripMenuItem;
        private System.Windows.Forms.ErrorProvider errorProvider;
        public System.Windows.Forms.ListBox listSTLObjects;
        public System.Windows.Forms.OpenFileDialog openFileSTLorGcode;
        public System.Windows.Forms.ToolStripSplitButton printStripSplitButton4;
        public System.Windows.Forms.ToolStripMenuItem sDCardToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        public System.Windows.Forms.ToolStripSplitButton importSTLToolSplitButton1;
        public System.Windows.Forms.ToolStripMenuItem loadGCodeToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem showWorkdirectoryToolStripMenuItem;
        public System.Windows.Forms.ToolStripSeparator toolStripEndHistory;
        public System.Windows.Forms.ToolStripSeparator toolStripStartHistory;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.ToolStripSplitButton sliceToolSplitButton3;
        public System.Windows.Forms.ToolStripButton emergencyStopStripButton6;
        public System.Windows.Forms.ToolStripSplitButton connectToolStripSplitButton;
        public System.Windows.Forms.ToolStripMenuItem killJobToolStripMenuItem;
        public System.Windows.Forms.ToolStripButton positionToolSplitButton2;
        public System.Windows.Forms.ToolStripMenuItem saveGCodeToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem saveNewSTLMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem slicerToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem slicerSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gcodeEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopSlicingProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton modeToolStripSplitButton1;
        public System.Windows.Forms.ToolStripMenuItem loadAFileMenuModeMenuOption;
        public System.Windows.Forms.ToolStripMenuItem STLEditorMenuOption;
        public System.Windows.Forms.ToolStripMenuItem gCodeVisualizationMenuOption;
        public System.Windows.Forms.ToolStripMenuItem livePrintingMenuOption;
        private System.Windows.Forms.ToolStripMenuItem slicerConfigurationToolStripMenuItem;
        public System.Windows.Forms.SaveFileDialog saveSTL;
        public System.Windows.Forms.ToolStripMenuItem withRaftToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem withSupportsToolStripMenuItem1;
        public System.Windows.Forms.ToolStripProgressBar toolProgress;
        private System.Windows.Forms.ToolStripMenuItem dViewSettingsToolStripMenuItem;
    }
}

