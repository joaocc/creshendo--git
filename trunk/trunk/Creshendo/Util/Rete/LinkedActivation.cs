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
    /// LinkedActivation is different than BasicActivation in a couple of
    /// ways. LinkedActivation makes it easier to Remove Activations from
    /// an ActivationList, without having to iterate over the activations.
    /// When the activation is executed or removed, it needs to make sure
    /// it checks the previous and Current and set them correctly.
    /// 
    /// </author>
    public class LinkedActivation : IActivation
    {
        private readonly Index index;
        private long aggreTime = - 1;
        private LinkedActivation next = null;
        private LinkedActivation prev = null;

        private IRule theRule;

        private TerminalNode2 tnode = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedActivation"/> class.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <param name="inx">The inx.</param>
        public LinkedActivation(IRule rule, Index inx)
        {
            theRule = rule;
            index = inx;
            // calculateTime(inx.getFacts());
        }

        /// <summary>
        /// the method will set the previous activation to the one
        /// passed to the method and it will also set previous.Current
        /// to this instance.
        /// </summary>
        /// <value>The previous.</value>
        public LinkedActivation Previous
        {
            get { return prev; }

            set
            {
                prev = value;
                if (value != null)
                {
                    value.next = this;
                }
            }
        }

        /// <summary>
        /// the method will set the Current activation to the one
        /// passed to the method and it will set Current.prev to
        /// this instance.
        /// </summary>
        /// <value>The next.</value>
        public LinkedActivation Next
        {
            get { return next; }

            set
            {
                next = value;
                if (value != null)
                {
                    value.prev = this;
                }
            }
        }

        /// <summary>
        /// Gets or sets the terminal node.
        /// </summary>
        /// <value>The terminal node.</value>
        public TerminalNode2 TerminalNode
        {
            get { return tnode; }

            set { tnode = value; }
        }

        #region IActivation Members

        /// <summary>
        /// The aggregate time is the sum of the Fact timestamps
        /// </summary>
        /// <value></value>
        /// <returns>
        /// </returns>
        public long AggregateTime
        {
            get
            {
                if (aggreTime == - 1)
                {
                    calculateTime(index.Facts);
                }
                return aggreTime;
            }
        }

        /// <summary>
        /// Get the Facts that triggered the rule
        /// </summary>
        /// <value></value>
        /// <returns>
        /// </returns>
        public IFact[] Facts
        {
            get { return index.Facts; }
        }

        /// <summary>
        /// Get the Index for the Facts
        /// </summary>
        /// <value></value>
        /// <returns>
        /// </returns>
        public Index Index
        {
            get { return index; }
        }

        /// <summary>
        /// Get the rule that should fire
        /// </summary>
        /// <value></value>
        /// <returns>
        /// </returns>
        public IRule Rule
        {
            get { return theRule; }
        }


        /* (non-Javadoc)
		* @see woolfel.engine.rete.Activation#compare(woolfel.engine.rete.Activation)
		*/

        /// <summary>
        /// If the activation passed in the parameter has the same rule
        /// and facts, the method should return true
        /// </summary>
        /// <param name="act">The act.</param>
        /// <returns></returns>
        public bool compare(IActivation act)
        {
            if (act == this)
            {
                return true;
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

        /* (non-Javadoc)
		* @see woolfel.engine.rete.Activation#executeActivation(woolfel.engine.Creshendo.Util.Rete.Rete)
		*/

        /// <summary>
        /// Execute the right-hand side (aka actions) of the rule.
        /// </summary>
        /// <param name="engine">The engine.</param>
        public void executeActivation(Rete engine)
        {
            // if previous and Current are not null, set the previous/Current
            // of each and then set the reference to null
            remove(engine);
            try
            {
                theRule.TriggerFacts = index.Facts;
                IAction[] actions = theRule.Actions;
                for (int idx = 0; idx < actions.Length; idx++)
                {
                    if (actions[idx] != null)
                    {
                        actions[idx].executeAction(engine, index.Facts);
                    }
                    else
                    {
                        throw new ExecuteException(ExecuteException.NULL_ACTION);
                    }
                }
            }
            catch (ExecuteException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// after the activation is executed, Clear has to be called.
        /// </summary>
        public void clear()
        {
            theRule = null;
            tnode = null;
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
            IFact[] facts = index.Facts;
            for (int idx = 0; idx < facts.Length; idx++)
            {
                buf.Append(", id-" + facts[idx].FactId);
            }
            buf.Append(" AggrTime-" + aggreTime);
            return buf.ToString();
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
                aggreTime += facts[idx].timeStamp() + facts[idx].FactId;
            }
        }

        /// <summary>
        /// Remove the Activation from the list and set the previous
        /// and Current activation correctly. There's basically 3 cases
        /// we have to handle.
        /// 1. first
        /// 2. last
        /// 3. somewhere in between
        /// The current implementation will first set the previous
        /// and Current. Once they are correctly set, it will set
        /// the references to those LinkedActivation to null.
        /// </summary>
        public void remove()
        {
            if (prev != null && next != null)
            {
                prev.Next = next;
            }
            else if (prev != null && next == null)
            {
                prev.Next = null;
            }
            else if (prev == null && next != null)
            {
                next.Previous = null;
            }
            prev = null;
            next = null;
        }

        /// <summary>
        /// method is used to make sure the activation is removed from
        /// TerminalNode2.
        /// </summary>
        /// <param name="engine">The engine.</param>
        protected internal void remove(Rete engine)
        {
            if (tnode != null)
            {
                tnode.removeActivation(engine.WorkingMemory, this);
            }
        }

        /// <summary>
        /// Clones the activation.
        /// </summary>
        /// <returns></returns>
        public LinkedActivation cloneActivation()
        {
            LinkedActivation la = new LinkedActivation(theRule, index);
            return la;
        }
    }
}