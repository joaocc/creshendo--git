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
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// 
    /// 
    /// </author>
    [Serializable]
    public class EqFunction : IFunction
    {
        public const String EQUAL = "eq";

        /// <summary> 
        /// </summary>
        public EqFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return EQUAL; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam[])}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            DefaultReturnVector ret = new DefaultReturnVector();
            bool eq = false;
            if (params_Renamed != null && params_Renamed.Length > 1)
            {
                Object first = null;
                if (params_Renamed[0] is ValueParam)
                {
                    ValueParam n = (ValueParam) params_Renamed[0];
                    first = n.Value;
                }
                else if (params_Renamed[0] is BoundParam)
                {
                    BoundParam bp = (BoundParam) params_Renamed[0];
                    first = (Decimal) engine.getBinding(bp.VariableName);
                }
                else if (params_Renamed[0] is FunctionParam2)
                {
                    FunctionParam2 n = (FunctionParam2) params_Renamed[0];
                    n.Engine = engine;
                    n.lookUpFunction();
                    IReturnVector rval = (IReturnVector) n.Value;
                    first = rval.firstReturnValue().Value;
                }
                bool eval = true;
                for (int idx = 1; idx < params_Renamed.Length; idx++)
                {
                    Object right = null;
                    if (params_Renamed[idx] is ValueParam)
                    {
                        ValueParam n = (ValueParam) params_Renamed[idx];
                        right = n.Value;
                    }
                    else if (params_Renamed[idx] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[idx];
                        right = engine.getBinding(bp.VariableName);
                    }
                    else if (params_Renamed[idx] is FunctionParam2)
                    {
                        FunctionParam2 n = (FunctionParam2) params_Renamed[idx];
                        n.Engine = engine;
                        n.lookUpFunction();
                        IReturnVector rval = (IReturnVector) n.Value;
                        right = rval.firstReturnValue().Value;
                    }
                    if (first == null && right != null)
                    {
                        eval = false;
                        break;
                    }
                    else if (first != null && !first.Equals(right))
                    {
                        eval = false;
                        break;
                    }
                }
                eq = eval;
            }
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, eq);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(eq (<literal> | <binding>)+)\n" + "Function description:\n" + "\tCompares a literal value against one or more" + "bindings. \n\tIf all of the bindings are equal to the constant value," + "\n\tthe function returns true.";
        }

        #endregion
    }
}