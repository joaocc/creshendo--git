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

namespace Creshendo.Util.Rete.Strategies
{
    /// <author>  Peter Lin
    /// *
    /// Breadth strategy is very similar to CLIPS breadth strategy. The design
    /// of Strategies in Sumatra is inspired by CLIPS, but the implementation
    /// is quite different. In CLIPS, there's isn't really an interface and
    /// there isn't the concept of lazy comparison. Since Sumatra uses these
    /// concepts, the design and implementation is quite different.
    /// Breadth strategy is often referred to as FIFO (First In First Out).
    /// What this means in practice is that matches with older facts will be
    /// executed before matches with newer facts. By executed, we mean the
    /// actions of the rule will be executed.
    /// CLIPS beginner guide provides a Clear explanation of breadth:
    /// 5.3.2 Breadth Strategy
    /// Newly activated rules are placed below all rules of the same salience.
    /// 
    /// </author>
    [Serializable]
    public class BreadthStrategy : IStrategy
    {
        /// <summary> 
        /// </summary>
        public BreadthStrategy()
        {
        }

        #region Strategy Members

        public virtual String Name
        {
            get { return "breadth"; }
        }


        public virtual void addActivation(IActivationList thelist, IActivation newActivation)
        {
            thelist.addActivation(newActivation);
        }

        /* (non-Javadoc)
		* @see woolfel.engine.rete.Strategy#nextActivation(woolfel.engine.rete.ActivationList)
		*/

        public virtual IActivation nextActivation(IActivationList thelist)
        {
            return thelist.nextActivation();
        }

        /// <summary>
        /// The method first compares the salience. If the salience is equal,
        /// we then compare the aggregate time.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual int compare(IActivation left, IActivation right)
        {
            if (left.Rule.Salience == right.Rule.Salience)
            {
                // Since Sumatra does not propogate based on natural order, we
                // don't use the Activation timestamp. Instead, we use the
                // aggregate time.
                if (left.AggregateTime == right.AggregateTime)
                {
                    return 0;
                }
                else
                {
                    if (left.AggregateTime > right.AggregateTime)
                    {
                        return - 1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            else
            {
                if (left.Rule.Salience > right.Rule.Salience)
                {
                    return 1;
                }
                else
                {
                    return - 1;
                }
            }
        }

        #endregion
    }
}