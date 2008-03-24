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
using System.IO;
using System.Text;
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <summary> ListDirectory will print out the files and folders in a given
    /// directory. It's the same as dir in DOS and ls in unix.
    /// </summary>
    /// <author>  pete
    /// *
    /// 
    /// </author>
    [Serializable]
    public class ListDirectoryFunction : IFunction
    {
        public const String LIST_DIR = "list-dir";

        public ListDirectoryFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.RETURN_VOID_TYPE; }
        }

        public virtual String Name
        {
            get { return LIST_DIR; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[0]; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                FileInfo dir = new FileInfo(params_Renamed[0].StringValue);
                if (Directory.Exists(dir.FullName))
                {
                    FileInfo[] files = dir.Directory.GetFiles();
                    for (int idx = 0; idx < files.Length; idx++)
                    {
                        if (Directory.Exists(files[idx].FullName))
                        {
                            engine.writeMessage("d " + files[idx] + Constants.LINEBREAK);
                        }
                        else
                        {
                            engine.writeMessage("- " + files[idx] + Constants.LINEBREAK);
                        }
                    }
                    engine.writeMessage(files.Length + " files in the directory" + Constants.LINEBREAK, "t");
                }
                else
                {
                }
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            return ret;
        }


        /// <summary> for now, just return the simple form. need to implement the method
        /// completely.
        /// </summary>
        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (indents > 0)
            {
                StringBuilder buf = new StringBuilder();
                for (int idx = 0; idx < indents; idx++)
                {
                    buf.Append(" ");
                }
                buf.Append("(list-dir)");
                return buf.ToString();
            }
            else
            {
                return "(list-dir)";
            }
        }

        #endregion
    }
}