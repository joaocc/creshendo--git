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
using Creshendo.Util.Rete;

namespace Creshendo.Util.Rule
{
    /// <author>  Peter Lin
    /// *
    /// Predicate constraint binds the slot and then performs some function
    /// on it. For example 
    /// 
    /// 
    /// </author>
    public class PredicateConstraint : IConstraint
    {
        /// <summary> the name of the function
        /// </summary>
        protected internal String functionName = null;

        protected internal bool isPredicateJoin_Renamed_Field = false;

        /// <summary> the name of the slot
        /// </summary>
        protected internal String name = null;

        protected internal List<object> parameters;
        protected internal Object value_Renamed = null;

        /// <summary> the name of the variable
        /// </summary>
        protected internal String varName = null;

        /// <summary> 
        /// </summary>
        public PredicateConstraint() 
        {
            InitBlock();
        }

        public virtual String VariableName
        {
            get { return varName; }

            set { varName = value; }
        }

        public virtual String FunctionName
        {
            get { return functionName; }

            set { functionName = value; }
        }

        public virtual bool PredicateJoin
        {
            get { return isPredicateJoin_Renamed_Field; }
        }

        public virtual IList Parameters
        {
            get { return parameters; }
        }

        #region Constraint Members

        public virtual String Name
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rule.Constraint#getName()
			*/

            get { return name; }
            /* (non-Javadoc)
			* @see woolfel.engine.rule.Constraint#setName(java.lang.String)
			*/

            set { name = value; }
        }

        public virtual Object Value
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rule.Constraint#getValue()
			*/

            get { return value_Renamed; }
            /* (non-Javadoc)
			* @see woolfel.engine.rule.Constraint#setValue(java.lang.Object)
			*/

            set { value_Renamed = value; }
        }

        public virtual String toPPString()
        {
            if (value_Renamed is BoundParam)
            {
                return "    (" + name + " ?" + varName + "&:(" + functionName + " ?" + varName + " " + ((BoundParam) value_Renamed).toPPString() + ") )" + Constants.LINEBREAK;
            }
            else
            {
                return "    (" + name + " ?" + varName + "&:(" + functionName + " ?" + varName + " " + value_Renamed.ToString() + ") )" + Constants.LINEBREAK;
            }
        }

        #endregion

        private void InitBlock()
        {
            parameters = new List<Object>();
        }


        public virtual void addParameters(IList params_Renamed)
        {
            foreach (AbstractParam param in params_Renamed)
            {
                parameters.Add(param);
            }
            
            int bcount = 0;
            // we try to set the value
            IEnumerator itr = parameters.GetEnumerator();
            while (itr.MoveNext())
            {
                Object p = itr.Current;
                // for now, a simple implementation
                if (p is ValueParam)
                {
                    Value = ((ValueParam) p).Value;
                    break;
                }
                else if (p is BoundParam)
                {
                    BoundParam bp = (BoundParam) p;
                    if (!bp.VariableName.Equals(varName))
                    {
                        Value = p;
                    }
                    bcount++;
                }
            }
            if (bcount > 1)
            {
                isPredicateJoin_Renamed_Field = true;
            }
        }

        public virtual void addParameter(IParameter param)
        {
            parameters.Add(param);
            if (param is ValueParam)
            {
                Value = ((ValueParam) param).Value;
            }
            else if (param is BoundParam && varName == null)
            {
                varName = ((BoundParam) param).VariableName;
            }
        }


        public virtual int parameterCount()
        {
            return parameters.Count;
        }

        /// <summary> the purpose of normalize is to look at the order of the
        /// parameters and flip the operator if necessary
        /// *
        /// </summary>
        public virtual void normalize()
        {
        }
    }
}