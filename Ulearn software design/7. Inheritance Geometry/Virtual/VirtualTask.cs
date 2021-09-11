using System;
using System.Collections.Generic;
using System.Linq;

namespace Inheritance.Geometry.Virtual
{
    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract bool ContainsPoint(Vector3 point);
        public abstract RectangularCuboid GetBoundingBox();
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var vector = point - Position;
            var length2 = vector.GetLength2();
            return length2 <= Radius * Radius;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return new RectangularCuboid(Position, 2 * Radius, 2 * Radius, 2 * Radius);
        }
    }

    public class RectangularCuboid : Body
    {
        public double SizeX { get; }
        public double SizeY { get; }
        public double SizeZ { get; }

        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var minPoint = new Vector3(
                    Position.X - SizeX / 2,
                    Position.Y - SizeY / 2,
                    Position.Z - SizeZ / 2);
            var maxPoint = new Vector3(
                Position.X + SizeX / 2,
                Position.Y + SizeY / 2,
                Position.Z + SizeZ / 2);

            return point >= minPoint && point <= maxPoint;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return this;
        }
    }

    public class Cylinder : Body
    {
        public double SizeZ { get; }

        public double Radius { get; }

        public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
        {
            SizeZ = sizeZ;
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var vectorX = point.X - Position.X;
            var vectorY = point.Y - Position.Y;
            var length2 = vectorX * vectorX + vectorY * vectorY;
            var minZ = Position.Z - SizeZ / 2;
            var maxZ = minZ + SizeZ;

            return length2 <= Radius * Radius && point.Z >= minZ && point.Z <= maxZ;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return new RectangularCuboid(Position, 2 * Radius, 2 * Radius, SizeZ);
        }
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            return Parts.Any(body => body.ContainsPoint(point));
        }

        public override RectangularCuboid GetBoundingBox()
        {
            int count = Parts.Count;
            var bariCenter = new Vector3(0, 0, 0);
            foreach (var part in Parts)
            {
                bariCenter += new Vector3(part.Position.X / count, part.Position.Y / count, part.Position.Z / count);
            }
            var minX = Position.X;
            var maxX = Position.X;
            var minY = Position.Y;
            var maxY = Position.Y;
            var minZ = Position.Z;
            var maxZ = Position.Z;
            foreach (var part in Parts)
            {
                var box = part.GetBoundingBox();
                var minPointCurr = new Vector3(box.Position.X - box.SizeX / 2, box.Position.Y - box.SizeY / 2, box.Position.Z - box.SizeZ / 2);
                if (minPointCurr.X < minX)
                    minX = minPointCurr.X;
                if (minPointCurr.Y < minY)
                    minY = minPointCurr.Y;
                if (minPointCurr.Z < minZ)
                    minZ = minPointCurr.Z;
                var maxPointCurr = new Vector3(box.Position.X + box.SizeX / 2, box.Position.Y + box.SizeY / 2, box.Position.Z + box.SizeZ / 2);
                if (maxPointCurr.X > maxX)
                    maxX = maxPointCurr.X;
                if (maxPointCurr.Y > maxY)
                    maxY = maxPointCurr.Y;
                if (maxPointCurr.Z > maxZ)
                    maxZ = maxPointCurr.Z;
            }
            var center = new Vector3(minX + Math.Abs((maxX - minX)) / 2,
                    minY + Math.Abs((maxY - minY)) / 2,
                    minZ + Math.Abs((maxZ - minZ)) / 2);
            return new RectangularCuboid(center, maxX - minX, maxY - minY, maxZ - minZ);
        }

        public static bool Greater(Vector3 v1, Vector3 v2)
        {
            return v1.X > v2.X || v1.Y > v2.Y || v1.Z >= v2.Z;
        }

        public static bool Lesser(Vector3 v1, Vector3 v2)
        {
            return v1.X < v2.X || v1.Y < v2.Y || v1.Z < v2.Z;
        }

    }
}