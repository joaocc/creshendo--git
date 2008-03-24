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
    /// 
    /// Terminal node indicates the rule has matched fully and should execute the
    /// action of the rule. NOTE: currently this is not used directly. other terminal
    /// nodes extend it.
    /// 
    /// </author>
    public abstract class TerminalNode : BaseNode
    {
        protected internal Rule.IRule theRule = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalNode"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="rl">The rl.</param>
        public TerminalNode(int id, Rule.IRule rl)
            : base(id)
        {
            theRule = rl;
        }

        /// <summary>
        /// Gets the rule.
        /// </summary>
        /// <value>The rule.</value>
        public virtual Rule.IRule Rule
        {
            get { return theRule; }
        }

        /// <summary>
        /// The terminal nodes doesn't have a memory, so the method does nothing.
        /// </summary>
        /// <param name="mem"></param>
        public override void clear(IWorkingMemory mem)
        {
        }

        /// <summary>
        /// Once the facts propogate to this point, it means all the conditions of
        /// the rule have been met. The method creates a new Activation and adds it
        /// to the activationList of the correct module. Note: we may want to change
        /// the design so that we don't create a new Activation object.
        /// </summary>
        /// <param name="facts">The facts.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public abstract void assertFacts(Index facts, Rete engine, IWorkingMemory mem);

        /// <summary>
        /// Retract means we need to Remove the activation from the correct module
        /// agenda.
        /// </summary>
        /// <param name="facts">The facts.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public abstract void retractFacts(Index facts, Rete engine, IWorkingMemory mem);


        /// <summary>
        /// return the name of the rule
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return theRule.Name;
        }

        /// <summary>
        /// return the name of the rule
        /// </summary>
        /// <returns></returns>
        public override String toPPString()
        {
            return theRule.Name;
        }

        /// <summary> The terminal node has no successors, so this method does nothing.
        /// </summary>
        public override void removeAllSuccessors()
        {
        }
    }
}