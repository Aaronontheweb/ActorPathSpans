using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Akka.Actor;
using BenchmarkDotNet.Attributes;

namespace ActorPathSpans.Benchmarks
{
    [Config(typeof(MicroBenchmarkConfig))]
    public class StringWithSpanBenchmark
    {
        public static readonly string SplitString =
            "this is a big string that will need to be split into several smaller objects";

        public static readonly char SplitSeparator = ' ';

        [Benchmark]
        public string[] StringSplitDefault()
        {
            return SplitString.Split(SplitSeparator);
        }

        [Benchmark]
        public ReadOnlyMemory<char>[] SplitWithSpan()
        {
            return SplitString.SplitWithSpan(SplitSeparator); // call ToArray to force lazy evaluation
        }
    }
}
