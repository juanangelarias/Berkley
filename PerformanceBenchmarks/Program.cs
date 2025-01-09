// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

Console.WriteLine("Running Benchmarks...");
BenchmarkRunner.Run<Benchmarks>();

public class BenchMarkConfig : ManualConfig { }

[MemoryDiagnoser]
public class Benchmarks
{
    private static int itemnId = 0;

    private static string GetItemIdString()
    {
        var id = Interlocked.Increment(ref itemnId);
        return id.ToString("D5");
    }

    private static int iterations = 1000;
    [Benchmark]
    public ConcurrentDictionary<string, object> PlusOneParam()
    {
        var cd = new ConcurrentDictionary<string, object>();
        for (var i = 0; i < iterations; i++)
        {
            //Key creation method being tested
            var key = "ThisIsATest" + GetItemIdString();
            cd[key] = i;
        }
        return cd;
    }
    //[Benchmark]
    public ConcurrentDictionary<string, object> ShortNamePlusOneParam()
    {
        var cd = new ConcurrentDictionary<string, object>();
        for (var i = 0; i < iterations; i++)
        {
            //Key creation method being tested
            var key = "T" + GetItemIdString();
            cd[key] = i;
        }
        return cd;
    }
    //[Benchmark]
    public ConcurrentDictionary<string, object> LongNamePlusOneParam()
    {
        var cd = new ConcurrentDictionary<string, object>();
        for (var i = 0; i < iterations; i++)
        {
            //Key creation method being tested
            var key = "ThisIsATestThisIsATestThisIsATestThisIsATest" + GetItemIdString();
            cd[key] = i;
        }
        return cd;
    }
    [Benchmark]
    public ConcurrentDictionary<string, object> StringInterpolationOneParam()
    {
        var cd = new ConcurrentDictionary<string, object>();
        for (var i = 0; i < iterations; i++)
        {
            //Key creation method being tested
            var key = $"ThisIsATest{GetItemIdString()}";
            cd[key] = i;
        }
        return cd;
    }
    [Benchmark]
    public ConcurrentDictionary<string, object> StringFormatOneParam()
    {
        var cd = new ConcurrentDictionary<string, object>();
        for (var i = 0; i < iterations; i++)
        {
            //Key creation method being tested
            var key = string.Format("ThisIsATest{0}", GetItemIdString());
            cd[key] = i;
        }
        return cd;
    }
    [Benchmark]
    public ConcurrentDictionary<string, object> PlusTwoParam()
    {
        var cd = new ConcurrentDictionary<string, object>();
        for (var i = 0; i < iterations; i++)
        {
            //Key creation method being tested
            var param2 = "Flippity";
            var key = "ThisIsATest" + GetItemIdString() + param2;
            cd[key] = i;
        }
        return cd;
    }
    [Benchmark]
    public ConcurrentDictionary<string, object> StringInterpolationTwoParam()
    {
        var cd = new ConcurrentDictionary<string, object>();
        for (var i = 0; i < iterations; i++)
        {
            //Key creation method being tested
            var param2 = "Flippity";
            var key = $"ThisIsATest{GetItemIdString()}{param2}";
            cd[key] = i;
        }
        return cd;
    }
    [Benchmark]
    public ConcurrentDictionary<string, object> StringFormatTwoParam()
    {
        var cd = new ConcurrentDictionary<string, object>();
        for (var i = 0; i < iterations; i++)
        {
            //Key creation method being tested
            var param2 = "Flippity";
            var key = string.Format("ThisIsATest{0}{1}", GetItemIdString(), param2);
            cd[key] = i;
        }
        return cd;
    }
    [Benchmark]
    public ConcurrentDictionary<string, object> PlusSixParam()
    {
        var cd = new ConcurrentDictionary<string, object>();
        for (var i = 0; i < iterations; i++)
        {
            //Key creation method being tested
            var param2 = "Flippity";
            var param3 = 12384;
            var param4 = 123.463;
            var param5 = "Floppity";
            var param6 = "Bunny";
            var key = "ThisIsATest" + GetItemIdString() + param2 + param3+ param4 + param5 +param6;
            cd[key] = i;
        }
        return cd;
    }
    [Benchmark]
    public ConcurrentDictionary<string, object> StringInterpolationSixParam()
    {
        var cd = new ConcurrentDictionary<string, object>();
        for (var i = 0; i < iterations; i++)
        {
            //Key creation method being tested
            var param2 = "Flippity";
            var param3 = 12384;
            var param4 = 123.463;
            var param5 = "Floppity";
            var param6 = "Bunny";
            var key = $"ThisIsATest{GetItemIdString()}{param2}{param3}{param4}{param5}{param6}";
            cd[key] = i;
        }
        return cd;
    }
    [Benchmark]
    public ConcurrentDictionary<string, object> StringFormatSixParam()
    {
        var cd = new ConcurrentDictionary<string, object>();
        for (var i = 0; i < iterations; i++)
        {
            //Key creation method being tested
            var param2 = "Flippity";
            var param3 = 12384;
            var param4 = 123.463;
            var param5 = "Floppity";
            var param6 = "Bunny";
            var key = string.Format("ThisIsATest{0}{1}{2}{3}{4}{5}", GetItemIdString(), param2, param3, param4, param5, param6);
            cd[key] = i;
        }
        return cd;
    }

}