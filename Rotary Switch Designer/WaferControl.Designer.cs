﻿namespace Rotary_Switch_Designer
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // SpokeMenuStrip
            // 
            this.SpokeMenuStrip.Name = "SpokeMenuStrip";
            this.SpokeMenuStrip.Size = new System.Drawing.Size(153, 26);
            this.SpokeMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.SpokeMenuStrip_ItemClicked);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip SpokeMenuStrip;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
