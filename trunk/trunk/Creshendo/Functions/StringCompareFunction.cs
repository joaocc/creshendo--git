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
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// *
    /// 
    /// </author>
    [Serializable]
    public class StringCompareFunction : IFunction
    {
        public const String STRING_COMPARE = "str-compare";

        /// <summary> 
        /// </summary>
        public StringCompareFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.INTEGER_OBJECT; }
        }

        public virtual String Name
        {
            get { return STRING_COMPARE; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam), typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            int eq = - 1;
            if (params_Renamed != null && params_Renamed.Length == 2)
            {
                String val = params_Renamed[0].StringValue;
                String val2 = params_Renamed[1].StringValue;
                eq = val.CompareTo(val2);
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.INTEGER_OBJECT, eq);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(str-compare [string] [string])";
        }

        #endregion
    }
}