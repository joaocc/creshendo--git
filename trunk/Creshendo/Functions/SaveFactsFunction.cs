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
using System.IO;
using Creshendo.Util;
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// 
    /// Facts function will printout all the facts, not including any
    /// initial facts which are internal to the rule engine.
    /// 
    /// </author>
    [Serializable]
    public class SaveFactsFunction : IFunction
    {
        public const String SAVE_FACTS = "save-facts";

        /// <summary> 
        /// </summary>
        public SaveFactsFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return SAVE_FACTS; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam), typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            bool saved = false;
            bool sortid = true;
            DefaultReturnVector rv = new DefaultReturnVector();
            if (params_Renamed != null && params_Renamed.Length >= 1)
            {
                if (params_Renamed[1] != null && params_Renamed[1].StringValue.Equals("template"))
                {
                    sortid = false;
                }
                try
                {
                    StreamWriter writer = new StreamWriter(params_Renamed[0].StringValue);
                    System.Collections.Generic.IList<Object> facts = engine.AllFacts;
                    Object[] sorted = null;
                    if (sortid)
                    {
                        sorted = FactUtils.sortFacts(facts);
                    }
                    else
                    {
                        sorted = FactUtils.sortFactsByTemplate(facts);
                    }
                    for (int idx = 0; idx < sorted.Length; idx++)
                    {
                        Deffact ft = (Deffact) sorted[idx];
                        writer.Write(ft.toPPString() + Constants.LINEBREAK);
                    }
                    writer.Close();
                    saved = true;
                }
                catch (IOException e)
                {
                    // we should log this
                }
            }
            DefaultReturnValue drv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, saved);
            rv.addReturnValue(drv);
            return rv;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(save-facts [filename] [sort(id|template)])";
        }

        #endregion
    }
}