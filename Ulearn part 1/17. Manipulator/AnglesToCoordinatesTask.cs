using System;
using System.Drawing;
using NUnit.Framework;
using static System.Math;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        /// <summary>
        /// По значению углов суставов возвращает массив координат суставов
        /// в порядке new []{elbow, wrist, palmEnd}
        /// </summary>
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            double alpha;
            double beta = shoulder + PI + elbow;
            double gamma = beta + PI + wrist;
            if (shoulder <= PI / 2)
                alpha = shoulder;
            else if (shoulder > PI / 2 && shoulder <= 3 * PI / 2)
                alpha = PI - shoulder;
            else
                alpha = 2 * PI - shoulder;
            double xElbow = Math.Cos(alpha) * Manipulator.UpperArm;
            double yElbow = Math.Sin(alpha) * Manipulator.UpperArm;
            double xWrist = Math.Cos(beta) * Manipulator.Forearm;
            double yWrist = Math.Sin(beta) * Manipulator.Forearm;
            double xPalm = Math.Cos(gamma) * Manipulator.Palm;
            double yPalm = Math.Sin(gamma) * Manipulator.Palm;
            var elbowPos = new PointF((float) xElbow, (float) yElbow);
            var wristPos = new PointF((float) (xElbow + xWrist), (float) (yElbow + yWrist));
            var palmEndPos = new PointF((float)(xElbow + xWrist + xPalm), (float)(yElbow + yWrist + yPalm));
            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        // Доработайте эти тесты!
        // С помощью строчки TestCase можно добавлять новые тестовые данные.
        // Аргументы TestCase превратятся в аргументы метода.
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI / 2, 2 * Math.PI, Manipulator.Forearm - Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(2 * PI, PI/2, 3 * PI / 2, Manipulator.Forearm + Manipulator.Palm, - Manipulator.UpperArm)]
        [TestCase(2 * PI, PI/2, 2 * PI, Manipulator.UpperArm, -Manipulator.Forearm - Manipulator.Palm)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
            //Assert.Fail("TODO: проверить, что расстояния между суставами равны длинам сегментов манипулятора!");
        }
    }
}