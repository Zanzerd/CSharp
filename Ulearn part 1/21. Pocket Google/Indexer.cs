using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        static HashSet<char> delimiters = new HashSet<char>(new[] {' ', '.', ',', '!', '?', ':', '-', '\r', '\n' });
        private Dictionary<string, Dictionary<int, List<int>>> wordToId = 
            new Dictionary<string, Dictionary<int, List<int>>>();

        public void Add(int id, string documentText)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < documentText.Length; i++)
            {
                if (delimiters.Contains(documentText[i]) || i == documentText.Length - 1)
                {
                    if (i == documentText.Length - 1)
                        builder.Append(documentText[i]);
                    int index = i - builder.Length; // мб не такое значение индекса
                    var word = builder.ToString();
                    if (!wordToId.ContainsKey(word))
                    {
                        var idToIndex = new Dictionary<int, List<int>>();
                        var listOfIndexes = new List<int>();
                        idToIndex[id] = listOfIndexes;
                        wordToId[word] = idToIndex;
                    }
                    if (!wordToId[word].ContainsKey(id))
                    {
                        var listOfIndexes = new List<int>();
                        wordToId[word][id] = listOfIndexes;
                    }
                    wordToId[word][id].Add(index);
                    builder.Clear();
                }
                else
                    builder.Append(documentText[i]);
            }
        }

        public List<int> GetIds(string word)
        {
            var result = new List<int>();
            if (wordToId.ContainsKey(word))
            {
                foreach (var id in wordToId[word].Keys)
                    result.Add(id);
            }
            return result;
        }

        public List<int> GetPositions(int id, string word)
        {
            var result = new List<int>();
            if (wordToId.ContainsKey(word))
            {
                if (wordToId[word].ContainsKey(id))
                {
                    result = wordToId[word][id];
                }
            }
            return result;
        }

        public void Remove(int id)
        {
            foreach (var word in wordToId.Keys)
            {
                if (wordToId[word].ContainsKey(id))
                    wordToId[word].Remove(id);
            }

        }
    }
}
