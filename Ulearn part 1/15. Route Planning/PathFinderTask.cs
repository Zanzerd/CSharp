using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RoutePlanning
{
	public static class PathFinderTask
	{
		public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
		{
            var permutationsList = new List<int[]>();
            MakePermutations(new int[checkpoints.Length], 0, permutationsList);
            permutationsList = DeleteNotFromZeroPaths(permutationsList);
            var prices = EvaluateList(permutationsList, checkpoints);
            int minPriceIndex = prices.IndexOf(prices.Min());
            return permutationsList[minPriceIndex];
        }

        static List<int[]> DeleteNotFromZeroPaths(List<int[]> permutationsList)
        {
            for (int i = permutationsList.Count - 1; i >= 0; i--)
                if (permutationsList[i][0] != 0)
                    permutationsList.RemoveAt(i);
            return permutationsList;
        }
        static List<double> EvaluateList(List<int[]> permutationsList, Point[] checkpoints)
        {
            var prices = new List<double>();
            foreach (var permutation in permutationsList)
            {
                double len = PointExtensions.GetPathLength(checkpoints, permutation);
                prices.Add(len);
            }
            return prices;
        }
		static void MakePermutations(int[] permutation, int position, List<int[]> permutationsList)
        {
			if (position == permutation.Length)
            {
                var permutation2 = new int[permutation.Length];
                Array.Copy(permutation, permutation2, permutation.Length);
                permutationsList.Add(permutation2);
                return;
            }
            for (int i = 0; i < permutation.Length; i++)
            {
                var index = Array.IndexOf(permutation, i, 0, position);
                if (index != -1)
                    continue;
                permutation[position] = i;
                MakePermutations(permutation, position + 1, permutationsList);
            }
        }

	}
}