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
using System.Collections;
using Creshendo.Util.Collections;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// Interface defining working memory, which Contains all HashMaps and data structures
    /// related to the node memories, profiling, binding and scopes.
    /// 
    /// 
    /// </author>
    public interface IWorkingMemory
    {
        Agenda Agenda { get; }
        IGenericMap<object, object> DeffactMap { get; }
        DefglobalMap Defglobals { get; }
        System.Collections.Generic.IList<Object> AllFacts { get; }
        System.Collections.Generic.IList<Object> Deffacts { get; }
        System.Collections.Generic.IList<Object> Objects { get; }
        System.Collections.Generic.IList<Object> InitialFacts { get; }
        bool ProfileAssert { set; }
        bool ProfileFire { set; }
        bool ProfileRetract { set; }
        bool WatchFact { set; }
        bool WatchRules { set; }

        IGenericMap<object, object> DynamicFacts { /// ----- methods for getting the Fact map  ----- ///
            get; }

        IGenericMap<object, object> StaticFacts { get; }
        IModule CurrentFocus { get; }
        IModule Main { get; }
        IModule CurrentModule { set; }
        System.Collections.Generic.ICollection<Object> Modules { get; }

        IStrategy Strategy { /// ----- method for Strategy ----- ///
            get; set; }

        /// <summary> Return the RuleCompiler for this working memory
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        IRuleCompiler RuleCompiler { get; }

        /// ----- assert and retract methods ----- ///
        void assertFact(IFact fact);

        void assertObject(Object data, String template, bool statc, bool shadow);
        void assertObjects(IList objs);
        void retractFact(IFact fact);
        void retractObject(Object data);
        void modifyObject(Object data);
        IFact getFactById(long id);
        bool profileAssert();
        bool profileFire();
        bool profileRetract();
        bool watchFact();
        bool watchRules();
        Object getBinding(String name);
        void setBindingValue(String key, Object value_Renamed);
        void pushScope(IScope s);
        void popScope();

        /// ----- methods related to module ----- ///
        IModule addModule(String name);

        IModule findModule(String name);
        IModule removeModule(String name);

        /// <summary>
        /// The key for looking up the memory should be the node. Each node
        /// should pass itself as the key for the lookup.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// -----  methods for getting the memory for a given node ----- ///
        Object getAlphaMemory(Object key);

        /// <summary>
        /// In the case of AlphaMemory, during the compilation process,
        /// we may want to Remove an alpha memory if one already exists.
        /// This depends on how rule compilation works.
        /// </summary>
        /// <param name="key">The key.</param>
        void removeAlphaMemory(Object key);

        /// <summary>
        /// The key for the lookup should be the node. Each BetaNode has
        /// a left and right memory, so it's necessary to have a lookup
        /// method for each memory.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        IGenericMap<object, object> getBetaLeftMemory(Object key);

        /// <summary>
        /// The key for the lookup should be the node. Each BetaNode has
        /// a left and right memory, so it's necessary to have a lookup
        /// method for each memory.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Object getBetaRightMemory(Object key);

        /// <summary>
        /// The for the lookup is the terminalNode. Depending on the terminal
        /// node used, it may not have a memory.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Object getTerminalMemory(Object key);

        /// <summary>
        /// Printout the working memory. If the method is called with
        /// true, the workingmemory should print out the number of
        /// matches for each node. It isn't necessary to print the full
        /// detail of each fact in each node. For now, just the number
        /// of matches for each Node is sufficient.
        /// </summary>
        /// <param name="detailed">if set to <c>true</c> [detailed].</param>
        /// <param name="inputNodes">if set to <c>true</c> [input nodes].</param>
        void printWorkingMemory(bool detailed, bool inputNodes);

        /// <summary>
        /// Printout the working memory with the given filter. if no filer
        /// is passed, it should call printWorkingMemory(true,false);
        /// </summary>
        /// <param name="filter">The filter.</param>
        void printWorkingMemory(IGenericMap<Object, Object> filter);

        /// <summary> Printout the facts on the right side of BetaNodes.
        /// </summary>
        void printWorkingMemoryBetaRight();

        /// <summary> Clears everything in the working memory
        /// *
        /// </summary>
        void clear();

        void clearObjects();
        void clearFacts();
    }
}