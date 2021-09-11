using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Weights
{
    public class Indexer
    {
        private double[] data;
        private readonly int start;
        private readonly int length;

        public int Length { get { return length; } }

        public double this[int index]
        {
            get
            {
                if (index < 0 || index >= length)
                    throw new IndexOutOfRangeException();
                return data[start + index];
            }

            set
            {
                if (index < 0 || index >= length)
                    throw new IndexOutOfRangeException();
                data[start + index] = value;
            }
        }

        public Indexer(double[] data, int start, int length)
        {
            if (data == null)
                throw new ArgumentNullException();
            if (start < 0 || length < 0 || start + length > data.Length)
                throw new ArgumentException();
            this.data = data;
            this.start = start;
            this.length = length;
        }
    }
}
