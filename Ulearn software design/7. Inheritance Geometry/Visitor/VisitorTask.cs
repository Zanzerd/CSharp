
using System;
using System.Collections.Generic;

namespace Inheritance.Geometry.Visitor
{
    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract Body Accept(IVisitor visitor);
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override Body Accept(IVisitor visitor)
        {
            return visitor.visitBall(this);
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

        public override Body Accept(IVisitor visitor)
        {
            return visitor.visitRectangularCuboid(this);
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

        public override Body Accept(IVisitor visitor)
        {
            return visitor.visitCylinder(this);
        }
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override Body Accept(IVisitor visitor)
        {
            return visitor.visitCompoundBody(this);
        }
    }

    public interface IVisitor
    {
        Body visitBall(Ball ball);
        Body visitRectangularCuboid(RectangularCuboid rc);
        Body visitCylinder(Cylinder cyl);
        Body visitCompoundBody(CompoundBody cb);
    }
    public class BoundingBoxVisitor : IVisitor
    {
        public Body visitBall(Ball ball)
        {
            return new RectangularCuboid(ball.Position, 2 * ball.Radius, 2 * ball.Radius, 2 * ball.Radius);
        }

        public Body visitCylinder(Cylinder cyl)
        {
            return new RectangularCuboid(cyl.Position, 2 * cyl.Radius, 2 * cyl.Radius, cyl.SizeZ);
        }

        public Body visitRectangularCuboid(RectangularCuboid rc)
        {
            return rc;
        }

        public Body visitCompoundBody(CompoundBody cb)
        {
            var minX = cb.Position.X;
            var maxX = cb.Position.X;
            var minY = cb.Position.Y;
            var maxY = cb.Position.Y;
            var minZ = cb.Position.Z;
            var maxZ = cb.Position.Z;
            foreach (var part in cb.Parts)
            {
                var tempVisitor = new BoundingBoxVisitor();
                RectangularCuboid box;
                if (part is RectangularCuboid rcPart)
                {
                    box = (RectangularCuboid)tempVisitor.visitRectangularCuboid(rcPart);
                }
                else if (part is Ball ballPart)
                {
                    box = (RectangularCuboid)tempVisitor.visitBall(ballPart);
                }
                else if (part is Cylinder cylPart)
                {
                    box = (RectangularCuboid)tempVisitor.visitCylinder(cylPart);
                }
                else
                {
                    box = (RectangularCuboid)tempVisitor.visitCompoundBody((CompoundBody)part);
                }
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
    }

    public class BoxifyVisitor : IVisitor
    {
        public Body visitBall(Ball ball)
        {
            return new RectangularCuboid(ball.Position, 2 * ball.Radius, 2 * ball.Radius, 2 * ball.Radius);
        }

        public Body visitCylinder(Cylinder cyl)
        {
            return new RectangularCuboid(cyl.Position, 2 * cyl.Radius, 2 * cyl.Radius, cyl.SizeZ);
        }

        public Body visitRectangularCuboid(RectangularCuboid rc)
        {
            return rc;
        }

        public Body visitCompoundBody(CompoundBody cb)
        {
            var result = new List<Body>();
            foreach (var part in cb.Parts)
            {
                var tempVisitor = new BoxifyVisitor();
                Body box;
                if (part is RectangularCuboid rcPart)
                {
                    box = tempVisitor.visitRectangularCuboid(rcPart);
                }
                else if (part is Ball ballPart)
                {
                    box = tempVisitor.visitBall(ballPart);
                }
                else if (part is Cylinder cylPart)
                {
                    box = tempVisitor.visitCylinder(cylPart);
                }
                else
                {
                    box = tempVisitor.visitCompoundBody((CompoundBody)part);
                }
                result.Add(box);
            }
            return new CompoundBody(result);
        }
    }
}