using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Drawing.Drawing2D;

namespace Rotary_Switch_Designer
{
    public partial class SwitchControl : UserControl
    {
        #region Events
        public delegate void RequestOperationHandler(object sender, Action<Model.Switch> action);
        public event RequestOperationHandler RequestOperation;
        public delegate void SelectedItemChangedHandler(object sender);
        public event SelectedItemChangedHandler SelectedItemChanged;
        #endregion

        #region Variables
        private bool m_Dirty = false;
        private Model.Switch m_Data = null;
        private const string m_ClipboardFormat = "Wafer";
        private const int m_PreviewImageSize = 180;
        private const int m_PositionImageSize = 128;
        private const int m_PositionImagePad = 0;
        #endregion

        public SwitchControl()
        {
            InitializeComponent();
        }

        private void SwitchControl_Load(object sender, EventArgs e)
        {
            this.SideListView.LargeImageList = new ImageList();
            this.SideListView.LargeImageList.ImageSize = new Size(m_PreviewImageSize, m_PreviewImageSize);
            data_PropertyChanged(null, null);
        }
        
        public Model.Switch Data
        {
            get { return m_Data; }
            set
            {
                // clear the old callback
                if (m_Data != null)
                    m_Data.PropertyChanged -= data_PropertyChanged;

                // update the value
                m_Data = value;

                    // set the new callback
                if (m_Data != null)
                    m_Data.PropertyChanged += new PropertyChangedEventHandler(data_PropertyChanged);

                // update the GUI
                data_PropertyChanged(m_Data, new PropertyChangedEventArgs("Data"));
            }
        }

        void data_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!this.IsHandleCreated)
                return;

            lock (this)
            {
                m_Dirty = true;
            }

