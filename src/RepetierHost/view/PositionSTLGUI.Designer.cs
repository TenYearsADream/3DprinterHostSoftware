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
            this.textScaleZ = new System.Windows.Forms.TextBox();
            this.textScaleY = new System.Windows.Forms.TextBox();
            this.textScaleX = new System.Windows.Forms.TextBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.openFileSTL = new System.Windows.Forms.OpenFileDialog();
            this.saveSTL = new System.Windows.Forms.SaveFileDialog();
            this.PositionTabControl = new System.Windows.Forms.TabControl();
            this.movetabPage1 = new System.Windows.Forms.TabPage();
            this.zTransNum = new System.Windows.Forms.NumericUpDown();
            this.yTranNum = new System.Windows.Forms.NumericUpDown();
            this.xTransValue = new System.Windows.Forms.NumericUpDown();
            this.buttonAutoplace = new System.Windows.Forms.Button();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.rotateTabPage2 = new System.Windows.Forms.TabPage();
            this.rotateMinus90Z = new System.Windows.Forms.Button();
            this.rotateMinus90Y = new System.Windows.Forms.Button();
            this.rotate90Z = new System.Windows.Forms.Button();
            this.rotate90Y = new System.Windows.Forms.Button();
            this.rotateMinus90X = new System.Windows.Forms.Button();
            this.rotate90X = new System.Windows.Forms.Button();
            this.zRotateControl = new System.Windows.Forms.NumericUpDown();
            this.yRotateControl = new System.Windows.Forms.NumericUpDown();
            this.xRotateControl = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.scaleTabPage3 = new System.Windows.Forms.TabPage();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.copyModelTab = new System.Windows.Forms.TabPage();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.checkAutoposition = new System.Windows.Forms.CheckBox();
            this.numericCopies = new System.Windows.Forms.NumericUpDown();
            this.labelNumberOfCopies = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.PositionTabControl.SuspendLayout();
            this.movetabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zTransNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yTranNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xTransValue)).BeginInit();
            this.rotateTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zRotateControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yRotateControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xRotateControl)).BeginInit();
            this.scaleTabPage3.SuspendLayout();
            this.copyModelTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericCopies)).BeginInit();
            this.SuspendLayout();
            // 
            // checkScaleAll
            // 
            this.checkScaleAll.AutoSize = true;
            this.checkScaleAll.Checked = true;
            this.checkScaleAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkScaleAll.Location = new System.Drawing.Point(82, 48);
            this.checkScaleAll.Name = "checkScaleAll";
            this.checkScaleAll.Size = new System.Drawing.Size(108, 17);
            this.checkScaleAll.TabIndex = 8;
            this.checkScaleAll.Text = "Lock aspect ratio";
            this.checkScaleAll.UseVisualStyleBackColor = true;
            this.checkScaleAll.CheckedChanged += new System.EventHandler(this.checkScaleAll_CheckedChanged);
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
            this.PositionTabControl.Location = new System.Drawing.Point(0, 0);
            this.PositionTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.PositionTabControl.Multiline = true;
            this.PositionTabControl.Name = "PositionTabControl";
            this.PositionTabControl.SelectedIndex = 0;
            this.PositionTabControl.Size = new System.Drawing.Size(280, 137);
            this.PositionTabControl.TabIndex = 19;
            this.PositionTabControl.MouseLeave += new System.EventHandler(this.PositionTabControl_MouseLeave);
            // 
            // movetabPage1
            // 
            this.movetabPage1.Controls.Add(this.zTransNum);
            this.movetabPage1.Controls.Add(this.yTranNum);
            this.movetabPage1.Controls.Add(this.xTransValue);
            this.movetabPage1.Controls.Add(this.buttonAutoplace);
            this.movetabPage1.Controls.Add(this.buttonCenter);
            this.movetabPage1.Controls.Add(this.label1);
            this.movetabPage1.Controls.Add(this.label5);
            this.movetabPage1.Controls.Add(this.label9);
            this.movetabPage1.Location = new System.Drawing.Point(4, 25);
            this.movetabPage1.Name = "movetabPage1";
            this.movetabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.movetabPage1.Size = new System.Drawing.Size(272, 108);
            this.movetabPage1.TabIndex = 0;
            this.movetabPage1.Text = "Move";
            this.movetabPage1.UseVisualStyleBackColor = true;
            // 
            // zTransNum
            // 
            this.zTransNum.DecimalPlaces = 2;
            this.zTransNum.Location = new System.Drawing.Point(189, 19);
            this.zTransNum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.zTransNum.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.zTransNum.Name = "zTransNum";
            this.zTransNum.Size = new System.Drawing.Size(48, 20);
            this.zTransNum.TabIndex = 26;
            this.zTransNum.ValueChanged += new System.EventHandler(this.zTransNum_ValueChanged);
            // 
            // yTranNum
            // 
            this.yTranNum.DecimalPlaces = 2;
            this.yTranNum.Location = new System.Drawing.Point(115, 19);
            this.yTranNum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.yTranNum.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.yTranNum.Name = "yTranNum";
            this.yTranNum.Size = new System.Drawing.Size(48, 20);
            this.yTranNum.TabIndex = 25;
            this.yTranNum.ValueChanged += new System.EventHandler(this.yTranNum_ValueChanged);
            // 
            // xTransValue
            // 
            this.xTransValue.DecimalPlaces = 2;
            this.xTransValue.Location = new System.Drawing.Point(41, 19);
            this.xTransValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.xTransValue.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.xTransValue.Name = "xTransValue";
            this.xTransValue.Size = new System.Drawing.Size(48, 20);
            this.xTransValue.TabIndex = 24;
            this.xTransValue.ValueChanged += new System.EventHandler(this.xTransValue_ValueChanged);
            // 
            // buttonAutoplace
            // 
            this.buttonAutoplace.Location = new System.Drawing.Point(130, 66);
            this.buttonAutoplace.Name = "buttonAutoplace";
            this.buttonAutoplace.Size = new System.Drawing.Size(118, 33);
            this.buttonAutoplace.TabIndex = 20;
            this.buttonAutoplace.Text = "Auto Place";
            this.buttonAutoplace.UseVisualStyleBackColor = true;
            this.buttonAutoplace.Click += new System.EventHandler(this.buttonAutoplace_Click);
            // 
            // buttonCenter
            // 
            this.buttonCenter.Location = new System.Drawing.Point(6, 66);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(118, 34);
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(112, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Y-Axis";
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
            // rotateTabPage2
            // 
            this.rotateTabPage2.Controls.Add(this.rotateMinus90Z);
            this.rotateTabPage2.Controls.Add(this.rotateMinus90Y);
            this.rotateTabPage2.Controls.Add(this.rotate90Z);
            this.rotateTabPage2.Controls.Add(this.rotate90Y);
            this.rotateTabPage2.Controls.Add(this.rotateMinus90X);
            this.rotateTabPage2.Controls.Add(this.rotate90X);
            this.rotateTabPage2.Controls.Add(this.zRotateControl);
            this.rotateTabPage2.Controls.Add(this.yRotateControl);
            this.rotateTabPage2.Controls.Add(this.xRotateControl);
            this.rotateTabPage2.Controls.Add(this.label13);
            this.rotateTabPage2.Controls.Add(this.label14);
            this.rotateTabPage2.Controls.Add(this.label15);
            this.rotateTabPage2.Location = new System.Drawing.Point(4, 25);
            this.rotateTabPage2.Name = "rotateTabPage2";
            this.rotateTabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.rotateTabPage2.Size = new System.Drawing.Size(272, 108);
            this.rotateTabPage2.TabIndex = 1;
            this.rotateTabPage2.Text = "Rotate";
            this.rotateTabPage2.UseVisualStyleBackColor = true;
            // 
            // rotateMinus90Z
            // 
            this.rotateMinus90Z.Location = new System.Drawing.Point(189, 77);
            this.rotateMinus90Z.Name = "rotateMinus90Z";
            this.rotateMinus90Z.Size = new System.Drawing.Size(48, 26);
            this.rotateMinus90Z.TabIndex = 33;
            this.rotateMinus90Z.Text = "-90";
            this.rotateMinus90Z.UseVisualStyleBackColor = true;
            this.rotateMinus90Z.Click += new System.EventHandler(this.rotateMinus90Z_Click);
            // 
            // rotateMinus90Y
            // 
            this.rotateMinus90Y.Location = new System.Drawing.Point(115, 77);
            this.rotateMinus90Y.Name = "rotateMinus90Y";
            this.rotateMinus90Y.Size = new System.Drawing.Size(48, 26);
            this.rotateMinus90Y.TabIndex = 32;
            this.rotateMinus90Y.Text = "-90";
            this.rotateMinus90Y.UseVisualStyleBackColor = true;
            this.rotateMinus90Y.Click += new System.EventHandler(this.rotateMinus90Y_Click);
            // 
            // rotate90Z
            // 
            this.rotate90Z.Location = new System.Drawing.Point(189, 45);
            this.rotate90Z.Name = "rotate90Z";
            this.rotate90Z.Size = new System.Drawing.Size(48, 26);
            this.rotate90Z.TabIndex = 31;
            this.rotate90Z.Text = "+90";
            this.rotate90Z.UseVisualStyleBackColor = true;
            this.rotate90Z.Click += new System.EventHandler(this.rotate90Z_Click);
            // 
            // rotate90Y
            // 
            this.rotate90Y.Location = new System.Drawing.Point(115, 45);
            this.rotate90Y.Name = "rotate90Y";
            this.rotate90Y.Size = new System.Drawing.Size(48, 26);
            this.rotate90Y.TabIndex = 30;
            this.rotate90Y.Text = "+90";
            this.rotate90Y.UseVisualStyleBackColor = true;
            this.rotate90Y.Click += new System.EventHandler(this.rotate90Y_Click);
            // 
            // rotateMinus90X
            // 
            this.rotateMinus90X.Location = new System.Drawing.Point(41, 77);
            this.rotateMinus90X.Name = "rotateMinus90X";
            this.rotateMinus90X.Size = new System.Drawing.Size(48, 26);
            this.rotateMinus90X.TabIndex = 29;
            this.rotateMinus90X.Text = "-90";
            this.rotateMinus90X.UseVisualStyleBackColor = true;
            this.rotateMinus90X.Click += new System.EventHandler(this.rotateMinus90X_Click);
            // 
            // rotate90X
            // 
            this.rotate90X.Location = new System.Drawing.Point(41, 45);
            this.rotate90X.Name = "rotate90X";
            this.rotate90X.Size = new System.Drawing.Size(48, 26);
            this.rotate90X.TabIndex = 28;
            this.rotate90X.Text = "+90";
            this.rotate90X.UseVisualStyleBackColor = true;
            this.rotate90X.Click += new System.EventHandler(this.rotate90X_Click);
            // 
            // zRotateControl
            // 
            this.zRotateControl.DecimalPlaces = 2;
            this.zRotateControl.Location = new System.Drawing.Point(189, 19);
            this.zRotateControl.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.zRotateControl.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.zRotateControl.Name = "zRotateControl";
            this.zRotateControl.Size = new System.Drawing.Size(48, 20);
            this.zRotateControl.TabIndex = 27;
            this.zRotateControl.ValueChanged += new System.EventHandler(this.zRotateControl_ValueChanged);
            // 
            // yRotateControl
            // 
            this.yRotateControl.DecimalPlaces = 2;
            this.yRotateControl.Location = new System.Drawing.Point(115, 19);
            this.yRotateControl.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.yRotateControl.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.yRotateControl.Name = "yRotateControl";
            this.yRotateControl.Size = new System.Drawing.Size(48, 20);
            this.yRotateControl.TabIndex = 26;
            this.yRotateControl.ValueChanged += new System.EventHandler(this.yRotateControl_ValueChanged);
            // 
            // xRotateControl
            // 
            this.xRotateControl.DecimalPlaces = 2;
            this.xRotateControl.Location = new System.Drawing.Point(41, 19);
            this.xRotateControl.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.xRotateControl.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.xRotateControl.Name = "xRotateControl";
            this.xRotateControl.Size = new System.Drawing.Size(48, 20);
            this.xRotateControl.TabIndex = 25;
            this.xRotateControl.ValueChanged += new System.EventHandler(this.xRotateControl_ValueChanged);
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
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(112, 3);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(36, 13);
            this.label14.TabIndex = 13;
            this.label14.Text = "Y-Axis";
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
            // scaleTabPage3
            // 
            this.scaleTabPage3.Controls.Add(this.label16);
            this.scaleTabPage3.Controls.Add(this.checkScaleAll);
            this.scaleTabPage3.Controls.Add(this.label17);
            this.scaleTabPage3.Controls.Add(this.label18);
            this.scaleTabPage3.Controls.Add(this.textScaleX);
            this.scaleTabPage3.Controls.Add(this.textScaleY);
            this.scaleTabPage3.Controls.Add(this.textScaleZ);
            this.scaleTabPage3.Location = new System.Drawing.Point(4, 25);
            this.scaleTabPage3.Name = "scaleTabPage3";
            this.scaleTabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.scaleTabPage3.Size = new System.Drawing.Size(272, 108);
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
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(112, 3);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(36, 13);
            this.label17.TabIndex = 13;
            this.label17.Text = "Y-Axis";
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
            // copyModelTab
            // 
            this.copyModelTab.Controls.Add(this.buttonCopy);
            this.copyModelTab.Controls.Add(this.checkAutoposition);
            this.copyModelTab.Controls.Add(this.numericCopies);
            this.copyModelTab.Controls.Add(this.labelNumberOfCopies);
            this.copyModelTab.Location = new System.Drawing.Point(4, 25);
            this.copyModelTab.Name = "copyModelTab";
            this.copyModelTab.Padding = new System.Windows.Forms.Padding(3);
            this.copyModelTab.Size = new System.Drawing.Size(272, 108);
            this.copyModelTab.TabIndex = 3;
            this.copyModelTab.Text = "Copy";
            this.copyModelTab.UseVisualStyleBackColor = true;
            // 
            // buttonCopy
            // 
            this.buttonCopy.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonCopy.Location = new System.Drawing.Point(3, 76);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(75, 23);
            this.buttonCopy.TabIndex = 8;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopyObjects_Click);
            // 
            // checkAutoposition
            // 
            this.checkAutoposition.AutoSize = true;
            this.checkAutoposition.Checked = true;
            this.checkAutoposition.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkAutoposition.Location = new System.Drawing.Point(6, 38);
            this.checkAutoposition.Name = "checkAutoposition";
            this.checkAutoposition.Size = new System.Drawing.Size(183, 17);
            this.checkAutoposition.TabIndex = 7;
            this.checkAutoposition.Text = "Auto position after adding objects";
            this.checkAutoposition.UseVisualStyleBackColor = true;
            // 
            // numericCopies
            // 
            this.numericCopies.Location = new System.Drawing.Point(103, 4);
            this.numericCopies.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericCopies.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericCopies.Name = "numericCopies";
            this.numericCopies.Size = new System.Drawing.Size(51, 20);
            this.numericCopies.TabIndex = 6;
            this.numericCopies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelNumberOfCopies
            // 
            this.labelNumberOfCopies.AutoSize = true;
            this.labelNumberOfCopies.Location = new System.Drawing.Point(3, 4);
            this.labelNumberOfCopies.Name = "labelNumberOfCopies";
            this.labelNumberOfCopies.Size = new System.Drawing.Size(93, 13);
            this.labelNumberOfCopies.TabIndex = 5;
            this.labelNumberOfCopies.Text = "Number of copies:";
            // 
            // PositionSTLGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PositionTabControl);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PositionSTLGUI";
            this.Size = new System.Drawing.Size(283, 137);
            this.Leave += new System.EventHandler(this.PositionSTLGUI_Leave);
            this.MouseLeave += new System.EventHandler(this.PositionSTLGUI_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.PositionTabControl.ResumeLayout(false);
            this.movetabPage1.ResumeLayout(false);
            this.movetabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zTransNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yTranNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xTransValue)).EndInit();
            this.rotateTabPage2.ResumeLayout(false);
            this.rotateTabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zRotateControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yRotateControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xRotateControl)).EndInit();
            this.scaleTabPage3.ResumeLayout(false);
            this.scaleTabPage3.PerformLayout();
            this.copyModelTab.ResumeLayout(false);
            this.copyModelTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericCopies)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkScaleAll;
        private System.Windows.Forms.TextBox textScaleZ;
        private System.Windows.Forms.TextBox textScaleY;
        private System.Windows.Forms.TextBox textScaleX;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.OpenFileDialog openFileSTL;
        private System.Windows.Forms.SaveFileDialog saveSTL;
        private System.Windows.Forms.TabControl PositionTabControl;
        private System.Windows.Forms.TabPage movetabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage rotateTabPage2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TabPage scaleTabPage3;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TabPage copyModelTab;
        private System.Windows.Forms.Button buttonCenter;
        private System.Windows.Forms.Button buttonAutoplace;
        private System.Windows.Forms.Button buttonCopy;
        public System.Windows.Forms.CheckBox checkAutoposition;
        public System.Windows.Forms.NumericUpDown numericCopies;
        private System.Windows.Forms.Label labelNumberOfCopies;
        private System.Windows.Forms.NumericUpDown xTransValue;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.NumericUpDown yTranNum;
        private System.Windows.Forms.NumericUpDown zTransNum;
        private System.Windows.Forms.NumericUpDown zRotateControl;
        private System.Windows.Forms.NumericUpDown yRotateControl;
        private System.Windows.Forms.NumericUpDown xRotateControl;
        private System.Windows.Forms.Button rotateMinus90X;
        private System.Windows.Forms.Button rotate90X;
        private System.Windows.Forms.Button rotateMinus90Z;
        private System.Windows.Forms.Button rotateMinus90Y;
        private System.Windows.Forms.Button rotate90Z;
        private System.Windows.Forms.Button rotate90Y;
    }
}
