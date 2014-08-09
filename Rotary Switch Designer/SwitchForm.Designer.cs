namespace Rotary_Switch_Designer
{
    partial class SwitchForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbRear = new System.Windows.Forms.RadioButton();
            this.rbFront = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbCCW = new System.Windows.Forms.RadioButton();
            this.rbCW = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.SymbolNameTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Symbol Start Angle:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(134, 12);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            359,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(99, 20);
            this.numericUpDown1.TabIndex = 1;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(158, 122);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 30);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(77, 122);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 30);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbRear);
            this.panel1.Controls.Add(this.rbFront);
            this.panel1.Location = new System.Drawing.Point(121, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(131, 22);
            this.panel1.TabIndex = 4;
            // 
            // rbRear
            // 
            this.rbRear.AutoSize = true;
            this.rbRear.Location = new System.Drawing.Point(62, 0);
            this.rbRear.Name = "rbRear";
            this.rbRear.Size = new System.Drawing.Size(48, 17);
            this.rbRear.TabIndex = 1;
            this.rbRear.Text = "Rear";
            this.rbRear.UseVisualStyleBackColor = true;
            // 
            // rbFront
            // 
            this.rbFront.AutoSize = true;
            this.rbFront.Checked = true;
            this.rbFront.Location = new System.Drawing.Point(13, 0);
            this.rbFront.Name = "rbFront";
            this.rbFront.Size = new System.Drawing.Size(49, 17);
            this.rbFront.TabIndex = 0;
            this.rbFront.TabStop = true;
            this.rbFront.Text = "Front";
            this.rbFront.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Symbol View:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Text Direction (Front):";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbCCW);
            this.panel2.Controls.Add(this.rbCW);
            this.panel2.Location = new System.Drawing.Point(121, 61);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(131, 22);
            this.panel2.TabIndex = 7;
            // 
            // rbCCW
            // 
            this.rbCCW.AutoSize = true;
            this.rbCCW.Location = new System.Drawing.Point(62, 0);
            this.rbCCW.Name = "rbCCW";
            this.rbCCW.Size = new System.Drawing.Size(50, 17);
            this.rbCCW.TabIndex = 1;
            this.rbCCW.Text = "CCW";
            this.rbCCW.UseVisualStyleBackColor = true;
            // 
            // rbCW
            // 
            this.rbCW.AutoSize = true;
            this.rbCW.Checked = true;
            this.rbCW.Location = new System.Drawing.Point(13, 0);
            this.rbCW.Name = "rbCW";
            this.rbCW.Size = new System.Drawing.Size(43, 17);
            this.rbCW.TabIndex = 0;
            this.rbCW.TabStop = true;
            this.rbCW.Text = "CW";
            this.rbCW.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Symbol Name:";
            // 
            // SymbolNameTextBox
            // 
            this.SymbolNameTextBox.Location = new System.Drawing.Point(134, 85);
            this.SymbolNameTextBox.Name = "SymbolNameTextBox";
            this.SymbolNameTextBox.Size = new System.Drawing.Size(99, 20);
            this.SymbolNameTextBox.TabIndex = 9;
            // 
            // SwitchForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(254, 164);
            this.Controls.Add(this.SymbolNameTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.Name = "SwitchForm";
            this.Text = "Switch";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbRear;
        private System.Windows.Forms.RadioButton rbFront;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbCCW;
        private System.Windows.Forms.RadioButton rbCW;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SymbolNameTextBox;
    }
}