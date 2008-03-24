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
using System.Reflection;
using System.Text;
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// *
    /// SetMemberFunction is equivalent to JESS set-member function. This is a completely
    /// clean implementation from scratch. The name and function signature are similar,
    /// but the design and implementation are different. The design of the function is
    /// strongly influenced by CLIPS, since the primary goal is full CLIPS compatability.
    /// 
    /// </author>
    [Serializable]
    public class SetMembertFunction : IFunction
    {
        public const String SET_MEMBER = "set-member";

        /// <summary> 
        /// </summary>
        public SetMembertFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rete.Function#getReturnType()
			*/

            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rete.Function#getName()
			*/

            get { return SET_MEMBER; }
        }

        /// <summary> The current implementation expects 3 parameters in the following
        /// sequence:<br/>
        /// BoundParam
        /// StringParam
        /// ValueParam
        /// <br/>
        /// Example: (set-member ?objectVariable slotName value)
        /// </summary>
        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (BoundParam), typeof (StringParam), typeof (ValueParam)}; }
        }


        /* (non-Javadoc)
		* @see woolfel.engine.rete.Function#executeFunction(woolfel.engine.Creshendo.Util.Rete.Rete, woolfel.engine.rete.Parameter[])
		*/

        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            if (engine != null && params_Renamed != null && params_Renamed.Length == 3)
            {
                BoundParam bp = (BoundParam) params_Renamed[0];
                StringParam slot = (StringParam) params_Renamed[1];
                ValueParam val = (ValueParam) params_Renamed[2];
                Object instance = bp.ObjectRef;
                Defclass dc = engine.findDefclass(instance);
                // we check to make sure the Defclass exists
                if (dc != null)
                {
                    MethodInfo setm = dc.getWriteMethod(slot.StringValue);
                    try
                    {
                        setm.Invoke(instance, (Object[]) new Object[] {val});
                    }
                    catch (UnauthorizedAccessException e)
                    {
                    }
                    catch (TargetInvocationException e)
                    {
                    }
                }
            }
            return new DefaultReturnVector();
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            StringBuilder buf = new StringBuilder();
            return buf.ToString();
        }

        #endregion
    }
}