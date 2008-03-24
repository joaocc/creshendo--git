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
    /// Sqrt returns the square root value of a double value.
    /// 
    /// </author>
    [Serializable]
    public class Sqrt : IFunction
    {
        public const String SQRT = "sqrt";

        /// <summary> 
        /// </summary>
        public Sqrt() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BIG_DECIMAL; }
        }

        public virtual String Name
        {
            get { return SQRT; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam[])}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            Decimal bdval = 0;
            double bdh = 0.0;
            if (params_Renamed.Length == 1)
            {
                bdval = (Decimal) params_Renamed[0].getValue(engine, Constants.BIG_DECIMAL);
                bdh = System.Math.Sqrt(Decimal.ToDouble(bdval));
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
                buf.Append("(sqrt");
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
                return "(sqrt <expression>)\n" + "Function description:\n" + "\tReturns the square root of its only argument.";
            }
        }

        #endregion
    }
}