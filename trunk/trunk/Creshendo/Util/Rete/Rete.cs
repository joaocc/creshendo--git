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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Creshendo.Functions;
using Creshendo.Functions.Math;
using Creshendo.Util.Collections;
using Creshendo.Util.Logging;
using Creshendo.Util.Messagerouter;
using Creshendo.Util.Rete.Compiler;
using Creshendo.Util.Rete.Exception;
using Creshendo.Util.Rule;

namespace Creshendo.Util.Rete
{
    public enum ProfileType
    {
        PROFILE_ADD_ACTIVATION = 101,
        PROFILE_ASSERT = 102,
        PROFILE_ALL = 103,
        PROFILE_FIRE = 104,
        PROFILE_RETRACT = 105,
        PROFILE_RM_ACTIVATION = 106,
    }

    public enum WatchType
    {
        WATCH_ACTIVATIONS = 1,
        WATCH_ALL = 2,
        WATCH_FACTS = 3,
        WATCH_RULES = 4,
    }

    /// <author>  Peter Lin
    /// 
    /// This is the main Rete engine class. For now it's called Rete, but I may
    /// change it to Engine to be more generic.
    /// 
    /// </author>
    [Serializable]
    public class Rete : /*PropertyChangeListener, ICompilerListener,*/ IDisposable
    {
        private readonly IRuleCompiler compiler = null;
        private readonly bool debug = false;

        /// <summary> the key is the Class object. The value is the defclass. the defclass is
        /// then used to lookup the deftemplate in the current Module.
        /// </summary>
        protected internal IGenericMap<object, object> defclass;

        private DeffunctionGroup deffunctions;
        protected internal int firingcount = 0;
        private List<object> functionGroups;

        /// <summary> this is the HashMap for all functions. This means all function names are
        /// unique.
        /// </summary>
        protected internal IGenericMap<object, object> functions;

        protected internal bool halt = true;
        protected internal Deftemplate initFact;
        private InterpretedFunction intrFunction = null;
        private long lastFactId = 1;

        private int lastNodeId = 0;

        /// <summary> an org.jamocha.rete.util.List for the listeners
        /// </summary>
        protected internal List<object> listeners;

        protected internal IGenericMap<object, object> outputStreams;
        protected internal bool prettyPrint = false;
        private RootNode root;
        private MessageRouter router;
        private IGenericMap<object, object> rulesFired;
        protected internal IGenericMap<object, object> templateToDefclass;
        protected internal IWorkingMemory workingMem = null;

        /// <summary> 
        /// </summary>
        public Rete()
        {
            InitBlock();
            compiler = new DefaultRuleCompiler(this, root.ObjectTypeNodes);
            workingMem = new DefaultWM(this, root, compiler);
            //CollectionFactory.init();
            init();
            startLog();

            compiler.RuleAdded += ruleAdded;
            compiler.RuleRemoved += ruleRemoved;
            compiler.CompileError += compileError;
        }


        //~Rete()
        //{
        //    router.ShutDown();
        //}

        /// <summary> Method returns the current focus. Only the rules in the current focus
        /// will be fired. Activations in other modules will not be fired until the
        /// focus is changed to it.
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual IModule CurrentFocus
        {
            get { return workingMem.CurrentFocus; }
        }

        /// <summary> Return a Set of the declass instances
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual ICollection<object> Defclasses
        {
            get { return defclass.Values; }
        }

        /// <summary> Returns a list of the function groups. If a function is not in a group,
        /// Get the complete list of functions using getAllFunctions instead.
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual IList FunctionGroups
        {
            get { return functionGroups; }
        }

        /// <summary> Returns a collection of the function instances
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual ICollection<object> AllFunctions
        {
            get { return functions.Values; }
        }

        public virtual RootNode RootNode
        {
            get { return root; }
        }

        public virtual InterpretedFunction InterpretedFunction
        {
            set { intrFunction = value; }
        }

        /// <summary>
        /// set the focus to a different module
        /// </summary>
        /// <value>The focus.</value>
        public virtual String Focus
        {
            set
            {
                IModule mod = workingMem.findModule(value);
                if (mod != null)
                {
                    workingMem.CurrentModule = mod;
                }
            }
        }

        /// <summary>
        /// Rete class Contains a list of items that can be watched. Call the method
        /// with one of the four types:<br/> activations<br/> all<br/> facts<br/>
        /// rules<br/>
        /// </summary>
        /// <value>The watch.</value>
        public virtual WatchType Watch
        {
            set
            {
                if (value == WatchType.WATCH_ACTIVATIONS)
                {
                    workingMem.Agenda.Watch = true;
                }
                else if (value == WatchType.WATCH_ALL)
                {
                    workingMem.Agenda.Watch = true;
                    workingMem.WatchFact = true;
                    workingMem.WatchRules = true;
                }
                else if (value == WatchType.WATCH_FACTS)
                {
                    workingMem.WatchFact = true;
                }
                else if (value == WatchType.WATCH_RULES)
                {
                    workingMem.WatchRules = true;
                }
            }
        }

