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
using System.Text;
using Creshendo.Util.Rete;
using Creshendo.Util.Rete.Compiler;

namespace Creshendo.Util.Rule
{
    /// <author>  Peter Lin
    /// *
    /// TemporalCondition extends AbstractCondition and adds 2 additional
    /// attributes: relativeTime and varname. Since all temporal nodes have
    /// to have both, we make it easier to set and Get.
    /// 
    /// </author>
    public class TemporalCondition : ObjectCondition
    {
        private String function = null;
        private int intervalTime = 0;
        private IParameter[] parameters = null;
        protected internal int relativeTime = 0;
        protected internal String varname = null;

        /// <summary> 
        /// </summary>
        public TemporalCondition()
        {
        }

        public virtual String VariableName
        {
            get { return varname; }

            set { varname = value; }
        }

        public virtual int RelativeTime
        {
            get { return relativeTime; }

            set { relativeTime = value; }
        }

        public string Function
        {
            get { return function; }
            set { function = value; }
        }

        public IParameter[] Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public int IntervalTime
        {
            get { return intervalTime; }
            set { intervalTime = value; }
        }

        public void AddFunction(IList list)
        {
            if (list.Count > 0)
            {
                //Object[] array = list.ToArray();
                ValueParam vp = (ValueParam)list[0];
                function = vp.ToString();
                IParameter[] parms = new IParameter[list.Count - 1];
                for (int idx = 1; idx < list.Count; idx++)
                {
                    parms[idx - 1] = (IParameter)list[idx];
                }
                parameters = parms;
            }
        }

        /// <summary> TODO - currently we don't need it and it isn't implemented.
        /// should finish implementing it.
        /// </summary>
        public override bool compare(ICondition cond)
        {
            return false;
        }

        /// <summary> The current implementation expects the deffact or object binding
        /// constriant to be first.
        /// </summary>
        public override String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            int start = 0;
            // this is a hack, but it keeps the code simple for spacing
            // default indent for CE is 2 spaces
            String pad = "  ";
            buf.Append(pad + "(temporal" + Constants.LINEBREAK);
            if (negated)
            {
                buf.Append(pad + pad + "(relative-time " + relativeTime + ")" + Constants.LINEBREAK);
            }
            else
            {
                buf.Append(pad + pad + "?" + varname + Constants.LINEBREAK);
                buf.Append(pad + pad + "(relative-time " + relativeTime + ")" + Constants.LINEBREAK);
            }
            if (negated)
            {
                buf.Append(pad + pad + "(not" + Constants.LINEBREAK);
                pad = "      ";
            }
            else
            {
                pad = "    ";
            }
            buf.Append(pad + "(" + templateName + Constants.LINEBREAK);
            for (int idx = start; idx < constraints.Count; idx++)
            {
                IConstraint cnstr = (IConstraint) constraints[idx];
                if (negated)
                {
                    buf.Append("    " + cnstr.toPPString());
                }
                else
                {
                    buf.Append("  " + cnstr.toPPString());
                }
            }
            if (negated)
            {
                buf.Append(pad + ")" + Constants.LINEBREAK);
            }
            pad = "  ";
            buf.Append(pad + pad + ")" + Constants.LINEBREAK);
            buf.Append(pad + ")" + Constants.LINEBREAK);
            return buf.ToString();
        }

        public override IConditionCompiler getCompiler(IRuleCompiler ruleCompiler)
        {
            CompilerProvider.getInstance(ruleCompiler);
            return CompilerProvider.temporalConditionCompiler;
        }
    }
}