using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rotary_Switch_Designer
{
    public partial class SideForm : Form
    {
        public SideForm()
        {
            InitializeComponent();
            Shaft = 0;
        }

        public IList<string> OtherIDs { get; set; }

        public IList<Tuple<int, bool>> OtherShaftPositions { get; set; }

        public string ID
        {
            get { return NameTextBox.Text; }
            set { NameTextBox.Text = value; }
        }

        public uint ShaftPositions
        {
            get { return (uint)PositionUpDown.Value; }
            set
            {
                PositionUpDown.Value = value;
            }
        }

        public uint Levels
        {
            get { return (uint)LevelsUpDown.Value; }
            set { LevelsUpDown.Value = value; }
        }

        public int Shaft { get; set; }

        public int ShaftPosition
        {
            get { return (int)ShaftPositionUpDown.Value - 1; }
            set { ShaftPositionUpDown.Value = value + 1; }
        }

        public bool ShaftPositionBack
        {
            get { return BackRadioButton.Checked; }
            set { BackRadioButton.Checked = value; }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                MessageBox.Show("Please enter a valid deck name.", "Error");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            if (OtherShaftPositions != null && OtherShaftPositions.Contains(Tuple.Create(ShaftPosition, ShaftPositionBack)))
            {
                MessageBox.Show("The given shaft position has already been used.  Please choose another.", "Error");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            if (OtherIDs != null && OtherIDs.Contains(ID))
            {
                MessageBox.Show("The specified deck name already exists.  Please choose a different name.", "Error");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
        }
    }
}
