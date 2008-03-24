/*
 * Copyright 2002-2006 Peter Lin & RuleML.
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
 
namespace woolfel.rete{

import java.util.ArrayList;
import java.util.Random;

import org.jamocha.rete.Rete;

import woolfel.examples.model.Account4;
import woolfel.examples.model.Transaction;
import junit.framework.TestCase;
*/
/**
 * @author Peter Lin
 *
 * SimpleJoin test is used to measure basic join performance for very
 * simple cases.
 */
using System;
using System.Collections;
using Creshendo.UnitTests.Model;
using Creshendo.Util.Rete;
using NUnit.Framework;

namespace Creshendo.UnitTests
{
    [TestFixture]
    public class SimpleJoinTest
    {
        [Test]
        public void testFiveRules()
        {
            int objCount = 25000;
            Random ran = new Random();
            ArrayList facts = new ArrayList();
            // loop and create account and transaction objects
            for (int idx = 0; idx < objCount; idx++)
            {
                Account4 acc = new Account4();
                acc.AccountId = "acc" + idx;
                acc.AccountType = Convert.ToString(ran.Next(100000));
                acc.First = Convert.ToString(ran.Next(100000));
                acc.Last = Convert.ToString(ran.Next(100000));
                acc.Middle = Convert.ToString(ran.Next(100000));
                acc.Status = Convert.ToString(ran.Next(100000));
                acc.Title = Convert.ToString(ran.Next(100000));
                acc.Username = Convert.ToString(ran.Next(100000));
                acc.CountryCode = "US";
                acc.Cash = 1298.00;
                facts.Add(acc);
                Transaction tx = new Transaction();
                tx.AccountId = "acc" + idx;
                tx.Total = 1200000;
                facts.Add(tx);
            }
            Console.WriteLine("created " + objCount + " Accounts and Transactions");
            Rete engine = new Rete();
            engine.declareObject(typeof (Account4));
            engine.declareObject(typeof (Transaction));
            Console.WriteLine("delcare the objects");
            engine.close();
        }
    }
}