/*
* Copyright 2002-2007 Peter Lin
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
using System.Text;

namespace Creshendo.Util.Parser.Clips
{
    public class ParserUtils
    {
        /// <summary> convienant method to Get string literal
        /// 
        /// </summary>
        /// <param name="">text
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static String getStringLiteral(String text)
        {
            StringBuilder buf = new StringBuilder();
            int len = text.Length - 1;
            bool escaping = false;
            for (int i = 1; i < len; i++)
            {
                char ch = text[i];
                if (escaping)
                {
                    buf.Append(ch);
                    escaping = false;
                }
                else if (ch == '\\')
                {
                    escaping = true;
                }
                else
                {
                    buf.Append(ch);
                }
            }
            return buf.ToString();
        }

        /// <summary> convienant utility method to escape string literals
        /// 
        /// </summary>
        /// <param name="">text
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static String escapeStringLiteral(String text)
        {
            StringBuilder buffer = new StringBuilder();
            char[] chararray = text.ToCharArray();
            for (int idx = 0; idx < chararray.Length; idx++)
            {
                char chr = chararray[idx];
                if (chr == '"' || chr == '\\')
                {
                    buffer.Append('\\');
                }
                buffer.Append(chr);
            }
            return buffer.ToString();
        }
    }
}