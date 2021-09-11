using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Manipulation
{
	public static class VisualizerTask
	{
		public static double X = 220;
		public static double Y = -100;
		public static double Alpha = 0.05;
		public static double Wrist = 2 * Math.PI / 3;
		public static double Elbow = 3 * Math.PI / 4;
		public static double Shoulder = Math.PI / 2;
		public static double delta = Math.PI / 180;

		public static Brush UnreachableAreaBrush = new SolidBrush(Color.FromArgb(255, 255, 230, 230));
		public static Brush ReachableAreaBrush = new SolidBrush(Color.FromArgb(255, 230, 255, 230));
		public static Pen ManipulatorPen = new Pen(Color.Black, 3);
		public static Brush JointBrush = Brushes.Gray;

		public static void KeyDown(Form form, KeyEventArgs key)
		{
			if (key.KeyCode == Keys.Q)
			{
				Shoulder += delta;
				Wrist = -Alpha - Shoulder - Elbow;
			}
			else if (key.KeyCode == Keys.A)
			{
				Shoulder -= delta;
				Wrist = -Alpha - Shoulder - Elbow;
			}
			else if (key.KeyCode == Keys.W)
			{
				Elbow += delta;
				Wrist = -Alpha - Shoulder - Elbow;
			}
			else if (key.KeyCode == Keys.S)
			{
				Elbow -= delta;
				Wrist = -Alpha - Shoulder - Elbow;
			}

			// TODO: Добавьте реакцию на QAWS и пересчитывать Wrist
			form.Invalidate(); // 
		}


		public static void MouseMove(Form form, MouseEventArgs e)
		{
			// TODO: Измените X и Y пересчитав координаты (e.X, e.Y) в логические.
			var newPoint = ConvertWindowToMath(new PointF((float)e.X, (float)e.Y), GetShoulderPos(form));
			X = newPoint.X;
			Y = newPoint.Y;
			form.Invalidate();
			UpdateManipulator();
		}

		public static void MouseWheel(Form form, MouseEventArgs e)
		{
			// TODO: Измените Alpha, используя e.Delta — размер прокрутки колеса мыши
			Alpha += e.Delta;
			UpdateManipulator();
			form.Invalidate();
		}

		public static void UpdateManipulator()
		{
			// Вызовите ManipulatorTask.MoveManipulatorTo и обновите значения полей Shoulder, Elbow и Wrist, 
			// если они не NaN. Это понадобится для последней задачи.
			double[] arr = new double[3]; //shoulder, elbow, wrist
			arr = ManipulatorTask.MoveManipulatorTo(X, Y, Alpha);
			if (arr[0] != double.NaN)
				Shoulder = arr[0];
			if (arr[1] != double.NaN)
				Elbow = arr[1];
			if (arr[2] != double.NaN)
				Wrist = arr[2];
		}

		public static void DrawManipulator(Graphics graphics, PointF shoulderPos)
		{
			var joints = AnglesToCoordinatesTask.GetJointPositions(Shoulder, Elbow, Wrist); // elbowPos, wristPos, palmEndPos
			var elbowPosWindow = ConvertMathToWindow(joints[0], shoulderPos);
			var wristPosWindow = ConvertMathToWindow(joints[1], shoulderPos);
			var palmPosWindow = ConvertMathToWindow(joints[2], shoulderPos);
			graphics.DrawString(
                $"X={X:0}, Y={Y:0}, Alpha={Alpha:0.00}", 
                new Font(SystemFonts.DefaultFont.FontFamily, 12), 
                Brushes.DarkRed, 
                10, 
                10);
			DrawReachableZone(graphics, ReachableAreaBrush, UnreachableAreaBrush, shoulderPos, joints);
			var windowPoint = ConvertMathToWindow(new PointF((float)X, (float)Y), shoulderPos);
			graphics.DrawLine(ManipulatorPen, shoulderPos, elbowPosWindow);
			graphics.DrawLine(ManipulatorPen, elbowPosWindow, wristPosWindow);
			graphics.DrawLine(ManipulatorPen, wristPosWindow, palmPosWindow);
			graphics.FillEllipse(JointBrush, shoulderPos.X-7, shoulderPos.Y-7, 14, 14);
			graphics.FillEllipse(JointBrush, elbowPosWindow.X - 7, elbowPosWindow.Y - 7, 14, 14);
			graphics.FillEllipse(JointBrush, wristPosWindow.X - 7, wristPosWindow.Y - 7, 14, 14);
			// Нарисуйте сегменты манипулятора методом graphics.DrawLine используя ManipulatorPen.
			// Нарисуйте суставы манипулятора окружностями методом graphics.FillEllipse используя JointBrush.
			// Не забудьте сконвертировать координаты из логических в оконные
		}

		private static void DrawReachableZone(
            Graphics graphics, 
            Brush reachableBrush, 
            Brush unreachableBrush, 
            PointF shoulderPos, 
            PointF[] joints)
		{
			var rmin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
			var rmax = Manipulator.UpperArm + Manipulator.Forearm;
			var mathCenter = new PointF(joints[2].X - joints[1].X, joints[2].Y - joints[1].Y);
			var windowCenter = ConvertMathToWindow(mathCenter, shoulderPos);
			graphics.FillEllipse(reachableBrush, windowCenter.X - rmax, windowCenter.Y - rmax, 2 * rmax, 2 * rmax);
			graphics.FillEllipse(unreachableBrush, windowCenter.X - rmin, windowCenter.Y - rmin, 2 * rmin, 2 * rmin);
		}

		public static PointF GetShoulderPos(Form form)
		{
			return new PointF(form.ClientSize.Width / 2f, form.ClientSize.Height / 2f);
		}

		public static PointF ConvertMathToWindow(PointF mathPoint, PointF shoulderPos)
		{
			return new PointF(mathPoint.X + shoulderPos.X, shoulderPos.Y - mathPoint.Y);
		}

		public static PointF ConvertWindowToMath(PointF windowPoint, PointF shoulderPos)
		{
			return new PointF(windowPoint.X - shoulderPos.X, shoulderPos.Y - windowPoint.Y);
		}

	}
}