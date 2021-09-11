using System;
using System.Collections.Generic;
using System.Linq;

namespace Clones
{

	public class StackItem<T>
	{
		public T Value { get; set; }
		public StackItem<T> Next { get; set; }
	}

	public class One_sided_stack<T>
    {
		public int Counter { get; set; } = 0;
		public StackItem<T> Head { get; set; }
		public StackItem<T> Tail { get; set; }

		public One_sided_stack()
        {
			Head = Tail = null;
        }

		public void Push(T item)
        {
			StackItem<T> NewItem = new StackItem<T> { Value = item };

			if (Head == null)
			{
				Head = Tail = NewItem;
				Counter++;
			}
			else
            {
				NewItem.Next = Head;
				Head = NewItem;
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
				Head = Head.Next;
				return result.Value;
            }
		}

		public int Count
		{
			get
			{
				return Counter;
			}
		}
	}

	public class Clonian : ICloneable
    {
		public int number { get; set; } = 1;
		public One_sided_stack<string> programs { get; set; } = new One_sided_stack<string>();
		public One_sided_stack<string> rollback_history { get; set; } = new One_sided_stack<string>();
		public object Clone()
        {
			One_sided_stack<string> programs2 = new One_sided_stack<string>();
			One_sided_stack<string> rollback_history2 = new One_sided_stack<string>();
			programs2.Head = this.programs.Head;
			rollback_history2.Head = this.rollback_history.Head;
			return new Clonian { programs = programs2, rollback_history = rollback_history2, number = this.number + 1 };
        }
    }

	public class CloneVersionSystem : ICloneVersionSystem
	{
		public List<Clonian> CloneList = new List<Clonian>();
		public CloneVersionSystem() 
		{
			CloneList.Add(new Clonian { number = 1 });
		}
		public string Execute(string query)
		{
			string result = null;
			string[] s_arr = query.Split();
			switch(s_arr[0])
            {
				case "learn":
					CloneList[Int32.Parse(s_arr[1])-1].programs.Push(s_arr[2]);
					break;
				case "rollback":
					CloneList[Int32.Parse(s_arr[1])-1].rollback_history.Push(CloneList[Int32.Parse(s_arr[1])-1].programs.Pop());
					break;
				case "relearn":
					CloneList[Int32.Parse(s_arr[1])-1].programs.Push(CloneList[Int32.Parse(s_arr[1])-1].rollback_history.Pop());
					break;
				case "clone":
					CloneList.Add((Clonian)CloneList[Int32.Parse(s_arr[1]) - 1].Clone());
					break;
				case "check":
					if (CloneList[Int32.Parse(s_arr[1])-1].programs.Head == null)
						result = "basic";
					else
					{
						result = CloneList[Int32.Parse(s_arr[1])-1].programs.Pop();
						CloneList[Int32.Parse(s_arr[1])-1].programs.Push(result);
					}
					break;
				default:
					break;
			}
            
			return result;
		}
	}
}
