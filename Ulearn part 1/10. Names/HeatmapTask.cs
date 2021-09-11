using System;

namespace Names
{
    internal static class HeatmapTask
    {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            double[,] countNames = new double[30,12];
            string[] labelsDays = new string[30];
            string[] labelsMonths = new string[12];

            for (int i = 0; i < labelsDays.Length; i++)
                labelsDays[i] = (i + 2).ToString();
            for (int i = 0; i < labelsMonths.Length; i++)
                labelsMonths[i] = (i + 1).ToString();

                foreach (var month in labelsMonths)
                {
                    int monthInt = Int32.Parse(month);
                    foreach (var name in names)
                    if (name.BirthDate.Month == monthInt && name.BirthDate.Day != 1)
                        countNames[name.BirthDate.Day - 2, monthInt-1]++;
                }

            return new HeatmapData(
                "Пример карты интенсивностей",
                countNames, 
                labelsDays, 
                labelsMonths);
        }
    }
}