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
    /// <author>  Peter Lin
    /// 
    /// Deffact is a concrete implementation of Fact interface. It is
    /// equivalent to deffact in CLIPS.
    /// 
    /// </author>
    public class Deffact : IFact
    {
        protected internal Slot[] boundSlots = null;
        protected internal Deftemplate deftemplate = null;
        private EqualityIndex Eindex = null;
        protected internal bool hasBinding_Renamed_Field = false;

        /// <summary> the Fact id must be unique, since we use it for the indexes
        /// </summary>
        protected internal long id;

        protected internal Object objInstance;

        protected internal Slot[] slots = null;

        private long timeStamp_Renamed_Field = 0;

        /// <summary> this is the default constructor
        /// </summary>
        /// <param name="">instance
        /// </param>
        /// <param name="">values
        /// 
        /// </param>
        public Deffact(Deftemplate template, Object instance, Slot[] values, long id)
        {
            deftemplate = template;
            objInstance = instance;
            slots = values;
            this.id = id;
            timeStamp_Renamed_Field = (DateTime.Now.Ticks - 621355968000000000)/10000;
        }

        /// <summary> if the factId is -1, the fact will Get will the Current fact id
        /// from Rete and set it. Otherwise, the fact will use the same one.
        /// </summary>
        /// <param name="">engine
        /// 
        /// </param>
        public virtual Rete setFactId
        {
            set
            {
                if (id == - 1)
                {
                    id = value.nextFactId();
                }
            }
        }

        #region Fact Members

        /// <summary> If the fact is a shadow fact, it will return the
        /// object instance. If the fact is just a deffact
        /// and isn't a shadow fact, it return null.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual Object ObjectInstance
        {
            get { return objInstance; }
        }

        /// <summary> Return the long factId
        /// </summary>
        public virtual long FactId
        {
            get { return id; }
        }

        /// <summary> Return the deftemplate for the fact
        /// </summary>
        public virtual Deftemplate Deftemplate
        {
            get { return deftemplate; }
        }

        /// <summary> Method returns the value of the given slot at the
        /// id.
        /// </summary>
        /// <param name="">id
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual Object getSlotValue(int id)
        {
            return slots[id].Value;
        }

        /// <summary> Method will iterate over the slots until finds the match.
        /// If no match is found, it return -1.
        /// </summary>
        public virtual int getSlotId(String name)
        {
            int col = - 1;
            for (int idx = 0; idx < slots.Length; idx++)
            {
                if (slots[idx].Name.Equals(name))
                {
                    col = idx;
                    break;
                }
            }
            return col;
        }


        /// <summary> Method will return the fact in a string format.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual String toFactString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("f-" + id + " (" + deftemplate.Name);
            if (slots.Length > 0)
            {
                buf.Append(" ");
            }
            for (int idx = 0; idx < slots.Length; idx++)
            {
                buf.Append("(" + slots[idx].Name + " " + ConversionUtils.formatSlot(slots[idx].Value) + ") ");
            }
            buf.Append(")");
            return buf.ToString();
        }

        /// <summary> Returns the string format for the fact without the fact-id. this is used
        /// to make sure that if an user asserts an equivalent fact, we can easily
        /// check it.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual EqualityIndex equalityIndex()
        {
            if (Eindex == null)
            {
                Eindex = new EqualityIndex(this);
            }
            return Eindex;
        }

        /// <summary> update the slots
        /// </summary>
        public virtual void updateSlots(Rete engine, Slot[] updates)
        {
            for (int idx = 0; idx < updates.Length; idx++)
            {
                Slot uslot = updates[idx];
                if (uslot.Value is BoundParam)
                {
                    BoundParam bp = (BoundParam) uslot.Value;
                    Object val = engine.getBinding(bp.VariableName);
                    slots[uslot.Id].Value = val;
                }
                else
                {
                    slots[uslot.Id].Value = uslot.Value;
                }
            }
            timeStamp_Renamed_Field = (DateTime.Now.Ticks - 621355968000000000)/10000;
        }


        /// <summary> the implementation returns nano time
        /// </summary>
        public virtual long timeStamp()
        {
            return timeStamp_Renamed_Field;
        }

        /// <summary> this will make sure the fact is GC immediately
        /// </summary>
        public virtual void clear()
        {
            deftemplate = null;
            objInstance = null;
            slots = null;
            id = 0;
            timeStamp_Renamed_Field = 0;
        }

        #endregion

        /// <summary> 
        /// </summary>
        /// <param name="">util
        /// 
        /// </param>
        public virtual void compileBinding(Rule.IRule util)
        {
            List<object> list = new List<Object>();
            for (int idx = 0; idx < slots.Length; idx++)
            {
                if (slots[idx].Value is BoundParam)
                {
                    hasBinding_Renamed_Field = true;
                    list.Add(slots[idx]);
                    BoundParam bp = (BoundParam) slots[idx].Value;
                    Binding bd = util.getBinding(bp.VariableName);
                    if (bd != null)
                    {
                        bp.rowId = bd.LeftRow;
                        bp.column = bd.LeftIndex;
                    }
                }
            }
            if (list.Count > 0)
            {
                Slot[] ary = new Slot[list.Count];
                list.CopyTo(ary,0);
                boundSlots = ary;
            }
        }

        /// <summary> In some cases, a deffact may have bindings. This is a design choice. When
        /// rules are parsed and compiled, actions that assert facts are converted to
        /// Deffact instances with BoundParam for the slot value.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool hasBinding()
        {
            return hasBinding_Renamed_Field;
        }

        public virtual void resolveValues(Rete engine, IFact[] triggerFacts)
        {
            for (int idx = 0; idx < boundSlots.Length; idx++)
            {
                if (boundSlots[idx] is MultiSlot)
                {
                    // for multislot we have to resolve each slot
                    Object[] mvals = (Object[]) ((MultiSlot) boundSlots[idx]).Value;
                    for (int mdx = 0; mdx < mvals.Length; mdx++)
                    {
                        if (mvals[mdx] is BoundParam)
                        {
                            BoundParam bp = (BoundParam) mvals[mdx];
                            bp.ResolvedValue = engine.getBinding(bp.VariableName);
                        }
                    }
                }
                else if (boundSlots[idx].Value is BoundParam)
                {
                    BoundParam bp = (BoundParam) boundSlots[idx].Value;
                    if (bp.column > - 1)
                    {
                        bp.Facts = triggerFacts;
                    }
                    else
                    {
                        bp.ResolvedValue = engine.getBinding(bp.VariableName);
                    }
                }
            }
        }

        public virtual String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("(" + deftemplate.Name);
            if (slots.Length > 0)
            {
                buf.Append(" ");
            }
            for (int idx = 0; idx < slots.Length; idx++)
            {
                if (slots[idx].Value is BoundParam)
                {
                    BoundParam bp = (BoundParam) slots[idx].Value;
                    buf.Append("(" + slots[idx].Name + " ?" + bp.VariableName + ") ");
                }
                else
                {
                    buf.Append("(" + slots[idx].Name + " " + ConversionUtils.formatSlot(slots[idx].Value) + ") ");
                }
            }
            buf.Append(")");
            return buf.ToString();
        }

        /// <summary> this is used by the EqualityIndex class
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        protected internal virtual int slotHash()
        {
            int hash = 0;
            for (int idx = 0; idx < slots.Length; idx++)
            {
                hash += slots[idx].Name.GetHashCode() + slots[idx].Value.GetHashCode();
            }
            return hash;
        }


        /// <summary> this is used to reset the id, in the event an user tries to
        /// assert the same fact again, we reset the id to the existing one.
        /// </summary>
        /// <param name="">fact
        /// 
        /// </param>
        protected internal virtual void resetID(Deffact fact)
        {
            id = fact.id;
        }

        /// <summary> the current implementation only compares the values, since the slot
        /// names are equal. It would be a waste of time to compare the slot
        /// names. The exception to the case is when a deftemplate is changed.
        /// Since that feature isn't supported yet, it's currently not an issue.
        /// Even if updating deftemplates is added in the future, the deffacts
        /// need to be updated. If the deffacts weren't updated, it could lead
        /// to NullPointerExceptions.
        /// </summary>
        /// <param name="">fact
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool slotEquals(Deffact fact)
        {
            bool eq = true;
            Slot[] cslots = fact.slots;
            for (int idx = 0; idx < slots.Length; idx++)
            {
                if (!slots[idx].Value.Equals(cslots[idx].Value))
                {
                    eq = false;
                    break;
                }
            }
            return eq;
        }

        /// <summary> Convienance method for cloning a fact. If a slot's value is a BoundParam,
        /// the cloned fact uses the value of the BoundParam.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual Deffact cloneFact()
        {
            Deffact newfact = new Deffact(deftemplate, objInstance, deftemplate.cloneAllSlots(), - 1);
            Slot[] slts = newfact.slots;
            for (int idx = 0; idx < slts.Length; idx++)
            {
                // probably need to revisit this and make sure
                if (slots[idx] is MultiSlot)
                {
                    // it's multislot so we have to replace the bound values
                    // correctly
                    MultiSlot ms = (MultiSlot) slots[idx];
                    Object[] sval = (Object[]) ms.Value;
                    Object[] mval = new Object[sval.Length];
                    for (int mdx = 0; mdx < mval.Length; mdx++)
                    {
                        Object v = sval[mdx];
                        if (v is BoundParam)
                        {
                            mval[mdx] = ((BoundParam) v).Value;
                        }
                        else
                        {
                            mval[mdx] = v;
                        }
                    }
                    slts[idx].Value = mval;
                }
                else if (slots[idx].Value is BoundParam)
                {
                    if (slts[idx].ValueType == Constants.STRING_TYPE)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                        slts[idx].Value = ((BoundParam) slots[idx].Value).Value.ToString();
                    }
                    else
                    {
                        slts[idx].Value = ((BoundParam) slots[idx].Value).Value;
                    }
                }
                else
                {
                    slts[idx].Value = slots[idx].Value;
                }
            }
            return newfact;
        }
    }
}