            this.BeginInvoke((Action)(() =>
            {
                lock (this)
                {
                    if (!m_Dirty)
                        return;
                    m_Dirty = false;
                }

                // refresh the list views
                RefreshSideListViewItems();
                RefreshPositionListViewItems();

                if (m_Data != null)
                {
                    uint pos = 0;
                    wafer1.StatorStart = m_Data.StatorStart;
                    wafer1.RotorPosition = pos * 360 / m_Data.Shafts[0].Detents;
                    wafer1.RearView = m_Data.RearView;
                    wafer1.TextCCW = m_Data.TextCCW;
                }
            }));
        }

        private void RefreshSideListViewItems()
        {
            if (m_Data == null)
            {
                this.SideListView.Items.Clear();
                return;
            }

            // get the current selected entry
            var selected = SideListView.SelectedItems != null && SideListView.SelectedItems.Count > 0 ? SideListView.SelectedItems[0] : null;

            // check each deck
            for (int i = 0; i < m_Data.Sides.Count; ++i)
            {
                var data = m_Data.Sides[i];
                if (i >= this.SideListView.Items.Count)
                {
                    // add an entry to the list view if there isn't enough for each side
                    this.SideListView.Items.Add(new ListViewItem(data.Name, i) { Tag = data });
                    this.SideListView.LargeImageList.Images.Add(new Bitmap(1, 1));
                }
                else if (this.SideListView.Items[i].Text != data.Name || this.SideListView.Items[i].Tag != data)
                {
                    // update the existing entry
                    this.SideListView.Items[i].Text = data.Name;
                    this.SideListView.Items[i].Tag = data;

                    // if this is the selected item then update the wafer side views
                    if (selected == this.SideListView.Items[i])
                    {
                        this.wafer1.Data = data;
                    }
                }
            }

            // remove any excess sides
            while (this.SideListView.Items.Count > m_Data.Sides.Count)
            {
                this.SideListView.LargeImageList.Images.RemoveAt(this.SideListView.Items.Count - 1);
                this.SideListView.Items.RemoveAt(this.SideListView.Items.Count - 1);
            }

            // update the icons
            var bg_brush = new SolidBrush(SideListView.BackColor);
            var fg_brush = new SolidBrush(SideListView.ForeColor);
            var fg_pen = new Pen(fg_brush);
            try
            {
                var i = 0;
                var lil = this.SideListView.LargeImageList;
                var client = new Rectangle(0, 0, lil.ImageSize.Width, lil.ImageSize.Height);
                foreach (ListViewItem item in this.SideListView.Items)
                {
                    var data = (Model.Side)item.Tag;
                    if (data != null)
                    {
                        var image = new Bitmap(client.Width, client.Height);
                        var g = Graphics.FromImage(image);
                        WaferControl.CreateThumbnail(m_Data.StatorStart, m_Data.RearView, m_Data.TextCCW, data, g, client, 0, bg_brush, fg_pen, fg_brush, this.Font);
                        lil.Images[i] = image;
                    }
                    i++;
                }
            }
            finally
            {
                bg_brush.Dispose();
                fg_brush.Dispose();
                fg_pen.Dispose();
            }

            // redraw the list view
            if (SideListView.Items.Count > 0)
                SideListView.RedrawItems(0, SideListView.Items.Count - 1, false);
        }

        private void RefreshPositionListViewItems()
        {
            var selected = SelectedItem;
            if (m_Data == null || selected == null || m_Data.Shafts.Count == 0)
            {
                pictureBox1.Image = new Bitmap(1, 1);
                return;
            }

            // update the icons
            var bg_brush = new SolidBrush(pictureBox1.BackColor);
            var fg_brush = new SolidBrush(pictureBox1.ForeColor);
            var fg_pen = new Pen(fg_brush);
            try
            {
                // calculate the detent information
                uint detents = m_Data.Shafts[0].Detents;
                uint count = m_Data.Shafts[0].DetentStopCount != 0 ? (uint)m_Data.Shafts[0].DetentStopCount : detents;
                uint first_index = m_Data.Shafts[0].DetentStopCount != 0 ? (uint)m_Data.Shafts[0].DetentStopFirst : 0u;

                // measure the label size
                var label_size = Graphics.FromImage(new Bitmap(1, 1)).MeasureString(first_index.ToString(), panel1.Font);

                // create the backing image
                int element_total_size = m_PositionImageSize + m_PositionImagePad + (int)label_size.Height;
                //var image = new Bitmap(element_total_size * count, m_PositionImageSize);
                var image = new Bitmap(m_PositionImageSize, element_total_size * (int)count);
                var g = Graphics.FromImage(image);

                // draw each angle
                for (uint i = 0; i < count; ++i)
                {
                    uint position = (i + first_index) % detents;
                    uint angle = position * 360u / m_Data.Shafts[0].Detents;
                    
                    // create the thumbnail
                    //var client = new Rectangle(element_total_size * i, 0, m_PositionImageSize, m_PositionImageSize);
                    var client = new Rectangle(0, element_total_size * (int)i, m_PositionImageSize, m_PositionImageSize);
                    g.FillRectangle(bg_brush, client);
                    WaferControl.CreateThumbnail(m_Data.StatorStart, m_Data.RearView, m_Data.TextCCW, selected, g, client, angle, bg_brush, fg_pen, fg_brush, this.Font);

                    // draw the position number
                    string text = (position + 1).ToString();
                    var text_size = g.MeasureString(text, panel1.Font);
                    var p = new Point((int)(image.Width - text_size.Width) / 2, (int)(element_total_size * i + m_PositionImageSize));
                    g.FillRectangle(bg_brush, p.X, p.Y, text_size.Width, text_size.Height);
                    g.DrawString((position + 1).ToString(), panel1.Font, fg_brush, p.X, p.Y);
                }
                pictureBox1.Image = image;
            }
            finally
            {
                bg_brush.Dispose();
                fg_brush.Dispose();
                fg_pen.Dispose();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var deck = SelectedItem;
            this.wafer1.Data = deck;
            RefreshPositionListViewItems();
            if (SelectedItemChanged != null)
                SelectedItemChanged(this);
        }

        public Model.Side SelectedItem
        {
            get
            {
                var lvi = SideListView.SelectedItems != null && SideListView.SelectedItems.Count > 0 ? SideListView.SelectedItems[0] : null;
                return lvi != null ? (Model.Side)lvi.Tag : null;
            }
        }

        public int SelectedItemIndex
        {
            get
            {
                var lvi = SideListView.SelectedItems != null && SideListView.SelectedItems.Count > 0 ? SideListView.SelectedItems[0] : null;
                return lvi != null ? SideListView.Items.IndexOf(lvi) : -1;
            }
            set
            {
                if (value < -1 || value >= SideListView.Items.Count)
                    throw new ArgumentOutOfRangeException("value");
                SideListView.SelectedIndices.Clear();
                if (value != -1)
                    SideListView.SelectedIndices.Add(value);
            }
        }

        private void Operation(Action<Model.Switch> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            if (RequestOperation != null)
                RequestOperation(this, action);
        }

        private void wafer1_RequestOperation(object sender, Action<Model.Side> action)
        {
            if (sender == null)
                throw new ArgumentNullException("sender");
            if (action == null)
                throw new ArgumentNullException("action");
            var wafer = (WaferControl)sender;

            var lvi = SideListView.SelectedItems != null && SideListView.SelectedItems.Count > 0 ? SideListView.SelectedItems[0] : null;
            if (lvi != null)
            {
                int index = SideListView.Items.IndexOf(lvi);
                if (index == -1)
                    throw new Exception("invalid listview index");

                Operation((Action<Model.Switch>)((data) =>
                {
                    // argument checks
                    if (data == null)
                        throw new ArgumentNullException("data");
                    if (index < 0 || index >= data.Sides.Count)
                        throw new Exception("invalid side index");

                    // perform the action
                    action(data.Sides[index]);
                }));
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (SelectedItem != null && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.listViewMenuStrip.Show(this.SideListView, e.Location);
            }
        }

        public void Delete()
        {
            int index = this.SelectedItemIndex;
            if (index == -1)
                return;

            Operation((Action<Model.Switch>)((data) =>
            {
                // santify checks
                if (data == null)
                    throw new ArgumentNullException("data");
                if (index < 0 || index >= data.Sides.Count)
                    throw new ArgumentOutOfRangeException("index");

                // remove the item
                data.Sides.RemoveAt(index);
            }));
        }

        private char[] m_Base26Table = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private string ConvertBase26(int value)
        {
            string result = "";
            while (result == "" || value != 0)
            {
                result = m_Base26Table[(int)(value % m_Base26Table.Length)] + result;
                value /= m_Base26Table.Length;
            }

            return result;
        }

        public void Add(Model.Side side)
        {
            if (side == null)
                throw new ArgumentNullException("side");
            if (m_Data == null)
                throw new Exception("data not set");
            if (side.Shaft < 0 || side.Shaft >= m_Data.Shafts.Count)
                throw new ArgumentOutOfRangeException("side.Shaft");

            // check if the shaft position is duplicated
            bool rear = side.ShaftPositionRear;
            int shaft_position = side.ShaftPosition;
            while (m_Data.Sides.Any((value) => value.ShaftPositionRear == rear && value.ShaftPosition == shaft_position))
                shaft_position++;

            // check if the name is duplicated (which it probably will be if the user copied & pasted in the same document)
            string name = side.Name;
            if (m_Data.Sides.Any((value) => value.Name == side.Name))
            {
                // find the next letter that is free
                int next_name = 0;
                while (m_Data.Sides.Any((s) => s.Name == ConvertBase26(next_name)))
                    next_name++;

                // update the name with the new one
                name = ConvertBase26(next_name);
            }

            Operation((Action<Model.Switch>)((data) =>
            {
                // santify checks
                if (data == null)
                    throw new ArgumentNullException("data");

                // change the name and side to the randomly assigned one
                var tmp = side.CloneObject();
                tmp.Name = name;
                tmp.ShaftPosition = shaft_position;

                // add the item
                data.Sides.Add(tmp);
            }));
        }

        public void Copy()
        {
            if (this.SelectedItem == null)
                throw new Exception("no item selected to cut/copy");

            // get the data
            var data = (Model.Side)this.SelectedItem.CloneObject();

            // convert the data to a string
            string result = null;
            var xs = new XmlSerializer(typeof(Model.Side));
            using (var writer = new StringWriter())
            {
                xs.Serialize(writer, data);
                result = writer.ToString();
            }

            // copy it to the clipboard
            try { Clipboard.SetData(m_ClipboardFormat, result); }
            catch
            {
                // if the clipboard is busy then wait 1 second and try again
                System.Threading.Thread.Sleep(1000);
                Clipboard.SetData(m_ClipboardFormat, result);
            }
        }

        public bool CanPaste
        {
            get
            {
                // check if the clipboard has our text on it
                try { return Clipboard.ContainsData(m_ClipboardFormat); }
                catch { }
                return false;
            }
        }

        public void Paste()
        {
            try
            {
                if (Clipboard.ContainsData(m_ClipboardFormat))
                {
                    // get the data from the clipboard
                    string text = (string)Clipboard.GetData(m_ClipboardFormat);

                    // convert the data from a string
                    Model.Side data = null;
                    var xs = new XmlSerializer(typeof(Model.Side));
                    using (var reader = new StringReader(text))
                    {
                        data = (Model.Side)xs.Deserialize(reader);
                    }

                    // add it to the items
                    if (data != null)
                        Add(data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Cannot paste deck: {0}", ex.Message), "Error");
            }
        }

        public void Properties()
        {
            if (m_Data == null)
                throw new Exception("data not set");
            if (SelectedItem == null || SelectedItemIndex < 0)
                return;

            try
            {
                var other_shaft_positions = from side in m_Data.Sides
                                            where side.Shaft != SelectedItem.Shaft && side.ShaftPosition != SelectedItem.ShaftPosition && side.ShaftPositionRear != SelectedItem.ShaftPositionRear
                                            select Tuple.Create(side.ShaftPosition, side.ShaftPositionRear);

                using (var form = new SideForm()
                {
                    OtherIDs = m_Data.Sides.Select((side) => side.Name).Where((name) => name != SelectedItem.Name).ToList(),
                    OtherShaftPositions = other_shaft_positions.ToList(),
                    ID = SelectedItem.Name,
                    ShaftPositions = (uint)SelectedItem.Positions.Count,
                    Shaft = SelectedItem.Shaft,
                    ShaftPosition = SelectedItem.ShaftPosition,
                    ShaftPositionBack = SelectedItem.ShaftPositionRear,
                    Levels = (uint)(SelectedItem.RotorLevels()),
                })
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        var copy = new
                        {
                            Index = SelectedItemIndex,
                            Name = form.ID,
                            ShaftPositions = form.ShaftPositions,
                            Levels = form.Levels,
                            Shaft = form.Shaft,
                            ShaftPosition = form.ShaftPosition,
                            ShaftPositionBack = form.ShaftPositionBack,
                        };
                        Operation((Action<Model.Switch>)((data) =>
                        {
                            // argument checks
                            if (data == null)
                                throw new ArgumentNullException("data");
                            if (copy.Index >= data.Sides.Count)
                                throw new Exception("invalid side index");
                            if (data.Sides[copy.Index] == null)
                                throw new Exception("side is null");

                            // update the name
                            var side = data.Sides[copy.Index];
                            side.Name = copy.Name;
                            side.Shaft = copy.Shaft;
                            side.ShaftPosition = copy.ShaftPosition;
                            side.ShaftPositionRear = copy.ShaftPositionBack;

                            // make sure there are enough positions
                            while (side.Positions.Count < copy.ShaftPositions)
                                side.Positions.Add(new Model.Position());

                            // remove any excess positions
                            while (side.Positions.Count > copy.ShaftPositions)
                                side.Positions.RemoveAt(side.Positions.Count - 1);

                            // check the positions
                            for (int j = 0; j < side.Positions.Count; ++j)
                            {
                                var position = side.Positions[j];
                                if (position == null)
                                    throw new Exception("position is null");

                                // add any new rotor levels
                                while (position.RotorSlices.Count < copy.Levels)
                                    position.RotorSlices.Add(new Model.RotorSlice());

                                // remove any excess rotor levels
                                while (position.RotorSlices.Count > copy.Levels)
                                    position.RotorSlices.RemoveAt(position.RotorSlices.Count - 1);
                            }
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error");
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
            Delete();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties();
        }
    }
}
