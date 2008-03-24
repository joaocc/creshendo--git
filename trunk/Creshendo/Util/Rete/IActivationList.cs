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
namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// ActivationList defines the basic operations for an activation list. This
    /// makes it easier to experiment with different ways of implementing an
    /// activation list. The potential methods are queue, priorityQueue, stack
    /// and linkedlist.
    /// Since I haven't decided on the approach, using an interface will allow me
    /// to replace the implementation later on. Rather than guess, my plan is to
    /// implement different versions and benchmark them. This way, I can use the
    /// one that works the better.
    /// 
    /// </author>
    public interface IActivationList
    {
        /// <summary> In some cases, if most of the activations will be removed, it makes
        /// sense to do lazy comparison. This means that any strategy could
        /// potentially work lazily
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> If an activation list is lazy, it will delay the compare until
        /// nextActivation is called.
        /// </summary>
        /// <param name="">lazy
        /// 
        /// </param>
        bool Lazy { get; set; }

        /// <summary> return the current strategy
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> set the strategy for the activation list
        /// </summary>
        /// <param name="">strat
        /// 
        /// </param>
        IStrategy Strategy { get; set; }

        /// <summary> Depending on whether lazy is set or not, the activation list may
        /// assume the activations are ordered by priority and should just
        /// return the first or last activation. in the case where the agenda
        /// is lazy, it will need to compare the evaluations.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        IActivation nextActivation();

        /// <summary> Add a new activation to the list
        /// </summary>
        /// <param name="">act
        /// 
        /// </param>
        void addActivation(IActivation act);

        /// <summary> Remove a given activation from the list
        /// </summary>
        /// <param name="">act
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        IActivation removeActivation(IActivation act);

        /// <summary> In order for strategies to prioritize the activations, we have
        /// to expose the underlying list.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        void clear();

        /// <summary> number of activation in the list
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        int size();

        /// <summary> sometimes we need to clone the list, so that users can see what is
        /// in the activation list or print it out.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        IActivationList cloneActivationList();
    }
}