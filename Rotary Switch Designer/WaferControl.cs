using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Drawing.Drawing2D;

namespace Rotary_Switch_Designer
{
    public partial class WaferControl : UserControl
    {
        #region Events
        public delegate void RequestOperationHandler(object sender, Action<Model.Side> action);
        public event RequestOperationHandler RequestOperation;
        #endregion

        #region Types
        private enum HitType
        {
            None,
            SpokeHole,
            EdgeCCW,
            Midsection,
            EdgeCW
        }
        #endregion

        #region Variables
        private Model.Side m_Data = null;
        private uint m_RotorPosition = 0;
        private Point? m_MousePosition = null;
        private const double SpokeHoleSize = .02;
        private const double SpokeHoleDistance = 0.75;
        private const double TextDistance = 0.85;
        private const int TextPad = 4;
        private const double RotorMinDistance = 0.3;
        private const double RotorMaxDistance = 0.6;
        private const double MinTextSize = 0.1;
        private const float GapRatio = 0.5f;
        private bool m_dirty = false;
        private HitType m_Hit = HitType.None;
        private int m_HitPosition = -1;
        private int m_HitSlice = -1;
        private Brush m_BgBrush;
        private Pen m_FgPen;
        private Brush m_FgBrush;
        private int m_SpokeHoleHitPosition = -1;
        private uint[,] m_FloodFillColourMap = null;
        private uint m_StatorStart = 0;
        #endregion

