using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        public T Value { get; set; }
        private BinaryTree<T> Left { get; set; }
        private BinaryTree<T> Right { get; set; }
        private BinaryTree<T> Parent { get; set; }
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

        public BinaryTree(T value, BinaryTree<T> left, BinaryTree<T> right)
        {
            Value = value;
            Left = left;
            Right = right;
            Parent = null;
            isEmpty = false;
            mass = 1;
        }

        public BinaryTree()
        {
            Left = null;
            Right = null;
            Parent = null;
        }

        public void Add(T value)
        {
            BinaryTree<T> currNode = this;
            BinaryTree<T> prevNode = null;
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
                prevNode = currNode;
                prevNode.mass++;
                if (value.CompareTo(prevNode.Value) < 0)
                {
                    currNode = prevNode.Left;
                    if (currNode == null)
                    {
                        prevNode.Left = new BinaryTree<T>(value, null, null);
                        prevNode.Left.Parent = prevNode;
                        prevNode.Left.mass = 1;
                        return;
                    }
                }
                else
                {
                    currNode = prevNode.Right;
                    if (currNode == null)
                    {
                        prevNode.Right = new BinaryTree<T>(value, null, null);
                        prevNode.Right.Parent = prevNode;
                        prevNode.Right.mass = 1;
                        return;
                    }
                }
            }
        }

        public bool Contains(T value)
        {
            if (!isEmpty)
            {
                var currNode = this;
                while (currNode != null)
                {
                    if (currNode.Value.Equals(value))
                    {
                        return true;
                    }
                    else if (value.CompareTo(currNode.Value) < 0)
                    {
                        currNode = currNode.Left;
                    }
                    else
                    {
                        currNode = currNode.Right;
                    }
                }
            }
            return false;
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
}
