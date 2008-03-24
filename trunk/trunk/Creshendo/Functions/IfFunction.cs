/*
* Copyright 2006 Nikolaus Koemm, Christian Ebert 
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
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <author>  Nikolaus Koemm, Christian Ebert
    /// 
    /// Returns the absolute value of a double value.
    /// 
    /// </author>
    [Serializable]
    public class IfFunction : IFunction
    {
        public const String IF = "if";

        /// <summary> 
        /// </summary>
        private const long serialVersionUID = 1L;

        /// <summary> 
        /// </summary>
        public IfFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.OBJECT_TYPE; }
        }

        public virtual String Name
        {
            get { return IF; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam[])}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            Object result = null;
            if (params_Renamed != null)
            {
                if (params_Renamed.Length >= 3)
                {
                    bool conditionValue = false;
                    if (params_Renamed[0] is ValueParam)
                    {
                        ValueParam n = (ValueParam) params_Renamed[0];
                        conditionValue = n.BooleanValue;
                    }
                    else if (params_Renamed[0] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[0];
                        conditionValue = ((Boolean) engine.getBinding(bp.VariableName));
                    }
                    else if (params_Renamed[0] is FunctionParam2)
                    {
                        FunctionParam2 n = (FunctionParam2) params_Renamed[0];
                        n.Engine = engine;
                        n.lookUpFunction();
                        IReturnVector rval = (IReturnVector) n.Value;
                        conditionValue = rval.firstReturnValue().BooleanValue;
                    }
                    if (params_Renamed[1] is ValueParam && "then".Equals(params_Renamed[1].StringValue))
                    {
                        bool elseExpressions = false;
                        for (int i = 2; i < params_Renamed.Length; ++i)
                        {
                            if (params_Renamed[i] is ValueParam && "else".Equals(params_Renamed[i].StringValue))
                            {
                                elseExpressions = true;
                            }
                            else
                            {
                                if ((conditionValue && !elseExpressions) || (!conditionValue && elseExpressions))
                                {
                                    if (params_Renamed[i] is ValueParam)
                                    {
                                        ValueParam n = (ValueParam) params_Renamed[i];
                                        result = n.Value;
                                    }
                                    else if (params_Renamed[i] is BoundParam)
                                    {
                                        BoundParam bp = (BoundParam) params_Renamed[i];
                                        result = engine.getBinding(bp.VariableName);
                                    }
                                    else if (params_Renamed[i] is FunctionParam2)
                                    {
                                        FunctionParam2 n = (FunctionParam2) params_Renamed[i];
                                        n.Engine = engine;
                                        n.lookUpFunction();
                                        IReturnVector rval = (IReturnVector) n.Value;
                                        if (rval.size() > 0)
                                        {
                                            result = rval.firstReturnValue().Value;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.OBJECT_TYPE, result);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length >= 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(if");
                int idx = 0;
                if (params_Renamed[idx] is BoundParam)
                {
                    BoundParam bp = (BoundParam) params_Renamed[idx];
                    buf.Append(" ?" + bp.VariableName);
                }
                else if (params_Renamed[idx] is ValueParam)
                {
                    buf.Append(" " + params_Renamed[idx].StringValue);
                }
                else
                {
                    buf.Append(" " + params_Renamed[idx].StringValue);
                }
                buf.Append(")");
                return buf.ToString();
            }
            else
            {
                return "(if <boolean expression> then <expression>+ [else <expression>+])\n" + "Function description:\n" + "\tExecutes the expressions after then if the boolean expressions evaluates to true, otherwise it executes the expressions after the optional else.";
            }
        }

        #endregion
    }
}