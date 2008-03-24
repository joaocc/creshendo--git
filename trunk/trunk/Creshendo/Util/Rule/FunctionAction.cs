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
using Creshendo.Functions;
using Creshendo.Util.Rete;

namespace Creshendo.Util.Rule
{
    /// <author>  Peter Lin
    /// *
    /// A FunctionAction is responsible for executing a function in the action
    /// of the rule. It uses built-in or user written functions. When the rule
    /// is loaded, the engine looks up the functions. At run time, the rule
    /// simply executes it.
    /// 
    /// </author>
    public class FunctionAction : IAction
    {
        protected internal IFunction faction = null;
        protected internal String functionName = null;
        protected internal IParameter[] parameters = null;

        /// <summary> 
        /// </summary>
        public FunctionAction() 
        {
        }

        public virtual IFunction Function
        {
            get { return faction; }

            set
            {
                if (value is ShellFunction)
                {
                    ShellFunction sf = (ShellFunction) value;
                    functionName = sf.Name;
                    parameters = sf.Parameters;
                }
                else
                {
                    faction = value;
                    functionName = value.Name;
                }
            }
        }

        public virtual String FunctionName
        {
            get { return functionName; }

            set { functionName = value; }
        }

        public virtual IParameter[] Parameters
        {
            get { return parameters; }

            set { parameters = value; }
        }

        #region Action Members

        /// <summary> Configure will lookup the function and set it
        /// </summary>
        public virtual void configure(Rete.Rete engine, IRule util)
        {
            if (functionName != null && engine.findFunction(functionName) != null)
            {
                faction = engine.findFunction(functionName);
            }
            // now setup the BoundParameters if there are any
            for (int idx = 0; idx < parameters.Length; idx++)
            {
                if (parameters[idx] is BoundParam)
                {
                    BoundParam bp = (BoundParam) parameters[idx];
                    Binding bd = util.getBinding(bp.VariableName);
                    if (bd != null)
                    {
                        bp.Row = bd.LeftRow;
                        bp.Column = bd.LeftIndex;
                    }
                }
                else if (parameters[idx] is FunctionParam2)
                {
                    FunctionParam2 fp2 = (FunctionParam2) parameters[idx];
                    fp2.configure(engine, util);
                }
                else if (parameters[idx] is ValueParam)
                {
                    ValueParam vp = (ValueParam) parameters[idx];
                    // if the value is a deffact, we need to check and make sure
                    // the slots with BoundParam value are compiled properly
                    if (vp.Value is Deffact)
                    {
                        ((Deffact) vp.Value).compileBinding(util);
                    }
                }
            }
            // in the case of Assert, we do further compilation
            if (faction is AssertFunction)
            {
                Deftemplate tmpl = (Deftemplate) engine.CurrentFocus.getTemplate(parameters[0].StringValue);
                Deffact fact = (Deffact) tmpl.createFact((Object[]) parameters[1].Value, - 1);
                fact.compileBinding(util);
                parameters = new ValueParam[1];
                parameters[0] = new ValueParam(Constants.OBJECT_TYPE, fact);
            }
        }

        /* (non-Javadoc)
		* @see woolfel.engine.rule.Action#executeAction(woolfel.engine.Creshendo.Util.Rete.Rete, woolfel.engine.rete.Fact[])
		*/

        public virtual void executeAction(Rete.Rete engine, IFact[] facts)
        {
            // first we iterate over the parameters and pass the facts
            // to the BoundParams.
            for (int idx = 0; idx < parameters.Length; idx++)
            {
                if (parameters[idx] is BoundParam)
                {
                    ((BoundParam) parameters[idx]).Facts = facts;
                }
                else if (parameters[idx] is FunctionParam)
                {
                    ((FunctionParam) parameters[idx]).Facts = facts;
                }
                else if (parameters[idx] is FunctionParam2)
                {
                    ((FunctionParam2) parameters[idx]).Engine = engine;
                }
            }
            // we treat AssertFunction a little different
            if (faction is AssertFunction)
            {
                ((AssertFunction) faction).TriggerFacts = facts;
            }
            else if (faction is ModifyFunction)
            {
                ((ModifyFunction) faction).TriggerFacts = facts;
            }
            // now we find the function
            faction.executeFunction(engine, parameters);
        }

        /// <summary> method implements the necessary logic to print out the action
        /// </summary>
        public virtual String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            /// <summary>buf.append("  (" + this.functionName);
            /// for (int idx=0; idx < this.parameters.length; idx++) {
            /// if (parameters[idx] instanceof BoundParam) {
            /// BoundParam bp = (BoundParam)this.parameters[idx];
            /// buf.append(bp.toPPString());
            /// } else if (parameters[idx] instanceof ValueParam) {
            /// buf.append(" " + this.parameters[idx].getStringValue());
            /// } else if (parameters[idx] instanceof FunctionParam) {
            /// FunctionParam fp = (FunctionParam)parameters[idx];
            /// buf.append(fp.toString());
            /// } else if (parameters[idx] instanceof FunctionParam2) {
            /// FunctionParam2 fp2 = (FunctionParam2)parameters[idx];
            /// buf.append(fp2.getFunctionName());
            /// }
            /// }
            /// buf.append(")" + Constants.LINEBREAK);
            /// *
            /// </summary>
            buf.Append("  " + faction.toPPString(parameters, 1) + Constants.LINEBREAK);
            return buf.ToString();
        }

        #endregion
    }
}