using System;

namespace GeometryTasks
{
    public class Vector
    {
        public double X;
        public double Y;
        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public Vector Add(Vector vec)
        {
            return Geometry.Add(this, vec);
        }

        public bool Belongs(Segment seg)
        {
            return Geometry.IsVectorInSegment(this, seg);
        }
    }

    public class Geometry
    {
        public static double GetLength(Vector vec)
        {
            return Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
        }

        public static Vector Add(Vector vec1, Vector vec2)
        {
            return new Vector { X = vec1.X + vec2.X, Y = vec1.Y + vec2.Y };
        }

        public static double GetLength(Segment seg)
        {
            return Math.Sqrt((seg.Begin.X - seg.End.X) * (seg.Begin.X - seg.End.X) +
                (seg.Begin.Y - seg.End.Y) * (seg.Begin.Y - seg.End.Y));
        }

        public static bool IsVectorInSegment(Vector vec, Segment seg)
        {
            var segHelper1 = new Segment { Begin = seg.Begin, End = vec };
            var segHelper2 = new Segment { Begin = vec, End = seg.End };
            if (GetLength(segHelper1) + GetLength(segHelper2) == GetLength(seg))
                return true;
            return false;
        }
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public bool Contains(Vector vec)
        {
            return Geometry.IsVectorInSegment(vec, this);
        }
    }
}

