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
    /// Degrees converts an angle measured in radians to an approximately equivalent
    /// angle measured in degrees.
    /// 
    /// </author>
    [Serializable]
    public class Degrees : IFunction
    {
        public const String DEGREES = "degrees";

        /// <summary> 
        /// </summary>
        public Degrees() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BIG_DECIMAL; }
        }

        public virtual String Name
        {
            get { return DEGREES; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam[])}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            Decimal bdval = new Decimal(0);
            if (params_Renamed != null)
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
                double bdh = Decimal.ToDouble(bdval);
                bdval = Convert.ToDecimal(bdh*57.29578);
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
                buf.Append("(degrees");
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
                return "(degrees (<literal> | <binding>))\n" + "Function description:\n" + "\t Converts its only argument from units of radians" + "to units of degrees.";
            }
        }

        #endregion
    }
}