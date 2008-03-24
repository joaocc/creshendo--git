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
    /// The design of the agenda is based on CLIPS, which uses modules to
    /// contain different rulesets. When a new activation is added to the
    /// agenda, it is added to a specific module. By default, the rule
    /// engine creates a main module. If no additional modules are created,
    /// all activations are added to the main module. If there are multiple
    /// modules, the activation is added to the activation list of that
    /// given module.
    /// Only the activations of the current module will be fired.
    /// 
    /// </author>
    [Serializable]
    public class Agenda
    {
        private Rete engine = null;

        /// <summary> The org.jamocha.rete.util.List for the modules.
        /// </summary>
        //UPGRADE_NOTE: The initialization of  'modules' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        protected internal IGenericMap<object, object> modules;

        private bool profAdd = false;

        private bool profRm = false;
        private bool watch_Renamed_Field = false;

        /// <summary> The agenda takes an instance of Rete. the agenda needs a
        /// handle to the engine to do work.
        /// </summary>
        public Agenda(Rete engine) 
        {
            InitBlock();
            this.engine = engine;
        }

        public virtual bool Watch
        {
            set { watch_Renamed_Field = value; }
        }

        public virtual bool ProfileAdd
        {
            set { profAdd = value; }
        }

        public virtual bool ProfileRemove
        {
            set { profRm = value; }
        }

        private void InitBlock()
        {
            modules = CollectionFactory.localMap();
        }


        public virtual bool watch()
        {
            return watch_Renamed_Field;
        }


        public virtual bool profileAdd()
        {
            return profAdd;
        }


        public virtual bool profileRemove()
        {
            return profRm;
        }

        /// <summary> Add an activation to the agenda.
        /// </summary>
        /// <param name="">actv
        /// 
        /// </param>
        public virtual void addActivation(IActivation actv)
        {
            // the implementation should Get the current focus from Rete
            // and then Add the activation to the Module.
            if (profAdd)
            {
                addActivationWProfile(actv);
            }
            else
            {
                if (watch_Renamed_Field)
                {
                    engine.writeMessage("=> " + actv.toPPString() + "\r\n", "t");
                }
                actv.Rule.Module.addActivation(actv);
            }
        }

        /// <summary> if profiling is turned on, the method is called to Add
        /// new activations to the agenda
        /// </summary>
        /// <param name="">actv
        /// 
        /// </param>
        public virtual void addActivationWProfile(IActivation actv)
        {
            ProfileStats.startAddActivation();
            actv.Rule.Module.addActivation(actv);
            ProfileStats.endAddActivation();
        }

        /// <summary> Method is called to Remove an activation from the agenda.
        /// </summary>
        /// <param name="">actv
        /// 
        /// </param>
        public virtual void removeActivation(IActivation actv)
        {
            if (profRm)
            {
                removeActivationWProfile(actv);
            }
            else
            {
                if (watch_Renamed_Field)
                {
                    engine.writeMessage("<= " + actv.toPPString() + "\r\n", "t");
                }
                actv.Rule.Module.removeActivation(actv);
            }
        }

        /// <summary> if the profiling is turned on for Remove, the method is
        /// called to Remove activations.
        /// </summary>
        /// <param name="">actv
        /// 
        /// </param>
        public virtual void removeActivationWProfile(IActivation actv)
        {
            ProfileStats.startRemoveActivation();
            actv.Rule.Module.removeActivation(actv);
            ProfileStats.endRemoveActivation();
        }

        /// <summary> Clear will Clear all the modules and Remove all activations
        /// </summary>
        public virtual void clear()
        {
            IEnumerator itr = modules.Keys.GetEnumerator();
            while (itr.MoveNext())
            {
                Object key = itr.Current;
                IModule mod = (IModule) modules.Get(key);
                mod.clear();
            }
            modules.Clear();
        }
    }
}