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
using Creshendo.Util;
using Creshendo.Util.Rete;
//using IList=Creshendo.Util.IList;

namespace Creshendo.Functions
{
    /// <author>  Sebastian Reinartz
    /// 
    /// 
    /// </author>
    [Serializable]
    public class ModulesFunction : IFunction
    {
        public const String MODULES = "modules";

        public ModulesFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.STRING_TYPE; }
        }

        public virtual String Name
        {
            get { return MODULES; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            IList modules = (IList) engine.WorkingMemory.Modules;
            int count = modules.Count;
            IEnumerator itr = modules.GetEnumerator();
            while (itr.MoveNext())
            {
                IModule r = (IModule) itr.Current;
                engine.writeMessage(r.ModuleName + Constants.LINEBREAK, "t");
            }
            engine.writeMessage("for a total of " + count + Constants.LINEBREAK, "t");
            DefaultReturnVector rv = new DefaultReturnVector();
            return rv;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(set-focus)";
        }

        #endregion
    }
}