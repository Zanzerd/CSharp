using System;
using System.Collections.Generic;
using System.Linq;

namespace Recognizer
{
	public static class ThresholdFilterTask
	{
		public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
		{
			int x = original.GetLength(0);
			int y = original.GetLength(1);
			int N = x * y;
			if (N == 1)
			{
				if (whitePixelsFraction < 1.0)
					return new double[,] { { 0.0 } };
				else
					return new double[,] { { 1.0 } };
			}
			if (whitePixelsFraction == 1.0)
			{
				original = BorderCase(original, 1.0);
				return original;
			}
			if (whitePixelsFraction == 0.0)
			{
				original = BorderCase(original, 0.0);
				return original;
			}
			int fraction = (int)Math.Floor(whitePixelsFraction * N);
			double[] original1D = Array2DtoArray1D(original);
			double[] original1DSorted = InsertionSort(original1D);
			double med = MedianNumber(original1DSorted);
			int rc = RepeatCounter(original1D);
			if (rc > 0)
				fraction += (int)(whitePixelsFraction * rc);
			double[] arrTemp = ThresholdFilter(original1D, med);

			double[] result1D = BruteForceThreshold(original1D, arrTemp, fraction, med);
			double[,] result2D = Array1DtoArray2D(result1D, x, y);
			return result2D;
		}

		public static int RepeatCounter(double[] arr)
		{
			int max = 1;
			for (int i = 0; i < arr.GetLength(0); i++)
			{
				int counter = 1;
				double temp = arr[i];
				for (int j = 0; j < arr.GetLength(0) && j != i; j++)
					if (arr[i] == arr[j])
						counter++;
				if (counter > max)
					max = counter;
			}
			return max;
		}
		public static double[] BruteForceThreshold(double[] original, double[] arrTemp, int fraction,
			double med)
		{
			double TNew;
			double[] result1D = new double[] { };
			double border = original.Max();
			if (fraction == 0)
			{
				for (int i = 0; i < original.Length; i++)
					arrTemp[i] = 0.0;
				return arrTemp;
			}
			if (WhitePixelsCounter(arrTemp) > fraction)
			{
				for (double eps = 0; eps + med <= border; eps += 0.001)
				{
					TNew = med + eps;
					double[] arrNew = ThresholdFilter(original, TNew);
					if (WhitePixelsCounter(arrNew) <= fraction)
					{
						result1D = arrNew;
						break;
					}
					if (SameColourCounter(arrNew, arrNew[0]) == arrNew.Length)
					{
						result1D = arrNew;
						break;
					}
				}
			}
			else if (WhitePixelsCounter(arrTemp) < fraction)
			{
				for (double eps = 0; eps + med <= border; eps += 0.001)
				{
					TNew = med - eps;
					double[] arrNew = ThresholdFilter(original, TNew);
					if (WhitePixelsCounter(arrNew) >= fraction)
					{
						result1D = arrNew;
						break;
					}
					if (SameColourCounter(arrNew, arrNew[0]) == arrNew.Length)
					{
						result1D = arrNew;
						break;
					}
				}
			}
			else
			{
				result1D = arrTemp;
				TNew = med;
			}
			return result1D;
		}

		public static double[] ThresholdFilter(double[] original, double T)
		{
			double[] result = new double[original.GetLength(0)];
			for (int i = 0; i < original.Length; i++)
			{
				if (original[i] >= T)
					result[i] = 1.0;
				else
					result[i] = 0.0;
			}
			return result;
		}

		public static int WhitePixelsCounter(double[] array)
		{
			int counter = 0;
			foreach (double p in array)
			{
				if (p == 1.0)
					counter++;
			}
			return counter;
		}

		public static double[] Array2DtoArray1D(double[,] original)
		{
			int x = original.GetLength(0);
			int y = original.GetLength(1);
			List<double> original1DList = new List<double>();
			for (int i = 0; i < x; i++)
				for (int j = 0; j < y; j++)
					original1DList.Add(original[i, j]);
			double[] original1D = original1DList.ToArray();
			return original1D;
		}

		public static double[,] Array1DtoArray2D(double[] original, int x, int y)
		{
			var queue = new Queue<double>(original);
			double[,] result = new double[x, y];
			for (int i = 0; i < x; i++)
				for (int j = 0; j < y; j++)
				{
					result[i, j] = queue.Dequeue();
				}
			return result;
		}

		public static double[] InsertionSort(double[] array)
		{
			double[] newarr = new double[array.GetLength(0)];
			for (int k = 0; k < array.GetLength(0); k++)
			{
				newarr[k] = array[k];
			}
			for (int i = 0; i < newarr.GetLength(0); i++)
			{
				double max = newarr[i];
				for (int j = 0; j < i; j++)
				{
					if (newarr[j] > max)
					{
						double temp = newarr[j];
						newarr[j] = newarr[i];
						newarr[i] = temp;
					}
				}
			}
			return newarr;
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

		public static double SameColourCounter(double[] array, double colour)
		{
			int counter = 0;
			for (int i = 0; i < array.GetLength(0); i++)
			{
				if (array[i] == colour)
					counter++;
			}
			return counter;
		}
		public static double[,] BorderCase(double[,] original, double wpFraction)
		{
			int x = original.GetLength(0);
			int y = original.GetLength(1);
			for (int i = 0; i < x; i++)
				for (int j = 0; j < y; j++)
					original[i, j] = wpFraction;
			return original;
		}
	}
}


