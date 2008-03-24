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
using Creshendo.Util.Collections;
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <summary> 
    /// </summary>
    /// <author>  Peter Lin
    /// 
    /// </author>
    public class InterpretedFunction : IFunction, IScope //: FunctionScope
    {
        private GenericHashMap<string, object> bindings;

        /// <summary> these are the functions we pass to the top level function.
        /// they may be different than the input parameters for the
        /// function.
        /// </summary>
        private IParameter[][] functionParams = null;

        protected internal IParameter[] inputParams = null;
        private IFunction[] internalFunction = null;
        private String name = null;
        protected internal String ppString = null;

        /// <summary> 
        /// </summary>
        public InterpretedFunction(String name, IParameter[] params_Renamed, IFunction[] func, IParameter[][] functionParams)
        {
            InitBlock();
            this.name = name;
            inputParams = params_Renamed;
            internalFunction = func;
            this.functionParams = functionParams;
        }

        public virtual IParameter[] InputParameters
        {
            get { return inputParams; }
        }

        public virtual IParameter[][] FunctionParams
        {
            get { return functionParams; }

            set { functionParams = value; }
        }

        #region Function Members

        public virtual String Name
        {
            get { return name; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (BoundParam)}; }
        }

        public virtual int ReturnType
        {
            get { return Constants.OBJECT_TYPE; }
        }

        /* (non-Javadoc)
		* @see org.jamocha.rete.Function#executeFunction(org.jamocha.Creshendo.Util.Rete.Rete, org.jamocha.rete.Parameter[])
		*/

        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            // the first thing we do is set the values
            DefaultReturnVector ret = new DefaultReturnVector();
            if (params_Renamed.Length == inputParams.Length)
            {
                for (int idx = 0; idx < inputParams.Length; idx++)
                {
                    BoundParam bp = (BoundParam) inputParams[idx];
                    bindings.Put(bp.VariableName, params_Renamed[idx].Value);
                }
                engine.pushScope(this);
                for (int idx = 0; idx < functionParams.Length; idx++)
                {
                    ret = (DefaultReturnVector) internalFunction[idx].executeFunction(engine, functionParams[idx]);
                }
                engine.popScope();
                return ret;
            }
            else
            {
                DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, false);
                ret.addReturnValue(rv);
                DefaultReturnValue rv2 = new DefaultReturnValue(Constants.STRING_TYPE, "incorrect number of parameters");
                ret.addReturnValue(rv2);
                return ret;
            }
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return ppString;
        }

        #endregion

        #region Scope Members

        public virtual Object getBindingValue(string var)
        {
            return bindings.Get(var);
        }

        public virtual void setBindingValue(String name, Object value_Renamed)
        {
            bindings.Put(name, value_Renamed);
        }

        #endregion

        private void InitBlock()
        {
            bindings = new GenericHashMap<string, object>();
        }

        public virtual void configureFunction(Rete engine)
        {
        }
    }
}