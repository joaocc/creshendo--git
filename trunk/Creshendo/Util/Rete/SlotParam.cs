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
    /// TODO To change the template for this generated type comment go to
    /// Window - Preferences - Java - Code Style - Code Templates
    /// 
    /// </author>
    public class SlotParam : AbstractParam
    {
        protected internal Slot slot = null;
        protected internal int valueType;

        /// <summary> 
        /// </summary>
        /// <param name="">type
        /// </param>
        /// <param name="">slot
        /// 
        /// </param>
        public SlotParam(Slot slot) 
        {
            InitBlock();
            this.slot = slot;
        }

        public override int ValueType
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rete.ReturnValue#getValueType()
			*/
            set { }
            get { return valueType; }
        }

        public override Object Value
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rete.ReturnValue#getValue()
			*/
            set { }
            get { return slot; }
        }

        public virtual Slot SlotValue
        {
            get { return slot; }
        }

        public override bool ObjectBinding
        {
            get { return false; }
        }

        private void InitBlock()
        {
            valueType = Constants.SLOT_TYPE;
        }

        /// <summary> Slot parameter is only used internally, so normal user functions
        /// should not need to deal with slot parameters.
        /// </summary>
        public override Object getValue(Rete engine, int valueType)
        {
            return slot;
        }

        /* (non-Javadoc)
		* @see woolfel.engine.rete.Parameter#reset()
		*/

        public override void reset()
        {
            slot = null;
        }
    }
}