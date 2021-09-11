using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryTasks;

namespace GeometryPainting
{
    //Напишите здесь код, который заставит работать методы segment.GetColor и segment.SetColor

    public static class SegmentExtensions
    {

        public static Dictionary<Segment, Color> dictSegColor = new Dictionary<Segment, Color>();

        public static void SetColor(this Segment seg, Color color)
        {
            dictSegColor[seg] = color;
        }

        public static Color GetColor(this Segment seg)
        {
            if (dictSegColor.ContainsKey(seg))
                return dictSegColor[seg];
            else
                return Color.Black;
        }
    }
}
