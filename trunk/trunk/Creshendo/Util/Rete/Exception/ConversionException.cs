/*
* Copyright 2002-2007 Peter Lin Licensed under the Apache License, Version 2.0
* (the "License"); you may not use this file except in compliance with the
* License. You may obtain a copy of the License at
* http://jamocha.sourceforge.net/ Unless required by applicable law or
* agreed to in writing, software distributed under the License is distributed
* on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
* express or implied. See the License for the specific language governing
* permissions and limitations under the License.
*/
using System;

namespace Creshendo.Util.Rete.Exception
{
    /// <summary> conversion exception is used to handle conversion of strings to
    /// numbers and booleans.
    /// </summary>
    /// <author>  Peter Lin
    /// 
    /// </author>
    public class ConversionException : System.Exception
    {
        /// <summary> 
        /// </summary>
        public ConversionException()
        {
        }

        /// <param name="">message
        /// 
        /// </param>
        public ConversionException(String message) : base(message)
        {
        }

        //UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
        /// <param name="">cause
        /// 
        /// </param>
        public ConversionException(System.Exception cause)
            : base(cause.Message)
        {
        }

        //UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
        /// <param name="">message
        /// </param>
        /// <param name="">cause
        /// 
        /// </param>
        public ConversionException(String message, System.Exception cause)
            : base(message, cause)
        {
        }
    }
}