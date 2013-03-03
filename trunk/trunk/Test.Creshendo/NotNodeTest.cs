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
using Test.Creshendo.Model;
using Creshendo.Util.Collections;
using Creshendo.Util.Rete;
using Creshendo.Util.Rete.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Creshendo
{
    [TestClass]
    public class NotNodeTest
    {
        [TestMethod]
        public void testAssertAndRetract()
        {
            // first create a rule engine instance
            Rete engine = new Rete();
            NotJoin bn = new NotJoin(engine.nextNodeId());
            Assert.IsNotNull(bn);

            // create a defclass
            Defclass dc = new Defclass(typeof (TestBean2));
            // create deftemplate
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            Assert.IsNotNull(dtemp);
            Binding[] binds = new Binding[1];
            Binding b1 = new Binding();
            b1.LeftIndex = (0);
            b1.IsObjectVar = (false);
            b1.LeftRow = (0);
            b1.RightIndex = (0);
            b1.VarName = ("var1");
            binds[0] = b1;

            // set the binding
            bn.Bindings = (binds);

            int count = 10;
            ArrayList data = new ArrayList();
            for (int idx = 0; idx < count; idx++)
            {
                TestBean2 bean = new TestBean2();
                bean.Attr1 = ("random");
                bean.Attr2 = (101);
                short s = 10001;
                bean.Attr3 = (s);
                long l = 10101018;
                bean.Attr4 = (l);
                bean.Attr5 = (1010101);
                bean.Attr6 = (1001.1001);
                IFact fact = dtemp.createFact(bean, dc, engine.nextFactId());
                data.Add(fact);
            }

            IEnumerator itr = data.GetEnumerator();
            while (itr.MoveNext())
            {
                try
                {
                    IFact f1 = (IFact) itr.Current;
                    bn.assertLeft(new Index(new IFact[] {f1}), engine, engine.WorkingMemory);
                    bn.assertRight(f1, engine, engine.WorkingMemory);
                }
                catch (AssertException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            IGenericMap<IFact, IFact> rbmem = (IGenericMap<IFact, IFact>)engine.WorkingMemory.getBetaRightMemory(bn);
            Assert.AreEqual(count, rbmem.Count);

            IGenericMap<Object, Object> lbmem = (IGenericMap<Object, Object>) engine.WorkingMemory.getBetaLeftMemory(bn);
            Assert.AreEqual(count, lbmem.Count);

            int retract = 5;
            try
            {
                for (int idx = 0; idx < retract; idx++)
                {
                    IFact f2 = (IFact) data[idx];
                    bn.retractRight(f2, engine, engine.WorkingMemory);
                }
            }
            catch (RetractException e)
            {
                Console.WriteLine(e.Message);
            }

            rbmem = (IGenericMap<IFact, IFact>)engine.WorkingMemory.getBetaRightMemory(bn);
            Assert.AreEqual(retract, rbmem.Count);

            lbmem = (IGenericMap<Object, Object>) engine.WorkingMemory.getBetaLeftMemory(bn);
            Assert.AreEqual(count, lbmem.Count);

            // now check the BetaMemory has matches
            Console.WriteLine(bn.toPPString());
            IEnumerator mitr = lbmem.Values.GetEnumerator();
            while (mitr.MoveNext())
            {
                IBetaMemory btm = (IBetaMemory) mitr.Current;
                Console.WriteLine("match count=" + btm.matchCount() +
                                  " - " + btm.toPPString());
            }
            engine.close();
        }

        [TestMethod]
        public void testAssertLeftMultiple()
        {
            // first create a rule engine instance
            Rete engine = new Rete();
            NotJoin bn = new NotJoin(engine.nextNodeId());
            Assert.IsNotNull(bn);

            // create a defclass
            Defclass dc = new Defclass(typeof (TestBean2));
            // create deftemplate
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            Assert.IsNotNull(dtemp);
            Binding[] binds = new Binding[1];
            Binding b1 = new Binding();
            b1.LeftIndex = (0);
            b1.IsObjectVar = (false);
            b1.LeftRow = (0);
            b1.RightIndex = (0);
            b1.VarName = ("var1");
            binds[0] = b1;

            // set the binding
            bn.Bindings = (binds);

            int count = 10;
            ArrayList data = new ArrayList();
            for (int idx = 0; idx < count; idx++)
            {
                TestBean2 bean = new TestBean2();
                bean.Attr1 = ("random" + (idx + 1));
                bean.Attr2 = (101);
                short s = 10001;
                bean.Attr3 = (s);
                long l = 10101018;
                bean.Attr4 = (l);
                bean.Attr5 = (1010101);
                bean.Attr6 = (1001.1001);
                IFact fact = dtemp.createFact(bean, dc, engine.nextFactId());
                data.Add(fact);
            }
            IEnumerator itr = data.GetEnumerator();
            while (itr.MoveNext())
            {
                try
                {
                    IFact f1 = (IFact) itr.Current;
                    bn.assertLeft(new Index(new IFact[] {f1}), engine, engine.WorkingMemory);
                }
                catch (AssertException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            IGenericMap<Object, Object> bmem = (IGenericMap<Object, Object>) engine.WorkingMemory.getBetaLeftMemory(bn);
            Assert.AreEqual(count, bmem.Count);
            engine.close();
        }

        [TestMethod]
        public void testAssertLeftOne()
        {
            // first create a rule engine instance
            Rete engine = new Rete();
            NotJoin bn = new NotJoin(engine.nextNodeId());
            Assert.IsNotNull(bn);

            // create a defclass
            Defclass dc = new Defclass(typeof (TestBean2));
            // create deftemplate
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            Assert.IsNotNull(dtemp);
            Binding[] binds = new Binding[1];
            Binding b1 = new Binding();
            b1.LeftIndex = (0);
            b1.IsObjectVar = (false);
            b1.LeftRow = (0);
            b1.RightIndex = (0);
            b1.VarName = ("var1");
            binds[0] = b1;

            // set the binding
            bn.Bindings = (binds);

            TestBean2 bean = new TestBean2();
            bean.Attr1 = ("random1");
            bean.Attr2 = (101);
            short s = 10001;
            bean.Attr3 = (s);
            long l = 10101018;
            bean.Attr4 = (l);
            bean.Attr5 = (1010101);
            bean.Attr6 = (1001.1001);
            IFact f1 = dtemp.createFact(bean, dc, engine.nextFactId());
            try
            {
                bn.assertLeft(new Index(new IFact[] {f1}), engine, engine.WorkingMemory);
                IGenericMap<Object, Object> bmem = (IGenericMap<Object, Object>) engine.WorkingMemory.getBetaLeftMemory(bn);
                Assert.AreEqual(1, bmem.Count);
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }
            engine.close();
        }

        /**
         * Assert several object down the right input
         */

        [TestMethod]
        public void testAssertRightMultiple()
        {
            // first create a rule engine instance
            Rete engine = new Rete();
            NotJoin bn = new NotJoin(engine.nextNodeId());
            Assert.IsNotNull(bn);

            // create a defclass
            Defclass dc = new Defclass(typeof (TestBean2));
            // create deftemplate
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            Assert.IsNotNull(dtemp);
            Binding[] binds = new Binding[1];
            Binding b1 = new Binding();
            b1.LeftIndex = (0);
            b1.IsObjectVar = (false);
            b1.LeftRow = (0);
            b1.RightIndex = (0);
            b1.VarName = ("var1");
            binds[0] = b1;

            // set the binding
            bn.Bindings = (binds);

            int count = 10;
            ArrayList data = new ArrayList();
            for (int idx = 0; idx < count; idx++)
            {
                TestBean2 bean = new TestBean2();
                bean.Attr1 = ("random" + (idx + 1));
                bean.Attr2 = (101);
                short s = 10001;
                bean.Attr3 = (s);
                long l = 10101018;
                bean.Attr4 = (l);
                bean.Attr5 = (1010101);
                bean.Attr6 = (1001.1001);
                IFact fact = dtemp.createFact(bean, dc, engine.nextFactId());
                data.Add(fact);
            }
            IEnumerator itr = data.GetEnumerator();
            while (itr.MoveNext())
            {
                try
                {
                    IFact f1 = (IFact) itr.Current;
                    bn.assertRight(f1, engine, engine.WorkingMemory);
                }
                catch (AssertException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            IGenericMap<IFact, IFact> bmem = (IGenericMap<IFact, IFact>)engine.WorkingMemory.getBetaRightMemory(bn);
            Assert.AreEqual(count, bmem.Count);
            engine.close();
        }

        [TestMethod]
        public void testCreateNode()
        {
            Rete engine = new Rete();
            NotJoin bn = new NotJoin(engine.nextNodeId());
            Assert.IsNotNull(bn);
            engine.close();
        }

        [TestMethod]
        public void testCreateNode2()
        {
            // first create a rule engine instance
            Rete engine = new Rete();
            NotJoin bn = new NotJoin(engine.nextNodeId());
            Assert.IsNotNull(bn);

            // create a defclass
            Defclass dc = new Defclass(typeof (TestBean2));
            // create deftemplate
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            Assert.IsNotNull(dtemp);
            Binding[] binds = new Binding[1];
            Binding b1 = new Binding();
            b1.LeftIndex = (0);
            b1.IsObjectVar = (false);
            b1.LeftRow = (0);
            b1.RightIndex = (0);
            b1.VarName = ("var1");
            binds[0] = b1;

            // set the binding
            bn.Bindings = (binds);
            engine.close();
        }

        /**
         * Try asserting 10 objects and make sure the results are correct
         */

        [TestMethod]
        public void testMatch()
        {
            // first create a rule engine instance
            Rete engine = new Rete();
            NotJoin bn = new NotJoin(engine.nextNodeId());
            Assert.IsNotNull(bn);

            // create a defclass
            Defclass dc = new Defclass(typeof (TestBean2));
            // create deftemplate
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            Assert.IsNotNull(dtemp);
            Binding[] binds = new Binding[1];
            Binding b1 = new Binding();
            b1.LeftIndex = (0);
            b1.IsObjectVar = (false);
            b1.LeftRow = (0);
            b1.RightIndex = (0);
            b1.VarName = ("var1");
            binds[0] = b1;

            // set the binding
            bn.Bindings = (binds);

            int count = 10;
            ArrayList data = new ArrayList();
            for (int idx = 0; idx < count; idx++)
            {
                TestBean2 bean = new TestBean2();
                bean.Attr1 = ("random");
                bean.Attr2 = (101);
                short s = 10001;
                bean.Attr3 = (s);
                long l = 10101018;
                bean.Attr4 = (l);
                bean.Attr5 = (1010101);
                bean.Attr6 = (1001.1001);
                IFact fact = dtemp.createFact(bean, dc, engine.nextFactId());
                data.Add(fact);
            }

            IEnumerator itr = data.GetEnumerator();
            while (itr.MoveNext())
            {
                try
                {
                    IFact f1 = (IFact) itr.Current;
                    bn.assertLeft(new Index(new IFact[] {f1}), engine, engine.WorkingMemory);
                    bn.assertRight(f1, engine, engine.WorkingMemory);
                }
                catch (AssertException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            IGenericMap<IFact, IFact> rbmem = (IGenericMap<IFact, IFact>)engine.WorkingMemory.getBetaRightMemory(bn);
            Assert.AreEqual(count, rbmem.Count);

            IGenericMap<Object, Object> lbmem = (IGenericMap<Object, Object>) engine.WorkingMemory.getBetaLeftMemory(bn);
            Assert.AreEqual(count, lbmem.Count);

            // now check the BetaMemory has matches
            Console.WriteLine(bn.toPPString());
            IEnumerator mitr = lbmem.Values.GetEnumerator();
            while (mitr.MoveNext())
            {
                IBetaMemory btm = (IBetaMemory) mitr.Current;
                Assert.AreEqual(9, btm.matchCount());
                Console.WriteLine("match count=" + btm.matchCount() +
                                  " - " + btm.toPPString());
            }
            engine.close();
        }

        /**
         * Try asserting 10 objects and make sure the results are correct
         */

        /**
         * test the NotJoin with facts that don't match. Each BetaMemory
         * should have a match count of zero.
         */

        [TestMethod]
        public void testNoMatch()
        {
            // first create a rule engine instance
            Rete engine = new Rete();
            NotJoin bn = new NotJoin(engine.nextNodeId());
            Assert.IsNotNull(bn);

            // create a defclass
            Defclass dc = new Defclass(typeof (TestBean2));
            // create deftemplate
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            Assert.IsNotNull(dtemp);
            Binding[] binds = new Binding[1];
            Binding b1 = new Binding();
            b1.LeftIndex = (0);
            b1.IsObjectVar = (false);
            b1.LeftRow = (0);
            b1.RightIndex = (0);
            b1.VarName = ("var1");
            binds[0] = b1;

            // set the binding
            bn.Bindings = (binds);

            int count = 10;
            ArrayList data = new ArrayList();
            for (int idx = 0; idx < count; idx++)
            {
                TestBean2 bean = new TestBean2();
                bean.Attr1 = ("random" + idx);
                bean.Attr2 = (101 + idx);
                short s = 10001;
                bean.Attr3 = (s);
                long l = 10101018 + idx;
                bean.Attr4 = (l);
                bean.Attr5 = (1010101);
                bean.Attr6 = (1001.1001);
                IFact fact = dtemp.createFact(bean, dc, engine.nextFactId());
                data.Add(fact);
            }

            IEnumerator itr = data.GetEnumerator();
            while (itr.MoveNext())
            {
                try
                {
                    IFact f1 = (IFact) itr.Current;
                    bn.assertLeft(new Index(new IFact[] {f1}), engine, engine.WorkingMemory);
                    bn.assertRight(f1, engine, engine.WorkingMemory);
                }
                catch (AssertException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            IGenericMap<IFact, IFact> rbmem = (IGenericMap<IFact, IFact>)engine.WorkingMemory.getBetaRightMemory(bn);
            Assert.AreEqual(count, rbmem.Count);

            IGenericMap<Object, Object> lbmem = (IGenericMap<Object, Object>) engine.WorkingMemory.getBetaLeftMemory(bn);
            Assert.AreEqual(count, lbmem.Count);

            // now check the BetaMemory has matches
            Console.WriteLine(bn.toPPString());
            IEnumerator mitr = lbmem.Values.GetEnumerator();
            while (mitr.MoveNext())
            {
                IBetaMemory btm = (IBetaMemory) mitr.Current;
                Assert.AreEqual(0, btm.matchCount());
                Console.WriteLine("match count=" + btm.matchCount() +
                                  " - " + btm.toPPString());
            }
            engine.close();
        }

        /**
         * the test creates a NotJoin and adds a BetaNode as a successor.
         * it then asserts 10 facts and makes sure the facts are propogated
         * to the BetaNode.
         */

        /**
         * method will make sure the correct number of facts are asserted,
         * propogated and retracted.
         */

        [TestMethod]
        public void testPropogateChange()
        {
            Console.WriteLine("testPropogateChange");
            // first create a rule engine instance
            Rete engine = new Rete();
            NotJoin nj = new NotJoin(engine.nextNodeId());
            HashedEqBNode bn2 = new HashedEqBNode(engine.nextNodeId());
            Assert.IsNotNull(nj);

            // create a defclass
            Defclass dc = new Defclass(typeof (TestBean2));
            // create deftemplate
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            Assert.IsNotNull(dtemp);
            Binding[] binds = new Binding[1];
            Binding b1 = new Binding();
            b1.LeftIndex = (0);
            b1.IsObjectVar = (false);
            b1.LeftRow = (0);
            b1.RightIndex = (0);
            b1.VarName = ("var1");
            binds[0] = b1;

            Binding[] binds2 = new Binding[1];
            Binding b2 = new Binding();
            b2.LeftIndex = (1);
            b2.IsObjectVar = (false);
            b2.LeftRow = (0);
            b2.RightIndex = (1);
            b2.VarName = ("var2");
            binds2[0] = b2;

            // set the binding
            nj.Bindings = (binds);

            bn2.Bindings = (binds2);

            // now add the second Not to the first
            try
            {
                nj.addSuccessorNode(bn2, engine, engine.WorkingMemory);
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }

            int count = 2;
            ArrayList data = new ArrayList();
            for (int idx = 0; idx < count; idx++)
            {
                TestBean2 bean = new TestBean2();
                bean.Attr1 = ("random");
                bean.Attr2 = (101 + idx);
                short s = 10001;
                bean.Attr3 = (s);
                long l = 10101018 + idx;
                bean.Attr4 = (l);
                bean.Attr5 = (1010101);
                bean.Attr6 = (1001.1001);
                IFact fact = dtemp.createFact(bean, dc, engine.nextFactId());
                data.Add(fact);
            }

            IEnumerator itr = data.GetEnumerator();
            while (itr.MoveNext())
            {
                try
                {
                    IFact f1 = (IFact) itr.Current;
                    nj.assertLeft(new Index(new IFact[] {f1}), engine, engine.WorkingMemory);
                }
                catch (AssertException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            IGenericMap<Object, Object> lbmem = (IGenericMap<Object, Object>) engine.WorkingMemory.getBetaLeftMemory(nj);
            Assert.AreEqual(count, lbmem.Count);

            IGenericMap<Object, Object> lbmem2 = (IGenericMap<Object, Object>) engine.WorkingMemory.getBetaLeftMemory(bn2);
            Assert.AreEqual(2, lbmem2.Count);

            itr = data.GetEnumerator();
            while (itr.MoveNext())
            {
                try
                {
                    IFact f1 = (IFact) itr.Current;
                    nj.assertRight(f1, engine, engine.WorkingMemory);
                }
                catch (AssertException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            IGenericMap<IFact, IFact> rbmem = (IGenericMap<IFact, IFact>)engine.WorkingMemory.getBetaRightMemory(nj);
            Assert.AreEqual(count, rbmem.Count);

            // once the facts are asserted to the right, there should be no
            // facts in successor. this makes sure that assertRight correctly
            // results in a retract.
            lbmem2 = (IGenericMap<Object, Object>) engine.WorkingMemory.getBetaLeftMemory(bn2);
            Assert.AreEqual(0, lbmem2.Count);
            engine.close();
        }

        /**
         * This test makes sure that when facts are asserted, only the
         * first results in retract propogation. this is because NotJoin
         * should only propogate assert and retract when the match count
         * goes from zero to one or one to zero.
         */

        [TestMethod]
        public void testPropogateChange2()
        {
            Console.WriteLine("testPropogateChange2");
            // first create a rule engine instance
            Rete engine = new Rete();
            NotJoin nj = new NotJoin(engine.nextNodeId());
            HashedEqBNode bn2 = new HashedEqBNode(engine.nextNodeId());
            Assert.IsNotNull(nj);
            Assert.IsNotNull(bn2);

            // create a defclass
            Defclass dc = new Defclass(typeof (TestBean2));
            // create deftemplate
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            Assert.IsNotNull(dtemp);
            Binding[] binds = new Binding[1];
            Binding b1 = new Binding();
            b1.LeftIndex = (0);
            b1.IsObjectVar = (false);
            b1.LeftRow = (0);
            b1.RightIndex = (0);
            b1.VarName = ("var1");
            binds[0] = b1;

            Binding[] binds2 = new Binding[1];
            Binding b2 = new Binding();
            b2.LeftIndex = (1);
            b2.IsObjectVar = (false);
            b2.LeftRow = (0);
            b2.RightIndex = (1);
            b2.VarName = ("var2");
            binds2[0] = b2;

            // set the binding
            nj.Bindings = (binds);

            bn2.Bindings = (binds2);

            // now add the second Not to the first
            try
            {
                nj.addSuccessorNode(bn2, engine, engine.WorkingMemory);
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }

            int count = 5;
            ArrayList data = new ArrayList();
            for (int idx = 0; idx < count; idx++)
            {
                TestBean2 bean = new TestBean2();
                bean.Attr1 = ("random");
                bean.Attr2 = (101 + idx);
                short s = 10001;
                bean.Attr3 = (s);
                long l = 10101018 + idx;
                bean.Attr4 = (l);
                bean.Attr5 = (1010101);
                bean.Attr6 = (1001.1001);
                IFact fact = dtemp.createFact(bean, dc, engine.nextFactId());
                data.Add(fact);
            }

            IEnumerator itr = data.GetEnumerator();
            while (itr.MoveNext())
            {
                try
                {
                    IFact f1 = (IFact) itr.Current;
                    nj.assertLeft(new Index(new IFact[] {f1}), engine, engine.WorkingMemory);
                }
                catch (AssertException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            IGenericMap<Object, Object> lbmem = (IGenericMap<Object, Object>) engine.WorkingMemory.getBetaLeftMemory(nj);
            Assert.AreEqual(count, lbmem.Count);

            IGenericMap<Object, Object> lbmem2 = (IGenericMap<Object, Object>) engine.WorkingMemory.getBetaLeftMemory(bn2);
            Assert.AreEqual(count, lbmem2.Count);


            try
            {
                IFact f = (IFact) data[0];
                nj.assertRight(f, engine, engine.WorkingMemory);
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }

            IGenericMap<IFact, IFact> rbmem = (IGenericMap<IFact, IFact>)engine.WorkingMemory.getBetaRightMemory(nj);
            Assert.AreEqual(1, rbmem.Count);

            // once the facts are asserted to the right, there should be no
            // facts in successor. this makes sure that assertRight correctly
            // results in a retract.
            lbmem2 = (IGenericMap<Object, Object>) engine.WorkingMemory.getBetaLeftMemory(bn2);
            Assert.AreEqual(1, lbmem2.Count);

            // now assert the other facts to the NotJoin.assertRight
            try
            {
                for (int idx = 1; idx < count; idx++)
                {
                    IFact f = (IFact) data[idx];
                    nj.assertRight(f, engine, engine.WorkingMemory);
                }
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(nj.toPPString());
            IEnumerator mitr2 = lbmem.Values.GetEnumerator();
            while (mitr2.MoveNext())
            {
                IBetaMemory btm = (IBetaMemory) mitr2.Current;
                Console.WriteLine("match count=" + btm.matchCount() +
                                  " - " + btm.toPPString());
            }
            engine.close();
        }

        [TestMethod]
        public void testPropogateNoMatch()
        {
            Console.WriteLine("testPropogateNoMatch");
            // first create a rule engine instance
            Rete engine = new Rete();
            NotJoin nj = new NotJoin(engine.nextNodeId());
            HashedEqBNode bn2 = new HashedEqBNode(engine.nextNodeId());
            Assert.IsNotNull(nj);

            // create a defclass
            Defclass dc = new Defclass(typeof (TestBean2));
            // create deftemplate
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            Assert.IsNotNull(dtemp);
            Binding[] binds = new Binding[1];
            Binding b1 = new Binding();
            b1.LeftIndex = (0);
            b1.IsObjectVar = (false);
            b1.LeftRow = (0);
            b1.RightIndex = (0);
            b1.VarName = ("var1");
            binds[0] = b1;

            Binding[] binds2 = new Binding[1];
            Binding b2 = new Binding();
            b2.LeftIndex = (1);
            b2.IsObjectVar = (false);
            b2.LeftRow = (0);
            b2.RightIndex = (1);
            b2.VarName = ("var2");
            binds2[0] = b2;

            // set the binding
            nj.Bindings = (binds);

            bn2.Bindings = (binds2);

            // now add the second Not to the first
            try
            {
                nj.addSuccessorNode(bn2, engine, engine.WorkingMemory);
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }

            int count = 10;
            ArrayList data = new ArrayList();
            for (int idx = 0; idx < count; idx++)
            {
                TestBean2 bean = new TestBean2();
                bean.Attr1 = ("random" + idx);
                bean.Attr2 = (101 + idx);
                short s = 10001;
                bean.Attr3 = (s);
                long l = 10101018 + idx;
                bean.Attr4 = (l);
                bean.Attr5 = (1010101);
                bean.Attr6 = (1001.1001);
                IFact fact = dtemp.createFact(bean, dc, engine.nextFactId());
                data.Add(fact);
            }

            IEnumerator itr = data.GetEnumerator();
            while (itr.MoveNext())
            {
                try
                {
                    IFact f1 = (IFact) itr.Current;
                    nj.assertLeft(new Index(new IFact[] {f1}), engine, engine.WorkingMemory);
                    nj.assertRight(f1, engine, engine.WorkingMemory);
                }
                catch (AssertException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            IGenericMap<IFact, IFact> rbmem = (IGenericMap<IFact, IFact>)engine.WorkingMemory.getBetaRightMemory(nj);
            Assert.AreEqual(count, rbmem.Count);

            IGenericMap<Object, Object> lbmem = (IGenericMap<Object, Object>) engine.WorkingMemory.getBetaLeftMemory(nj);
            Assert.AreEqual(count, lbmem.Count);

            // now check the BetaMemory has matches
            Console.WriteLine(nj.toPPString());
            IEnumerator mitr = lbmem.Values.GetEnumerator();
            while (mitr.MoveNext())
            {
                IBetaMemory btm = (IBetaMemory) mitr.Current;
                Assert.AreEqual(0, btm.matchCount());
                Console.WriteLine("match count=" + btm.matchCount() +
                                  " - " + btm.toPPString());
            }

            IGenericMap<Object, Object> lbmem2 = (IGenericMap<Object, Object>) engine.WorkingMemory.getBetaLeftMemory(bn2);
            Assert.AreEqual(count, lbmem2.Count);
            Console.WriteLine(bn2.toPPString());
            IEnumerator mitr2 = lbmem2.Values.GetEnumerator();
            engine.close();
            // TODO need to update the test to check the match count
            // by getting the right memory
        }
    }
}