namespace Rotary_Switch_Designer
{
    partial class WaferControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SpokeMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sharedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SpokeMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // SpokeMenuStrip
            // 
            this.SpokeMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sharedToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.SpokeMenuStrip.Name = "SpokeMenuStrip";
            this.SpokeMenuStrip.Size = new System.Drawing.Size(153, 70);
            this.SpokeMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.SpokeMenuStrip_ItemClicked);
            // 
            // sharedToolStripMenuItem
            // 
            this.sharedToolStripMenuItem.Name = "sharedToolStripMenuItem";
            this.sharedToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sharedToolStripMenuItem.Tag = "";
            this.sharedToolStripMenuItem.Text = "Shared";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem.Tag = "";
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // WaferControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "WaferControl";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Wafer_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WaferControl_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WaferControl_KeyUp);
            this.MouseLeave += new System.EventHandler(this.WaferControl_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Wafer_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.WaferControl_MouseUp);
            this.SpokeMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip SpokeMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem sharedToolStripMenuItem;
    }
}
