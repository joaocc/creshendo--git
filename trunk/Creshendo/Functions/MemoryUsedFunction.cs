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
    public class MemoryUsedFunction : IFunction
    {
        public const String MEMORY_FREE = "mem-used";

        public MemoryUsedFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return MEMORY_FREE; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[0]; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            Process rt = Process.GetCurrentProcess();
            long free = GC.GetTotalMemory(false);
            long total = GC.GetTotalMemory(false);
            long used = total - free;
            used = used/1024/1024;
            total = total/1024;
            long mbtotal = total/1024;
            engine.writeMessage(used.ToString() + "Mb used of " + mbtotal.ToString() + "Mb " + Constants.LINEBREAK, "t");
            DefaultReturnVector ret = new DefaultReturnVector();
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(mem-free)";
        }

        #endregion
    }
}