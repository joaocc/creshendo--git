/*
* Copyright 2006 Nikolaus Koemm
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

namespace Creshendo.Functions.Math
{
    /// <author>  Nikolaus Koemm
    /// 
    /// Log returns the natural logarithm of a double value.
    /// 
    /// </author>
    [Serializable]
    public class Log : IFunction
    {
        public const String LOG = "log";

        /// <summary> 
        /// </summary>
        public Log() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BIG_DECIMAL; }
        }

        public virtual String Name
        {
            get { return LOG; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam[])}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            Decimal bdval = new Decimal(0);
            if (params_Renamed.Length == 1)
            {
                if (params_Renamed[0] is ValueParam)
                {
                    ValueParam n = (ValueParam) params_Renamed[0];
                    bdval = n.BigDecimalValue;
                }
                else if (params_Renamed[0] is BoundParam)
                {
                    BoundParam bp = (BoundParam) params_Renamed[0];
                    bdval = (Decimal) engine.getBinding(bp.VariableName);
                }
                else if (params_Renamed[0] is FunctionParam2)
                {
                    FunctionParam2 n = (FunctionParam2) params_Renamed[0];
                    n.Engine = engine;
                    n.lookUpFunction();
                    IReturnVector rval = (IReturnVector) n.Value;
                    bdval = rval.firstReturnValue().BigDecimalValue;
                }
                double bdh = System.Math.Log(Decimal.ToDouble(bdval)); //.pow(Math.E, bdval.doubleValue());
                bdval = new Decimal(bdh);
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BIG_DECIMAL, bdval);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length >= 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(log");
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
                return "(log <expression>)\n" + "Function description:\n" + "\tCalculates the natural logarithm of its argument.";
            }
        }

        #endregion
    }
}