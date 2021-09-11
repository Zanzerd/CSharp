using System;

namespace Rectangles
{
	public static class RectanglesTask
	{
		// Пересекаются ли два прямоугольника (пересечение только по границе также считается пересечением)
		public static bool AreIntersected(Rectangle r1, Rectangle r2)
		{
			// так можно обратиться к координатам левого верхнего угла первого прямоугольника: r1.Left, r1.Top
			if (AreIntersectedHorizontally(r1, r2) && AreIntersectedVertically(r1, r2))
				return true;
			else if (r1.Right == r2.Left && ((r1.Top >= r2.Top && r1.Top <= r2.Bottom) || (r1.Top <= r2.Top &&
				r1.Bottom >= r2.Top)))
				return true;
			else if (r2.Right == r1.Left && ((r2.Top >= r1.Top && r2.Top <= r1.Bottom) || (r2.Top <= r1.Top &&
				r2.Bottom >= r1.Top)))
				return true;
			else if (r1.Bottom == r2.Top && ((r1.Right >= r2.Right && r1.Left <= r2.Right) || 
				(r1.Left <= r2.Left && r1.Right >= r2.Left)))
				return true;
			return false;
		}
		public static bool AreIntersectedVertically(Rectangle r1, Rectangle r2)
        {
			if ((r2.Height == 0 && (r2.Top < r1.Top || r2.Bottom > r1.Bottom)) ||
				(r1.Height == 0 && (r1.Top < r2.Top || r1.Bottom > r2.Bottom)))
				return false;
			else if ((r2.Height == 0 && (r2.Top >= r1.Top || r2.Bottom <= r1.Bottom)) ||
				(r1.Height == 0 && (r1.Top >= r2.Top || r1.Bottom <= r2.Bottom)))
				return true;
			else if (r1.Bottom >= r2.Top 
				&& (Math.Abs(r1.Bottom - r2.Bottom) <= r1.Height ||
				Math.Abs(r1.Top - r2.Top) <= r2.Height))
				return true;
			else if (r2.Bottom >= r1.Top
				&& (Math.Abs(r2.Bottom - r1.Bottom) <= r2.Height ||
				Math.Abs(r2.Top - r1.Top) <= r1.Height))
				return true;
			return false;
		}
		public static bool AreIntersectedHorizontally(Rectangle r1, Rectangle r2)
        {
			if ((r2.Width == 0 && (r2.Left < r1.Left || r2.Right > r1.Right)) ||
				(r1.Width == 0 && (r1.Left < r2.Left || r1.Right > r2.Right)))
				return false;
			else if ((r2.Width == 0 && (r2.Left >= r1.Left || r2.Right <= r1.Right)) ||
				(r1.Width == 0 && (r1.Left >= r2.Left || r1.Right <= r2.Right)))
				return true;
			else if (r1.Right >= r2.Left
				&& (Math.Abs(r2.Right - r1.Right) <= r1.Width ||
				Math.Abs(r1.Left - r2.Left) <= r2.Width))
				return true;
			else if (r2.Right >= r1.Left
				&& (Math.Abs(r1.Right - r2.Right) <= r2.Width ||
				Math.Abs(r1.Left - r2.Left) <= r1.Width))
				return true;
			return false;
        }

