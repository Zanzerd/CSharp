using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication
{

    public class StackItem<T>
    {
        public T Value { get; set; }
        public StackItem<T> Next { get; set; }
        public StackItem<T> Prev { get; set; }

    }
    public class LimitedSizeStack<T>
    {
        public int Limit { get; set; }
        public int Counter { get; set; } = 0;
        public StackItem<T> Head { get; set; }
        public StackItem<T> Tail { get; set; }
        
        
        public LimitedSizeStack(int limit)
        {
            Limit = limit;
            Head = Tail = null;
        }

        public void Push(T item)
        {
            StackItem<T> NewItem = new StackItem<T> { Value = item };
            if (Limit == 0)
            {

            }
            else
            {
                if (Head == null)
                {
                    Head = Tail = NewItem;
                    Counter++;
                }
                else
                {
                    Head.Next = NewItem;
                    StackItem<T> pointer = Head;
                    Head = NewItem;
                    Head.Prev = pointer;
                    if (Counter == Limit)
                    {
                        Tail = Tail.Next;
                        Tail.Prev = null;
                    }
                    else
                        Counter++;
                }
            }
        }

        public T Pop()
        {
            StackItem<T> result;
            if (Head == null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                result = Head;
                Head = Head.Prev;
                if (Head != null)
                    Head.Next = null;
                Counter--;
            }
            if (Head == null)
                Tail = null;
            return result.Value;
        }

        public int Count
        {
            get
            {
                return Counter;
            }
        }
    }
}
