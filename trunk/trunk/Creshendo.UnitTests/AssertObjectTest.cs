using System;
using Creshendo.UnitTests.Model;
using Creshendo.Util.Rete;
using Creshendo.Util.Rete.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Creshendo.UnitTests
{
    [TestClass]
    public class AssertObjectTest
    {
        [TestMethod]
        public void testAssertTwoObjects()
        {
            Console.WriteLine("start testAssertTwoObjects");
            Rete engine = new Rete();
            engine.declareObject(typeof (Account));
            engine.declareObject(typeof (TestBean3));
            Assert.IsNotNull(engine);
            // create instance of Account
            Account acc1 = new Account();
            acc1.AccountId = "1234";
            acc1.AccountType = "new";
            acc1.First = "fName";
            acc1.Last = "lName";
            acc1.Middle = "m";
            acc1.OfficeCode = "MA";
            acc1.RegionCode = "NE";
            acc1.Status = "active";
            acc1.Title = "MR";
            acc1.Username = "user1";
            // create instance of TestBean3
            TestBean3 b = new TestBean3();
            b.Count = 3;
            b.Float = 10000;
            try
            {
                long start = DateTime.Now.Ticks;
                engine.assertObject(acc1, null, false, true);
                int count = engine.ObjectCount;
                engine.assertObject(b, null, false, true);
                int count2 = engine.ObjectCount;
                long end = DateTime.Now.Ticks;
                int fired = engine.fire();
                Assert.IsTrue(true);
                Assert.IsTrue(true);
                Assert.AreEqual(1, count);
                Assert.AreEqual(2, count2);
                Console.WriteLine("Number of facts: " + count);
                Console.WriteLine("Number of facts: " + count2);
                Console.WriteLine("ET: " + (end - start) + " ns");
                double el = ((double) end - (double) start)/100000;
                Console.WriteLine("ET: " + el + " ms");
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [TestMethod]
        public void testAssertWithInterface()
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("start testAssertWithInterface");
            Rete engine = new Rete();
            engine.declareObject(typeof (IAccount), "account");
            Assert.IsNotNull(engine);
            Account acc1 = new Account();
            acc1.AccountId = "1234";
            acc1.AccountType = "new";
            acc1.First = "fName";
            acc1.Last = "lName";
            acc1.Middle = "m";
            acc1.OfficeCode = "MA";
            acc1.RegionCode = "NE";
            acc1.Status = "active";
            acc1.Title = "MR";
            acc1.Username = "user1";
            try
            {
                engine.assertObject(acc1, "account", false, true);
                Assert.IsTrue(true);
                Assert.AreEqual(1, engine.ObjectCount);
                Console.WriteLine("Number of facts: " + engine.ObjectCount);
                engine.printWorkingMemory(true, true);
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }
            engine.close();
        }

        [TestMethod]
        public void testAssertWithSubclass()
        {
            Console.WriteLine("start testAssertWithSubclass");
            Rete engine = new Rete();
            engine.declareObject(typeof (IAccount), "account");
            Assert.IsNotNull(engine);
            BackupAccount acc1 = new BackupAccount();
            acc1.AccountId = "1234";
            acc1.AccountType = "new";
            acc1.First = "fName";
            acc1.Last = "lName";
            acc1.Middle = "m";
            acc1.OfficeCode = "MA";
            acc1.RegionCode = "NE";
            acc1.Status = "active";
            acc1.Title = "MR";
            acc1.Username = "user1";
            try
            {
                engine.assertObject(acc1, "account", false, true);
                Assert.IsTrue(true);
                Assert.AreEqual(1, engine.ObjectCount);
                Console.WriteLine("Number of facts: " + engine.ObjectCount);
                engine.printWorkingMemory(true, true);
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }
            engine.close();
        }

        [TestMethod]
        public void testAssertWithSubclass2()
        {
            Console.WriteLine("\nstart testAssertWithSubclass2");
            Rete engine = new Rete();
            engine.declareObject(typeof (IAccount), "account");
            engine.declareObject(typeof (BackupAccount), "backupAccount");
            Assert.IsNotNull(engine);
            BackupAccount acc1 = new BackupAccount();
            acc1.AccountId = "1234";
            acc1.AccountType = "new";
            acc1.First = "fName";
            acc1.Last = "lName";
            acc1.Middle = "m";
            acc1.OfficeCode = "MA";
            acc1.RegionCode = "NE";
            acc1.Status = "active";
            acc1.Title = "MR";
            acc1.Username = "user1";
            try
            {
                engine.assertObject(acc1, "backupAccount", false, true);
                Assert.IsTrue(true);
                Assert.AreEqual(1, engine.ObjectCount);
                Console.WriteLine("Number of facts: " + engine.ObjectCount);
                engine.printWorkingMemory(true, true);
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }
            engine.close();
        }

        [TestMethod]
        public void testAssertWithSubclass3()
        {
            Console.WriteLine("\nstart testAssertWithSubclass3");
            Rete engine = new Rete();
            engine.declareObject(typeof (IAccount), "account");
            engine.declareObject(typeof (BackupAccount), "backupAccount");
            Assert.IsNotNull(engine);
            BackupAccount acc1 = new BackupAccount();
            acc1.AccountId = "1234";
            acc1.AccountType = "new";
            acc1.First = "fName";
            acc1.Last = "lName";
            acc1.Middle = "m";
            acc1.OfficeCode = "MA";
            acc1.RegionCode = "NE";
            acc1.Status = "active";
            acc1.Title = "MR";
            acc1.Username = "user1";
            try
            {
                engine.assertObject(acc1, "account", false, true);
                Assert.IsTrue(true);
                Assert.AreEqual(1, engine.ObjectCount);
                Console.WriteLine("Number of facts: " + engine.ObjectCount);
                engine.printWorkingMemory(true, true);
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }
            engine.close();
        }

        [TestMethod]
        public void testAssertWithSubclassWithParent()
        {
            Console.WriteLine("\nstart testAssertWithSubclassWithParent");
            Rete engine = new Rete();
            engine.declareObject(typeof (IAccount), "account");
            engine.declareObject(typeof (BackupAccount), "backupAccount", "account");
            Assert.IsNotNull(engine);
            BackupAccount acc1 = new BackupAccount();
            acc1.AccountId = "1234";
            acc1.AccountType = "new";
            acc1.First = "fName";
            acc1.Last = "lName";
            acc1.Middle = "m";
            acc1.OfficeCode = "MA";
            acc1.RegionCode = "NE";
            acc1.Status = "active";
            acc1.Title = "MR";
            acc1.Username = "user1";
            try
            {
                engine.assertObject(acc1, "backupAccount", false, true);
                Assert.IsTrue(true);
                Assert.AreEqual(1, engine.ObjectCount);
                Console.WriteLine("Number of facts: " + engine.ObjectCount);
                engine.printWorkingMemory(true, true);
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }
            engine.close();
        }

        [TestMethod]
        public void testAssertWithSubclassWithParent2()
        {
            Console.WriteLine("\nstart testAssertWithSubclassWithParent2");
            Rete engine = new Rete();
            engine.declareObject(typeof (IAccount), "account");
            engine.declareObject(typeof (BackupAccount), null, "account");
            Assert.IsNotNull(engine);
            BackupAccount acc1 = new BackupAccount();
            acc1.AccountId = "1234";
            acc1.AccountType = "new";
            acc1.First = "fName";
            acc1.Last = "lName";
            acc1.Middle = "m";
            acc1.OfficeCode = "MA";
            acc1.RegionCode = "NE";
            acc1.Status = "active";
            acc1.Title = "MR";
            acc1.Username = "user1";
            try
            {
                engine.assertObject(acc1, null, false, true);
                Assert.IsTrue(true);
                Assert.AreEqual(1, engine.ObjectCount);
                Console.WriteLine("Number of facts: " + engine.ObjectCount);
                engine.printWorkingMemory(true, true);
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }
            engine.close();
        }

        [TestMethod]
        public void testAssertWithSubclassWithParent3()
        {
            Console.WriteLine("\nstart testAssertWithSubclassWithParent3");
            Rete engine = new Rete();
            engine.declareObject(typeof (IAccount), "account");
            engine.declareObject(typeof (BackupAccount), null, "account");
            engine.declareObject(typeof (DeletedAccount), null, typeof (BackupAccount));
            Assert.IsNotNull(engine);
            DeletedAccount acc1 = new DeletedAccount();
            acc1.AccountId = "1234";
            acc1.AccountType = "new";
            acc1.First = "fName";
            acc1.Last = "lName";
            acc1.Middle = "m";
            acc1.OfficeCode = "MA";
            acc1.RegionCode = "NE";
            acc1.Status = "active";
            acc1.Title = "MR";
            acc1.Username = "user1";
            try
            {
                engine.assertObject(acc1, null, false, true);
                Assert.IsTrue(true);
                Assert.AreEqual(1, engine.ObjectCount);
                Console.WriteLine("Number of facts: " + engine.ObjectCount);
                engine.printWorkingMemory(true, true);
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }
            engine.close();
        }

        [TestMethod]
        public void testRepeatedAssert()
        {
            Console.WriteLine("start testRepatedAssert");
            Rete engine = new Rete();
            engine.declareObject(typeof (Account));
            engine.declareObject(typeof (TestBean3));
            Assert.IsNotNull(engine);
            // create instance of Account
            Account acc1 = new Account();
            acc1.AccountId = "1234";
            acc1.AccountType = "new";
            acc1.First = "fName";
            acc1.Last = "lName";
            acc1.Middle = "m";
            acc1.OfficeCode = "MA";
            acc1.RegionCode = "NE";
            acc1.Status = "active";
            acc1.Title = "MR";
            acc1.Username = "user1";
            // create instance of TestBean3
            TestBean3 b = new TestBean3();
            b.Count = 3;
            b.Float = 10000;
            try
            {
                long start = DateTime.Now.Ticks;
                for (int idx = 0; idx < 100; idx++)
                {
                    engine.assertObject(acc1, null, false, true);
                }
                int count = engine.ObjectCount;
                for (int idx = 0; idx < 100; idx++)
                {
                    engine.assertObject(b, null, false, true);
                }
                int count2 = engine.ObjectCount;
                long end = DateTime.Now.Ticks;

                Assert.IsTrue(true);
                Assert.IsTrue(true);
                Assert.AreEqual(1, count);
                Assert.AreEqual(2, count2);
                Console.WriteLine("Number of facts: " + count);
                Console.WriteLine("Number of facts: " + count2);
                Console.WriteLine("ET: " + (end - start) + " ns");
                double el = ((double) end - (double) start)/100000;
                Console.WriteLine("ET: " + el + " ms");
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }
            engine.close();
        }

        [TestMethod]
        public void testSimpleAssert()
        {
            Console.WriteLine("start testSimpleAssert");
            Rete engine = new Rete();
            engine.declareObject(typeof (Account));
            Assert.IsNotNull(engine);
            Account acc1 = new Account();
            acc1.AccountId = "1234";
            acc1.AccountType = "new";
            acc1.First = "fName";
            acc1.Last = "lName";
            acc1.Middle = "m";
            acc1.OfficeCode = "MA";
            acc1.RegionCode = "NE";
            acc1.Status = "active";
            acc1.Title = "MR";
            acc1.Username = "user1";
            try
            {
                engine.assertObject(acc1, null, false, true);
                Assert.IsTrue(true);
                Assert.AreEqual(1, engine.ObjectCount);
                Console.WriteLine("Number of facts: " + engine.Defclasses.Count);
            }
            catch (AssertException e)
            {
                Console.WriteLine(e.Message);
            }
            engine.close();
        }
    }
}