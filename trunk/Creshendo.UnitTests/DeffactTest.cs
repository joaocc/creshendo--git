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
using Creshendo.UnitTests.Model;
using Creshendo.Util.Rete;
using NUnit.Framework;

namespace Creshendo.UnitTests
{
    [TestFixture]
    public class DeffactTest
    {
        [Test]
        public void testCreateDeffact()
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
        }

        [Test]
        public void testCreateDeffactWithNull()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            TestBean2 bean = new TestBean2();
            bean.Attr1 = (null);
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
        }

        [Test]
        public void testCreateDeffactWithPrimitive()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            TestBean2 bean = new TestBean2();
            bean.Attr1 = ("testString");

            IFact fact = dtemp.createFact(bean, dc, 1);
            Assert.IsNotNull(fact);
            Console.WriteLine(fact.toFactString());
        }
    }
}