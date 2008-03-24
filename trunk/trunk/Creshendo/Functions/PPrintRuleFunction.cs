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
using Creshendo.Util.Rule;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// 
    /// The function will print out the rule in a pretty format. Note the
    /// format may not be identicle to what the user wrote. It is a normalized
    /// and cleaned up format.
    /// 
    /// </author>
    public class PPrintRuleFunction : IFunction
    {
        public const String PPRULES = "ppdefrule";

        /// <summary> 
        /// </summary>
        public PPrintRuleFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return PPRULES; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    IRule rls = engine.CurrentFocus.findRule(params_Renamed[idx].StringValue);
                    engine.writeMessage(rls.toPPString(), "t");
                }
            }
            DefaultReturnVector rv = new DefaultReturnVector();
            return rv;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            StringBuilder buf = new StringBuilder();
            return buf.ToString();
        }

        #endregion
    }
}