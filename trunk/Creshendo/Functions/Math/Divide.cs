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
using System.Text;
using Creshendo.Util.Rete;

namespace Creshendo.Functions.Math
{
    /// <author>  Peter Lin
    /// *
    /// Divide will divide one or more numbers and return a Double value
    /// 
    /// </author>
    [Serializable]
    public class Divide : IFunction
    {
        public const String DIVIDE = "divide";

        /// <summary> 
        /// </summary>
        public Divide() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BIG_DECIMAL; }
        }

        public virtual String Name
        {
            get { return DIVIDE; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam[])}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            Decimal bdval = 0;
            if (params_Renamed != null)
            {
                bdval = (Decimal) params_Renamed[0].getValue(engine, Constants.BIG_DECIMAL);
                for (int idx = 1; idx < params_Renamed.Length; idx++)
                {
                    Decimal bd = (Decimal) params_Renamed[idx].getValue(engine, Constants.BIG_DECIMAL);
                    bdval = secureDivide(bdval, bd);
                }
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.DOUBLE_PRIM_TYPE, bdval);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(/");
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
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
                }
                buf.Append(")");
                return buf.ToString();
            }
            else
            {
                return "(/ (<literal> | <binding>)+)\n" + "Function description:\n" + "\t Returns the value of the first argument divided by " + "each of the subsequent arguments.";
            }
        }

        #endregion

        private Decimal secureDivide(Decimal dividend, Decimal divisor)
        {
            try
            {
                dividend = Decimal.Divide(dividend, divisor);
            }
            catch (ArithmeticException e)
            {
                dividend = new Decimal(Decimal.ToDouble(dividend)/Decimal.ToDouble(divisor));
            }

            return dividend;
        }
    }
}