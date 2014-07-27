using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;

namespace Rotary_Switch_Designer
{
    namespace Model
    {
        /// <summary>
        /// Represents a vertical slice at a given wafer position.
        /// </summary>
        [XmlTypeAttribute("RotorSlice")]
        public class RotorSlice : INotifyPropertyChanged
        {
            #region Events
            public event PropertyChangedEventHandler PropertyChanged;
            #endregion

            #region Variables
            private bool m_EdgeCCW = false;
            private bool m_Midsection = false;
            private bool m_EdgeCW = false;
            #endregion

            #region Properties

            /// <summary>
            /// Indicates if there is a slice on the counter clockwise side of this position.
            /// </summary>
            [XmlAttribute]
            [DefaultValueAttribute(false)]
            public bool EdgeCCW
            {
                get { return m_EdgeCCW; }
                set
                {
                    if (value != m_EdgeCCW)
                    {
                        m_EdgeCCW = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("EdgeCCW"));
                    }
                }
            }

            /// <summary>
            /// Indicates if the slice exists at the middle of this position.
            /// </summary>
            [XmlAttribute]
            [DefaultValueAttribute(false)]
            public bool Midsection
            {
                get { return m_Midsection; }
                set
                {
                    if (value != m_Midsection)
                    {
                        m_Midsection = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("Midsection"));
                    }
                }
            }

