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
    /// NoMemAlphaNode has no memory. It is different than the normal
    /// AlphaNode in that asserts and retract always propogate. this means
    /// retract performance is a lot slower than AlphaNode.<br/>
    /// <br/>
    /// 
    /// 
    /// </author>
    public class NoMemANode : BaseAlpha2
    {
        protected internal CompositeIndex compIndex = null;
        protected internal String hashstring = null;

        /// <summary> The use of Slot(s) is similar to CLIPS design
        /// </summary>
        protected internal Slot slot = null;

        /// <summary> 
        /// </summary>
        public NoMemANode(int id) : base(id)
        {
        }

        /// <summary> Set the operator using the int value
        /// </summary>
        /// <param name="">opr
        /// 
        /// </param>
        public override int Operator
        {
            get { return operator_Renamed; }
            set { operator_Renamed = value; }
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
            set { slot = value; }
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

        public virtual CompositeIndex HashIndex
        {
            get
            {
                if (compIndex == null)
                {
                    compIndex = new CompositeIndex(slot.Name, operator_Renamed, slot.Value);
                }
                return compIndex;
            }
        }


        /// <summary> 
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
                // if watch is on, we notify the engine. Rather than
                // create an event class here, we let Rete do that.
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
            return Evaluate.evaluate(operator_Renamed, factInstance.getSlotValue(slot.Id), slot.Value);
        }

        /// <summary> Method returns the string format of the node's condition. later on
        /// this should be cleaned up.
        /// </summary>
        public override String ToString()
        {
            return "slot(" + slot.Id + ") " + ConversionUtils.getPPOperator(operator_Renamed) + " " + slot.Value.ToString() + " - useCount=" + useCount;
        }


        /// <summary> Method returns a hash string for ObjectTypeNode. The format is
        /// slotName:operator:value
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public override String hashString()
        {
            if (hashstring == null)
            {
                hashstring = slot.Id + ":" + operator_Renamed + ":" + slot.Value.ToString();
            }
            return hashstring;
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
            return "<node-" + nodeID + "> slot(" + slot.Name + ") " + ConversionUtils.getPPOperator(operator_Renamed) + " " + ConversionUtils.formatSlot(slot.Value) + " - useCount=" + useCount;
        }
    }
}