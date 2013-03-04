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
using Creshendo.Functions;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// AlphaNodePredConstr is similar to AlphaNode with the difference that
    /// this node calls a function. The function must return boolean type.
    /// In other words, the function has to evaluate to true or false.
    /// example of a predicate constraint:
    /// 
    /// 
    /// </author>
    public class AlphaNodePredConstr : BaseAlpha
    {
        protected internal CompositeIndex compIndex = null;

        /// <summary> The function to call
        /// </summary>
        protected internal IFunction function = null;

        protected internal String hashstring = null;

        protected internal IParameter[] params_Renamed = null;

        /// <summary> The use of Slot(s) is similar to CLIPS design
        /// </summary>
        protected internal Slot slot = null;

        /// <summary> The default constructor takes a Node id and function
        /// </summary>
        /// <param name="">id
        /// </param>
        /// <param name="">func
        /// 
        /// </param>
        public AlphaNodePredConstr(int id, IFunction func, IParameter[] params_Renamed) : base(id)
        {
            function = func;
            this.params_Renamed = params_Renamed;
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

        /* (non-Javadoc)
		* @see woolfel.engine.rete.BaseAlpha#assertFact(woolfel.engine.rete.Fact, woolfel.engine.Creshendo.Util.Rete.Rete, woolfel.engine.rete.WorkingMemory)
		*/

        public override void assertFact(IFact fact, Rete engine, IWorkingMemory mem)
        {
            if (evaluate(fact, engine))
            {
                IAlphaMemory alpha = (IAlphaMemory) mem.getAlphaMemory(this);
                alpha.addPartialMatch(fact);
                propogateAssert(fact, engine, mem);
            }
        }

        /*
		* (non-Javadoc)
		* 
		* @see woolfel.engine.rete.BaseAlpha#retractFact(woolfel.engine.rete.Fact,
		*      woolfel.engine.Creshendo.Util.Rete.Rete, woolfel.engine.rete.WorkingMemory)
		*/

        public override void retractFact(IFact fact, Rete engine, IWorkingMemory mem)
        {
            IAlphaMemory alpha = (IAlphaMemory) mem.getAlphaMemory(this);
            if (alpha.removePartialMatch(fact) != null)
            {
                propogateRetract(fact, engine, mem);
            }
        }

        /// <summary> The method uses the function to evaluate the fact
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool evaluate(IFact factInstance, Rete engine)
        {
            for (int idx = 0; idx < params_Renamed.Length; idx++)
            {
                if (params_Renamed[idx] is BoundParam)
                {
                    ((BoundParam) params_Renamed[idx]).Facts = new IFact[] {factInstance};
                }
            }
            IReturnVector rv = function.executeFunction(engine, params_Renamed);
            IReturnValue rval = rv.firstReturnValue();
            return rval.BooleanValue;
        }


        /* (non-Javadoc)
		* @see woolfel.engine.rete.BaseNode#hashString()
		*/

        public override String hashString()
        {
            if (hashstring == null)
            {
                hashstring = slot.Id + ":" + operator_Renamed + ":" + slot.Value.ToString();
            }
            return hashstring;
        }

        public override String ToString()
        {
            return "slot(" + slot.Id + ") " + ConversionUtils.getPPOperator(operator_Renamed) + " " + slot.Value.ToString() + " - useCount=" + useCount;
        }

        public override String toPPString()
        {
            return "node-" + nodeID + "> slot(" + slot.Name + ") " + ConversionUtils.getPPOperator(operator_Renamed) + " " + ConversionUtils.formatSlot(slot.Value) + " - useCount=" + useCount;
        }
    }
}