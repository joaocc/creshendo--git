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
/**
 * @author Peter Lin
 *
 * DefclassTest performs basic unit test of the core functionality
 * of Defclass.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Creshendo.UnitTests.Model;
using Creshendo.Util.Rete;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Creshendo.UnitTests
{
    [TestClass]
    public class DefclassTest
    {
        /**
         * Test TestBean2, which does implement add/remove
         * listener.
         */

        /**
         * Test TestBean and get the BeanInfo
         */

        [TestMethod]
        public void testBeanInfo()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Assert.IsNotNull(dc.BeanInfo);
        }

        [TestMethod]
        public void testDeclareObject()
        {
            Rete engine = new Rete();
            Assert.IsNotNull(engine);
            engine.declareObject(typeof (Account));
            ICollection<object> templ = engine.CurrentFocus.Templates;
            Assert.AreEqual(2, templ.Count);
            engine.close();
        }

        /**
         * Test TestBean2 and make sure the PropertyDescriptor
         * isn't null.
         */

        /**
         * Test createDeftemplate(String) method to make sure the
         * Defclass can create a deftemplate for the given Defclass.
         */

        [TestMethod]
        public void testGetDeftemplate()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            Assert.IsNotNull(dtemp);
            Assert.AreEqual("testBean2", dtemp.Name);
            Console.WriteLine("deftemplate name: " + dtemp.Name);
            Assert.AreEqual(6, dtemp.NumberOfSlots);
            Console.WriteLine("slot count: " + dtemp.NumberOfSlots);
        }

        /**
         * Test defclass using an interface, which defines a domain object
         */

        [TestMethod]
        public void testInterface()
        {
            Defclass dc = new Defclass(typeof (IAccount));
            Assert.IsNotNull(dc);
            Deftemplate dtemp = dc.createDeftemplate("account");
            Assert.IsNotNull(dtemp);
            Assert.AreEqual(14, dtemp.AllSlots.Length);
        }

        /**
         * Test defclass using an object that implements an interface
         */

        [TestMethod]
        public void testInterfaceSlot()
        {
            Defclass dc = new Defclass(typeof (IAccount));
            Assert.IsNotNull(dc);
            Deftemplate dtemp = dc.createDeftemplate("account");
            Assert.IsNotNull(dtemp);
            Assert.AreEqual(14, dtemp.AllSlots.Length);
        }

        [TestMethod]
        public void testJavaBean()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Assert.AreEqual(true, dc.JavaBean);
        }

        /**
         * Test TestBean, which does not implement add/remove
         * listener.
         */

        [TestMethod]
        public void testNonJavaBeans()
        {
            Defclass dc = new Defclass(typeof (TestBean));
            Assert.AreEqual(false, dc.JavaBean);
        }

        [TestMethod]
        public void testObject()
        {
            Defclass dc = new Defclass(typeof (Account));
            Assert.IsNotNull(dc);
            Deftemplate dtemp = dc.createDeftemplate("account");
            Assert.IsNotNull(dtemp);
        }

        [TestMethod]
        public void testPropertyCount()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            if (dc.PropertyDescriptors != null)
            {
                PropertyInfo[] pds = dc.PropertyDescriptors;
                Console.WriteLine(pds.Length);
                for (int idx = 0; idx < pds.Length; idx++)
                {
                    PropertyInfo pd = pds[idx];
                    Console.WriteLine(idx + " name=" + pd.Name);
                }
                Assert.AreEqual(6, pds.Length);
            }
            else
            {
                // this will report the test failed
                Assert.IsNotNull(dc.PropertyDescriptors);
            }
        }

        [TestMethod]
        public void testPropertyDescriptor()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Assert.IsNotNull(dc.PropertyDescriptors);
        }
    }
}