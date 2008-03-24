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
    public class GetMembertFunction : IFunction
    {
        public const String GET_MEMBER = "Get-member";

        /// <summary> 
        /// </summary>
        public GetMembertFunction() 
        {
        }

        #region Function Members

        /// <summary> By default, the function returns Object type. Since the function
        /// can be used to call any number of getXXX methods and we wrap
        /// all primitives in their object equivalent, returning Object type
        /// makes the most sense.
        /// </summary>
        public virtual int ReturnType
        {
            get { return Constants.OBJECT_TYPE; }
        }

        public virtual String Name
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rete.Function#getName()
			*/

            get { return GET_MEMBER; }
        }

        /// <summary> The current implementation expects 3 parameters in the following
        /// sequence:<br/>
        /// BoundParam - the bound object
        /// StringParam - the slot name
        /// ValueParam - the value to set the field
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
            Object rtn = null;
            DefaultReturnVector drv = new DefaultReturnVector();
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
                    MethodInfo getm = dc.getWriteMethod(slot.StringValue);
                    try
                    {
                        rtn = getm.Invoke(instance, (Object[]) new Object[] {val});
                        int rtype = getMethodReturnType(getm);
                        DefaultReturnValue rvalue = new DefaultReturnValue(rtype, rtn);
                        drv.addReturnValue(rvalue);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        // TODO we should handle error, for now not implemented
                    }
                    catch (TargetInvocationException e)
                    {
                        // TODO we should handle error, for now not implemented
                    }
                }
            }
            return drv;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                return buf.ToString();
            }
            else
            {
                return "(Get-member)";
            }
        }

        #endregion

        /// <summary> For now, this utility method is here, but maybe I should move it
        /// to some place else later.
        /// </summary>
        /// <param name="">m
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual int getMethodReturnType(MethodInfo m)
        {
            if (m.ReturnType == typeof (String))
            {
                return Constants.STRING_TYPE;
            }
            else if (m.ReturnType == typeof (int) || m.ReturnType == (Object) typeof (Int32))
            {
                return Constants.INT_PRIM_TYPE;
            }
            else if (m.ReturnType == typeof (short) || m.ReturnType == (Object) typeof (Int16))
            {
                return Constants.SHORT_PRIM_TYPE;
            }
            else if (m.ReturnType == typeof (long) || m.ReturnType == (Object) typeof (Int64))
            {
                return Constants.LONG_PRIM_TYPE;
            }
            else if (m.ReturnType == typeof (float) || m.ReturnType == (Object) typeof (Single))
            {
                return Constants.FLOAT_PRIM_TYPE;
            }
            else if (m.ReturnType == typeof (double) || m.ReturnType == (Object) typeof (Double))
            {
                return Constants.DOUBLE_PRIM_TYPE;
            }
            else
            {
                return Constants.OBJECT_TYPE;
            }
        }
    }
}