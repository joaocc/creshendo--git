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
    /// <summary> TemporalFact is an extension which supports the concepts mentioned by
    /// karl. The extension adds the following concepts.
    /// 1. expire - expiration time
    /// 2. source - URL
    /// 3. service type - method used to obtain the fact
    /// 4. validity - probability of the facts validity
    /// These ideas are useful for semantic web, agents, temporal systems and
    /// services.
    /// </summary>
    /// <author>  Peter Lin
    /// *
    /// 
    /// </author>
    public struct TemporalFact_Fields
    {
        public static readonly String EXPIRATION = "expiration-time";
        public static readonly String SERVICE_TYPE = "service-type";
        public static readonly String SOURCE = "source";
        public static readonly String VALIDITY = "validity";
    }

    public interface ITemporalFact : IFact
    {
        //UPGRADE_NOTE: Members of interface 'TemporalFact' were extracted into structure 'TemporalFact_Fields'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1045"'
        long ExpirationTime { get; set; }
        String Source { get; set; }
        String ServiceType { get; set; }
        int Validity { get; set; }
    }
}