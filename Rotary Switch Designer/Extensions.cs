using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Rotary_Switch_Designer
{
    public static class Extensions
    {
        public static int RotorLevels(this Model.Side data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Positions == null)
                throw new Exception("data.WaferPositions is null");
            int result = 0;
            foreach (var position in data.Positions)
            {
                if (position.RotorSlices == null)
                    throw new Exception("position.RotorSlices is null");
                result = Math.Max(result, position.RotorSlices.Count);
            }
            return result;
        }

        public static T CloneObject<T>(this T value)
        {
            if (value == null)
                return default(T);

            using (var ms = new MemoryStream())
            {
                var xs = new XmlSerializer(value.GetType());
                xs.Serialize(ms, value);
                ms.Position = 0;
                return (T)xs.Deserialize(ms);
            }
        }
    }
}
