/*
 * Created on Aug 29, 2006
 *
 * TODO To change the template for this generated file go to
 * Window - Preferences - Java - Code Style - Code Templates
 */
using System;
using System.Collections;
using System.Collections.Generic;
using Creshendo.Util.Rete;
using Creshendo.Util.Rule;
using NUnit.Framework;

namespace Creshendo.UnitTests
{
    [TestFixture]
    public class LoadRulesetTest : FileTestBase
    {
        [Test]
        public void testLoadTest()
        {
            Rete engine = new Rete();
            engine.loadRuleset(getRoot("test.clp"));
            ICollection<object> rules = engine.CurrentFocus.AllRules;
            int count = rules.Count;
            IEnumerator itr = rules.GetEnumerator();
            while (itr.MoveNext())
            {
                Defrule r = (Defrule) itr.Current;
                Console.WriteLine(r.toPPString());
            }
            Assert.AreEqual(5, count);
            engine.close();
        }
    }
}