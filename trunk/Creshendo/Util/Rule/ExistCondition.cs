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
using Creshendo.Util.Rete.Compiler;

namespace Creshendo.Util.Rule
{
    /// <author>  Peter Lin
    /// *
    /// ExistCondition for existential quantifier.
    /// TODO - maybe we should just have ExistCondition extend ObjectCondition
    /// 
    /// </author>
    public class ExistCondition : ObjectCondition
    {
        /// <summary> 
        /// </summary>
        public ExistCondition() 
        {
        }

        public override String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            int start = 0;
            String pad = "  ";
            buf.Append(pad + "(exists" + Constants.LINEBREAK);
            pad = "    ";
            buf.Append(pad + "(" + TemplateName + Constants.LINEBREAK);
            for (int idx = start; idx < Constraints.Length; idx++)
            {
                IConstraint cnstr = (IConstraint) Constraints[idx];
                buf.Append("  " + cnstr.toPPString());
            }
            buf.Append(pad + ")" + Constants.LINEBREAK);
            pad = "  ";
            buf.Append(pad + ")" + Constants.LINEBREAK);
            return buf.ToString();
        }

        public override IConditionCompiler getCompiler(IRuleCompiler ruleCompiler)
        {
            CompilerProvider.getInstance(ruleCompiler);
            return CompilerProvider.existConditionCompiler;
        }

        public static ExistCondition newExistCondition(ObjectCondition cond)
        {
            ExistCondition exc = new ExistCondition();
            exc.constraints = cond.constraints;
            exc.negated = cond.negated;
            exc.nodes = cond.nodes;
            exc.template = cond.template;
            exc.templateName = cond.templateName;
            return exc;
        }
    }
}