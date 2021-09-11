using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
	public class GreedyPathFinder : IPathFinder
	{
		public List<Point> FindPathToCompleteGoal(State state)
		{
			int chestCount = 0;
			var path = new List<Point>();
			var from = state.Position;
			var to = state.Chests.ToHashSet();
			int cost = 0;
			while(chestCount < state.Goal)
            {
				var dijkstraPathFinder = new DijkstraPathFinder();
				var pathPart = dijkstraPathFinder.GetPathsByDijkstra(state, from, to);
				if (pathPart == null || !pathPart.Any())
					break;
				var firstPath = pathPart.FirstOrDefault();
				var prevPath = new List<Point>(path);
				path = path.Concat(firstPath.Path.Skip(1)).ToList();
				from = path.LastOrDefault();
				to.Remove(from);
				cost += firstPath.Cost - state.CellCost[from.X, from.Y];
				if (cost > state.Energy)
					return prevPath;
				chestCount++;
            }
			if (chestCount < state.Goal)
				return new List<Point>();
			return path;
		}
	}
}