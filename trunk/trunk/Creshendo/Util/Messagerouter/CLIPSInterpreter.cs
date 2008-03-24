using System;
using Creshendo.Functions;
using Creshendo.Util.Rete;

/// <summary> Copyright 2006 Alexander Wilden, Christoph Emonds, Sebastian Reinartz
/// *
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// *
/// http://ruleml-dev.sourceforge.net/
/// *
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// 
/// </summary>
namespace Creshendo.Util.Messagerouter
{
    public class CLIPSInterpreter
    {
        private readonly Rete.Rete engine;

        public CLIPSInterpreter(Rete.Rete engine)
        {
            this.engine = engine;
        }

        public virtual IReturnVector executeCommand(Object command)
        {
            IFunction func = command as IFunction;
            if (func != null)
            {
                return func.executeFunction(engine, null);
            }
            throw new SystemException("Illegal command.");
        }
    }
}