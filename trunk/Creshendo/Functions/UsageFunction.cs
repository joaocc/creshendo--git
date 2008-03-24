/*
* Copyright 2006 Karl-Heinz Krempels
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
    /// <author>  Karl-Heinz Krempels
    /// 
    /// </author>
    /// <returns> a short usage for a function name passed as argument.
    /// 
    /// </returns>
    /// <param name="the">name of a function.
    /// 
    /// </param>
    [Serializable]
    public class UsageFunction : IFunction
    {
        public const String USAGE = "usage";

        /// <summary> 
        /// </summary>
        public UsageFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.STRING_TYPE; }
        }

        public virtual String Name
        {
            get { return USAGE; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam[])}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            String sval = new String("".ToCharArray());
            if (params_Renamed != null)
            {
                if (params_Renamed.Length == 1)
                {
                    if (params_Renamed[0] is ValueParam)
                    {
                        ValueParam n = (ValueParam) params_Renamed[0];
                        sval = n.StringValue;
                        IFunction aFunction = engine.findFunction(sval);
                        if (aFunction != null)
                            sval = aFunction.toPPString(null, 0);
                        else
                            sval = toPPString(null, 0);
                    }
                    else if (params_Renamed[0] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[0];
                        sval = bp.StringValue;
                        IFunction aFunction = engine.findFunction(sval);
                        if (aFunction != null)
                            sval = aFunction.toPPString(null, 0);
                        else
                            sval = toPPString(null, 0);
                    }
                    else if (params_Renamed[0] is FunctionParam2)
                    {
                        FunctionParam2 n = (FunctionParam2) params_Renamed[0];
                        n.Engine = engine;
                        n.lookUpFunction();
                        IReturnVector rval = (IReturnVector) n.Value;
                        sval = rval.firstReturnValue().StringValue;
                        IFunction aFunction = engine.findFunction(sval);
                        if (aFunction != null)
                            sval = aFunction.toPPString(null, 0);
                        else
                            sval = toPPString(null, 0);
                    }
                }
                else
                    sval = toPPString(null, 0);
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.STRING_TYPE, sval);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length >= 0)
            {
                StringBuilder buf = new StringBuilder();

                buf.Append("(usage ");
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
                return "(usage <function-name>)\n" + "Function description:\n" + "\tPrint a short description of <function-name>.\n" + "\tPlease use the command \"functions\" to Get a list of all functions.";
            }
        }

        #endregion
    }
}