using System.Collections.Generic;
using System.Linq;


namespace yield
{
    public static class ExpSmoothingTask
    {
        public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
        {
            int i = 0;
            DataPoint prevPoint = new DataPoint(12.139633389642889, 12.139633389642889);
            foreach (DataPoint el in data)
            {
                if (i == 0)
                {
                    prevPoint = el.WithExpSmoothedY(el.OriginalY); ;
                    yield return prevPoint;
                }
                else
                {
                    var current = el.WithExpSmoothedY(prevPoint.ExpSmoothedY + alpha * (el.OriginalY - prevPoint.ExpSmoothedY));
                    yield return current;
                    prevPoint = current;
                }
                i++;
            }

        }
    }
}