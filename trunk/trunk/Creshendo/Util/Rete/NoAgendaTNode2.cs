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

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// NoAgendaTNode2 is similar to NoAgendaTNode with one difference. The rule
    /// may be deploy, but isn't effective until a given time. The terminal node
    /// will only create the activation if the current time is between the effective
    /// and expiration date of the rule.
    /// 
    /// </author>
    public class NoAgendaTNode2 : NoAgendaTNode
    {
        /// <param name="">id
        /// 
        /// </param>
        public NoAgendaTNode2(int id, Rule.IRule rl)
            : base(id, rl)
        {
            theRule = rl;
        }

        /// <param name="">facts
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void assertFacts(Index inx, Rete engine, IWorkingMemory mem)
        {
            long time = (DateTime.Now.Ticks - 621355968000000000)/10000;
            if (theRule.ExpirationDate > 0 && time > theRule.EffectiveDate && time < theRule.ExpirationDate)
            {
                LinkedActivation act = new LinkedActivation(theRule, inx);
                act.TerminalNode = this;
                // fire the activation immediately
                engine.fireActivation(act);
            }
        }
    }
}