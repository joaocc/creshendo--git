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
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Creshendo.Util.Parser.Clips2;
using Creshendo.Util.Rete;
using Creshendo.Util.Rule;
//using ParseException=Creshendo.Util.Parser.Clips.ParseException;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// *
    /// Functional equivalent of (batch file.clp) in CLIPS and JESS.
    /// 
    /// </author>
    [Serializable]
    public class BatchFunction : IFunction
    {
        public const String BATCH = "batch";

        /// <summary> 
        /// </summary>
        public BatchFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return BATCH; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        /// <summary> method will attempt to load one or more files. If batch is called without
        /// any parameters, the function does nothing and just returns.
        /// </summary>
        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            DefaultReturnVector rv = new DefaultReturnVector();
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    try
                    {
                        String input = params_Renamed[idx].StringValue;
                        Stream inStream;
                        // Check for a protocol indicator at the beginning of the
                        // String. If we have one use a URL.
                        if (Regex.IsMatch(input, "^[a-zA-Z]+://.*"))
                        {
                            //UPGRADE_TODO: Class 'java.net.URL' was converted to a 'System.Uri' which does not throw an exception if a URL specifies an unknown protocol. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1132"'
                            Uri url = new Uri(input);
                            inStream = ((HttpWebRequest) WebRequest.Create(url)).GetResponse().GetResponseStream();
                            // Otherwise treat it as normal file on the Filesystem
                        }
                        else
                        {
                            inStream = new FileStream(new FileInfo(input).FullName, FileMode.Open, FileAccess.Read);
                        }
                        parse(engine, inStream, rv);
                        inStream.Close();
                        if (inStream is IDisposable)
                        {
                            inStream.Dispose();
                        }
                    }
                    catch (FileNotFoundException e)
                    {
                        // we should report the error
                        rv.addReturnValue(new DefaultReturnValue(Constants.BOOLEAN_OBJECT, false));
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                        engine.writeMessage(e.Message + Constants.LINEBREAK, Constants.DEFAULT_OUTPUT);
                    }
                    catch (IOException e)
                    {
                        rv.addReturnValue(new DefaultReturnValue(Constants.BOOLEAN_OBJECT, false));
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                        engine.writeMessage(e.Message + Constants.LINEBREAK, Constants.DEFAULT_OUTPUT);
                    }
                }
            }
            return rv;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(batch");
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    if (params_Renamed[idx] is BoundParam)
                    {
                        BoundParam bp = (BoundParam) params_Renamed[idx];
                        buf.Append(" ?" + bp.VariableName);
                    }
                    else if (params_Renamed[idx] is ValueParam)
                    {
                        buf.Append(" \"" + params_Renamed[idx].StringValue + "\"");
                    }
                }
                buf.Append(")");
                return buf.ToString();
            }
            else
            {
                return "(batch <filename>)\n" + "Command description:\n" + "\tLoads and executes the file <filename>.";
            }
        }

        #endregion

        /// <summary>
        /// method does the actual work of creating a CLIPSParser and parsing
        /// the file.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="ins">The ins.</param>
        /// <param name="rv">The rv.</param>
        public virtual void parse(Rete engine, Stream ins, DefaultReturnVector rv)
        {
            try
            {
                CLIPSParser parser = new CLIPSParser(engine, ins);
                Object expr = null;
                while ((expr = parser.basicExpr()) != null)
                {
                    if (expr is Defrule)
                    {
                        Defrule rl = (Defrule) expr;
                        engine.RuleCompiler.addRule(rl);
                    }
                    else if (expr is Deftemplate)
                    {
                        Deftemplate dft = (Deftemplate) expr;
                        engine.CurrentFocus.addTemplate(dft, engine, engine.WorkingMemory);
                    }
                    else if (expr is IFunction)
                    {
                        IFunction fnc = (IFunction) expr;
                        fnc.executeFunction(engine, null);
                    }
                }
                if (rv != null)
                {
                    rv.addReturnValue(new DefaultReturnValue(Constants.BOOLEAN_OBJECT, true));
                }
            }
            catch (ParseException e)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                engine.writeMessage(e.Message + Constants.LINEBREAK, Constants.DEFAULT_OUTPUT);
            }
        }
    }
}