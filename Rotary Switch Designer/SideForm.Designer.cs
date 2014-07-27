namespace Rotary_Switch_Designer
{
    partial class SideForm
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
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.LevelsUpDown = new System.Windows.Forms.NumericUpDown();
            this.PositionUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ShaftPositionUpDown = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.FrontRadioButton = new System.Windows.Forms.RadioButton();
            this.BackRadioButton = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.LevelsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShaftPositionUpDown)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(109, 18);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(116, 20);
            this.NameTextBox.TabIndex = 2;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(150, 160);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 30);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(69, 160);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 30);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Rotor Levels:";
            // 
            // LevelsUpDown
            // 
            this.LevelsUpDown.Location = new System.Drawing.Point(109, 70);
            this.LevelsUpDown.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.LevelsUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LevelsUpDown.Name = "LevelsUpDown";
            this.LevelsUpDown.Size = new System.Drawing.Size(55, 20);
            this.LevelsUpDown.TabIndex = 4;
            this.LevelsUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // PositionUpDown
            // 
            this.PositionUpDown.Location = new System.Drawing.Point(109, 44);
            this.PositionUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PositionUpDown.Name = "PositionUpDown";
            this.PositionUpDown.Size = new System.Drawing.Size(55, 20);
            this.PositionUpDown.TabIndex = 3;
            this.PositionUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Angular Positions:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Shaft Position:";
            // 
            // ShaftPositionUpDown
            // 
            this.ShaftPositionUpDown.Location = new System.Drawing.Point(109, 96);
            this.ShaftPositionUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ShaftPositionUpDown.Name = "ShaftPositionUpDown";
            this.ShaftPositionUpDown.Size = new System.Drawing.Size(55, 20);
            this.ShaftPositionUpDown.TabIndex = 10;
            this.ShaftPositionUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.FrontRadioButton);
            this.panel1.Controls.Add(this.BackRadioButton);
            this.panel1.Location = new System.Drawing.Point(99, 122);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(126, 21);
            this.panel1.TabIndex = 12;
            // 
            // FrontRadioButton
            // 
            this.FrontRadioButton.AutoSize = true;
            this.FrontRadioButton.Checked = true;
            this.FrontRadioButton.Location = new System.Drawing.Point(10, 0);
            this.FrontRadioButton.Name = "FrontRadioButton";
            this.FrontRadioButton.Size = new System.Drawing.Size(49, 17);
            this.FrontRadioButton.TabIndex = 0;
            this.FrontRadioButton.TabStop = true;
            this.FrontRadioButton.Text = "Front";
            this.FrontRadioButton.UseVisualStyleBackColor = true;
            // 
            // BackRadioButton
            // 
            this.BackRadioButton.AutoSize = true;
            this.BackRadioButton.Location = new System.Drawing.Point(65, 0);
            this.BackRadioButton.Name = "BackRadioButton";
            this.BackRadioButton.Size = new System.Drawing.Size(50, 17);
            this.BackRadioButton.TabIndex = 1;
            this.BackRadioButton.TabStop = true;
            this.BackRadioButton.Text = "Back";
            this.BackRadioButton.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Side:";
            // 
            // SideForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 206);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ShaftPositionUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.NameTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LevelsUpDown);
            this.Controls.Add(this.PositionUpDown);
            this.Name = "SideForm";
            this.Text = "Side";
            ((System.ComponentModel.ISupportInitialize)(this.LevelsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShaftPositionUpDown)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown LevelsUpDown;
        private System.Windows.Forms.NumericUpDown PositionUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown ShaftPositionUpDown;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton BackRadioButton;
        private System.Windows.Forms.RadioButton FrontRadioButton;
        private System.Windows.Forms.Label label5;
    }
}