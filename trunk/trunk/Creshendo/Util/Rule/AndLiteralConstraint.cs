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
    public class AndLiteralConstraint : IConstraint
    {
        protected internal String name;
        protected internal bool negated = false;
        protected internal List<object> value_Renamed;

        /// <summary> 
        /// </summary>
        public AndLiteralConstraint() 
        {
            InitBlock();
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

            set
            {
                if (value is List<Object>)
                {
                    value_Renamed = (List<Object>) value;
                }
            }
        }

        public virtual String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            IEnumerator itr = value_Renamed.GetEnumerator();
            buf.Append("    (" + name + " ");
            int count = 0;
            while (itr.MoveNext())
            {
                MultiValue mv = (MultiValue) itr.Current;
                if (count > 0)
                {
                    buf.Append("&");
                }
                if (mv.Negated)
                {
                    buf.Append("~" + ConversionUtils.formatSlot(mv.Value));
                }
                else
                {
                    buf.Append(ConversionUtils.formatSlot(mv.Value));
                }
                count++;
            }
            buf.Append(")" + Constants.LINEBREAK);
            return buf.ToString();
        }

        #endregion

        private void InitBlock()
        {
            value_Renamed = new List<Object>();
        }


        public virtual void addValue(MultiValue mv)
        {
            value_Renamed.Add(mv);
        }

        public virtual void addValues(IList list)
        {
            foreach (object o in list)
            {
                value_Renamed.Add(o);
            }
            
        }
    }
}