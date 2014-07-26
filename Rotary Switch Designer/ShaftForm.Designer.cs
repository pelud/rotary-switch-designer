namespace Rotary_Switch_Designer
{
    partial class ShaftForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DetentStopCountUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.DetentStartFirstUpDown = new System.Windows.Forms.NumericUpDown();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.AngularPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.DetentsUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DetentStopCountUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DetentStartFirstUpDown)).BeginInit();
            this.AngularPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DetentsUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.DetentStopCountUpDown);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.DetentStartFirstUpDown);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(12, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 72);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(87, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Count";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DetentStopCountUpDown
            // 
            this.DetentStopCountUpDown.Location = new System.Drawing.Point(87, 40);
            this.DetentStopCountUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DetentStopCountUpDown.Name = "DetentStopCountUpDown";
            this.DetentStopCountUpDown.Size = new System.Drawing.Size(55, 20);
            this.DetentStopCountUpDown.TabIndex = 15;
            this.DetentStopCountUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Start";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DetentStartFirstUpDown
            // 
            this.DetentStartFirstUpDown.Location = new System.Drawing.Point(10, 40);
            this.DetentStartFirstUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DetentStartFirstUpDown.Name = "DetentStartFirstUpDown";
            this.DetentStartFirstUpDown.Size = new System.Drawing.Size(55, 20);
            this.DetentStartFirstUpDown.TabIndex = 9;
            this.DetentStartFirstUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(89, 116);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 30);
            this.buttonCancel.TabIndex = 16;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(8, 116);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 30);
            this.buttonOK.TabIndex = 15;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(23, 38);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 17);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.Text = "Rotation Stops";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // AngularPanel
            // 
            this.AngularPanel.Controls.Add(this.label2);
            this.AngularPanel.Controls.Add(this.DetentsUpDown);
            this.AngularPanel.Location = new System.Drawing.Point(8, 2);
            this.AngularPanel.Name = "AngularPanel";
            this.AngularPanel.Size = new System.Drawing.Size(160, 30);
            this.AngularPanel.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Angular Positions:";
            // 
            // DetentsUpDown
            // 
            this.DetentsUpDown.Location = new System.Drawing.Point(98, 5);
            this.DetentsUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.DetentsUpDown.Name = "DetentsUpDown";
            this.DetentsUpDown.Size = new System.Drawing.Size(55, 20);
            this.DetentsUpDown.TabIndex = 10;
            this.DetentsUpDown.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // ShaftForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(180, 159);
            this.Controls.Add(this.AngularPanel);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Name = "ShaftForm";
            this.Text = "Shaft";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DetentStopCountUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DetentStartFirstUpDown)).EndInit();
            this.AngularPanel.ResumeLayout(false);
            this.AngularPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DetentsUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown DetentStopCountUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown DetentStartFirstUpDown;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Panel AngularPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown DetentsUpDown;
    }
}