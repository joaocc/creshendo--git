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
    /// EchoFunction is used to echo variable bindings in the shell.
    /// 
    /// </author>
    [Serializable]
    public class EchoFunction : IFunction
    {
        public const String ECHO = "echo";

        /// <summary> 
        /// </summary>
        public EchoFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.STRING_TYPE; }
        }

        public virtual String Name
        {
            get { return ECHO; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ShellBoundParam[])}; }
        }


        /// <summary> The method expects an array of ShellBoundParam. The method will use
        /// StringBuffer to resolve the binding and print out 1 binding per
        /// line.
        /// </summary>
        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            StringBuilder buf = new StringBuilder();
            for (int idx = 0; idx < params_Renamed.Length; idx++)
            {
                if (params_Renamed[idx] is ShellBoundParam)
                {
                    ShellBoundParam bp = (ShellBoundParam) params_Renamed[idx];
                    bp.resolveBinding(engine);
                    buf.Append(bp.StringValue + Constants.LINEBREAK);
                }
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.STRING_TYPE, buf.ToString());
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(echo");
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[idx];
                        buf.Append(" ?" + bp.VariableName);
                    }
                    else
                    {
                        buf.Append(" \"" + params_Renamed[idx].StringValue + "\"");
                    }
                }
                buf.Append(")");
                return buf.ToString();
            }
            else
            {
                return "(echo [parameter])";
            }
        }

        #endregion
    }
}