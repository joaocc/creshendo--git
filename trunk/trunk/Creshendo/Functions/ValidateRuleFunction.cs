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
    /// 
    /// WatchFunction allows users to watch different engine process, like
    /// activations, facts and rules.
    /// 
    /// </author>
    [Serializable]
    public class ValidateRuleFunction : IFunction
    {
        protected internal const String VALIDATE_RULE = "validate-rule";

        /// <summary> 
        /// </summary>
        public ValidateRuleFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return VALIDATE_RULE; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            DefaultReturnVector ret = new DefaultReturnVector();
            bool val = false;
            if (params_Renamed != null && params_Renamed.Length == 1)
            {
                if (params_Renamed[0].BooleanValue)
                {
                    engine.ValidateRules = true;
                    val = true;
                }
                else if (!params_Renamed[0].BooleanValue)
                {
                    engine.ValidateRules = false;
                    val = true;
                }
            }
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, val);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(validate-rule [true|false])";
        }

        #endregion
    }
}