        #region Constructor
        public WaferControl()
        {
            InitializeComponent();
            m_BgBrush = new SolidBrush(this.BackColor);
            m_FgPen = new Pen(this.ForeColor);
            m_FgBrush = new SolidBrush(this.ForeColor);
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing && m_BgBrush != null)
            {
                m_BgBrush.Dispose();
                m_BgBrush = null;
                m_FgPen.Dispose();
                m_FgPen = null;
                m_FgBrush.Dispose();
                m_FgBrush = null;
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Indicates the starting location of the stator, in degrees starting from the 12 o'clock position.
        /// </summary>
        public uint StatorStart
        {
            get { return m_StatorStart; }
            set
            {
                if (m_StatorStart != value)
                {
                    m_StatorStart = value;
                    PostRefresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets the wafer data associated with the control.
        /// </summary>
        public Model.Side Data
        {
            get { return m_Data; }
            set
            {
                // remove the changed event from the old data value
                if (m_Data != null)
                    m_Data.PropertyChanged -= m_Data_PropertyChanged;

                // update the value
                m_Data = value;

                // listen to the changed event on the new event
                if (m_Data != null)
                    m_Data.PropertyChanged += new PropertyChangedEventHandler(m_Data_PropertyChanged);

                // clear any hit positioning
                m_Hit = HitType.None;

                // update the property related information
                // this will call redraw
                m_Data_PropertyChanged(m_Data, null);
            }
        }

        /// <summary>
        /// Indicates the current rotor position.
        /// </summary>
        public uint RotorPosition
        {
            get { return m_RotorPosition; }
            set
            {
                if (value != m_RotorPosition)
                {
                    m_RotorPosition = value;
                    PostRefresh();
                }
            }
        }

        public static void CreateThumbnail(uint stator_start, Model.Side data, Graphics g, Rectangle client, uint rotor_position, Brush bg_brush, Pen fg_pen, Brush fg_brush, Font font)
        {
            HitType hit;
            int hit_position;
            int hit_slice;
            Render(data, rotor_position, stator_start, g, bg_brush, fg_pen, fg_brush, false, font, client, null, null, out hit, out hit_position, out hit_slice);
        }

        #endregion

        #region Event Handlers

        static bool GetRotorValue(Model.Side side, int i, int j)
        {
            int index = i / 3;
            if (index >= side.Positions.Count)
                return false;

            var position = side.Positions[index];
            if (j >= position.RotorSlices.Count)
                return false;

            var slice = position.RotorSlices[j];

            switch (i % 3)
            {
                case 0:
                    return slice.EdgeCCW;
                case 1:
                    return slice.Midsection;
                case 2:
                    return slice.EdgeCW;
            }

            return false;
        }

        private static void FloodFill(Model.Side side, uint[,] map, int i, int j, bool value, uint colour)
        {
            if (map[i, j] != 0 || GetRotorValue(side, i,j) != value)
                return; // done

            // set the colour at this location
            // Note that the CCW and CW directions assume that the colour is
            // set before recursion in order to prevent an infinite loop.
            map[i, j] = colour;

            // CCW
            int position_length = map.GetUpperBound(0) + 1;
            FloodFill(side, map, (i + position_length - 1) % position_length, j, value, colour);

            // CW
            FloodFill(side, map, (i + 1) % position_length, j, value, colour);
        }

        /// <summary>
        /// Generate a map of each of the clickable rotor positions, with a different number corresponding to each region.
        /// </summary>
        /// <param name="side">The side data.</param>
        /// <returns>An two dimensional array of clickable rotor positions.</returns>
        /// <remarks>
        /// The return value is an array with the first dimension being the
        /// number of angular positions on the side multiplied by 3 for each
        /// of the CCW, middle and CW sections, repsectively, and the second
        /// dimension being the rotor slices.
        /// 
        /// This is used by the hit tracking and mouse click operation to set
        /// an entire region at a time.
        /// </remarks>
        private static uint[,] GenerateColourMap(Model.Side side)
        {
            if (side == null)
                throw new ArgumentNullException("side");

            var position3 = side.Positions.Count * 3;
            var rotor_levels = side.RotorLevels();
            var map = new uint[position3, rotor_levels];
            uint next_colour = 1;
            for (int i = 0; i < position3; ++i)
            {
                var position = side.Positions[i / 3];
                if (position != null)
                {
                    for (int j = 0; j < position.RotorSlices.Count; ++j)
                    {
                        var slice = position.RotorSlices[j];
                        if (slice != null)
                        {
                            if (map[i, j] == 0)
                            {
                                FloodFill(side, map, i, j, GetRotorValue(side, i, j), next_colour++);
                            }
                        }
                    }
                }
            }

            return map;
        }

        void m_Data_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (m_Data != null)
            {
                // update the flood fill colour map
                m_FloodFillColourMap = GenerateColourMap(m_Data);
            }
            else
                m_FloodFillColourMap = null;

            // refresh the screen
            PostRefresh();
        }

        private void Operation(Action<Model.Side> action)
        {
            if (RequestOperation != null)
                RequestOperation(this, action);
        }

        void PostRefresh()
        {
            if (!this.IsHandleCreated)
                return;
            lock (this)
            {
                m_dirty = true;
            }
            this.BeginInvoke((Action)(() =>
            {
                lock (this)
                {
                    if (!m_dirty)
                        return;
                    m_dirty = false;
                }
                Refresh();
                UpdateToolTip();
            }));
        }

        /// <summary>
        /// Convert from polar to cartesian co-ordinates
        /// </summary>
        private static Point p2c(double angle, double radius, Point center)
        {
            return new Point((int)(Math.Cos(angle) * radius) + center.X, (int)(Math.Sin(angle) * radius) + center.Y);
        }

        /// <summary>
        /// Draw the given side to the graphics device and perform any mouse hit tracking.
        /// </summary>
        /// <remarks>
        /// This method needs to be cleaned up.
        /// </remarks>
        private static void Render(Model.Side data, uint rotor_position_angle, uint stator_start, Graphics g, Brush bg_brush, Pen fg_pen, Brush fg_brush, bool editor_mode, Font font, Rectangle client, Point? mouse, uint[,] fill_map, out HitType hit, out int hit_position, out int hit_slice)
        {
            hit = HitType.None;
            hit_position = -1;
            hit_slice = -1;

            // draw the background
            if (g != null)
                g.FillRectangle(bg_brush, client);

            if (data == null)
                return;

            // test if we should draw the text
            bool labels = false;
            var size = Math.Min(client.Width, client.Height);
            if (g != null && font != null)
            {
                var label_size = g.MeasureString(data.Positions.Count.ToString(), font);
                if (Math.Max(label_size.Width, label_size.Height) < size * MinTextSize)
                    labels = true;
            }

            // convert the shared positions into a bitmap
            var WaferPositions = data.Positions;//.ToList();
            var rotor_levels = data.RotorLevels();
            var center = new Point(client.Left + client.Width / 2, client.Top + client.Height / 2);
            var r = size / 2;
            var AngleOffset = 0.5;
            var RotorLevelSize = (RotorMaxDistance - RotorMinDistance) / rotor_levels;
            var SpokeHoleRadius = (int)(Math.Min(client.Width, client.Height) * SpokeHoleSize);
            uint hit_colour = 0;
            for (int ri = 0; ri < WaferPositions.Count; ++ri)
            {
                var rotor = WaferPositions[ri];
                var ri_ccw = (int)(((uint)ri + (uint)WaferPositions.Count - 1) % (uint)WaferPositions.Count);
                var rotor_ccw = WaferPositions[ri_ccw];
                var ri_cw = (int)(((uint)ri + 1) % (uint)WaferPositions.Count);
                var rotor_cw = WaferPositions[ri_cw];
                if (rotor != null && rotor_ccw != null && rotor_cw != null && rotor.RotorSlices != null && rotor.RotorSlices.Count > 0)
                {
                    var slices = rotor.RotorSlices;
                    for (int j = 0; j < slices.Count; ++j)
                    {
                        var slice = slices[j];

                        //                j-1  j  j+1
                        //  +---+        +---+---+---+
                        //  |   |        |   |   |   |
                        //  |   |        |   |   |   | i-1
                        //  |   |        |   |   |   |
                        //  |   +---+    +---+---+---+
                        //  |       |    |   |   |   |
                        //  |       |    |   |   |   |  i
                        //  |   +---+    |   |   |   |
                        //  +---+        +---+---+---+
                        //  +---+        |   |   |   |
                        //  |   |        |   |   |   | i+1
                        //  |   |        |   |   |   |
                        //  +---+        +---+---+---+
                        //
                        //  +----------- RotorStartRadius
                        //  |
                        //  |      +---- RotorEndRadius
                        //  |      |
                        //  v      v
                        //  +------+ <-- RotorStartAngleDeg
                        //  |      |
                        //  +------+ <-- RotorPreGapAngleDeg
                        //  |      |
                        //  |      |
                        //  |      |
                        //  +------+ <-- RotorPostGapAngleDeg
                        //  |      |
                        //  +------+ <-- RotorEndAngleDeg
                        //
                        var RotorStartRadius = (int)(r * (RotorMinDistance + RotorLevelSize * j));
                        var RotorEndRadius = (int)(r * (RotorMinDistance + RotorLevelSize * (j + 1)));
                        var RotorStartAngleDeg = rotor_position_angle + ((float)ri + (float)AngleOffset - 0.5f) / WaferPositions.Count * 360.0f - 90.0f + stator_start;
                        var RotorPreGapAngleDeg = rotor_position_angle + ((float)ri + (float)AngleOffset - GapRatio / 2) / WaferPositions.Count * 360.0f - 90.0f + stator_start;
                        var RotorPostGapAngleDeg = rotor_position_angle + ((float)ri + (float)AngleOffset + GapRatio / 2) / WaferPositions.Count * 360.0f - 90.0f + stator_start;
                        var RotorEndAngleDeg = rotor_position_angle + ((float)ri + (float)AngleOffset + 0.5f) / WaferPositions.Count * 360.0f - 90.0f + stator_start;

                        using (var p = new GraphicsPath())
                        {
                            // add a polygon for the pre-section
                            p.AddPolygon(new Point[]
                            {
                                p2c(RotorStartAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center),
                                p2c(RotorStartAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center),
                                p2c(RotorPreGapAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center),
                                p2c(RotorPreGapAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center)
                            });

                            // check if the mouse is over the pre-section
                            if (mouse != null && hit == HitType.None && p.IsVisible(mouse.Value))
                            {
                                hit = HitType.EdgeCCW;
                                hit_position = ri;
                                hit_slice = j;
                                if (fill_map != null)
                                    hit_colour = fill_map[ri * 3, j];
                            }
                        }

                        using (var p = new GraphicsPath())
                        {
                            // add a polygon for the mid-section
                            p.AddPolygon(new Point[]
                            {
                                p2c(RotorPreGapAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center),
                                p2c(RotorPreGapAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center),
                                p2c(RotorPostGapAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center),
                                p2c(RotorPostGapAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center)
                            });

                            // check if the mouse is over the mid-section
                            if (mouse != null && hit == HitType.None && p.IsVisible(mouse.Value))
                            {
                                hit = HitType.Midsection;
                                hit_position = ri;
                                hit_slice = j;
                                if (fill_map != null)
                                    hit_colour = fill_map[ri * 3 + 1, j];
                            }
                        }

                        using (var p = new GraphicsPath())
                        {
                            // add a polygon for the post-section
                            p.AddPolygon(new Point[]
                            {
                                p2c(RotorPostGapAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center),
                                p2c(RotorPostGapAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center),
                                p2c(RotorEndAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center),
                                p2c(RotorEndAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center)
                            });

                            // check if the mouse is over the post-section
                            if (mouse != null && hit == HitType.None && p.IsVisible(mouse.Value))
                            {
                                hit = HitType.EdgeCW;
                                hit_position = ri;
                                hit_slice = j;
                                if (fill_map != null)
                                    hit_colour = fill_map[ri * 3 + 2, j];
                            }
                        }
                    }
                }
            }

            for (int ri = 0; ri < WaferPositions.Count; ++ri)
            {
                var rotor = WaferPositions[ri];
                var ri_ccw = (int)(((uint)ri + (uint)WaferPositions.Count - 1) % (uint)WaferPositions.Count);
                var rotor_ccw = WaferPositions[ri_ccw];
                var ri_cw = (int)(((uint)ri + 1) % (uint)WaferPositions.Count);
                var rotor_cw = WaferPositions[ri_cw];
                if (rotor != null && rotor_ccw != null && rotor_cw != null && rotor.RotorSlices != null && rotor.RotorSlices.Count > 0)
                {
                    var slices = rotor.RotorSlices;
                    for (int j = 0; j < slices.Count; ++j)
                    {
                        var slice = slices[j];
                        var RotorStartRadius = (int)(r * (RotorMinDistance + RotorLevelSize * j));
                        var RotorEndRadius = (int)(r * (RotorMinDistance + RotorLevelSize * (j + 1)));
                        var RotorStartAngleDeg = rotor_position_angle + ((float)ri + (float)AngleOffset - 0.5f) / WaferPositions.Count * 360.0f - 90.0f + stator_start;
                        var RotorPreGapAngleDeg = rotor_position_angle + ((float)ri + (float)AngleOffset - GapRatio / 2) / WaferPositions.Count * 360.0f - 90.0f + stator_start;
                        var RotorPostGapAngleDeg = rotor_position_angle + ((float)ri + (float)AngleOffset + GapRatio / 2) / WaferPositions.Count * 360.0f - 90.0f + stator_start;
                        var RotorEndAngleDeg = rotor_position_angle + ((float)ri + (float)AngleOffset + 0.5f) / WaferPositions.Count * 360.0f - 90.0f + stator_start;

                        using (var p = new GraphicsPath())
                        {
                            // add a polygon for the pre-section
                            p.AddPolygon(new Point[]
                            {
                                p2c(RotorStartAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center),
                                p2c(RotorStartAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center),
                                p2c(RotorPreGapAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center),
                                p2c(RotorPreGapAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center)
                            });

                            if (g != null && ((hit == HitType.EdgeCCW && hit_position == ri && hit_slice == j) || (fill_map != null && fill_map[ri * 3, j] == hit_colour)))
                                g.FillPath(SystemBrushes.Highlight, p);
                        }

                        using (var p = new GraphicsPath())
                        {
                            // add a polygon for the mid-section
                            p.AddPolygon(new Point[]
                            {
                                p2c(RotorPreGapAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center),
                                p2c(RotorPreGapAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center),
                                p2c(RotorPostGapAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center),
                                p2c(RotorPostGapAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center)
                            });

                            if (g != null && ((hit == HitType.Midsection && hit_position == ri && hit_slice == j) || (fill_map != null && fill_map[ri * 3 + 1, j] == hit_colour)))
                                g.FillPath(SystemBrushes.Highlight, p);
                        }

                        using (var p = new GraphicsPath())
                        {
                            // add a polygon for the post-section
                            p.AddPolygon(new Point[]
                            {
                                p2c(RotorPostGapAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center),
                                p2c(RotorPostGapAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center),
                                p2c(RotorEndAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center),
                                p2c(RotorEndAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center)
                            });

                            if (g != null && ((hit == HitType.EdgeCW && hit_position == ri && hit_slice == j) || (fill_map != null && fill_map[ri * 3 + 2, j] == hit_colour)))
                                g.FillPath(SystemBrushes.Highlight, p);
                        }
                    }
                }

                // get the position associated with the stator
                var stator = WaferPositions[ri];
                if (stator != null)
                {
                    // outside angle
                    var StatorAngleRad = (stator_start / 360.0 + ((double)ri + AngleOffset) / WaferPositions.Count) * 2.0 * Math.PI - Math.PI / 2;

                    // draw the text
                    if (g != null && labels && (editor_mode || stator.Spoke != -1))
                    {
                        var TextPosition = p2c(StatorAngleRad, r * TextDistance + TextPad, center);
                        var Text = (ri + 1).ToString();
                        var TextSize = g.MeasureString(Text, font, client.Width);
                        TextPosition.Offset(-(int)((TextSize.Width) / 2), -(int)((TextSize.Height) / 2));
                        g.DrawString((ri + 1).ToString(), font, fg_brush, TextPosition);
                    }

                    using (var p = new GraphicsPath())
                    {
                        // add the circle for the spoke hole
                        var SpokePosition = p2c(StatorAngleRad, r * SpokeHoleDistance, center); ;
                        var SpokeHole = new Rectangle(SpokePosition.X - SpokeHoleRadius, SpokePosition.Y - SpokeHoleRadius, SpokeHoleRadius * 2, SpokeHoleRadius * 2);
                        p.AddEllipse(SpokeHole);

                        // draw the spoke hole
                        if (g != null && stator.Spoke != -1)
                            g.DrawPath(fg_pen, p);
                        else if (g != null && editor_mode)
                            g.DrawPath(SystemPens.GrayText, p);

                        // check if the mouse is over the hole for the spoke
                        if (mouse != null && hit == HitType.None && p.IsVisible(mouse.Value))
                        {
                            hit = HitType.SpokeHole;
                            hit_position = ri;
                        }

                        if (g != null)
                        {
                            // draw the selector for the spoke hole
                            if (hit == HitType.SpokeHole && hit_position == ri)
                                g.FillPath(SystemBrushes.Highlight, p);
                            else if (stator.Shared)
                                g.FillPath(fg_brush, p);
                        }
                    }

                    // draw the spoke
                    if (stator.Spoke > 0)
                    {
                        using (var p = new GraphicsPath())
                        {
                            // add the line for the spoke
                            var SpokePosition2 = stator.Spoke - 1;
                            var SpokeLength = Math.Min(SpokePosition2, rotor_levels);
                            var SpokeStart = p2c(StatorAngleRad, r * SpokeHoleDistance - SpokeHoleRadius, center);
                            var SpokeEndRadius = r * (RotorMinDistance + SpokeLength * RotorLevelSize + RotorLevelSize / 2);
                            var SpokeEnd = p2c(StatorAngleRad, SpokeEndRadius, center);
                            p.AddLine(SpokeStart, SpokeEnd);

                            var pen = hit == HitType.SpokeHole && hit_position == ri ? SystemPens.HotTrack : fg_pen;
                            var brush = hit == HitType.SpokeHole && hit_position == ri ? SystemBrushes.HotTrack : fg_brush;

                            // draw the spoke line
                            if (g != null)
                                g.DrawLine(pen, SpokeStart, SpokeEnd);

                            using (var p2 = new GraphicsPath())
                            {
                                // add the arrow head
                                var SpokeHead = p2c(StatorAngleRad, SpokeEndRadius + SpokeHoleRadius, center);
                                var SpokeHeadVector = new Point(SpokeHead.X - SpokeEnd.X, SpokeHead.Y - SpokeEnd.Y);
                                var SpokeHead2a = new Point(SpokeHead.X + SpokeHeadVector.Y, SpokeHead.Y - SpokeHeadVector.X);
                                var SpokeHead2b = new Point(SpokeHead.X - SpokeHeadVector.Y, SpokeHead.Y + SpokeHeadVector.X);
                                p2.AddPolygon(new Point[] { SpokeHead2a, SpokeEnd, SpokeHead2b, SpokeHead2a });

                                if (g != null)
                                    g.FillPath(brush, p2);
                            }
                        }
                    }
                }

                if (rotor != null && rotor_ccw != null && rotor_cw != null && rotor.RotorSlices != null && rotor.RotorSlices.Count > 0)
                {
                    var slices = rotor.RotorSlices;
                    for (int j = 0; j < slices.Count; ++j)
                    {
                        var slice = slices[j];
                        var RotorStartRadius = (int)(r * (RotorMinDistance + RotorLevelSize * j));
                        var RotorEndRadius = (int)(r * (RotorMinDistance + RotorLevelSize * (j + 1)));
                        var RotorStartAngleDeg = rotor_position_angle + ((float)ri + (float)AngleOffset - 0.5f) / WaferPositions.Count * 360.0f - 90.0f + stator_start;
                        var RotorPreGapAngleDeg = rotor_position_angle + ((float)ri + (float)AngleOffset - GapRatio / 2) / WaferPositions.Count * 360.0f - 90.0f + stator_start;
                        var RotorPostGapAngleDeg = rotor_position_angle + ((float)ri + (float)AngleOffset + GapRatio / 2) / WaferPositions.Count * 360.0f - 90.0f + stator_start;
                        var RotorEndAngleDeg = rotor_position_angle + ((float)ri + (float)AngleOffset + 0.5f) / WaferPositions.Count * 360.0f - 90.0f + stator_start;

                        if (g != null)
                        {
                            if (slice.EdgeCCW)
                            {
                                // start (ccw) vertical edge
                                if (rotor_ccw.RotorSlices == null || j >= rotor_ccw.RotorSlices.Count || !rotor_ccw.RotorSlices[j].EdgeCW)
                                    g.DrawLine(fg_pen, p2c(RotorStartAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center), p2c(RotorStartAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center));

                                // inner pre gap horizontal edge
                                if (j == 0 || slices[j - 1] == null || !slices[j - 1].EdgeCCW)
                                {
                                    var box = new Rectangle(center.X - RotorStartRadius, center.Y - RotorStartRadius, RotorStartRadius * 2, RotorStartRadius * 2);
                                    g.DrawArc(fg_pen, box, RotorStartAngleDeg, RotorPreGapAngleDeg - RotorStartAngleDeg);
                                }

                                // outer pre gap horizontal edge
                                if (j == (slices.Count - 1) || slices[j + 1] == null || !slices[j + 1].EdgeCCW)
                                {
                                    var box = new Rectangle(center.X - RotorEndRadius, center.Y - RotorEndRadius, RotorEndRadius * 2, RotorEndRadius * 2);
                                    g.DrawArc(fg_pen, box, RotorStartAngleDeg, RotorPreGapAngleDeg - RotorStartAngleDeg);
                                }
                            }

                            if ((slice.EdgeCCW && !slice.Midsection) || (!slice.EdgeCCW && slice.Midsection))
                            {
                                // pre gap (ccw) vertical edge
                                g.DrawLine(fg_pen, p2c(RotorPreGapAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center), p2c(RotorPreGapAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center));
                            }

                            if (slice.Midsection)
                            {
                                // inner horizontal edge
                                if (j == 0 || slices[j - 1] == null || !slices[j - 1].Midsection)
                                {
                                    var box = new Rectangle(center.X - RotorStartRadius, center.Y - RotorStartRadius, RotorStartRadius * 2, RotorStartRadius * 2);
                                    g.DrawArc(fg_pen, box, RotorPreGapAngleDeg, RotorPostGapAngleDeg - RotorPreGapAngleDeg);
                                }

                                // outer horizonal edge
                                if (j == (slices.Count - 1) || slices[j + 1] == null || !slices[j + 1].Midsection)
                                {
                                    var box = new Rectangle(center.X - RotorEndRadius, center.Y - RotorEndRadius, RotorEndRadius * 2, RotorEndRadius * 2);
                                    g.DrawArc(fg_pen, box, RotorPreGapAngleDeg, RotorPostGapAngleDeg - RotorPreGapAngleDeg);
                                }
                            }

                            if ((slice.EdgeCW && !slice.Midsection) || (!slice.EdgeCW && slice.Midsection))
                            {
                                // post gap (cw) vertical edge
                                g.DrawLine(fg_pen, p2c(RotorPostGapAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center), p2c(RotorPostGapAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center));
                            }

                            if (slice.EdgeCW)
                            {
                                // inner post gap horizontal edge
                                if (j == 0 || slices[j - 1] == null || !slices[j - 1].EdgeCW)
                                {
                                    var box = new Rectangle(center.X - RotorStartRadius, center.Y - RotorStartRadius, RotorStartRadius * 2, RotorStartRadius * 2);
                                    g.DrawArc(fg_pen, box, RotorPostGapAngleDeg, RotorEndAngleDeg - RotorPostGapAngleDeg);
                                }

                                // outer post gap horizontal edge
                                if (j == (slices.Count - 1) || slices[j + 1] == null || !slices[j + 1].EdgeCW)
                                {
                                    var box = new Rectangle(center.X - RotorEndRadius, center.Y - RotorEndRadius, RotorEndRadius * 2, RotorEndRadius * 2);
                                    g.DrawArc(fg_pen, box, RotorPostGapAngleDeg, RotorEndAngleDeg - RotorPostGapAngleDeg);
                                }

                                // end (cw) vertical edge
                                if (rotor_cw.RotorSlices == null || j >= rotor_cw.RotorSlices.Count || !rotor_cw.RotorSlices[j].EdgeCCW)
                                    g.DrawLine(fg_pen, p2c(RotorEndAngleDeg / 360.0 * 2 * Math.PI, RotorStartRadius, center), p2c(RotorEndAngleDeg / 360.0 * 2 * Math.PI, RotorEndRadius, center));
                            }
                        }
                    }
                }
            }
        }

        private void Wafer_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                var g = e.Graphics;

                // check if control is pressed
                var modifiers = Control.ModifierKeys;
                bool fill = (modifiers & Keys.Control) != 0;

                // draw the control
                Render(m_Data, this.RotorPosition, m_StatorStart, g, m_BgBrush, m_FgPen, m_FgBrush, true, this.Font, this.ClientRectangle, m_MousePosition, fill ? m_FloodFillColourMap : null, out m_Hit, out m_HitPosition, out m_HitSlice);

                // draw a border in design mode
                if (this.DesignMode)
                {
                    using (var pen = new Pen(this.ForeColor))
                    {
                        pen.DashStyle = DashStyle.Dash;
                        g.DrawRectangle(pen, this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
                    }
                }
            }
            finally { }
        }

        private void Wafer_MouseMove(object sender, MouseEventArgs e)
        {
            m_MousePosition = e.Location;
            PostRefresh();
        }

        private void WaferControl_MouseLeave(object sender, EventArgs e)
        {
            m_MousePosition = null;
            PostRefresh();
        }

        private void WaferControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_Data == null)
                return;
            if (!this.Focused)
                return;

            // check if control is pressed
            var modifiers = Control.ModifierKeys;
            bool fill = (modifiers & Keys.Control) != 0;

            // update the hit position
            m_MousePosition = e.Location;
            Render(m_Data, this.RotorPosition, m_StatorStart, null, m_BgBrush, m_FgPen, m_FgBrush, true, this.Font, this.ClientRectangle, this.m_MousePosition, fill ? m_FloodFillColourMap : null, out m_Hit, out m_HitPosition, out m_HitSlice);

            // check if the user clicked on the spoke
            if (m_Hit == HitType.SpokeHole && m_HitPosition != -1)
            {
                // re-create the menu items
                SpokeMenuStrip.Items.Clear();
                var position = m_Data.Positions[m_HitPosition];
                SpokeMenuStrip.Items.Add(new ToolStripMenuItem("Delete") { Tag = -1 });
                SpokeMenuStrip.Items.Add(new ToolStripMenuItem("Contact Only") { Tag = 0 });
                var rotor_levels = m_Data.RotorLevels();
                switch (rotor_levels)
                {
                    case 0:
                        break; // do nothing
                    case 1:
                        SpokeMenuStrip.Items.Add(new ToolStripMenuItem("Add") { Tag = 1 });
                        break;
                    case 2:
                        SpokeMenuStrip.Items.Add(new ToolStripMenuItem("Inner") { Tag = 1 });
                        SpokeMenuStrip.Items.Add(new ToolStripMenuItem("Outer") { Tag = 2 });
                        break;
                    default:
                        SpokeMenuStrip.Items.Add(new ToolStripMenuItem("Position 1 (Inner)") { Tag = 1 });
                        for (int i = 1; i < (rotor_levels - 1); ++i)
                            SpokeMenuStrip.Items.Add(new ToolStripMenuItem(string.Format("Position {0}", i + 1)) { Tag = i + 1 });
                        SpokeMenuStrip.Items.Add(new ToolStripMenuItem(string.Format("Position {0} (Outer)", rotor_levels)) { Tag = rotor_levels });
                        break;
                }
                SpokeMenuStrip.Items.Add(new ToolStripMenuItem("Shared") { Tag = -2, Checked = position.Shared });

                // enable/disable the menu items based on the current spoke position
                foreach (ToolStripItem item in SpokeMenuStrip.Items)
                    item.Enabled = position.Spoke != (int)item.Tag;

                // show the menu and exit
                m_SpokeHoleHitPosition = m_HitPosition;
                SpokeMenuStrip.Show(this, e.Location);
                return;
            }

            // check if anything was hit
            if (m_Hit != HitType.None)
            {
                if (!fill)
                {
                    // normal click, queue the action
                    HitType hit = m_Hit; // copy for lambda closure
                    int hit_position = m_HitPosition; // copy for lambda closure
                    int hit_slice = m_HitSlice; // copy for lambda closure
                    Operation((Action<Model.Side>)((data) =>
                    {
                        // sanity checks
                        if (data == null)
                            throw new ArgumentNullException("data");
                        if (data.Positions == null)
                            throw new Exception("data.Positions is null");
                        if (hit_position >= data.Positions.Count)
                            throw new Exception("invalid hit position");
                        var wafer_position = data.Positions[hit_position];

                        // make sure the rotor slices exist
                        while (hit_slice >= wafer_position.RotorSlices.Count)
                            wafer_position.RotorSlices.Add(new Model.RotorSlice());

                        // perform the operation
                        var rotor = wafer_position.RotorSlices[hit_slice];
                        switch (hit)
                        {
                            case HitType.EdgeCCW:
                                rotor.EdgeCCW = !rotor.EdgeCCW;
                                break;
                            case HitType.Midsection:
                                rotor.Midsection = !rotor.Midsection;
                                break;
                            case HitType.EdgeCW:
                                rotor.EdgeCW = !rotor.EdgeCW;
                                break;
                            default:
                                throw new Exception("invalid hit type");
                        }
                    }));
                }
                else
                {
                    // get the hit colour
                    var colour_map = (uint[,])m_FloodFillColourMap.Clone(); // copy for lambda closure
                    uint fill_colour = 0;
                    switch (m_Hit)
                    {
                        case HitType.EdgeCCW:
                            fill_colour = colour_map[m_HitPosition * 3, m_HitSlice];
                            break;
                        case HitType.Midsection:
                            fill_colour = colour_map[m_HitPosition * 3 + 1, m_HitSlice];
                            break;
                        case HitType.EdgeCW:
                            fill_colour = colour_map[m_HitPosition * 3 + 2, m_HitSlice];
                            break;
                        default:
                            throw new Exception("invalid hit type");
                    }

                    // flood full, queue the operation
                    Operation((Action<Model.Side>)((data) =>
                    {
                        // sanity checks
                        if (data == null)
                            throw new ArgumentNullException("data");
                        if (data.Positions == null)
                            throw new Exception("data.Positions is null");

                        for (int i = 0; i < data.Positions.Count; ++i)
                        {
                            var position = data.Positions[i];
                            if (position != null)
                            {
                                for (int j = 0; j < position.RotorSlices.Count; ++j)
                                {
                                    var rotor = position.RotorSlices[j];
                                    if (rotor != null)
                                    {
                                        if (colour_map[i * 3, j] == fill_colour)
                                            rotor.EdgeCCW = !rotor.EdgeCCW;
                                        if (colour_map[i * 3 + 1, j] == fill_colour)
                                            rotor.Midsection = !rotor.Midsection;
                                        if (colour_map[i * 3 + 2, j] == fill_colour)
                                            rotor.EdgeCW = !rotor.EdgeCW;
                                    }
                                }
                            }
                        }
                    }));
                }
            }
        }

