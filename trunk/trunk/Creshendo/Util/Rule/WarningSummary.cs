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
using System.Text;
using Creshendo.Util.Rete;

namespace Creshendo.Util.Rule
{
    /// <author>  Peter Lin
    /// *
    /// 
    /// </author>
    public class WarningSummary : ISummary
    {

        private String[] warnings;

        /// <summary> 
        /// </summary>
        public WarningSummary()
        {
            InitBlock();
        }

        #region Summary Members

        public virtual String Message
        {
            get
            {
                StringBuilder buf = new StringBuilder();
                for (int idx = 0; idx < warnings.Length; idx++)
                {
                    buf.Append(warnings[idx] + Constants.LINEBREAK);
                }
                return buf.ToString();
            }
        }

        public virtual String[] Messages
        {
            get { return warnings; }
        }

        public virtual void addMessage(String reason)
        {
            int len = warnings.Length;
            String[] newwarn = new String[len + 1];
            Array.Copy(warnings, 0, newwarn, 0, warnings.Length);
            newwarn[len] = reason;
            warnings = newwarn;
        }

        #endregion

        private void InitBlock()
        {
            warnings = new String[0];
        }
    }
}