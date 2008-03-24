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

namespace Creshendo.Util
{
    /// <author>  Peter Lin
    /// *
    /// ProfileStats is used to collect statistics about the runtime.
    /// 
    /// </author>
    public class ProfileStats
    {
        public static long addActivation = 0;
        public static int addcount = 0;
        protected internal static long addend = 0;
        protected internal static long addstart = 0;
        protected internal static long assertend = 0;
        protected internal static long assertstart = 0;
        public static long assertTime = 0;
        protected internal static long fend = 0;
        public static long fireTime = 0;

        protected internal static long fstart = 0;
        protected internal static long retractend = 0;
        protected internal static long retractstart = 0;
        public static long retractTime = 0;
        public static long rmActivation = 0;
        public static int rmcount = 0;

        protected internal static long rmend = 0;
        protected internal static long rmstart = 0;

        public ProfileStats() 
        {
        }

        public static void resetStats()
        {
            assertTime = 0;
            retractTime = 0;
            rmActivation = 0;
            addActivation = 0;
            fireTime = 0;
        }

        /// <summary> method should be called when Rete.fire is called or simply
        /// turn on profiling in Rete.
        /// </summary>
        public static void startFire()
        {
            fstart = (DateTime.Now.Ticks - 621355968000000000)/10000;
        }

        /// <summary> endFire will automatically calculate the elapsed time
        /// and Add it to the total fire time. if the start fire 
        /// timestamp is zero, the elapsed time will not be
        /// calculated.
        /// </summary>
        public static void endFire()
        {
            fend = (DateTime.Now.Ticks - 621355968000000000)/10000;
            if (fstart > 0)
            {
                addFireET(fend - fstart);
            }
        }

        /// <summary> Add a long time to the total fire time
        /// </summary>
        /// <param name="">time
        /// 
        /// </param>
        public static void addFireET(long time)
        {
            fireTime += time;
        }

        /// <summary> method should be called before assert is called or turn
        /// profiling in Rete.
        /// </summary>
        public static void startAssert()
        {
            assertstart = (DateTime.Now.Ticks - 621355968000000000)/10000;
        }

        /// <summary> method will automatically calculate the elapsed time and
        /// Add it to the total assert time. if the start assert
        /// timestamp is zero, elpased time will not be calculated
        /// and added.
        /// </summary>
        public static void endAssert()
        {
            assertend = (DateTime.Now.Ticks - 621355968000000000)/10000;
            if (assertstart > 0)
            {
                addAssertET(assertend - assertstart);
            }
        }

        /// <summary> Add elapsted time to assert total time
        /// </summary>
        /// <param name="">time
        /// 
        /// </param>
        public static void addAssertET(long time)
        {
            assertTime += time;
        }

        /// <summary> the method should be called before retract is called or
        /// turn of profiling in the Rete class.
        /// </summary>
        public static void startRetract()
        {
            retractstart = (DateTime.Now.Ticks - 621355968000000000)/10000;
        }

        /// <summary> method will calculate the elapsed time and Add it to the
        /// total retract time. if the start retract timestamp is zero
        /// the elapsed time will not be calculated.
        /// </summary>
        public static void endRetract()
        {
            retractend = (DateTime.Now.Ticks - 621355968000000000)/10000;
            if (retractstart > 0)
            {
                addRetractET(retractend - retractstart);
            }
        }

        /// <summary> Add elapsed time to retract total
        /// </summary>
        /// <param name="">time
        /// 
        /// </param>
        public static void addRetractET(long time)
        {
            retractTime += time;
        }

        public static void startAddActivation()
        {
            addstart = (DateTime.Now.Ticks - 621355968000000000)/10000;
        }

        public static void endAddActivation()
        {
            addend = (DateTime.Now.Ticks - 621355968000000000)/10000;
            if (addstart > 0)
            {
                addAddActivationET(addend - addstart);
                addcount++;
            }
        }

        public static void addAddActivationET(long time)
        {
            addActivation += time;
        }

        public static void startRemoveActivation()
        {
            rmstart = (DateTime.Now.Ticks - 621355968000000000)/10000;
        }

        public static void endRemoveActivation()
        {
            rmend = (DateTime.Now.Ticks - 621355968000000000)/10000;
            if (rmstart > 0)
            {
                addRemoveActivationET(rmend - rmstart);
                rmcount++;
            }
        }

        public static void addRemoveActivationET(long time)
        {
            rmActivation += time;
        }

        public static void reset()
        {
            assertTime = 0;
            retractTime = 0;
            rmActivation = 0;
            addActivation = 0;
            fireTime = 0;
            fstart = 0;
            fend = 0;
            assertstart = 0;
            assertend = 0;
            retractstart = 0;
            retractend = 0;
            addstart = 0;
            addend = 0;
            rmstart = 0;
            rmend = 0;
            addcount = 0;
            rmcount = 0;
        }
    }
}