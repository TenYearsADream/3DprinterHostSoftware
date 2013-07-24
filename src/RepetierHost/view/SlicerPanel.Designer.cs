﻿namespace RepetierHost.view
{
    partial class SlicerPanel
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlicerPanel));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.generateRaftCheckbox = new System.Windows.Forms.CheckBox();
            this.generateSupportCheckbox = new System.Windows.Forms.CheckBox();
            this.buttonSetupSlic3r = new System.Windows.Forms.Button();
            this.buttonSlic3rConfigure = new System.Windows.Forms.Button();
            this.comboSlic3rPrinterSettings = new System.Windows.Forms.ComboBox();
            this.mainBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.comboSlic3rFilamentSettings3 = new System.Windows.Forms.ComboBox();
            this.comboSlic3rFilamentSettings2 = new System.Windows.Forms.ComboBox();
            this.comboSlic3rFilamentSettings = new System.Windows.Forms.ComboBox();
            this.labelSlic3rExtruder3 = new System.Windows.Forms.Label();
            this.comboSlic3rPrintSettings = new System.Windows.Forms.ComboBox();
            this.labelSlic3rExtruder2 = new System.Windows.Forms.Label();
            this.switchSlic3rActive = new RepetierHost.view.SwitchButton();
            this.labelSlic3rExtruder1 = new System.Windows.Forms.Label();
            this.labelPrinterSettings = new System.Windows.Forms.Label();
            this.labelFilamentSettings = new System.Windows.Forms.Label();
            this.labelPrintSettings = new System.Windows.Forms.Label();
            this.groupSkeinforge = new System.Windows.Forms.GroupBox();
            this.buttonSetupSkeinforge = new System.Windows.Forms.Button();
            this.buttonSkeinConfigure = new System.Windows.Forms.Button();
            this.comboSkeinProfile = new System.Windows.Forms.ComboBox();
            this.switchSkeinforge = new RepetierHost.view.SwitchButton();
            this.labelProfile = new System.Windows.Forms.Label();
            this.panelSlic3rPos = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainBindingSource)).BeginInit();
            this.groupSkeinforge.SuspendLayout();
            this.panelSlic3rPos.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.buttonSetupSlic3r);
            this.groupBox2.Controls.Add(this.buttonSlic3rConfigure);
            this.groupBox2.Controls.Add(this.comboSlic3rPrinterSettings);
            this.groupBox2.Controls.Add(this.comboSlic3rFilamentSettings3);
            this.groupBox2.Controls.Add(this.comboSlic3rFilamentSettings2);
            this.groupBox2.Controls.Add(this.comboSlic3rFilamentSettings);
            this.groupBox2.Controls.Add(this.labelSlic3rExtruder3);
            this.groupBox2.Controls.Add(this.comboSlic3rPrintSettings);
            this.groupBox2.Controls.Add(this.labelSlic3rExtruder2);
            this.groupBox2.Controls.Add(this.switchSlic3rActive);
            this.groupBox2.Controls.Add(this.labelSlic3rExtruder1);
            this.groupBox2.Controls.Add(this.labelPrinterSettings);
            this.groupBox2.Controls.Add(this.labelFilamentSettings);
            this.groupBox2.Controls.Add(this.labelPrintSettings);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(604, 219);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Slic3r";
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(10, 317);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(585, 36);
            this.label3.TabIndex = 6;
            this.label3.Text = "The support and raft checkboxes will over ride specific slic3r configurations. Ad" +
    "vanced users should uncheck the boxes and use their own custom slic3r settings. " +
    "";
            // 
            // generateRaftCheckbox
            // 
            this.generateRaftCheckbox.AutoSize = true;
            this.generateRaftCheckbox.Location = new System.Drawing.Point(132, 356);
            this.generateRaftCheckbox.Name = "generateRaftCheckbox";
            this.generateRaftCheckbox.Size = new System.Drawing.Size(93, 17);
            this.generateRaftCheckbox.TabIndex = 5;
            this.generateRaftCheckbox.Text = "Generate Raft";
            this.generateRaftCheckbox.UseVisualStyleBackColor = true;
            this.generateRaftCheckbox.CheckedChanged += new System.EventHandler(this.generateRaftCheckbox_CheckedChanged);
            // 
            // generateSupportCheckbox
            // 
            this.generateSupportCheckbox.AutoSize = true;
            this.generateSupportCheckbox.Location = new System.Drawing.Point(16, 356);
            this.generateSupportCheckbox.Name = "generateSupportCheckbox";
            this.generateSupportCheckbox.Size = new System.Drawing.Size(110, 17);
            this.generateSupportCheckbox.TabIndex = 4;
            this.generateSupportCheckbox.Text = "Generate Support";
            this.generateSupportCheckbox.UseVisualStyleBackColor = true;
            this.generateSupportCheckbox.CheckedChanged += new System.EventHandler(this.generateSupportCheckbox_CheckedChanged);
            // 
            // buttonSetupSlic3r
            // 
            this.buttonSetupSlic3r.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSetupSlic3r.Image = ((System.Drawing.Image)(resources.GetObject("buttonSetupSlic3r.Image")));
            this.buttonSetupSlic3r.Location = new System.Drawing.Point(481, 179);
            this.buttonSetupSlic3r.Name = "buttonSetupSlic3r";
            this.buttonSetupSlic3r.Size = new System.Drawing.Size(111, 23);
            this.buttonSetupSlic3r.TabIndex = 3;
            this.buttonSetupSlic3r.Text = "Setup";
            this.buttonSetupSlic3r.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSetupSlic3r.UseVisualStyleBackColor = true;
            this.buttonSetupSlic3r.Click += new System.EventHandler(this.buttonSetupSlic3r_Click);
            // 
            // buttonSlic3rConfigure
            // 
            this.buttonSlic3rConfigure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSlic3rConfigure.Image = ((System.Drawing.Image)(resources.GetObject("buttonSlic3rConfigure.Image")));
            this.buttonSlic3rConfigure.Location = new System.Drawing.Point(481, 20);
            this.buttonSlic3rConfigure.Name = "buttonSlic3rConfigure";
            this.buttonSlic3rConfigure.Size = new System.Drawing.Size(111, 23);
            this.buttonSlic3rConfigure.TabIndex = 2;
            this.buttonSlic3rConfigure.Text = "Configure";
            this.buttonSlic3rConfigure.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSlic3rConfigure.UseVisualStyleBackColor = true;
            this.buttonSlic3rConfigure.Click += new System.EventHandler(this.buttonSlic3rConfigure_Click);
            // 
            // comboSlic3rPrinterSettings
            // 
            this.comboSlic3rPrinterSettings.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.mainBindingSource, "Slic3rPrinter", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboSlic3rPrinterSettings.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mainBindingSource, "Slic3rPrinter", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboSlic3rPrinterSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSlic3rPrinterSettings.FormattingEnabled = true;
            this.comboSlic3rPrinterSettings.Location = new System.Drawing.Point(126, 73);
            this.comboSlic3rPrinterSettings.Name = "comboSlic3rPrinterSettings";
            this.comboSlic3rPrinterSettings.Size = new System.Drawing.Size(163, 21);
            this.comboSlic3rPrinterSettings.TabIndex = 1;
            this.comboSlic3rPrinterSettings.SelectedIndexChanged += new System.EventHandler(this.comboSlic3rPrinterSettings_SelectedIndexChanged);
            // 
            // mainBindingSource
            // 
            this.mainBindingSource.DataSource = typeof(RepetierHost.model.PrinterModel);
            // 
            // comboSlic3rFilamentSettings3
            // 
            this.comboSlic3rFilamentSettings3.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.mainBindingSource, "Slic3rFilament3", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboSlic3rFilamentSettings3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mainBindingSource, "Slic3rFilament3", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboSlic3rFilamentSettings3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSlic3rFilamentSettings3.FormattingEnabled = true;
            this.comboSlic3rFilamentSettings3.Location = new System.Drawing.Point(126, 179);
            this.comboSlic3rFilamentSettings3.Name = "comboSlic3rFilamentSettings3";
            this.comboSlic3rFilamentSettings3.Size = new System.Drawing.Size(163, 21);
            this.comboSlic3rFilamentSettings3.TabIndex = 1;
            this.comboSlic3rFilamentSettings3.SelectedIndexChanged += new System.EventHandler(this.comboSlic3rFilamentSettings3_SelectedIndexChanged);
            // 
            // comboSlic3rFilamentSettings2
            // 
            this.comboSlic3rFilamentSettings2.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.mainBindingSource, "Slic3rFilament2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboSlic3rFilamentSettings2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mainBindingSource, "Slic3rFilament2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboSlic3rFilamentSettings2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSlic3rFilamentSettings2.FormattingEnabled = true;
            this.comboSlic3rFilamentSettings2.Location = new System.Drawing.Point(126, 152);
            this.comboSlic3rFilamentSettings2.Name = "comboSlic3rFilamentSettings2";
            this.comboSlic3rFilamentSettings2.Size = new System.Drawing.Size(163, 21);
            this.comboSlic3rFilamentSettings2.TabIndex = 1;
            this.comboSlic3rFilamentSettings2.SelectedIndexChanged += new System.EventHandler(this.comboSlic3rFilamentSettings2_SelectedIndexChanged);
            // 
            // comboSlic3rFilamentSettings
            // 
            this.comboSlic3rFilamentSettings.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.mainBindingSource, "Slic3rFilament1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboSlic3rFilamentSettings.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mainBindingSource, "Slic3rFilament1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboSlic3rFilamentSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSlic3rFilamentSettings.FormattingEnabled = true;
            this.comboSlic3rFilamentSettings.Location = new System.Drawing.Point(126, 127);
            this.comboSlic3rFilamentSettings.Name = "comboSlic3rFilamentSettings";
            this.comboSlic3rFilamentSettings.Size = new System.Drawing.Size(163, 21);
            this.comboSlic3rFilamentSettings.TabIndex = 1;
            this.comboSlic3rFilamentSettings.SelectedIndexChanged += new System.EventHandler(this.comboSlic3rFilamentSettings_SelectedIndexChanged);
            // 
            // labelSlic3rExtruder3
            // 
            this.labelSlic3rExtruder3.AutoSize = true;
            this.labelSlic3rExtruder3.Location = new System.Drawing.Point(10, 182);
            this.labelSlic3rExtruder3.Name = "labelSlic3rExtruder3";
            this.labelSlic3rExtruder3.Size = new System.Drawing.Size(58, 13);
            this.labelSlic3rExtruder3.TabIndex = 0;
            this.labelSlic3rExtruder3.Text = "Extruder 3:";
            // 
            // comboSlic3rPrintSettings
            // 
            this.comboSlic3rPrintSettings.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.mainBindingSource, "Slic3rPrint", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboSlic3rPrintSettings.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mainBindingSource, "Slic3rPrint", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboSlic3rPrintSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSlic3rPrintSettings.FormattingEnabled = true;
            this.comboSlic3rPrintSettings.Location = new System.Drawing.Point(126, 46);
            this.comboSlic3rPrintSettings.Name = "comboSlic3rPrintSettings";
            this.comboSlic3rPrintSettings.Size = new System.Drawing.Size(163, 21);
            this.comboSlic3rPrintSettings.TabIndex = 1;
            this.comboSlic3rPrintSettings.SelectedIndexChanged += new System.EventHandler(this.comboSlic3rPrintSettings_SelectedIndexChanged);
            // 
            // labelSlic3rExtruder2
            // 
            this.labelSlic3rExtruder2.AutoSize = true;
            this.labelSlic3rExtruder2.Location = new System.Drawing.Point(10, 155);
            this.labelSlic3rExtruder2.Name = "labelSlic3rExtruder2";
            this.labelSlic3rExtruder2.Size = new System.Drawing.Size(58, 13);
            this.labelSlic3rExtruder2.TabIndex = 0;
            this.labelSlic3rExtruder2.Text = "Extruder 2:";
            // 
            // switchSlic3rActive
            // 
            this.switchSlic3rActive.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.switchSlic3rActive.ButtonFlatBorderColor = System.Drawing.Color.Empty;
            this.switchSlic3rActive.ButtonFlatBorderSize = 1;
            this.switchSlic3rActive.ButtonFlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.switchSlic3rActive.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.switchSlic3rActive.Location = new System.Drawing.Point(10, 20);
            this.switchSlic3rActive.MinimumSize = new System.Drawing.Size(90, 23);
            this.switchSlic3rActive.Name = "switchSlic3rActive";
            this.switchSlic3rActive.On = false;
            this.switchSlic3rActive.Size = new System.Drawing.Size(100, 23);
            this.switchSlic3rActive.TabIndex = 0;
            this.switchSlic3rActive.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.switchSlic3rActive.TextOff = "Active";
            this.switchSlic3rActive.TextOn = "Active";
            this.switchSlic3rActive.OnChange += new RepetierHost.view.SwitchEventHandler(this.switchSlic3rActive_OnChange);
            this.switchSlic3rActive.Load += new System.EventHandler(this.switchSlic3rActive_Load);
            this.switchSlic3rActive.Click += new System.EventHandler(this.switchSlic3rActive_Click);
            // 
            // labelSlic3rExtruder1
            // 
            this.labelSlic3rExtruder1.AutoSize = true;
            this.labelSlic3rExtruder1.Location = new System.Drawing.Point(10, 130);
            this.labelSlic3rExtruder1.Name = "labelSlic3rExtruder1";
            this.labelSlic3rExtruder1.Size = new System.Drawing.Size(58, 13);
            this.labelSlic3rExtruder1.TabIndex = 0;
            this.labelSlic3rExtruder1.Text = "Extruder 1:";
            // 
            // labelPrinterSettings
            // 
            this.labelPrinterSettings.AutoSize = true;
            this.labelPrinterSettings.Location = new System.Drawing.Point(10, 76);
            this.labelPrinterSettings.Name = "labelPrinterSettings";
            this.labelPrinterSettings.Size = new System.Drawing.Size(79, 13);
            this.labelPrinterSettings.TabIndex = 0;
            this.labelPrinterSettings.Text = "Printer settings:";
            // 
            // labelFilamentSettings
            // 
            this.labelFilamentSettings.AutoSize = true;
            this.labelFilamentSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFilamentSettings.Location = new System.Drawing.Point(10, 103);
            this.labelFilamentSettings.Name = "labelFilamentSettings";
            this.labelFilamentSettings.Size = new System.Drawing.Size(106, 13);
            this.labelFilamentSettings.TabIndex = 0;
            this.labelFilamentSettings.Text = "Filament settings:";
            // 
            // labelPrintSettings
            // 
            this.labelPrintSettings.AutoSize = true;
            this.labelPrintSettings.Location = new System.Drawing.Point(10, 49);
            this.labelPrintSettings.Name = "labelPrintSettings";
            this.labelPrintSettings.Size = new System.Drawing.Size(70, 13);
            this.labelPrintSettings.TabIndex = 0;
            this.labelPrintSettings.Text = "Print settings:";
            // 
            // groupSkeinforge
            // 
            this.groupSkeinforge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSkeinforge.Controls.Add(this.buttonSetupSkeinforge);
            this.groupSkeinforge.Controls.Add(this.buttonSkeinConfigure);
            this.groupSkeinforge.Controls.Add(this.comboSkeinProfile);
            this.groupSkeinforge.Controls.Add(this.switchSkeinforge);
            this.groupSkeinforge.Controls.Add(this.labelProfile);
            this.groupSkeinforge.Location = new System.Drawing.Point(3, 228);
            this.groupSkeinforge.Name = "groupSkeinforge";
            this.groupSkeinforge.Size = new System.Drawing.Size(604, 86);
            this.groupSkeinforge.TabIndex = 2;
            this.groupSkeinforge.TabStop = false;
            this.groupSkeinforge.Text = "Skeinforge";
            // 
            // buttonSetupSkeinforge
            // 
            this.buttonSetupSkeinforge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSetupSkeinforge.Image = ((System.Drawing.Image)(resources.GetObject("buttonSetupSkeinforge.Image")));
            this.buttonSetupSkeinforge.Location = new System.Drawing.Point(481, 48);
            this.buttonSetupSkeinforge.Name = "buttonSetupSkeinforge";
            this.buttonSetupSkeinforge.Size = new System.Drawing.Size(111, 23);
            this.buttonSetupSkeinforge.TabIndex = 3;
            this.buttonSetupSkeinforge.Text = "Setup";
            this.buttonSetupSkeinforge.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSetupSkeinforge.UseVisualStyleBackColor = true;
            this.buttonSetupSkeinforge.Click += new System.EventHandler(this.buttonSetupSkeinforge_Click);
            // 
            // buttonSkeinConfigure
            // 
            this.buttonSkeinConfigure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSkeinConfigure.Image = ((System.Drawing.Image)(resources.GetObject("buttonSkeinConfigure.Image")));
            this.buttonSkeinConfigure.Location = new System.Drawing.Point(481, 19);
            this.buttonSkeinConfigure.Name = "buttonSkeinConfigure";
            this.buttonSkeinConfigure.Size = new System.Drawing.Size(111, 23);
            this.buttonSkeinConfigure.TabIndex = 2;
            this.buttonSkeinConfigure.Text = "Configure";
            this.buttonSkeinConfigure.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSkeinConfigure.UseVisualStyleBackColor = true;
            this.buttonSkeinConfigure.Click += new System.EventHandler(this.buttonSkeinConfigure_Click);
            // 
            // comboSkeinProfile
            // 
            this.comboSkeinProfile.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.mainBindingSource, "SkeinforgeProfile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboSkeinProfile.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mainBindingSource, "SkeinforgeProfile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboSkeinProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSkeinProfile.FormattingEnabled = true;
            this.comboSkeinProfile.Location = new System.Drawing.Point(126, 50);
            this.comboSkeinProfile.Name = "comboSkeinProfile";
            this.comboSkeinProfile.Size = new System.Drawing.Size(163, 21);
            this.comboSkeinProfile.TabIndex = 1;
            this.comboSkeinProfile.SelectedIndexChanged += new System.EventHandler(this.comboSkeinProfile_SelectedIndexChanged);
            // 
            // switchSkeinforge
            // 
            this.switchSkeinforge.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.switchSkeinforge.ButtonFlatBorderColor = System.Drawing.Color.Empty;
            this.switchSkeinforge.ButtonFlatBorderSize = 1;
            this.switchSkeinforge.ButtonFlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.switchSkeinforge.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.switchSkeinforge.Location = new System.Drawing.Point(6, 19);
            this.switchSkeinforge.MinimumSize = new System.Drawing.Size(90, 23);
            this.switchSkeinforge.Name = "switchSkeinforge";
            this.switchSkeinforge.On = false;
            this.switchSkeinforge.Size = new System.Drawing.Size(100, 23);
            this.switchSkeinforge.TabIndex = 0;
            this.switchSkeinforge.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.switchSkeinforge.TextOff = "Active";
            this.switchSkeinforge.TextOn = "Active";
            this.switchSkeinforge.OnChange += new RepetierHost.view.SwitchEventHandler(this.switchSkeinforge_OnChange);
            this.switchSkeinforge.Load += new System.EventHandler(this.switchSkeinforge_Load);
            this.switchSkeinforge.Click += new System.EventHandler(this.switchSkeinforge_Click);
            // 
            // labelProfile
            // 
            this.labelProfile.AutoSize = true;
            this.labelProfile.Location = new System.Drawing.Point(10, 53);
            this.labelProfile.Name = "labelProfile";
            this.labelProfile.Size = new System.Drawing.Size(39, 13);
            this.labelProfile.TabIndex = 0;
            this.labelProfile.Text = "Profile:";
            // 
            // panelSlic3rPos
            // 
            this.panelSlic3rPos.Controls.Add(this.label1);
            this.panelSlic3rPos.Controls.Add(this.generateRaftCheckbox);
            this.panelSlic3rPos.Controls.Add(this.button1);
            this.panelSlic3rPos.Controls.Add(this.groupSkeinforge);
            this.panelSlic3rPos.Controls.Add(this.generateSupportCheckbox);
            this.panelSlic3rPos.Controls.Add(this.label3);
            this.panelSlic3rPos.Controls.Add(this.groupBox2);
            this.panelSlic3rPos.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSlic3rPos.Location = new System.Drawing.Point(0, 0);
            this.panelSlic3rPos.Name = "panelSlic3rPos";
            this.panelSlic3rPos.Size = new System.Drawing.Size(610, 477);
            this.panelSlic3rPos.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(265, 412);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 34);
            this.button1.TabIndex = 7;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(262, 396);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Changes are Automatically Saved";
            // 
            // SlicerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelSlic3rPos);
            this.Name = "SlicerPanel";
            this.Size = new System.Drawing.Size(610, 480);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainBindingSource)).EndInit();
            this.groupSkeinforge.ResumeLayout(false);
            this.groupSkeinforge.PerformLayout();
            this.panelSlic3rPos.ResumeLayout(false);
            this.panelSlic3rPos.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupSkeinforge;
        private SwitchButton switchSlic3rActive;
        private System.Windows.Forms.ComboBox comboSlic3rPrinterSettings;
        private System.Windows.Forms.ComboBox comboSlic3rFilamentSettings;
        private System.Windows.Forms.ComboBox comboSlic3rPrintSettings;
        private System.Windows.Forms.Label labelPrinterSettings;
        private System.Windows.Forms.Label labelFilamentSettings;
        private System.Windows.Forms.Label labelPrintSettings;
        private System.Windows.Forms.Button buttonSlic3rConfigure;
        private System.Windows.Forms.Button buttonSkeinConfigure;
        private SwitchButton switchSkeinforge;
        private System.Windows.Forms.Label labelProfile;
        public System.Windows.Forms.ComboBox comboSkeinProfile;
        private System.Windows.Forms.Button buttonSetupSlic3r;
        private System.Windows.Forms.Button buttonSetupSkeinforge;
        private System.Windows.Forms.ComboBox comboSlic3rFilamentSettings3;
        private System.Windows.Forms.ComboBox comboSlic3rFilamentSettings2;
        private System.Windows.Forms.Label labelSlic3rExtruder3;
        private System.Windows.Forms.Label labelSlic3rExtruder2;
        private System.Windows.Forms.Label labelSlic3rExtruder1;
        private System.Windows.Forms.Panel panelSlic3rPos;
        private System.Windows.Forms.BindingSource mainBindingSource;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.CheckBox generateRaftCheckbox;
        public System.Windows.Forms.CheckBox generateSupportCheckbox;
    }
}
