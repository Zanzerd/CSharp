using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    public class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Terrain { } };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Player;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }

    public class Player : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            var command = new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Player { } };
            switch (Game.KeyPressed)
            {
                case Keys.Up:
                    command.DeltaY--;
                    break;
                case Keys.Down:
                    command.DeltaY++;
                    break;
                case Keys.Left:
                    command.DeltaX--;
                    break;
                case Keys.Right:
                    command.DeltaX++;
                    break;
                default:
                    break;
            }
            if (x + command.DeltaX >= Game.MapWidth || x + command.DeltaX < 0 || 
                Game.Map[x + command.DeltaX, y] is Sack)
                command.DeltaX = 0;
            if (y + command.DeltaY >= Game.MapHeight || y + command.DeltaY < 0 ||
                Game.Map[x, y + command.DeltaY] is Sack)
                command.DeltaY = 0;
            return command;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            
            if (conflictedObject is Sack && ((Sack)conflictedObject).playerDeath)
                return true;
            return conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }
    }

    public class Sack : ICreature
    {
        public int fallTickCounter = 0;
        public bool playerDeath = false;

        public CreatureCommand Act(int x, int y)
        {
            var command = new CreatureCommand { DeltaX = 0, DeltaY = 0 };
            if (y + 1 < Game.MapHeight && Game.Map[x, y + 1] is null)
            {
                command.DeltaY++;
                fallTickCounter++;
            }
            else if (y + 1 < Game.MapHeight && Game.Map[x, y + 1] is Player && fallTickCounter > 0)
                playerDeath = true;
            else if (y + 1 < Game.MapHeight && Game.Map[x, y + 1] is Monster && fallTickCounter > 0)
            {
                Game.Map[x, y + 1] = null;
                command.DeltaY++;
            }
            else
            {
                if (fallTickCounter > 1)
                    command.TransformTo = new Gold { };
                fallTickCounter = 0;
            }
            if (playerDeath == true)
            {
                Game.Map[x, y + 1] = null; // ИСКУСТВЕННАЯ ХРЕНЬ, вообще надо было бы заставить работать
                playerDeath = false;       // метод Player.DeathInConflict
                command.DeltaY++;
            }
            return command;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Sack.png"; 
        }
    }

    public class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Gold { } };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Player)
            {
                Game.Scores += 10;
                return true;
            }
            return conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }

    public class Monster : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            var command = new CreatureCommand { TransformTo = new Monster { } };
            int xPlayer = -1;
            int yPlayer = -1;
            List<(int, int)> shortPath;
            for (int i = 0; i < Game.Map.GetLength(0); i++)
            {
                for (int j = 0; j < Game.Map.GetLength(1); j++)
                {
                    if (Game.Map[i, j] is Player)
                    {
                        xPlayer = i;
                        yPlayer = j;
                    }
                }
            }
            if (xPlayer != -1 && yPlayer != -1)
            {
                var bfs = new BFSAlgo { };
                shortPath = bfs.BFS(Game.Map, x, y, xPlayer, yPlayer);
                if (shortPath[0] == (-1, -1))
                {
                    command.DeltaX = 0;
                    command.DeltaY = 0;
                }
                else
                {
                    var probDeltaX = shortPath[1].Item1 - x;
                    var probDeltaY = shortPath[1].Item2 - y;
                    if (Game.Map[x + probDeltaX, y + probDeltaY] is Monster)
                    {
                        command.DeltaX = 0;
                        command.DeltaY = 0;
                    }
                    else
                    {
                        command.DeltaX = probDeltaX;
                        command.DeltaY = probDeltaY;
                    }
                }
            }
            else
            {
                command.DeltaX = 0;
                command.DeltaY = 0;
            }
            return command;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName()
        {
            return "Monster.png";
        }
    }

    public class BFSAlgo
    {
        public bool[,] visited;
        public Dictionary<(int, int), (int, int)> prevDict;
        public Queue<(int, int)> queue;

        public List<(int, int)> BFS(ICreature[,] Map, int startX, int startY, int endX, int endY)
        {
            visited = new bool[Game.Map.GetLength(0), Game.Map.GetLength(1)];
            queue = new Queue<(int, int)>();
            List<(int, int)> resultPath = new List<(int, int)>();
            prevDict = new Dictionary<(int, int), (int, int)>();
            for (int i = 0; i < visited.GetLength(0); i++)
                for (int j = 0; j < visited.GetLength(1); j++)
                    visited[i, j] = false;
            queue.Enqueue((startX, startY));
            visited[startX, startY] = true;
            while (queue.Count != 0)
            {
                (var xCur, var yCur) = queue.Dequeue();
                if (xCur == endX && yCur == endY)
                    resultPath.Add((-1, -1)); ///ЧЕКАТЬ ЕСЛИ КООРДИНАТЫ СУЩЕСТВУЮТ 
                var neighbours = new List<(int, int)>();
                if (xCur + 1 < Map.GetLength(0)) neighbours.Add((xCur + 1, yCur));
                if (yCur + 1 < Map.GetLength(1)) neighbours.Add((xCur, yCur + 1));
                if (xCur - 1 >= 0) neighbours.Add((xCur - 1, yCur));
                if (yCur - 1 >= 0) neighbours.Add((xCur, yCur - 1));
                foreach (var neighbour in neighbours)
                {
                    if (visited[neighbour.Item1, neighbour.Item2] == false && !(Map[neighbour.Item1, neighbour.Item2]
                        is Sack) && !(Map[neighbour.Item1, neighbour.Item2] is Terrain))
                    {
                        queue.Enqueue(neighbour);
                        visited[neighbour.Item1, neighbour.Item2] = true;
                        if (!prevDict.ContainsKey(neighbour))
                            prevDict[neighbour] = (xCur, yCur);
                    }
                }
            }
            if (visited[endX, endY] == false)
                resultPath.Add((-1, -1));
            else
            {
                var cur = (endX, endY);
                resultPath.Add(cur);
                while (prevDict.ContainsKey(cur))
                {
                    cur = prevDict[cur];
                    resultPath.Add(cur);
                }
            }
            resultPath.Reverse();
            return resultPath; // FFFFF
        }
    }
}

     