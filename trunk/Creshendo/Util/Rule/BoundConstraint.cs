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
    /// BoundConstraint is a basic implementation of Constraint interface
    /// for bound constraints. When a rule declares a slot as a binding,
    /// a BoundConstraint is used.
    /// 
    /// </author>
    public class BoundConstraint : IConstraint
    {
        protected internal bool firstDeclaration_Renamed_Field = false;

        /// <summary> if the binding is for a multislot, it should be
        /// set to true. by default, it is false.
        /// </summary>
        protected internal bool isMultislot_Renamed_Field = false;

        protected internal bool isObjectBinding = false;

        /// <summary> the name is the slot name
        /// </summary>
        protected internal String name;

        protected internal bool negated = false;

        private bool intraFactJoin = false;
        protected List<Object> ifjoins = new List<Object>();

        /// <summary> In the case of BoundConstraints, the value is the name of
        /// the variable given my the user
        /// </summary>
        protected internal Object value_Renamed;

        /// <summary> 
        /// </summary>
        public BoundConstraint() 
        {
        }

        public BoundConstraint(String name, bool isObjBind) 
        {
            Name = name;
            isObjectBinding = isObjBind;
        }

        public virtual String VariableName
        {
            get { return (String) value_Renamed; }
        }

        /// <summary> if the binding is to an object or deffact, the method will
        /// return true.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> Set the constraint to true if the binding is for an object or
        /// a deffact.
        /// </summary>
        /// <param name="">obj
        /// 
        /// </param>
        public virtual bool IsObjectBinding
        {
            get { return isObjectBinding; }

            set { isObjectBinding = value; }
        }

        /// <summary> if the binding is for a multislot, it will return true.
        /// by default is is false.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool Multislot
        {
            get { return isMultislot_Renamed_Field; }
        }

        /// <summary> only set the multislot to true if the slot is defined
        /// as a multislot
        /// </summary>
        /// <param name="">multi
        /// 
        /// </param>
        public virtual bool IsMultislot
        {
            set { isMultislot_Renamed_Field = value; }
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

        public virtual bool FirstDeclaration
        {
            set { firstDeclaration_Renamed_Field = value; }
        }

        #region Constraint Members

        /// <summary> The name of the slot or object field.
        /// </summary>
        /// <summary> the name is the name of the slot or object field.
        /// </summary>
        public virtual String Name
        {
            get { return name; }

            set
            {
                if (value.StartsWith("?"))
                {
                    name = value.Substring(1);
                }
                else
                {
                    name = value;
                }
            }
        }

        /// <summary> The value is the name of the variable. In the case of CLIPS,
        /// if the rule as "?name", the value returned is "name" without
        /// the question mark prefix.
        /// </summary>
        /// 
        /// <summary> The input parameter should be a string and it should be
        /// the name of the variable. Make sure to parse out the
        /// prefix. For example, CLIPS uses "?" to denote a variable.
        /// </summary>
        public virtual Object Value
        {
            get { return value_Renamed; }

            set { value_Renamed = value; }
        }

        public bool IntraFactJoin
        {
            get { return intraFactJoin; }
            set { intraFactJoin = value; }
        }

        /// <summary> returns the constriant in a pretty printer format
        /// </summary>
        public virtual String toPPString()
        {
            if (isMultislot_Renamed_Field)
            {
                return "    (" + name + " $?" + value_Renamed.ToString() + ")" + Constants.LINEBREAK;
            }
            else if (negated)
            {
                return "    (" + name + " ~?" + value_Renamed.ToString() + ")" + Constants.LINEBREAK;
            }
            else
            {
                return "    (" + name + " ?" + value_Renamed.ToString() + ")" + Constants.LINEBREAK;
            }
        }

        #endregion

        /// <summary> by default the method returns false, unless it is set to true
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool firstDeclaration()
        {
            return firstDeclaration_Renamed_Field;
        }

        public void addIntrFactJoin(IList list)
        {
            foreach (BoundConstraint bc in list)
            {
                bc.Name = this.name;
                ifjoins.Add(bc);
            }
        }

        public BoundConstraint getFirstIFJ()
        {
            return (BoundConstraint)this.ifjoins[0];
        }

        public virtual String toFactBindingPPString()
        {
            return "  ?" + value_Renamed.ToString() + " <-";
        }
    }
}