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
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// 
    /// The purpose of spool function is to capture the output to a file,
    /// and make it easier to record what happens. This is inspired by
    /// Oracle SqlPlus spool function.
    /// 
    /// </author>
    [Serializable]
    public class SpoolFunction : IFunction
    {
        public const String SPOOL = "spool";

        /// <summary> 
        /// </summary>
        public SpoolFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return SPOOL; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            bool sp = true;
            if (params_Renamed != null && params_Renamed.Length >= 2)
            {
                String val = params_Renamed[0].StringValue;
                if (val.Equals("off"))
                {
                    // turn off spooling
                    String name = params_Renamed[1].StringValue;
                    TextWriter writer = engine.removePrintWriter(name);
                    if (writer != null)
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
                else
                {
                    // turn on spooling
                    // we expected a file name
                    String spname = params_Renamed[0].StringValue;
                    String fname = params_Renamed[1].StringValue;
                    try
                    {
                        FileInfo nfile = new FileInfo(fname);
                        nfile.Create();
                        FileStream fos = new FileStream(nfile.FullName, FileMode.Create);
                        StreamWriter writer = new StreamWriter(fos);
                        engine.addPrintWriter(spname, writer);
                    }
                    catch (FileNotFoundException e)
                    {
                        // we should report it
                        sp = false;
                    }
                    catch (IOException e)
                    {
                        sp = false;
                    }
                }
            }
            else
            {
                sp = false;
            }
            DefaultReturnVector ret = new DefaultReturnVector();
            DefaultReturnValue rv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, sp);
            ret.addReturnValue(rv);
            return ret;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            return "(spool <name> <file>| off <name>)";
        }

        #endregion
    }
}