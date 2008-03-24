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
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// *
    /// Function is the base interface for all functions. We probably should move
    /// this to another package later on. For now, I Put it here.
    /// The design of Function is very similar to Methods, since a function is
    /// basically a stand alone method.
    /// 
    /// </author>
    public interface IFunction
    {
        /// <summary> every function needs to declare what the return type is.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        int ReturnType { get; }

        /// <summary> Every function must declare the name
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        String Name { get; }

        /// <summary> Functions should declare what kind of parameters it takes.
        /// If a function doesn't take any parameters, the method should return
        /// null.
        /// For example, set-member function returns 
        /// BoundParam.class,StringParam.class,ValueParam.class. This means
        /// the first parameter is a binding, the second is a slotname and the
        /// third is the value.
        /// Another example is Add function. It returns ValueParam[].class, since
        /// it can take one or more numbers and Add them.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        Type[] Parameter { get; }

        /// <summary> Functions must implement concrete logic for the function.
        /// To execute a function, 2 parameters are needed. The first
        /// is the rule engine, which is needed to resolve global variables,
        /// and other engine activities like asserts. The second is an
        /// array of parameters, which can be bindings, literal values or
        /// other functions.
        /// It is the responsibility of the programmer to iterate over the
        /// parameters to find the data they need to execute the function.
        /// </summary>
        /// <param name="">engine
        /// </param>
        /// <param name="">params
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        IReturnVector executeFunction(Rete engine, IParameter[] params_Renamed);

        /// <summary> A convienance method to Get a pretty printer formatted string
        /// for the function.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        String toPPString(IParameter[] params_Renamed, int indents);
    }
}