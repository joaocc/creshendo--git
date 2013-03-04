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
using System.Collections.Generic;
using System.IO;
using Creshendo.Util.Rete;

namespace Creshendo.Util
{
    /// <summary> IOUtilities Contains some commonly used static methods for saving and
    /// loading files.
    /// </summary>
    /// <author>  Peter
    /// 
    /// </author>
    public class IOUtilities
    {
        public static bool saveFacts(IList<Object> facts, String output)
        {
            try
            {
                StreamWriter writer = new StreamWriter(output);
                IEnumerator<Object> itr = facts.GetEnumerator();
                while (itr.MoveNext())
                {
                    IFact f = (IFact) itr.Current;
                    writer.Write(f.toFactString());
                }
                writer.Flush();
                writer.Close();
                return true;
            }
            catch (IOException e)
            {
                return false;
            }
        }
    }
}