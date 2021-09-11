using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greedy.Architecture;
using System.Drawing;

namespace Greedy
{
    class DijkstraData
    {
        public Point Previous { get; set; }
        public int Price { get; set; }

        public DijkstraData(Point previous, int price)
        {
            Previous = previous;
            Price = price;
        }
    }
    public class DijkstraPathFinder
    {
        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
            IEnumerable<Point> targets)
        {
            var chestsSet = targets.ToHashSet();
            var openPoints = new HashSet<Point>();
            var track = new Dictionary<Point, DijkstraData>();
            var noPoint = new Point(-int.MaxValue, -int.MaxValue);
            int foundChestsCount = 0;
            int mapCount = state.MapHeight * state.MapWidth;
            track[start] = new DijkstraData(noPoint, 0);
            while (true)
            {
                Point toOpen = noPoint;
                var bestPrice = double.PositiveInfinity;
                foreach (var point in track.Keys)
                {
                    if (!openPoints.Contains(point) && track[point].Price < bestPrice)
                    {
                        bestPrice = track[point].Price;
                        toOpen = point;
                    }
                }
                if (toOpen == noPoint) break;
                if (chestsSet.Contains(toOpen))
                {
                    var pointer = toOpen;
                    var path = new List<Point>();
                    int cost = track[toOpen].Price;
                    while (pointer != noPoint)
                    {
                        path.Add(pointer);
                        pointer = track[pointer].Previous;
                    }
                    path.Reverse();
                    yield return new PathWithCost(cost, path.ToArray());
                    foundChestsCount++;
                    if (foundChestsCount == chestsSet.Count)
                        break;
                }
                var arrOfD = new (int, int)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
                foreach ((var dx, var dy) in arrOfD)
                {
                    var nextPoint = new Point(toOpen.X + dx, toOpen.Y + dy);
                    if (state.InsideMap(nextPoint))
                    {
                        if (state.IsWallAt(nextPoint))
                            openPoints.Add(nextPoint);
                        else
                        {
                            var currPrice = track[toOpen].Price + state.CellCost[nextPoint.X, nextPoint.Y];
                            if (!track.ContainsKey(nextPoint) || track[nextPoint].Price > currPrice)
                                track[nextPoint] = new DijkstraData(toOpen, currPrice);
                        }
                    }
                }
                openPoints.Add(toOpen);
                if (openPoints.Count == mapCount)
                    break;
            }
        }
    }
}
