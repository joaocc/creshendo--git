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
using System.Collections;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// AlphaNodeMS is similar to AlphaNode. The main difference is AlphaNode2
    /// is for comparing a slot against multiple values with equal/not equal
    /// operator.
    /// <br/>
    /// Although there is a class called Alpha memory, Alpha nodes use a
    /// simple Creshendo.rete.util.IList to remember which facts matched. A decision was made to
    /// use a simple Creshendo.rete.util.List, since assert only takes a single Fact object.
    /// If we change assert to take an array of facts, we would need to
    /// replace the List with a map of AlphaMemory.<br/>
    /// 
    /// 
    /// </author>
    public class NoMemAnd : BaseAlpha2
    {
        /// <summary> The use of Slot(s) is similar to CLIPS design
        /// </summary>
        protected internal Slot2 slot = null;

        /// <summary> The useCount is used to keep track of how many times
        /// an Alpha node is shared. This is needed so that we
        /// can dynamically Remove a rule at run time and Remove
        /// the node from the network. If we didn't keep count,
        /// it would be harder to figure out if we can Remove the node.
        /// </summary>
        protected internal new int useCount = 0;

        /// <summary> 
        /// </summary>
        public NoMemAnd(int id) : base(id)
        {
        }

        /// <summary> the first time the RETE compiler makes the node shared,
        /// it needs to increment the useCount.
        /// </summary>
        /// <param name="">share
        /// 
        /// </param>
        public virtual bool Shared
        {
            get
            {
                if (useCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary> Set the slot id. The slot id is the deftemplate slot id
        /// </summary>
        /// <param name="">id
        /// 
        /// </param>
        public override Slot Slot
        {
            set { slot = (Slot2) value; }
        }

        /// <summary> return the times the node is shared
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual int UseCount
        {
            get { return useCount; }
        }

        /// <summary> method is not implemented, since it doesn't apply
        /// </summary>
        public override int Operator
        {
            get { return 1; }
            set { }
        }


        /// <summary> every time the node is shared, the method
        /// needs to be called so we keep an accurate count.
        /// </summary>
        public override void incrementUseCount()
        {
            useCount++;
        }

        /// <summary> every time a rule is removed from the network
        /// we need to decrement the count. Once the count
        /// reaches zero, we can Remove the node by calling
        /// it's finalize.
        /// </summary>
        public override void decrementUseCount()
        {
            useCount--;
        }

        /// <summary> the implementation will first check to see if the fact already matched.
        /// If it did, the fact stops and doesn't go any further. If it doesn't,
        /// it will attempt to evaluate it and Add the fact if it matches.
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void assertFact(IFact fact, Rete engine, IWorkingMemory mem)
        {
            if (evaluate(fact))
            {
                propogateAssert(fact, engine, mem);
            }
        }

        /// <summary> Retract a fact from the node
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void retractFact(IFact fact, Rete engine, IWorkingMemory mem)
        {
            if (evaluate(fact))
            {
                propogateRetract(fact, engine, mem);
            }
        }

        /// <summary> evaluate the node's value against the slot's value. The method
        /// uses Evaluate class to perform the evaluation
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool evaluate(IFact factInstance)
        {
            bool equal = true;
            IEnumerator itr = slot.EqualList.GetEnumerator();
            Object fval = factInstance.getSlotValue(slot.Id);
            while (itr.MoveNext())
            {
                Object mv = itr.Current;
                if (!mv.Equals(fval))
                {
                    equal = false;
                    break;
                }
            }
            if (equal)
            {
                itr = slot.NotEqualList.GetEnumerator();
                while (itr.MoveNext())
                {
                    Object mv = itr.Current;
                    if (mv.Equals(fval))
                    {
                        equal = false;
                        break;
                    }
                }
            }
            return equal;
        }


        /// <summary> method returns toString() for the hash
        /// </summary>
        public override String hashString()
        {
            return ToString();
        }

        /// <summary> Method returns the string format of the node's condition. later on
        /// this should be cleaned up.
        /// </summary>
        public override String ToString()
        {
            return "slot(" + slot.Name + ") " + slot.toString("&") + " - useCount=" + useCount;
        }

        /// <summary> Method returns the pretty printer formatted string of the node's
        /// condition. For now, the method just replaces the operator. It might
        /// be nice to replace the slot id with the slot name.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public override String toPPString()
        {
            return "and node-" + nodeID + "> slot(" + slot.Name + ") " + slot.toString("&") + " - useCount=" + useCount;
        }
    }
}