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
    public partial class DeckForm : Form
    {
        public DeckForm()
        {
            InitializeComponent();
        }

        public uint Detents { get; set; }

        public IList<string> OtherIDs { get; set; }

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

        public uint FrontLevels
        {
            get { return (uint)FrontLevelsUpDown.Value; }
            set { FrontLevelsUpDown.Value = value; }
        }

        public uint BackLevels
        {
            get { return (uint)BackLevelsUpDown.Value; }
            set { BackLevelsUpDown.Value = value; }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (Detents != 0 && (ShaftPositions % Detents) != 0)
            {
                MessageBox.Show("The switch positions must be a multiple of the shaft positions");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            if (string.IsNullOrWhiteSpace(ID))
            {
                MessageBox.Show("Please enter a valid deck name.", "Error");
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
