using System;

/// <summary> Copyright 2006 Alexander Wilden, Christoph Emonds, Sebastian Reinartz
/// *
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// *
/// http://ruleml-dev.sourceforge.net/
/// *
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// 
/// </summary>
namespace Creshendo.Util.Messagerouter
{
    /// <summary> An Exception that is thrown whenever the specified language is not supported by
    /// the MessageRouter.
    /// 
    /// </summary>
    /// <author>  Alexander Wilden, Christoph Emonds, Sebastian Reinartz
    /// 
    /// 
    /// </author>
    public class LanguageNotSupportedException : Exception
    {
        public LanguageNotSupportedException(String language) : base(language)
        {
        }

        public virtual String Language
        {
            get
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                return Message;
            }
        }
    }
}