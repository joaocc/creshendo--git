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
    /// Parameter can be a value, a bound variable or the result of a function.
    /// It is up to the implementing class to provide the necessary logic.
    /// 
    /// </author>
    public interface IParameter : IReturnValue
    {
        /// <summary> If the parameter is an object binding, the method should return true
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        bool ObjectBinding { get; }

        /// <summary> In some cases, we may need to reset the parameter. For example,
        /// function and bound parameters may need to be reset, so the
        /// instance can be reused.
        /// </summary>
        void reset();

        /// <summary> Functions should use this method to Get the value from the parameter.
        /// Each parameter type will have logic to return the correct value
        /// or throw an exception if the class can't implicitly cast the value
        /// to the target value type.
        /// </summary>
        /// <param name="">engine
        /// </param>
        /// <param name="">valueType
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        Object getValue(Rete engine, int valueType);
    }
}