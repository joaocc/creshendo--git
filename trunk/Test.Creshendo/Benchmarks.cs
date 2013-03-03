using System;
using System.IO;
using Creshendo.Util.Rete;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Creshendo
{
    [TestClass]
    public class Benchmarks : FileTestBase
    {
        /*
        [TestMethod]
        public void manners128()
        {
            long ts = DateTime.Now.Ticks;
            using (TextWriter writer = Console.Out)
            {
                Rete engine = new Rete();
                engine.addPrintWriter("Console", writer);
                engine.loadRuleset(getRoot("manners128guestsd.clp"));
                engine.printWorkingMemory(false, false);
                writer.Flush();
                writer.Close();
                engine.MessageRouter.ShutDown();
            }
            double endTime = ts.TotalMilliseconds;
            System.Console.WriteLine(String.Format("Manners 128 completed in {0} seconds.", (endTime / 10000000000000).ToString("0.000000")));
        }
        */

        [TestMethod]
        public void manners16()
        {
            long ts = DateTime.Now.Ticks;
            using (TextWriter writer = Console.Out)
            {
                Rete engine = new Rete();
                engine.addPrintWriter("Console", writer);
                engine.loadRuleset(getRoot("manners16guestsd.clp"));
                engine.printWorkingMemory(false, false);
                writer.Flush();
                writer.Close();
                engine.close();
                //engine.MessageRouter.ShutDown();
            }
            double endTime = DateTime.Now.Ticks - ts;
            Console.WriteLine(String.Format("Manners 16 completed in {0} seconds.", (endTime/10000000).ToString("0.000000")));
            //AppDomain.Unload(AppDomain.CurrentDomain);
        }

        [TestMethod]
        public void manners64()
        {
            long ts = DateTime.Now.Ticks;
            using (TextWriter writer = Console.Out)
            {
                Rete engine = new Rete();
                engine.addPrintWriter("Console", writer);
                engine.loadRuleset(getRoot("manners64guests.clp"));
                engine.printWorkingMemory(false, false);
                writer.Flush();
                writer.Close();
                //engine.MessageRouter.ShutDown();
                engine.close();
            }
            double endTime = DateTime.Now.Ticks - ts;
            Console.WriteLine(String.Format("Manners 64 completed in {0} seconds.", (endTime/10000000).ToString("0.000000")));
            //AppDomain.Unload(AppDomain.CurrentDomain);
        }
    }
}