        private void SpokeMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (m_SpokeHoleHitPosition != -1)
            {
                int index = (int)e.ClickedItem.Tag;
                int position = m_SpokeHoleHitPosition; // make a copy for the lambda closure
                Operation((Action<Model.Side>)((data) =>
                {
                    if (data == null)
                        throw new ArgumentNullException("data");
                    if (data.Positions == null)
                        throw new Exception("data.Positions is null");
                    if (position >= data.Positions.Count)
                        throw new Exception("invalid position");

                    if (index == -2)
                    {
                        // toggle shared
                        data.Positions[position].Shared = !data.Positions[position].Shared;

                        // make sure the contact is visible
                        if (data.Positions[position].Shared && data.Positions[position].Spoke == -1)
                            data.Positions[position].Spoke = 0; // set to "Contact Only"
                    }
                    else
                    {
                        // set spoke position
                        data.Positions[position].Spoke = index;

                        // disable the shared flag if it's not visible
                        if (index == -1)
                            data.Positions[position].Shared = false;
                    }
                }));
            }
        }

        private string m_ToolTipCurrentText = null;
        private void UpdateToolTip()
        {
            if (m_Data == null)
                return;

            // show the tool tip
            string text = null;
            switch (m_Hit)
            {
                case HitType.SpokeHole:
                    text = "Click to set or clear the spoke at this position.";
                    break;
                case HitType.EdgeCCW:
                    text = "Click to set or clear the CCW edge of the rotor contact at this position.  Hold the Control key while clicking to fill an area.";
                    break;
                case HitType.Midsection:
                    text = "Click to set or clear the middle section of the rotor contact at this position.  Hold the Control key while clicking to fill an area.";
                    break;
                case HitType.EdgeCW:
                    text = "Click to set or clear the CW edge of the rotor contact at this position.  Hold the Control key while clicking to fill an area.";
                    break;
            }

            if (text != m_ToolTipCurrentText)
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    //System.Diagnostics.Debug.Print("Displaying tooltip: {0}", text);
                    this.toolTip1.Hide(this);
                    this.toolTip1.SetToolTip(this, text);
                }
                else
                    this.toolTip1.SetToolTip(this, null);
                m_ToolTipCurrentText = text;
            }
        }

        private void WaferControl_KeyDown(object sender, KeyEventArgs e)
        {
            PostRefresh();
        }

        private void WaferControl_KeyUp(object sender, KeyEventArgs e)
        {
            PostRefresh();
        }
        #endregion
    }
}
