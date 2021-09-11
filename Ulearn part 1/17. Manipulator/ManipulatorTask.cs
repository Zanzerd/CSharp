using System;
using static System.Math;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        /// <summary>
        /// Возвращает массив углов (shoulder, elbow, wrist),
        /// необходимых для приведения эффектора манипулятора в точку x и y 
        /// с углом между последним суставом и горизонталью, равному alpha (в радианах)
        /// См. чертеж manipulator.png!
        /// </summary>
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            // Используйте поля Forearm, UpperArm, Palm класса Manipulator
            double palmLenY = Manipulator.Palm * Math.Sin(alpha);
            double palmLenX = Manipulator.Palm * Math.Sin(Math.PI / 2 - alpha);
            double wristX = x;
            double wristY = y;
            double amod2pi = alpha % (2 * PI);
            if (amod2pi < 0)
                amod2pi = 2 * PI + amod2pi;
            if (amod2pi >= 0 && amod2pi % (2 * PI) <= PI / 2)
            {
                wristX -= Math.Abs(palmLenX);
                wristY += Math.Abs(palmLenY);
            }
            else if (amod2pi % (2 * PI) > PI / 2 && amod2pi % (2 * PI) <= PI)
            {
                wristX += Math.Abs(palmLenX);
                wristY += Math.Abs(palmLenY);
            }
            else if (amod2pi % (2 * PI) > PI && amod2pi % (2 * PI) <= 3 * PI / 2)
            {
                wristX += Math.Abs(palmLenX);
                wristY -= Math.Abs(palmLenY);
            }
            else if (amod2pi % (2 * PI) > 3 * PI / 2 && amod2pi % (2 * PI) < 2 * PI)
            {
                wristX -= Math.Abs(palmLenX);
                wristY -= Math.Abs(palmLenY);
            }
            double distWristShoulder = Math.Sqrt(wristX * wristX + wristY * wristY);
            double elbowAngle = TriangleTask.GetABAngle(Manipulator.UpperArm,
                Manipulator.Forearm, distWristShoulder);
            double shoulderAngle1 = TriangleTask.GetABAngle(Manipulator.UpperArm,
                distWristShoulder, Manipulator.Forearm);
            double shoulderAngle2 = Math.Atan2(wristY, wristX);
            double shoulderAngle = shoulderAngle1 + shoulderAngle2;
            double wristAngle = -alpha - shoulderAngle - elbowAngle;
            if (wristAngle == double.NaN || shoulderAngle == double.NaN || elbowAngle == double.NaN)
                return new[] { double.NaN, double.NaN, double.NaN };
            return new[] { shoulderAngle, elbowAngle, wristAngle };
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        [Test]
        public void TestMoveManipulatorTo()
        {
            for (int i = 0; i < 100; i++)
            {
                Random rnd = new Random();
                double randX = rnd.NextDouble() * 1000.0;
                double randY = rnd.NextDouble() * 1000.0;
                double alpha = rnd.NextDouble() * 2 * Math.PI;
                double[] angles = ManipulatorTask.MoveManipulatorTo(randX, randY, alpha);
                PointF[] actualPositions = AnglesToCoordinatesTask.GetJointPositions(angles[0], angles[1], angles[2]);
                double rmin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
                double rmax = Manipulator.UpperArm + Manipulator.Forearm;
                double lenHelp = Math.Sqrt(randX * randX + randY * randY);
                if (lenHelp > rmin && lenHelp < rmax)
                {
                    Assert.AreEqual((float)randX, actualPositions[2].X, 1e-5, "X coordinate of palm");
                    Assert.AreEqual((float)randY, actualPositions[2].Y, 1e-5, "Y coordinate of palm");
                }
                else
                {
                    Assert.AreEqual(actualPositions[2].X, float.NaN);
                    Assert.AreEqual(actualPositions[2].Y, float.NaN);
                }
            }
        }
    }
}