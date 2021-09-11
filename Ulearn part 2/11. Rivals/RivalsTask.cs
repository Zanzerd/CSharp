using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rivals
{
	public class RivalsTask
	{
		public static IEnumerable<OwnedLocation> AssignOwners(Map map)
		{
			var locationsSet = new HashSet<OwnedLocation>();
			var playersSet = map.Players.ToHashSet();
			var queue = new Queue<OwnedLocation>();
			var locationsDict = new Dictionary<Point, (int, int)>(); // точка, владелец и дистанция
			var visited = new HashSet<Point>();
			for (int i = 0; i < map.Players.Length; i++)
			{
				var startPoint = map.Players[i];
				var player = new OwnedLocation(i, startPoint, 0);
				queue.Enqueue(player);
				visited.Add(startPoint);
				locationsSet.Add(player);
				locationsDict[startPoint] = (i, 0);
			}
			while (queue.Count != 0)
			{
				var currLoc = queue.Dequeue();
				if (!locationsDict.ContainsKey(currLoc.Location) ||
					(locationsDict[currLoc.Location].Item2 > currLoc.Distance))
				{
					locationsDict[currLoc.Location] = (currLoc.Owner, currLoc.Distance);
					//locationsSet.RemoveWhere(loc => loc.Location == currLoc.Location);
					locationsSet.Add(currLoc);
				}
				var arrOfD = new (int, int)[] { (1, 0), (0, 1), (0, -1), (-1, 0) };
				foreach ((var dx, var dy) in arrOfD)
				{
					var nextPoint = new Point(currLoc.Location.X + dx, currLoc.Location.Y + dy);
					if (map.InBounds(nextPoint) && !visited.Contains(nextPoint) &&
						map.Maze[nextPoint.X, nextPoint.Y] == MapCell.Empty && !playersSet.Contains(nextPoint))
					{
						var nextLoc = new OwnedLocation(currLoc.Owner, nextPoint, currLoc.Distance + 1);
						queue.Enqueue(nextLoc);
						visited.Add(nextPoint);
					}
				}
			}
			return locationsSet;
		}
	}
}
