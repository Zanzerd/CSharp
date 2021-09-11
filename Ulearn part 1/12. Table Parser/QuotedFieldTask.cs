using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("'a\"b'", 0, "a\"b", 5)]
        [TestCase("\"\"", 0, "", 2)]
        [TestCase("\"a\"", 0, "a", 3)]
        [TestCase("'a\"'", 0, "a\"", 4)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }

        // Добавьте свои тесты
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            if ((line[0] == '\'' || line[0] == '\"') && line.Length == 1)
                return new Token("", startIndex, 1);
            StringBuilder builder = new StringBuilder();
            string nline = line.Remove(0, startIndex);
            string newline = nline.Remove(0, 1);
            int counter = 0;
            if (nline[0] == '\'' || (nline[0] == '\\' && nline[1] == '"'))
            {
                (builder, counter) = charsInLineCycle(newline, '\'');
            }
            else if (nline[0] == '\\' && (nline[1] == '"') || nline[0] == '\"')
            {
                (builder, counter) = charsInLineCycle(newline, '"');
            }

            string result = builder.ToString();
            int indexOfStop = newline.IndexOf(line[0], 0, newline.Length);
            if (indexOfStop == -1)
                return new Token(result, startIndex, counter + 1);
            else
                return new Token(result, startIndex, counter + 2);
        }
        static (StringBuilder, int) charsInLineCycle(string line, char stop)
        {
            var builder = new StringBuilder();
            int slashCount = 0;
            int symbolCount = 0;
            foreach (char c in line)
            {
                if (c == stop && slashCount == 0)
                    break;
                else if (c == stop && slashCount > 0)
                {
                    builder.Append(c);
                    slashCount = 0;
                }
                else if (c == '\\')
                    if (slashCount == 0)
                    {
                        slashCount++;
                    }
                    else
                    {
                        builder.Append(c);
                        slashCount = 0;
                    }
                else
                {
                    builder.Append(c);
                    slashCount = 0;
                }
                symbolCount++;
            }
            return (builder, symbolCount);
        }
    }
}

