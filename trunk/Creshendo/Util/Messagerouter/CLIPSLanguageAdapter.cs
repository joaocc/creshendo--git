using System;
using System.IO;

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
    /// <summary> A simple LanguageAdapter that accepts only CLIPS as language. Any command
    /// will directly be forwarded to the CLIPSParser.
    /// 
    /// </summary>
    /// <author>  Alexander Wilden, Christoph Emonds, Sebastian Reinartz
    /// 
    /// 
    /// </author>
    public class CLIPSLanguageAdapter : ILanguageAdapter
    {
        #region LanguageAdapter Members

        /// <summary>
        /// Returns a String-Array containing all supported languages.
        /// </summary>
        /// <value></value>
        /// <returns> The String-Array containing all supported languages.
        /// </returns>
        public virtual String[] SupportedLanguages
        {
            get { return new String[] {"CLIPS"}; }
        }

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
        public virtual String Evaluate(Rete.Rete engine, String command, String language)
        {
            StringReader reader = new StringReader(command);
            StringWriter writer = new StringWriter();
            ////	CLIPSParser parser = new CLIPSParser(engine, reader, writer, false);
            //	parser.startParser();
            //	String res = writer.toString();
            //	parser = null;
            return null;
        }

        #endregion
    }
}