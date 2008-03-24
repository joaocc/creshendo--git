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
    /// *
    /// 
    /// </author>
    [Serializable]
    public class DefruleFunction : IFunction
    {
        public const String DEFRULE = "defrule";

        public DefruleFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return DEFRULE; }
        }

        /// <summary> the input parameter is a single ValueParam containing a Defrule
        /// instance.
        /// </summary>
        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            bool add = true;
            if (params_Renamed.Length == 1 && params_Renamed[0].Value is Defrule)
            {
                Defrule rl = (Defrule) params_Renamed[0].Value;
                if (!engine.CurrentFocus.containsRule(rl))
                {
                    add = engine.RuleCompiler.addRule(rl);
                }
            }
            else
            {
                add = false;
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, add);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null)
            {
                StringBuilder buf = new StringBuilder();
                return buf.ToString();
            }
            else
            {
                return "(defrule <rule-name> (declare (properties)+?) (CE)+ => ([function]))" + "" + "(defrule <rule-name> \"optional_comment\" " + "	(pattern_1) 		; Left-Hand Side (LHS)" + "	(pattern_2) 		; of the rule consisting of elements" + "	...					; before the \"=>\"" + "	...					" + "	...					" + "	(pattern_N)" + "	=>" + "	(action_1) 			; Right-Hand Side (RHS)" + "	(action_2) 			; of the rule consisting of elements" + "	...					; after the \"=>\"" + "	...					" + "	...					" + "	(action_M)) 		; The last \")\" balances the opening" + "						; \")\" to the left of \"defrule\"." + "" + "Be sure all your parentheses balance or you will Get error messages!";
            }
        }

        #endregion
    }
}