        /// <summary>
        /// Call the method with the type to unwatch activations<br/> facts<br/>
        /// rules<br/>
        /// </summary>
        /// <value>The un watch.</value>
        public virtual WatchType UnWatch
        {
            set
            {
                if (value == WatchType.WATCH_ACTIVATIONS)
                {
                    workingMem.Agenda.Watch = false;
                }
                else if (value == WatchType.WATCH_ALL)
                {
                    workingMem.Agenda.Watch = false;
                    workingMem.WatchFact = false;
                    workingMem.WatchRules = false;
                }
                else if (value == WatchType.WATCH_FACTS)
                {
                    workingMem.WatchFact = false;
                }
                else if (value == WatchType.WATCH_RULES)
                {
                    workingMem.WatchRules = false;
                }
            }
        }

        /// <summary>
        /// To turn on profiling, call the method with the appropriate parameter. The
        /// parameters are defined in Rete class as static int values.
        /// </summary>
        /// <value>The profile.</value>
        public virtual ProfileType Profile
        {
            set
            {
                if (value == ProfileType.PROFILE_ADD_ACTIVATION)
                {
                    workingMem.Agenda.ProfileAdd = true;
                }
                else if (value == ProfileType.PROFILE_ASSERT)
                {
                    workingMem.ProfileAssert = true;
                }
                else if (value == ProfileType.PROFILE_ALL)
                {
                    workingMem.Agenda.ProfileAdd = true;
                    workingMem.ProfileAssert = true;
                    workingMem.ProfileFire = true;
                    workingMem.ProfileRetract = true;
                    workingMem.Agenda.ProfileRemove = true;
                }
                else if (value == ProfileType.PROFILE_FIRE)
                {
                    workingMem.ProfileFire = true;
                }
                else if (value == ProfileType.PROFILE_RETRACT)
                {
                    workingMem.ProfileRetract = true;
                }
                else if (value == ProfileType.PROFILE_RM_ACTIVATION)
                {
                    workingMem.Agenda.ProfileRemove = true;
                }
            }
        }

        /// <summary>
        /// To turn off profiling, call the method with the appropriate parameter.
        /// The parameters are defined in Rete class as static int values.
        /// </summary>
        /// <value>The profile off.</value>
        public virtual ProfileType ProfileOff
        {
            set
            {
                if (value == ProfileType.PROFILE_ADD_ACTIVATION)
                {
                    workingMem.Agenda.ProfileAdd = false;
                }
                else if (value == ProfileType.PROFILE_ASSERT)
                {
                    workingMem.ProfileAssert = false;
                }
                else if (value == ProfileType.PROFILE_ALL)
                {
                    workingMem.Agenda.ProfileAdd = false;
                    workingMem.ProfileAssert = false;
                    workingMem.ProfileFire = false;
                    workingMem.ProfileRetract = false;
                    workingMem.Agenda.ProfileRemove = false;
                }
                else if (value == ProfileType.PROFILE_FIRE)
                {
                    workingMem.ProfileFire = false;
                }
                else if (value == ProfileType.PROFILE_RETRACT)
                {
                    workingMem.ProfileRetract = false;
                }
                else if (value == ProfileType.PROFILE_RM_ACTIVATION)
                {
                    workingMem.Agenda.ProfileRemove = false;
                }
            }
        }

        /// <summary>
        /// return a list of all the facts including deffacts and shadow of objects
        /// </summary>
        /// <value>All facts.</value>
        /// <returns>
        /// </returns>
        public virtual IList<Object> AllFacts
        {
            get { return workingMem.AllFacts; }
        }

        /// <summary>
        /// Return a list of the objects asserted in the working memory
        /// </summary>
        /// <value>The objects.</value>
        /// <returns>
        /// </returns>
        public virtual IList<Object> Objects
        {
            get { return workingMem.Objects; }
        }

        /// <summary>
        /// Return a list of all facts which are not shadows of Objects.
        /// </summary>
        /// <value>The deffacts.</value>
        /// <returns>
        /// </returns>
        public virtual IList<Object> Deffacts
        {
            get { return workingMem.Deffacts; }
        }

        /// <summary> return just the number of deffacts
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual int DeffactCount
        {
            get { return workingMem.DeffactMap.Count; }
        }

        /// <summary>
        /// Method returns a list of the rules that fired
        /// </summary>
        /// <value>The rules fired.</value>
        /// <returns>
        /// </returns>
        public virtual IList<Object> RulesFired
        {
            get
            {
                List<Object> list = new List<Object>();
                list.AddRange(rulesFired.Keys);
                return list;
            }
        }

        /// <summary>
        /// Gets the agenda.
        /// </summary>
        /// <value>The agenda.</value>
        public virtual Agenda Agenda
        {
            get { return workingMem.Agenda; }
        }

        /// <summary>
        /// Gets the rule compiler.
        /// </summary>
        /// <value>The rule compiler.</value>
        public virtual IRuleCompiler RuleCompiler
        {
            get { return workingMem.RuleCompiler; }
        }

        /// <summary>
        /// Gets the working memory.
        /// </summary>
        /// <value>The working memory.</value>
        public virtual IWorkingMemory WorkingMemory
        {
            get { return workingMem; }
        }

        /// <summary>
        /// Gets the strategy.
        /// </summary>
        /// <value>The strategy.</value>
        public virtual IStrategy Strategy
        {
            get { return workingMem.Strategy; }
        }

        /// <summary>
        /// Gets the activation list.
        /// </summary>
        /// <value>The activation list.</value>
        public virtual IActivationList ActivationList
        {
            get { return workingMem.CurrentFocus.AllActivations; }
        }

