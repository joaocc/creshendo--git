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
    /// RuleEngineFunction is responsible for loading all the rule functions
    /// related to engine operation.
    /// 
    /// </author>
    [Serializable]
    public class BooleanFunctions : IFunctionGroup
    {
        //UPGRADE_NOTE: The initialization of  'funcs' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private List<object> funcs;

        public BooleanFunctions() 
        {
            InitBlock();
        }

        #region FunctionGroup Members

        public virtual String Name
        {
            get { return "Boolean functions"; }
        }


        public virtual void loadFunctions(Rete engine)
        {
            NotFunction not = new NotFunction();
            engine.declareFunction(not);
            funcs.Add(not);

            TrueFunction trueFunc = new TrueFunction();
            engine.declareFunction(trueFunc);
            funcs.Add(trueFunc);

            FalseFunction falseFunc = new FalseFunction();
            engine.declareFunction(falseFunc);
            funcs.Add(falseFunc);
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