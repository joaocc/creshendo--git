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
    /// Template defines the methods to access an object, which is the
    /// equivalent of un-ordered facts. It defines all the necessary
    /// methods for Deftemplate.
    /// 
    /// </author>
    public interface ITemplate : IPrettyPrint
    {
        /// <summary> The name of the template may be the fully qualified
        /// class name, or an alias.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        String Name { get; }

        /// <summary> templates may have 1 or more slots. A slot is a named
        /// column with a specific type of value.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        int NumberOfSlots { get; }

        /// <summary> Return an array of all the slots.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        Slot[] AllSlots { get; }

        /// <summary> if watch is set to true, the rule engine will pass events
        /// when the fact traverses the network.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> Set the watch flag
        /// </summary>
        /// <param name="">watch
        /// 
        /// </param>
        bool Watch { get; set; }

        /// <summary> If a template has a parent, the method should
        /// return the parent, otherwise it should return
        /// null
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> set the parent template
        /// </summary>
        /// <param name="">parent
        /// 
        /// </param>
        ITemplate Parent { get; set; }

        /// <returns>
        /// 
        /// </returns>
        String ClassName { get; }

        /// <summary> Return the slot with the String name
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        Slot getSlot(String name);

        /// <summary> Get the Slot at the given column id
        /// </summary>
        /// <param name="">column
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        Slot getSlot(int column);

        /// <summary> Get the column index with the given name
        /// </summary>
        /// <param name="">name
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        int getColumnIndex(String name);

        /// <summary> 
        /// </summary>
        /// <param name="">data
        /// </param>
        /// <param name="">id
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        IFact createFact(Object data, Defclass clazz, long id);

        /// <summary> If the template is currently in use, we should not Remove it
        /// until all the dependent rules are removed first.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        bool inUse();

        /// <summary> 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        String toString();
    }
}