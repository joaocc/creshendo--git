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

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// 
    /// BindFunction is responsible for calling the appropriate method in Rete to
    /// create the defglobal.
    /// 
    /// </author>
    [Serializable]
    public class BindFunction : IFunction
    {
        public const String BIND = "bind";

        /// <summary> 
        /// </summary>
        public BindFunction() 
        {
        }

        #region Function Members

        /// <summary> the return type is Boolean. If the function was successful, it returns
        /// true. Otherwise it returns false.
        /// </summary>
        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return BIND; }
        }

        /// <summary> The function takes 2 parameters. The first is the name of the variable
        /// and the second is some value. At the moment, the function does not hand
        /// </summary>
        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam), typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            bool bound = false;
            if (params_Renamed.Length == 2)
            {
                String name = params_Renamed[0].StringValue;
                Object val = null;
                if (params_Renamed[1] is ValueParam)
                {
                    val = params_Renamed[1].Value;
                }
                else if (params_Renamed[1] is FunctionParam2)
                {
                    FunctionParam2 fp2 = (FunctionParam2) params_Renamed[1];
                    fp2.Engine = engine;
                    fp2.lookUpFunction();
                    DefaultReturnVector drv = (DefaultReturnVector) fp2.Value;
                    val = drv.firstReturnValue().Value;
                }
                engine.setBindingValue(name, val);
                bound = true;
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, bound);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(bind ?" + params_Renamed[0].StringValue);
                for (int idx = 1; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is ValueParam)
                    {
                        buf.Append(" " + params_Renamed[idx].StringValue);
                    }
                    else if (params_Renamed[idx] is FunctionParam2)
                    {
                        FunctionParam2 fp2 = (FunctionParam2) params_Renamed[idx];
                        buf.Append(" " + fp2.toPPString());
                    }
                }
                buf.Append(" )");
                return buf.ToString();
            }
            else
            {
                return "(bind ?<variable-name> <expression>)\n" + "Function description:\n" + "\tBinds the value of the argument <expression> to the \n" + "\tvariable <variable-name>.";
            }
        }

        #endregion
    }
}