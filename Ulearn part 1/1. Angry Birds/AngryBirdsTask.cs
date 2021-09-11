using System;
using static System.Math;

namespace AngryBirds
{
	public static class AngryBirdsTask
	{
		const double g = 9.8;
		/// <param name="v">Начальная скорость</param>
		/// <param name="distance">Расстояние до цели</param>
		/// <returns>Угол прицеливания в радианах от 0 до Pi/2</returns>
		public static double FindSightAngle(double v, double distance)
		{
			(double t1, double t2) = SquareEquationRoots(g / 2, v, -distance);
			double t_needed = Min(t1, t2);
			if (t_needed < 0)
				t_needed = Max(t1, t2);
			double numerator = (g * Pow(t_needed, 2)) / 2;
			double denominator = v * t_needed;
			double result = Asin(numerator / denominator);
			if (result < 0)
				result = -result;
			return (0.5) * Asin((g * distance) / Pow(v, 2));
		}

		public static (double root1, double root2) SquareEquationRoots(double a, double b, double c)
		{
			double discriminant = Pow(b, 2) - 4 * a * c;
			double root1 = (-b + Sqrt(discriminant)) / (2 * a);
			double root2 = (-b - Sqrt(discriminant)) / (2 * a);
			return (root1, root2);
		}

	}
}