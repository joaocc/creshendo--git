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
using Creshendo.Util;
using Creshendo.Util.Rete;
//using List<Object>=Creshendo.Util.List<Object>;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// 
    /// The purpose of the function is to print out the names of the rules
    /// and the comment.
    /// 
    /// </author>
    [Serializable]
    public class TemplatesFunction : IFunction
    {
        public const String LISTTEMPLATES = "list-deftemplates";
        public const String TEMPLATES = "templates";

        public TemplatesFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return TEMPLATES; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[0]; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            List<Object> templates = (List<Object>) engine.CurrentFocus.Templates;
            int count = templates.Count;
            IEnumerator itr = templates.GetEnumerator();
            while (itr.MoveNext())
            {
                ITemplate r = (ITemplate) itr.Current;
                engine.writeMessage(r.Name + Constants.LINEBREAK, "t");
            }
            engine.writeMessage("for a total of " + count + Constants.LINEBREAK, "t");
            DefaultReturnVector rv = new DefaultReturnVector();
            return rv;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(templates)";
        }

        #endregion
    }
}