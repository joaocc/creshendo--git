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
using Creshendo.Util.Collections;
using Creshendo.Util.Rete;
//using List<Object>=Creshendo.Util.List<Object>;

namespace Creshendo.Functions
{
    /// Peter Lin
    /// 
    /// PPrintTemplate stands for Pretty Print deftemplate. It does the same
    /// thing as (ppdeftemplate in CLIPS.
    /// 
    /// 
    [Serializable]
    public class PPrintTemplateFunction : IFunction
    {
        public const String PPTEMPLATES = "ppdeftemplate";

        /// <summary> 
        /// </summary>
        public PPrintTemplateFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return PPTEMPLATES; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (String)}; }
        }


        /// <summary> the function will printout one or more templates. This implementation
        /// is slightly different than CLIPS in that it can take one or more
        /// template names. The definition in CLIPS beginners guide states the 
        /// function does the following: (ppdeftemplate &lt;deftemplate-name>)
        /// </summary>
        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            GenericHashMap<object, object> filter = new GenericHashMap<object, object>();
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is ValueParam)
                    {
                        Object df = ((ValueParam) params_Renamed[idx]).Value;
                        filter.Put(df, df);
                    }
                }
            }
            List<Object> templ = (List<Object>) engine.CurrentFocus.Templates;
            IEnumerator itr = templ.GetEnumerator();
            while (itr.MoveNext())
            {
                ITemplate tp = (ITemplate) itr.Current;
                if (filter.Get(tp.Name) != null)
                {
                    engine.writeMessage(tp.toPPString() + "\r\n", "t");
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