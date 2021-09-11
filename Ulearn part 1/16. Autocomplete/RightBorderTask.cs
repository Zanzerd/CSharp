using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocomplete
{
    public class RightBorderTask
    {
        /// <returns>
        /// Возвращает индекс правой границы. 
        /// То есть индекс минимального элемента, который не начинается с prefix и большего prefix.
        /// Если такого нет, то возвращает items.Length
        /// </returns>
        /// <remarks>
        /// Функция должна быть НЕ рекурсивной
        /// и работать за O(log(items.Length)*L), где L — ограничение сверху на длину фразы
        /// </remarks>
        public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            // Я задался целью написать самый уродливый код из возможных
            left++;
            right--;
            if (phrases.Count == 0)
                return 0;
            while (left != right)
            {
                int m = (left + right) / 2;
                if (string.Compare(prefix, phrases[m], StringComparison.OrdinalIgnoreCase) > 0)
                    left = m + 1;
                else
                    right = m;
            }
            while (right < phrases.Count - 1 && phrases[right].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                right++;
            if (right == phrases.Count - 1 && phrases[right].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases.Count;
            else if (right == phrases.Count - 1 && !phrases[right].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                if (string.Compare(prefix, phrases[right], StringComparison.OrdinalIgnoreCase) > 0)
                    return phrases.Count;
                return phrases.Count - 1;
            }
            if (prefix == "")
                return phrases.Count;
            if (!phrases[right + 1].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return right;
            else
                return phrases.Count;
        }
    }
}