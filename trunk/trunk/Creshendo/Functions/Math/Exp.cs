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
    /// Exp returns Euler's number raised to the power of the arguments.
    /// 
    /// </author>
    [Serializable]
    public class Exp : IFunction
    {
        public const String EXP = "exp";

        /// <summary> 
        /// </summary>
        public Exp() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BIG_DECIMAL; }
        }

        public virtual String Name
        {
            get { return EXP; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam[])}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            Decimal bdval = new Decimal(0);
            Decimal bd = new Decimal(0);
            if (params_Renamed != null)
            {
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is ValueParam)
                    {
                        ValueParam n = (ValueParam) params_Renamed[idx];
                        bdval = n.BigDecimalValue;
                    }
                    else if (params_Renamed[idx] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[idx];
                        bdval = (Decimal) engine.getBinding(bp.VariableName);
                    }
                    else if (params_Renamed[idx] is FunctionParam2)
                    {
                        FunctionParam2 n = (FunctionParam2) params_Renamed[idx];
                        n.Engine = engine;
                        n.lookUpFunction();
                        IReturnVector rval = (IReturnVector) n.Value;
                        bdval = rval.firstReturnValue().BigDecimalValue;
                    }
                    double bdh = System.Math.Pow(System.Math.E, Decimal.ToDouble(bdval));
                    bdval = new Decimal(bdh);
                }
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
                buf.Append("(exp");
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
                return "(exp (<literal> | <binding>)+)\n" + "Function description:\n" + "\tRaises the value e to the power specified by its\n" + "\targuments.";
            }
        }

        #endregion
    }
}