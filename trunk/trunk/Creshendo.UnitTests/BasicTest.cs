using System;
using Creshendo.UnitTests.Model;
using Creshendo.Util.Rete;
using NUnit.Framework;

namespace Creshendo.UnitTests
{
    [TestFixture]
    public class BasicTest
    {
        [Test]
        public void First()
        {
        }

        [Test]
        public void testOneSlot()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            TestBean2 bean = new TestBean2();
            Slot[] slts = dtemp.AllSlots;
            ObjectTypeNode otn = new ObjectTypeNode(1, dtemp);
            AlphaNode an = new AlphaNode(1);
            slts[0].Value = ConversionUtils.convert(110);
            an.Operator = Constants.EQUAL;
            an.Slot = (slts[0]);
            Console.WriteLine("node::" + an.ToString());
            Assert.IsNotNull(an.ToString(), "Should have a value.");
        }

        [Test]
        public void testTwoSlots()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            TestBean2 bean = new TestBean2();
            Slot[] slts = dtemp.AllSlots;
            ObjectTypeNode otn = new ObjectTypeNode(1, dtemp);
            AlphaNode an1 = new AlphaNode(1);
            AlphaNode an2 = new AlphaNode(1);

            slts[0].Value = ("testString");
            slts[1].Value = (ConversionUtils.convert(999));

            an1.Slot = (slts[0]);
            an1.Operator = (Constants.EQUAL);
            Console.WriteLine("node::" + an1.toPPString());
            Assert.IsNotNull(an1.toPPString());

            an2.Slot = (slts[1]);
            an2.Operator = (Constants.GREATER);
            Console.WriteLine("node::" + an2.toPPString());
            Assert.IsNotNull(an2.toPPString());
        }
    }
}