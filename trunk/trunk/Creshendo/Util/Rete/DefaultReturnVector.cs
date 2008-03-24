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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// Basic implementation of ReturnVector used by functions to return
    /// the results.
    /// 
    /// </author>
    public class DefaultReturnVector : IReturnVector
    {
        //UPGRADE_NOTE: The initialization of  'items' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        protected internal List<Object> items;


        /// <summary> 
        /// </summary>
        public DefaultReturnVector() 
        {
            InitBlock();
        }

        #region ReturnVector Members

        /// <summary> the implementation returns itself, since ReturnVector extends
        /// IEnumerator interface.
        /// </summary>
        public virtual IEnumerator Iterator
        {
            get { return items.GetEnumerator(); }
        }

        public virtual void clear()
        {
            items.Clear();
        }

        /// <summary> Current implementation returns the Count of the Vector
        /// </summary>
        public virtual int size()
        {
            return items.Count;
        }


        /// <summary> Return the first item in the vector
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual IReturnValue firstReturnValue()
        {
            //UPGRADE_TODO: Method java.util.Vector.Get was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
            return (IReturnValue) items[0];
        }

        public virtual void addReturnValue(IReturnValue val)
        {
            items.Add(val);
        }

        #endregion

        private void InitBlock()
        {
            items = new List<Object>(2);
        }

        public override String ToString()
        {
            IEnumerator itr = Iterator;
            StringBuilder sb = new StringBuilder();
            while (itr.MoveNext())
            {
                IReturnValue rval = (IReturnValue) itr.Current;
                sb.Append(rval.StringValue).Append('\n');
            }
            return sb.ToString();
        }
    }
}