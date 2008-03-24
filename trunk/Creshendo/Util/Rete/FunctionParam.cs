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
    /// FunctionParam is a parameter which gets a value from a nested
    /// function call. As a general rule, a function parameter may need
    /// to assert/modify/retract facts; therefore the default constructor
    /// takes Rete. This can happen when a user writes a test pattern
    /// like (test (> 10 (* .5 ?var) ) ) .
    /// In the example above, the test would first multiple the bound
    /// variable with .5. The product is then compared to 10. At runtime,
    /// the TestNode would pass the necessary fact to a function that
    /// uses a bound variable.
    /// 
    /// </author>
    public class FunctionParam : AbstractParam
    {
        protected internal Rete engine = null;
        protected internal IFact[] facts;

        /// <summary> The function to call
        /// </summary>
        protected internal IFunction func = null;

        protected internal BoundParam[] params_Renamed = null;
        protected internal Object value_Renamed = null;
        protected internal int valueType = - 1;

        /// <summary> The constructor takes a parameter
        /// </summary>
        public FunctionParam(IFunction func, Rete rete) 
        {
            this.func = func;
            engine = rete;
        }

        /// <summary> Return the return value type.
        /// </summary>
        public override int ValueType
        {
            get { return valueType; }
            set { }
        }

        /// <summary> getValue() should trigger the function 
        /// </summary>
        public override Object Value
        {
            get
            {
                // execute the function and return the value
                initParams();
                value_Renamed = func.executeFunction(engine, params_Renamed);
                return value_Renamed;
            }
            set { }
        }

        /// <summary> 
        /// </summary>
        /// <param name="">facts
        /// 
        /// </param>
        public virtual IFact[] Facts
        {
            set { facts = value; }
        }

        public override bool ObjectBinding
        {
            get { return false; }
        }


        public override Object getValue(Rete engine, int valueType)
        {
            initParams();
            IReturnVector rval = func.executeFunction(engine, params_Renamed);
            return rval.firstReturnValue().BigDecimalValue;
        }


        /// <summary> 
        /// *
        /// </summary>
        protected internal virtual void initParams()
        {
            for (int idx = 0; idx < params_Renamed.Length; idx++)
            {
                if (params_Renamed[idx].ObjectBinding)
                {
                    params_Renamed[idx].Facts = facts;
                }
                else if (params_Renamed[idx] is BoundParam)
                {
                    // we look up the value
                    BoundParam bp = (BoundParam) params_Renamed[idx];
                    Object val = engine.getDefglobalValue(bp.VariableName);
                    bp.ResolvedValue = val;
                }
            }
        }

        /// <summary> reset the function and set the references to the facts
        /// to null
        /// </summary>
        public override void reset()
        {
            facts = null;
            value_Renamed = null;
        }
    }
}