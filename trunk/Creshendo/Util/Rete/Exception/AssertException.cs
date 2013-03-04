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
    /// 
    /// AssertException should be thrown if a node encounters issues matching a fact.
    /// Normally, this should not occur. If it does, it generally means there's a bug
    /// in the core RETE nodes.
    /// 
    /// </author>
    public class AssertException : System.Exception
    {
        /// <summary> 
        /// </summary>
        public AssertException() 
        {
        }

        /// <param name="">message
        /// 
        /// </param>
        public AssertException(String message) : base(message)
        {
        }

        /// <param name="">message
        /// </param>
        /// <param name="">cause
        /// 
        /// </param>
        public AssertException(String message, System.Exception cause) : base(message, cause)
        {
        }

        /// <param name="">cause
        /// 
        /// </param>
        public AssertException(System.Exception cause)
            : base(cause.Message)
        {
        }
    }
}