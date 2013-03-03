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
using Test.Creshendo.Model;
using Creshendo.Util.Rete;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Creshendo
{
    [TestClass]
    public class DeftemplateTest
    {
        /**
         * Basic test of Defclass.createDeftemplate(String). the method
         * uses TestBean2 to create a Defclass.
         */

        [TestMethod]
        public void testCreateFactFromInstance()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            TestBean2 bean = new TestBean2();
            bean.Attr1 = ("random1");
            bean.Attr2 = (101);
            short s = 10001;
            bean.Attr3 = (s);
            long l = 10101018;
            bean.Attr4 = (l);
            bean.Attr5 = (1010101);
            bean.Attr6 = (1001.1001);
            IFact fact = dtemp.createFact(bean, dc, 0);
            Assert.IsNotNull(fact);
            Console.WriteLine(fact.toFactString());
        }

        [TestMethod]
        public void testCreateTemplate2()
        {
            Defclass dc = new Defclass(typeof (Account));
            Deftemplate dtemp = dc.createDeftemplate("account");
            Assert.IsNotNull(dtemp);
            Assert.IsNotNull(dtemp.toPPString());
            Console.WriteLine(dtemp.toPPString());
        }

        [TestMethod]
        public void testCreateTemplateFromClass()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            Assert.IsNotNull(dtemp);
            Console.WriteLine(dtemp.toPPString());
        }

        /**
         * the test creates 4 slots and uses them to create a deftemplate
         * the method only test the getNumberOfSlots method and 
         * toPPEString.
         */

        [TestMethod]
        public void testCreateTemplateFromSlots()
        {
            Slot[] slots = new Slot[4];
            slots[0] = new Slot();
            slots[0].Id = (0);
            slots[0].Name = ("col1");
            slots[0].ValueType = (Constants.INT_PRIM_TYPE);

            slots[1] = new Slot();
            slots[1].Id = (1);
            slots[1].Name = ("col2");
            slots[1].ValueType = (Constants.DOUBLE_PRIM_TYPE);

            slots[2] = new Slot();
            slots[2].Id = (2);
            slots[2].Name = ("col3");
            slots[2].ValueType = (Constants.OBJECT_TYPE);

            slots[3] = new Slot();
            slots[3].Id = (3);
            slots[3].Name = ("col4");
            slots[3].ValueType = (Constants.LONG_PRIM_TYPE);

            Deftemplate dtemp = new Deftemplate("template1", null, slots);
            Assert.IsNotNull(dtemp);
            Assert.AreEqual(4, dtemp.NumberOfSlots);
            Console.WriteLine(dtemp.toPPString());
        }

        /**
         * method uses Account class to create a Defclass. the method tests
         * toPPEString.
         */

        /**
         * method will use Account class to create a Defclass first.
         * Once it has the deftemplate, we check to make sure the slots
         * have the correct slot id, which is the column id. this makes
         * sure that we can efficiently update facts using the slot id.
         */

        [TestMethod]
        public void testCreateTemplateSlot()
        {
            String acc = "account";
            Defclass dc = new Defclass(typeof (Account));
            Deftemplate dtemp = dc.createDeftemplate(acc);
            Assert.IsNotNull(dtemp);
            Assert.IsNotNull(dtemp.toPPString());
            Assert.AreEqual(acc, dtemp.Name);
            Console.WriteLine(dtemp.toPPString());
            Slot[] theslots = dtemp.AllSlots;
            for (int idx = 0; idx < theslots.Length; idx++)
            {
                Slot aslot = theslots[idx];
                Assert.AreEqual(idx, aslot.Id);
                Console.WriteLine("slot id: " + aslot.Id);
            }
        }

        [TestMethod]
        public void testSlotID()
        {
            Slot[] slots = new Slot[4];
            slots[0] = new Slot();
            slots[0].Id = (0);
            slots[0].Name = ("col1");
            slots[0].ValueType = (Constants.INT_PRIM_TYPE);

            slots[1] = new Slot();
            slots[1].Id = (1);
            slots[1].Name = ("col2");
            slots[1].ValueType = (Constants.DOUBLE_PRIM_TYPE);

            slots[2] = new Slot();
            slots[2].Id = (2);
            slots[2].Name = ("col3");
            slots[2].ValueType = (Constants.OBJECT_TYPE);

            slots[3] = new Slot();
            slots[3].Id = (3);
            slots[3].Name = ("col4");
            slots[3].ValueType = (Constants.LONG_PRIM_TYPE);

            Deftemplate dtemp = new Deftemplate("template1", null, slots);
            Assert.IsNotNull(dtemp);
            Assert.AreEqual(4, dtemp.NumberOfSlots);
            Console.WriteLine(dtemp.toPPString());
            Slot[] theslots = dtemp.AllSlots;
            for (int idx = 0; idx < theslots.Length; idx++)
            {
                Slot aslot = theslots[idx];
                Assert.AreEqual(idx, aslot.Id);
                Console.WriteLine("slot id: " + aslot.Id);
            }
        }
    }
}