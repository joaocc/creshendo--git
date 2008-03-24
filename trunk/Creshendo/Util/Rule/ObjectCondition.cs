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
using System.Text;
using Creshendo.Util.Rete;
using Creshendo.Util.Rete.Compiler;

namespace Creshendo.Util.Rule
{
    /// <author>  Peter Lin
    /// *
    /// ObjectCondition is equivalent to RuleML 0.83 resourceType. ObjectCondition
    /// matches on the fields of an object. The patterns may be simple value
    /// comparisons, or joins against other objects.
    /// 
    /// </author>
    public class ObjectCondition : AbstractCondition
    {
        //hasNotEqual and hasPredicateJoin determine which kind of joinNode to create
        private bool hasNotEqual = false;

        private bool hasPredicateJoin = false;

        /// <summary> 
        /// </summary>
        public ObjectCondition() 
        {
        }

        public virtual bool HasNotEqual
        {
            get { return hasNotEqual; }

            set { hasNotEqual = value; }
        }

        public virtual bool HasPredicateJoin
        {
            get { return hasPredicateJoin; }

            set { hasPredicateJoin = value; }
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
            bool obind = false;
            IConstraint cn = (IConstraint) constraints[0];
            if (cn is BoundConstraint)
            {
                BoundConstraint bc = (BoundConstraint) cn;
                if (bc.IsObjectBinding)
                {
                    start = 1;
                    buf.Append(bc.toFactBindingPPString());
                    // since the first Constraint is a fact binding we
                    // change the padding to 1 space
                    pad = " ";
                    obind = true;
                }
            }
            if (negated)
            {
                buf.Append(pad + "(not" + Constants.LINEBREAK);
                pad = "    ";
            }
            buf.Append(pad + "(" + templateName + Constants.LINEBREAK);
            for (int idx = start; idx < constraints.Count; idx++)
            {
                IConstraint cnstr = (IConstraint) constraints[idx];
                if (negated)
                {
                    buf.Append("  " + cnstr.toPPString());
                }
                else
                {
                    buf.Append(cnstr.toPPString());
                }
            }
            if (negated)
            {
                buf.Append(pad + ")" + Constants.LINEBREAK);
                pad = "  ";
            }
            if (obind && !negated)
            {
                buf.Append(pad + " )" + Constants.LINEBREAK);
            }
            else
            {
                buf.Append(pad + ")" + Constants.LINEBREAK);
            }
            return buf.ToString();
        }

        public override IConditionCompiler getCompiler(IRuleCompiler ruleCompiler)
        {
            CompilerProvider.getInstance(ruleCompiler);
            return CompilerProvider.objectConditionCompiler;
        }
    }
}