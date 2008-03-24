/*
* Copyright 2006 Christian Ebert 
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
    /// <author>  Christian Ebert
    /// 
    /// Returns the trigonometric cosine of an angle.
    /// 
    /// </author>
    [Serializable]
    public class Cos : IFunction
    {
        public const String COS = "cos";

        /// <summary> 
        /// </summary>
        public Cos() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.DOUBLE_PRIM_TYPE; }
        }

        public virtual String Name
        {
            get { return COS; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam[])}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            double dval = 0;
            if (params_Renamed != null)
            {
                if (params_Renamed.Length == 1)
                {
                    if (params_Renamed[0] is ValueParam)
                    {
                        ValueParam n = (ValueParam) params_Renamed[0];
                        dval = n.DoubleValue;
                        dval = System.Math.Cos(dval);
                    }
                    else if (params_Renamed[0] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[0];
                        dval = bp.DoubleValue;
                        dval = System.Math.Cos(dval);
                    }
                    else if (params_Renamed[0] is FunctionParam2)
                    {
                        FunctionParam2 n = (FunctionParam2) params_Renamed[0];
                        n.Engine = engine;
                        n.lookUpFunction();
                        IReturnVector rval = (IReturnVector) n.Value;
                        dval = rval.firstReturnValue().DoubleValue;
                        dval = System.Math.Cos(dval);
                    }
                }
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.DOUBLE_PRIM_TYPE, dval);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length >= 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(cos");
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
                return "(cos <literal> | <binding>)\n" + "Function description:\n" + "\tCalculates the cosine of the numeric argument.\n" + "\tThe argument is expected to be in radians.";
            }
        }

        #endregion
    }
}