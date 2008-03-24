/*
* Copyright 2002-2007 Peter Lin
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
using Creshendo.Util.Collections;

namespace Creshendo.Util.Rete.Strategies
{
    /// <summary> Strategy is where new strategies are registered and where the functions
    /// find the strategy class to set the strategy.
    /// </summary>
    /// <author>  woolfel
    /// *
    /// 
    /// </author>
    public class Strategies
    {
        public static IStrategy BREADTH;
        public static IStrategy DEPTH;
        public static IStrategy RECENCY;
        private static readonly GenericHashMap<string, IStrategy> registry;

        static Strategies()
        {
            DEPTH = new DepthStrategy();
            BREADTH = new BreadthStrategy();
            RECENCY = new RecencyStrategy();
            registry = new GenericHashMap<String, IStrategy>();
            {
                registry.Put(DEPTH.Name, DEPTH);
                registry.Put(BREADTH.Name, BREADTH);
                registry.Put(RECENCY.Name, RECENCY);
            }
        }


        public static void register(IStrategy strat)
        {
            if (strat != null)
            {
                registry.Put(strat.Name, strat);
            }
        }

        public static IStrategy getStrategy(String key)
        {
            return registry.Get(key);
        }
    }
}