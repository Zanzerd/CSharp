using System;

namespace DistanceTask
{
	public static class DistanceTask
	{
		// Расстояние от точки (x, y) до отрезка AB с координатами A(ax, ay), B(bx, by)
		public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
		{
			double distance;
			double segmentVecX = bx - ax;
			double segmentVecY = by - ay;
			double segmentLength = Math.Sqrt(segmentVecX * segmentVecX + segmentVecY * segmentVecY);
			double vecWaX = x - ax;
			double vecWaY = y - ay;
			double vecWbX = x - bx;
			double vecWbY = y - by;
			if (ScalarProduct(vecWaX, vecWaY, segmentVecX, segmentVecY) <= 0)
			{
				distance = DistanceBetweenDots(x, y, ax, ay);
			}
			else if (ScalarProduct(vecWbX, vecWbY, segmentVecX, segmentVecY) >= 0)
			{
				distance = DistanceBetweenDots(x, y, bx, by);
			}
			else
            {
				distance = DistanceToLine(x, y, ax, ay, bx, by);
            }
			return Math.Abs(distance);
		}

		public static double DistanceBetweenDots(double x1, double y1, double x2, double y2)
        {
			return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }
		public static double ScalarProduct(double x1, double y1, double x2, double y2)
		{
			return x1 * x2 + y1 * y2;
		}
		public static double DistanceToLine(double x0, double y0, double x1, double y1, double x2, double y2)
        {
			return Math.Abs((y2-y1)*x0 - (x2-x1)*y0 + x2*y1 - y2*x1) / DistanceBetweenDots(x1, y1, x2, y2);
        }
	}
}