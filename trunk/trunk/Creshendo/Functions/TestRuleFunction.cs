/*
* Copyright 2002-2007 Peter Lin
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
using System.Text;
using Creshendo.Util;
using Creshendo.Util.Rete;
using Creshendo.Util.Rete.Exception;
using Creshendo.Util.Rule;
using Creshendo.Util.Rule.Util;
//using List<Object>=Creshendo.Util.List<Object>;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// *
    /// ClearFunction will call Rete.Clear()
    /// 
    /// </author>
    [Serializable]
    public class TestRuleFunction : IFunction
    {
        public const String TESTRULE = "test-rule";

        /// <summary> 
        /// </summary>
        public TestRuleFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return TESTRULE; }
        }

        /// <summary> The function does not take any parameters
        /// </summary>
        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            DefaultReturnVector ret = new DefaultReturnVector();
            if (params_Renamed != null && params_Renamed.Length == 1)
            {
                String rlz = params_Renamed[0].StringValue;
                Defrule r = (Defrule) engine.CurrentFocus.findRule(params_Renamed[0].StringValue);
                IList<object> facts = GenerateFacts.generateFacts(r, engine);
                if (facts.Count > 0)
                {
                    try
                    {
                        engine.Watch = WatchType.WATCH_ALL;
                        IEnumerator itr = facts.GetEnumerator();
                        while (itr.MoveNext())
                        {
                            Object data = itr.Current;
                            if (data is Deffact)
                            {
                                engine.assertFact((Deffact) data);
                            }
                            else
                            {
                                engine.assertObject(data, null, false, true);
                            }
                        }
                        engine.fire();
                        engine.UnWatch = WatchType.WATCH_ALL;
                    }
                    catch (AssertException e)
                    {
                        System.Diagnostics.Trace.WriteLine(e.Message);
                    }
                }
            }
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, true);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (indents > 0)
            {
                StringBuilder buf = new StringBuilder();
                for (int idx = 0; idx < indents; idx++)
                {
                    buf.Append(" ");
                }
                buf.Append("(test-rule)");
                return buf.ToString();
            }
            else
            {
                return "(test-rule [rule])\n" + "Function description:\n" + "\tGenerate the facts for a rule, assert them and\n" + "\tcall (fire).\n";
            }
        }

        #endregion
    }
}