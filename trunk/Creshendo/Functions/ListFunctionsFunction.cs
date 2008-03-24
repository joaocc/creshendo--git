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
using System.Text;
using Creshendo.Util;
using Creshendo.Util.Rete;
//using IList=Creshendo.Util.IList;

namespace Creshendo.Functions
{
    [Serializable]
    public class ListFunctionsFunction : IFunction
    {
        public const String LIST_FUNCTIONS = "list-deffunctions";

        public ListFunctionsFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return LIST_FUNCTIONS; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[0]; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            IList fgroups = engine.FunctionGroups;
            IEnumerator itr = fgroups.GetEnumerator();
            int counter = 0;
            while (itr.MoveNext())
            {
                // we iterate over the function groups and print out the
                // functions in each group
                IFunctionGroup fg = (IFunctionGroup) itr.Current;
                engine.writeMessage("++++ " + fg.Name + " ++++" + Constants.LINEBREAK, "t");
                IEnumerator listitr = fg.listFunctions().GetEnumerator();
                while (listitr.MoveNext())
                {
                    IFunction f = (IFunction) listitr.Current;
                    engine.writeMessage("  " + f.Name + Constants.LINEBREAK, "t");
                    counter++;
                }
            }
            engine.writeMessage(counter + " functions" + Constants.LINEBREAK, "t");
            DefaultReturnVector ret = new DefaultReturnVector();
            return ret;
        }


        /// <summary> for now, just return the simple form. need to implement the method
        /// completely.
        /// </summary>
        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (indents > 0)
            {
                StringBuilder buf = new StringBuilder();
                for (int idx = 0; idx < indents; idx++)
                {
                    buf.Append(" ");
                }
                buf.Append("(list-deffunctions)");
                return buf.ToString();
            }
            else
            {
                return "(list-deffunctions)";
            }
        }

        #endregion
    }
}