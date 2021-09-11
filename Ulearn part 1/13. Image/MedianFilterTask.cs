using System.Linq;
using System;

namespace Recognizer
{
	internal static class MedianFilterTask
	{
		/* 
		 * Для борьбы с пиксельным шумом, подобным тому, что на изображении,
		 * обычно применяют медианный фильтр, в котором цвет каждого пикселя, 
		 * заменяется на медиану всех цветов в некоторой окрестности пикселя.
		 * https://en.wikipedia.org/wiki/Median_filter
		 * 
		 * Используйте окно размером 3х3 для не граничных пикселей,
		 * Окно размером 2х2 для угловых и 3х2 или 2х3 для граничных.
		 */
		public static double[,] MedianFilter(double[,] original)
		{
			int x = original.GetLength(0);
			int y = original.GetLength(1);
			double[,] modified = new double[x, y];

			if (x == y && x == 1)
			{
				return original;
			}
			if (x == 1)
			{
				if (y == 2)
				{
					double[] temp = new double[] { original[0, 0], original[0, 1] };
					modified[0, 0] = MedianNumber(InsertionSort(temp));
					modified[0, 1] = modified[0, 0];
				}
				else
				{
					double[] temp1 = new double[] { original[0, 0],
						original[0, 1] };
					modified[0, 0] = MedianNumber(InsertionSort(temp1));
					double[] temp2 = new double[] {original[0, y-1],
						original[0, y-2] };
					modified[0, y - 1] = MedianNumber(InsertionSort(temp2));
					for (int i = 1; i < y - 1; i++)
					{
						double[] temp = new double[] { original[0, i - 1], original[0, i], original[0, i + 1] };
						modified[0, i] = MedianNumber(InsertionSort(temp));
					}
				}
				return modified;
			}
			if (y == 1)
			{
				{
					if (x == 2)
					{
						double[] temp = new double[] { original[1, 0], original[0, 0] };
						modified[0, 0] = MedianNumber(InsertionSort(temp));
						modified[1, 0] = modified[0, 0];
					}
					else
					{
						modified[0, 0] = MedianNumber(InsertionSort(new double[] { original[0, 0],
						original[1, 0] }));
						modified[x - 1, 0] = MedianNumber(InsertionSort(new double[] { original[x-1, 0],
						original[x-2, 0] }));
						for (int i = 1; i < x - 1; i++)
						{
							double[] temp = new double[] { original[i - 1, 0], original[i, 0], original[i + 1, 0] };
							modified[i, 0] = MedianNumber(InsertionSort(temp));
						}
					}
					return modified;
				}
			}

			for (int i = 0; i < x; i++)
				for (int j = 0; j < y; j++)
				{
					if (i == 0 && j == 0)
					{
						modified[i, j] = MedianFilterForCorners(original, i, j, i + 1, j + 1);
					}
					else if (i == 0 && j % (y - 1) == 0 && j != 0)
					{
						modified[i, j] = MedianFilterForCorners(original, i, j, i + 1, j - 1);
					}
					else if (i % (x - 1) == 0 && i != 0 && j == 0)
					{
						modified[i, j] = MedianFilterForCorners(original, i, j, i - 1, j + 1);
					}
					else if (i % (x - 1) == 0 && i != 0 && j % (y - 1) == 0 && j != 0)
					{
						modified[i, j] = MedianFilterForCorners(original, i, j, i - 1, j - 1);
					}
					else if (i == 0 && j % (y - 1) != 0)
					{
						double[] temp = new double[] { original[i, j], original[i + 1, j], original[i, j+1],
							original[i + 1, j + 1], original[i, j-1], original[i+1, j-1]};
						modified[i, j] = MedianNumber(InsertionSort(temp));
					}
					else if (i % (x - 1) == 0 && i != 0 && j % (y - 1) != 0)
					{
						double[] temp = new double[] {original[i,j], original[i, j+1], original[i, j-1],
							original[i-1, j], original[i-1, j-1], original[i-1, j+1]};
						modified[i, j] = MedianNumber(InsertionSort(temp));
					}
					else if (i % (x - 1) != 0 && j == 0)
					{
						double[] temp = new double[] {original[i,j], original[i, j+1], original[i+1, j+1],
							original[i-1, j], original[i-1, j+1], original[i+1, j]};
						modified[i, j] = MedianNumber(InsertionSort(temp));
					}
					else if (i % (x - 1) != 0 && j % (y - 1) == 0 && j != 0)
					{
						double[] temp = new double[] {original[i,j], original[i, j-1], original[i+1, j],
							original[i+1, j-1], original[i-1, j], original[i-1, j-1]};
						modified[i, j] = MedianNumber(InsertionSort(temp));
					}
					else
					{
						double[] temp = new double[] {original[i,j], original[i+1,j], original[i, j+1], original[i-1, j],
							original[i, j-1], original[i-1, j-1], original[i-1, j+1], original[i+1, j-1], original[i+1, j+1]};
						modified[i, j] = MedianNumber(InsertionSort(temp));
					}
				}

			return modified;
		}

		public static double MedianFilterForCorners(double[,] original, int i1, int j1, int i2, int j2)
		{
			double[] temp = new double[] { original[i1, j1], original[i1, j2], original[i2, j1], original[i2, j2] };
			temp = InsertionSort(temp);
			double med = MedianNumber(temp);
			return med;
		}
		public static double MedianNumber(double[] array)
		{
			int len = array.GetLength(0);
			// Console.WriteLine($"{array[0]}, {array[1]}, {array[2]}, {array[3]}");
			if (len == 2)
				return (array[0] + array[1]) / 2;
			if (len % 2 == 0)
				return (array[len / 2 - 1] + array[(len / 2)]) / 2;
			else
				return array[(int)Math.Ceiling((double)(len / 2))];
		}

		public static double[] InsertionSort(double[] array)
		{
			for (int i = 0; i < array.GetLength(0); i++)
			{
				double max = array[i];
				for (int j = 0; j < i; j++)
				{
					if (array[j] > max)
					{
						double temp = array[j];
						array[j] = array[i];
						array[i] = temp;
					}
				}
			}
			return array;
		}
	}
}