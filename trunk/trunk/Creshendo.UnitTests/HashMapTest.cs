using System;
using System.Collections.Generic;
using Creshendo.UnitTests.Support;
using Creshendo.Util.Collections;
using NUnit.Framework;

namespace Creshendo.UnitTests
{
    [TestFixture]
    public class HashMapTest
    {
        [Test]
        public void HashPerformance()
        {
            //int[] series = new int[] {1000, 10000, 100000, 1000000, 10000000};
            int[] series = new int[] {1000, 10000};
            HashMapBenchmark util = new HashMapBenchmark();
            Random ran = new Random();
            for (int idx = 0; idx < series.Length; idx++)
            {
                GenericHashMap<object, object> map = util.createHashMap(series[idx]);
                long start = DateTime.Now.Ticks;
                for (int idz = 0; idz < 10000000; idz++)
                {
                    String key = ran.Next(series[idx]).ToString();
                    Object val = map.Get(key);
                }
                long end = DateTime.Now.Ticks;
                long el = end - start;
                double perOp = (double) el/10000000;
                String fstring = (perOp*1000000).ToString();
                Console.WriteLine("Test for " + series[idx] + " items");
                Console.WriteLine("elapsed time " + el + " millisecond");
                Console.WriteLine("per lookup " + fstring + " nanosecond");
            }
        }

        [Test]
        public void GenericHashPerformance()
        {
            //int[] series = new int[] {1000, 10000, 100000, 1000000, 10000000};
            int[] series = new int[] { 1000, 10000 };
            HashMapBenchmark util = new HashMapBenchmark();
            Random ran = new Random();
            for (int idx = 0; idx < series.Length; idx++)
            {
                GenericHashMap<string, string> map = util.createGenericHashMap(series[idx]);
                long start = DateTime.Now.Ticks;
                for (int idz = 0; idz < 10000000; idz++)
                {
                    String key = ran.Next(series[idx]).ToString();
                    Object val = map.Get(key);
                }

                foreach (IHashMapEntry<string, string> entry in map)
                {
                    string val = entry.Value;
                }

                List<String> keys = new List<String>(map.Count);

                foreach (String key in map.Keys)
                {
                    keys.Add(key);
                    string val = map[key];
                }

                foreach (String val in map.Values)
                {
                    bool yes = map.ContainsValue(val);
                }


                foreach (String key in keys)
                {
                   map.Remove(key);
                }

                long end = DateTime.Now.Ticks;
                long el = end - start;
                double perOp = (double)el / 10000000;
                String fstring = (perOp * 1000000).ToString();
                Console.WriteLine("Test for " + series[idx] + " items");
                Console.WriteLine("elapsed time " + el + " millisecond");
                Console.WriteLine("per lookup " + fstring + " nanosecond");
            }
        }
    }
}