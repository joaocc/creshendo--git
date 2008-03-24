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
    /// ReturnValue defines the base methods to Get the value and type of
    /// the return value. Since users will be able to use CLIPS syntax
    /// to define functions, we provide this functionality.
    /// 
    /// </author>
    public interface IReturnValue
    {
        int ValueType { get; }
        Object Value { get; }
        String StringValue { get; }
        bool BooleanValue { get; }
        int IntValue { get; }
        short ShortValue { get; }
        long LongValue { get; }
        float FloatValue { get; }
        double DoubleValue { get; }
        Decimal BigIntegerValue { get; }
        Decimal BigDecimalValue { get; }
    }
}