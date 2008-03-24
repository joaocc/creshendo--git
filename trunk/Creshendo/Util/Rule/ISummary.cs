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

namespace Creshendo.Util.Rule
{
    /// <summary> The purpose of Summary interface is to define common methods for
    /// getting the validation summary of a rule. If a rule passes validation
    /// there may not be any summary or the summary have no details.
    /// 
    /// If the validation failed, the summary should contain information about
    /// what failed and why.
    /// </summary>
    /// <author>  Peter Lin
    /// *
    /// 
    /// </author>
    public interface ISummary
    {
        /// <summary> Get the messages as an array of String
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        String[] Messages { get; }

        /// <summary> return the errors and warnings as a single string
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        String Message { get; }

        /// <summary> Add a message about the error or warning
        /// </summary>
        /// <param name="">reason
        /// 
        /// </param>
        void addMessage(String reason);
    }
}