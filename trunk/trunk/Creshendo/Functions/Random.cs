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

namespace Creshendo.Functions
{
    /// <author>  Christian Ebert
    /// 
    /// Returns a double value with a positive sign, greater than or equal to 0.0 and less 
    /// than 1.0. Returned values are chosen pseudorandomly with (approximately)
    /// uniform distribution from that range.
    /// 
    /// </author>
    [Serializable]
    public class Random : IFunction
    {
        public const String RANDOM = "random";

        private readonly System.Random _random;

        /// <summary> 
        /// </summary>
        public Random()
        {
            _random = new System.Random();
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.DOUBLE_PRIM_TYPE; }
        }

        public virtual String Name
        {
            get { return RANDOM; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            double dval = _random.NextDouble();
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
                buf.Append("(random");
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
                return "(random)\n" + "Function description:\n" + "\tReturns a random value between 0.0 and 1.0.";
            }
        }

        #endregion
    }
}