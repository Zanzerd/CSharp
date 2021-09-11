using System;
using System.Configuration;
using System.Collections.Generic;

// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;
using System.Linq;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var dictDocCompare = new Dictionary<(DocumentTokens, DocumentTokens), ComparisonResult>();
            foreach(var first in documents)
            { 
                foreach(var second in documents)
                {
                    if (first != second && !dictDocCompare.ContainsKey((first, second)) && 
                        !dictDocCompare.ContainsKey((second, first)))
                    {
                        var opt = new double[first.Count + 1, second.Count + 1];
                        for (var i = 0; i <= first.Count; ++i)
                            opt[i, 0] = i;
                        for (var j = 0; j <= second.Count; ++j)
                            opt[0, j] = j;
                        for (var i = 1; i <= first.Count; ++i)
                            for (var j = 1; j <= second.Count; ++j)
                            {
                                var tokenDistance =
                                    TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]);
                                if (tokenDistance == 0)
                                    opt[i, j] = opt[i - 1, j - 1];
                                else
                                {
                                    opt[i, j] = Math.Min(tokenDistance + opt[i - 1, j - 1],
                                        Math.Min(1 + opt[i - 1, j], 1 + opt[i, j - 1]));
                                }
                            }
                        dictDocCompare[(first, second)] =
                            new ComparisonResult(first, second, opt[first.Count, second.Count]);
                    }
                }
            }
            return dictDocCompare.Values.ToList();
        }
    }
}
