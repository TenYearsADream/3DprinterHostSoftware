namespace RepetierHost.view
{
    partial class ThreeDControl
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
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.gl = new RepetierHost.view.utils.RHOpenGL();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 33;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // gl
            // 
            this.gl.AutoSize = true;
            this.gl.BackColor = System.Drawing.Color.Black;
            this.gl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gl.Location = new System.Drawing.Point(0, 0);
            this.gl.Name = "gl";
            this.gl.Size = new System.Drawing.Size(1206, 534);
            this.gl.TabIndex = 2;
            this.gl.VSync = false;
            this.gl.Paint += new System.Windows.Forms.PaintEventHandler(this.gl_Paint);
            this.gl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ThreeDControl_KeyDown);
            this.gl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ThreeDControl_KeyPress);
            this.gl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gl_MouseDown);
            this.gl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gl_MouseMove);
            this.gl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gl_MouseUp);
            this.gl.Resize += new System.EventHandler(this.gl_Resize);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // ThreeDControl
            // 
            this.AutoSize = true;
            this.Controls.Add(this.gl);
            this.Name = "ThreeDControl";
            this.Size = new System.Drawing.Size(1206, 534);
            this.Load += new System.EventHandler(this.ThreeDControl_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ThreeDControl_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ThreeDControl_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        public RepetierHost.view.utils.RHOpenGL gl;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}