		// Площадь пересечения прямоугольников
		public static int IntersectionSquare(Rectangle r1, Rectangle r2)
		{
			int height = 0, width = 0;
			if (IndexOfInnerRectangle(r1, r2) == 0)
				return r1.Width * r1.Height;
			else if (IndexOfInnerRectangle(r1, r2) == 1)
				return r2.Width * r2.Height;
			if (AreIntersected(r1, r2))
			{
				if (r1.Height == 0 || r1.Width == 0 || r2.Width == 0 || r2.Height == 0)
					return 0;
				else if (r1.Top <= r2.Top && r2.Bottom >= r1.Bottom && r1.Right >= r2.Right && r1.Left <= r2.Left)
				{
					height = Math.Abs(r1.Bottom - r2.Top);
					width = Math.Abs(r2.Right - r2.Left);
					 // проверка для тестов 2-5
				}
				else if (r2.Top <= r1.Top && r1.Bottom >= r2.Bottom && r2.Right >= r1.Right && r2.Left <= r1.Left)
				{
					height = Math.Abs(r2.Bottom - r1.Top);
					width = Math.Abs(r1.Right - r1.Left);
					 //проверка для тестов 2 - 5
				}
				else if (r2.Top <= r1.Top && r2.Bottom >= r1.Bottom && r1.Right >= r2.Right && r1.Left <= r2.Left)
				{
					height = Math.Abs(r1.Bottom - r1.Top);
					width = Math.Abs(r2.Right - r2.Left);
					// проверка для теста 1
				}
				else if (r1.Top <= r2.Top && r1.Bottom >= r2.Bottom && r2.Right >= r1.Right && r2.Left <= r1.Left)
				{
					height = Math.Abs(r2.Bottom - r2.Top);
					width = Math.Abs(r1.Right - r1.Left);
					// проверка для теста 1	
				}
				else if (r1.Top <= r2.Top && r1.Bottom <= r2.Bottom && r2.Right >= r1.Right && r2.Left <= r1.Left)
				{
					height = Math.Abs(r1.Bottom - r2.Top);
					width = Math.Abs(r1.Right - r1.Left);
				}
				else if (r2.Top <= r1.Top && r2.Bottom <= r1.Bottom && r1.Right >= r2.Right && r1.Left <= r2.Left)
				{
					height = Math.Abs(r2.Bottom - r1.Top);
					width = Math.Abs(r2.Right - r2.Left);
				}
				else if (r1.Top >= r2.Top && r1.Bottom <= r2.Bottom && r1.Right <= r2.Right && r1.Left <= r2.Left)
				{
					height = Math.Abs(r1.Bottom - r1.Top);
					width = Math.Abs(r1.Right - r2.Left);
				}
				else if (r2.Top >= r1.Top && r2.Bottom <= r1.Bottom && r2.Right <= r1.Right && r2.Left <= r1.Left)
				{
					height = Math.Abs(r2.Bottom - r2.Top);
					width = Math.Abs(r2.Right - r1.Left);
				}
				else if (r1.Top >= r2.Top && r1.Bottom <= r2.Bottom && r1.Right >= r2.Right && r1.Left >= r2.Left)
                {
					height = Math.Abs(r1.Bottom - r1.Top);
					width = Math.Abs(r2.Right - r1.Left);
                }
				else if (r2.Top <= r1.Top && r2.Right >= r1.Right)
				{
					height = Math.Abs(r2.Bottom - r1.Top);
					width = Math.Abs(r1.Right - r2.Left);
				}
				else if (r1.Top <= r2.Top && r1.Right >= r2.Right)
				{
					height = Math.Abs(r1.Bottom - r2.Top);
					width = Math.Abs(r2.Right - r1.Left);
				}
				else if (r1.Top <= r2.Top && r1.Right <= r2.Right)
				{
					height = Math.Abs(r1.Bottom - r2.Top);
					width = Math.Abs(r1.Right - r2.Left);
				}
				else if (r2.Top <= r1.Top && r2.Right <= r1.Right)
				{
					height = Math.Abs(r2.Bottom - r1.Top);
					width = Math.Abs(r2.Right - r1.Left);
				}
			}
			return height * width;
		}

		// Если один из прямоугольников целиком находится внутри другого — вернуть номер (с нуля) внутреннего.
		// Иначе вернуть -1
		// Если прямоугольники совпадают, можно вернуть номер любого из них.
		public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
		{
			if (r1.Bottom <= r2.Bottom && r1.Top >= r2.Top && r1.Right <= r2.Right && r1.Left >= r2.Left)
				return 0;
			else if (r2.Bottom <= r1.Bottom && r2.Top >= r1.Top && r2.Right <= r1.Right && r2.Left >= r1.Left)
				return 1;
			return -1;
		}
	}
}