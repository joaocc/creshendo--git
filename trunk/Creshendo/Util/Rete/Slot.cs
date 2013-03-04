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
    /// Slot is similar to CLIPS slots, though slightly different.
    /// 
    /// 
    /// </author>
    public class Slot : AbstractSlot
    {
        private Object _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Slot"/> class.
        /// </summary>
        public Slot()
        {
            InitBlock();
        }

        /// <summary>
        /// Create a new instance with a given name
        /// </summary>
        /// <param name="name">The name.</param>
        public Slot(String name) : base(name)
        {
            InitBlock();
        }

        /// <summary>
        /// For convenience you can create here a slot with a given value directly
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="val">The val.</param>
        public Slot(String name, Object val) : base(name)
        {
            InitBlock();
            this._value = val;
        }

        /// <summary>
        /// Get the value of the slot
        /// </summary>
        /// <value>The value.</value>
        /// <returns>
        /// </returns>
        public virtual Object Value
        {
            get { return _value; }

            set
            {
                _value = value;
                if (ValueType < 0)
                {
                    inspectType();
                }
            }
        }

        ///// <summary>
        ///// In some cases, a deftemplate can be define with a
        ///// default value.
        ///// </summary>
        ///// <value>The default value.</value>
        ///// <param name="">value
        ///// </param>
        //public virtual Object DefaultValue
        //{
        //    set { _value = value; }
        //}

        private void InitBlock()
        {
            _value = Constants.NIL_SYMBOL;
        }


        /// <summary> method will look at the value and set the int type
        /// </summary>
        protected internal virtual void inspectType()
        {
            if (_value is Double)
            {
                ValueType = Constants.DOUBLE_PRIM_TYPE;
            }
            else if (_value is Int64)
            {
                ValueType = Constants.LONG_PRIM_TYPE;
            }
            else if (_value is Single)
            {
                ValueType = Constants.FLOAT_PRIM_TYPE;
            }
            else if (_value is Int16)
            {
                ValueType = Constants.SHORT_PRIM_TYPE;
            }
            else if (_value is Int32)
            {
                ValueType = Constants.INT_PRIM_TYPE;
            }
            else
            {
                ValueType = Constants.OBJECT_TYPE;
            }
        }

        /// <summary> A convienance method to clone slots
        /// </summary>
        public override Object Clone()
        {
            Slot newslot = new Slot();
            newslot.Id = Id;
            newslot.Name = Name;
            newslot._value = _value;
            newslot.ValueType = ValueType;
            return newslot;
        }

        public virtual String valueToString()
        {
            if (ValueType == Constants.STRING_TYPE)
            {
                return "\"" + _value + "\"";
            }
            else
            {
                return _value.ToString();
            }
        }
    }
}