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
        public delegate void RequestOperationHandler(object sender, Action<Model.Shaft> action);
        public event RequestOperationHandler RequestOperation;
        public delegate void SelectedItemChangedHandler(object sender);
        public event SelectedItemChangedHandler SelectedItemChanged;
        #endregion

        #region Variables
        private bool m_Dirty = false;
        private Model.Shaft m_Data = new Model.Shaft(); // must not be null
        private const string m_ClipboardFormat = "Wafer";
        private uint m_StatorStart = 0;
        #endregion

        public SwitchControl()
        {
            InitializeComponent();
        }

        private void SwitchControl_Load(object sender, EventArgs e)
        {
            this.listView1.LargeImageList = new ImageList();
            this.listView1.LargeImageList.ImageSize = new Size(128, 2 * 128);
        }

        /// <summary>
        /// Indicates the starting location of the stator, in degrees starting from the 12 o'clock position.
        /// </summary>
        public uint StatorStart
        {
            get { return m_StatorStart; }
            set
            {
                WaferFront.StatorStart = value;
                WaferBack.StatorStart = value;
                if (m_StatorStart != value)
                {
                    m_StatorStart = value;
                    data_PropertyChanged(null, null);
                }
            }
        }
        
        public Model.Shaft Data
        {
            get { return m_Data; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                // clear the old callback
                if (m_Data != null)
                    m_Data.PropertyChanged -= data_PropertyChanged;

                // update the value
                m_Data = value;

                // set the new callback
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

                // update the number of detents
                WaferFront.Detents = m_Data.Detents;
                WaferBack.Detents = m_Data.Detents;
            }));
        }

        private void RefreshListViewItems()
        {
            // get the current selected entry
            var selected = listView1.SelectedItems != null && listView1.SelectedItems.Count > 0 ? listView1.SelectedItems[0] : null;

            // check each deck
            for (int i = 0; i < m_Data.Decks.Count; ++i)
            {
                var deck = m_Data.Decks[i];
                if (i >= this.listView1.Items.Count)
                {
                    // add an entry to the list view if there isn't enough for each deck
                    var lvi = new ListViewItem(deck.Name, i);
                    lvi.Tag = deck;
                    this.listView1.Items.Add(lvi);
                    this.listView1.LargeImageList.Images.Add(new Bitmap(1, 1));
                }
                else if (this.listView1.Items[i].Text != deck.Name || this.listView1.Items[i].Tag != deck)
                {
                    // update the existing entry
                    this.listView1.Items[i].Text = deck.Name;
                    this.listView1.Items[i].Tag = deck;

                    // if this is the selected item then update the wafer side views
                    if (selected == this.listView1.Items[i])
                    {
                        this.WaferFront.Data = deck;
                        this.WaferBack.Data = deck;
                    }
                }
            }

            // remove any excess decks
            while (this.listView1.Items.Count > m_Data.Decks.Count)
            {
                this.listView1.Items.RemoveAt(m_Data.Decks.Count);
                this.listView1.LargeImageList.Images.RemoveAt(m_Data.Decks.Count);
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
                        var data = (Model.Deck)item.Tag;
                        var lil = this.listView1.LargeImageList;
                        if (lil != null)
                        {
                            var image = new Bitmap(lil.ImageSize.Width, lil.ImageSize.Height);
                            var g = Graphics.FromImage(image);
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
                            WaferControl.CreateThumbnail(m_Data.Detents, m_StatorStart, data, 0, g, client0, 0, bg_brush, fg_pen, fg_brush, this.Font);
                            WaferControl.CreateThumbnail(m_Data.Detents, m_StatorStart, data, 1, g, client1, 0, bg_brush, fg_pen, fg_brush, this.Font);

                            // draw the labels
                            g.FillRectangle(bg_brush, (int)((image.Width - label_size0.Width) / 2), 0, label_size0.Width, label_size0.Height);
                            g.DrawString("Front", this.Font, fg_brush, (int)((image.Width - label_size0.Width) / 2), 0);
                            g.FillRectangle(bg_brush, (int)((image.Width - label_size1.Width) / 2), client_height + pad, label_size1.Width, label_size1.Height);
                            g.DrawString("Back", this.Font, fg_brush, (int)((image.Width - label_size1.Width) / 2), client_height + pad);
                            g.Flush();

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
            this.WaferBack.Data = deck;
            if (SelectedItemChanged != null)
                SelectedItemChanged(this);
        }

        public Model.Deck SelectedItem
        {
            get
            {
                var lvi = listView1.SelectedItems != null && listView1.SelectedItems.Count > 0 ? listView1.SelectedItems[0] : null;
                return lvi != null ? (Model.Deck)lvi.Tag : null;
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

        private void Operation(Action<Model.Shaft> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            if (RequestOperation != null)
                RequestOperation(this, action);
        }

        private void wafer1_RequestOperation(object sender, Action<Model.Deck,int> action)
        {
            if (sender == null)
                throw new ArgumentNullException("sender");
            if (action == null)
                throw new ArgumentNullException("action");
            var wafer = (WaferControl)sender;

            var lvi = listView1.SelectedItems != null && listView1.SelectedItems.Count > 0 ? listView1.SelectedItems[0] : null;
            if (lvi != null)
            {
                int side = (int)wafer.Side;
                int deck = listView1.Items.IndexOf(lvi);
                if (deck == -1)
                    throw new Exception("invalid listview index");

                Operation((Action<Model.Shaft>)((data) =>
                {
                    // argument checks
                    if (data == null)
                        throw new ArgumentNullException("data");
                    if (deck < 0 || deck >= data.Decks.Count)
                        throw new Exception("invalid deck index");
                    if (side < 0 || side >= data.Decks[deck].Sides.Count)
                        throw new Exception("invalid side index");

                    // perform the action
                    action(data.Decks[deck], side);
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

            Operation((Action<Model.Shaft>)((data) =>
            {
                // santify checks
                if (data == null)
                    throw new ArgumentNullException("data");
                if (data.Decks == null)
                    throw new Exception("data.Decks is null");
                if (index < 0 || index >= data.Decks.Count)
                    throw new ArgumentOutOfRangeException("index");

                // remove the item
                data.Decks.RemoveAt(index);
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

        public void Add(Model.Deck deck)
        {
            if (deck == null)
                throw new ArgumentNullException("wafer");
            if (deck.Sides.Count == 0)
                throw new ArgumentOutOfRangeException("wafer");
            if (m_Data.Decks.Count != 0 &&
                m_Data.Decks[0].Sides.Count != 0 &&
                m_Data.Decks[0].Sides[0].Positions.Count != deck.Sides[0].Positions.Count)
                throw new Exception("different number of positions (detents)");

            // check if the name is duplicated (which it probably will be if the user copied & pasted in the same document)
            string name = deck.Name;
            if (m_Data.Decks.Any((value) => value.Name == deck.Name))
            {
                // find the next letter that is free
                int next_name = 0;
                while (m_Data.Decks.Any((side) => side.Name == ConvertBase26(next_name)))
                    next_name++;

                // update the name with the new one
                name = ConvertBase26(next_name);
            }

            Operation((Action<Model.Shaft>)((data) =>
            {
                // santify checks
                if (data == null)
                    throw new ArgumentNullException("data");
                if (data.Decks == null)
                    throw new Exception("data.Wafers is null");

                // change the name to the randomly assigned one
                var tmp = (Model.Deck)deck.CloneObject();
                tmp.Name = name;

                // add the item
                data.Decks.Add(tmp);
            }));
        }

        public void Copy()
        {
            if (this.SelectedItem == null)
                throw new Exception("no item selected to cut/copy");

            // get the data
            var data = (Model.Deck)this.SelectedItem.CloneObject();

            // convert the data to a string
            string result = null;
            var xs = new XmlSerializer(typeof(Model.Deck));
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
                    Model.Deck data = null;
                    var xs = new XmlSerializer(typeof(Model.Deck));
                    using (var reader = new StringReader(text))
                    {
                        data = (Model.Deck)xs.Deserialize(reader);
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
            if (SelectedItem == null || SelectedItemIndex < 0)
                return;
            if (SelectedItem.Sides.Count == 0)
                return;

            try
            {
                using (var form = new DeckForm()
                {
                    Detents = m_Data.Detents,
                    OtherIDs = m_Data.Decks.Select((deck) => deck.Name).Where((name) => name != SelectedItem.Name).ToList(),
                    ID = SelectedItem.Name,
                    ShaftPositions = (uint)SelectedItem.Sides[0].Positions.Count,
                    FrontLevels = (uint)(SelectedItem.Sides.Count >= 1 ? SelectedItem.Sides[0].RotorLevels() : 0),
                    BackLevels = (uint)(SelectedItem.Sides.Count >= 2 ? SelectedItem.Sides[1].RotorLevels() : 0),
                })
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        var copy = new
                        {
                            Deck = SelectedItemIndex,
                            Name = form.ID,
                            FrontLevels = form.FrontLevels,
                            BackLevels = form.BackLevels,
                        };
                        Operation((Action<Model.Shaft>)((data) =>
                        {
                            // argument checks
                            if (data == null)
                                throw new ArgumentNullException("data");
                            if (copy.Deck >= data.Decks.Count)
                                throw new Exception("invalid deck index");
                            if (data.Decks[copy.Deck] == null)
                                throw new Exception("deck is null");

                            // update the name
                            var deck = data.Decks[copy.Deck];
                            deck.Name = copy.Name;

                            // check the sides
                            for (int i = 0; i < deck.Sides.Count; ++i)
                            {
                                var side = deck.Sides[i];
                                if (side == null)
                                    throw new Exception("side is null");

                                var levels = i == 0 ? copy.FrontLevels : copy.BackLevels;
                                for (int j = 0; j < side.Positions.Count; ++j)
                                {
                                    var position = side.Positions[j];
                                    if (position == null)
                                        throw new Exception("position is null");

                                    // add any new rotor levels
                                    while (position.RotorSlices.Count < levels)
                                        position.RotorSlices.Add(new Model.RotorSlice());

                                    // remove any excess rotor levels
                                    while (position.RotorSlices.Count > levels)
                                        position.RotorSlices.RemoveAt((int)levels);
                                }
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
