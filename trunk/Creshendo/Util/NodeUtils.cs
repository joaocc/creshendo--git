/*
* Copyright 2002-2007 Peter Lin
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
using Creshendo.Util.Rete;

namespace Creshendo.Util
{
    public class NodeUtils
    {
        /// <summary> Get the values from the left side for nodes that do not have
        /// joins with !=
        /// </summary>
        /// <param name="">facts
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static Object[] getLeftValues(Binding[] binds, IFact[] facts)
        {
            Object[] vals = new Object[binds.Length];
            for (int idx = 0; idx < binds.Length; idx++)
            {
                vals[idx] = facts[binds[idx].LeftRow].getSlotValue(binds[idx].LeftIndex);
            }
            return vals;
        }

        /// <summary> convienance method for getting the values based on the bindings
        /// for nodes that do not have !=
        /// </summary>
        /// <param name="">ft
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static Object[] getRightValues(Binding[] binds, IFact ft)
        {
            Object[] vals = new Object[binds.Length];
            for (int idx = 0; idx < binds.Length; idx++)
            {
                vals[idx] = ft.getSlotValue(binds[idx].RightIndex);
            }
            return vals;
        }

        /// <summary> convienance method for getting the values based on the
        /// bindings
        /// </summary>
        /// <param name="">ft
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static BindValue[] getRightBindValues(Binding[] binds, IFact ft)
        {
            BindValue[] vals = new BindValue[binds.Length];
            for (int idx = 0; idx < binds.Length; idx++)
            {
                vals[idx] = new BindValue(ft.getSlotValue(binds[idx].RightIndex), binds[idx].negated());
            }
            return vals;
        }

        /// <summary> Get the values from the left side
        /// </summary>
        /// <param name="">facts
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static BindValue[] getLeftBindValues(Binding[] binds, IFact[] facts)
        {
            BindValue[] vals = new BindValue[binds.Length];
            for (int idx = 0; idx < binds.Length; idx++)
            {
                vals[idx] = new BindValue(facts[binds[idx].LeftRow].getSlotValue(binds[idx].LeftIndex), binds[idx].negated());
            }
            return vals;
        }
    }
}