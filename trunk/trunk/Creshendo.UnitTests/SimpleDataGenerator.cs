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
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Creshendo.UnitTests
{
    [TestFixture]
    public class SimpleDataGenerator : FileTestBase
    {
        // 10 country codes
        public static String[] countries = {
                                               "US", "BR", "FR", "NZ", "CA", "MX",
                                               "CH", "TW", "NU", "IT"
                                           };

        public static String[] cusips = {
                                            "576335338",
                                            "847737565",
                                            "584420736",
                                            "776465086",
                                            "280242230",
                                            "334158152"
                                        };

        public static String[] exchange = {
                                              "NYSE", "NSDQ", "LNSE", "TKYO",
                                              "TWSE", "PSEX", "RMSE"
                                          };

        // 9 gics codes
        public static String[] gics = {
                                          "25201010", "25201020", "25201030",
                                          "25201040", "25201050", "25301010", "25301020",
                                          "25301030", "25301040"
                                      };

        // 8 issuers
        public static String[] issuers = {
                                             "AAA", "BBB", "CCC", "DDD",
                                             "EEE", "FFF", "GGG", "HHH"
                                         };

        public static String LINEBREAK = Environment.NewLine;

        public String fileName = null;

        // 7 exchanges

        private StreamWriter wtr = null;


        public void setFilename(String name)
        {
            fileName = getRoot(name, true);
        }

        public void generate(int count)
        {
            try
            {
                wtr = new StreamWriter(fileName, false);
                createDeffacts(count);
                wtr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void createDeffacts(int count)
        {
            for (int idx = 0; idx < count; idx++)
            {
                String country = countries[ran.Next(countries.Length)];
                String gicsCode = gics[ran.Next(gics.Length - 1)];
                String iss = issuers[ran.Next(issuers.Length - 1)];
                String ex = exchange[ran.Next(exchange.Length - 1)];
                String csip = cusips[ran.Next(cusips.Length - 1)];
                StringBuilder buf = new StringBuilder();
                buf.Append("(assert (transaction");
                buf.Append(" (accountId \"" + idx + "id\")");
                buf.Append(" (buyPrice 55.23)");
                buf.Append(" (countryCode \"" + country + "\")");
                buf.Append(" (currentPrice 58.95)");
                buf.Append(" (cusip " + csip + ")");
                buf.Append(" (exchange \"" + ex + "\")");
                buf.Append(" (industryGroupID " + gicsCode.Substring(0, 4) + ")");
                buf.Append(" (industryID " + gicsCode.Substring(0, 6) + ")");
                buf.Append(" (issuer \"" + iss + "\")");
                buf.Append(" (lastPrice 50.12)");
                buf.Append(" (sectorID " + gicsCode.Substring(0, 2) + ")");
                buf.Append(" (shares 100)");
                buf.Append(" (subIndustryID " + gicsCode + ")");
                buf.Append(" ) )" + LINEBREAK);
                try
                {
                    wtr.Write(buf.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        [Test]
        public void mainTest()
        {
            string file = "generated_file.clp";

            int count = 5;
            SimpleDataGenerator gen = new SimpleDataGenerator();
            gen.setFilename(file);
            gen.generate(count);
            Console.WriteLine("done!!");
        }
    }
}