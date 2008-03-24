using System;
using System.IO;
using Creshendo.UnitTests;
using Creshendo.UnitTests.Support;
using Creshendo.Util.Rete;

namespace Creshendo.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Rete engine = new Rete();
            //Shell shell = new Shell(engine);

            //shell.Run();

            //shell.run();

            //TestForObjectBindings test2 = new TestForObjectBindings();

            //test2.BasicObjectBindingTest1();
            //test2.ObjectBindingTest1();
            //test2.ObjectBindingTest2();

            //AssertWRules test1 = new AssertWRules();
            //test1.MainTest();

            //HashMapTest hashMapBenchmark = new HashMapTest();
            //hashMapBenchmark.GenericHashPerformance();
            //Benchmarks test = new Benchmarks();


            //test.manners64();

            //test.manners16();

            //test.manners128();
            System.Console.WriteLine("Working...");
            System.Console.WriteLine();
            double totTime = 0;
#if DEBUG
            int loopCnt = 1;
#else
            int loopCnt = 10;
#endif
            for (int i = 0; i < loopCnt; i++)
            {
                double thisTime = manners64();
                System.Console.WriteLine("Completed iteration " + i + " in " + (thisTime / 10000000).ToString("0.000000") + " seconds.");
                totTime += thisTime;
            }
            double endTime = totTime / loopCnt;
            System.Console.WriteLine();
            System.Console.WriteLine(String.Format("Manners 64 completed {0} iterations in an average of {1} seconds.", loopCnt, (endTime / 10000000).ToString("0.000000")));

        }

        public static double manners64()
        {
            long ts = DateTime.Now.Ticks;
            long totTime = 0;
            using (TextWriter writer = System.Console.Out)
            {
                Rete engine = new Rete();
                //engine.CurrentFocus.Lazy = true;
                engine.addPrintWriter("Console", writer);
                engine.loadRuleset(getRoot("manners64guests.clp"));
                engine.printWorkingMemory(false, false);
                totTime = DateTime.Now.Ticks - ts;
                writer.Flush();
                writer.Close();
                engine.close();
            }
            return totTime;
        }

        protected static string getRoot(string fileName)
        {
            FileInfo file = new FileInfo(Path.Combine(@"D:\Src\Creshendo\trunk\Creshendo.UnitTests\Data", fileName));

            if (file.Exists)
                return file.FullName;

            throw new FileNotFoundException("File not found", file.FullName);
        }
    }
}