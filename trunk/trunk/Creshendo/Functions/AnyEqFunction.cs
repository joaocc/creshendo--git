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
    /// Any equal is used to compare a literal value against one or more
    /// bindings. If any of the bindings is equal to the constant value,
    /// the function returns true.
    /// 
    /// </author>
    [Serializable]
    public class AnyEqFunction : IFunction
    {
        public const String ANYEQUAL = "any-eq";

        /// <summary> 
        /// </summary>
        public AnyEqFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return ANYEQUAL; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam), typeof (BoundParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            DefaultReturnVector ret = new DefaultReturnVector();
            bool eq = false;
            if (params_Renamed != null && params_Renamed.Length > 1)
            {
                Object constant = params_Renamed[0].getValue(engine, Constants.OBJECT_TYPE);
                for (int idx = 1; idx < params_Renamed.Length; idx++)
                {
                    if (constant.Equals(params_Renamed[idx].getValue(engine, Constants.OBJECT_TYPE)))
                    {
                        eq = true;
                        break;
                    }
                }
            }
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, eq);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(any-eq (<literal> | <binding>)+)\n" + "Function description:\n" + "\tCompares a literal value against one or more" + "bindings. \n\tIf any of the bindings is equal to the constant value," + "\n\tthe function returns true.";
        }

        #endregion
    }
}