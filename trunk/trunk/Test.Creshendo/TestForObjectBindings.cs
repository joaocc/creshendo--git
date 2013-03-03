using System;
using System.IO;
using System.Text;
using Test.Creshendo.Model;
using Creshendo.Util.Rete;
using Creshendo.Util.Rule;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Creshendo
{
    [TestClass]
    public class TestForObjectBindings : FileTestBase
    {
        private Account GetAcct1()
        {
            Account acc = new Account();
            acc.AccountId = "acc1";
            acc.AccountType = "standard";
            acc.First = "Bilbo";
            acc.Last = "Baggins";
            acc.Middle = "NMI";
            acc.OfficeCode = Convert.ToString(ran.Next(100000));
            acc.RegionCode = Convert.ToString(ran.Next(100000));
            acc.Status = "active";
            acc.Title = "mr";
            acc.Username = Convert.ToString(ran.Next(100000));
            acc.AreaCode = Convert.ToString(ran.Next(999));
            acc.Exchange = Convert.ToString(ran.Next(999));
            acc.Number = Convert.ToString(ran.Next(999));
            acc.Ext = Convert.ToString(ran.Next(9999));
            return acc;
        }

        private Account GetAcct0()
        {
            Account acc = new Account();
            acc.AccountId = "acc0";
            acc.AccountType = "standard";
            acc.First = "Floyd";
            acc.Last = "Rose";
            acc.Middle = "A";
            acc.OfficeCode = Convert.ToString(ran.Next(100000));
            acc.RegionCode = Convert.ToString(ran.Next(100000));
            acc.Status = "active";
            acc.Title = "mr";
            acc.Username = Convert.ToString(ran.Next(100000));
            acc.AreaCode = Convert.ToString(ran.Next(999));
            acc.Exchange = Convert.ToString(ran.Next(999));
            acc.Number = Convert.ToString(ran.Next(999));
            acc.Ext = Convert.ToString(ran.Next(9999));
            return acc;
        }

        private Stream getRule1()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("(defrule rule0");
            sb.AppendLine("(Account");
            sb.AppendLine("(AccountId ?accid)");
            sb.AppendLine("(AccountType ?acctype)");
            sb.AppendLine("(Title ?title)");
            sb.AppendLine("(Status ?stat)");
            sb.AppendLine(")");
            sb.AppendLine("=>");
            sb.AppendLine("(printout t \"rule0 was fired for \" ?accid crlf)");
            sb.AppendLine(")");
            return new MemoryStream(ASCIIEncoding.ASCII.GetBytes(sb.ToString()));
        }

        private Stream getRule2()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("(defrule rule2");
            sb.AppendLine("(Basic");
            sb.AppendLine("(This ?this)");
            sb.AppendLine("(That ?that)");
            sb.AppendLine(")");
            sb.AppendLine("=>");
            sb.AppendLine("(printout t \"rule2 was fired for \" ?this crlf)");
            sb.AppendLine(")");
            return new MemoryStream(ASCIIEncoding.ASCII.GetBytes(sb.ToString()));
        }

        private Stream getRule3()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("(defrule rule3 (Basic (This ?this))");
            sb.AppendLine("=>");
            sb.AppendLine("(printout t \"rule3 was fired.\" crlf)");
            sb.AppendLine(")");
            return new MemoryStream(ASCIIEncoding.ASCII.GetBytes(sb.ToString()));
        }

        [TestMethod]
        public void BasicObjectBindingTest1()
        {
            long ts = DateTime.Now.Ticks;
            int fired = 0;
            int activations = 0;
            Basic basic = new Basic("one", 1);
            using (TextWriter writer = Console.Out)
            {
                Rete engine = new Rete();
                engine.Watch = WatchType.WATCH_ALL;
                engine.addPrintWriter("Console", writer);
                engine.declareObject(typeof (Basic), "Basic");
                engine.loadRuleset(getRule3());

                foreach (Defrule rule in  engine.CurrentFocus.AllRules)
                {
                    Console.WriteLine(rule.toPPString());
                }

                engine.assertObject(basic, "Basic", false, false);
                activations = engine.CurrentFocus.ActivationCount;
                fired = engine.fire();
                engine.printWorkingMemory(true, false);
                writer.Flush();
                writer.Close();
                engine.close();
            }
            double endTime = DateTime.Now.Ticks - ts;
            Console.WriteLine(String.Format("BasicObjectBindingTest1 completed in {0} seconds.", (endTime/10000000).ToString("0.000000")));
            Assert.IsTrue(fired == 1);
            Assert.IsTrue(activations == 1);
            //AppDomain.Unload(AppDomain.CurrentDomain);
        }


        [TestMethod]
        public void ObjectBindingTest1()
        {
            long ts = DateTime.Now.Ticks;
            int fired = 0;
            int activations = 0;
            using (TextWriter writer = Console.Out)
            {
                Rete engine = new Rete();
                engine.Watch = WatchType.WATCH_ALL;
                engine.addPrintWriter("Console", writer);
                engine.declareObject(typeof (Account), "Account");
                engine.loadRuleset(getRule1());
                engine.assertObject(GetAcct0(), "Account", false, false);
                activations = engine.CurrentFocus.ActivationCount;
                fired = engine.fire();
                engine.printWorkingMemory(true, false);
                writer.Flush();
                writer.Close();
                engine.close();
            }
            double endTime = DateTime.Now.Ticks - ts;
            Console.WriteLine(String.Format("ObjectBindingTest1 completed in {0} seconds.", (endTime/10000000).ToString("0.000000")));
            Assert.IsTrue(fired == 1);
            Assert.IsTrue(activations == 1);
            //AppDomain.Unload(AppDomain.CurrentDomain);
        }

        [TestMethod]
        public void ObjectBindingTest2()
        {
            long ts = DateTime.Now.Ticks;
            int fired = 0;
            int activations = 0;
            using (TextWriter writer = Console.Out)
            {
                Rete engine = new Rete();
                engine.Watch = WatchType.WATCH_ALL;
                engine.addPrintWriter("Console", writer);
                engine.declareObject(typeof (Account), "Account");
                engine.loadRuleset(getRule1());
                engine.assertObject(GetAcct0(), "Account", false, false);

                int cnt = engine.ObjectCount;

                engine.assertObject(GetAcct1(), "Account", false, false);

                cnt = engine.ObjectCount;

                activations = engine.CurrentFocus.ActivationCount;
                fired = engine.fire();
                engine.printWorkingMemory(true, false);
                writer.Flush();
                writer.Close();
                engine.close();
            }
            double endTime = DateTime.Now.Ticks - ts;
            Console.WriteLine(String.Format("ObjectBindingTest2 completed in {0} seconds.", (endTime/10000000).ToString("0.000000")));
            Assert.IsTrue(fired == 2);
            Assert.IsTrue(activations == 2);
            //AppDomain.Unload(AppDomain.CurrentDomain);
        }
    }
}