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

namespace Creshendo.Util.Rete.Exception
{
    /// <author>  Peter Lin
    /// *
    /// TODO To change the template for this generated type comment go to
    /// Window - Preferences - Java - Code Style - Code Templates
    /// 
    /// </author>
    public class CompileRuleException : System.Exception
    {
        public const String ADD_FAILURE = "Unable to Add the rule, due to compilation.";
        public const String INVALID_RULE = "The rule was not added because it is invalid";

        /// <summary> 
        /// </summary>
        public CompileRuleException() 
        {
        }

        /// <param name="">message
        /// 
        /// </param>
        public CompileRuleException(String message) : base(message)
        {
        }

        //UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
        /// <param name="">message
        /// </param>
        /// <param name="">cause
        /// 
        /// </param>
        public CompileRuleException(String message, System.Exception cause)
            : base(message, cause)
        {
        }

        //UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
        /// <param name="">cause
        /// 
        /// </param>
        public CompileRuleException(System.Exception cause)
            : base(cause.Message)
        {
        }
    }
}