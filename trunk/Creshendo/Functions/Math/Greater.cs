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
    /// 
    /// Greater will compare 2 or more numeric values and return true if the (n-1)th value
    /// is greater than the nth.
    /// 
    /// </author>
    [Serializable]
    public class Greater : IFunction
    {
        public const String GREATER = "greater";

        /// <summary> 
        /// </summary>
        public Greater() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            /*
			* (non-Javadoc)
			* 
			* @see woolfel.engine.rete.Function#getReturnType()
			*/

            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return GREATER; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam), typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            bool eval = false;
            Decimal left;
            Decimal right;
            if (params_Renamed != null)
            {
                left = params_Renamed[0].BigDecimalValue;
                right = (Decimal) params_Renamed[1].getValue(engine, Constants.BIG_DECIMAL);
                eval = (Decimal.ToDouble(left) > Decimal.ToDouble(right));
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, eval);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(>");
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
                return "(> (<literal> | <binding>)+)\n" + "Function description:\n" + "\t Returns the symbol TRUE if for all its arguments, " + "argument \n \t n-1 is greater than argument n";
            }
        }

        #endregion
    }
}