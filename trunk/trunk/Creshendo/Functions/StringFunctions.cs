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
    public class StringFunctions : IFunctionGroup
    {
        //UPGRADE_NOTE: The initialization of  'funcs' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private List<Object> funcs;

        public StringFunctions() 
        {
            InitBlock();
        }

        #region FunctionGroup Members

        public virtual String Name
        {
            get { return ("String functions"); }
        }


        public virtual void loadFunctions(Rete engine)
        {
            StringCompareFunction compare = new StringCompareFunction();
            engine.declareFunction(compare);
            funcs.Add(compare);
            StringIndexFunction indx = new StringIndexFunction();
            engine.declareFunction(indx);
            funcs.Add(indx);
            StringLengthFunction strlen = new StringLengthFunction();
            engine.declareFunction(strlen);
            funcs.Add(strlen);
            StringLowerFunction lower = new StringLowerFunction();
            engine.declareFunction(lower);
            funcs.Add(lower);
            StringReplaceFunction strrepl = new StringReplaceFunction();
            engine.declareFunction(strrepl);
            funcs.Add(strrepl);
            StringUpperFunction upper = new StringUpperFunction();
            engine.declareFunction(upper);
            funcs.Add(upper);
            SubStringFunction sub = new SubStringFunction();
            engine.declareFunction(sub);
            funcs.Add(sub);
            StringTrimFunction trim = new StringTrimFunction();
            engine.declareFunction(trim);
            funcs.Add(trim);
        }

        public virtual IList listFunctions()
        {
            return funcs;
        }

        #endregion

        private void InitBlock()
        {
            funcs = new List<Object>();
        }
    }
}