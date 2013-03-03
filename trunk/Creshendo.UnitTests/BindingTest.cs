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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Creshendo.UnitTests
{
    [TestClass]
    public class BindingTest
    {
        [TestInitialize]
        public void setUp()
        {
            Console.WriteLine("this test does not do any setup");
        }

        [TestCleanup]
        public void tearDown()
        {
            Console.WriteLine("this test does not do any teardown");
        }

        [TestMethod]
        public void testSingleBinding()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");

            Slot[] slts = dtemp.AllSlots;

            Binding bn = new Binding();
            bn.LeftRow = (0);
            bn.LeftIndex = (0);
            bn.RightIndex = (0);

            Binding[] binds = {bn};
            HashedEqBNode btnode = new HashedEqBNode(1);
            btnode.Bindings = (binds);

            Console.WriteLine("betaNode::" + btnode.toPPString());
            Assert.IsNotNull(btnode.toPPString());
        }

        [TestMethod]
        public void testThreeBinding()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");

            Defclass dc2 = new Defclass(typeof (TestBean3));
            Deftemplate dtemp2 = dc.createDeftemplate("testBean3");


            Slot[] slts = dtemp.AllSlots;
            Slot[] slts2 = dtemp2.AllSlots;

            Binding bn = new Binding();
            bn.LeftRow = (0);
            bn.LeftIndex = (0);
            bn.RightIndex = (0);

            Binding bn2 = new Binding();
            bn2.LeftRow = (0);
            bn2.LeftIndex = (2);
            bn2.RightIndex = (2);

            Binding bn3 = new Binding();
            bn3.LeftRow = (1);
            bn3.LeftIndex = (0);
            bn3.RightIndex = (0);

            Binding[] binds = {bn, bn2, bn3};
            HashedEqBNode btnode = new HashedEqBNode(1);
            btnode.Bindings = (binds);

            Console.WriteLine("betaNode::" + btnode.toPPString());
            Assert.IsNotNull(btnode.toPPString());
        }

        [TestMethod]
        public void testThreeBinding2()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");

            Defclass dc2 = new Defclass(typeof (TestBean3));
            Deftemplate dtemp2 = dc.createDeftemplate("testBean3");


            Slot[] slts = dtemp.AllSlots;
            Slot[] slts2 = dtemp2.AllSlots;

            Binding bn = new Binding();
            bn.LeftRow = (0);
            bn.LeftIndex = (0);
            bn.RightIndex = (0);

            Binding bn2 = new Binding();
            bn2.LeftRow = (0);
            bn2.LeftIndex = (2);
            bn2.RightIndex = (2);

            Binding bn3 = new Binding();
            bn3.LeftRow = (0);
            bn3.LeftIndex = (0);
            bn3.RightIndex = (0);

            Binding[] binds = {bn, bn2, bn3};
            HashedEqBNode btnode = new HashedEqBNode(1);
            btnode.Bindings = (binds);

            Console.WriteLine("betaNode::" + btnode.toPPString());
            Assert.IsNotNull(btnode.toPPString());
        }

        [TestMethod]
        public void testTwoBinding()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");

            Slot[] slts = dtemp.AllSlots;

            Binding bn = new Binding();
            bn.LeftRow = (0);
            bn.LeftIndex = (0);
            bn.RightIndex = (0);

            Binding bn2 = new Binding();
            bn2.LeftRow = (0);
            bn2.LeftIndex = (2);
            bn2.RightIndex = (2);

            Binding[] binds = {bn, bn2};
            HashedEqBNode btnode = new HashedEqBNode(1);
            btnode.Bindings = (binds);

            Console.WriteLine("betaNode::" + btnode.toPPString());
            Assert.IsNotNull(btnode.toPPString());
        }
    }
}