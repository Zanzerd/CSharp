using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        public string Name { get; private set; }
        public MessageTopic MessageTopic { get; private set; }
        public MessageType MessageType { get; private set; }
        private int hash = 16777619;
        public Category(string name, MessageType type, MessageTopic topic)
        {
            Name = name;
            MessageTopic = topic;
            MessageType = type;
        }

        public override bool Equals(object obj)
        {
            var objAsCategory = obj as Category;
            if (!(objAsCategory is null))
            {
                if (objAsCategory.Name == this.Name
                    && this.MessageTopic.Equals(objAsCategory.MessageTopic)
                    && this.MessageType.Equals(objAsCategory.MessageType))
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                foreach (var c in Name)
                {
                    hash *= 2171;
                    hash ^= c.GetHashCode();
                }
                hash += (int)MessageType * 217;
                hash += (int)MessageTopic * 219;
            }
            return hash;
        }

        public override string ToString()
        {
            return Name + "." + MessageType.ToString() + "." + MessageTopic.ToString();
        }

        public int CompareTo(object obj)
        {
            var objAsCategory = obj as Category;
            if (objAsCategory is null)
                return 1; 
            if(this.Name is null)
            {
                if (objAsCategory.Name is null)
                    return 0;
                else
                    return -1;
            }
            if (this.Name.CompareTo(objAsCategory.Name) < 0)
                return -1;
            else if (this.Name.CompareTo(objAsCategory.Name) > 0)
                return 1;
            else
            {
                if (this.MessageType.CompareTo(objAsCategory.MessageType) < 0)
                    return -1;
                else if (this.MessageType.CompareTo(objAsCategory.MessageType) > 0)
                    return 1;
                else
                {
                    if (this.MessageTopic.CompareTo(objAsCategory.MessageTopic) < 0)
                        return -1;
                    else if (this.MessageTopic.CompareTo(objAsCategory.MessageTopic) > 0)
                        return 1;
                    else
                        return 0;
                }
            }
        }

        public static bool operator< (Category a, Category b)
        {
            return a.CompareTo(b) < 0;
        }
        
        public static bool operator> (Category a, Category b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator== (Category a, Category b)
        {
            return a.CompareTo(b) == 0;
        }

        public static bool operator!= (Category a, Category b)
        {
            return !(a == b);
        }

        public static bool operator<= (Category a, Category b)
        {
            return a < b || a == b;
        }

        public static bool operator>= (Category a, Category b)
        {
            return a > b || a == b;
        }
    }
}
