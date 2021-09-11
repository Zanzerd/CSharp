using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
	class PointData
    {
		Point from;
		Point to;
		public PointData(Point from, Point to)
        {
			this.from = from;
			this.to = to;
        }
    }
	public class NotGreedyPathFinder : IPathFinder
	{
		public List<Point> FindPathToCompleteGoal(State state)
		{
			var energy = state.InitialEnergy;
			var pathsBetweenTargets = new Dictionary<(Point, Point), PathWithCost>();
			var pathsFinal = new Dictionary<int, List<Point>>();
			//var setHelper = new HashSet<(Point, Point)>();
			var targetsSet = state.Chests.ToHashSet();
			targetsSet.Add(state.Position);
			var dijkstraPathFinder = new DijkstraPathFinder();
			foreach (var target1 in targetsSet)
			{
				foreach (var target2 in targetsSet)
				{
					if (target1 != target2)
					{
						PathWithCost path = null;
						if (!pathsBetweenTargets.ContainsKey((target1, target2))
							&& !pathsBetweenTargets.ContainsKey((target2, target1)))
						{
							path = dijkstraPathFinder.GetPathsByDijkstra(state, target1, new[] { target2 })
								.SingleOrDefault();
							if (path != null && path.Path.Any())
							{
								//pathList = path.Path.Skip(1).ToList();
								pathsBetweenTargets[(target1, target2)] = path;
								var reversePath = new Point[path.Path.Count];
								int reverseCost = path.Cost - state.CellCost[path.End.X, path.End.Y] +
									state.CellCost[path.Start.X, path.Start.Y];
								
								path.Path.CopyTo(reversePath);
								Array.Reverse(reversePath);
								pathsBetweenTargets[(target2, target1)] = new PathWithCost(reverseCost, reversePath);
							}
						}
					}
				}
			}
			targetsSet.Remove(state.Position);
			var initialPermutation = new Point[targetsSet.Count()];
			var noPoint = new Point(-1, -1);
			for (int i = 0; i < initialPermutation.Length; i++)
			{
				initialPermutation[i] = noPoint;
			}
			var targetsPermutations = MakePermutations(initialPermutation, 0, targetsSet);
			var listOfCosts = new List<int>();
			int c = 0;
			foreach (var el in targetsPermutations)
			{
				c++;
				var permutation = new Point[el.Length + 1];
				permutation[0] = state.Position;
				Array.Copy(el, 0, permutation, 1, el.Length);
				int costOfPermutation = 0;
				int chestsCount = 0;
				int currMax = 0;
				var maxSet = new HashSet<int>();
				var pathOfPermutation = new List<Point>();
				var skippedSet = new HashSet<Point>();
				for (int i = 0; i < permutation.Length - 1; i++)
				{
					if (pathsBetweenTargets.ContainsKey((permutation[i], permutation[i + 1])))
					{
						//if (skippedSet.Contains(permutation[i + 1]))
							//break;
						var pathWithCost = pathsBetweenTargets[(permutation[i], permutation[i + 1])];
						costOfPermutation += pathWithCost.Cost;
						if (costOfPermutation > energy)
						{
							break;
						}
						listOfCosts.Add(costOfPermutation);
						var pathPart = pathsBetweenTargets[(permutation[i], permutation[i + 1])]
							.Path.Skip(1) // что насчёт cost здесь? 
							.ToList();
						var tempIntersect = pathPart
							.Take(pathPart.Count - 1)
							.Intersect(targetsSet);
						if (tempIntersect.Any())
							skippedSet.UnionWith(tempIntersect);
						pathOfPermutation = pathOfPermutation.Concat(pathPart).ToList();
						chestsCount++;
						if (chestsCount > currMax)
                        {
							currMax = chestsCount;
							maxSet.Add(currMax);
                        }
						if (chestsCount == targetsSet.Count)
							break;
					}
					else
					{
						break;
					}
				}
				if (pathOfPermutation != null) //проверять ли на ContainsKey?
				{
					pathsFinal[chestsCount] = pathOfPermutation;
				}
				if (c == 15000 && currMax == maxSet.Max())
					break;
			}
			listOfCosts = listOfCosts.OrderByDescending(d => d).ToList();
			var sortedDict = pathsFinal.OrderByDescending(kvp => kvp.Key);
			return sortedDict.FirstOrDefault().Value;
		}


		static IEnumerable<Point[]> MakePermutations(Point[] permutation, int position, HashSet<Point> targetsSet)
		{

			if (position == permutation.Length)
				yield return permutation;
			else
			{
				foreach (var el in targetsSet)
				{
					var index = Array.IndexOf(permutation, el, 0, position);
					if (index != -1)
						continue;
					permutation[position] = el;
					var permutationCopy = new Point[permutation.Length];
					Array.Copy(permutation, permutationCopy, permutation.Length);
					foreach (var x in MakePermutations(permutationCopy, position + 1, targetsSet))
						yield return x;
				}
			}
		}
	}
}