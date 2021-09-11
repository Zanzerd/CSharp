using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class BfsTask
    {
        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            int count = 0;
            //var beforeStart = new SinglyLinkedList<Point>(new Point(-1, -1));
            var checkedChests = new HashSet<Point>();
            var chestsSet = chests.ToHashSet();
            foreach (var chest in chests)
            {
                var sll = new SinglyLinkedList<Point>(start);
                if (count == chestsSet.Count()) break;
                //var chest = beforeStart;
                var queue = new Queue<SinglyLinkedList<Point>>();
                var visited = new HashSet<Point>();
                queue.Enqueue(sll);
                var path = new Dictionary<SinglyLinkedList<Point>, SinglyLinkedList<Point>>();
                //path[sll] = beforeStart;
                //bool flag = false;
                while (queue.Count != 0)
                {
                    var sllCurr = queue.Dequeue();
                    var valueCurr = sllCurr.Value;
                    visited.Add(valueCurr);
                    var arrOfD = new (int, int)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
                    foreach ((var dx, var dy) in arrOfD)
                    { 
                        var nextPoint = new Point(valueCurr.X + dx, valueCurr.Y + dy);
                        if (map.InBounds(nextPoint) && !visited.Contains(nextPoint) &&
                        map.Dungeon[nextPoint.X, nextPoint.Y] == MapCell.Empty)
                        {
                            var newSll = new SinglyLinkedList<Point>(nextPoint, sllCurr);
                            //path[newSll] = sllCurr;
                            queue.Enqueue(newSll);
                            if (chestsSet.Contains(nextPoint) && !checkedChests.Contains(nextPoint))
                            {
                                checkedChests.Add(nextPoint);
                                visited.Add(nextPoint);
                                //flag = true; этот флаг вообще не нужен
                                yield return newSll;
                                count++;
                            }
                        }
                    }
                    
                }
            }
        }
    }
}
            