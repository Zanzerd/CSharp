using System.Collections.Generic;
using System.Linq;
namespace yield
{
	public static class MovingAverageTask
	{
		public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
		{
			Averager averager = new Averager(windowWidth);
			foreach(DataPoint dp in data)
            {
				averager.measurement = dp.OriginalY;
				yield return new DataPoint(dp.X, averager.Measure());
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