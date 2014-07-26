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
    public partial class ShaftForm : Form
    {
        public ShaftForm()
        {
            InitializeComponent();
        }

        public uint Detents
        {
            get { return (uint)DetentsUpDown.Value; }
            set { DetentsUpDown.Value = value; }
        }

        public bool DetentsReadOnly
        {
            get { return AngularPanel.Enabled; }
            set { AngularPanel.Enabled = !value; }
        }

        public int DetentStopFirst
        {
            get { return (int)DetentStartFirstUpDown.Value; }
            set { DetentStartFirstUpDown.Value = value; }
        }

        public int DetentStopCount
        {
            get { return (int)DetentStopCountUpDown.Value; }
            set { DetentStopCountUpDown.Value = value; }
        }

        public bool HasStops
        {
            get { return checkBox1.Checked; }
            set { checkBox1.Checked = value; }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = checkBox1.Checked;
        }
    }
}
