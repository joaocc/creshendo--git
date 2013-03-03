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
using System.Collections.Generic;
using Test.Creshendo.Model;
using Creshendo.Util.Rete;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Creshendo
{
    [TestClass]
    public class DeclareClassTest
    {
        [TestMethod]
        public void testDeclareClass()
        {
            Rete engine = new Rete();
            Assert.IsNotNull(engine);
            engine.declareObject(typeof (Account));
            int count = engine.Defclasses.Count;
            Assert.AreEqual(1, count);
            Console.WriteLine("number of Defclass is " + count);
            ICollection<object> templates = engine.CurrentFocus.Templates;
            Assert.AreEqual(2, templates.Count);
            IEnumerator itr = templates.GetEnumerator();
            while (itr.MoveNext())
            {
                Deftemplate dtemp = (Deftemplate) itr.Current;
                Console.WriteLine(dtemp.toPPString());
            }
            Console.WriteLine("--------------------------------");
            engine.close();
        }

        [TestMethod]
        public void testDeclareClass2()
        {
            Rete engine = new Rete();
            Assert.IsNotNull(engine);
            engine.declareObject(typeof (Account));
            engine.declareObject(typeof (TestBean3));
            int count = engine.Defclasses.Count;
            Assert.AreEqual(2, count);
            Console.WriteLine("number of Defclass is " + count);
            ICollection<object> templates = engine.CurrentFocus.Templates;
            Assert.AreEqual(3, templates.Count);
            IEnumerator itr = templates.GetEnumerator();
            while (itr.MoveNext())
            {
                Deftemplate dtemp = (Deftemplate) itr.Current;
                Console.WriteLine(dtemp.toPPString());
            }
            Console.WriteLine("--------------------------------");
            engine.close();
        }

        [TestMethod]
        public void testDeclareClassInheritance()
        {
            Console.WriteLine("\ntestDeclareClassInheritance");
            Rete engine = new Rete();
            Assert.IsNotNull(engine);
            engine.declareObject(typeof (Account));
            engine.declareObject(typeof (BackupAccount));
            int count = engine.Defclasses.Count;
            Assert.AreEqual(2, count);
            Console.WriteLine("number of Defclass is " + count);
            ITemplate acctemp = engine.CurrentFocus.getTemplate(typeof (Account).FullName);
            ITemplate bkacc = engine.CurrentFocus.getTemplate(typeof (BackupAccount).FullName);
            Slot[] accslots = acctemp.AllSlots;
            Slot[] bkslots = bkacc.AllSlots;
            for (int idx = 0; idx < accslots.Length; idx++)
            {
                Assert.IsTrue(accslots[idx].Name.Equals(bkslots[idx].Name));
                Console.WriteLine(accslots[idx].Name + "=" + bkslots[idx].Name);
            }
            engine.close();
        }

        [TestMethod]
        public void testDeclareClassInheritance2()
        {
            Console.WriteLine("\ntestDeclareClassInheritance2");
            Rete engine = new Rete();
            Assert.IsNotNull(engine);
            engine.declareObject(typeof (BackupAccount));
            engine.declareObject(typeof (Account));
            int count = engine.Defclasses.Count;
            Assert.AreEqual(2, count);
            Console.WriteLine("number of Defclass is " + count);
            ITemplate acctemp = engine.CurrentFocus.getTemplate(typeof (Account).FullName);
            ITemplate bkacc = engine.CurrentFocus.getTemplate(typeof (BackupAccount).FullName);
            Slot[] accslots = acctemp.AllSlots;
            Slot[] bkslots = bkacc.AllSlots;
            for (int idx = 0; idx < accslots.Length; idx++)
            {
                Assert.IsTrue(accslots[idx].Name.Equals(bkslots[idx].Name));
                Console.WriteLine(accslots[idx].Name + "=" + bkslots[idx].Name);
            }
            engine.close();
        }

        [TestMethod]
        public void testDeclareClassInheritance3()
        {
            Console.WriteLine("\ntestDeclareClassInheritance3");
            Rete engine = new Rete();
            Assert.IsNotNull(engine);
            engine.declareObject(typeof (Account));
            engine.declareObject(typeof (Account2), null, typeof (Account).FullName);
            int count = engine.Defclasses.Count;
            Assert.AreEqual(2, count);
            Console.WriteLine("number of Defclass is " + count);
            ITemplate acctemp = engine.CurrentFocus.getTemplate(typeof (Account).FullName);
            ITemplate acc2 = engine.CurrentFocus.getTemplate(typeof (Account2).FullName);
            Slot[] accslots = acctemp.AllSlots;
            Slot[] acc2slots = acc2.AllSlots;
            for (int idx = 0; idx < accslots.Length; idx++)
            {
                Assert.IsTrue(accslots[idx].Name.Equals(acc2slots[idx].Name));
                Console.WriteLine(accslots[idx].Name + "=" + acc2slots[idx].Name);
            }
            engine.close();
        }

        [TestMethod]
        public void testDeclareClassInheritance4()
        {
            Console.WriteLine("\ntestDeclareClassInheritance3");
            Rete engine = new Rete();
            Assert.IsNotNull(engine);
            engine.declareObject(typeof (Account));
            engine.declareObject(typeof (Account2), null, typeof (Account).FullName);
            engine.declareObject(typeof (Account3), null, typeof (Account2).FullName);
            int count = engine.Defclasses.Count;
            Assert.AreEqual(3, count);
            Console.WriteLine("number of Defclass is " + count);
            ITemplate acctemp = engine.CurrentFocus.getTemplate(typeof (Account).FullName);
            ITemplate acc3 = engine.CurrentFocus.getTemplate(typeof (Account3).FullName);
            Slot[] accslots = acctemp.AllSlots;
            Slot[] acc3slots = acc3.AllSlots;
            for (int idx = 0; idx < accslots.Length; idx++)
            {
                Assert.IsTrue(accslots[idx].Name.Equals(acc3slots[idx].Name));
                Console.WriteLine(accslots[idx].Name + "=" + acc3slots[idx].Name);
            }
            engine.close();
        }

        [TestMethod]
        public void testDeftemplate()
        {
            Rete engine = new Rete();
            Assert.IsNotNull(engine);
            engine.declareObject(typeof (Account));
            Assert.IsNotNull(engine.CurrentFocus.Templates);
            int count = engine.CurrentFocus.TemplateCount;
            Assert.AreEqual(2, count);
            Console.WriteLine("number of Deftemplates is " + count);
            engine.close();
        }
    }
}