using System.Collections.Generic;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase("text", new[] { "text" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("''", new[] { "" })]
        [TestCase("'a'", new[] { "a" })]
        [TestCase("'a\"b'", new[] { "a\"b" })]
        [TestCase("\"\"", new[] { "" })]
        [TestCase("'\"\"'", new[] { "\"\"" })]
        [TestCase("\"''\"", new[] { "''" })]
        [TestCase("\"'a', 'b'\" 'c' 'd'", new[] { "'a', 'b'", "c", "d" })]
        [TestCase("\"\\\\\"", new[] { "\\" })]
        [TestCase("\"\" \"dcf\"", new[] { "", "dcf" })]
        [TestCase("a\"f\"", new[] { "a", "f" })]
        [TestCase(" d ", new[] { "d" })]
        [TestCase("\\ \\", new[] { "\\", "\\" })]
        [TestCase("\t", new[] { "\t" })]
        [TestCase("'d", new[] { "d" })]
        [TestCase(@"'\'\''", new[] { @"''" })]
        [TestCase(@"\\servername\share", new[] { "\\\\servername\\share" })]
        [TestCase("'\"\"'", new[] { "\"\"" })]
        [TestCase("' ", new[] { " " })]
        [TestCase(" '", new[] { "" })]
        [TestCase("\"\\\"", new[] { "\"" })]
        [TestCase("a   b", new[] { "a", "b" })]
        [TestCase("'a' b", new[] { "a", "b" })]
        [TestCase("", new string[0])]

        // Вставляйте сюда свои тесты
        public static void RunTests(string input, string[] expectedOutput)
        {
            // Тело метода изменять не нужно
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        // При решении этой задаче постарайтесь избежать создания методов, длиннее 10 строк.
        // Подумайте как можно использовать ReadQuotedField и Token в этой задаче.
        public static List<Token> ParseLine(string line)
        {
            if (line == string.Empty)
                return new List<Token>() { };
            if (line == "")
                return new List<Token>() { new Token(string.Empty, 0, 0) };
            int i = 0;
            while (i < line.Length && line[i] == ' ')
            {
                i++;
            }
            var result = new List<Token>();
            var token = ReadField(line, i);
            result.Add(token);
            if (line == token.Value)
                return result;
            while (i < line.Length-1)
            {
                i = token.GetIndexNextToToken();
                for (int j = i; j < line.Length; j++)
                {
                    if (line[j] == ' ')
                        i++;
                    else
                        break;
                }
                token = ReadField(line, i);
                result.Add(token);
            }
            if (result.Count > 1 && result[result.Count-1].Value == "" && result[result.Count - 2].Length != 0)
                result.RemoveAt(result.Count-1);
            return result;
        }

        private static Token ReadField(string line, int startIndex)
        {
              string nline = line.Remove(0, startIndex);
            if (nline.Length == 1 && nline[0] == ' ')
                return new Token("", startIndex, 0);
            var builder = new StringBuilder();
            int len = 0;
            Token result;
            for (int i = 0; i < nline.Length; i++)
            {
                if ((nline[i] == '\'' || nline[i] == '"') && i == 0)
                {
                    result = ReadQuotedField(line, startIndex + i);
                    return result;
                }
                else if ((nline[i] == ' ' || nline[i] == '\'' || nline[i] == '"')&& i != 0)
                    break;
                else if (nline[i] == ' ' && i == 0)
                    continue;
                else
                    builder.Append(nline[i]);
                len++;
            }
            return new Token(builder.ToString(), startIndex, len);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}