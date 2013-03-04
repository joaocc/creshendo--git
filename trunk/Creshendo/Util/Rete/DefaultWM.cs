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
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Creshendo.Util.Collections;
using Creshendo.Util.Rete.Exception;
using Creshendo.Util.Rule;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// This a new implementation of the working memory that is a clean rewrite to make
    /// it organized. The old one was getting a bit messy and refactoring it was becoming
    /// a pain.
    /// 
    /// </author>
    [Serializable]
    public class DefaultWM : IWorkingMemory
    {
        private readonly Agenda agenda = null;
        protected internal IGenericMap<object, object> alphaMemories;

        protected internal IGenericMap<object, IGenericMap<object, object>> betaLeftMemories;

        protected internal IGenericMap<object, object> betaRightMemories;
        protected internal IRuleCompiler compiler = null;

        private IModule currentModule = null;

        /// <summary> We use a HashMap to make it easy to determine if an existing deffact
        /// already exists in the working memory. this is only used for deffacts and
        /// not for objects
        /// </summary>
        protected internal IGenericMap<object, object> deffactMap;

        /// <summary> Container for Defglobals
        /// </summary>
        protected internal DefglobalMap defglobals;

        /// <summary> We keep a map of the dynamic object instances. When the rule engine is
        /// notified
        /// </summary>
        protected internal IGenericMap<object, object> dynamicFacts;

        protected internal Rete engine = null;
        protected internal List<Object> focusStack;

        /// <summary> The initial facts the rule engine needs at startup
        /// </summary>
        protected internal List<Object> initialFacts;

        private IModule main = null;

        /// <summary> The Creshendo.rete.util.List for the modules.
        /// </summary>
        protected internal IGenericMap<object, object> modules;

        private bool profileAssert_Renamed_Field = false;
        private bool profileFire_Renamed_Field = false;
        private bool profileRetract_Renamed_Field = false;
        protected internal RootNode root = null;
        private Stack scopes;

        /// <summary> We keep a map between the object instance and the corresponding shadown
        /// fact. If an object is added as static, it is added to this map. When the
        /// rule engine is notified of changes, it will check this list. If the
        /// object instance is in this list, we ignore it.
        /// </summary>
        protected internal IGenericMap<object, object> staticFacts;

        protected internal IGenericMap<object, object> terminalMemories;
        private IStrategy theStrat = null;
        private bool watchFact_Renamed_Field = false;
        private bool watchRules_Renamed_Field = false;

        public DefaultWM(Rete engine, RootNode node, IRuleCompiler compiler)
        {
            InitBlock();
            this.engine = engine;
            root = node;
            this.compiler = compiler;
            this.compiler.WorkingMemory = this;
            agenda = new Agenda(engine);
            init();
        }

        #region WorkingMemory Members

        public virtual System.Collections.Generic.ICollection<object> Modules
        {
            get { return modules.Values; }
        }

        public virtual Agenda Agenda
        {
            get { return agenda; }
        }

        public virtual IGenericMap<object, object> DeffactMap
        {
            get { return deffactMap; }
        }

        public virtual DefglobalMap Defglobals
        {
            get { return defglobals; }
        }

        public virtual IList<Object> AllFacts
        {
            get
            {
                List<Object> facts = new List<Object>();
                facts.AddRange(Objects);
                facts.AddRange(Deffacts);
                return facts;
            }
        }

        public virtual IList<Object> Deffacts
        {
            get
            {
                List<Object> objects = new List<Object>();
                IEnumerator itr = DeffactMap.Values.GetEnumerator();
                while (itr.MoveNext())
                {
                    Object fact = itr.Current;
                    objects.Add(fact);
                }
                return objects;
            }
        }

        public virtual IList<Object> Objects
        {
            get
            {
                List<Object> objects = new List<Object>();
                IEnumerator itr = DynamicFacts.Keys.GetEnumerator();
                while (itr.MoveNext())
                {
                    Object key = itr.Current;
                    if (!(key is IFact))
                    {
                        objects.Add(key);
                    }
                }
                itr = StaticFacts.Keys.GetEnumerator();
                while (itr.MoveNext())
                {
                    Object key = itr.Current;
                    if (!(key is IFact))
                    {
                        objects.Add(key);
                    }
                }
                return objects;
            }
        }

        public virtual IList<Object> InitialFacts
        {
            get { return initialFacts; }
        }

        public virtual IModule CurrentFocus
        {
            get { return currentModule; }
        }

        public virtual IModule Main
        {
            get { return main; }
        }

        public virtual IGenericMap<object, object> DynamicFacts
        {
            get { return dynamicFacts; }
        }

        public virtual IRuleCompiler RuleCompiler
        {
            get { return compiler; }
        }

        public virtual IGenericMap<object, object> StaticFacts
        {
            get { return staticFacts; }
        }

        /// <summary> the implementation sets the strategy for the current module
        /// in focus. If there are multiple modules, it does not set
        /// the strategy for the other modules.
        /// </summary>
        public virtual IStrategy Strategy
        {
            get { return theStrat; }

            set
            {
                theStrat = value;
                CurrentFocus.Strategy = value;
            }
        }

        public virtual IModule CurrentModule
        {
            set { currentModule = value; }
        }

        public virtual bool ProfileAssert
        {
            set { profileAssert_Renamed_Field = value; }
        }

        public virtual bool ProfileFire
        {
            set { profileFire_Renamed_Field = value; }
        }

        public virtual bool ProfileRetract
        {
            set { profileRetract_Renamed_Field = value; }
        }

        public virtual bool WatchFact
        {
            set { watchFact_Renamed_Field = value; }
        }

        public virtual bool WatchRules
        {
            set { watchRules_Renamed_Field = value; }
        }

        public virtual IModule addModule(String name)
        {
            IModule mod = findModule(name);
            if (mod == null)
            {
                mod = new Defmodule(name);
                modules.Put(mod.ModuleName, mod);
                CurrentModule = mod;
            }
            return mod;
        }


        public virtual void assertFact(IFact fact)
        {
            Deffact f = (Deffact) fact;
            if (!containsFact(f))
            {
                deffactMap.Put(fact.equalityIndex(), f);
                f.setFactId = engine;
                if (profileAssert_Renamed_Field)
                {
                    assertFactWProfile(f);
                }
                else
                {
                    if (watchFact_Renamed_Field)
                    {
                        engine.writeMessage("==> " + fact.toFactString() + Constants.LINEBREAK, "t");
                    }
                    root.assertObject(f, engine, this);
                }
            }
            else
            {
                f.resetID((Deffact) deffactMap.Get(fact.equalityIndex()));
            }
        }

        /// <summary>
        /// The current implementation of assertObject is simple, but flexible. This
        /// version is not multi-threaded and doesn't use an event queue. Later on a
        /// multi-threaded version will be written which overrides the base
        /// implementation. If the user passes a specific template name, the engine
        /// will attempt to only propogate the fact down that template. if no
        /// template name is given, the engine will propogate the fact down all input
        /// nodes, including parent templates.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="template">The template.</param>
        /// <param name="statc">if set to <c>true</c> [statc].</param>
        /// <param name="shadow">if set to <c>true</c> [shadow].</param>
        public virtual void assertObject(Object data, String template, bool statc, bool shadow)
        {
            Defclass dc = null;
            if (template == null)
            {
                dc = engine.findDefclass(data);
            }
            else
            {
                dc = engine.findDefclassByTemplate(template);
            }
            if (dc != null)
            {
                if (statc && !StaticFacts.ContainsKey(data))
                {
                    IFact shadowfact = createFact(data, dc, template, engine.nextFactId());
                    // Add it to the static fact map
                    StaticFacts.Put(data, shadowfact);
                    assertFact(shadowfact);
                }
                else if (!DynamicFacts.ContainsKey(data))
                {
                    if (shadow)
                    {
                        // first Add the rule engine as a listener
                        if (dc.JavaBean)
                        {
                            try
                            {
                                MethodInfo miHandler = typeof(Rete).GetMethod("PropertyHasChanged", BindingFlags.NonPublic | BindingFlags.Instance);
                                Delegate d = Delegate.CreateDelegate(dc.DelegateType, this.engine, miHandler);
                                dc.AddListenerMethod.Invoke(data, (Object[]) new Object[] {d});
                            }
                            catch (TargetInvocationException e)
                            {
                                System.Diagnostics.Trace.WriteLine(e.Message);
                            }
                            catch (UnauthorizedAccessException e)
                            {
                                System.Diagnostics.Trace.WriteLine(e.Message);
                            }
                        }
                        // second, lookup the deftemplate and create the
                        // shadow fact
                        IFact shadowfact = createFact(data, dc, template, engine.nextFactId());
                        // Add it to the dynamic fact map
                        DynamicFacts.Put(data, shadowfact);
                        assertFact(shadowfact);
                    }
                    else
                    {
                        IFact nsfact = createNSFact(data, dc, engine.nextFactId());
                        DynamicFacts.Put(data, nsfact);
                        assertFact(nsfact);
                    }
                }
            }
        }

        /// <summary>
        /// By default assertObjects will assert with shadow and dynamic. It also
        /// assumes the classes aren't using an user defined template name.
        /// </summary>
        /// <param name="objs">The objs.</param>
        public virtual void assertObjects(IList objs)
        {
            IEnumerator itr = objs.GetEnumerator();
            while (itr.MoveNext())
            {
                assertObject(itr.Current, null, false, false);
            }
        }

        public virtual void clear()
        {
            IEnumerator amitr = alphaMemories.Values.GetEnumerator();
            while (amitr.MoveNext())
            {
                IAlphaMemory am = (IAlphaMemory) amitr.Current;
                am.clear();
            }
            alphaMemories.Clear();
            // aggressivley Clear the memories
            IEnumerator blitr = betaLeftMemories.Values.GetEnumerator();
            while (blitr.MoveNext())
            {
                Object bval = blitr.Current;
                if (bval is IGenericMap<Object, Object>)
                {
                    IGenericMap<Object, Object> lmem = (IGenericMap<Object, Object>) bval;
                    // now iterate over the betamemories
                    IEnumerator bmitr = lmem.Keys.GetEnumerator();
                    while (bmitr.MoveNext())
                    {
                        Index indx = (Index) bmitr.Current;
                        indx.clear();
                    }
                    lmem.Clear();
                }
            }
            betaLeftMemories.Clear();
            IEnumerator britr = betaRightMemories.Values.GetEnumerator();
            while (britr.MoveNext())
            {
                Object val = britr.Current;
                if (val is HashedAlphaMemoryImpl)
                {
                    ((HashedAlphaMemoryImpl) val).clear();
                }
                else if (val is TemporalHashedAlphaMem)
                {
                    ((TemporalHashedAlphaMem) val).clear();
                }
                else
                {
                    IGenericMap<IFact, IFact> mem = (IGenericMap<IFact, IFact>)val;
                    mem.Clear();
                }
            }
            betaRightMemories.Clear();
            terminalMemories.Clear();
            root.clear();
            focusStack.Clear();
            //contexts.Clear();
            agenda.clear();
            main.clear();
            currentModule.clear();
            addModule(main);
        }

        /// <summary> Clear the deffacts from the working memory. This does not include facts
        /// asserted using assertObject.
        /// </summary>
        public virtual void clearFacts()
        {
            if (DynamicFacts.Count > 0)
            {
                try
                {
                    IEnumerator itr = DynamicFacts.Keys.GetEnumerator();
                    while (itr.MoveNext())
                    {
                        Object obj = itr.Current;
                        if (obj is IFact)
                        {
                            retractFact((IFact) obj);
                        }
                    }
                    DynamicFacts.Clear();
                }
                catch (RetractException e)
                {
                    // log.Debug(e);
                }
            }
            if (StaticFacts.Count > 0)
            {
                try
                {
                    IEnumerator itr = StaticFacts.Keys.GetEnumerator();
                    while (itr.MoveNext())
                    {
                        Object obj = itr.Current;
                        if (obj is IFact)
                        {
                            retractFact((IFact) obj);
                        }
                    }
                    StaticFacts.Clear();
                }
                catch (RetractException e)
                {
                    // log.Debug(e);
                }
            }
        }

        /// <summary> Clear the objects from the working memory
        /// </summary>
        public virtual void clearObjects()
        {
            if (DynamicFacts.Count > 0)
            {
                try
                {
                    IEnumerator itr = DynamicFacts.Keys.GetEnumerator();
                    while (itr.MoveNext())
                    {
                        Object obj = itr.Current;
                        if (!(obj is Deffact))
                        {
                            retractObject(obj);
                        }
                    }
                }
                catch (RetractException e)
                {
                    // log.Debug(e);
                }
            }
            if (StaticFacts.Count > 0)
            {
                try
                {
                    IEnumerator itr = StaticFacts.Keys.GetEnumerator();
                    while (itr.MoveNext())
                    {
                        Object obj = itr.Current;
                        if (!(obj is Deffact))
                        {
                            retractObject(obj);
                        }
                    }
                }
                catch (RetractException e)
                {
                    // log.Debug(e);
                }
            }
        }

        public virtual IModule findModule(String name)
        {
            return (IModule) modules.Get(name);
        }

        /// <summary> The current implementation will try to find the memory for the node.
        /// If it doesn't find it, it will create a new one.
        /// </summary>
        public virtual Object getAlphaMemory(Object key)
        {
            Object m = alphaMemories.Get(key);
            if (m == null)
            {
                String mname = "alphamem" + ((BaseNode) key).nodeID;
                m = new AlphaMemoryImpl(mname);
                alphaMemories.Put(key, m);
            }
            return m;
        }

        /// <summary> the current implementation will try to find the memory for the node.
        /// If it doesn't find it, it will create a new Left memory, which is
        /// HashMap.
        /// </summary>
        public virtual IGenericMap<object,object> getBetaLeftMemory(Object key)
        {
            IGenericMap<object, object> m = betaLeftMemories.Get(key);
            if (m == null)
            {
                // it should create a new memory
                // and return it.
                String mname = "blmem" + ((BaseNode) key).nodeID;
                m = CollectionFactory.newBetaMemoryMap(mname);
                betaLeftMemories.Put(key, m);
            }
            return m;
        }

        /// <summary> the current implementation will try to find the memory for the node.
        /// If it doesn't find it, it checks the node type and creates the
        /// appropriate AlphaMemory for the node. Since right memories are
        /// hashed, it creates the appropriate type of Hashed memory.
        /// </summary>
        public virtual Object getBetaRightMemory(Object key)
        {
            Object val = betaRightMemories.Get(key);
            if (val != null)
            {
                return val;
            }
            else
            {
                if (key is HashedEqBNode || key is HashedEqNJoin || key is ExistJoin)
                {
                    String mname = "hnode" + ((BaseNode) key).nodeID;
                    HashedAlphaMemoryImpl alpha = new HashedAlphaMemoryImpl(mname);
                    betaRightMemories.Put(key, alpha);
                    return alpha;
                }
                else if (key is HashedNotEqBNode || key is HashedNotEqNJoin || key is ExistNeqJoin)
                {
                    String mname = "hneq" + ((BaseNode) key).nodeID;
                    HashedNeqAlphaMemory alpha = new HashedNeqAlphaMemory(mname);
                    betaRightMemories.Put(key, alpha);
                    return alpha;
                }
                else if (key is TemporalEqNode)
                {
                    String mname = "hnode" + ((BaseNode) key).nodeID;
                    TemporalHashedAlphaMem alpha = new TemporalHashedAlphaMem(mname);
                    betaRightMemories.Put(key, alpha);
                    return alpha;
                }
                else
                {
                    String mname = "brmem" + ((BaseNode) key).nodeID;
                    IGenericMap<IFact, IFact> right = CollectionFactory.newAlphaMemoryMap(mname);
                    betaRightMemories.Put(key, right);
                    return right;
                }
            }
        }


        public virtual Object getBinding(String name)
        {
            if (scopes.Count != 0 && !name.StartsWith("*"))
            {
                Object val = ((IScope) scopes.Peek()).getBindingValue(name);
                return val;
            }
            else
            {
                return Defglobals.getValue(name);
            }
        }


        public virtual IFact getFactById(long id)
        {
            IFact df = null;
            IEnumerator itr = DeffactMap.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                df = (Deffact) itr.Current;
                if (df.FactId == id)
                {
                    return df;
                }
            }
            // now search the object facts
            if (df == null)
            {
                // check dynamic facts
                IEnumerator itr2 = DynamicFacts.Values.GetEnumerator();
                while (itr2.MoveNext())
                {
                    df = (IFact) itr2.Current;
                    if (df.FactId == id)
                    {
                        return df;
                    }
                }
                if (df == null)
                {
                    itr2 = StaticFacts.Values.GetEnumerator();
                    while (itr2.MoveNext())
                    {
                        df = (IFact) itr2.Current;
                        if (df.FactId == id)
                        {
                            return df;
                        }
                    }
                }
            }
            return null;
        }


        public virtual Object getTerminalMemory(Object key)
        {
            Object m = terminalMemories.Get(key);
            if (m == null)
            {
                m = CollectionFactory.newTerminalMap();
                terminalMemories.Put(key, m);
            }
            return m;
        }

        /// <summary>
        /// Modify will call retract with the old fact, followed by updating the fact
        /// instance and asserting the fact.
        /// </summary>
        /// <param name="data">The data.</param>
        public virtual void modifyObject(Object data)
        {
            if (DynamicFacts.ContainsKey(data))
            {
                Defclass dc = (Defclass) engine.findDefclass(data);
                // first we retract the fact
                IFact ft = (IFact) DynamicFacts.RemoveWithReturn(data);
                String tname = ft.Deftemplate.Name;
                long fid = ft.FactId;
                retractFact(ft);
                // create a new fact with the same ID
                ft = createFact(data, dc, tname, fid);
                DynamicFacts.Put(data, ft);
                assertFact(ft);
            }
        }

        public virtual bool profileAssert()
        {
            return profileAssert_Renamed_Field;
        }

        public virtual bool profileFire()
        {
            return profileFire_Renamed_Field;
        }

        public virtual bool profileRetract()
        {
            return profileRetract_Renamed_Field;
        }

        public virtual bool watchFact()
        {
            return watchFact_Renamed_Field;
        }

        public virtual bool watchRules()
        {
            return watchRules_Renamed_Field;
        }

        public virtual void popScope()
        {
            scopes.Pop();
        }

        public virtual void printWorkingMemory(bool detailed, bool inputNodes)
        {
            engine.writeMessage("AlphaNode count " + alphaMemories.Count + Constants.LINEBREAK);
            IEnumerator itr = alphaMemories.Keys.GetEnumerator();
            int memTotal = 0;
            while (itr.MoveNext())
            {
                BaseNode key = (BaseNode) itr.Current;
                if (!(key is ObjectTypeNode) && !(key is LIANode))
                {
                    IAlphaMemory am = (IAlphaMemory) alphaMemories.Get(key);
                    if (detailed)
                    {
                        engine.writeMessage(key.toPPString() + " count=" + am.size() + Constants.LINEBREAK);
                    }
                    memTotal += am.size();
                }
                else
                {
                    if (inputNodes)
                    {
                        IAlphaMemory am = (IAlphaMemory) alphaMemories.Get(key);
                        engine.writeMessage(key.toPPString() + " count=" + am.size() + Constants.LINEBREAK);
                    }
                }
            }
            engine.writeMessage("total AlphaMemories = " + memTotal + Constants.LINEBREAK);

            // now write out the left beta memory
            engine.writeMessage("BetaNode Count " + betaLeftMemories.Count + Constants.LINEBREAK);
            int betaTotal = 0;
            itr = betaLeftMemories.Keys.GetEnumerator();
            while (itr.MoveNext())
            {
                BaseNode key = (BaseNode) itr.Current;
                if (key is BaseJoin)
                {
                    printBetaNodes((BaseJoin) key, detailed, betaTotal);
                }
            }
            engine.writeMessage("total BetaMemories = " + betaTotal + Constants.LINEBREAK);
        }

        /// <summary> TODO - not implemented yet
        /// </summary>
        public virtual void printWorkingMemory(IGenericMap<Object, Object> filter)
        {
            if (filter != null && filter.Count > 0)
            {
                // not implemented yet
            }
            else
            {
                printWorkingMemory(true, false);
            }
        }

        public virtual void printWorkingMemoryBetaRight()
        {
            StringBuilder buf = new StringBuilder();
            IEnumerator itr = betaRightMemories.Keys.GetEnumerator();
            while (itr.MoveNext())
            {
                BaseJoin key = (BaseJoin) itr.Current;
                buf.Append(key.toPPString());
                Object rmem = betaRightMemories.Get(key);
                StringBuilder buf2 = new StringBuilder();
                if (rmem is IGenericMap<Object, Object>)
                {
                    int count = 0;
                    IEnumerator fitr = ((IGenericMap<Object, Object>) rmem).Values.GetEnumerator();
                    buf2.Append(": ");
                    while (fitr.MoveNext())
                    {
                        IFact ft = (IFact) fitr.Current;
                        buf2.Append(ft.FactId + ",");
                        count++;
                    }
                    buf.Append("- total=" + count + " ");
                    buf.Append(buf2.ToString());
                    buf.Append(Constants.LINEBREAK);
                }
                else
                {
                    HashedAlphaMemoryImpl ham = (HashedAlphaMemoryImpl) rmem;
                    int count = 0;
                    Object[] fitr = ham.iterateAll();
                    if (fitr != null)
                    {
                        for (int idz = 0; idz < fitr.Length; idz++)
                        {
                            IFact ft = (IFact) fitr[idz];
                            buf2.Append(ft.FactId + ",");
                            count++;
                        }
                    }
                    buf.Append("- total=" + count + " :");
                    buf.Append(buf2.ToString());
                    buf.Append(Constants.LINEBREAK);
                }
            }
            engine.writeMessage(buf.ToString());
        }

        public virtual void pushScope(IScope s)
        {
            StackPush(scopes, s);
        }

        public Object StackPush(Stack stack, Object element)
        {
            stack.Push(element);
            return element;
        }

        public virtual void removeAlphaMemory(Object key)
        {
            alphaMemories.Remove(key);
        }

        public virtual IModule removeModule(String name)
        {
            return (IModule) modules.RemoveWithReturn(name);
        }

        public virtual void retractFact(IFact fact)
        {
            Deffact f = (Deffact) fact;
            deffactMap.Remove(f.equalityIndex());
            if (profileRetract_Renamed_Field)
            {
                retractFactWProfile(f);
            }
            else
            {
                if (watchFact_Renamed_Field)
                {
                    engine.writeMessage("<== " + fact.toFactString() + Constants.LINEBREAK, "t");
                }
                root.retractObject(f, engine, this);
            }
        }

        /// <summary>
        /// Retracts the object.
        /// </summary>
        /// <param name="data">The data.</param>
        public virtual void retractObject(Object data)
        {
            if (StaticFacts.ContainsKey(data))
            {
                IFact ft = (IFact) StaticFacts.RemoveWithReturn(data);
                retractFact(ft);
                // we should probably recyle the factId before we
                // clean the fact
                ft.clear();
            }
            else if (DynamicFacts.ContainsKey(data))
            {
                IFact ft = (IFact) DynamicFacts.RemoveWithReturn(data);
                retractFact(ft);
                // we should probably recyle the factId before we
                // clean the fact
                ft.clear();
            }
        }

        public virtual void setBindingValue(String key, Object value_Renamed)
        {
            if (scopes.Count != 0 && !key.StartsWith("*"))
            {
                ((IScope) scopes.Peek()).setBindingValue(key, value_Renamed);
            }
            else
            {
                Defglobals.declareDefglobal(key, value_Renamed);
            }
        }

        #endregion

        private void InitBlock()
        {
            alphaMemories = CollectionFactory.newMap();
            betaLeftMemories = new GenericHashMap<object, IGenericMap<object, object>>();
            betaRightMemories = CollectionFactory.newMap();
            terminalMemories = CollectionFactory.newMap();
            staticFacts = CollectionFactory.localMap();
            dynamicFacts = CollectionFactory.localMap();
            deffactMap = CollectionFactory.localMap();
            defglobals = new DefglobalMap();
            initialFacts = new List<Object>();
            modules = CollectionFactory.localMap();
            focusStack = new List<Object>();
            scopes = new Stack();
        }

        protected internal void init()
        {
            theStrat = Strategies.Strategies.DEPTH;
            main = new Defmodule(Constants.MAIN_MODULE);
            main.Strategy = theStrat;
            addModule(main);
            currentModule = main;
        }

        public virtual void addModule(IModule mod)
        {
            if (mod != null)
            {
                modules.Put(mod.ModuleName, mod);
                CurrentModule = mod;
            }
        }

        /// <summary>
        /// The implementation will look in the current module in focus. If it isn't
        /// found, it will search the other modules. The last module it checks should
        /// be the main module.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="dclass">The dclass.</param>
        /// <param name="template">The template.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        protected internal virtual IFact createFact(Object data, Defclass dclass, String template, long id)
        {
            IFact ft = null;
            ITemplate dft = null;
            if (template == null)
            {
                dft = CurrentFocus.getTemplate(dclass.ClassObject.FullName);
            }
            else
            {
                dft = CurrentFocus.getTemplate(template);
            }
            // if the deftemplate is null, check the other modules
            if (dft == null)
            {
                // Get the entry set from the agenda and iterate
                IEnumerator itr = modules.Values.GetEnumerator();
                while (itr.MoveNext())
                {
                    IModule mod = (IModule) itr.Current;
                    if (mod.containsTemplate(dclass))
                    {
                        dft = mod.getTemplate(dclass);
                    }
                }
                // we've searched every module, so now check main
                if (dft == null && main.containsTemplate(dclass))
                {
                    dft = main.getTemplate(dclass);
                }
                else
                {
                    // throw an exception
                    throw new AssertException("Could not find the template");
                }
            }
            ft = dft.createFact(data, dclass, id);
            return ft;
        }

        /// <summary>
        /// convienance method for creating a Non-Shadow fact.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="dclass">The dclass.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        protected internal virtual IFact createNSFact(Object data, Defclass dclass, long id)
        {
            Deftemplate dft = (Deftemplate) CurrentFocus.getTemplate(dclass);
            NSFact fact = new NSFact(dft, dclass, data, dft.AllSlots, id);
            return fact;
        }


        /// ----- helper methods that are not defined in WorkingMemory interface ----- ///
        protected internal virtual void assertFactWProfile(Deffact fact)
        {
            ProfileStats.startAssert();
            root.assertObject(fact, engine, this);
            ProfileStats.endAssert();
        }

        public virtual bool containsFact(Deffact fact)
        {
            return deffactMap.ContainsKey(fact.equalityIndex());
        }

        protected internal virtual void printBetaNodes(BaseJoin bjoin, bool detailed, int betaTotal)
        {
            if (bjoin is HashedEqBNode || bjoin is HashedEqNJoin)
            {
                IGenericMap<Object, Object> bm = (IGenericMap<Object, Object>) betaLeftMemories.Get(bjoin);
                // we iterate over the keys in the HashMap
                IEnumerator bitr = bm.Keys.GetEnumerator();
                while (bitr.MoveNext())
                {
                    Index indx = (Index) bm.Get(bitr.Current);
                    if (detailed)
                    {
                        engine.writeMessage(bjoin.toPPString(), Constants.DEFAULT_OUTPUT);
                        HashedAlphaMemoryImpl rightmem = (HashedAlphaMemoryImpl) getBetaRightMemory(bjoin);

                        EqHashIndex eqinx = new EqHashIndex(NodeUtils.getLeftValues(bjoin.binds, indx.Facts));
                        // Add to the total count
                        betaTotal += rightmem.count(eqinx);
                        engine.writeMessage(" count=" + betaTotal + " - " + indx.toPPString() + ": ", Constants.DEFAULT_OUTPUT);
                        IEnumerator ritr = rightmem.iterator(eqinx);
                        if (ritr != null)
                        {
                            StringBuilder buf = new StringBuilder();
                            while (ritr.MoveNext())
                            {
                                buf.Append(((IFact) ritr.Current).FactId + ",");
                            }
                            engine.writeMessage(buf.ToString(), Constants.DEFAULT_OUTPUT);
                        }
                        engine.writeMessage(Constants.LINEBREAK, Constants.DEFAULT_OUTPUT);
                    }
                }
            }
            else if (bjoin is HashedNotEqNJoin || bjoin is HashedNotEqBNode)
            {
                IGenericMap<Object, Object> bm = (IGenericMap<Object, Object>) betaLeftMemories.Get(bjoin);
                // we iterate over the keys in the HashMap
                IEnumerator bitr = bm.Keys.GetEnumerator();
                while (bitr.MoveNext())
                {
                    Index indx = (Index) bm.Get(bitr.Current);
                    if (detailed)
                    {
                        engine.writeMessage(bjoin.toPPString(), Constants.DEFAULT_OUTPUT);
                        HashedNeqAlphaMemory rightmem = (HashedNeqAlphaMemory) getBetaRightMemory(bjoin);

                        EqHashIndex eqinx = new EqHashIndex(NodeUtils.getLeftValues(bjoin.binds, indx.Facts));
                        // Add to the total count
                        betaTotal += rightmem.count(eqinx);
                        engine.writeMessage(" count=" + betaTotal + " - " + indx.toPPString() + ": ", Constants.DEFAULT_OUTPUT);
                        IEnumerator ritr = rightmem.iterator(eqinx);
                        if (ritr != null)
                        {
                            StringBuilder buf = new StringBuilder();
                            while (ritr.MoveNext())
                            {
                                buf.Append(((IFact) ritr.Current).FactId + ",");
                            }
                            engine.writeMessage(buf.ToString(), Constants.DEFAULT_OUTPUT);
                        }
                        engine.writeMessage(Constants.LINEBREAK, Constants.DEFAULT_OUTPUT);
                    }
                }
            }
            else if (bjoin is ExistJoin)
            {
                ExistJoin henj = (ExistJoin) bjoin;
                IGenericMap<Object, Object> bm = (IGenericMap<Object, Object>) betaLeftMemories.Get(henj);
                // we iterate over the keys in the HashMap
                IEnumerator bitr = bm.Keys.GetEnumerator();
                while (bitr.MoveNext())
                {
                    Index indx = (Index) bm.Get(bitr.Current);
                    if (detailed)
                    {
                        engine.writeMessage(bjoin.toPPString(), Constants.DEFAULT_OUTPUT);
                        HashedAlphaMemoryImpl rightmem = (HashedAlphaMemoryImpl) getBetaRightMemory(henj);

                        EqHashIndex eqinx = new EqHashIndex(NodeUtils.getLeftValues(henj.binds, indx.Facts));
                        // Add to the total count
                        betaTotal += rightmem.count(eqinx);
                        engine.writeMessage(" count=" + betaTotal + " - " + indx.toPPString() + ": ", Constants.DEFAULT_OUTPUT);
                        IEnumerator ritr = rightmem.iterator(eqinx);
                        if (ritr != null)
                        {
                            StringBuilder buf = new StringBuilder();
                            while (ritr.MoveNext())
                            {
                                buf.Append(((IFact) ritr.Current).FactId + ",");
                            }
                            engine.writeMessage(buf.ToString(), Constants.DEFAULT_OUTPUT);
                        }
                        engine.writeMessage(Constants.LINEBREAK, Constants.DEFAULT_OUTPUT);
                    }
                }
            }
            else if (bjoin is NotJoin)
            {
                NotJoin nj = (NotJoin) bjoin;
                IGenericMap<Object, Object> bm = (IGenericMap<Object, Object>) getBetaLeftMemory(bjoin);
                IEnumerator bitr = bm.Keys.GetEnumerator();
                while (bitr.MoveNext())
                {
                    Index indx = (Index) bitr.Current;
                    IBetaMemory bmem = (IBetaMemory) bm.Get(indx);
                    engine.writeMessage(bmem.toPPString());
                }
            }
            else if (bjoin is TemporalEqNode)
            {
                TemporalEqNode ten = (TemporalEqNode) bjoin;
            }
            else
            {
                IGenericMap<Object, Object> bm = (IGenericMap<Object, Object>) betaLeftMemories.Get(bjoin);
                // we iterate over the keys in the HashMap
                IEnumerator bitr = bm.Keys.GetEnumerator();
                while (bitr.MoveNext())
                {
                    Index indx = (Index) bm.Get(bitr.Current);
                    Object rightmem = betaRightMemories.Get(bjoin);
                    if (detailed)
                    {
                        if (rightmem is HashedAlphaMemoryImpl)
                        {
                            HashedAlphaMemoryImpl hami = (HashedAlphaMemoryImpl) rightmem;
                            engine.writeMessage(bjoin.toPPString() + " count=" + hami.size() + " - " + indx.toPPString() + Constants.LINEBREAK);
                        }
                        else
                        {
                            IGenericMap<Object, Object> rmap = (IGenericMap<Object, Object>) rightmem;
                            engine.writeMessage(bjoin.toPPString() + " count=" + rmap.Count + " - " + indx.toPPString() + Constants.LINEBREAK);
                        }
                    }
                    if (rightmem is HashedAlphaMemoryImpl)
                    {
                        betaTotal += ((HashedAlphaMemoryImpl) rightmem).size();
                    }
                    else
                    {
                        betaTotal += ((IGenericMap<IFact, IFact>)rightmem).Count;
                    }
                }
            }
        }

        /// <summary>
        /// Printout the memory for the given rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public virtual void printWorkingMemory(Rule.IRule rule)
        {
            engine.writeMessage("Memories for " + rule.Name);
            ICondition[] conds = rule.Conditions;
            int memTotal = 0;
            for (int idx = 0; idx < conds.Length; idx++)
            {
                ICondition c = conds[idx];
                IList l = c.Nodes;
                IEnumerator itr = l.GetEnumerator();
                while (itr.MoveNext())
                {
                    BaseNode key = (BaseNode) itr.Current;
                    IAlphaMemory am = (IAlphaMemory) alphaMemories.Get(key);
                    engine.writeMessage(key.toPPString() + " count=" + am.size() + Constants.LINEBREAK);
                    memTotal += am.size();
                }
            }
        }

        /// <summary>
        /// Retracts the fact W profile.
        /// </summary>
        /// <param name="fact">The fact.</param>
        protected internal virtual void retractFactWProfile(Deffact fact)
        {
            ProfileStats.startRetract();
            root.retractObject(fact, engine, this);
            ProfileStats.endRetract();
        }
    }
}