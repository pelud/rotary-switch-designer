namespace Rotary_Switch_Designer
{
    partial class DeckForm
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
            this.FrontLevelsUpDown = new System.Windows.Forms.NumericUpDown();
            this.BackLevelsUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.PositionUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.FrontLevelsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackLevelsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionUpDown)).BeginInit();
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
            this.NameTextBox.Location = new System.Drawing.Point(98, 18);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(116, 20);
            this.NameTextBox.TabIndex = 2;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(139, 152);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 30);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(58, 152);
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
            this.label3.Location = new System.Drawing.Point(12, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Rotor Levels:";
            // 
            // FrontLevelsUpDown
            // 
            this.FrontLevelsUpDown.Location = new System.Drawing.Point(98, 89);
            this.FrontLevelsUpDown.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.FrontLevelsUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FrontLevelsUpDown.Name = "FrontLevelsUpDown";
            this.FrontLevelsUpDown.Size = new System.Drawing.Size(55, 20);
            this.FrontLevelsUpDown.TabIndex = 4;
            this.FrontLevelsUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // BackLevelsUpDown
            // 
            this.BackLevelsUpDown.Location = new System.Drawing.Point(159, 89);
            this.BackLevelsUpDown.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.BackLevelsUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BackLevelsUpDown.Name = "BackLevelsUpDown";
            this.BackLevelsUpDown.Size = new System.Drawing.Size(55, 20);
            this.BackLevelsUpDown.TabIndex = 5;
            this.BackLevelsUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(98, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "Front";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(156, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 16);
            this.label6.TabIndex = 15;
            this.label6.Text = "Back";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PositionUpDown
            // 
            this.PositionUpDown.Location = new System.Drawing.Point(98, 44);
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
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Shaft Positions:";
            // 
            // DeckForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(234, 204);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.BackLevelsUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.NameTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FrontLevelsUpDown);
            this.Controls.Add(this.PositionUpDown);
            this.Name = "DeckForm";
            this.Text = "Deck";
            ((System.ComponentModel.ISupportInitialize)(this.FrontLevelsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackLevelsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown FrontLevelsUpDown;
        private System.Windows.Forms.NumericUpDown BackLevelsUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown PositionUpDown;
        private System.Windows.Forms.Label label2;
    }
}