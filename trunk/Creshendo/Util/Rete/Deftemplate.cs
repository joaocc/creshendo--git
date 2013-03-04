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
using System.Collections.Generic;
using System.Text;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin Deftemplate is equivalent to CLIPS deftemplate<br/>
    /// 
    /// Deftemplate Contains an array of slots that represent un-ordered facts.
    /// Currently, deftemplate does not have a reference to the corresponding Defclass,
    /// since many objects in java.beans and java.lang.reflect are not serializable.
    /// This means when ever we need to lookup the defclass from the deftemplate, we
    /// have to use the String form and do the lookup.
    /// 
    /// Some general design notes about the current implementation. In the case where
    /// a class is declared to create the deftemplate, the order of the slots are
    /// based on java Introspection. In the case where an user declares the
    /// deftemplate from console or directly, the order is the same as the string
    /// equivalent. The current implementation does not address redeclaring a
    /// deftemplate for a couple of reasons. The primary one is how does it affect
    /// the existing RETE nodes. One possible approach is to always Add new slots to
    /// the end of the deftemplate and ignore the explicit order. Another is to
    /// recompute the deftemplate, binds and all nodes. The second approach is very
    /// costly and would make redeclaring a deftemplate undesirable.
    /// 
    /// </author>
    [Serializable]
    public class Deftemplate : ITemplate
    {
        /// <summary> Defclass and Deftemplate are decoupled, so it uses a string
        /// to look up the Defclass rather than have a link to it. This
        /// is because the reflection classes are not serializable.
        /// </summary>
        private String defclass = null;

        private ITemplate parent = null;

        protected internal Slot[] slots;

        private String templateName = null;
        private bool watch = false;

        public Deftemplate(String name, String defclass, Slot[] slots)
        {
            templateName = name;
            this.defclass = defclass;
            this.slots = slots;
        }

        public Deftemplate(String name, String defclass, Slot[] slots, ITemplate parent) : this(name, defclass, slots)
        {
            this.parent = parent;
        }

        public Deftemplate(String name)
        {
            templateName = name;
        }

        public Deftemplate(String name, ITemplate parent)
        {
            templateName = name;
            this.parent = parent;
        }

        public Deftemplate()
        {
        }

        #region Template Members

        public virtual ITemplate Parent
        {
            get { return parent; }

            set { parent = value; }
        }

        /// <summary> return whether the deftemplate should be watched
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> set whether the deftemplate should be watched
        /// 
        /// </summary>
        /// <param name="">watch
        /// 
        /// </param>
        public virtual bool Watch
        {
            get { return watch; }

            set { watch = value; }
        }

        /// <summary> the template name is an alias for an object
        /// 
        /// </summary>
        /// <param name="">name
        /// 
        /// </param>
        public virtual String Name
        {
            get { return templateName; }
        }

        /// <summary> Get the class the deftemplate represents
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual String ClassName
        {
            get { return defclass; }
        }

        /// <summary> Return the number of slots in the deftemplate
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual int NumberOfSlots
        {
            get { return slots.Length; }
        }

        /// <summary> Return all the slots
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual Slot[] AllSlots
        {
            get { return slots; }
        }


        /// <summary> A convienance method for finding the slot matching the String name.
        /// 
        /// </summary>
        /// <param name="">name
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual Slot getSlot(String name)
        {
            for (int idx = 0; idx < slots.Length; idx++)
            {
                if (slots[idx].Name.Equals(name))
                {
                    return slots[idx];
                }
            }
            return null;
        }

        /// <summary> Get the Slot at the given column id
        /// 
        /// </summary>
        /// <param name="">id
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual Slot getSlot(int id)
        {
            return slots[id];
        }

        /// <summary> Look up the column index of the slot
        /// 
        /// </summary>
        /// <param name="">name
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual int getColumnIndex(String name)
        {
            for (int idx = 0; idx < slots.Length; idx++)
            {
                if (slots[idx].Name.Equals(name))
                {
                    return idx;
                }
            }
            return - 1;
        }

        /// <summary> Method will create a Fact from the given object instance
        /// 
        /// </summary>
        /// <param name="">data
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual IFact createFact(Object data, Defclass clazz, long id)
        {
            // first we clone the slots
            Slot[] values = cloneAllSlots();
            // now we set the values
            for (int idx = 0; idx < values.Length; idx++)
            {
                Object val = clazz.getSlotValue(idx, data);
                if (val == null)
                {
                    values[idx].Value = Constants.NIL_SYMBOL;
                }
                else
                {
                    values[idx].Value = val;
                }
            }
            Deffact newfact = new Deffact(this, data, values, id);
            return newfact;
        }

        /// <summary> If any slot has a usecount greater than 0, we return true.
        /// </summary>
        public virtual bool inUse()
        {
            for (int idx = 0; idx < slots.Length; idx++)
            {
                if (slots[idx].NodeCount > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary> Method will return a string format with the int type code for the slot
        /// type
        /// </summary>
        public virtual String toString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("(" + templateName + " ");
            for (int idx = 0; idx < slots.Length; idx++)
            {
                buf.Append("(" + slots[idx].Name + " (type " + ConversionUtils.getTypeName(slots[idx].ValueType) + ") ) ");
            }
            if (defclass != null)
            {
                buf.Append("[" + defclass + "] ");
            }
            buf.Append(")");
            return buf.ToString();
        }

        /// <summary> Method will generate a pretty printer format of the Deftemplate
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("(" + templateName + Constants.LINEBREAK);
            for (int idx = 0; idx < slots.Length; idx++)
            {
                buf.Append("  (" + slots[idx].Name + " (type " + ConversionUtils.getTypeName(slots[idx].ValueType) + ") )" + Constants.LINEBREAK);
            }
            if (defclass != null)
            {
                buf.Append("[" + defclass + "] ");
            }
            buf.Append(")");
            return buf.ToString();
        }

        #endregion

        /// <summary> checkName will see if the user defined the module to declare the
        /// template. if it is, it will create the module and return it.
        /// 
        /// </summary>
        /// <param name="">engine
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual IModule checkName(Rete engine)
        {
            if (templateName.IndexOf("::") > 0)
            {
                String[] sp = templateName.Split("::".ToCharArray());
                templateName = sp[1];
                return engine.addModule(sp[0], false);
            }
            else
            {
                return null;
            }
        }

        /// <summary> convienance method for incrementing the column's use count.
        /// </summary>
        /// <param name="">name
        /// 
        /// </param>
        public virtual void incrementColumnUseCount(String name)
        {
            for (int idx = 0; idx < slots.Length; idx++)
            {
                if (slots[idx].Name.Equals(name))
                {
                    slots[idx].incrementNodeCount();
                }
            }
        }

        /// <summary> Method takes a list of Slots and creates a deffact from it.
        /// 
        /// </summary>
        /// <param name="">data
        /// </param>
        /// <param name="">id
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual IFact createFact(IList data, long id)
        {
            Slot[] values = cloneAllSlots();
            IEnumerator itr = data.GetEnumerator();
            while (itr.MoveNext())
            {
                Slot s = (Slot) itr.Current;
                for (int idx = 0; idx < values.Length; idx++)
                {
                    if (values[idx].Name.Equals(s.Name))
                    {
                        if (s.Value == null)
                        {
                            values[idx].Value = Constants.NIL_SYMBOL;
                        }
                        else if (values[idx].ValueType == Constants.STRING_TYPE && !(s.Value is BoundParam))
                        {
                            values[idx].Value = s.Value.ToString();
                        }
                        else
                        {
                            values[idx].Value = s.Value;
                        }
                    }
                }
            }
            Deffact newfact = new Deffact(this, null, values, id);
            // we call this to create the string used to map the fact.
            newfact.equalityIndex();
            return newfact;
        }

        public virtual IFact createFact(Object[] data, long id)
        {
            Slot[] values = cloneAllSlots();
            List<Object> bslots = new List<Object>();
            bool hasbinding = false;
            for (int idz = 0; idz < data.Length; idz++)
            {
                Slot s = (Slot) data[idz];
                for (int idx = 0; idx < values.Length; idx++)
                {
                    if (values[idx].Name.Equals(s.Name))
                    {
                        if (s is MultiSlot)
                        {
                            // since the value is multislot, we have to
                            // check for boundparams
                            MultiSlot ms = (MultiSlot) s;
                            Object[] mvals = (Object[]) ms.Value;
                            for (int mdx = 0; mdx < mvals.Length; mdx++)
                            {
                                if (mvals[mdx] is BoundParam)
                                {
                                    bslots.Add((MultiSlot) ms.Clone());
                                    hasbinding = true;
                                    break;
                                }
                            }
                            values[idx].Value = s.Value;
                        }
                        else
                        {
                            if (s.Value == null)
                            {
                                values[idx].Value = Constants.NIL_SYMBOL;
                            }
                            else if (values[idx].ValueType == Constants.STRING_TYPE && !(s.Value is BoundParam))
                            {
                                values[idx].Value = s.Value.ToString();
                            }
                            else if (s.Value is BoundParam)
                            {
                                values[idx].Value = s.Value;
                                bslots.Add((Slot) s.Clone());
                                hasbinding = true;
                            }
                            else
                            {
                                values[idx].Value = s.Value;
                            }
                        }
                        break;
                    }
                }
            }
            Deffact newfact = new Deffact(this, null, values, id);
            if (hasbinding)
            {
                Slot[] slts2 = new Slot[bslots.Count];
                bslots.CopyTo(slts2,0);
                newfact.boundSlots = slts2;
                newfact.hasBinding_Renamed_Field = true;
            }
            // we call this to create the string used to map the fact.
            newfact.equalityIndex();
            return newfact;
        }

        public virtual IFact createTemporalFact(Object[] data, long id)
        {
            Slot[] values = cloneAllSlots();
            long expire = 0;
            String source = "";
            String service = "";
            int valid = 0;
            for (int idz = 0; idz < data.Length; idz++)
            {
                Slot s = (Slot) data[idz];
                // check to see if the slot is a temporal fact attribute
                if (isTemporalAttribute(s))
                {
                    if (s.Name.Equals(TemporalFact_Fields.EXPIRATION))
                    {
                        expire = Decimal.ToInt64(((Decimal) s.Value));
                    }
                    else if (s.Name.Equals(TemporalFact_Fields.SERVICE_TYPE))
                    {
                        service = (String) s.Value;
                    }
                    else if (s.Name.Equals(TemporalFact_Fields.SOURCE))
                    {
                        source = (String) s.Value;
                    }
                    else if (s.Name.Equals(TemporalFact_Fields.VALIDITY))
                    {
                        valid = Decimal.ToInt32(((Decimal) s.Value));
                    }
                }
                else
                {
                    for (int idx = 0; idx < values.Length; idx++)
                    {
                        if (values[idx].Name.Equals(s.Name))
                        {
                            if (s.Value == null)
                            {
                                values[idx].Value = Constants.NIL_SYMBOL;
                            }
                            else if (values[idx].ValueType == Constants.STRING_TYPE && !(s.Value is BoundParam))
                            {
                                values[idx].Value = s.Value.ToString();
                            }
                            else if (s.Value is BoundParam)
                            {
                                values[idx].Value = s.Value;
                            }
                            else
                            {
                                values[idx].Value = s.Value;
                            }
                        }
                    }
                }
            }
            TemporalDeffact newfact = new TemporalDeffact(this, null, values, id);
            // we call this to create the string used to map the fact.
            newfact.ExpirationTime = expire;
            newfact.ServiceType = service;
            newfact.Source = source;
            newfact.Validity = valid;
            newfact.equalityIndex();
            return newfact;
        }

        public static bool isTemporalAttribute(Slot s)
        {
            if (s.Name.Equals(TemporalFact_Fields.EXPIRATION) || s.Name.Equals(TemporalFact_Fields.SERVICE_TYPE) || s.Name.Equals(TemporalFact_Fields.SOURCE) || s.Name.Equals(TemporalFact_Fields.VALIDITY))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary> clone the slots
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual Slot[] cloneAllSlots()
        {
            Slot[] cloned = new Slot[slots.Length];
            for (int idx = 0; idx < cloned.Length; idx++)
            {
                cloned[idx] = (Slot) slots[idx].Clone();
            }
            return cloned;
        }

        /// <summary> TODO - need to finish implementing this
        /// </summary>
        public virtual Deftemplate cloneDeftemplate()
        {
            Deftemplate dt = new Deftemplate(templateName, defclass, slots);

            return dt;
        }
    }
}