using System.Collections.Generic;

namespace StructBenchmarking
{
    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            foreach (var size in Constants.FieldCounts)
            {
                var ArrayCreationStruct = new StructArrayCreationTask(size);
                var ArrayCreationClass = new StructArrayCreationTask(size);
                double classMeanDuration = benchmark.MeasureDurationInMs(ArrayCreationClass, 10000);
                double structMeanDuration = benchmark.MeasureDurationInMs(ArrayCreationStruct, 10000);
                classesTimes.Add(new ExperimentResult(size, classMeanDuration));
                structuresTimes.Add(new ExperimentResult(size, structMeanDuration));
            }

            return new ChartData
            {
                Title = "Create array",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();
            
            foreach (var size in Constants.FieldCounts)
            {
                var methodCallStruct = new MethodCallWithStructArgumentTask(size);
                var methodCallClass = new MethodCallWithClassArgumentTask(size);
                double structMeanDuration = benchmark.MeasureDurationInMs(methodCallStruct, 10000);
                double classMeanDuration = benchmark.MeasureDurationInMs(methodCallClass, 10000);
                structuresTimes.Add(new ExperimentResult(size, structMeanDuration));
                classesTimes.Add(new ExperimentResult(size, classMeanDuration));
            }

            return new ChartData
            {
                Title = "Call method with argument",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }
    }
}