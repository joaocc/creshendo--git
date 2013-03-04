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


using System.Collections;

namespace Creshendo.Util.Rete
{
    public class ParameterUtils
    {
        /// <summary> The method takes a list containing Parameters and converts it to
        /// an array of Parameter[]. Do not pass a list with other types
        /// </summary>
        /// <param name="">list
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static IParameter[] convertParameters(IList list)
        {
            IParameter[] pms = new IParameter[list.Count];
            for (int idx = 0; idx < list.Count; idx++)
            {
                pms[idx] = (IParameter) list[idx];
            }
            return pms;
        }

        /// <summary> slotToParameters is a convienant utility method that converts
        /// a list containing parameters and Slots to an array of Parameter[].
        /// The method is used by the parser to handle modify statements.
        /// </summary>
        /// <param name="">list
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static IParameter[] slotToParameters(IList list)
        {
            IParameter[] pms = new IParameter[list.Count];
            for (int idx = 0; idx < list.Count; idx++)
            {
                if (list[idx] is Slot)
                {
                    pms[idx] = new SlotParam((Slot) list[idx]);
                }
                else
                {
                    pms[idx] = (IParameter) list[idx];
                }
            }
            return pms;
        }
    }
}