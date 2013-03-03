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
using Creshendo.Util.Collections;
using Creshendo.Util.Rete;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Creshendo
{
    [TestClass]
    public class CompositeIndexTest
    {
        [TestMethod]
        public void testEqual()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            TestBean2 bean = new TestBean2();
            bean.Attr1 = ("testString");
            bean.Attr2 = (1);
            short a3 = 3;
            bean.Attr3 = (a3);
            long a4 = 101;
            bean.Attr4 = (a4);
            float a5 = 10101;
            bean.Attr5 = (a5);
            double a6 = 101.101;
            bean.Attr6 = (a6);

            IFact fact = dtemp.createFact(bean, dc, 1);
            Assert.IsNotNull(fact);
            Console.WriteLine(fact.toFactString());
            CompositeIndex ci = new CompositeIndex("attr1", Constants.EQUAL, fact.getSlotValue(0));
            Assert.IsNotNull(ci);
            Console.WriteLine(ci.toPPString());
        }

        [TestMethod]
        public void testIndex()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            TestBean2 bean = new TestBean2();
            bean.Attr1 = ("testString");
            bean.Attr2 = (1);
            short a3 = 3;
            bean.Attr3 = (a3);
            long a4 = 101;
            bean.Attr4 = (a4);
            float a5 = 10101;
            bean.Attr5 = (a5);
            double a6 = 101.101;
            bean.Attr6 = (a6);

            IFact fact = dtemp.createFact(bean, dc, 1);
            Assert.IsNotNull(fact);
            Console.WriteLine(fact.toFactString());
            CompositeIndex ci =
                new CompositeIndex("attr1", Constants.EQUAL, fact.getSlotValue(0));
            Assert.IsNotNull(ci);
            Console.WriteLine(ci.toPPString());
            GenericHashMap<object, object> map = new GenericHashMap<object, object>();
            map.Put(ci, bean);

            CompositeIndex ci2 =
                new CompositeIndex("attr1", Constants.EQUAL, fact.getSlotValue(0));
            Assert.IsTrue(map.ContainsKey(ci2));

            CompositeIndex ci3 =
                new CompositeIndex("attr1", Constants.NOTEQUAL, fact.getSlotValue(0));
            Assert.IsFalse(map.ContainsKey(ci3));

            CompositeIndex ci4 =
                new CompositeIndex("attr1", Constants.NILL, fact.getSlotValue(0));
            Assert.IsFalse(map.ContainsKey(ci4));

            CompositeIndex ci5 =
                new CompositeIndex("attr1", Constants.NOTNILL, fact.getSlotValue(0));
            Assert.IsFalse(map.ContainsKey(ci5));
        }

        [TestMethod]
        public void testIndex2()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            TestBean2 bean = new TestBean2();
            bean.Attr1 = ("testString");
            bean.Attr2 = (1);
            short a3 = 3;
            bean.Attr3 = (a3);
            long a4 = 101;
            bean.Attr4 = (a4);
            float a5 = 10101;
            bean.Attr5 = (a5);
            double a6 = 101.101;
            bean.Attr6 = (a6);

            IFact fact = dtemp.createFact(bean, dc, 1);
            Assert.IsNotNull(fact);
            Console.WriteLine(fact.toFactString());
            CompositeIndex ci =
                new CompositeIndex("attr2", Constants.EQUAL, fact.getSlotValue(1));
            Assert.IsNotNull(ci);
            Console.WriteLine(ci.toPPString());
            GenericHashMap<object, object> map = new GenericHashMap<object, object>();
            map.Put(ci, bean);

            CompositeIndex ci2 =
                new CompositeIndex("attr2", Constants.EQUAL, fact.getSlotValue(1));
            Assert.IsTrue(map.ContainsKey(ci2));

            CompositeIndex ci3 =
                new CompositeIndex("attr2", Constants.NOTEQUAL, fact.getSlotValue(1));
            Assert.IsFalse(map.ContainsKey(ci3));

            CompositeIndex ci4 =
                new CompositeIndex("attr2", Constants.NILL, fact.getSlotValue(1));
            Assert.IsFalse(map.ContainsKey(ci4));

            CompositeIndex ci5 =
                new CompositeIndex("attr2", Constants.NOTNILL, fact.getSlotValue(1));
            Assert.IsFalse(map.ContainsKey(ci5));
        }

        [TestMethod]
        public void testNil()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            TestBean2 bean = new TestBean2();
            bean.Attr2 = (1);
            short a3 = 3;
            bean.Attr3 = (a3);
            long a4 = 101;
            bean.Attr4 = (a4);
            float a5 = 10101;
            bean.Attr5 = (a5);
            double a6 = 101.101;
            bean.Attr6 = (a6);

            IFact fact = dtemp.createFact(bean, dc, 1);
            Assert.IsNotNull(fact);
            Console.WriteLine(fact.toFactString());
            CompositeIndex ci =
                new CompositeIndex("attr1", Constants.NILL, fact.getSlotValue(0));
            Assert.IsNotNull(ci);
            Console.WriteLine(ci.toPPString());
        }

        [TestMethod]
        public void testNotEqual()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            TestBean2 bean = new TestBean2();
            bean.Attr1 = ("testString");
            bean.Attr2 = (1);
            short a3 = 3;
            bean.Attr3 = (a3);
            long a4 = 101;
            bean.Attr4 = (a4);
            float a5 = 10101;
            bean.Attr5 = (a5);
            double a6 = 101.101;
            bean.Attr6 = (a6);

            IFact fact = dtemp.createFact(bean, dc, 1);
            Assert.IsNotNull(fact);
            Console.WriteLine(fact.toFactString());
            CompositeIndex ci =
                new CompositeIndex("attr1", Constants.NOTEQUAL, fact.getSlotValue(0));
            Assert.IsNotNull(ci);
            Console.WriteLine(ci.toPPString());
        }

        [TestMethod]
        public void testNotNil()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            TestBean2 bean = new TestBean2();
            bean.Attr1 = ("testString");
            bean.Attr2 = (1);
            short a3 = 3;
            bean.Attr3 = (a3);
            long a4 = 101;
            bean.Attr4 = (a4);
            float a5 = 10101;
            bean.Attr5 = (a5);
            double a6 = 101.101;
            bean.Attr6 = (a6);

            IFact fact = dtemp.createFact(bean, dc, 1);
            Assert.IsNotNull(fact);
            Console.WriteLine(fact.toFactString());
            CompositeIndex ci =
                new CompositeIndex("attr1", Constants.NOTNILL, fact.getSlotValue(0));
            Assert.IsNotNull(ci);
            Console.WriteLine(ci.toPPString());
        }
    }
}