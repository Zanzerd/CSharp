using System;
using System.Collections.Generic;

namespace TodoApplication
{
    public class Action<Titem>
    {
        public int index;
        public bool Deletion;
        public bool Addition;
        public StackItem<Titem> DeletedItem;
        public Action()
        {
            index = -1;
            Deletion = false;
            Addition = false;
            DeletedItem = new StackItem<Titem>();
        }
    }
    public class ListModel<TItem>
    {
        public List<TItem> Items { get; }
        public LimitedSizeStack<Action<TItem>> Actions { get; set; }
        public int Limit;
        

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            Limit = limit;
            Actions = new LimitedSizeStack<Action<TItem>>(limit);
        }

        public void AddItem(TItem item)
        {
            Action<TItem> Act = new Action<TItem>() { Addition = true };
            Items.Add(item);
            Actions.Push(Act);
        }

        public void RemoveItem(int index)
        {
            Action<TItem> Act = new Action<TItem>() { Deletion = true };
            Act.DeletedItem.Value = Items[index];
            Act.index = index;
            Items.RemoveAt(index);
            Actions.Push(Act);
        }

        public bool CanUndo()   
        {
            if (Actions.Count > 0 && Limit > 0)
                return true;
            else
                return false;
        }

        public void Undo()
        {
            if (CanUndo())
            {
                Action<TItem> action = Actions.Pop();
                if (action.Addition)
                    UndoAddition();
                if (action.Deletion)
                    UndoDeletion(action.index, action.DeletedItem.Value);
            }
        }

        public void UndoAddition()
        {
            Items.RemoveAt(Items.Count - 1);
        }

        public void UndoDeletion(int index, TItem DeletedItem)
        {
            Items.Insert(index, DeletedItem);
        }

    }
}
