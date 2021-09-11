using System;
using System.Collections.Generic;
using System.Linq;

namespace yield
{

	public static class MovingMaxTask
	{
		public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
		{
			LinkedList<double> deque = new LinkedList<double>();
			Queue<double> queueHelper = new Queue<double>();
			Stack<double> stackHelper;
			foreach (DataPoint dp in data)
			{
				stackHelper = new Stack<double>();
				double Y = dp.OriginalY;
				queueHelper.Enqueue(Y);
				//var node = deque.First;
				foreach (double y_d in deque)
				{
					if (y_d < Y)
					{
						stackHelper.Push(y_d);
					}
				}
				foreach (double y_d in stackHelper)
					deque.Remove(y_d);
				deque.AddLast(Y);
				var firstelem = deque.First.Value;
				if (queueHelper.Count > windowWidth && queueHelper.Dequeue() == firstelem)
				{
					deque.Remove(firstelem);
				}

				yield return new DataPoint(dp.X, dp.OriginalY).WithMaxY(deque.First.Value); // first или max?
			}
		}
	}
}