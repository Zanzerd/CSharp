using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskTree
{
	[TestFixture]
    public class DiskTreeTask_should
    {

        public void MakeTest(List<string> input, List<string> answer)
        {
            var result = DiskTreeTask.Solve(input);
            CollectionAssert.AreEqual(answer, result);
        }

        [Test]
        public void Test1() { MakeTest(new List<string> { @"WINNT\SYSTEM32\CONFIG", @"GAMES", @"WINNT\DRIVERS", @"HOME", @"WIN\SOFT", @"GAMES\DRIVERS", @"WINNT\SYSTEM32\CERTSRV\CERTCO~1\X86", }, new List<string> { @"GAMES", @" DRIVERS", @"HOME", @"WIN", @" SOFT", @"WINNT", @" DRIVERS", @" SYSTEM32", @"  CERTSRV", @"   CERTCO~1", @"    X86", @"  CONFIG", }); }

        [Test]
        public void Test2() { MakeTest(new List<string> { @"USERS", }, new List<string> { @"USERS", }); }

        [Test]
        public void Test3() { MakeTest(new List<string> { @"A", @"B", @"C", @"D", @"E", @"F", @"G", }, new List<string> { @"A", @"B", @"C", @"D", @"E", @"F", @"G", }); }

        [Test]
        public void Test4() { MakeTest(new List<string> { @"!#$%&'()\-@^_`{}~\!#$%&'()\-@^_`{}~\!#$%&'()\-@^_`{}~\!#$%&'()\-@^_`{}~\!#$%&'()", }, new List<string> { @"!#$%&'()", @" -@^_`{}~", @"  !#$%&'()", @"   -@^_`{}~", @"    !#$%&'()", @"     -@^_`{}~", @"      !#$%&'()", @"       -@^_`{}~", @"        !#$%&'()", }); }

        [Test]
        public void Test5() { MakeTest(new List<string> { @"AAAA", @"AAAA\AAAA", @"AAA\AA\AAA", @"A\AAA\AAA", @"AA\A\AAAA", @"AAA\A\AAAA", @"AA\AAA", @"A\A\A\A", @"AA\AA\AA\AA", @"AAA\AAA\AAA\AAA", @"AAAA\AAAA\AAAA\AAAA", @"AA\AA\AA", @"A\AA\A\AA", }, new List<string> { @"A", @" A", @"  A", @"   A", @" AA", @"  A", @"   AA", @" AAA", @"  AAA", @"AA", @" A", @"  AAAA", @" AA", @"  AA", @"   AA", @" AAA", @"AAA", @" A", @"  AAAA", @" AA", @"  AAA", @" AAA", @"  AAA", @"   AAA", @"AAAA", @" AAAA", @"  AAAA", @"   AAAA", }); }
        [Test]
        public void Test6() { MakeTest(new List<string> { @"A\A", @"B\A" }, new List<string> { "A", " A", "B", " A" }); }

        [Test]
        public void Test7() { MakeTest(new List<string> { @"a", @"a\b\c\d\e\f", @"a\b\r\t\p" }, new List<string> { "a", " b", "  c", "   d", "    e", "     f", "  r", "   t", "    p" }); }

        [Test]
        public void Test8() { MakeTest(new List<string> { @"a\b", @"b\a", @"a\c\d", @"a\c\e" }, new List<string> { "a", " b", " c", "  d", "  e", "b", " a" }); }

        [Test]
        public void Test9() { MakeTest(new List<string> { @"ab\c\d\e" }, new List<string> { "ab", " c", "  d", "   e" }); }

        [Test]
        public void Test10() { MakeTest(new List<string> { @"a\baba", @"a\bab", @"a\babab", @"a\bab\ba" }, new List<string> { "a", " bab", "  ba", " baba", " babab"}); }

        [Test]
        public void Test11() { MakeTest(new List<string> { @"a", @"a\b\c\d\e", @"b\f\e\t", @"a\b\p" }, new List<string> { "a", " b", "  c", "   d", "    e", "  p", "b", " f", "  e", "   t" }); }

    }
}

    //public void TestN() { MakeTest(new List<string> { }, new List<string> { }); }

