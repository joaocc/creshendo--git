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
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <author>  Nikolaus Koemm
    /// 
    /// 
    /// </author>
    [Serializable]
    public class NeqFunction : IFunction
    {
        public const String NEQUAL = "neq";

        /// <summary> 
        /// </summary>
        public NeqFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return NEQUAL; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam[])}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            DefaultReturnVector ret = new DefaultReturnVector();
            bool eq = true;
            if (params_Renamed != null && params_Renamed.Length > 1)
            {
                Object first = params_Renamed[0].getValue(engine, Constants.OBJECT_TYPE);
                for (int idx = 1; idx < params_Renamed.Length; idx++)
                {
                    Object other = params_Renamed[idx].getValue(engine, Constants.OBJECT_TYPE);
                    if (((first == null && other == null) || (first != null && first.Equals(other))))
                    {
                        eq = false;
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
            return "(neq (<literal> | <binding>)+)\n" + "Function description:\n" + "\tCompares a literal value against one or more" + "bindings. \n\tIf all of the bindings are equal to the constant value," + "\n\tthe function returns true.";
        }

        #endregion
    }
}