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

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// BoundParam is a parameter that is a binding. The test node will need to
    /// call setFact(Fact[] facts) so the parameter can access the value.
    /// 
    /// </author>
    public class BoundParam : AbstractParam
    {
        /// <summary> By default the action is assert
        /// </summary>
        //UPGRADE_NOTE: The initialization of  'actionType' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        protected internal int actionType;

        /// <summary> Column refers to the column of the fact. the value of the column
        /// should be a non-negative integer.
        /// </summary>
        protected internal int column = - 1;

        /// <summary> The fact
        /// </summary>
        protected internal IFact fact = null;

        /// <summary> if the binding is for a multislot, it should be
        /// set to true. by default, it is false.
        /// </summary>
        protected internal bool isMultislot_Renamed_Field = false;

        protected internal Object resolvedVal = null;

        /// <summary> the row id of the fact as defined by the rule
        /// </summary>
        protected internal int rowId = - 1;

        /// <summary> the int value defining the valueType
        /// </summary>
        protected internal int valueType = - 1;

        /// <summary> the name of the variable
        /// </summary>
        protected internal String variableName = null;

        private bool objBinding;

        public BoundParam() 
        {
            InitBlock();
        }

        /// <summary> 
        /// </summary>
        public BoundParam(int col, int vType) 
        {
            InitBlock();
            column = col;
            valueType = vType;
            objBinding = true;
        }

        public BoundParam(int col, int vType, bool objBinding) 
        {
            InitBlock();
            column = col;
            valueType = vType;
            this.objBinding = objBinding;
        }

        public BoundParam(int row, int col, int vType, bool obj) 
        {
            InitBlock();
            rowId = row;
            column = col;
            valueType = vType;
            objBinding = obj;
        }

        public BoundParam(IFact fact)
        {
            InitBlock();
            this.fact = fact;
            objBinding = true;
            valueType = Constants.FACT_TYPE;
        }

        public virtual String VariableName
        {
            get { return variableName; }

            set
            {
                if (value.Substring(0, (1) - (0)).Equals("?"))
                {
                    variableName = value.Substring(1);
                }
                else
                {
                    variableName = value;
                }
            }
        }

        /// <summary> Get the value type
        /// </summary>
        public override int ValueType
        {
            set { }
            get { return valueType; }
        }

        /// <summary> Get the value of the given slot
        /// </summary>
        public override Object Value
        {
            set { }
            get
            {
                if (fact != null)
                {
                    return fact.getSlotValue(column);
                }
                else
                {
                    return resolvedVal;
                }
            }
        }

        public virtual Object ResolvedValue
        {
            set { resolvedVal = value; }
        }

        /// <summary> Return the fact
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual IFact Fact
        {
            get { return fact; }
        }

        //UPGRADE_TODO: Method 'setFact' was converted to a set modifier. This name conflicts with another property. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1137"'
        /// <summary>
        /// The TestNode should call this method to set the fact. The fact should
        /// never be null, since it has to have matched preceding patterns. We
        /// may be able to Remove the check for null. If the row id is less than
        /// zero, it means the binding is an object binding.
        /// </summary>
        /// <value>The facts.</value>
        public virtual IFact[] Facts
        {
            set
            {
                if (rowId > - 1 && value[rowId] != null)
                {
                    fact = value[rowId];
                }
            }
        }

        /// <summary> The method will return the Object instance for the given shadow fact
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual Object ObjectRef
        {
            get { return fact.ObjectInstance; }
        }

        /// <summary>
        /// if the binding is bound to an object, the method will return true.
        /// By default, the method will return false.
        /// </summary>
        /// <value></value>
        public override bool ObjectBinding
        {
            get { return objBinding; }
        }


        public void setObjectBinding(bool val)
        {
            objBinding = val;
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

        /// <summary>
        /// only set the multislot to true if the slot is defined
        /// as a multislot
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is multislot; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsMultislot
        {
            set { isMultislot_Renamed_Field = value; }
        }

        /// <summary>
        /// In some cases, we need to know what the action for the parameter.
        /// </summary>
        /// <value>The type of the action.</value>
        /// <returns>
        /// </returns>
        public virtual int ActionType
        {
            get { return actionType; }

            set { actionType = value; }
        }

        public virtual int Row
        {
            set { rowId = value; }
        }

        public virtual int Column
        {
            set
            {
                column = value;
                if (column == - 1)
                {
                    objBinding = true;
                }
            }
        }

        private void InitBlock()
        {
            actionType = Constants.ACTION_ASSERT;
        }


        /// <summary> method will try to resolve the variable and return the value.
        /// </summary>
        public override Object getValue(Rete engine, int valueType)
        {
            if (fact != null)
            {
                return fact.getSlotValue(column);
            }
            else
            {
                return engine.getBinding(variableName);
            }
        }


        /// <summary> reset sets the Fact handle to null
        /// </summary>
        public override void reset()
        {
            fact = null;
        }

        public virtual String toPPString()
        {
            if (isMultislot_Renamed_Field)
            {
                return "$?" + variableName;
            }
            else
            {
                return "?" + variableName;
            }
        }
    }
}