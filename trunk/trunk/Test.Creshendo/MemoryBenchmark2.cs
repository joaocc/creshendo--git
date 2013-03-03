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
using Creshendo.Util.Rete.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Creshendo
{
    //[TestClass]
    public class MemoryBenchmark2 : FileTestBase
    {
        public static void main(String[] args)
        {
            String rulefile = "./benchmark_files/4K_rules_50Kdata_sequential.clp";
            //String datafile = "./benchmark_files/test.clp";
            //bool keepopen = false;
            if (args != null && args.Length > 0)
            {
                rulefile = args[0];
            }
            //if (args.Length >= 2)
            //{
            //    if (args[1].Equals("true", StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        keepopen = true;
            //    }
            //}
            Console.WriteLine("Using file " + rulefile);

            MemoryBenchmark2 mb = new MemoryBenchmark2();
            long begin = DateTime.Now.Ticks;
            long totalET = 0;
            long parseET = 0;
            ArrayList facts = new ArrayList(50000);
            // Runtime rt = Runtime.getRuntime();
            long total1 = GC.GetTotalMemory(true);
            //long free1 = rt.freeMemory();
            //long used1 = total1 - free1;
            int loopcount = 5;
            Console.WriteLine("Used memory before creating engine " + total1 + " bytes " +
                              (total1/1024) + " Kb");
            for (int loop = 0; loop < loopcount; loop++)
            {
                Console.WriteLine(" ---------------------------------- ");
                Rete engine = new Rete();
                facts.Clear();

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
                    long el = end - start;
                    parser.close();
                    //rt.gc();
                    parseET += el;

                    long total3 = GC.GetTotalMemory(true);
                    //long free3 = rt.freeMemory();
                    //long used3 = total3 - free3;
                    Console.WriteLine("Used memory after loading rules, and parsing data " +
                                      (total3/1024) + " Kb " + (total3/1024/1024) + " Mb");
                    Console.WriteLine("elapsed time to parse the rules and data " +
                                      el + " ms");

                    Console.WriteLine("Number of rules: " +
                                      engine.CurrentFocus.RuleCount);
                    // now assert the facts
                    IEnumerator itr = facts.GetEnumerator();
                    long start2 = DateTime.Now.Ticks;
                    while (itr.MoveNext())
                    {
                        Deffact fct = (Deffact) itr.Current;
                        engine.assertFact(fct);
                    }
                    int actCount = engine.ActivationList.size();
                    long end2 = DateTime.Now.Ticks;
                    long et2 = end2 - start2;
                    totalET += et2;
                    // now fire the rules
                    long start3 = 0;
                    long end3 = 0;
                    int fired = 0;
                    try
                    {
                        start3 = DateTime.Now.Ticks;
                        fired = engine.fire();
                        end3 = DateTime.Now.Ticks;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    facts.Clear();

                    long total4 = GC.GetTotalMemory(true);
                    //long free4 = rt.freeMemory();
                    //long used4 = total4 - free4;

                    Console.WriteLine("");
                    Console.WriteLine("Number of facts - " + engine.DeffactCount);
                    Console.WriteLine("Time to assert facts " + et2 + " ms");
                    Console.WriteLine("Used memory after assert " +
                                      (total4/1024) + " Kb " + (total4/1024/1024) + " Mb");
                    engine.printWorkingMemory(true, false);
                    Console.WriteLine("number of activations " + actCount);
                    Console.WriteLine("rules fired " + fired);
                    Console.WriteLine("time to fire rules " + (end3 - start3) + " ms");
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (AssertException e)
                {
                    Console.WriteLine(e.Message);
                }
                engine.close();
                engine = null;
                //rt.gc();engine.close();
            }
            long finished = DateTime.Now.Ticks;
            Console.WriteLine("average parse ET - " + parseET/loopcount + " ms");
            Console.WriteLine("average assert ET - " + totalET/loopcount + " ms");
            Console.WriteLine("total run time " + (finished - begin) + " ms");
        }
    }
}