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
using Test.Creshendo.Model;
using Creshendo.Util.Parser.Clips2;
using Creshendo.Util.Rete;
using Creshendo.Util.Rete.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Creshendo
{
    [TestClass]
    public class AssertWRules : FileTestBase
    {
        [TestMethod]
        public void MainTest()
        {
            String rulefile = getRoot("account_5.clp");

            ArrayList objects = new ArrayList();
            //AssertWRules awr = new AssertWRules();

            // Runtime rt = Runtime.getRuntime();
            long total1 = GC.GetTotalMemory(true);
            //long free1 = rt.freeMemory();
            //long used1 = total1 - free1;
            int count = 5000;
            Console.WriteLine("loading file " + rulefile);
            Console.WriteLine("Used memory before creating objects " + total1 + " bytes " +
                              (total1/1024) + " Kb");
            for (int idx = 0; idx < count; idx++)
            {
                Account acc = new Account();
                acc.AccountId = "acc" + idx;
                // acc.setAccountId("acc" + ran.Next(4));
                acc.AccountType = "standard";
                acc.First = Convert.ToString(ran.Next(100000));
                acc.Last = Convert.ToString(ran.Next(100000));
                acc.Middle = Convert.ToString(ran.Next(100000));
                acc.OfficeCode = Convert.ToString(ran.Next(100000));
                acc.RegionCode = Convert.ToString(ran.Next(100000));
                acc.Status = "active";
                acc.Title = "mr";
                acc.Username = Convert.ToString(ran.Next(100000));
                acc.AreaCode = Convert.ToString(ran.Next(999));
                acc.Exchange = Convert.ToString(ran.Next(999));
                acc.Number = Convert.ToString(ran.Next(999));
                acc.Ext = Convert.ToString(ran.Next(9999));
                objects.Add(acc);
            }
            long total2 = GC.GetTotalMemory(true);
            //long free2 = rt.freeMemory();
            //long used2 = total2 - free2;
            Console.WriteLine("Used memory after creating objects " + total2 + " bytes " +
                              (total2/1024) + " Kb " + (total2/1024/1024) + " Mb");
            int loop = 5;
            long ETTotal = 0;
            for (int idx = 0; idx < loop; idx++)
            {
                Rete engine = new Rete();
                engine.declareObject(typeof (Account), "Account");

                try
                {
                    StreamReader freader = new StreamReader(rulefile);
                    CLIPSParser parser = new CLIPSParser(engine, freader);
                    //Object item = null;
                    ArrayList list = new ArrayList();
                    long start = DateTime.Now.Ticks;
                    parse(engine, parser, list);
                    long end = DateTime.Now.Ticks;
                    long el = end - start;
                    // parser.close();
                    //rt.gc();
                    Console.WriteLine("time to parse rules " + el + " ms");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                IEnumerator itr = objects.GetEnumerator();
                long start2 = DateTime.Now.Ticks;
                try
                {
                    while (itr.MoveNext())
                    {
                        engine.assertObject(itr.Current, "Account", false, false);
                    }
                }
                catch (AssertException e)
                {
                    Console.WriteLine(e.Message);
                }
                long end2 = DateTime.Now.Ticks;
                long start3 = DateTime.Now.Ticks;
                int fired = 0;
                try
                {
                    fired = engine.fire();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                long end3 = DateTime.Now.Ticks;
                long total3 = GC.GetTotalMemory(true);
                //long free3 = rt.freeMemory();
                //long used3 = total3 - free3;
                Console.WriteLine("Number of rules: " +
                                  engine.CurrentFocus.RuleCount);
                Console.WriteLine("rules fired " + fired);
                Console.WriteLine("Used memory after asserting objects " + total3 + " bytes " +
                                  (total3/1024) + " Kb " + (total3/1024/1024) + " Mb");
                Console.WriteLine("number of facts " + engine.ObjectCount);
                Console.WriteLine("memory used by facts " + (total3 - total3)/1024/1024 + " Mb");
                Console.WriteLine("elapsed time to assert " + (end2 - start2) + " ms");
                Console.WriteLine("elapsed time to fire " + (end3 - start3) + " ms");
                ETTotal += (end2 - start2);
                engine.printWorkingMemory(true, false);
                //engine.close();
                engine.clearAll();
                engine.close();
                //rt.gc();
            }
            Console.WriteLine("Average ET to assert " + (ETTotal/loop) + " ms");
        }
    }
}