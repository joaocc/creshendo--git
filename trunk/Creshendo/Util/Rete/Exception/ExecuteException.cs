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
    /// ExecuteException is only thrown when the RHS of the rule is executed.
    /// If the action didn't execute correctly, throw an exception with
    /// sufficient details to debug the issue.
    /// 
    /// </author>
    public class ExecuteException : System.Exception
    {
        public const String NULL_ACTION = "Could not execute the action. " + "The action was NULL";

        /// <summary> 
        /// </summary>
        public ExecuteException() 
        {
        }

        /// <param name="">message
        /// 
        /// </param>
        public ExecuteException(String message) : base(message)
        {
        }

        /// <param name="">message
        /// </param>
        /// <param name="">cause
        /// 
        /// </param>
        public ExecuteException(String message, System.Exception cause)
            : base(message, cause)
        {
        }

        /// <param name="">cause
        /// 
        /// </param>
        public ExecuteException(System.Exception cause)
            : base(cause.Message)
        {
        }
    }
}