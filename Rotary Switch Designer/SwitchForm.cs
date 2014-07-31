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
    public partial class SwitchForm : Form
    {
        public SwitchForm()
        {
            InitializeComponent();
        }

        public uint NumberingStartAngle
        {
            get { return (uint)numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        public bool RearView
        {
            get { return rbRear.Checked; }
            set { rbRear.Checked = value; }
        }

        public bool TextCCW
        {
            get { return rbCCW.Checked; }
            set { rbCCW.Checked = value; }
        }
    }
}