            /// <summary>
            /// Indicates if there is a slice on the clockwise side of this position.
            /// </summary>
            [XmlAttribute]
            [DefaultValueAttribute(false)]
            public bool EdgeCW
            {
                get { return m_EdgeCW; }
                set
                {
                    if (value != m_EdgeCW)
                    {
                        m_EdgeCW = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("EdgeCW"));
                    }
                }
            }

            public override string ToString()
            {
                return string.Format("{ ccw={0}, mid={1}, cw={2} }", m_EdgeCCW, m_Midsection, m_EdgeCW);
            }

            #endregion
        }

        /// <summary>
        /// Represents a single switch position of the wafer.
        /// </summary>
        /// <remarks>
        /// The positions are all assumed to be referenced from a RotorPosition
        /// of 0.
        /// </remarks>
        public class Position : INotifyPropertyChanged
        {
            #region Events
            public event PropertyChangedEventHandler PropertyChanged;
            #endregion

            #region Variables
            private int m_Spoke = -1;
            private ObservableCollection<RotorSlice> m_RotorSlices = new ObservableCollection<RotorSlice>();
            private bool m_Shared = false;
            #endregion

            #region Constructor
            public Position()
            {
                m_RotorSlices.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(m_RotorSlices_CollectionChanged);
            }

            private void m_RotorSlices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                if (e.NewItems != null)
                {
                    foreach (RotorSlice item in e.NewItems)
                    {
                        item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (RotorSlice item in e.OldItems)
                    {
                        item.PropertyChanged -= item_PropertyChanged;
                    }
                }
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("RotorSlices"));
            }

            private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("RotorSlices"));
            }
            #endregion

            #region Properties

            /// <summary>
            /// Indicates the index of the spoke connecting to the rotor.
            /// </summary>
            /// <remarks>
            /// Set to -1 if there isn't a spoke at this switch detent, 0 if
            /// the contact should be drawn but no spoke, and 1 and above for
            /// starting at the innermost position.
            /// </remarks>
            [XmlAttribute]
            [DefaultValue(-1)]
            public int Spoke
            {
                get { return m_Spoke; }
                set
                {
                    if (value != m_Spoke)
                    {
                        m_Spoke = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("Spoke"));
                    }
                }
            }

            /// <summary>
            /// Indicates that the contact of this position may be shared with the opposite side.
            /// </summary>
            /// <remarks>
            /// Both this side and the opposite side must have this value set
            /// for the contact to be shared.
            /// </remarks>
            [XmlAttribute]
            [DefaultValue(false)]
            public bool Shared
            {
                get { return m_Shared; }
                set
                {
                    if (value != m_Shared)
                    {
                        m_Shared = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("Shared"));
                    }
                }
            }

            /// <summary>
            /// Contains the map of rotor slices at this switch position.
            /// </summary>
            public ObservableCollection<RotorSlice> RotorSlices { get { return m_RotorSlices; } }

            #endregion
        }

        /// <summary>
        /// Represents a collection of wafer positions of a single wafer side.
        /// </summary>
        public class Side : INotifyPropertyChanged
        {
            #region Events
            public event PropertyChangedEventHandler PropertyChanged;
            #endregion

            #region Variables
            private ObservableCollection<Position> m_Positions = new ObservableCollection<Position>();
            private string m_Name = "";
            private int m_Shaft = -1;
            private int m_ShaftPosition = -1;
            private bool m_ShaftPositionRear = false;
            #endregion

            #region Constructor / Event Handlers
            public Side()
            {
                m_Positions.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(m_WaferPositions_CollectionChanged);
            }

            private void m_WaferPositions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                if (e.NewItems != null)
                {
                    foreach (Position item in e.NewItems)
                    {
                        item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (Position item in e.OldItems)
                    {
                        item.PropertyChanged -= item_PropertyChanged;
                    }
                }
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Positions"));
            }

            private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Positions"));
            }

            #endregion

            #region Properties

            /// <summary>
            /// Indicates the position dependent wafer data.
            /// </summary>
            public ObservableCollection<Position> Positions { get { return m_Positions; } }

            /// <summary>
            /// Returns the name of the side for the user, such as "A-FRONT".
            /// </summary>
            [XmlAttribute]
            public string Name
            {
                get { return m_Name; }
                set
                {
                    if (value != m_Name)
                    {
                        m_Name = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    }
                }
            }

            /// <summary>
            /// The shaft number, starting from 0.
            /// </summary>
            /// <remarks>
            /// Currently only a single shaft is supported.
            /// </remarks>
            [XmlAttribute]
            public int Shaft
            {
                get { return m_Shaft; }
                set
                {
                    if (value != m_Shaft)
                    {
                        m_Shaft = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("Shaft"));
                    }
                }
            }

            /// <summary>
            /// The shaft position number, starting from 0 at the front.
            /// </summary>
            [XmlAttribute]
            public int ShaftPosition
            {
                get { return m_ShaftPosition; }
                set
                {
                    if (value != m_ShaftPosition)
                    {
                        m_ShaftPosition = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("ShaftPosition"));
                    }
                }
            }

            /// <summary>
            /// Indicates the side resides on the back of the given shaft position.
            /// </summary>
            [XmlAttribute]
            [DefaultValue(false)]
            public bool ShaftPositionRear
            {
                get { return m_ShaftPositionRear; }
                set
                {
                    if (value != m_ShaftPositionRear)
                    {
                        m_ShaftPositionRear = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("ShaftPositionRear"));
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Represents a collection of wafers connected by a single shaft.
        /// </summary>
        [XmlRootAttribute(IsNullable = false)]
        public class Shaft : INotifyPropertyChanged
        {
            #region Events
            public event PropertyChangedEventHandler PropertyChanged;
            #endregion

            #region Variables
            private uint m_Detents = 1;
            private int m_DetentStopFirst = 1;
            private int m_DetentStopCount = 0;
            private uint m_FlatAngle;
            #endregion

            #region Constructor / Event Handlers
            public Shaft()
            {
            }
            #endregion

            #region Properties

            /// <summary>
            /// Represents the number of angular stops or positions on the shaft.
            /// </summary>
            [XmlAttribute]
            public uint Detents
            {
                get { return m_Detents; }
                set
                {
                    if (value != m_Detents)
                    {
                        m_Detents = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("Detents"));
                    }
                }
            }

            /// <summary>
            /// Represents the first detent of the stop, starting from 1 as the start position.
            /// </summary>
            [XmlAttribute]
            [DefaultValue(1)]
            public int DetentStopFirst
            {
                get { return m_DetentStopFirst; }
                set
                {
                    if (value != m_DetentStopFirst)
                    {
                        m_DetentStopFirst = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("DetentStopFirst"));
                    }
                }
            }

            /// <summary>
            /// Represents the number of detents of the stop, or 0 if continuous.
            /// </summary>
            [XmlAttribute]
            [DefaultValue(0)]
            public int DetentStopCount
            {
                get { return m_DetentStopCount; }
                set
                {
                    if (value != m_DetentStopCount)
                    {
                        m_DetentStopCount = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("DetentStopCount"));
                    }
                }
            }

            /// <summary>
            /// Represents the position of the flat on the shaft.
            /// </summary>
            [XmlAttribute]
            [DefaultValue(0)]
            public uint FlatAngle
            {
                get { return m_FlatAngle; }
                set
                {
                    if (value != FlatAngle)
                    {
                        m_FlatAngle = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("FlatAngle"));
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Represents a wafer switch containing one or more shafts.
        /// </summary>
        /// <remarks>
        /// Currently only a single shaft is supported by the program.
        /// </remarks>
        public class Switch : INotifyPropertyChanged
        {
            #region Events
            public event PropertyChangedEventHandler PropertyChanged;
            #endregion

            #region Variables
            private uint m_StatorStart = 0;
            private ObservableCollection<Side> m_Sides = new ObservableCollection<Side>();
            private ObservableCollection<Shaft> m_Shafts = new ObservableCollection<Shaft>();
            #endregion

            #region Constructor / Event Handlers
            public Switch()
            {
                m_Sides.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(m_Sides_CollectionChanged);
                m_Shafts.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(m_Shafts_CollectionChanged);
            }

            private void m_Shafts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                if (e.NewItems != null)
                {
                    foreach (Shaft item in e.NewItems)
                    {
                        item.PropertyChanged += new PropertyChangedEventHandler(Shaft_item_PropertyChanged);
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (Shaft item in e.OldItems)
                    {
                        item.PropertyChanged -= Shaft_item_PropertyChanged;
                    }
                }
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Shafts"));
            }

            private void Shaft_item_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Shafts"));
            }

            private void m_Sides_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                if (e.NewItems != null)
                {
                    foreach (Side item in e.NewItems)
                    {
                        item.PropertyChanged += new PropertyChangedEventHandler(Side_item_PropertyChanged);
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (Side item in e.OldItems)
                    {
                        item.PropertyChanged -= Side_item_PropertyChanged;
                    }
                }
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Sides"));
            }

            private void Side_item_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Sides"));
            }
            #endregion

            #region Properties

            /// <summary>
            /// Indicates the starting location of the stator, in degrees starting from the 12 o'clock position.
            /// </summary>
            [XmlAttribute]
            [DefaultValueAttribute(0u)]
            public uint StatorStart
            {
                get { return m_StatorStart; }
                set
                {
                    if (value != m_StatorStart)
                    {
                        m_StatorStart = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("StatorStart"));
                    }
                }
            }

            /// <summary>
            /// Represents all of the sides within the switch.
            /// </summary>
            public ObservableCollection<Side> Sides { get { return m_Sides; } }

            /// <summary>
            /// Represents all of the shafts of a switch.
            /// </summary>
            public ObservableCollection<Shaft> Shafts { get { return m_Shafts; } }

            #endregion
        }
    }
}