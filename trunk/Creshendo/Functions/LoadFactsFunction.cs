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
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Creshendo.Util;
using Creshendo.Util.Parser.Clips2;
using Creshendo.Util.Rete;
using Creshendo.Util.Rete.Exception;
//using ParseException=Creshendo.Util.Parser.Clips.ParseException;

//using IList=Creshendo.Util.IList;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// LoadFunction will create a new instance of CLIPSParser and load the
    /// facts in the data file.
    /// 
    /// </author>
    [Serializable]
    public class LoadFactsFunction : IFunction
    {
        public const String LOAD = "load-facts";

        /// <summary> 
        /// </summary>
        public LoadFactsFunction() 
        {
        }

        #region Function Members

        public virtual int ReturnType
        {
            get { return Constants.BOOLEAN_OBJECT; }
        }

        public virtual String Name
        {
            get { return LOAD; }
        }

        public virtual Type[] Parameter
        {
            get { return new Type[] {typeof (ValueParam)}; }
        }


        public virtual IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed)
        {
            DefaultReturnVector rv = new DefaultReturnVector();
            bool loaded = true;
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                for (int idx = 0; idx < params_Renamed.Length; idx++)
                {
                    String input = null;
                    if (params_Renamed[idx] is ValueParam)
                    {
                        input = ((ValueParam) params_Renamed[idx]).StringValue;
                    }
                    else if (params_Renamed[idx] is BoundParam)
                    {
                    }
                    if (input.IndexOf((Char) '\\') > - 1)
                    {
                        input.Replace("\\", "/");
                    }
                    // check to see if the path is an absolute windows path
                    // or absolute unix path
                    if (input.IndexOf(":") < 0 && !input.StartsWith("/") && !input.StartsWith("./"))
                    {
                        input = "./" + input;
                    }
                    try
                    {
                        Stream inStream = null;
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
                        CLIPSParser parser = new CLIPSParser(inStream);
                        IList data = parser.loadExpr();
                        IEnumerator itr = data.GetEnumerator();
                        while (itr.MoveNext())
                        {
                            Object val = itr.Current;
                            ValueParam[] vp = (ValueParam[]) val;
                            Deftemplate tmpl = (Deftemplate) engine.CurrentFocus.getTemplate(vp[0].StringValue);
                            Deffact fact = (Deffact) tmpl.createFact((Object[]) vp[1].Value, - 1);

                            engine.assertFact(fact);
                        }
                    }
                    catch (FileNotFoundException e)
                    {
                        loaded = false;
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                        engine.writeMessage(e.Message + Constants.LINEBREAK, Constants.DEFAULT_OUTPUT);
                    }
                    catch (ParseException e)
                    {
                        loaded = false;
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                        engine.writeMessage(e.Message + Constants.LINEBREAK, Constants.DEFAULT_OUTPUT);
                    }
                    catch (AssertException e)
                    {
                        loaded = false;
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                        engine.writeMessage(e.Message + Constants.LINEBREAK, Constants.DEFAULT_OUTPUT);
                    }
                    catch (IOException e)
                    {
                        loaded = false;
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                        engine.writeMessage(e.Message + Constants.LINEBREAK, Constants.DEFAULT_OUTPUT);
                    }
                }
            }
            else
            {
                loaded = false;
            }
            DefaultReturnValue drv = new DefaultReturnValue(Constants.BOOLEAN_OBJECT, loaded);
            rv.addReturnValue(drv);
            return rv;
        }


        public virtual String toPPString(IParameter[] params_Renamed, int indents)
        {
            if (params_Renamed != null && params_Renamed.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("(load-facts");
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
                return "(load <filename>)\n" + "Command description:\n" + "\tLoad the file <filename>.";
            }
        }

        #endregion
    }
}