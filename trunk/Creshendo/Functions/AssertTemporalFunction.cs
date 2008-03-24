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
using Creshendo.Util.Rete.Exception;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// *
    /// 
    /// </author>
    [Serializable]
    public class AssertTemporalFunction : IFunction
    {
        public const String ASSERT_TEMPORAL = "assert-temporal";

        protected internal IFact[] triggerFacts = null;

        public virtual IFact[] TriggerFacts
        {
            set { triggerFacts = value; }
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.STRING_TYPE; }
        }

        public virtual String Name
        {
            get { return ASSERT_TEMPORAL; }
        }

        /// <summary> The expected parameter is a deffact instance. According to CLIPS
        /// beginner guide, assert only takes facts and returns the id of the
        /// fact. For objects, there's (assert-object ?binding).
        /// </summary>
        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            String asrt = "";
            if (params_Renamed.Length > 0)
            {
                Deffact fact = null;
                if (params_Renamed[0].Value is Deffact)
                {
                    fact = (Deffact) params_Renamed[0].Value;
                }
                else
                {
                    Deftemplate tmpl = (Deftemplate) engine.CurrentFocus.getTemplate(params_Renamed[0].StringValue);
                    // before we create the fact, we need to Remove the four
                    // slots for temporal facts
                    fact = (Deffact) tmpl.createTemporalFact((Object[]) params_Renamed[1].Value, - 1);
                }
                if (fact.hasBinding())
                {
                    fact.resolveValues(engine, triggerFacts);
                    fact = fact.cloneFact();
                }
                try
                {
                    engine.assertFact(fact);
                    // if the fact id is still -1, it means it wasn't asserted
                    // if it was asserted, we return the fact id, otherwise
                    // we return "false".
                    if (fact.FactId > 0)
                    {
                        asrt = fact.FactId.ToString();
                    }
                    else
                    {
                        asrt = "false";
                    }
                }
                catch (AssertException e)
                {
                    // we should log this and output an error
                    asrt = "false";
                }
            }
            else
            {
                asrt = "false";
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.STRING_TYPE, asrt);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(assert-temporal ");
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    // the parameter should be a deffact
                    Deffact fact = (Deffact) params_Renamed[idx].Value;
                    buf.Append(fact.toPPString());
                }
                buf.Append(" )");
                return buf.ToString();
            }
            else
            {
                return "(assert-temporal [deffact])";
            }
        }

        #endregion
    }
}