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

namespace Creshendo.Util.Rule
{
    /// <author>  Peter Lin
    /// The purpose of the class is for conditions that have  or |. For
    /// 
    /// In those cases, we don't want to create a Literal constraint, since
    /// they are all for the same slot.
    /// 
    /// </author>
    [Serializable]
    public class MultiValue
    {
        protected internal bool negated = false;
        protected internal Object value_Renamed = null;

        /// <summary> 
        /// </summary>
        public MultiValue() 
        {
        }

        public MultiValue(Object val)
        {
            Value = val;
        }

        public MultiValue(Object val, bool neg)
        {
            Value = val;
            negated = neg;
        }

        public virtual Object Value
        {
            get { return value_Renamed; }

            set { value_Renamed = value; }
        }

        public virtual bool Negated
        {
            get { return negated; }

            set { negated = value; }
        }
    }
}