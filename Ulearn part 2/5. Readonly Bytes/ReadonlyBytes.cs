using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace hashes
{
	public class ReadonlyBytes : IEnumerable
    {
        public byte[] Value { get; }
        public int Length { get { return Value.Length; } }
        private bool isEmpty { get; set; } = false;
        public byte this[int index]
        {
            get { return Value[index]; }
        }
        private int hash = 16777619;
        public ReadonlyBytes(params byte[] val)
        {
            if (val is null)
                throw new ArgumentNullException();
            //int[] intval = val.Cast<int>().ToArray();
            Value = new byte[val.Length];
            for (int i = 0; i < val.Length; i++)
            {
                Value[i] = val[i];
            }
            CountHash();
        }


    public ReadonlyBytes()
        {
            Value = new byte[1];
            isEmpty = true;
            CountHash();
        }

        public IEnumerator<byte> GetEnumerator()
        {
            for(int i = 0; i < Value.Length; i++)
            {
                yield return Value[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ReadonlyBytes) || obj.GetType().IsSubclassOf(typeof(ReadonlyBytes))) 
                return false;
            ReadonlyBytes RB_check = obj as ReadonlyBytes;
            if (this.Length != RB_check.Length)
                return false;
            for(int i = 0; i < this.Length; i++)
            {
                if (this.Value[i] != RB_check.Value[i])
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            if (isEmpty)
                return "[]";
            else
            {
                string[] helpArr = new string[Length+2];
                helpArr[0] = "[";
                helpArr[Length + 1] = "]";
                for (int i = 0; i < Length; i++)
                {
                    if (i < Length - 1)
                        helpArr[i + 1] = String.Format("{0}, ", Value[i]);
                    else
                        helpArr[i + 1] = String.Format("{0}", Value[i]);
                }
                return String.Join("", helpArr);
            }
        }
        
        private void CountHash()
        {
            unchecked
            {
                foreach (var v in Value)
                {
                    hash *= 2171;
                    hash ^= v.GetHashCode();
                }
            }
        }
        public override int GetHashCode()
        {
            return hash;
        }
    }
}