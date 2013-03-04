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
using System.Text;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// NSFact stands for Non-Shadow Fact. NSFact is different than
    /// Deffact which is a shadow fact for an object instance. NSFact
    /// should only be used for cases where fact modification isn't
    /// needed. In all cases where the application expects to modify
    /// facts in the reasoning cycle, Deffacts should be used. Using
    /// NSFact for situations where facts are modified or asserted
    /// during the reasoning cycle will produce unreliable results.
    /// It will violate the principle of truth maintenance, which
    /// means the final result is true and accurate.
    /// 
    /// Cases where NSFact is useful are routing scenarios where the
    /// facts are filtered to determien where they should go. In
    /// cases like that, the consequence produces results which are
    /// used by the application, but aren't used by the rule engine
    /// for reasoning.
    /// 
    /// </author>
    [Serializable]
    public class NSFact : Deffact
    {
        private Defclass dclazz = null;
        private Deftemplate deftemplate = null;

        /// <summary> the Fact id must be unique, since we use it for the indexes
        /// </summary>
        private long id;

        private Object objInstance;
        private Slot[] slots = null;
        private long timeStamp_Renamed_Field = 0;

        /// <summary> 
        /// </summary>
        public NSFact(Deftemplate template, Defclass clazz, Object instance, Slot[] values, long id): base(template,instance,values,id)
        {
            deftemplate = template;
            dclazz = clazz;
            objInstance = instance;
            slots = values;
            this.id = id;
            timeStamp_Renamed_Field = (DateTime.Now.Ticks - 621355968000000000)/10000;
        }

        #region Fact Members

        /// <summary> The object instance for the fact
        /// </summary>
        public override Object ObjectInstance
        {
            get { return objInstance; }
        }

        /// <summary> Return the unique fact id
        /// </summary>
        public override long FactId
        {
            get { return id; }
        }

        /// <summary> Return the deftemplate for the fact
        /// </summary>
        public override Deftemplate Deftemplate
        {
            get { return deftemplate; }
        }

        /// <summary> The implementation gets the Defclass and passes the 
        /// objectInstance to invoke the read method.
        /// </summary>

        public override Object getSlotValue(int id)
        {
            return dclazz.getSlotValue(id, objInstance);
        }

        /// <summary> 
        /// </summary>
        public override int getSlotId(String name)
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


        /// <summary> The method will return the Fact as a string
        /// </summary>
        public override String toFactString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("(" + deftemplate.Name + " ");
            for (int idx = 0; idx < slots.Length; idx++)
            {
                buf.Append("(" + slots[idx].Name + " " + dclazz.getSlotValue(idx, objInstance).ToString() + ") ");
            }
            buf.Append(")");
            return buf.ToString();
        }


        /// <summary> Non-Shadow Fact does not implement this, since this method
        /// doesn't apply to facts derived from objects.
        /// </summary>
        public override void updateSlots(Rete engine, Slot[] updates)
        {
        }


        /// <summary> the implementation returns nano time
        /// </summary>
        public override long timeStamp()
        {
            return timeStamp_Renamed_Field;
        }

        /// <summary> Clear will set all the references to null. this makes sure
        /// objects are GC.
        /// </summary>
        public override void clear()
        {
            slots = null;
            objInstance = null;
            deftemplate = null;
            id = 0;
        }

        protected internal override int slotHash()
        {
            if (objInstance != null)
                return objInstance.GetHashCode();

            int hash = 0;
            for (int idx = 0; idx < slots.Length; idx++)
            {
                hash += slots[idx].Name.GetHashCode() + slots[idx].Value.GetHashCode();
            }
            return hash;
        }
/*
        /// <summary> non shadow fact does not implement the method, since it
        /// doesn't apply.
        /// </summary>
        public override EqualityIndex equalityIndex()
        {
            if (objInstance != null)
                return objInstance.GetHashCode();
            return base.equalityIndex();
        }
        */
        #endregion
    }
}