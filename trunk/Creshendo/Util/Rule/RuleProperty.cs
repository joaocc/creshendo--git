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

namespace Creshendo.Util.Rule
{
    /// <author>  Peter Lin
    /// *
    /// Version is a generic object for version information. For now, the purpose
    /// is for rule version information. Since RuleML supports the notion of rule
    /// version and CLIPS doesn't, this is an extension.
    /// 
    /// </author>
    public class RuleProperty
    {
        public const String AUTO_FOCUS = "auto-focus";

        /// <summary> A rule can have a direction declaration. Although backward
        /// chaining isn't implemented yet, it's here for the future
        /// </summary>
        public const String DIRECTION = "chaining-direction";

        public const String EFFECTIVE_DATE = "effective-date";
        public const String EXPIRATION_DATE = "expiration-date";

        /// <summary> if a rule has no-agenda set to true, it will skip the agenda
        /// and fire immediately.
        /// </summary>
        public const String NO_AGENDA = "no-agenda";

        /// <summary> The alpha memories can be explicitly turned off by the user
        /// </summary>
        public const String REMEMBER_MATCH = "remember-match";

        /// <summary> Salience defines the priority of a rule. It's a concept
        /// from CLIPS, ART and OPS5
        /// </summary>
        public const String SALIENCE = "salience";

        public const String TEMPORAL_ACTIVATION = "temporal-activation";

        /// <summary> This a rule property specific to Sumatra and is an extension
        /// </summary>
        public const String VERSION = "rule-version";

        private bool boolVal = true;
        private int intVal = 0;

        private String name = null;
        private String value_Renamed = null;

        /// <summary> 
        /// </summary>
        public RuleProperty() 
        {
        }

        public RuleProperty(String name, String ver)
        {
            this.name = name;
            value_Renamed = ver;
        }

        public RuleProperty(String name, int val)
        {
            this.name = name;
            intVal = val;
        }

        public RuleProperty(String name, bool val)
        {
            this.name = name;
            boolVal = val;
        }

        public virtual String Name
        {
            get { return name; }

            set { name = value; }
        }

        public virtual String Value
        {
            get { return value_Renamed; }

            set { value_Renamed = value; }
        }

        public virtual int IntValue
        {
            get { return intVal; }

            set { intVal = value; }
        }

        public virtual bool BooleanValue
        {
            get { return boolVal; }

            set { boolVal = value; }
        }
    }
}