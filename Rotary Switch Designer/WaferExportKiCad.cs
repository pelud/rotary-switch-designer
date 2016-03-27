using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Rotary_Switch_Designer
{
    public class WaferExportKiCad : IWaferExport
    {
        private const string m_file_template =
@"EESchema-LIBRARY Version 2.3  Date: {0}
#encoding utf-8
{1}#
#End Library";

        private const string m_line_end = @"
";

        public string Name { get { return "KiCad Library"; } }
        public string Extension { get { return "lib"; } }

        class KiCadGraphics : IGraphics
        {
            private IList<string> m_result = new List<string>();

            public string Name { get; set; }
            public int Unit { get; set; }
            public int TextSize { get; set; }
            public Rectangle Client { get; set; }

            public KiCadGraphics()
            {
                Unit = 0;
                TextSize = 60;
            }

            public Size MeasureString(string text)
            {
                return new Size(TextSize * text.Length, TextSize);
            }

            public void DrawString(string text, Point TextPosition)
            {
                m_result.Add(Text(TransformPosition(TextPosition), false, text));
            }

            public void DrawCircle(Point center, int radius)
            {
                m_result.Add(Circle(TransformPosition(center), radius, false));
            }

            public void FillCircle(Point center, int radius, bool highlight)
            {
                m_result.Add(Circle(TransformPosition(center), radius, true));
            }

            public void DrawPolygon(Point[] points)
            {
                m_result.Add(Polygon(points.Select((point) => TransformPosition(point)).ToArray(), false));
            }

            public void FillPolygon(Point[] points, bool highlight)
            {
                m_result.Add(Polygon(points.Select((point) => TransformPosition(point)).ToArray(), true));
            }

            public void DrawLine(Point start, Point end)
            {
                m_result.Add(Polygon(new Point[] { TransformPosition(start), TransformPosition(end) }, false));
            }

            public void DrawArc(Point center, int radius, float start_angle, float sweep_angle)
            {
                m_result.Add(Arc(TransformPosition(center), radius, TransformAngle(start_angle), -sweep_angle, false));
            }

            public void AddPin(Point center, int number)
            {
                m_result.Add(Pin(TransformPosition(center), number, "~"));
            }

            private Point TransformPosition(Point position)
            {
                var center = this.Client.Y + this.Client.Height / 2;
                position.Y = position.Y - center;
                position.Y = -position.Y;
                position.Y = position.Y + center;
                return position;
            }

            private float TransformAngle(float angle)
            {
                return 360 - angle;
            }

            private string Text(Point position, bool vertical, string text)
            {
                //T orientation posx posy dimension unit convert Text
                //With:
                //• orientation = horizontal orientation (=0) or vertical (=1).
                //• type = always 0.
                //• unit = 0 if common to the parts. If not, the number of the part (1. .n).
                //• convert = 0 if common to the representations, if not 1 or 2.
                //throw new NotImplementedException();
                // i.e. T 0 -350 100 60 0 2 0 b  Normal 0 C C
                return string.Format("T {0} {1} {2} {3} {4} {5} {6}",
                    vertical ? 1 : 0,
                    position.X, position.Y,
                    this.TextSize,
                    this.Unit,
                    0,
                    text);
            }

            private string Circle(Point center, int radius, bool filled)
            {
                //C posx posy radius unit convert thickness cc
                //With
                //• posx posy = circle center position
                //• unit = 0 if common to the parts; if not, number of part (1. .n).
                //• convert = 0 if common to all parts. If not, number of the part (1. .n).
                //• thickness = thickness of the outline.
                //• cc = N F or F ( F = filled circle,; f = . filled circle, N = transparent background)
                // i.e. C 100 150 50 5 0 0 F
                return string.Format("C {0} {1} {2} {3} {4} {5} {6}",
                    center.X, center.Y,
                    radius,
                    this.Unit,
                    0,
                    0,
                    filled ? "F" : "N");
            }

            private string Arc(Point center, int radius, float start_angle, float sweep_angle, bool filled)
            {
                //A posx posy radius start end part convert thickness cc start_pointX start_pointY end_pointX end_pointY.
                //With:
                //• posx posy = arc center position
                //• start = angle of the starting point (in 0,1 degrees).
                //• end = angle of the end point (in 0,1 degrees).
                //• unit = 0 if common to all parts; if not, number of the part (1. .n).
                //• convert = 0 if common to the representations, if not 1 or 2.
                //• thickness = thickness of the outline or 0 to use the default line thickness.
                //• cc = N F or F ( F = filled arc,; f = . filled arc, N = transparent background)
                //• start_pointX start_pointY = coordinate of the starting point (role similar to start)
                // i.e. A 0 0 200 -1799 -1 4 0 0 N -200 0 200 0
                int start = (int)Math.Round(start_angle * 10);
                int end = (int)Math.Round((start_angle + sweep_angle) * 10) % 3600;
                Point start_point = new Point(
                    (int)Math.Round(center.X + Math.Cos(start_angle / 180 * Math.PI) * radius),
                    (int)Math.Round(center.Y + Math.Sin(start_angle / 180 * Math.PI) * radius));
                Point end_point = new Point(
                    (int)Math.Round(center.X + Math.Cos((start_angle + sweep_angle) / 180 * Math.PI) * radius),
                    (int)Math.Round(center.Y + Math.Sin((start_angle + sweep_angle) / 180 * Math.PI) * radius));
                return string.Format("A {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12}",
                    center.X, center.Y,
                    radius,
                    start,
                    end,
                    this.Unit,
                    0,
                    0,
                    filled ? "F" : "N",
                    start_point.X, start_point.Y,
                    end_point.X, end_point.Y);
            }

            private string Polygon(Point[] points, bool filled)
            {
                //P Nb parts convert thickness x0 y0 x1 y1 xi yi cc
                //With:
                //• Nb = a number of points.
                //• unit = 0 if common to the parts; if not, number of part (1. .n).
                //• convert = 0 if common to the 2 representations, if not 1 or 2.
                //• thickness = line thickness.
                //• xi yi coordinates of end i.
                //• cc = N F or F ( F = filled polygon; f = . filled polygon, N = transparent background)
                return string.Format("P {0} {1} {2} {3}  {4} {5}",
                    points.Length,
                    this.Unit,
                    0,
                    0,
                    string.Join("  ", points.Select((p) => string.Format("{0} {1}", p.X, p.Y))),
                    filled ? "F" : "N");
            }

            private string Pin(Point p, int number, string name)
            {
                // Description of Pins
                //X name number posx posy length orientation Snum Snom unit convert Etype [shape].
                //With:
                //• orientation = U (up) D (down) R (right) L (left).
                //• name = name (without space) of the pin. if ~: no name
                //• number = n pin number (4 characters maximum).
                //• length = pin length.
                //• Snum = pin number text size.
                //• Snom = pin name text size.
                //• unit = 0 if common to all parts. If not, number of the part (1. .n).
                //• convert = 0 if common to the representations, if not 1 or 2.
                //• Etype = electric type (1 character)
                //• shape = if present: pin shape (clock, inversion…).
                //X ~ 1 -550 0 300 R 60 40 1 1 I
                int center_x = Client.X + Client.Width / 2;
                return string.Format("X {0} {1} {2} {3} 0 {4} {5} {5} {6} 0 I",
                    name,
                    number,
                    p.X, p.Y,
                    p.X <= center_x ? "L" : "R",
                    this.TextSize,
                    this.Unit);
            }

            private const string m_symbol_template =
@"#
# {0}
#
DEF {0} {1} 0 {2} Y N {3} L N
F0 ""{1}"" {4} {5} {2} H V C CNN
F1 ""{0}"" {6} {7} {2} H V C CNN
F2 ""~"" 0 0 {2} H V C CNN
F3 ""~"" 0 0 {2} H V C CNN
DRAW
{8}
ENDDRAW
ENDDEF
";
            // 0 = name
            // 1 = reference
            // 2 = text_offset
            // 3 = unit_count
            // 4 = reference posx
            // 5 = reference posy
            // 6 = name posx
            // 7 = name posy
            // 8 = drawing data

            public string Result()
            {
                //The format is as follows :
                //DEF name reference unused text_offset draw_pinnumber draw_pinname unit_count units_locked option_flag
                //ALIAS name1 name2…
                //fields list
                //DRAW
                //list graphic elements and pins
                //ENDDRAW
                //ENDDEF
                //Parameters for DEF :
                //• name = component name in library (74LS02 ...)
                //• référence = Reference ( U, R, IC .., which become U3, U8, R1, R45, IC4...)
                //• unused = 0 (reserved)
                //• text_offset = offset for pin name position
                //• draw_pinnumber = Y (display pin number) ou N (do not display pin number).
                //• draw_pinname = Y (display pin name) ou N (do not display pin name).
                //• unit_count = Number of part ( or section) in a component package.
                //• units_locked = = L (units are not identical and cannot be swapped) or F (units are 
                //identical and therefore can be swapped) (Used only if unit_count > 1)
                //• option_flag = N (normal) or P (component type "power")
                //
                // Description of fields
                //F n “text” posx posy dimension orientation visibility hjustify vjustify/italic/bold “name”
                //with:
                //• n = field number :
                //• reference = 0.
                //• value = 1.
                //• Pcb FootPrint = 2.
                //• User doc link = 3. At present time: not used 
                //• n = 4..11 = fields 1 to 8 (since January 2009 more than 8 field allowed, so n can be > 11.
                //• text (delimited by double quotes)
                //• position X and Y
                //• dimension (default = 50)
                //• orientation = H (horizontal) or V (vertical).
                //• Visibility = V (visible) or I (invisible)
                //• hjustify vjustify = L R C B or T
                //• L= left
                //• R = Right
                //• C = centre
                //• B = bottom
                //• T = Top
                //• Style: Italic = I or N ( since January 2009)
                //• Style Bold = B or N ( since January 2009)
                //• Name of the field (delimited by double quotes) (only if it is not the default name)
                //F0 "U" 0 300 60 H V C CNN
                //F1 "TEST" 0 -300 60 H V C CNN
                //F2 "~" 0 0 60 H V C CNN
                //F3 "~" 0 0 60 H V C CNN
                //
                //#
                //# TEST
                //#
                //DEF TEST U 0 40 Y Y 6 L N
                //F0 "U" 0 300 60 H V C CNN
                //F1 "TEST" 0 -300 60 H V C CNN
                //F2 "~" 0 0 60 H V C CNN
                //F3 "~" 0 0 60 H V C CNN
                //DRAW
                //T 0 -350 100 60 0 2 0 b  Normal 0 C C
                //P 5 2 0 0  -200 0  0 200  200 0  0 -150  -200 0 N
                //S -150 0 150 200 3 0 0 N
                //A 0 0 200 -1799 -1 4 0 0 N -200 0 200 0
                //C 100 150 50 5 0 0 F
                //A 93 113 113 -864 190 6 0 100 N 100 0 200 150
                //C -300 100 50 6 0 0 f
                //C 0 0 250 1 1 0 N
                //X ~ 1 -550 0 300 R 60 40 1 1 I
                //ENDDRAW
                //ENDDEF
                return string.Format(m_symbol_template,
                    this.Name,
                    "S",
                    this.TextSize,
                    this.Unit,
                    0, this.Client.Y + this.Client.Height,
                    0, this.Client.Y,
                    string.Join(m_line_end, m_result));
            }

        }

        private string CreateSymbol(Model.Switch data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (string.IsNullOrWhiteSpace(data.SymbolName))
                throw new Exception("Please enter a valid symbol name in the switch properties dialog.");
            if (data.Sides.Count == 0)
                throw new Exception("Please create at least one wafer side.\n");

            // create the object to capture the drawing operations
            var symbol = new KiCadGraphics()
            {
                Name = data.SymbolName,
                TextSize = 60,
                Client = new Rectangle(-300, -300, 600, 600),
            };

            // draw each of the sides
            var pin_grid = 50;
            for (int i = 0; i < data.Sides.Count; ++i)
            {
                symbol.Unit = i + 1;
                WaferControl.Draw(symbol, symbol.Client, data.Sides[i], 0, data.StatorStart, data.RearView, data.TextCCW, true, pin_grid);
            }

            // create the data
            return string.Format(m_file_template,
                DateTime.Now,
                symbol.Result());
        }

        // see https://github.com/AdharLabs/Kicad-tools/tree/master/libgen
        public void Export(Model.Switch data, string filename)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            // generate the symbol data
            var text = CreateSymbol(data);

            // write the data to disk
            using (var f = new System.IO.StreamWriter(filename))
            {
                f.Write(text);
                f.Close();
            }
        }
    }
}
