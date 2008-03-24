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
    public class StringParam : AbstractParam
    {
        protected internal String value_Renamed = null;
        protected internal int valueType;

        /// <summary> 
        /// </summary>
        public StringParam(int vtype, String value_Renamed) 
        {
            valueType = vtype;
            this.value_Renamed = value_Renamed;
        }

        /// <summary> The value types are defined in woolfel.engine.rete.Constants
        /// </summary>
        public override int ValueType
        {
            set { }
            get { return valueType; }
        }

        /// <summary> Method will return the value as on Object. This means primitive
        /// values are wrapped in their Object equivalent.
        /// </summary>
        public override Object Value
        {
            set { }
            get { return value_Renamed; }
        }

        public override bool ObjectBinding
        {
            get { return false; }
        }


        /// <summary> String parameters do not need to do any lookup, so it just
        /// returns the value.
        /// </summary>
        public override Object getValue(Rete engine, int valueType)
        {
            return value_Renamed;
        }

        /// <summary> implementation sets the value to null and the type to Object
        /// </summary>
        public override void reset()
        {
            value_Renamed = null;
            valueType = Constants.OBJECT_TYPE;
        }
    }
}