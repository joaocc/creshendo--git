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
    /// 
    /// MultiSlot always returns Constants.ARRAY_TYPE. It is the class for array
    /// types.
    /// 
    /// </author>
    public class MultiSlot : Slot
    {
        protected internal new int type;

        /// <summary> 
        /// </summary>
        public MultiSlot() 
        {
            InitBlock();
        }

        public MultiSlot(String name)
        {
            InitBlock();
            base.Name = name;
        }

        public MultiSlot(String name, Object[] value_Renamed)
        {
            InitBlock();
            base.Name = name;
            this.Value = value_Renamed;
        }

        public virtual Object[] sValue
        {
            set { Value = value; }
        }

        public override Object Value
        {
            get
            {
                if (Value != Constants.NIL_SYMBOL)
                {
                    return (Object[]) Value;
                }
                else
                {
                    return new Object[] {Value};
                }
            }
        }

        /// <summary> In some cases, a deftemplate can be define with a default value.
        /// 
        /// </summary>
        /// <param name="">value
        /// 
        /// </param>
        public virtual Object[] DefaultValue
        {
            set { Value = value; }
        }

        /// <summary> We override the base implementation and do nothing, since a multislot is
        /// an object array. That means it is an array type
        /// </summary>
        public override int ValueType
        {
            set { }
        }

        private void InitBlock()
        {
            type = Constants.ARRAY_TYPE;
        }


        public override String valueToString()
        {
            return Value.ToString();
        }

        /// <summary> method returns a clone and set id, name and value.
        /// </summary>
        public Object Clone()
        {
            MultiSlot newms = new MultiSlot();
            newms.Id = Id;
            newms.Name = Name;
            newms.Value = (Object[]) Value;
            return newms;
        }
    }
}