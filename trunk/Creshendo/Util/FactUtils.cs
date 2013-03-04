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

namespace Creshendo.Util
{
    /// <author>  Peter Lin
    /// 
    /// the class Contains utilities for doing things like sortig the facts for
    /// printing.
    /// 
    /// </author>
    public class FactUtils
    {
        public static readonly FactComparator COMPARATOR = new FactComparator();

        public static readonly FactTemplateComparator TEMPLATECOMP = new FactTemplateComparator();

        public FactUtils() 
        {
        }

        public static Object[] sortFacts(System.Collections.Generic.IList<Object> facts)
        {
            Object[] sorted = new object[facts.Count];
            facts.CopyTo(sorted,0);
            Arrays.sort(sorted, COMPARATOR);
            return sorted;
        }

        public static Object[] sortFactsByTemplate(System.Collections.Generic.IList<Object> facts)
        {
            Object[] sorted = new object[facts.Count];
            facts.CopyTo(sorted, 0);
            Arrays.sort(sorted, TEMPLATECOMP);
            return sorted;
        }
    }
}