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
    /// this class is used by hash not equal beta node. It uses to create
    /// the Hash index to look up the matches on the right.
    /// 
    /// </author>
    [Serializable]
    public class BindValue
    {
        protected internal bool negated_Renamed_Field = false;
        protected internal Object value_Renamed = null;

        public BindValue(Object val, bool negate) 
        {
            value_Renamed = val;
            negated_Renamed_Field = negate;
        }

        public BindValue(Object val) 
        {
            value_Renamed = val;
        }

        public virtual Object Value
        {
            get { return value_Renamed; }
        }


        public virtual bool negated()
        {
            return negated_Renamed_Field;
        }
    }
}