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
    /// Value parameter is meant for values. It extends AbstractParam, which provides
    /// implementation for the convienance methods that convert the value to
    /// primitive types.
    /// 
    /// </author>
    public class ValueParam : AbstractParam
    {
        protected internal Object value_Renamed = null;
        protected internal int valueType;

        public ValueParam() 
        {
        }

        /// <summary> 
        /// </summary>
        public ValueParam(int vtype, Object value_Renamed) 
        {
            valueType = vtype;
            this.value_Renamed = value_Renamed;
        }

        /// <summary> The value types are defined in woolfel.engine.rete.Constants
        /// </summary>
        public override int ValueType
        {
            get { return valueType; }

            set { valueType = value; }
        }

        /// <summary> Method will return the value as on Object. This means primitive
        /// values are wrapped in their Object equivalent.
        /// </summary>
        public override Object Value
        {
            get { return value_Renamed; }

            set { value_Renamed = value; }
        }

        public override bool ObjectBinding
        {
            get { return false; }
        }


        /// <summary> Value parameter don't need to resolve the value, so it just
        /// returns it.
        /// </summary>
        public override Object getValue(Rete engine, int valueType)
        {
            if (this.valueType == Constants.STRING_TYPE)
            {
                return Decimal.Parse((String) value_Renamed);
            }
            else
            {
                return value_Renamed;
            }
        }

        /// <summary> implementation sets the value to null and the type to Object
        /// </summary>
        public override void reset()
        {
            value_Renamed = null;
            valueType = Constants.OBJECT_TYPE;
        }

        public virtual ValueParam cloneParameter()
        {
            ValueParam vp = new ValueParam();
            vp.value_Renamed = value_Renamed;
            vp.valueType = valueType;
            return vp;
        }
    }
}