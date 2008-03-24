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
using Creshendo.Util.Rete;
using NUnit.Framework;

namespace Creshendo.UnitTests
{
    [TestFixture]
    public class ReteInitTest
    {
        [Test]
        public void testInit()
        {
            Rete engine = new Rete();
            Assert.IsNotNull(engine);
        }

        [Test]
        public void testInitModule()
        {
            Rete engine = new Rete();
            Assert.IsNotNull(engine);
            Assert.IsNotNull(engine.CurrentFocus);
            Assert.IsNotNull(engine.CurrentFocus.ModuleName);
            Assert.AreEqual(engine.CurrentFocus.ModuleName, Constants.MAIN_MODULE);
            Console.WriteLine("default module is " + engine.CurrentFocus.ModuleName);
            engine.close();
        }

        /**
         * Simple test to make sure the nodeId method work correctly
         *
         */

        [Test]
        public void testNodeId()
        {
            Rete engine = new Rete();
            Assert.IsNotNull(engine);
            Assert.AreEqual(3, engine.peakNextNodeId());
            Assert.AreEqual(3, engine.peakNextNodeId());
            Assert.AreEqual(3, engine.peakNextNodeId());
            Console.WriteLine("we call peakNextNodeId() 3 times and it should return 3");
            Assert.AreEqual(3, engine.nextNodeId());
            Assert.AreEqual(4, engine.nextNodeId());
            Assert.AreEqual(5, engine.nextNodeId());
            int id = engine.nextNodeId();
            Assert.AreEqual(6, id);
            Console.WriteLine("if the test passes, the last id should be 6. it is " + id);
            engine.close();
        }
    }
}