using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            string[] sentences = text.Split(new char[] { '.', '!', '?', ':', ';', '(', ')','[',']' });
            for(int i = 0; i < sentences.Length ; i++)
            {
                int symbolCounter = 0;
                sentences[i] = sentences[i].ToLower();
                sentences[i] = sentences[i].Trim();
                List<string> wordsList = new List<string>();
                var builder = new StringBuilder();
                bool elseBranch = false;
                foreach (char c in sentences[i])
                {
                    elseBranch = false;
                    if (char.IsLetter(c) || c == '\'')
                    {
                        symbolCounter++;
                        builder.Append(c);
                    }
                    else 
                    {
                        elseBranch = true;
                        if (builder.Length > 0 && !string.IsNullOrEmpty(builder.ToString()))
                        {
                            wordsList.Add(builder.ToString());
                            builder.Clear();
                        }
                    }
                }
                if (elseBranch == false)
                {
                    wordsList.Add(builder.ToString());
                    builder.Clear();
                }
                int notEmptyCounter = 0;
                foreach (var w in wordsList)
                    if (!(string.IsNullOrEmpty(w)))
                        notEmptyCounter++;
                if (symbolCounter > 0 && notEmptyCounter > 0) 
                {
                    sentencesList.Add(wordsList);
                }
            }
            return sentencesList;
        }
    }
}