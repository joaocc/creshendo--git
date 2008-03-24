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
namespace Creshendo.Util.Rule
{
    /// <author>  Peter Lin
    /// *
    /// TODO To change the template for this generated type comment go to
    /// 
    /// </author>
    public class DefaultComplexity : IComplexity
    {
        /// <summary> 
        /// </summary>
        public DefaultComplexity() 
        {
            // TODO Auto-generated constructor stub
        }

        #region Complexity Members

        public virtual int Value
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rule.Complexity#getValue()
			*/

            get
            {
                // TODO Auto-generated method stub
                return 0;
            }
        }


        /* (non-Javadoc)
		* @see woolfel.engine.rule.Complexity#calculateComplexity()
		*/

        public virtual void calculateComplexity()
        {
            // TODO Auto-generated method stub
        }

        #endregion

        //[STAThread]
        //public static void  Main(System.String[] args)
        //{
        //}
    }
}