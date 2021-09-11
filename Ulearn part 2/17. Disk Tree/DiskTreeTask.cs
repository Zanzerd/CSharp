using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskTree
{
    public static class DiskTreeTask
    {
        public static List<string> Solve(List<string> input)
        {
            //var dirList = new List<DirectoryNode>();
            var rootDir = new DirectoryNode(null, null);
            foreach(var dir in input)
            {
                var dirSplit = dir.Split('\\');
                rootDir.Add(dir, dirSplit, rootDir);
            }
            return rootDir.Evaluate().ToList();
        }
    }

    public class DirectoryNode
    {
        public string dirPath;
        public string shortName;
        public List<DirectoryNode> Children;
        public DirectoryNode parent;
        public int depth;

        public DirectoryNode(string dirName, string shortName, DirectoryNode parent)
        {
            dirPath = dirName;
            this.parent = parent;
            depth = parent.depth + 1;
            var spaces = new String(' ', depth);
            this.shortName = spaces + shortName;
            Children = new List<DirectoryNode>();
        }

        public DirectoryNode(string dirName, string shortName)
        {
            dirPath = dirName;
            this.shortName = shortName;
            depth = -1;
            Children = new List<DirectoryNode>();
        }

        public void Add(string dirName, string[] dirSplit, DirectoryNode node, int count = 0)
        {
            for (int i = 0; i < dirSplit.Length; i++)
            {
                bool flag = false;
                DirectoryNode newNode = null;
                var tempDirSplit = string.Join("\\", dirSplit.Take(i+1).ToArray());
                if (node.Children.Count > 0)
                {
                    foreach (var c in node.Children)
                    {
                        if (c.dirPath == tempDirSplit)
                        {
                            newNode = c;
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        newNode = new DirectoryNode(string.Join("\\", tempDirSplit), dirSplit[i], node);
                        node.Children.Add(newNode);
                    }
                }
                else
                {
                    newNode = new DirectoryNode(string.Join("\\", tempDirSplit), dirSplit[i], node);
                    node.Children.Add(newNode);
                }
                node = newNode;
            }
            return;
        }

        public IEnumerable<string> Evaluate()
        {
            if (shortName != null)
                yield return shortName;
            if (Children.Count > 0)
            {
                Children.Sort((DirectoryNode x, DirectoryNode y) 
                    => { return string.Compare(x.shortName, y.shortName, StringComparison.Ordinal); });
                foreach (var c in Children)
                {
                    foreach (var d in c.Evaluate())
                    {
                        yield return d;
                    }
                }
            }
        }
    }
}
