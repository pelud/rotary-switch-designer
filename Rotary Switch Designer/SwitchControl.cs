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
        #endregion

        public SwitchControl()
        {
            InitializeComponent();
        }

        private void SwitchControl_Load(object sender, EventArgs e)
        {
            this.listView1.LargeImageList = new ImageList();
            this.listView1.LargeImageList.ImageSize = new Size(m_PreviewImageSize, m_PreviewImageSize);
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

                // refresh the list view
                RefreshListViewItems();

                if (m_Data != null)
                {
                    uint pos = 0;
                    WaferFront.RotorPosition = pos * 360 / m_Data.Shafts[0].Detents;
                }
            }));
        }

        private void RefreshListViewItems()
        {
            if (m_Data == null)
            {
                this.listView1.Items.Clear();
                return;
            }

            // get the current selected entry
            var selected = listView1.SelectedItems != null && listView1.SelectedItems.Count > 0 ? listView1.SelectedItems[0] : null;

            // check each deck
            for (int i = 0; i < m_Data.Sides.Count; ++i)
            {
                var data = m_Data.Sides[i];
                if (i >= this.listView1.Items.Count)
                {
                    // add an entry to the list view if there isn't enough for each deck
                    var lvi = new ListViewItem(data.Name, i);
                    lvi.Tag = data;
                    this.listView1.Items.Add(lvi);
                    this.listView1.LargeImageList.Images.Add(new Bitmap(1, 1));
                }
                else if (this.listView1.Items[i].Text != data.Name || this.listView1.Items[i].Tag != data)
                {
                    // update the existing entry
                    this.listView1.Items[i].Text = data.Name;
                    this.listView1.Items[i].Tag = data;

                    // if this is the selected item then update the wafer side views
                    if (selected == this.listView1.Items[i])
                    {
                        this.WaferFront.Data = data;
                    }
                }
            }

            // remove any excess decks
            while (this.listView1.Items.Count > m_Data.Sides.Count)
            {
                this.listView1.Items.RemoveAt(m_Data.Sides.Count);
                this.listView1.LargeImageList.Images.RemoveAt(m_Data.Sides.Count);
            }

            // update the icons
            var bg_brush = new SolidBrush(listView1.BackColor);
            var fg_brush = new SolidBrush(listView1.ForeColor);
            var fg_pen = new Pen(fg_brush);
            try
            {
                var i = 0;
                foreach (ListViewItem item in this.listView1.Items)
                {
                    if (item.Tag != null)
                    {
                        var data = (Model.Side)item.Tag;
                        var lil = this.listView1.LargeImageList;
                        if (lil != null)
                        {
                            var image = new Bitmap(lil.ImageSize.Width, lil.ImageSize.Height);
                            var g = Graphics.FromImage(image);
                            var client = new Rectangle(0, 0, image.Width, image.Height);
                            WaferControl.CreateThumbnail(m_Data.StatorStart, data, g, client, 0, bg_brush, fg_pen, fg_brush, this.Font);

#if false
                            //g.SmoothingMode = SmoothingMode.AntiAlias;
                            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            //g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                            // calculate the positions
                            var label_size0 = g.MeasureString("Front", this.Font);
                            var label_size1 = g.MeasureString("Back", this.Font);
                            int label_height = (int)label_size0.Height;
                            int client_height = image.Height / 2;
                            int pad = 4;
                            var client0 = new Rectangle(0, label_height, image.Width, client_height - label_height - pad);
                            var client1 = new Rectangle(0, client_height + label_height + pad, image.Width, client_height - label_height - pad);

                            // draw the entries
                            WaferControl.CreateThumbnail(m_Data.StatorStart, data, g, client0, 0, bg_brush, fg_pen, fg_brush, this.Font);
                            WaferControl.CreateThumbnail(m_Data.StatorStart, data, g, client1, 0, bg_brush, fg_pen, fg_brush, this.Font);

                            // draw the labels
                            g.FillRectangle(bg_brush, (int)((image.Width - label_size0.Width) / 2), 0, label_size0.Width, label_size0.Height);
                            g.DrawString("Front", this.Font, fg_brush, (int)((image.Width - label_size0.Width) / 2), 0);
                            g.FillRectangle(bg_brush, (int)((image.Width - label_size1.Width) / 2), client_height + pad, label_size1.Width, label_size1.Height);
                            g.DrawString("Back", this.Font, fg_brush, (int)((image.Width - label_size1.Width) / 2), client_height + pad);
                            g.Flush();
#endif

                            lil.Images[i] = image;
                        }
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
            if (listView1.Items.Count > 0)
                listView1.RedrawItems(0, listView1.Items.Count - 1, false);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var deck = SelectedItem;
            this.WaferFront.Data = deck;
            if (SelectedItemChanged != null)
                SelectedItemChanged(this);
        }

        public Model.Side SelectedItem
        {
            get
            {
                var lvi = listView1.SelectedItems != null && listView1.SelectedItems.Count > 0 ? listView1.SelectedItems[0] : null;
                return lvi != null ? (Model.Side)lvi.Tag : null;
            }
        }

        public int SelectedItemIndex
        {
            get
            {
                var lvi = listView1.SelectedItems != null && listView1.SelectedItems.Count > 0 ? listView1.SelectedItems[0] : null;
                return lvi != null ? listView1.Items.IndexOf(lvi) : -1;
            }
            set
            {
                if (value < -1 || value >= listView1.Items.Count)
                    throw new ArgumentOutOfRangeException("value");
                listView1.SelectedIndices.Clear();
                if (value != -1)
                    listView1.SelectedIndices.Add(value);
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

            var lvi = listView1.SelectedItems != null && listView1.SelectedItems.Count > 0 ? listView1.SelectedItems[0] : null;
            if (lvi != null)
            {
                int index = listView1.Items.IndexOf(lvi);
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
                this.listViewMenuStrip.Show(this.listView1, e.Location);
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

            // TODO: FIXME: Check to make sure the shaft information is valid
            if (side.Shaft < 0 || side.Shaft >= m_Data.Shafts.Count)
                throw new ArgumentOutOfRangeException("side.Shaft");

            // check if the shaft position is duplicated
            int shaft_position = side.ShaftPosition;
            while (m_Data.Sides.Any((value) => value.ShaftPosition == shaft_position))
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

                            // check the sides
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
