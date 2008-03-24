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

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// ShellBoundParam is meant for calling EchoFunction in the shell. It is
    /// different than BoundParam in a couple of ways. The first is that users
    /// can bind an object, fact or value. Bindings in the shell are global
    /// bindings.
    /// 
    /// </author>
    public class ShellBoundParam : AbstractParam
    {
        protected internal String globalVarName = "";
        protected internal Object value_Renamed = null;

        /// <summary> the int value defining the valueType
        /// </summary>
        protected internal int valueType = - 1;

        /// <summary> 
        /// </summary>
        public ShellBoundParam() 
        {
        }

        public virtual String DefglobalName
        {
            get { return globalVarName; }

            set { globalVarName = value; }
        }

        public override int ValueType
        {
            set { }
            get { return valueType; }
        }

        /// <summary> The method returns the bound object
        /// </summary>
        public override Object Value
        {
            set { }
            get { return value_Renamed; }
        }

        /// <summary> if the value was null, the method returns a message "defglobal
        /// not found".
        /// </summary>
        public override String StringValue
        {
            get
            {
                if (Value != null)
                {
                    if (Value is String)
                    {
                        return (String) Value;
                    }
                    else
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                        return Value.ToString();
                    }
                }
                else
                {
                    return "defglobal not found";
                }
            }
        }

        public override bool ObjectBinding
        {
            get { return false; }
        }


        /// <summary> The method needs to be called before getting the value. First
        /// we need to lookup the binding.
        /// </summary>
        /// <param name="">engine
        /// 
        /// </param>
        public virtual void resolveBinding(Rete engine)
        {
            value_Renamed = engine.getDefglobalValue(globalVarName);
        }


        /// <summary> the class will resolve the variable with the engine
        /// </summary>
        public override Object getValue(Rete engine, int valueType)
        {
            return value_Renamed = engine.getDefglobalValue(globalVarName);
        }


        public override void reset()
        {
            valueType = - 1;
            globalVarName = "";
        }
    }
}