using System;
using System.Linq;

namespace Names
{
    internal static class HistogramTask
    {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            double[] countNames = new double[31];
            string[] labels = new string[31];
            for (int i = 0; i < labels.Length; i++)
                labels[i] = (i + 1).ToString();
            foreach (var n in names)
            {
                if (n.Name == name && n.BirthDate.Day != 1)
                    countNames[n.BirthDate.Day-1] += 1;
            }
            return new HistogramData(
                string.Format("Рождаемость людей с именем '{0}'", name), 
                labels, 
                countNames);
        }
    }
}