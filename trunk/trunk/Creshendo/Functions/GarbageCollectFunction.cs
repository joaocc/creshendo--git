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
using System.Diagnostics;
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    [Serializable]
    public class GarbageCollectFunction : IFunction
    {
        public const String GARBAGE_COLLECT = "gc";

        public GarbageCollectFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return GARBAGE_COLLECT; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[0]; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            Process rt = Process.GetCurrentProcess();
            GC.Collect();
            DefaultReturnVector ret = new DefaultReturnVector();
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(gc)";
        }

        #endregion
    }
}