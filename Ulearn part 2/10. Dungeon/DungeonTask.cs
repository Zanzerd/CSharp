using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
	public class DungeonTask
	{
		public static MoveDirection[] FindShortestPath(Map map)
		{
			var pathPlayerExit = BfsTask.FindPaths(map, map.InitialPosition, new[] { map.Exit })
				.FirstOrDefault();
			var pathSet = new HashSet<Point>();
			if (pathPlayerExit != null)
				pathSet = pathPlayerExit.ToHashSet();
			else
				return new MoveDirection[] { };
			var pathPlayerExitFinal = SingleListToDirections(pathPlayerExit);
			foreach (var chest in map.Chests)
				if (pathSet.Contains(chest))
					return pathPlayerExitFinal.ToArray();
			var pathsExitManyChests = BfsTask.FindPaths(map, map.Exit, map.Chests);
			SinglyLinkedList<Point> exitToChest = null;
			var min = int.MaxValue;
			if (pathsExitManyChests == null || !pathsExitManyChests.Any())
				return pathPlayerExitFinal.ToArray();
			var pathsChecked = new HashSet<SinglyLinkedList<Point>>();
			MoveDirection[] minPath = null;
			foreach (var path in pathsExitManyChests)
			{
				//if (curr < min && !pathsChecked.Contains(path))
				//{
				//	exitToChest = path;
				//	min = curr;
				//	pathsChecked.Add(path);
				//}
				exitToChest = path;
				var chestToExitFinal = SingleListToDirectionsReverse(exitToChest).ToList();
				var whichChest = exitToChest.Value;
				var pathPlayerChestNNN = BfsTask.FindPaths(map, map.InitialPosition, new[] { whichChest });
				var pathPlayerChest = pathPlayerChestNNN.FirstOrDefault();
				if (pathPlayerChest == null || !pathPlayerChest.Any())
					continue;
				var pathPlayerChest2 = SingleListToDirections(pathPlayerChest);
				var pathPlayerChestFinal = pathPlayerChest2
					.ToList();
				//var completePath = RemoveAndConcat(pathPlayerChestFinal, chestToExitFinal);
				var completePath = new MoveDirection[pathPlayerChestFinal.Count + chestToExitFinal.Count];
				pathPlayerChestFinal.CopyTo(completePath, 0);
				chestToExitFinal.CopyTo(completePath, pathPlayerChestFinal.Count);
				if (completePath.Length < min)
				{
					min = completePath.Length;
					minPath = completePath;
				}
				else
					continue;
			}
			if (minPath != null)
				return minPath;
			return new MoveDirection[] { };
		}

		public static List<MoveDirection> SingleListToDirections(SinglyLinkedList<Point> sll)
		{
			var resultList = new List<MoveDirection>();
			while (sll.Previous != null)
			{
				if (sll.Previous.Value.X == sll.Value.X + 1)
					resultList.Add(MoveDirection.Left);
				else if (sll.Previous.Value.X == sll.Value.X - 1)
					resultList.Add(MoveDirection.Right);
				else if (sll.Previous.Value.Y == sll.Value.Y + 1)
					resultList.Add(MoveDirection.Up);
				else
					resultList.Add(MoveDirection.Down);

				sll = sll.Previous;
			}
			resultList.Reverse();
			return resultList;
		}

		public static IEnumerable<MoveDirection> SingleListToDirectionsReverse(SinglyLinkedList<Point> sll)
        {
			var resultList = new List<MoveDirection>();
			while (sll.Previous != null)
			{
				if (sll.Previous.Value.X == sll.Value.X + 1)
					resultList.Add(MoveDirection.Right);
				else if (sll.Previous.Value.X == sll.Value.X - 1)
					resultList.Add(MoveDirection.Left);
				else if (sll.Previous.Value.Y == sll.Value.Y + 1)
					resultList.Add(MoveDirection.Down);
				else
					resultList.Add(MoveDirection.Up);

				sll = sll.Previous;
			}
			return resultList;
		}
	}
}
