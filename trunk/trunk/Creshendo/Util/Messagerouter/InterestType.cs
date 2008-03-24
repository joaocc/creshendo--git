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
    public enum InterestType
    {
        /// <summary> used for channels which are interested in every message output,
        /// regardless which channel caused this output.
        /// </summary>
        ALL = 300,

        /// <summary> used for channels which are only interested in the message output, which
        /// is in response for their to their own input.
        /// </summary>
        MINE = 200,

        /// <summary> used for channels which aren't interested in any message output.
        /// </summary>
        NONE = 100
    }
}