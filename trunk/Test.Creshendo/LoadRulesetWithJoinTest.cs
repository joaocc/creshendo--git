using System;
using System.Collections;
using System.Collections.Generic;
using Creshendo.Util.Rete;
using Creshendo.Util.Rule;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Creshendo
{
    [TestClass]
    public class LoadRulesetWithJoinTest : FileTestBase
    {
        [TestMethod]
        public void testLoadJoinSample13()
        {
            Rete engine = new Rete();
            engine.loadRuleset(getRoot("join_sample13.clp"));
            ICollection<object> rules = engine.CurrentFocus.AllRules;
            int count = rules.Count;
            IEnumerator itr = rules.GetEnumerator();
            while (itr.MoveNext())
            {
                Defrule r = (Defrule) itr.Current;
                Console.WriteLine(r.toPPString());
            }
            Assert.AreEqual(3, count);
            engine.close();
        }
    }
}