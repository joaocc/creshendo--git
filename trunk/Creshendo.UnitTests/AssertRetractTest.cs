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
using NUnit.Framework;

namespace Creshendo.UnitTests
{
    [TestFixture]
    public class AssertRetractTest
    {
        [Test]
        public void testRetractNoShadow()
        {
            Console.WriteLine("testRetractNoShadow");
            Random ran = new Random();
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
            long assertET = end - start;
            long total3 = GC.GetTotalMemory(true);
            //long free3 = rt.freeMemory();
            //long used3 = total3 - free3;
            //rt.gc();
            Console.WriteLine("Used memory after asserting objects " + total3 + " bytes " +
                              (total3/1024) + " Kb " + (total3/1024/1024) + " Mb");
            Console.WriteLine("number of facts " + engine.ObjectCount);
            Console.WriteLine("memory used by facts " + (total3 - total3)/1024/1024 + " Mb");
            Console.WriteLine("elapsed time is assert " + assertET + " ms");
            // now retract
            IEnumerator itr2 = objects.GetEnumerator();
            long retstart = DateTime.Now.Ticks;
            try
            {
                while (itr2.MoveNext())
                {
                    engine.retractObject(itr2.Current);
                }
            }
            catch (RetractException e)
            {
                Console.WriteLine(e.Message);
            }
            long retend = DateTime.Now.Ticks;
            long retractET = retend - retstart;
            long total4 = GC.GetTotalMemory(true);
            //long free4 = rt.freeMemory();
            //long used4 = total4 - free4;
            objects.Clear();
            engine.clearAll();
            engine.close();
            //rt.gc();
            Console.WriteLine("elapsed time to retract " + retractET + " ms");
            // the retract should be atleast 3 times shorter than the assert
#if DEBUG
            Assert.IsTrue(retractET > 0);
#else
            Assert.IsTrue((assertET > (retractET*3)));
#endif
        }

        [Test]
        public void testRetractWithShadow()
        {
            Console.WriteLine("testRetractWithShadow");
            Random ran = new Random();
            ArrayList objects = new ArrayList();
            // Runtime rt = Runtime.getRuntime();
            long total1 = GC.GetTotalMemory(true);
            //long free1 = rt.freeMemory();
            //long used1 = total1 - free1;
            int count = 5000;
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
                    engine.assertObject(itr.Current, null, false, true);
                }
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }
            long end = DateTime.Now.Ticks;
            long assertET = end - start;
            long total3 = GC.GetTotalMemory(true);
            //long free3 = rt.freeMemory();
            //long used3 = total3 - free3;
            //rt.gc();
            Console.WriteLine("Used memory after asserting objects " + total3 + " bytes " +
                              (total3/1024) + " Kb " + (total3/1024/1024) + " Mb");
            Console.WriteLine("number of facts " + engine.ObjectCount);
            Console.WriteLine("memory used by facts " + (total3 - total3)/1024/1024 + " Mb");
            Console.WriteLine("elapsed time is assert " + assertET + " ms");
            // now retract
            IEnumerator itr2 = objects.GetEnumerator();
            long retstart = DateTime.Now.Ticks;
            try
            {
                while (itr2.MoveNext())
                {
                    engine.retractObject(itr2.Current);
                }
            }
            catch (RetractException e)
            {
                Console.WriteLine(e.Message);
            }
            long retend = DateTime.Now.Ticks;
            long retractET = retend - retstart;
            long total4 = GC.GetTotalMemory(true);
            //long free4 = rt.freeMemory();
            //long used4 = total4 - free4;
            objects.Clear();
            //rt.gc();
            Console.WriteLine("elapsed time to retract " + retractET + " ms");
            // the retract should be atleast 3 times shorter than the assert
            Assert.IsTrue((assertET > (retractET*4)));
            engine.close();
        }
    }
}