        /// <summary>
        /// Gets the object count.
        /// </summary>
        /// <value>The object count.</value>
        public virtual int ObjectCount
        {
            get { return workingMem.DynamicFacts.Count + workingMem.StaticFacts.Count; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [validate rules].
        /// </summary>
        /// <value><c>true</c> if [validate rules]; otherwise, <c>false</c>.</value>
        public virtual bool ValidateRules
        {
            get { return workingMem.RuleCompiler.ValidateRule; }

            set { workingMem.RuleCompiler.ValidateRule = value; }
        }

        public virtual MessageRouter MessageRouter
        {
            get { return router; }
        }

        public virtual Deftemplate InitFact
        {
            get { return initFact; }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            close();
        }

        #endregion

        public event MessageHandler Message;

        /// <summary>
        /// For now, this is not implemented
        /// </summary>
        /// <param name="messageEventArgsRenamed">The event_ renamed.</param>
        public virtual void ruleAdded(object sender, CompileMessageEventArgs messageEventArgsRenamed)
        {
            TraceLogger.Instance.Info("added: " + messageEventArgsRenamed.Message);
        }

        /// <summary>
        /// For now, this is not implemented
        /// </summary>
        /// <param name="messageEventArgsRenamed">The event_ renamed.</param>
        public virtual void ruleRemoved(object sender, CompileMessageEventArgs messageEventArgsRenamed)
        {
            TraceLogger.Instance.Info("removed: " + messageEventArgsRenamed.Message);
        }

        /// <summary>
        /// For now, this is not implemented
        /// </summary>
        /// <param name="messageEventArgsRenamed">The event_ renamed.</param>
        public virtual void compileError(object sender, CompileMessageEventArgs messageEventArgsRenamed)
        {
            TraceLogger.Instance.Warn(messageEventArgsRenamed.Message);
        }

        private void InitBlock()
        {
            defclass = CollectionFactory.localMap();
            templateToDefclass = CollectionFactory.localMap();
            functions = CollectionFactory.localMap();
            outputStreams = CollectionFactory.localMap();
            listeners = new List<Object>();
            functionGroups = new List<Object>();
            //log = new Log4netLogger(typeof (Rete));
            router = new MessageRouter(this);
            initFact = new InitialFact();
            deffunctions = new DeffunctionGroup();
            root = new RootNode();
            rulesFired = CollectionFactory.localMap();
        }

        /// <summary> initialization logic should go here
        /// </summary>
        protected internal void init()
        {
            loadBuiltInFunctions();
            declareInitialFact();
        }

        protected internal virtual void loadBuiltInFunctions()
        {
            // load the engine relate functions like declaring rules, templates, etc
            RuleEngineFunctions rulefs = new RuleEngineFunctions();
            functionGroups.Add(rulefs);
            rulefs.loadFunctions(this);
            // load boolean functions
            BooleanFunctions boolfs = new BooleanFunctions();
            functionGroups.Add(boolfs);
            boolfs.loadFunctions(this);
            // load IO functions
            IOFunctions iofs = new IOFunctions();
            functionGroups.Add(iofs);
            iofs.loadFunctions(this);
            // load math functions
            MathFunctions mathfs = new MathFunctions();
            functionGroups.Add(mathfs);
            mathfs.loadFunctions(this);
            declareFunction(new IfFunction());
            functionGroups.Add(deffunctions);
        }

        protected internal virtual void clearBuiltInFunctions()
        {
            functions.Clear();
        }

        protected void startLog()
        {
            TraceLogger.Instance.Info("Creshendo started");
        }

        protected internal virtual void declareInitialFact()
        {
            declareTemplate(initFact);
            Deffact ifact = (Deffact) initFact.createFact(null, null, nextFactId());
            try
            {
                assertFact(ifact);
            }
            catch (AssertException e)
            {
                // an error should not occur
                TraceLogger.Instance.Info(e);
            }
        }

        // ----- methods for clearing rules and facts ----- //

        /// <summary> Clear the objects from the working memory
        /// </summary>
        public virtual void clearObjects()
        {
            workingMem.clearObjects();
        }

        /// <summary> Clear the deffacts from the working memory. This does not include facts
        /// asserted using assertObject.
        /// </summary>
        public virtual void clearFacts()
        {
            workingMem.clearFacts();
        }

        /// <summary> Clear all objects and deffacts
        /// </summary>
        public virtual void clearAll()
        {
            workingMem.DynamicFacts.Clear();
            workingMem.StaticFacts.Clear();
            workingMem.DeffactMap.Clear();
            workingMem.clear();
            // now we Clear all the rules and templates
            defclass.Clear();
            ProfileStats.reset();
            lastFactId = 1;
            lastNodeId = 1;
            clearBuiltInFunctions();
            loadBuiltInFunctions();
            declareInitialFact();
        }

        /// <summary> Method will Clear the engine of all rules, facts and objects.
        /// </summary>
        public virtual void close()
        {
            workingMem.clear();
            defclass.Clear();
            workingMem.DeffactMap.Clear();
            workingMem.DynamicFacts.Clear();
            functions.Clear();
            workingMem.InitialFacts.Clear();
            listeners.Clear();
            workingMem.StaticFacts.Clear();
            CompilerProvider.reset();
        }

        protected internal virtual void addRuleFired(IRule r)
        {
            rulesFired.Put(r, null);
        }

        /// <summary>
        /// this is useful for debugging purposes. clips allows the user to fire 1
        /// rule at a time.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public virtual int fire(int count)
        {
            int counter = 0;
            if (workingMem.CurrentFocus.ActivationCount > 0)
            {
                IActivation actv = null;
                if (workingMem.profileFire())
                {
                    ProfileStats.startFire();
                }
                while ((actv = workingMem.CurrentFocus.nextActivation(this)) != null && counter < count)
                {
                    try
                    {
                        if (workingMem.watchRules())
                        {
                            writeMessage("==> fire: " + actv.toPPString() + "\r\n", "t");
                        }
                        pushScope(actv.Rule);
                        actv.executeActivation(this);
                        actv.clear();
                        popScope();
                        counter++;
                        addRuleFired(actv.Rule);
                    }
                    catch (ExecuteException e)
                    {
                        // we need to report the exception
                        TraceLogger.Instance.Debug(e);
                        // we break out of the for loop
                        break;
                    }
                }
                if (workingMem.profileFire())
                {
                    ProfileStats.endFire();
                }
            }
            return counter;
        }

        /// <summary>
        /// this is the normal fire. it will fire all the rules that have matched
        /// completely.
        /// </summary>
        /// <returns></returns>
        public virtual int fire()
        {
            if (workingMem.CurrentFocus.ActivationCount > 0)
            {
                // we reset the rules fire count
                firingcount = 0;
                IActivation actv = null;
                if (workingMem.profileFire())
                {
                    ProfileStats.startFire();
                }
                while ((actv = workingMem.CurrentFocus.nextActivation(this)) != null)
                {
                    try
                    {
                        if (workingMem.watchRules())
                        {
                            writeMessage("==> fire: " + actv.toPPString() + "\r\n", "t");
                        }
                        // we push the rule into the scope
                        pushScope(actv.Rule);
                        actv.executeActivation(this);
                        //actv.clear(); //TODO Hack not sure
                        popScope();
                        firingcount++;
                        addRuleFired(actv.Rule);
                        actv.clear();
                    }
                    catch (ExecuteException e)
                    {
                        TraceLogger.Instance.Debug(e);
                    }
                }
                if (workingMem.profileFire())
                {
                    ProfileStats.endFire();
                }
                return firingcount;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// method is used to fire an activation immediately
        /// </summary>
        /// <param name="act">The act.</param>
        protected internal virtual void fireActivation(IActivation act)
        {
            if (act != null)
            {
                try
                {
                    pushScope(act.Rule);
                    act.executeActivation(this);
                    //act.clear(); TODO HACK
                    popScope();
                    firingcount++;
                    addRuleFired(act.Rule);
                    act.clear();
                }
                catch (ExecuteException e)
                {
                    TraceLogger.Instance.Debug(e);
                }
            }
        }

        // ----- defmodule related methods ----- //


        /// <summary>
        /// Adds the module.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual bool addModule(String name)
        {
            if (workingMem.addModule(name) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Adds the module.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="setfocus">if set to <c>true</c> [setfocus].</param>
        /// <returns></returns>
        public virtual IModule addModule(String name, bool setfocus)
        {
            IModule mod = workingMem.addModule(name);
            if (setfocus)
            {
                workingMem.CurrentModule = mod;
            }
            return mod;
        }

        /// <summary>
        /// Removes the module.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual IModule removeModule(String name)
        {
            return workingMem.removeModule(name);
        }

        /// <summary>
        /// Finds the module.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual IModule findModule(String name)
        {
            return workingMem.findModule(name);
        }

        /// <summary>
        /// Finds the function.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual IFunction findFunction(String name)
        {
            return (IFunction) functions.Get(name);
        }

        /// <summary>
        /// Method will look up the Template using the class
        /// </summary>
        /// <param name="clazz">The clazz.</param>
        /// <returns></returns>
        public virtual ITemplate findTemplate(Type clazz)
        {
            Object templ = defclass.Get(clazz);
            if (templ != null)
            {
                return (ITemplate) templ;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// find the template starting with other modules and ending with the main
        /// module.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual ITemplate findTemplate(String name)
        {
            ITemplate tmpl = null;
            IEnumerator itr = workingMem.Agenda.modules.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                Object val = itr.Current;
                if (val != workingMem.Main)
                {
                    tmpl = ((Defmodule) val).getTemplate(name);
                }
                if (tmpl != null)
                {
                    break;
                }
            }
            // if it wasn't found in any other module, check main
            if (tmpl == null)
            {
                tmpl = workingMem.Main.getTemplate(name);
            }
            return tmpl;
        }

        /// <summary>
        /// Users can write query adapters to execute an external query. The method
        /// will find all Query adapters for a given Template. There can be zero or
        /// more adapter registered for a given template. If the template hasn't been
        /// defined, the method return null.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        public virtual IQuery findQueryAdapter(ITemplate template)
        {
            return null;
        }

        /// <summary>
        /// Registers the query adapter.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        public virtual void registerQueryAdapter(IQuery adapter)
        {
        }

        // -------- method for declaring an object ------------------ //

        /// <summary>
        /// Declares the object.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="parent">The parent.</param>
        public virtual void declareObject(String className, String templateName, String parent)
        {
            //UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
            try
            {
                //UPGRADE_TODO: Format of parameters of method 'java.lang.Class.forName' are different in the equivalent in .NET. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1092"'
                Type clzz = Type.GetType(className);
                declareObject(clzz, templateName, parent);
            }
            catch (System.Exception e)
            {
                // for now do nothing, but we should report the error for real
                TraceLogger.Instance.Debug(e);
                throw e;
            }
        }

        /// <summary>
        /// Declares the object.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public virtual void declareObject(Type obj)
        {
            declareObject(obj, null);
        }

        /// <summary>
        /// Declare a class with a specific template name
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="templateName">Name of the template.</param>
        public virtual void declareObject(Type obj, String templateName)
        {
            declareObject(obj, templateName, String.Empty);
        }

        /// <summary>
        /// Declares the object.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="parent">The parent.</param>
        public virtual void declareObject(Type obj, String templateName, Type parent)
        {
            declareObject(obj, templateName, parent.FullName);
        }

        /// <summary>
        /// Declares the object.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="parent">-
        /// the parent template</param>
        public virtual void declareObject(Type obj, String templateName, String parent)
        {
            // if the class hasn't already been declared, we create a defclass
            // and deftemplate for the class.
            if (!defclass.ContainsKey(obj))
            {
                Defclass dclass = new Defclass(obj);
                defclass.Put(obj, dclass);
                if (templateName == null)
                {
                    templateName = obj.FullName;
                }
                templateToDefclass.Put(templateName, dclass);
                if (!CurrentFocus.containsTemplate(dclass))
                {
                    Deftemplate dtemp = null;
                    // if the parent is found, we set it
                    if (String.IsNullOrEmpty(parent) == false)
                    {
                        ITemplate ptemp = workingMem.CurrentFocus.findParentTemplate(parent);
                        if (ptemp != null)
                        {
                            dtemp = dclass.createDeftemplate(templateName, ptemp);
                            dtemp.Parent = ptemp;
                        }
                        else
                        {
                            // we need to throw an exception to let users know the
                            // parent template wasn't found
                        }
                    }
                    else
                    {
                        dtemp = dclass.createDeftemplate(templateName);
                    }
                    // the key for the deftemplate is the declass, this means
                    // that when we assert an object instance to the engine,
                    // we need to use the Class to lookup defclass and then
                    // use the defclass to lookup the deftemplate. Once we
                    // have the deftemplate, we can use it to create the shadow
                    // fact for the object instance.
                    if (dtemp != null)
                    {
                        CurrentFocus.addTemplate(dtemp, this, workingMem);
                        writeMessage(dtemp.Name, "t");
                    }
                }
            }
        }

        /// <summary>
        /// Finds the defclass.
        /// </summary>
        /// <param name="clazz">The clazz.</param>
        /// <returns></returns>
        public virtual Defclass findDefclass(Type clazz)
        {
            return (Defclass) defclass.Get(clazz);
        }

        /// <summary>
        /// Finds the defclass by template.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public virtual Defclass findDefclassByTemplate(String templateName)
        {
            return (Defclass) templateToDefclass.Get(templateName);
        }


        /// <summary>
        /// Implementation will lookup the defclass for a given object by using the
        /// Class as the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual Defclass findDefclass(Object key)
        {
            return (Defclass) defclass.Get(key.GetType());
        }

        /// <summary>
        /// method is specifically for templates that are declared in the shell and
        /// do not have a corresponding java class.
        /// </summary>
        /// <param name="temp">The temp.</param>
        public virtual void declareTemplate(ITemplate temp)
        {
            if (!CurrentFocus.containsTemplate(temp.Name))
            {
                // the module doesn't contain it, so we Add it
                CurrentFocus.addTemplate(temp, this, workingMem);
            }
        }

        /// <summary>
        /// To explicitly deploy a custom function, call the method with an instance
        /// of the function
        /// </summary>
        /// <param name="func">The func.</param>
        public virtual void declareFunction(IFunction func)
        {
            functions.Put(func.Name, func);
            if (func is InterpretedFunction)
            {
                deffunctions.addFunction(func);
            }
        }

        /// <summary>
        /// In some cases, we may want to declare a function under an alias. For
        /// example, Add can be alias as "+".
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <param name="func">The func.</param>
        public virtual void declareFunction(String alias, IFunction func)
        {
            functions.Put(alias, func);
        }

        /// <summary>
        /// Method will create an instance of the function and declare it. Once a
        /// function is declared, it can be used. All custom functions must be
        /// declared before they can be used.
        /// </summary>
        /// <param name="name">The name.</param>
        public virtual void declareFunction(String name)
        {
            //UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
            //UPGRADE_NOTE: Exception 'java.lang.InstantiationException' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
            try
            {
                //UPGRADE_TODO: Format of parameters of method 'java.lang.Class.forName' are different in the equivalent in .NET. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1092"'
                Type fclaz = Type.GetType(name);
                IFunction func = (IFunction) CreateNewInstance(fclaz);
                declareFunction(func);
            }
            catch (UnauthorizedAccessException e)
            {
                TraceLogger.Instance.Debug(e);
            }
            catch (System.Exception e)
            {
                TraceLogger.Instance.Debug(e);
            }
        }

        public Object CreateNewInstance(Type classType)
        {
            ConstructorInfo[] constructors = classType.GetConstructors();

            if (constructors.Length == 0)
                return null;

            ParameterInfo[] firstConstructor = constructors[0].GetParameters();
            int countParams = firstConstructor.Length;

            Type[] constructor = new Type[countParams];
            for (int i = 0; i < countParams; i++)
                constructor[i] = firstConstructor[i].ParameterType;

            return classType.GetConstructor(constructor).Invoke(new Object[] {});
        }

        /// <summary>
        /// Method will create in instance of the FunctionGroup class and load the
        /// functions.
        /// </summary>
        /// <param name="name">The name.</param>
        public virtual void declareFunctionGroup(String name)
        {
            try
            {
                Type fclaz = Type.GetType(name);
                IFunctionGroup group = (IFunctionGroup) CreateNewInstance(fclaz);
                declareFunctionGroup(group);
            }
            catch (UnauthorizedAccessException e)
            {
                TraceLogger.Instance.Debug(e);
            }
            catch (System.Exception e)
            {
                TraceLogger.Instance.Debug(e);
            }
        }

        /// <summary>
        /// Method will register the function of the FunctionGroup .
        /// </summary>
        /// <param name="functionGroup">FunctionGroup with the functions to register.</param>
        public virtual void declareFunctionGroup(IFunctionGroup functionGroup)
        {
            functionGroup.loadFunctions(this);
        }

        /// <summary>
        /// pass a filename to load the rules. The implementation uses BatchFunction
        /// to load the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public virtual void loadRuleset(String filename)
        {
            BatchFunction bf = (BatchFunction) functions.Get(BatchFunction.BATCH);
            IParameter[] params_Renamed = new IParameter[] {new ValueParam(Constants.STRING_TYPE, filename)};
            bf.executeFunction(this, params_Renamed);
        }

        /// <summary>
        /// load the rules from an inputstream. The implementation uses the Batch
        /// function to load the input.
        /// </summary>
        /// <param name="ins">The ins.</param>
        public virtual void loadRuleset(Stream ins)
        {
            BatchFunction bf = (BatchFunction) functions.Get(BatchFunction.BATCH);
            bf.parse(this, ins, null);
        }


        /// <summary>
        /// Declares the defglobal.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value_Renamed">The value_ renamed.</param>
        public virtual void declareDefglobal(String name, Object value_Renamed)
        {
            workingMem.Defglobals.declareDefglobal(name, value_Renamed);
        }

        /// <summary>
        /// Gets the defglobal value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual Object getDefglobalValue(String name)
        {
            return workingMem.Defglobals.getValue(name);
        }

        // -------------- Get / Set methods --------------------- //

        /// <summary>
        /// The current implementation will check to see if the variable is a
        /// defglobal. If it is, it will return the value. If not, it will see if
        /// there is an active rule and try to Get the local bound value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual Object getBinding(String name)
        {
            return workingMem.getBinding(name);
        }

        /// <summary>
        /// This is the main method for setting the bindings. The current
        /// implementation will check to see if the name of the variable begins and
        /// ends with "*". If it does, it will declare it as a defglobal. Otherwise,
        /// it will try to Add it to the rule being fired. Note: might need to have
        /// Add one for shell variables later.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value_Renamed">The value_ renamed.</param>
        public virtual void setBindingValue(String key, Object value_Renamed)
        {
            workingMem.setBindingValue(key, value_Renamed);
        }

        /// <summary>
        /// when a rule is active, it should push itself into the scopes. when
        /// the rule is done, it has to pop itself out of scope. The same applies
        /// to interpretedFunctions.
        /// </summary>
        /// <param name="s">The s.</param>
        public virtual void pushScope(IScope s)
        {
            workingMem.pushScope(s);
        }

        /// <summary>
        /// pop a scope out of the stack
        /// </summary>
        public virtual void popScope()
        {
            workingMem.popScope();
        }


        /// <summary>
        /// Get the shadow for the object
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual IFact getShadowFact(Object key)
        {
            IFact f = (IFact) workingMem.DynamicFacts.Get(key);
            if (f == null)
            {
                f = (IFact) workingMem.StaticFacts.Get(key);
            }
            return f;
        }

        /// <summary>
        /// changed the implementation so it searches for the fact by id.
        /// Starting with the HashMap for deffact, dynamic facts and finally
        /// static facts.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual IFact getFactById(long id)
        {
            return workingMem.getFactById(id);
        }


        // ----- method for adding output streams for spools ----- //
        /// <summary>
        /// this method is for adding printwriters for spools. the purpose of
        /// the spool function is to dump everything out to a file.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="writer">The writer.</param>
        public virtual void addPrintWriter(String name, TextWriter writer)
        {
            outputStreams.Put(name, writer);
        }

        /// <summary>
        /// It is up to spool function to make sure it removes the printer
        /// writer and closes it properly.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual TextWriter removePrintWriter(String name)
        {
            return (TextWriter) outputStreams.RemoveWithReturn(name);
        }

        // ----- method for writing messages out ----- //
        /// <summary>
        /// The method is called by classes to write watch, profiling and other
        /// messages to the output stream. There maybe 1 or more outputstreams.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public virtual void writeMessage(String msg)
        {
            writeMessage(msg, "t");
        }

        /// <summary>
        /// writeMessage will create a MessageEvent and pass it along to any
        /// channels. It will also write out all messages to all registered
        /// PrintWriters. For example, if there's a spool setup, it will write
        /// the messages to the printwriter.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="output">The output.</param>
        public virtual void writeMessage(String msg, String output)
        {
            if (Message != null)
            {
                Message(this, new MessageEventArgs(EventType.ENGINE, msg, output));
            }
            //router.postMessageEvent(new MessageEventArgs(EventType.ENGINE, msg, "t".Equals(output) ? router.CurrentChannelId : output));
            if (outputStreams.Count > 0)
            {
                IEnumerator itr = outputStreams.Values.GetEnumerator();
                while (itr.MoveNext())
                {
                    TextWriter wr = (TextWriter) itr.Current;
                    wr.Write(msg);
                    wr.Flush();
                }
            }
        }

        protected void OnMessage(MessageEventArgs e)
        {
            if (Message != null)
            {
                Message(this, e);
            }
        }

        /// <summary>
        /// The method will print out the node. It is up to the method to check if
        /// pretty printer is true and call the appropriate node method to Get the
        /// string.
        /// TODO - need to implement this
        /// </summary>
        /// <param name="node">The node.</param>
        public virtual void writeMessage(BaseNode node)
        {
        }

        /// <summary>
        /// Method will process the retractEvent, preferably with an event queue
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="fact">The fact.</param>
        public virtual void assertEvent(BaseNode node, IFact fact)
        {
            if (debug)
            {
                Trace.WriteLine("\"assert at nodeid=" + node.nodeID + " - " + node.ToString().Replace("\"", "'") + ":: with fact -" + fact.toFactString().Replace("\"", "'") + "::\"");
            }
            IEnumerator itr = listeners.GetEnumerator();
            while (itr.MoveNext())
            {
                EngineEventListener eel = (EngineEventListener) itr.Current;
                eel.eventOccurred(new EngineEvent(this, EngineEvent.ASSERT_EVENT, node, new IFact[] {fact}));
            }
        }

        /// <summary>
        /// Asserts the event.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="facts">The facts.</param>
        public virtual void assertEvent(BaseNode node, IFact[] facts)
        {
            if (debug)
            {
                if (node is TerminalNode)
                {
                    Trace.WriteLine(((TerminalNode) node).Rule.Name + " fired");
                }
                else
                {
                }
            }
            IEnumerator itr = listeners.GetEnumerator();
            while (itr.MoveNext())
            {
                EngineEventListener eel = (EngineEventListener) itr.Current;
                eel.eventOccurred(new EngineEvent(this, EngineEvent.ASSERT_EVENT, node, facts));
            }
        }

        /// <summary>
        /// Method will process the retractEvent, referably using an event queue
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="fact">The fact.</param>
        public virtual void retractEvent(BaseNode node, IFact fact)
        {
            IEnumerator itr = listeners.GetEnumerator();
            while (itr.MoveNext())
            {
                EngineEventListener eel = (EngineEventListener) itr.Current;
                eel.eventOccurred(new EngineEvent(this, EngineEvent.RETRACT_EVENT, node, new IFact[] {fact}));
            }
        }

        /// <summary>
        /// Retracts the event.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="facts">The facts.</param>
        public virtual void retractEvent(BaseNode node, IFact[] facts)
        {
            IEnumerator itr = listeners.GetEnumerator();
            while (itr.MoveNext())
            {
                EngineEventListener eel = (EngineEventListener) itr.Current;
                eel.eventOccurred(new EngineEvent(this, EngineEvent.ASSERT_EVENT, node, facts));
            }
        }

        /// <summary>
        /// the method calls WorkingMemory.assertObject
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="template">The template.</param>
        /// <param name="statc">if set to <c>true</c> [statc].</param>
        /// <param name="shadow">if set to <c>true</c> [shadow].</param>
        public virtual void assertObject(Object data, String template, bool statc, bool shadow)
        {
            workingMem.assertObject(data, template, statc, shadow);
        }

        /// <summary>
        /// By default assertObjects will assert with shadow and dynamic. It also
        /// assumes the classes aren't using an user defined template name.
        /// </summary>
        /// <param name="objs">The objs.</param>
        public virtual void assertObjects(IList objs)
        {
            workingMem.assertObjects(objs);
        }

        /// <summary>
        /// Retracts the object.
        /// </summary>
        /// <param name="data">The data.</param>
        public virtual void retractObject(Object data)
        {
            workingMem.retractObject(data);
        }

        /// <summary>
        /// Modify will call retract with the old fact, followed by updating the fact
        /// instance and asserting the fact.
        /// </summary>
        /// <param name="data">The data.</param>
        public virtual void modifyObject(Object data)
        {
            workingMem.modifyObject(data);
        }

        /// <summary>
        /// This method is explicitly used to assert facts.
        /// </summary>
        /// <param name="fact">The fact.</param>
        public virtual void assertFact(Deffact fact)
        {
            workingMem.assertFact(fact);
        }

        /// <summary>
        /// retract by fact id is slower than retracting by the deffact instance. the
        /// method will find the fact and then call retractFact(Deffact)
        /// </summary>
        /// <param name="id">The id.</param>
        public virtual void retractById(long id)
        {
            IEnumerator itr = workingMem.DeffactMap.Values.GetEnumerator();
            Deffact ft = null;
            while (itr.MoveNext())
            {
                Deffact f = (Deffact) itr.Current;
                if (f.FactId == id)
                {
                    ft = f;
                    break;
                }
            }
            if (ft != null)
            {
                retractFact(ft);
            }
        }

        /// <summary>
        /// Retract a fact directly
        /// </summary>
        /// <param name="fact">The fact.</param>
        public virtual void retractFact(Deffact fact)
        {
            workingMem.retractFact(fact);
        }

        /// <summary>
        /// Modify retracts the old fact and asserts the new fact. Unlike assertFact,
        /// modifyFact will not check to see if the fact already exists. This is
        /// because the old fact would already be unique.
        /// </summary>
        /// <param name="old">The old.</param>
        /// <param name="newfact">The newfact.</param>
        public virtual void modifyFact(Deffact old, Deffact newfact)
        {
            retractFact(old);
            assertFact(newfact);
        }

        /// <summary>
        /// Method will call resetObjects first, followed by resetFacts.
        /// </summary>
        public virtual void resetAll()
        {
            resetObjects();
            resetFacts();
        }

        /// <summary>
        /// Method will retract the objects and re-assert them. It does not reset the
        /// deffacts.
        /// </summary>
        public virtual void resetObjects()
        {
            try
            {
                IEnumerator itr = workingMem.StaticFacts.Values.GetEnumerator();
                while (itr.MoveNext())
                {
                    Deffact ft = (Deffact) itr.Current;
                    workingMem.retractFact(ft);
                }
                itr = workingMem.DynamicFacts.Values.GetEnumerator();
                while (itr.MoveNext())
                {
                    Deffact ft = (Deffact) itr.Current;
                    workingMem.retractFact(ft);
                }
                // now assert
                itr = workingMem.StaticFacts.Values.GetEnumerator();
                while (itr.MoveNext())
                {
                    Deffact ft = (Deffact) itr.Current;
                    workingMem.assertFact(ft);
                }
                itr = workingMem.DynamicFacts.Values.GetEnumerator();
                while (itr.MoveNext())
                {
                    Deffact ft = (Deffact) itr.Current;
                    workingMem.assertFact(ft);
                }
            }
            catch (RetractException e)
            {
                TraceLogger.Instance.Debug(e);
            }
            catch (AssertException e)
            {
                TraceLogger.Instance.Debug(e);
            }
        }

        /// <summary>
        /// Method will retract all the deffacts and then re-assert them. Reset does
        /// not reset the objects. To reset both the facts and objects, call
        /// resetAll. resetFacts handles deffacts which are not derived from objects.
        /// </summary>
        public virtual void resetFacts()
        {
            try
            {
                IList<Object> facts = new List<Object>(workingMem.DeffactMap.Values);
                IEnumerator itr = facts.GetEnumerator();
                while (itr.MoveNext())
                {
                    Deffact ft = (Deffact) itr.Current;
                    workingMem.retractFact(ft);
                }
                itr = facts.GetEnumerator();
                while (itr.MoveNext())
                {
                    Deffact ft = (Deffact) itr.Current;
                    workingMem.assertFact(ft);
                }
            }
            catch (RetractException e)
            {
                TraceLogger.Instance.Debug(e);
            }
            catch (AssertException e)
            {
                TraceLogger.Instance.Debug(e);
            }
        }

        /// <summary>
        /// This is temporary, it should be replaced with something like the current
        /// factHandleFactory().newFactHandle()
        /// </summary>
        /// <returns></returns>
        public virtual long nextFactId()
        {
            return lastFactId++;
        }


        /// <summary>
        /// return the Current rete node id for a new node
        /// </summary>
        /// <returns></returns>
        public virtual int nextNodeId()
        {
            return ++lastNodeId;
        }

        /// <summary>
        /// peak at the Current node id. Do not use this method to Get an id for the
        /// Current node. only nextNodeId() should be used to create new rete nodes.
        /// </summary>
        /// <returns></returns>
        public virtual int peakNextNodeId()
        {
            return lastNodeId + 1;
        }


        /// <summary>
        /// Prints the working memory.
        /// </summary>
        /// <param name="detailed">if set to <c>true</c> [detailed].</param>
        /// <param name="inputNodes">if set to <c>true</c> [input nodes].</param>
        public virtual void printWorkingMemory(bool detailed, bool inputNodes)
        {
            workingMem.printWorkingMemory(detailed, inputNodes);
        }

        /// <summary>
        /// not implemented yet
        /// </summary>
        /// <param name="event_Renamed">The event_ renamed.</param>
        public virtual void propertyChange(PropertyChangeEvent event_Renamed)
        {
            Object source = event_Renamed.getSource;
            try
            {
                modifyObject(source);
            }
            catch (RetractException e)
            {
                TraceLogger.Instance.Debug(e);
            }
            catch (AssertException e)
            {
                TraceLogger.Instance.Debug(e);
            }
        }

        /// <summary>
        /// Properties the has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Creshendo.Util.PropertyChangedHandlerEventArgs"/> instance containing the event data.</param>
        private void PropertyHasChanged(object sender, PropertyChangedHandlerEventArgs e)
        {
            Object source = sender;
            try
            {
                modifyObject(source);
            }
            catch (RetractException ex)
            {
                TraceLogger.Instance.Debug(ex);
            }
            catch (AssertException ex)
            {
                TraceLogger.Instance.Debug(ex);
            }
        }

        /// <summary>
        /// Add a listener if it isn't already a listener
        /// </summary>
        /// <param name="listen">The listen.</param>
        public virtual void addEngineEventListener(EngineEventListener listen)
        {
            if (!listeners.Contains(listen))
            {
                listeners.Add(listen);
            }
        }

        /// <summary>
        /// Remove a listener
        /// </summary>
        /// <param name="listen">The listen.</param>
        public virtual void removeEngineEventListener(EngineEventListener listen)
        {
            listeners.Remove(listen);
        }
    }
}