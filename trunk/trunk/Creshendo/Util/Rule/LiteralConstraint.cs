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
using Creshendo.Util.Rete;

namespace Creshendo.Util.Rule
{
    /// <author>  Peter Lin
    /// *
    /// Literal constraint is a comparison between an object field and a concrete
    /// value. for example, account.name is equal to "Peter Lin". I originally,
    /// named the class something else, but since CLIPS uses literal constraint,
    /// I decided to change the name of the class. Even though I don't like the
    /// term literal constraint, it doesn't make sense to fight existing
    /// terminology.
    /// 
    /// </author>
    public class LiteralConstraint : IConstraint
    {
        protected internal String name;
        protected internal bool negated = false;
        protected internal Object value_Renamed;

        /// <summary> 
        /// </summary>
        public LiteralConstraint() 
        {
        }

        /// <summary> if the literal constraint is negated, the method returns true
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> if the literal constraint is negated with a "~" tilda, call
        /// the method pass true.
        /// </summary>
        /// <param name="">negate
        /// 
        /// </param>
        public virtual bool Negated
        {
            get { return negated; }

            set { negated = value; }
        }

        #region Constraint Members

        /// <summary> the name is the slot name
        /// </summary>
        /// <summary> set the slot name as declared in the rule
        /// </summary>
        public virtual String Name
        {
            get { return name; }

            set { name = value; }
        }

        /// <summary> Set the value of the constraint. It should be a concrete value and
        /// not a binding.
        /// </summary>
        public virtual Object Value
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rule.Constraint#getValue()
			*/

            get { return value_Renamed; }

            set { value_Renamed = value; }
        }


        public virtual String toPPString()
        {
            if (negated)
            {
                return "    (" + name + " ~" + value_Renamed.ToString() + ")" + Constants.LINEBREAK;
            }
            else
            {
                return "    (" + name + " " + value_Renamed.ToString() + ")" + Constants.LINEBREAK;
            }
        }

        #endregion
    }
}