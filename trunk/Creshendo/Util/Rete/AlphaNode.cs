/*
* Copyright 2002-2007 Peter Lin
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
    /// AlphaNode is a single input node. The design is influenced by CLIPS
    /// PatternNode and PartialMatch. I've tried to stay as close to CLIPS
    /// design as practical. A obvious difference between CLIPS and this
    /// implementation is the lack of memory allocation or passing pointers.<br/>
    /// <br/>
    /// 
    /// 
    /// </author>
    public class AlphaNode : BaseAlpha2
    {
        protected internal CompositeIndex compIndex = null;
        protected internal String hashstring = null;

        /// <summary> The use of Slot(s) is similar to CLIPS design
        /// </summary>
        protected internal Slot slot = null;

        /// <summary> 
        /// </summary>
        public AlphaNode(int id) : base(id)
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
                IAlphaMemory alpha = (IAlphaMemory) mem.getAlphaMemory(this);
                alpha.addPartialMatch(fact);
                // if watch is on, we notify the engine. Rather than
                // create an event class here, we let Rete do that.
                propogateAssert(fact, engine, mem);
            }
        }

        /// <summary> Retract a fact from the node
        /// 
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void retractFact(IFact fact, Rete engine, IWorkingMemory mem)
        {
            IAlphaMemory alpha = (IAlphaMemory) mem.getAlphaMemory(this);
            if (alpha.removePartialMatch(fact) != null)
            {
                // if watch is on, we notify the engine. Rather than
                // create an event class here, we let Rete do that.
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
            //UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
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
            return "node-" + nodeID + "> slot(" + slot.Name + ") " + ConversionUtils.getPPOperator(operator_Renamed) + " " + ConversionUtils.formatSlot(slot.Value) + " - useCount=" + useCount;
        }
    }
}