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
using System.Text;
using Creshendo.Util.Rete.Exception;
using Creshendo.Util.Rule;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// The current implementation of Activation performs several steps
    /// 1. Get a timestamp for the activation
    /// 2. Add the timestamp for the facts
    /// 
    /// </author>
    [Serializable]
    public class BasicActivation : IActivation
    {
        /// <summary> these are the facts that activated the rule. It's important
        /// to keep in mind that any combination of facts may fire a
        /// rule. 
        /// </summary>
        private readonly Index index;

        /// <summary> the time tag is the time stamp of when the activation was
        /// created and added to the agenda.
        /// </summary>
        private readonly long timetag;

        /// <summary> The aggregate time of the facts that triggered the rule.
        /// </summary>
        private long aggreTime = 0;

        /// <summary> the rule to fire
        /// </summary>
        private IRule theRule;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicActivation"/> class.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <param name="inx">The inx.</param>
        public BasicActivation(IRule rule, Index inx)
        {
            theRule = rule;
            index = inx;
            timetag = (DateTime.Now.Ticks - 621355968000000000)/10000;
            calculateTime(inx.Facts);
        }

        /// <summary>
        /// the timestamp of when the activation was created. the time is in
        /// nanoseconds.
        /// </summary>
        /// <value>The time stamp.</value>
        /// <returns>
        /// </returns>
        public long TimeStamp
        {
            get { return timetag; }
        }

        #region IActivation Members

        /// <summary>
        /// The facts that matched the rule
        /// </summary>
        /// <value></value>
        /// <returns>
        /// </returns>
        public IFact[] Facts
        {
            get { return index.Facts; }
        }

        /// <summary>
        /// the index is used to compare the facts quickly
        /// </summary>
        /// <value></value>
        /// <returns>
        /// </returns>
        public Index Index
        {
            get { return index; }
        }

        /// <summary>
        /// the rule that matched
        /// </summary>
        /// <value></value>
        /// <returns>
        /// </returns>
        public IRule Rule
        {
            get { return theRule; }
        }

        /// <summary>
        /// Return the sum of the fact timestamp triggering the rule
        /// </summary>
        /// <value></value>
        /// <returns>
        /// </returns>
        public long AggregateTime
        {
            get { return aggreTime; }
        }


        /// <summary>
        /// Convienant method for comparing two Activations in a module's
        /// activation list. If the rule is the same and the index is the
        /// same, the method returns true. This compare method isn't meant
        /// to be used for strategies. It is up to strategies to compare
        /// two activations against each other using various criteria.
        /// </summary>
        /// <param name="act">The act.</param>
        /// <returns></returns>
        public bool compare(IActivation act)
        {
            if (act == this)
            {
                return false;
            }
            if (act.Rule == theRule && act.Index.Equals(index))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// The purpose of the method is to execute the actions of the
        /// rule. The current implementation calls Rule.setTriggerFacts()
        /// at the start and Rule.resetTriggerFacts() at the end.
        /// Note: Only one activation can be executing at any given time,
        /// so setting the trigger facts should not be an issue. Although
        /// one could queue up the assert/retract/modify in the rule
        /// action, that can lead to undesirable results. The only edge
        /// case that could occur is in backward chaining mode. If the
        /// actions of a rule results in the activation of a backward
        /// rule, it is possible to have nested execution of different
        /// rules. Generally speaking, a rule should not result in
        /// infinite recursion, since that would product a Stack over flow
        /// in Java.
        /// </summary>
        /// <param name="engine">The engine.</param>
        public void executeActivation(Rete engine)
        {
            try
            {
                theRule.TriggerFacts = Facts;
                IAction[] actions = theRule.Actions;
                for (int idx = 0; idx < actions.Length; idx++)
                {
                    if (actions[idx] != null)
                    {
                        actions[idx].executeAction(engine, Facts);
                    }
                    else
                    {
                        throw new ExecuteException(ExecuteException.NULL_ACTION);
                    }
                }
                theRule.resetTriggerFacts();
            }
            catch (ExecuteException e)
            {
                throw e;
            }
        }


        /// <summary>
        /// When watch activation is turned on, we use the method to print out
        /// the activation.
        /// </summary>
        /// <returns></returns>
        public String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("Activation: " + theRule.Name);
            for (int idx = 0; idx < Facts.Length; idx++)
            {
                buf.Append(", f-" + Facts[idx].FactId);
            }
            buf.Append(": AggrTime-" + aggreTime);
            return buf.ToString();
        }

        /// <summary>
        /// Clear will set the rule to null and call Index.Clear
        /// </summary>
        public void clear()
        {
            index.clear();
            theRule = null;
        }

        #endregion

        /// <summary>
        /// Calculates the time.
        /// </summary>
        /// <param name="facts">The facts.</param>
        protected internal void calculateTime(IFact[] facts)
        {
            for (int idx = 0; idx < facts.Length; idx++)
            {
                aggreTime += facts[idx].timeStamp();
            }
        }

        /// <summary>
        /// Clones the activation list.
        /// </summary>
        /// <returns></returns>
        public IActivationList cloneActivationList()
        {
            // TODO finish implementing
            return null;
        }
    }
}