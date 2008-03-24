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
    /// <summary> An Interface for LanguageAdapters used by the MessageRouter. A
    /// LanguageAdapter is responsible for the translation of a command into CLIPS,
    /// execute it in the Rete-engine and give back a result in the same language the
    /// command was send in. One LanguageAdapter may support multiple different
    /// languages.
    /// 
    /// </summary>
    /// <author>  Alexander Wilden, Christoph Emonds, Sebastian Reinartz
    /// 
    /// 
    /// </author>
    public interface ILanguageAdapter
    {
        /// <summary> Returns a String-Array containing all supported languages.
        /// 
        /// </summary>
        /// <returns> The String-Array containing all supported languages.
        /// 
        /// </returns>
        String[] SupportedLanguages { get; }

        /// <summary>
        /// Evaluates a command given in any language that is supported by this
        /// LanguageAdapter.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="command">The command.</param>
        /// <param name="language">The language.</param>
        /// <returns>
        /// The result returned from the Rete-engine in the given
        /// language.
        /// @throws ParseException
        /// </returns>
        String Evaluate(Rete.Rete engine, String command, String language);
    }
}