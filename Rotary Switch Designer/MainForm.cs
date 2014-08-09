using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

// TODO:
// - add show contact to spoke
// - draw orientation mark in center of wafer
// - print and print preview
// - see about splitting mid section into two for WV-98C style switches
// - add right click menu (add deck, paste) to blank area of list view
// - support multi-meter style switches
// - about form image
// - application icon (use picture of switch)
// - add multiple shafts
// - visualization of switches at different shaft positions
// - KiCad import
// - Eagle import
// - make GUI look nice
// - 3D view

namespace Rotary_Switch_Designer
{
    public partial class MainForm : Form
    {
        #region Variables
        private string m_Filename = null;
        private string m_Title;
        private Model.Switch m_Data = null;
        private Model.Switch m_StockData = null;
        private List<Action<Model.Switch>> m_UndoBuffer = new List<Action<Model.Switch>>();
        private int m_UndoIndex = 0;
        private int m_UndoSaveIndex = 0;
        private const int m_DefaultShaftPositions = 12;
        private static IList<IWaferExport> m_ExportFormats = new List<IWaferExport>()
        {
            new WaferExportKiCad()
        };
        #endregion

        #region Test Data
        private static Model.Switch TestData()
        {
            // S2A FRONT of RCA/Viz WV-510A
            var data = new Model.Switch();
            data.Shafts.Add(new Model.Shaft() { Detents = 12 });
            data.Sides.Add(new Model.Side());
            data.Sides[0].Positions.Add(new Model.Position() { Spoke = -1 }); //  0
            data.Sides[0].Positions.Add(new Model.Position() { Spoke = -1 }); //  1
            data.Sides[0].Positions.Add(new Model.Position() { Spoke = 2 }); //  2
            data.Sides[0].Positions.Add(new Model.Position() { Spoke = 1 }); //  3
            data.Sides[0].Positions.Add(new Model.Position() { Spoke = 2 }); //  4
            data.Sides[0].Positions.Add(new Model.Position() { Spoke = -1 }); //  5
            data.Sides[0].Positions.Add(new Model.Position() { Spoke = -1 }); //  6
            data.Sides[0].Positions.Add(new Model.Position() { Spoke = 1 }); //  7
            data.Sides[0].Positions.Add(new Model.Position() { Spoke = 2 }); //  8
            data.Sides[0].Positions.Add(new Model.Position() { Spoke = 2 }); //  9
            data.Sides[0].Positions.Add(new Model.Position() { Spoke = 2 }); // 10
            data.Sides[0].Positions.Add(new Model.Position() { Spoke = 2 }); // 11
            data.Sides[0].Positions[0].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = true, EdgeCW = true, Midsection = true });
            data.Sides[0].Positions[0].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = true, Midsection = true });
            data.Sides[0].Positions[1].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = true, EdgeCW = false, Midsection = true });
            data.Sides[0].Positions[1].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = true, EdgeCW = false, Midsection = true });
            data.Sides[0].Positions[2].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = true, Midsection = true });
            data.Sides[0].Positions[2].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = false, Midsection = false });
            data.Sides[0].Positions[3].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = true, EdgeCW = true, Midsection = true });
            data.Sides[0].Positions[3].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = false, Midsection = false });
            data.Sides[0].Positions[4].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = true, EdgeCW = true, Midsection = true });
            data.Sides[0].Positions[4].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = false, Midsection = false });
            data.Sides[0].Positions[5].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = true, EdgeCW = true, Midsection = true });
            data.Sides[0].Positions[5].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = false, Midsection = false });
            data.Sides[0].Positions[6].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = true, EdgeCW = false, Midsection = true });
            data.Sides[0].Positions[6].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = false, Midsection = true });
            data.Sides[0].Positions[7].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = false, Midsection = false });
            data.Sides[0].Positions[7].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = false, Midsection = false });
            data.Sides[0].Positions[8].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = false, Midsection = false });
            data.Sides[0].Positions[8].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = false, Midsection = false });
            data.Sides[0].Positions[9].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = false, Midsection = false });
            data.Sides[0].Positions[9].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = false, Midsection = false });
            data.Sides[0].Positions[10].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = false, Midsection = false });
            data.Sides[0].Positions[10].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = false, Midsection = false });
            data.Sides[0].Positions[11].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = true, Midsection = true });
            data.Sides[0].Positions[11].RotorSlices.Add(new Model.RotorSlice() { EdgeCCW = false, EdgeCW = false, Midsection = false });
            return data;
        }
        #endregion

        #region Constructor
        public MainForm()
        {
            InitializeComponent();
            m_Title = this.Text;
            printPreviewToolStripMenuItem.Visible = false;
            printToolStripMenuItem.Visible = false;
            printToolStripSeparator1.Visible = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // update the title and enable/disable the edit menu entries
            UpdateMenuItems();
        }
        #endregion

        #region Private Functions
        static private Model.Switch LoadDocument(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            var serializer = new XmlSerializer(typeof(Model.Switch));
            using (var fs = new FileStream(filename, FileMode.Open))
            {
                var sw = (Model.Switch)serializer.Deserialize(fs);
                return sw;
            }
        }

        static private void SaveDocument(Model.Switch data, string filename)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (filename == null)
                throw new ArgumentNullException("filename");

            var serializer = new XmlSerializer(typeof(Model.Switch));
            using (var writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, data);
            }
        }
        #endregion

        #region Undo Handling

        private void switchControl1_RequestOperation(object sender, Action<Model.Switch> action)
        {
            Operation(action);
        }

        private void Operation(Action<Model.Switch> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            // execute the action
            action(m_Data);

            // clip the redo entries
            if (m_UndoIndex < m_UndoBuffer.Count)
                m_UndoBuffer.RemoveRange(m_UndoIndex, m_UndoBuffer.Count - m_UndoIndex);

            // invalid the save index if it is in the future and the redo entries are no longer present
            if (m_UndoSaveIndex > m_UndoIndex)
                m_UndoSaveIndex = -1;

            // add the new action to the undo buffer
            m_UndoBuffer.Add(action);
            m_UndoIndex++;

            // update the controls
            UpdateMenuItems();
        }

        private void ResetData(Model.Switch data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            // unsubscribe to the property changed event on the old data
            if (m_Data != null)
                m_Data.PropertyChanged -= m_Data_PropertyChanged;

            // update the data
            m_Data = data;

            // subscribe to the collection changed event on the new data
            m_Data.PropertyChanged += new PropertyChangedEventHandler(m_Data_PropertyChanged);

            // update the undo information
            m_StockData = (Model.Switch)m_Data.CloneObject();
            m_UndoBuffer.Clear();
            m_UndoIndex = 0;
            m_UndoSaveIndex = 0;

            // update the switch control and menu items
            m_Data_PropertyChanged(null, null);
        }

        void m_Data_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (m_Data != null && m_Data.Shafts.Count > 0)
            {
                // update the switch control
                if (switchControl1.Data != m_Data)
                    switchControl1.Data = m_Data;
            }
            else
                switchControl1.Data = null;

            // update the menu items
            UpdateMenuItems();
        }

        public bool CanUndo { get { return m_UndoIndex > 0; } }

        public void SetSavePoint()
        {
            m_UndoSaveIndex = m_UndoIndex;
            UpdateMenuItems();
        }

        public bool CanSave { get { return m_UndoIndex != m_UndoSaveIndex; } }

        private bool SaveAs()
        {
            // prompt the user for the filename
            using (var dialog = new SaveFileDialog() { DefaultExt = ".wafers", Filter = "Wafer Files|*.wafers|All Files|*.*", RestoreDirectory = true })
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // update the filename and save the document
                    m_Filename = dialog.FileName;
                    return Save();
                }
            }
            return false;
        }

        private bool Save()
        {
            if (m_Filename == null)
                return SaveAs();

            // save the document
            SaveDocument(m_Data, m_Filename);

            // update the save point in the undo buffer
            SetSavePoint();
            return true;
        }

        public bool SaveCheck()
        {
            // return true if there is nothing to save
            if (!CanSave)
                return true;

            try
            {
                // Prompt the user to continue if the undo buffer is not empty
                var result = MessageBox.Show("This document has unsaved changes. Do you wish to save before continuing?", "Confirm", MessageBoxButtons.YesNoCancel);
                if (result == System.Windows.Forms.DialogResult.Cancel)
                    return false;
                if (result == System.Windows.Forms.DialogResult.No)
                    return true;

                // save the document
                return Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error saving document: {0}", ex.Message), "Error");
                return false;
            }
        }

        public void Undo()
        {
            if (m_UndoIndex == 0 || m_StockData == null)
                return;

            // reply the undo buffer up to one entry before the undo index
            var new_index = m_UndoIndex - 1;
            var data = (Model.Switch)m_StockData.CloneObject();
            for (int i = 0; i < new_index; ++i)
                m_UndoBuffer[i](data);

            // unsubscribe to the property changed event on the old data
            if (m_Data != null)
                m_Data.PropertyChanged -= m_Data_PropertyChanged;

            // commit
            m_Data = data;

            // subscribe to the collection changed event on the new data
            m_Data.PropertyChanged += new PropertyChangedEventHandler(m_Data_PropertyChanged);
            
            // update the undo index
            m_UndoIndex = new_index;

            // update the switch control and menu items
            m_Data_PropertyChanged(null, null);
        }

        public bool CanRedo { get { return m_UndoIndex < m_UndoBuffer.Count; } }

        public void Redo()
        {
            if (m_UndoIndex == m_UndoBuffer.Count)
                return;

            // reply the next item in the undo list
            m_UndoBuffer[m_UndoIndex++](m_Data);

            // update the menu items
            UpdateMenuItems();
        }

        #endregion

        #region Event Handlers
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Data == null)
                return;

            try
            {
                SaveAs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Could not save file: {0}", ex.Message), "Error Saving File", MessageBoxButtons.OK);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Data == null)
                return;

            try
            {
                Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Could not save file: {0}", ex.Message), "Error Saving File", MessageBoxButtons.OK);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // prompt the user to save if needed
                if (!SaveCheck())
                    return; // cancelled

                using (var dialog = new OpenFileDialog()
                {
                    DefaultExt = ".wafers",
                    Filter = "Wafer Files|*.wafers|All Files|*.*",
                    RestoreDirectory = true
                })
                {
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        // load the document
                        var data = LoadDocument(dialog.FileName);

                        // reset the undo history on the switch control
                        ResetData(data);
                        m_Filename = dialog.FileName;

                        // update the window title
                        UpdateMenuItems();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Could not read file: {0}", ex.Message), "Error Opening File", MessageBoxButtons.OK);
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void UpdateMenuItems()
        {
            undoToolStripMenuItem.Enabled = m_Data != null && CanUndo;
            redoToolStripMenuItem.Enabled = m_Data != null && CanRedo;
            bool item_selected = m_Data != null && this.switchControl1.SelectedItem != null;
            cutToolStripMenuItem.Enabled = item_selected;
            copyToolStripMenuItem.Enabled = item_selected;
            deleteToolStripMenuItem.Enabled = item_selected;
            deckPropertiesToolStripMenuItem.Enabled = item_selected;
            switchPropertiesToolStripMenuItem.Enabled = m_Data != null;
            shaftPropertiesToolStripMenuItem.Enabled = m_Data != null && m_Data.Shafts.Count > 0;
            string file_title = m_Filename != null ? Path.GetFileNameWithoutExtension(m_Filename) : "New Document";
            this.Text = m_Data != null ? string.Format("{0} - {1}{2}", m_Title, file_title, CanSave ? "*" : "") : m_Title;
            exportToolStripMenuItem.Visible = m_ExportFormats != null && m_ExportFormats.Count > 0;
            exportToolStripMenuItem.Enabled = m_Data != null && m_Data.Sides.Count > 0;
            saveToolStripMenuItem.Enabled = m_Data != null && CanSave;
            saveAsToolStripMenuItem.Enabled = m_Data != null && CanSave;
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // prompt the user to save if needed
            if (!SaveCheck())
                return; // cancelled

            // create the new document
            NewDocument();
        }

        private bool NewDocument()
        {
            using (var form = new ShaftForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // create new data
                    var data = new Model.Switch()
                    {
                        StatorStart = 0,
                        RearView = false,
                        TextCCW = false,
                    };
                    data.Shafts.Add(new Model.Shaft()
                    {
                        Detents = form.Detents,
                        DetentStopFirst = form.HasStops ? form.DetentStopFirst : 0,
                        DetentStopCount = form.HasStops ? form.DetentStopCount : 0,
                    });
                    ResetData(data);
                    m_Filename = null;
                    return true;
                }
            }
            return false;
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Data == null)
                return;

            switchControl1.Copy();
            switchControl1.Delete();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Data == null)
                return;

            switchControl1.Copy();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Data == null)
                return;

            switchControl1.Delete();
        }

        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            // enable the paste button if so
            pasteToolStripMenuItem.Enabled = switchControl1.CanPaste;
        }

        private void editToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            // re-enable the paste button so control-v works if control-c is pressed after wards
            pasteToolStripMenuItem.Enabled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // prompt the user to save if needed
            if (!SaveCheck())
                e.Cancel = true;
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (switchControl1.CanPaste && m_Data == null && !NewDocument())
                return;

            switchControl1.Paste();
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

        private void addSideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Data == null && !NewDocument())
                return;

            try
            {
                // find the next letter that is free
                int next_name = 0;
                while (m_Data.Sides.Any((side) => side.Name == ConvertBase26(next_name)))
                    next_name++;

                // find the next shaft position that is free
                int shaft = 0;
                int next_shaft_position = 0;
                while (m_Data.Sides.Any((side) => side.Shaft == shaft && side.ShaftPosition == next_shaft_position))
                    next_shaft_position++;

                // display the side form
                using (var form = new SideForm()
                {
                    ShaftPositions = m_Data.Shafts[0].Detents,
                    OtherIDs = m_Data.Sides.Select((deck) => deck.Name).ToList(),
                    OtherShaftPositions = m_Data.Sides.Select((side) => Tuple.Create(side.ShaftPosition, side.ShaftPositionRear)).ToList(),
                    Shaft = shaft,
                    ShaftPosition = next_shaft_position,
                    ShaftPositionBack = false,
                    ID = ConvertBase26(next_name),
                })
                {
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        // make sure a deck name is valid and doesn't already exist
                        if (string.IsNullOrWhiteSpace(form.Name))
                            throw new Exception("please enter a valid name");
                        if (m_Data.Sides.Any((value) => value.Name == form.Name))
                            throw new Exception("deck with given name already exists");

                        // create the new entry
                        var positions = form.ShaftPositions;
                        var data = new Model.Side()
                        {
                            Name = form.ID,
                            Shaft = form.Shaft,
                            ShaftPosition = form.ShaftPosition,
                            ShaftPositionRear = form.ShaftPositionBack,
                        };
                        for (int j = 0; j < positions; ++j)
                        {
                            var levels = form.Levels;
                            data.Positions.Add(new Model.Position());
                            for (int k = 0; k < levels; ++k)
                                data.Positions[j].RotorSlices.Add(new Model.RotorSlice());
                        }

                        // add it using the paste method
                        switchControl1.Add(data);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(string.Format("Could not create deck: {0}", ex.Message), "Error"); }
        }

        private void switchControl1_SelectedItemChanged(object sender)
        {
            UpdateMenuItems();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var about = new AboutBox())
            {
                about.ShowDialog();
            }
        }

        private void deckPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Data == null)
                return;

            if (switchControl1.SelectedItem != null)
                switchControl1.Properties();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDocument1.DocumentName = m_Filename != null ? Path.GetFileNameWithoutExtension(m_Filename) : "New Document";
            using (var dialog = new PrintPreviewDialog() { Document = printDocument1 })
            {
                dialog.ShowDialog();
            }

        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDocument1.DocumentName = m_Filename != null ? Path.GetFileNameWithoutExtension(m_Filename) : "New Document";
            using (var dialog = new PrintPreviewDialog() { Document = printDocument1 })
            {
                dialog.ShowDialog();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.PageSettings.Landscape = true;
            e.HasMorePages = false;
        }

        private void shaftPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Data == null || m_Data.Shafts.Count == 0)
                return;

            using (var form = new ShaftForm()
            {
                DetentsReadOnly = true,
            })
            {
                if (m_Data.Shafts[0].Detents != 0)
                    form.Detents = m_Data.Shafts[0].Detents;
                if (m_Data.Shafts[0].DetentStopCount != 0)
                {
                    form.DetentStopFirst = m_Data.Shafts[0].DetentStopFirst;
                    form.DetentStopCount = m_Data.Shafts[0].DetentStopCount;
                    form.HasStops = true;
                }
                else
                    form.HasStops = false;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    var copy = new
                    {
                        Detents = form.Detents,
                        HasStops = form.HasStops,
                        DetentStopFirst = form.DetentStopFirst,
                        DetentStopCount = form.DetentStopCount,
                    };

                    Operation((Action<Model.Switch>)((data) =>
                    {
                        // santify checks
                        if (data == null)
                            throw new ArgumentNullException("data");
                        if (data.Shafts == null)
                            throw new Exception("data.Wafers is null");
                        if (data.Shafts.Count == 0)
                            throw new Exception("no shaft");

                        // update the values
                        data.Shafts[0].Detents = copy.Detents;
                        data.Shafts[0].DetentStopFirst = copy.HasStops ? copy.DetentStopFirst : 0;
                        data.Shafts[0].DetentStopCount = copy.HasStops ? copy.DetentStopCount : 0;
                    }));
                }
            }
        }

        private void switchPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Data == null)
                return;

            using (var form = new SwitchForm()
            {
                NumberingStartAngle = m_Data.StatorStart,
                RearView = m_Data.RearView,
                TextCCW = m_Data.TextCCW,
                SymbolName = m_Data.SymbolName,
            })
            {
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    uint start = form.NumberingStartAngle;
                    bool rear = form.RearView;
                    bool ccw = form.TextCCW;
                    string name = form.SymbolName;
                    Operation((Action<Model.Switch>)((data) =>
                    {
                        // santify checks
                        if (data == null)
                            throw new ArgumentNullException("data");

                        // update the data
                        data.StatorStart = start;
                        data.RearView = rear;
                        data.TextCCW = ccw;
                        data.SymbolName = name;
                    }));
                }
            }
        }


        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Data == null || m_ExportFormats == null || m_ExportFormats.Count == 0)
                return;

            try
            {
                using (var dialog = new SaveFileDialog()
                {
                    Title = "Export",
                    Filter = string.Join("|", m_ExportFormats.Select((format) => string.Format("{0}|*.{1}", format.Name, format.Extension))),
                    FilterIndex = 1,
                    OverwritePrompt = true,
                    AutoUpgradeEnabled = true,
                    AddExtension = true,
                    RestoreDirectory = true,
                })
                {
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        // check that the selected item is correct
                        if (dialog.FilterIndex < 1 || dialog.FilterIndex > m_ExportFormats.Count)
                            throw new Exception("Invalid filter index selected");

                        // export the file
                        m_ExportFormats[dialog.FilterIndex - 1].Export(m_Data, dialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error exporting file: {0}", ex.Message), "Error Exporting File");
            }
        }
        #endregion
    }
}
