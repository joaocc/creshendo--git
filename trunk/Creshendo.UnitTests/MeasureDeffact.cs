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
using Creshendo.UnitTests.Model;
using Creshendo.Util.Rete;
using Creshendo.Util.Rete.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Creshendo.UnitTests
{
    [TestClass]
    public class MeasureDeffact
    {
        private static Random ran = new Random();

        [TestMethod]
        public void MeasureDeffactTest()
        {
            ArrayList objects = new ArrayList();
            // Runtime rt = Runtime.getRuntime();
            long total1 = GC.GetTotalMemory(true);
            //long free1 = rt.freeMemory();
            //long used1 = total1 - free1;
            int count = 50000;
            Console.WriteLine("Used memory before creating objects " + total1 + " bytes " +
                              (total1/1024) + " Kb");
            for (int idx = 0; idx < count; idx++)
            {
                Account acc = new Account();
                acc.AccountId = Convert.ToString(ran.Next(100000));
                acc.AccountType = Convert.ToString(ran.Next(100000));
                acc.First = Convert.ToString(ran.Next(100000));
                acc.Last = Convert.ToString(ran.Next(100000));
                acc.Middle = Convert.ToString(ran.Next(100000));
                acc.OfficeCode = Convert.ToString(ran.Next(100000));
                acc.RegionCode = Convert.ToString(ran.Next(100000));
                acc.Status = Convert.ToString(ran.Next(100000));
                acc.Title = Convert.ToString(ran.Next(100000));
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
            Rete engine = new Rete();
            engine.declareObject(typeof (Account));
            IEnumerator itr = objects.GetEnumerator();
            long start = DateTime.Now.Ticks;
            try
            {
                while (itr.MoveNext())
                {
                    engine.assertObject(itr.Current, null, false, false);
                }
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }
            long end = DateTime.Now.Ticks;
            long total3 = GC.GetTotalMemory(true);
            //long free3 = rt.freeMemory();
            //long used3 = total3 - free3;
            //rt.gc();
            Console.WriteLine("Used memory after asserting objects " + total3 + " bytes " +
                              (total3/1024) + " Kb " + (total3/1024/1024) + " Mb");
            Console.WriteLine("number of facts " + engine.ObjectCount);
            Console.WriteLine("memory used by facts " + (total3 - total3)/1024/1024 + " Mb");
            Console.WriteLine("elapsed time is " + (end - start) + " ms");
            engine.close();
        }
    }
}