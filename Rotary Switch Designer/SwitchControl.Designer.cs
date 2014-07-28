namespace Rotary_Switch_Designer
{
    partial class SwitchControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SwitchControl));
            this.listViewMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.SideListView = new System.Windows.Forms.ListView();
            this.PositionListView = new System.Windows.Forms.ListView();
            this.wafer1 = new Rotary_Switch_Designer.WaferControl();
            this.listViewMenuStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewMenuStrip
            // 
            this.listViewMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator3,
            this.propertiesToolStripMenuItem});
            this.listViewMenuStrip.Name = "contextMenuStrip1";
            this.listViewMenuStrip.Size = new System.Drawing.Size(145, 98);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.cutToolStripMenuItem.Text = "Cu&t";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(141, 6);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel1.Controls.Add(this.SideListView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.wafer1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.PositionListView, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(487, 549);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // SideListView
            // 
            this.SideListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.SideListView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.SideListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SideListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.SideListView.HideSelection = false;
            this.SideListView.Location = new System.Drawing.Point(3, 302);
            this.SideListView.MultiSelect = false;
            this.SideListView.Name = "SideListView";
            this.SideListView.ShowGroups = false;
            this.SideListView.Size = new System.Drawing.Size(306, 244);
            this.SideListView.TabIndex = 0;
            this.SideListView.UseCompatibleStateImageBehavior = false;
            this.SideListView.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.SideListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // PositionListView
            // 
            this.PositionListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.PositionListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PositionListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.PositionListView.Location = new System.Drawing.Point(315, 3);
            this.PositionListView.MultiSelect = false;
            this.PositionListView.Name = "PositionListView";
            this.tableLayoutPanel1.SetRowSpan(this.PositionListView, 2);
            this.PositionListView.ShowGroups = false;
            this.PositionListView.Size = new System.Drawing.Size(169, 543);
            this.PositionListView.TabIndex = 3;
            this.PositionListView.UseCompatibleStateImageBehavior = false;
            // 
            // wafer1
            // 
            this.wafer1.Data = null;
            this.wafer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wafer1.Location = new System.Drawing.Point(3, 3);
            this.wafer1.Name = "wafer1";
            this.wafer1.RotorPosition = ((uint)(0u));
            this.wafer1.Size = new System.Drawing.Size(306, 293);
            this.wafer1.StatorStart = ((uint)(0u));
            this.wafer1.TabIndex = 2;
            this.wafer1.Tag = "";
            this.wafer1.RequestOperation += new Rotary_Switch_Designer.WaferControl.RequestOperationHandler(this.wafer1_RequestOperation);
            // 
            // SwitchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SwitchControl";
            this.Size = new System.Drawing.Size(487, 549);
            this.Load += new System.EventHandler(this.SwitchControl_Load);
            this.listViewMenuStrip.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip listViewMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView SideListView;
        private WaferControl wafer1;
        private System.Windows.Forms.ListView PositionListView;
    }
}
