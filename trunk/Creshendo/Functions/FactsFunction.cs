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

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// 
    /// Facts function will printout all the facts, not including any
    /// initial facts which are internal to the rule engine.
    /// 
    /// </author>
    [Serializable]
    public class FactsFunction : IFunction
    {
        public const String FACTS = "facts";

        /// <summary> 
        /// </summary>
        public FactsFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return FACTS; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[0]; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            IList<object> facts = engine.AllFacts;
            Object[] sorted = FactUtils.sortFacts(facts);
            for (int idx = 0; idx < sorted.Length; idx++)
            {
                IFact ft = (IFact) sorted[idx];
                engine.writeMessage(ft.toFactString() + Constants.LINEBREAK);
            }
            engine.writeMessage("for a total of " + sorted.Length + Constants.LINEBREAK, Constants.DEFAULT_OUTPUT);
            return new DefaultReturnVector();
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(facts)\n" + "Function description:\n" + "\tPrints all facts except the initial facts.";
        }

        #endregion
    }
}