using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class StatisticsTask
	{
		public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
		{
			var groupedSortedVisits = visits
				.GroupBy(visit => visit.UserId)
				.Select(group => new
				{
					userId = group.Key,
					visitsSorted = group.OrderBy(visit => visit.DateTime)
				});
			//.OrderBy(group => group.ToList().Select(visit => visit.DateTime));
			var timeSpansPerUser = groupedSortedVisits
				.Select(group => new
				{
					userId = group.userId,
					visitsSorted = group.visitsSorted,
					zippedList = ExtensionsTask.Bigrams(group.visitsSorted.Select(visit => visit.DateTime))
						.Zip(ExtensionsTask.Bigrams(group.visitsSorted.Select(visit => visit.SlideType)),
						(x, y) => new Tuple<Tuple<DateTime, DateTime>, Tuple<SlideType, SlideType>>(x, y))
				});
			var minutesPerType = timeSpansPerUser
				.Select(group => group.zippedList.Where(pairOfPairs => pairOfPairs.Item2.Item1 == slideType)
					.Select(pairOfPairs => (double)(pairOfPairs.Item1.Item2 - pairOfPairs.Item1.Item1).TotalMinutes)
					.Where(d => d <= 120 && d >= 1))
				.SelectMany(g => g);
			double result;
			try
			{
				result = ExtensionsTask.Median(minutesPerType);
			}
			catch (InvalidOperationException)
			{
				result = 0;
			}
			return result;
		}
	}
}