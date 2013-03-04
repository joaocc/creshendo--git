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
using System.Text;
using Creshendo.Util;
using Creshendo.Util.Rete;
using Creshendo.Util.Rule;
using Creshendo.Util.Rule.Util;
//using List<Object>=Creshendo.Util.List<Object>;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// *
    /// Generate facts will call the utility class with the Rule object
    /// and return an Object[] array of facts. Depending on the rule,
    /// there should be one or more deffacts or object instances. The way
    /// to use this is to bind the result or Add it to a list.
    /// 
    /// </author>
    [Serializable]
    public class GenerateFactsFunction : IFunction
    {
        public const String GENERATEFACTS = "generate-facts";

        /// <summary> 
        /// </summary>
        public GenerateFactsFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.OBJECT_TYPE; }
        }

        public virtual String Name
        {
            get { return GENERATEFACTS; }
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
            bool echo = false;
            IList<Object> facts = null;
            String output = null;
            if (params_Renamed != null && params_Renamed.Length >= 1)
            {
                Defrule r = (Defrule) engine.CurrentFocus.findRule(params_Renamed[0].StringValue);
                if (params_Renamed.Length == 2)
                {
                    if (params_Renamed[1].BooleanValue)
                    {
                        echo = true;
                    }
                }
                // if there's 3 parameters, it means we should save the fact
                // to a file
                if (params_Renamed.Length == 3)
                {
                    output = params_Renamed[2].StringValue;
                }
                facts = GenerateFacts.generateFacts(r, engine);
                if (facts.Count > 0)
                {
                    if (echo)
                    {
                        IEnumerator itr = facts.GetEnumerator();
                        while (itr.MoveNext())
                        {
                            Object data = itr.Current;
                            if (data is Deffact)
                            {
                                Deffact f = (Deffact) data;
                                engine.writeMessage(f.toFactString());
                            }
                            else
                            {
                                engine.writeMessage(data.ToString());
                            }
                        }
                    }
                    if (output != null)
                    {
                        // we need to save facts to a file
                        IOUtilities.saveFacts(facts, output);
                    }

                    object[] ary = new object[facts.Count];
                    facts.CopyTo(ary,0);
                    DefaultReturnValue rv = new DefaultReturnValue(Constants.OBJECT_TYPE, ary);
                    ret.addReturnValue(rv);
                }
                else
                {
                    DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, false);
                    ret.addReturnValue(rv);
                }
            }
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
                buf.Append("(generate-facts)");
                return buf.ToString();
            }
            else
            {
                return "(generate-facts [<rule> [true | false] [output])\n" + "Function description:\n" + "\tGenerates the trigger facts for a single rule\n";
            }
        }

        #endregion
    }
}