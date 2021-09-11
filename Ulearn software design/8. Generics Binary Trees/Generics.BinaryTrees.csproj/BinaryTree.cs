using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T>
        where T : IComparable
    {
        public T Value { get; private set; }
        public BinaryTree<T> Left { get; private set; }
        public BinaryTree<T> Right { get; private set; }
        public BinaryTree<T> Parent { get; private set; }
        private bool isEmpty = true;
        private int mass;

        public T this[int index]
        {
            get
            {
                return getValByIndex(index, this, 0);
            }
        }

        private T getValByIndex(int index, BinaryTree<T> node, int cache)
        {
            if (isEmpty)
                throw new IndexOutOfRangeException();
            int nodeIndex = cache;
            if (node.Parent == null)
                nodeIndex = node.Left == null ? 0 : node.Left.mass;
            else
            {
                if (node == node.Parent.Left)
                    nodeIndex = node.Right == null ? cache - 1 : cache - node.Right.mass - 1;
                else
                    nodeIndex = node.Left == null ? cache + 1 : cache + node.Left.mass + 1;
            }
            if (index == nodeIndex)
                return node.Value;
            else if (index < nodeIndex)
            {
                if (node.Left != null)
                    return getValByIndex(index, node.Left, nodeIndex);
                throw new IndexOutOfRangeException();
            }
            else
            {
                if (node.Right != null)
                {
                    return getValByIndex(index, node.Right, nodeIndex);
                }
                throw new IndexOutOfRangeException();
            }
        }

        public BinaryTree()
        {
            Value = default(T);
            Left = null;
            Right = null;
            Parent = null;
        }

        public void Add(T value)
        {
            var currNode = this;
            BinaryTree<T> parentNode;
            var newNode = new BinaryTree<T>();
            newNode.Value = value;
            newNode.isEmpty = false;

            if (currNode == null)
                return;
            if (currNode.isEmpty)
            {
                Value = value;
                isEmpty = false;
                mass = 1;
                return;
            }

            while (true)
            {
                parentNode = currNode.Parent;
                if (parentNode != null)
                    parentNode.mass++;
                if (value.CompareTo(currNode.Value) <= 0)
                {
                    if (currNode.Left == null)
                    {
                        currNode.Left = newNode;
                        newNode.Parent = currNode;
                        newNode.mass = 1;
                        return;
                    }
                    else
                        currNode = currNode.Left;
                }
                else 
                {
                    if (currNode.Right == null)
                    {
                        currNode.Right = newNode;
                        newNode.Parent = currNode;
                        newNode.mass = 1;
                        return;
                    }
                    else
                        currNode = currNode.Right;
                }
            }
        }

        private IEnumerable<T> getValsSorted(BinaryTree<T> node)
        {
            if (node == null || node.isEmpty)
                yield break;
            foreach (var el in getValsSorted(node.Left))
                yield return el;
            yield return node.Value;
            foreach (var el in getValsSorted(node.Right))
                yield return el;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var el in getValsSorted(this))
                yield return el;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class BinaryTree
    {
        public static BinaryTree<T> Create<T>(params T[] paramsList)
            where T : IComparable
        {
            var root = new BinaryTree<T>();
            foreach (var element in paramsList)
            {
                root.Add(element);
            }
            return root;
        }
    }
}
