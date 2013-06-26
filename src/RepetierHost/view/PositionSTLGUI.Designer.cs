namespace RepetierHost.view
{
    partial class PositionSTLGUI
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
            this.checkScaleAll = new System.Windows.Forms.CheckBox();
            this.textRotZ = new System.Windows.Forms.TextBox();
            this.textScaleZ = new System.Windows.Forms.TextBox();
            this.textTransZ = new System.Windows.Forms.TextBox();
            this.textRotY = new System.Windows.Forms.TextBox();
            this.textRotX = new System.Windows.Forms.TextBox();
            this.textScaleY = new System.Windows.Forms.TextBox();
            this.textScaleX = new System.Windows.Forms.TextBox();
            this.textTransY = new System.Windows.Forms.TextBox();
            this.textTransX = new System.Windows.Forms.TextBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.openFileSTL = new System.Windows.Forms.OpenFileDialog();
            this.saveSTL = new System.Windows.Forms.SaveFileDialog();
            this.PositionTabControl = new System.Windows.Forms.TabControl();
            this.movetabPage1 = new System.Windows.Forms.TabPage();
            this.buttonAutoplace = new System.Windows.Forms.Button();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.zAxisSliderControl = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.yAxisMoveSliderControl = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.xAxisMoveSliderControl = new System.Windows.Forms.TrackBar();
            this.rotateTabPage2 = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.zAxisRotationSliderControl = new System.Windows.Forms.TrackBar();
            this.label14 = new System.Windows.Forms.Label();
            this.yAxisRotateSliderControl = new System.Windows.Forms.TrackBar();
            this.label15 = new System.Windows.Forms.Label();
            this.xAxisRotateSliderControl = new System.Windows.Forms.TrackBar();
            this.scaleTabPage3 = new System.Windows.Forms.TabPage();
            this.label16 = new System.Windows.Forms.Label();
            this.zAxisScaleSliderControl = new System.Windows.Forms.TrackBar();
            this.label17 = new System.Windows.Forms.Label();
            this.yAxisScaleSliderControl = new System.Windows.Forms.TrackBar();
            this.label18 = new System.Windows.Forms.Label();
            this.xAxisScaleSliderControl = new System.Windows.Forms.TrackBar();
            this.copyModelTab = new System.Windows.Forms.TabPage();
            this.buttonCopyObjects = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.PositionTabControl.SuspendLayout();
            this.movetabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zAxisSliderControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yAxisMoveSliderControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xAxisMoveSliderControl)).BeginInit();
            this.rotateTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zAxisRotationSliderControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yAxisRotateSliderControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xAxisRotateSliderControl)).BeginInit();
            this.scaleTabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zAxisScaleSliderControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yAxisScaleSliderControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xAxisScaleSliderControl)).BeginInit();
            this.copyModelTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkScaleAll
            // 
            this.checkScaleAll.AutoSize = true;
            this.checkScaleAll.Checked = true;
            this.checkScaleAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkScaleAll.Location = new System.Drawing.Point(73, 248);
            this.checkScaleAll.Name = "checkScaleAll";
            this.checkScaleAll.Size = new System.Drawing.Size(108, 17);
            this.checkScaleAll.TabIndex = 8;
            this.checkScaleAll.Text = "Lock aspect ratio";
            this.checkScaleAll.UseVisualStyleBackColor = true;
            this.checkScaleAll.CheckedChanged += new System.EventHandler(this.checkScaleAll_CheckedChanged);
            // 
            // textRotZ
            // 
            this.textRotZ.Location = new System.Drawing.Point(189, 22);
            this.textRotZ.Name = "textRotZ";
            this.textRotZ.Size = new System.Drawing.Size(49, 20);
            this.textRotZ.TabIndex = 11;
            this.textRotZ.TextChanged += new System.EventHandler(this.textRotZ_TextChanged);
            this.textRotZ.Validating += new System.ComponentModel.CancelEventHandler(this.float_Validating);
            // 
            // textScaleZ
            // 
            this.textScaleZ.Location = new System.Drawing.Point(189, 21);
            this.textScaleZ.Name = "textScaleZ";
            this.textScaleZ.Size = new System.Drawing.Size(49, 20);
            this.textScaleZ.TabIndex = 7;
            this.textScaleZ.TextChanged += new System.EventHandler(this.textScaleZ_TextChanged);
            this.textScaleZ.Validating += new System.ComponentModel.CancelEventHandler(this.float_Validating);
            // 
            // textTransZ
            // 
            this.textTransZ.Location = new System.Drawing.Point(189, 22);
            this.textTransZ.Name = "textTransZ";
            this.textTransZ.Size = new System.Drawing.Size(49, 20);
            this.textTransZ.TabIndex = 4;
            this.textTransZ.TextChanged += new System.EventHandler(this.textTransZ_TextChanged);
            this.textTransZ.Validating += new System.ComponentModel.CancelEventHandler(this.float_Validating);
            // 
            // textRotY
            // 
            this.textRotY.Location = new System.Drawing.Point(115, 22);
            this.textRotY.Name = "textRotY";
            this.textRotY.Size = new System.Drawing.Size(49, 20);
            this.textRotY.TabIndex = 10;
            this.textRotY.TextChanged += new System.EventHandler(this.textRotY_TextChanged);
            this.textRotY.Validating += new System.ComponentModel.CancelEventHandler(this.float_Validating);
            // 
            // textRotX
            // 
            this.textRotX.Location = new System.Drawing.Point(41, 21);
            this.textRotX.Name = "textRotX";
            this.textRotX.Size = new System.Drawing.Size(49, 20);
            this.textRotX.TabIndex = 9;
            this.textRotX.TextChanged += new System.EventHandler(this.textRotX_TextChanged);
            this.textRotX.Validating += new System.ComponentModel.CancelEventHandler(this.float_Validating);
            // 
            // textScaleY
            // 
            this.textScaleY.Location = new System.Drawing.Point(114, 22);
            this.textScaleY.Name = "textScaleY";
            this.textScaleY.Size = new System.Drawing.Size(49, 20);
            this.textScaleY.TabIndex = 6;
            this.textScaleY.TextChanged += new System.EventHandler(this.textScaleY_TextChanged);
            this.textScaleY.Validating += new System.ComponentModel.CancelEventHandler(this.float_Validating);
            // 
            // textScaleX
            // 
            this.textScaleX.Location = new System.Drawing.Point(41, 21);
            this.textScaleX.Name = "textScaleX";
            this.textScaleX.Size = new System.Drawing.Size(49, 20);
            this.textScaleX.TabIndex = 5;
            this.textScaleX.TextChanged += new System.EventHandler(this.textScaleX_TextChanged);
            this.textScaleX.Validating += new System.ComponentModel.CancelEventHandler(this.float_Validating);
            // 
            // textTransY
            // 
            this.textTransY.Location = new System.Drawing.Point(115, 22);
            this.textTransY.Name = "textTransY";
            this.textTransY.Size = new System.Drawing.Size(49, 20);
            this.textTransY.TabIndex = 3;
            this.textTransY.TextChanged += new System.EventHandler(this.textTransY_TextChanged);
            this.textTransY.Validating += new System.ComponentModel.CancelEventHandler(this.float_Validating);
            // 
            // textTransX
            // 
            this.textTransX.Location = new System.Drawing.Point(36, 22);
            this.textTransX.Name = "textTransX";
            this.textTransX.Size = new System.Drawing.Size(49, 20);
            this.textTransX.TabIndex = 2;
            this.textTransX.TextChanged += new System.EventHandler(this.textTransX_TextChanged);
            this.textTransX.Validating += new System.ComponentModel.CancelEventHandler(this.float_Validating);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // openFileSTL
            // 
            this.openFileSTL.DefaultExt = "stl";
            this.openFileSTL.Filter = "STL-Files|*.stl;*.STL|All files|*.*";
            this.openFileSTL.Multiselect = true;
            this.openFileSTL.Title = "Add STL file";
            // 
            // saveSTL
            // 
            this.saveSTL.DefaultExt = "stl";
            this.saveSTL.Filter = "STL-Files|*.stl;*.STL";
            this.saveSTL.Title = "Save composition";
            // 
            // PositionTabControl
            // 
            this.PositionTabControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.PositionTabControl.Controls.Add(this.movetabPage1);
            this.PositionTabControl.Controls.Add(this.rotateTabPage2);
            this.PositionTabControl.Controls.Add(this.scaleTabPage3);
            this.PositionTabControl.Controls.Add(this.copyModelTab);
            this.PositionTabControl.Location = new System.Drawing.Point(3, 3);
            this.PositionTabControl.Multiline = true;
            this.PositionTabControl.Name = "PositionTabControl";
            this.PositionTabControl.SelectedIndex = 0;
            this.PositionTabControl.Size = new System.Drawing.Size(280, 323);
            this.PositionTabControl.TabIndex = 19;
            // 
            // movetabPage1
            // 
            this.movetabPage1.Controls.Add(this.buttonAutoplace);
            this.movetabPage1.Controls.Add(this.buttonCenter);
            this.movetabPage1.Controls.Add(this.label1);
            this.movetabPage1.Controls.Add(this.zAxisSliderControl);
            this.movetabPage1.Controls.Add(this.label5);
            this.movetabPage1.Controls.Add(this.textTransZ);
            this.movetabPage1.Controls.Add(this.yAxisMoveSliderControl);
            this.movetabPage1.Controls.Add(this.textTransY);
            this.movetabPage1.Controls.Add(this.label9);
            this.movetabPage1.Controls.Add(this.xAxisMoveSliderControl);
            this.movetabPage1.Controls.Add(this.textTransX);
            this.movetabPage1.Location = new System.Drawing.Point(4, 25);
            this.movetabPage1.Name = "movetabPage1";
            this.movetabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.movetabPage1.Size = new System.Drawing.Size(272, 294);
            this.movetabPage1.TabIndex = 0;
            this.movetabPage1.Text = "Move";
            this.movetabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonAutoplace
            // 
            this.buttonAutoplace.Location = new System.Drawing.Point(144, 249);
            this.buttonAutoplace.Name = "buttonAutoplace";
            this.buttonAutoplace.Size = new System.Drawing.Size(109, 33);
            this.buttonAutoplace.TabIndex = 20;
            this.buttonAutoplace.Text = "Auto Place";
            this.buttonAutoplace.UseVisualStyleBackColor = true;
            // 
            // buttonCenter
            // 
            this.buttonCenter.Location = new System.Drawing.Point(6, 251);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(132, 34);
            this.buttonCenter.TabIndex = 20;
            this.buttonCenter.Text = "Center";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(186, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Z-Axis";
            // 
            // zAxisSliderControl
            // 
            this.zAxisSliderControl.Location = new System.Drawing.Point(189, 48);
            this.zAxisSliderControl.Minimum = -10;
            this.zAxisSliderControl.Name = "zAxisSliderControl";
            this.zAxisSliderControl.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.zAxisSliderControl.Size = new System.Drawing.Size(45, 195);
            this.zAxisSliderControl.TabIndex = 6;
            this.zAxisSliderControl.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(112, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Y-Axis";
            // 
            // yAxisMoveSliderControl
            // 
            this.yAxisMoveSliderControl.Location = new System.Drawing.Point(115, 45);
            this.yAxisMoveSliderControl.Minimum = -10;
            this.yAxisMoveSliderControl.Name = "yAxisMoveSliderControl";
            this.yAxisMoveSliderControl.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.yAxisMoveSliderControl.Size = new System.Drawing.Size(45, 195);
            this.yAxisMoveSliderControl.TabIndex = 3;
            this.yAxisMoveSliderControl.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(38, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "X-Axis";
            // 
            // xAxisMoveSliderControl
            // 
            this.xAxisMoveSliderControl.Location = new System.Drawing.Point(36, 48);
            this.xAxisMoveSliderControl.Minimum = -10;
            this.xAxisMoveSliderControl.Name = "xAxisMoveSliderControl";
            this.xAxisMoveSliderControl.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.xAxisMoveSliderControl.Size = new System.Drawing.Size(45, 195);
            this.xAxisMoveSliderControl.TabIndex = 0;
            this.xAxisMoveSliderControl.Visible = false;
            // 
            // rotateTabPage2
            // 
            this.rotateTabPage2.Controls.Add(this.label13);
            this.rotateTabPage2.Controls.Add(this.zAxisRotationSliderControl);
            this.rotateTabPage2.Controls.Add(this.label14);
            this.rotateTabPage2.Controls.Add(this.yAxisRotateSliderControl);
            this.rotateTabPage2.Controls.Add(this.label15);
            this.rotateTabPage2.Controls.Add(this.xAxisRotateSliderControl);
            this.rotateTabPage2.Controls.Add(this.textRotX);
            this.rotateTabPage2.Controls.Add(this.textRotY);
            this.rotateTabPage2.Controls.Add(this.textRotZ);
            this.rotateTabPage2.Location = new System.Drawing.Point(4, 25);
            this.rotateTabPage2.Name = "rotateTabPage2";
            this.rotateTabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.rotateTabPage2.Size = new System.Drawing.Size(272, 294);
            this.rotateTabPage2.TabIndex = 1;
            this.rotateTabPage2.Text = "Rotate";
            this.rotateTabPage2.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(186, 3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(36, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "Z-Axis";
            // 
            // zAxisRotationSliderControl
            // 
            this.zAxisRotationSliderControl.Location = new System.Drawing.Point(189, 47);
            this.zAxisRotationSliderControl.Maximum = 180;
            this.zAxisRotationSliderControl.Minimum = -180;
            this.zAxisRotationSliderControl.Name = "zAxisRotationSliderControl";
            this.zAxisRotationSliderControl.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.zAxisRotationSliderControl.Size = new System.Drawing.Size(45, 195);
            this.zAxisRotationSliderControl.TabIndex = 15;
            this.zAxisRotationSliderControl.Visible = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(112, 3);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(36, 13);
            this.label14.TabIndex = 13;
            this.label14.Text = "Y-Axis";
            // 
            // yAxisRotateSliderControl
            // 
            this.yAxisRotateSliderControl.Location = new System.Drawing.Point(115, 47);
            this.yAxisRotateSliderControl.Maximum = 180;
            this.yAxisRotateSliderControl.Minimum = -180;
            this.yAxisRotateSliderControl.Name = "yAxisRotateSliderControl";
            this.yAxisRotateSliderControl.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.yAxisRotateSliderControl.Size = new System.Drawing.Size(45, 195);
            this.yAxisRotateSliderControl.TabIndex = 12;
            this.yAxisRotateSliderControl.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(38, 3);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(36, 13);
            this.label15.TabIndex = 10;
            this.label15.Text = "X-Axis";
            // 
            // xAxisRotateSliderControl
            // 
            this.xAxisRotateSliderControl.Location = new System.Drawing.Point(41, 47);
            this.xAxisRotateSliderControl.Maximum = 180;
            this.xAxisRotateSliderControl.Minimum = -180;
            this.xAxisRotateSliderControl.Name = "xAxisRotateSliderControl";
            this.xAxisRotateSliderControl.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.xAxisRotateSliderControl.Size = new System.Drawing.Size(45, 195);
            this.xAxisRotateSliderControl.TabIndex = 9;
            this.xAxisRotateSliderControl.Visible = false;
            // 
            // scaleTabPage3
            // 
            this.scaleTabPage3.Controls.Add(this.label16);
            this.scaleTabPage3.Controls.Add(this.zAxisScaleSliderControl);
            this.scaleTabPage3.Controls.Add(this.checkScaleAll);
            this.scaleTabPage3.Controls.Add(this.label17);
            this.scaleTabPage3.Controls.Add(this.yAxisScaleSliderControl);
            this.scaleTabPage3.Controls.Add(this.label18);
            this.scaleTabPage3.Controls.Add(this.xAxisScaleSliderControl);
            this.scaleTabPage3.Controls.Add(this.textScaleX);
            this.scaleTabPage3.Controls.Add(this.textScaleY);
            this.scaleTabPage3.Controls.Add(this.textScaleZ);
            this.scaleTabPage3.Location = new System.Drawing.Point(4, 25);
            this.scaleTabPage3.Name = "scaleTabPage3";
            this.scaleTabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.scaleTabPage3.Size = new System.Drawing.Size(272, 294);
            this.scaleTabPage3.TabIndex = 2;
            this.scaleTabPage3.Text = "Scale";
            this.scaleTabPage3.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(186, 3);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(36, 13);
            this.label16.TabIndex = 16;
            this.label16.Text = "Z-Axis";
            // 
            // zAxisScaleSliderControl
            // 
            this.zAxisScaleSliderControl.Location = new System.Drawing.Point(189, 47);
            this.zAxisScaleSliderControl.Name = "zAxisScaleSliderControl";
            this.zAxisScaleSliderControl.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.zAxisScaleSliderControl.Size = new System.Drawing.Size(45, 195);
            this.zAxisScaleSliderControl.TabIndex = 15;
            this.zAxisScaleSliderControl.Visible = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(112, 3);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(36, 13);
            this.label17.TabIndex = 13;
            this.label17.Text = "Y-Axis";
            // 
            // yAxisScaleSliderControl
            // 
            this.yAxisScaleSliderControl.Location = new System.Drawing.Point(118, 47);
            this.yAxisScaleSliderControl.Name = "yAxisScaleSliderControl";
            this.yAxisScaleSliderControl.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.yAxisScaleSliderControl.Size = new System.Drawing.Size(45, 195);
            this.yAxisScaleSliderControl.TabIndex = 12;
            this.yAxisScaleSliderControl.Visible = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(38, 3);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(36, 13);
            this.label18.TabIndex = 10;
            this.label18.Text = "X-Axis";
            // 
            // xAxisScaleSliderControl
            // 
            this.xAxisScaleSliderControl.Location = new System.Drawing.Point(41, 47);
            this.xAxisScaleSliderControl.Name = "xAxisScaleSliderControl";
            this.xAxisScaleSliderControl.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.xAxisScaleSliderControl.Size = new System.Drawing.Size(45, 195);
            this.xAxisScaleSliderControl.TabIndex = 9;
            this.xAxisScaleSliderControl.Visible = false;
            this.xAxisScaleSliderControl.Scroll += new System.EventHandler(this.xAxisScaleSliderControl_Scroll);
            // 
            // copyModelTab
            // 
            this.copyModelTab.Controls.Add(this.buttonCopyObjects);
            this.copyModelTab.Location = new System.Drawing.Point(4, 25);
            this.copyModelTab.Name = "copyModelTab";
            this.copyModelTab.Padding = new System.Windows.Forms.Padding(3);
            this.copyModelTab.Size = new System.Drawing.Size(272, 294);
            this.copyModelTab.TabIndex = 3;
            this.copyModelTab.Text = "Copy";
            this.copyModelTab.UseVisualStyleBackColor = true;
            // 
            // buttonCopyObjects
            // 
            this.buttonCopyObjects.Location = new System.Drawing.Point(6, 23);
            this.buttonCopyObjects.Name = "buttonCopyObjects";
            this.buttonCopyObjects.Size = new System.Drawing.Size(97, 40);
            this.buttonCopyObjects.TabIndex = 0;
            this.buttonCopyObjects.Text = "Temp Copy";
            this.buttonCopyObjects.UseVisualStyleBackColor = true;
            // 
            // PositionSTLGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PositionTabControl);
            this.Name = "PositionSTLGUI";
            this.Size = new System.Drawing.Size(283, 326);
            this.Leave += new System.EventHandler(this.PositionSTLGUI_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.PositionTabControl.ResumeLayout(false);
            this.movetabPage1.ResumeLayout(false);
            this.movetabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zAxisSliderControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yAxisMoveSliderControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xAxisMoveSliderControl)).EndInit();
            this.rotateTabPage2.ResumeLayout(false);
            this.rotateTabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zAxisRotationSliderControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yAxisRotateSliderControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xAxisRotateSliderControl)).EndInit();
            this.scaleTabPage3.ResumeLayout(false);
            this.scaleTabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zAxisScaleSliderControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yAxisScaleSliderControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xAxisScaleSliderControl)).EndInit();
            this.copyModelTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textTransZ;
        private System.Windows.Forms.TextBox textTransY;
        private System.Windows.Forms.TextBox textTransX;
        private System.Windows.Forms.CheckBox checkScaleAll;
        private System.Windows.Forms.TextBox textRotZ;
        private System.Windows.Forms.TextBox textScaleZ;
        private System.Windows.Forms.TextBox textRotY;
        private System.Windows.Forms.TextBox textRotX;
        private System.Windows.Forms.TextBox textScaleY;
        private System.Windows.Forms.TextBox textScaleX;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.OpenFileDialog openFileSTL;
        private System.Windows.Forms.SaveFileDialog saveSTL;
        private System.Windows.Forms.TabControl PositionTabControl;
        private System.Windows.Forms.TabPage movetabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar zAxisSliderControl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar yAxisMoveSliderControl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TrackBar xAxisMoveSliderControl;
        private System.Windows.Forms.TabPage rotateTabPage2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TrackBar zAxisRotationSliderControl;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TrackBar yAxisRotateSliderControl;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TrackBar xAxisRotateSliderControl;
        private System.Windows.Forms.TabPage scaleTabPage3;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TrackBar zAxisScaleSliderControl;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TrackBar yAxisScaleSliderControl;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TrackBar xAxisScaleSliderControl;
        private System.Windows.Forms.TabPage copyModelTab;
        private System.Windows.Forms.Button buttonCenter;
        private System.Windows.Forms.Button buttonAutoplace;
        private System.Windows.Forms.Button buttonCopyObjects;
    }
}
