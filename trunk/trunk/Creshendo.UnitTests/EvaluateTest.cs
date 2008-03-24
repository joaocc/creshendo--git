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
    public class EvaluateTest
    {
        [Test]
        public void testBoolean1()
        {
            Boolean t = true;
            String f = "false";
            String t2 = "true";
            Boolean f2 = false;
            Assert.IsFalse(Evaluate.evaluateEqual(t, f));
            Assert.IsFalse(Evaluate.evaluateEqual(t2, f2));
            Assert.IsTrue(Evaluate.evaluateEqual(t, t2));
            Assert.IsTrue(Evaluate.evaluateEqual(f, f2));

            String t3 = "TRUE";
            String f3 = "FALSE";

            //I will probably burn in hell, but...

            Assert.IsTrue(Evaluate.evaluateEqual(t, t3));
            Assert.IsTrue(Evaluate.evaluateEqual(f2, f3));

            //Assert.IsFalse(Evaluate.evaluateEqual(t, t3));
            //Assert.IsFalse(Evaluate.evaluateEqual(f2, f3));
        }

        [Test]
        public void testLess1()
        {
            Int32 int1 = Convert.ToInt32(1);
            Int32 int2 = Convert.ToInt32(2);
            Console.WriteLine("---- evaluate int to int");
            Assert.AreEqual(true, Evaluate.evaluateLess(int1, int2));
            Console.WriteLine(int1 + " < " + int2 + " is " +
                              Evaluate.evaluateLess(int1, int2));

            Assert.AreEqual(false, Evaluate.evaluateLess(int2, int1));
            Console.WriteLine(int2 + " < " + int1 + " is " +
                              Evaluate.evaluateLess(int2, int1));

            Assert.AreEqual(false, Evaluate.evaluateLess(int1, int1));
            Console.WriteLine(int1 + " < " + int1 + " is " +
                              Evaluate.evaluateLess(int1, int1));

            // ----- now test using short ----- //
            short sh1 = Convert.ToInt16("1");
            short sh2 = Convert.ToInt16("2");
            Console.WriteLine("---- evaluate short to short");
            Assert.AreEqual(true, Evaluate.evaluateLess(sh1, sh2));
            Console.WriteLine(sh1 + " < " + sh2 + " is " +
                              Evaluate.evaluateLess(sh1, sh2));

            Assert.AreEqual(false, Evaluate.evaluateLess(sh2, sh1));
            Console.WriteLine(sh2 + " < " + sh1 + " is " +
                              Evaluate.evaluateLess(sh2, sh1));
            Assert.AreEqual(false, Evaluate.evaluateLess(sh1, sh1));
            Console.WriteLine(sh1 + " < " + sh1 + " is " +
                              Evaluate.evaluateLess(sh1, sh1));

            // ----- now test using long ----- //
            long ln1 = Convert.ToInt64(10001);
            long ln2 = Convert.ToInt64(10010);
            Console.WriteLine("---- evaluate long to long");
            Assert.AreEqual(true, Evaluate.evaluateLess(ln1, ln2));
            Console.WriteLine(ln1 + " < " + ln2 + " is " +
                              Evaluate.evaluateLess(ln1, ln2));

            Assert.AreEqual(false, Evaluate.evaluateLess(ln2, ln1));
            Console.WriteLine(ln2 + " < " + ln1 + " is " +
                              Evaluate.evaluateLess(ln2, ln1));
            Assert.AreEqual(false, Evaluate.evaluateLess(ln1, ln1));
            Console.WriteLine(ln1 + " < " + ln1 + " is " +
                              Evaluate.evaluateLess(ln1, ln1));

            // ----- now test using float ----- //
            float fl1 = 10001.01f;
            float fl2 = 10002.01f;
            Console.WriteLine("---- evaluate float to float");
            Assert.AreEqual(true, Evaluate.evaluateLess(fl1, fl2));
            Console.WriteLine(fl1 + " < " + fl2 + " is " +
                              Evaluate.evaluateLess(fl1, fl2));

            Assert.AreEqual(false, Evaluate.evaluateLess(fl2, fl1));
            Console.WriteLine(fl2 + " < " + fl1 + " is " +
                              Evaluate.evaluateLess(fl2, fl1));

            Assert.AreEqual(false, Evaluate.evaluateLess(fl1, fl1));
            Console.WriteLine(fl1 + " < " + fl1 + " is " +
                              Evaluate.evaluateLess(fl1, fl1));

            // ----- now test using Double ----- //
            Double db1 = Convert.ToDouble(1000.00);
            Double db2 = Convert.ToDouble(2000.00);
            Console.WriteLine("---- evaluate double to double");
            Assert.AreEqual(true, Evaluate.evaluateLess(db1, db2));
            Console.WriteLine(db1 + " < " + db2 + " is " +
                              Evaluate.evaluateLess(db1, db2));

            Assert.AreEqual(false, Evaluate.evaluateLess(db2, db1));
            Console.WriteLine(db2 + " < " + db1 + " is " +
                              Evaluate.evaluateLess(db2, db1));

            Assert.AreEqual(false, Evaluate.evaluateLess(db1, db1));
            Console.WriteLine(db1 + " < " + db1 + " is " +
                              Evaluate.evaluateLess(db1, db1));
        }

        [Test]
        public void testLessEqual1()
        {
            Int32 int1 = Convert.ToInt32(1);
            Int32 int2 = Convert.ToInt32(2);
            Console.WriteLine("---- evaluate int to int");
            Assert.AreEqual(true, Evaluate.evaluateLessEqual(int1, int2));
            Console.WriteLine(int1 + " <= " + int2 + " is " +
                              Evaluate.evaluateLessEqual(int1, int2));

            Assert.AreEqual(false, Evaluate.evaluateLessEqual(int2, int1));
            Console.WriteLine(int2 + " <= " + int1 + " is " +
                              Evaluate.evaluateLessEqual(int2, int1));

            Assert.AreEqual(true, Evaluate.evaluateLessEqual(int1, int1));
            Console.WriteLine(int1 + " <= " + int1 + " is " +
                              Evaluate.evaluateLessEqual(int1, int1));

            // ----- now test using short ----- //
            short sh1 = Convert.ToInt16("1");
            short sh2 = Convert.ToInt16("2");
            Console.WriteLine("---- evaluate short to short");
            Assert.AreEqual(true, Evaluate.evaluateLessEqual(sh1, sh2));
            Console.WriteLine(sh1 + " <= " + sh2 + " is " +
                              Evaluate.evaluateLessEqual(sh1, sh2));

            Assert.AreEqual(false, Evaluate.evaluateLessEqual(sh2, sh1));
            Console.WriteLine(sh2 + " <= " + sh1 + " is " +
                              Evaluate.evaluateLessEqual(sh2, sh1));
            Assert.AreEqual(true, Evaluate.evaluateLessEqual(sh1, sh1));
            Console.WriteLine(sh1 + " <= " + sh1 + " is " +
                              Evaluate.evaluateLessEqual(sh1, sh1));

            // ----- now test using long ----- //
            long ln1 = Convert.ToInt64(10001);
            long ln2 = Convert.ToInt64(10010);
            Console.WriteLine("---- evaluate long to long");
            Assert.AreEqual(true, Evaluate.evaluateLessEqual(ln1, ln2));
            Console.WriteLine(ln1 + " <= " + ln2 + " is " +
                              Evaluate.evaluateLessEqual(ln1, ln2));

            Assert.AreEqual(false, Evaluate.evaluateLessEqual(ln2, ln1));
            Console.WriteLine(ln2 + " <= " + ln1 + " is " +
                              Evaluate.evaluateLessEqual(ln2, ln1));
            Assert.AreEqual(true, Evaluate.evaluateLessEqual(ln1, ln1));
            Console.WriteLine(ln1 + " <= " + ln1 + " is " +
                              Evaluate.evaluateLessEqual(ln1, ln1));

            // ----- now test using float ----- //
            float fl1 = 10001.01f;
            float fl2 = 10002.01f;
            Console.WriteLine("---- evaluate float to float");
            Assert.AreEqual(true, Evaluate.evaluateLessEqual(fl1, fl2));
            Console.WriteLine(fl1 + " <= " + fl2 + " is " +
                              Evaluate.evaluateLessEqual(fl1, fl2));

            Assert.AreEqual(false, Evaluate.evaluateLessEqual(fl2, fl1));
            Console.WriteLine(fl2 + " <= " + fl1 + " is " +
                              Evaluate.evaluateLessEqual(fl2, fl1));

            Assert.AreEqual(true, Evaluate.evaluateLessEqual(fl1, fl1));
            Console.WriteLine(fl1 + " <= " + fl1 + " is " +
                              Evaluate.evaluateLessEqual(fl1, fl1));

            // ----- now test using Double ----- //
            Double db1 = Convert.ToDouble(1000.00);
            Double db2 = Convert.ToDouble(2000.00);
            Console.WriteLine("---- evaluate double to double");
            Assert.AreEqual(true, Evaluate.evaluateLessEqual(db1, db2));
            Console.WriteLine(db1 + " <= " + db2 + " is " +
                              Evaluate.evaluateLessEqual(db1, db2));

            Assert.AreEqual(false, Evaluate.evaluateLessEqual(db2, db1));
            Console.WriteLine(db2 + " <= " + db1 + " is " +
                              Evaluate.evaluateLessEqual(db2, db1));

            Assert.AreEqual(true, Evaluate.evaluateLessEqual(db1, db1));
            Console.WriteLine(db1 + " <= " + db1 + " is " +
                              Evaluate.evaluateLessEqual(db1, db1));
        }

        /**
         * the method will test comparing different numeric types to make sure
         * it all works correctly
         */

        [Test]
        public void testLessEqual2()
        {
            Console.WriteLine("testLessEqual2 -------");
            Int32 int1 = Convert.ToInt32(1);

            short sh1 = Convert.ToInt16("2");
            Console.WriteLine("---- evaluate int to short");
            Assert.AreEqual(true, Evaluate.evaluateLessEqual(int1, sh1));
            Console.WriteLine(int1 + " <= " + sh1 + " is " +
                              Evaluate.evaluateLessEqual(int1, sh1));

            Assert.AreEqual(false, Evaluate.evaluateLessEqual(sh1, int1));
            Console.WriteLine(sh1 + " <= " + int1 + " is " +
                              Evaluate.evaluateLessEqual(sh1, int1));

            long ln1 = Convert.ToInt64(2);
            Console.WriteLine("---- evaluate int to long");
            Assert.AreEqual(true, Evaluate.evaluateLessEqual(int1, ln1));
            Console.WriteLine(int1 + " <= " + ln1 + " is " +
                              Evaluate.evaluateLessEqual(int1, ln1));

            Assert.AreEqual(false, Evaluate.evaluateLessEqual(ln1, int1));
            Console.WriteLine(ln1 + " <= " + int1 + " is " +
                              Evaluate.evaluateLessEqual(ln1, int1));

            // ----- now test using float ----- //
            float fl1 = 10001f;
            Console.WriteLine("---- evaluate int to float");
            Assert.AreEqual(true, Evaluate.evaluateLessEqual(int1, fl1));
            Console.WriteLine(int1 + " <= " + fl1 + " is " +
                              Evaluate.evaluateLessEqual(ln1, fl1));

            Assert.AreEqual(false, Evaluate.evaluateLessEqual(fl1, int1));
            Console.WriteLine(fl1 + " <= " + int1 + " is " +
                              Evaluate.evaluateLessEqual(fl1, int1));

            // ----- now test using Double ----- //
            Double db1 = Convert.ToDouble(1000.00);
            Console.WriteLine("---- evaluate int to double");
            Assert.AreEqual(true, Evaluate.evaluateLessEqual(int1, db1));
            Console.WriteLine(int1 + " <= " + db1 + " is " +
                              Evaluate.evaluateLessEqual(db1, db1));

            Assert.AreEqual(false, Evaluate.evaluateLessEqual(db1, int1));
            Console.WriteLine(db1 + " <= " + int1 + " is " +
                              Evaluate.evaluateLessEqual(db1, int1));
        }
    }
}