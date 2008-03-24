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
    public class MemberTestFunction : IFunction
    {
        public const String MEMBERTEST = "member$";

        /// <summary> 
        /// </summary>
        public MemberTestFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.INTEGER_OBJECT; }
        }

        public virtual String Name
        {
            get { return MEMBERTEST; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (BoundParam), typeof (BoundParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            DefaultReturnVector ret = new DefaultReturnVector();
            int index = - 1;
            if (params_Renamed != null && params_Renamed.Length == 2)
            {
                Object first = params_Renamed[0].Value;
                Object[] second = (Object[]) params_Renamed[1].Value;
                for (int idx = 0; idx < second.Length; idx++)
                {
                    if (first.Equals(second[idx]))
                    {
                        index = idx;
                        break;
                    }
                }
            }
            if (index > - 1)
            {
                DefaultReturnValue rv = new DefaultReturnValue(Constants.INTEGER_OBJECT, index);
                ret.addReturnValue(rv);
            }
            else
            {
                DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, false);
                ret.addReturnValue(rv);
            }
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(member$ <expression> <multifield-expression>)\n" + "Function description:\n" + "\tCompares an expression against a multifield-expression." + "\n\tIf the single expression is in the second expression it," + "\n\treturns the integer position.";
        }

        #endregion
    }
}