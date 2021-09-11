using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tickets
{
    public static class TicketsTask
    {
        public static BigInteger Solve(int n, int k)
        {
            var opt = new BigInteger[k+1, n];
            for (int i = 0; i < 10 && i < k+1; i++)
                opt[i, 0] = 1;
            for (int j = 0; j < n; j++)
                opt[0, j] = 1;
            for (int i = 1; i < k + 1; i++)
                for (int j = 1; j < n; j++)
                {
                    BigInteger sum = 0;
                    for (int s = i; s >= 0 && s > i - 10; s--)
                    {
                        sum += opt[s, j - 1];
                    }
                    opt[i, j] = sum;
                }
            if (k % 2 == 0)
                return opt[k/2, n-1] * opt[k/2, n-1];
            return 0;
        }
    }
}
