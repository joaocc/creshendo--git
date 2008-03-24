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
using System.Collections;
using System.Collections.Generic;
using Creshendo.Util;
using Creshendo.Util.Rete;
using Creshendo.Util.Rule;
//using List<Object>=Creshendo.Util.List<Object>;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// 
    /// The purpose of the function is to print out the names of the rules
    /// and the comment.
    /// 
    /// </author>
    [Serializable]
    public class RulesFunction : IFunction
    {
        public const String LISTRULES = "list-defrules";
        public const String RULES = "rules";

        public RulesFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return RULES; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[0]; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            List<Object> rules = (List<Object>) engine.CurrentFocus.AllRules;
            int count = rules.Count;
            IEnumerator itr = rules.GetEnumerator();
            while (itr.MoveNext())
            {
                IRule r = (IRule) itr.Current;
                engine.writeMessage(r.Name + " \"" + r.Comment + "\" salience:" + r.Salience + " version:" + r.Version + " no-agenda:" + r.NoAgenda + "\r\n", "t");
            }
            engine.writeMessage("for a total of " + count + "\r\n", "t");
            DefaultReturnVector rv = new DefaultReturnVector();
            return rv;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(rules)";
        }

        #endregion
    }
}