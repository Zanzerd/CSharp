using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
	{
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            GC.Collect();                   // Эти две строчки нужны, чтобы уменьшить вероятность того,
            GC.WaitForPendingFinalizers();  // что Garbadge Collector вызовется в середине измерений
                                            // и как-то повлияет на них.
            task.Run();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < repetitionCount; i++)
            {
                task.Run();
            }
            watch.Stop();
            double duration = (double)watch.ElapsedMilliseconds / repetitionCount;
            return duration;
		}
	}

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            Benchmark benchStringBuilder = new Benchmark();
            Benchmark benchStringConstructor = new Benchmark();
            double stringBuilderTime = benchStringBuilder.MeasureDurationInMs(new StringBuilderTask(), 100);
            double stringConstructorTime = benchStringConstructor.MeasureDurationInMs(new StringConstructorTask(),
                100);
            Assert.Less(stringConstructorTime, stringBuilderTime);
        }
    }

    public class StringBuilderTask : ITask
    {
        public void Run()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < 10000; i++)
            {
                builder.Append('a');
            }
            string str = builder.ToString();
        }
    }

    public class StringConstructorTask : ITask
    {
        public void Run()
        {
            string str = new string('a', 10000);
        }
    }
}