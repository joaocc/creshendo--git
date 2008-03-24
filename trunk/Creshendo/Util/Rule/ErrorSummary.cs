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
    public class ErrorSummary : ISummary
    {
        //UPGRADE_NOTE: The initialization of  'errors' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private String[] errors;

        /// <summary> 
        /// </summary>
        public ErrorSummary()
        {
            InitBlock();
        }

        #region Summary Members

        public virtual String Message
        {
            get
            {
                StringBuilder buf = new StringBuilder();
                for (int idx = 0; idx < errors.Length; idx++)
                {
                    buf.Append(errors[idx] + Constants.LINEBREAK);
                }
                return buf.ToString();
            }
        }

        public virtual String[] Messages
        {
            get { return errors; }
        }

        public virtual void addMessage(String reason)
        {
            int len = errors.Length;
            String[] newerr = new String[len + 1];
            Array.Copy(errors, 0, newerr, 0, errors.Length);
            newerr[len] = reason;
            errors = newerr;
        }

        #endregion

        private void InitBlock()
        {
            errors = new String[0];
        }
    }
}