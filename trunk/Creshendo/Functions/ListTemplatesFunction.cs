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
using Creshendo.Util;
using Creshendo.Util.Rete;
//using List<Object>=Creshendo.Util.List<Object>;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// 
    /// ListTemplates will list all the templates and print them out.
    /// 
    /// </author>
    [Serializable]
    public class ListTemplatesFunction : IFunction
    {
        public const String LISTTEMPLATES = "list-deftemplates";

        /// <summary> 
        /// </summary>
        public ListTemplatesFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return LISTTEMPLATES; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (String)}; }
        }


        /// <summary> The current implementation will print out all the templates in
        /// no specific order. The function does basically the same thing
        /// as CLIPS (list-deftemplates)
        /// </summary>
        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            List<Object> templ = (List<Object>) engine.CurrentFocus.Templates;
            IEnumerator itr = templ.GetEnumerator();
            while (itr.MoveNext())
            {
                ITemplate tp = (ITemplate) itr.Current;
                engine.writeMessage(tp.toPPString() + "\r\n", "t");
            }
            return new DefaultReturnVector();
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
                return "(list-deftemplates)";
            }
        }

        #endregion
    }
}