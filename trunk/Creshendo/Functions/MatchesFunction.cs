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
using Creshendo.Util.Collections;
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// 
    /// MatchesFunction will print out all partial matches including alpha and 
    /// beta nodes.
    /// 
    /// </author>
    [Serializable]
    public class MatchesFunction : IFunction
    {
        public const String MATCHES = "matches";

        /// <summary> 
        /// </summary>
        public MatchesFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return MATCHES; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (String[])}; }
        }


        /// <summary> If the function is called without any parameters, it prints out
        /// all the memories. if parameters are passed, the output will be
        /// filtered.
        /// </summary>
        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            GenericHashMap<Object, Object> filter = new GenericHashMap<Object, Object>();
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                // now we populate the filter
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is ValueParam)
                    {
                        filter.Put(((ValueParam) params_Renamed[idx]).StringValue, null);
                    }
                    else if (params_Renamed[idx] is BoundParam)
                    {
                        // for now, BoundParam is not supported
                    }
                }
            }
            engine.WorkingMemory.printWorkingMemory(filter);
            return new DefaultReturnVector();
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(matches)\n" + "Function description:\n" + "\tPrints out all partial matches including alpha and beta nodes.";
        }

        #endregion
    }
}