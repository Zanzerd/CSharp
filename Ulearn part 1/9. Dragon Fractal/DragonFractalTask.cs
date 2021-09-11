// В этом пространстве имен содержатся средства для работы с изображениями. 
// Чтобы оно стало доступно, в проект был подключен Reference на сборку System.Drawing.dll
using System.Drawing;
using System;

namespace Fractals
{
	internal static class DragonFractalTask
	{
		public static readonly double SqrtOf2 = Math.Sqrt(2);
		public const double Pi = Math.PI;
		public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
		{

			var random = new Random(seed);
			double xPrev = 1.0;
			double yPrev = 0.0;
			for(int i = 0; i <= iterationsCount; i++)
            {
				pixels.SetPixel(xPrev, yPrev);
				var nextNumber = random.Next(2);
				(double xNew, double yNew) = CallTransformation(nextNumber, xPrev, yPrev);
				xPrev = xNew;
				yPrev = yNew;
			}
			/*
			Создайте генератор рандомных чисел с сидом seed
			Начните с точки (1, 0)
			
			На каждой итерации:

			1. Выберите случайно одно из следующих преобразований и примените его к текущей точке:

				Преобразование 1. (поворот на 45° и сжатие в sqrt(2) раз):
				x' = (x · cos(45°) - y · sin(45°)) / sqrt(2)
				y' = (x · sin(45°) + y · cos(45°)) / sqrt(2)

				Преобразование 2. (поворот на 135°, сжатие в sqrt(2) раз, сдвиг по X на единицу):
				x' = (x · cos(135°) - y · sin(135°)) / sqrt(2) + 1
				y' = (x · sin(135°) + y · cos(135°)) / sqrt(2)
		
			2. Нарисуйте текущую точку методом pixels.SetPixel(x, y)

			*/
		}
		
		public static (double, double) CallTransformation(int number, double x, double y)
        {
			double xNew, yNew;
			switch (number)
            {
				case 0:
					xNew = (x * Math.Cos(Pi / 4) - y * Math.Sin(Pi / 4)) / SqrtOf2;
					yNew = (x * Math.Sin(Pi / 4) + y * Math.Cos(Pi / 4)) / SqrtOf2;
					break;
				case 1:
					xNew = (x * Math.Cos(3 * Pi / 4) - y * Math.Sin(3 * Pi / 4)) / SqrtOf2 + 1;
					yNew = (x * Math.Sin(3 * Pi / 4) + y * Math.Cos(3 * Pi / 4)) / SqrtOf2;
					break;
				default:
					throw new Exception("chto proizoshlo v case?");
			}
			return (xNew, yNew);
        }
	}
}