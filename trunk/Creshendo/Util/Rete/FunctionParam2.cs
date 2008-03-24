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
    /// Describe difference between the Function parameters
    /// 
    /// </author>
    public class FunctionParam2 : AbstractParam
    {
        private Rete engine = null;
        protected internal IFunction func = null;
        protected internal String funcName = null;
        private IParameter[] params_Renamed = null;

        public FunctionParam2() 
        {
        }

        public virtual String FunctionName
        {
            get { return funcName; }

            set { funcName = value; }
        }

        public virtual Rete Engine
        {
            set { engine = value; }
        }

        public virtual IParameter[] Parameters
        {
            set { params_Renamed = value; }
        }

        public override int ValueType
        {
            set { }
            get { return func.ReturnType; }
        }

        public override Object Value
        {
            set { }
            get
            {
                if (params_Renamed != null)
                {
                    return func.executeFunction(engine, params_Renamed);
                }
                else
                {
                    return null;
                }
            }
        }

        public override bool ObjectBinding
        {
            get { return false; }
        }


        public virtual void configure(Rete engine, Rule.IRule util)
        {
            if (this.engine == null)
            {
                this.engine = engine;
            }
            for (int idx = 0; idx < params_Renamed.Length; idx++)
            {
                if (params_Renamed[idx] is BoundParam)
                {
                    // we need to set the row value if the binding is a slot or fact
                    BoundParam bp = (BoundParam) params_Renamed[idx];
                    Binding b1 = util.getBinding(bp.VariableName);
                    if (b1 != null)
                    {
                        bp.Row = b1.LeftRow;
                        if (b1.LeftIndex == - 1)
                        {
                            bp.setObjectBinding(true);
                        }
                    }
                }
            }
        }


        public virtual void lookUpFunction()
        {
            func = engine.findFunction(funcName);
        }


        /// <summary> TODO we may want to check the value type and throw and exception
        /// for now just getting it to work.
        /// </summary>
        public override Object getValue(Rete engine, int valueType)
        {
            if (params_Renamed != null)
            {
                this.engine = engine;
                lookUpFunction();
                IReturnVector rval = func.executeFunction(engine, params_Renamed);
                return rval.firstReturnValue().BigDecimalValue;
            }
            else
            {
                return null;
            }
        }

        public override void reset()
        {
            engine = null;
            params_Renamed = null;
        }

        public virtual String toPPString()
        {
            lookUpFunction();
            return func.toPPString(params_Renamed, 1);
        }
    }
}