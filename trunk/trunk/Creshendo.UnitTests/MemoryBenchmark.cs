/*
 * Copyright 2002-2006 Peter Lin
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://ruleml-dev.sourceforge.net/
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 */
using System;
using System.Collections;
using System.IO;
using Creshendo.Util.Parser.Clips2;
using Creshendo.Util.Rete;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Creshendo.UnitTests
{
    //[TestClass]
    public class MemoryBenchmark : FileTestBase
    {
        //[TestMethod]
        public void mainTest()
        {
            String rulefile = getRoot("random_5_w_50Kdata.clp");
            //String datafile = getRoot("test.clp");
            // in case it's run within OptimizeIt and we want to keep the test running
            //bool keepopen = false;

            Console.WriteLine("Using file " + rulefile);

            MemoryBenchmark mb = new MemoryBenchmark();
            ArrayList facts = new ArrayList(50000);
            // Runtime rt = Runtime.getRuntime();


            long total1 = GC.GetTotalMemory(true);
            //long free1 = rt.freeMemory();
            //long used1 = total1 - free1;
            //int count = 100000;
            Console.WriteLine("Used memory before creating engine " + total1 + " bytes " +
                              (total1/1024) + " Kb");
            Rete engine = new Rete();

            long total2 = GC.GetTotalMemory(true);
            //long free2 = rt.freeMemory();
            //long used2 = total2 - free2;
            Console.WriteLine("Used memory after creating engine " + total2 + " bytes " +
                              (total2/1024) + " Kb");

            try
            {
                StreamReader freader = new StreamReader(rulefile);
                CLIPSParser parser = new CLIPSParser(engine, freader);
                long start = DateTime.Now.Ticks;
                mb.parse(engine, parser, facts);
                long end = DateTime.Now.Ticks;
                // parser.close();
                // rt.gc();

                long total3 = GC.GetTotalMemory(true);
                //long free3 = rt.freeMemory();
                //long used3 = total3 - free3;
                Console.WriteLine("Used memory after loading rules, data and asserting facts " +
                                  total3 + " bytes " + (total3/1024) + " Kb " + (total3/1024/1024) + " Mb");
                Console.WriteLine("elapsed time to parse and assert the data " +
                                  (end - start) + " ms");
                engine.printWorkingMemory(true, false);

                engine.close();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}