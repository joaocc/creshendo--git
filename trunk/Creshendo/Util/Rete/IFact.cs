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
    /// Base interface for Facts
    /// 
    /// </author>
    public interface IFact
    {
        /// <summary> Return the object instance linked to the fact
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        Object ObjectInstance { get; }

        /// <summary> Return the unique ID for the fact
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        long FactId { get; }

        /// <summary> Return the Deftemplate for the fact
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        Deftemplate Deftemplate { get; }

        /// <summary> Return the value at the given slot id
        /// </summary>
        /// <param name="">id
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        Object getSlotValue(int id);

        /// <summary> Return id of the given slot name
        /// </summary>
        /// <param name="">name
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        int getSlotId(String name);

        /// <summary> Method will return the fact in a string format.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        String toFactString();

        /// <summary> If we need to update slots
        /// </summary>
        /// <param name="">slots
        /// 
        /// </param>
        void updateSlots(Rete engine, Slot[] slots);

        /// <summary> finalize the object and make it ready for GC
        /// </summary>
        void clear();

        /// <summary> the timestamp for the fact
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        long timeStamp();

        /// <summary> return the equality index
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        EqualityIndex equalityIndex();
    }
}