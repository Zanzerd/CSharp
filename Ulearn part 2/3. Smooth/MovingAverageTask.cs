using System.Collections.Generic;
using System.Linq;
namespace yield
{
	public static class MovingAverageTask
	{
		public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
		{
			double sum = 0;
			Averager averager = new Averager(windowWidth);
			Queue<double> queue = new Queue<double>();
			foreach (DataPoint dp in data)
			{
				double Y = dp.OriginalY;
				queue.Enqueue(Y);
				sum += Y;
				if (queue.Count > windowWidth)
					sum -= queue.Dequeue();
				yield return new DataPoint(dp.X, dp.OriginalY).WithAvgSmoothedY(sum/queue.Count);
			}
		}
	}

	public class Averager
	{
		public double measurement { get; set; }
		Queue<double> queue;
		int bufferLength;
		double sum;

		public Averager(int bufferLength)
		{
			this.bufferLength = bufferLength;
			queue = new Queue<double>();
		}
		public double Measure()
		{
			queue.Enqueue(measurement);
			sum += measurement;
			if (queue.Count > bufferLength)
				sum -= queue.Dequeue();
			return sum / queue.Count;
		}
	}
}