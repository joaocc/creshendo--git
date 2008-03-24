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
using System.Text;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// A binding can be an object or a field of a given object. When binding
    /// is an Object binding, the column id will not be set. The binding
    /// would be the row for the left memory.
    /// It is up to classes using the binding to check if it is an object
    /// binding and Get the appropriate fact using getLeftRow().
    /// One thing about the current design is the binding is position based.
    /// The benefit is it avoids having to set the binding and reset it
    /// multiple times. BetaNodes use the binding to Get the correct slot
    /// value and use it to evaluate an atomic condition. A significant
    /// downside of this approach is when deftemplates are re-declared at
    /// runtime. It means that we might need to recompute the bindings, which
    /// could be a very costly process. More thought and research is needed
    /// to figure out the best way to handle re-declaring deftemplates.
    /// 
    /// </author>
    [Serializable]
    public class Binding : ICloneable
    {
        /// <summary> if the binding is to an object, the field should
        /// be true. by default it is false.
        /// </summary>
        protected internal bool isObjVar = false;

        /// <summary> if the join is a predicate join with a function, the
        /// binding should be set to isPredicate
        /// </summary>
        protected internal bool isPredJoin = false;

        /// <summary> The indexes of the left deftemplate
        /// </summary>
        protected internal int leftIndex;

        /// <summary> by default the row index is -1, which means
        /// it's not set. Any index that is negative indicates
        /// it's not set.
        /// </summary>
        protected internal int leftrow = - 1;

        /// <summary> by default bindings test for equality. in some cases
        /// they test for inequality.
        /// </summary>
        protected internal bool negated_Renamed_Field = false;

        /// <summary> the indexes for the right deftemplate
        /// </summary>
        protected internal int rightIndex;

        /// <summary> We need this to keep track of which CE is the first to declare
        /// a binding. This is important to rule compilation.
        /// </summary>
        protected internal int rowDeclared_Renamed_Field = - 1;

        /// <summary> This is the name of the variable. Every binding must
        /// have a variable name. It can be user defined or auto-
        /// generated by the rule compiler.
        /// </summary>
        protected internal String varName = null;

        public Binding() 
        {
        }

        /// <summary> Return the name of the variable
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> Set the variable name. This is important, since the join
        /// nodes will use it at runtime.
        /// </summary>
        /// <param name="">name
        /// 
        /// </param>
        public virtual String VarName
        {
            get { return varName; }

            set { varName = value; }
        }

        /// <summary> If the binding is for an object, the method returns true.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> Set whether the binding is an object binding.
        /// </summary>
        /// <param name="">obj
        /// 
        /// </param>
        public virtual bool IsObjectVar
        {
            get { return isObjVar; }

            set { isObjVar = value; }
        }

        /// <summary> Set the row that declares the binding
        /// </summary>
        /// <param name="">row
        /// 
        /// </param>
        public virtual int RowDeclared
        {
            set { rowDeclared_Renamed_Field = value; }
        }

        /// <summary> Return the left Deftemplate 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> Set the left deftemplate
        /// </summary>
        /// <param name="">temp
        /// 
        /// </param>
        public virtual int LeftRow
        {
            get { return leftrow; }

            set { leftrow = value; }
        }

        /// <summary> Get the left index
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> set the left index
        /// </summary>
        /// <param name="">indx
        /// 
        /// </param>
        public virtual int LeftIndex
        {
            get { return leftIndex; }

            set { leftIndex = value; }
        }

        /// <summary> Get the right index
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> set the right index
        /// </summary>
        /// <param name="">indx
        /// 
        /// </param>
        public virtual int RightIndex
        {
            get { return rightIndex; }

            set { rightIndex = value; }
        }

        /// <summary> if a binding is negated, call the method with true
        /// </summary>
        /// <param name="">neg
        /// 
        /// </param>
        public virtual bool Negated
        {
            set { negated_Renamed_Field = value; }
        }

        public virtual bool PredJoin
        {
            set { isPredJoin = value; }
        }

        #region ICloneable Members

        /// <summary> convienance method for clonging a binding at rule compilation
        /// time.
        /// </summary>
        public Object Clone()
        {
            Binding bind = new Binding();
            bind.VarName = VarName;
            bind.IsObjectVar = IsObjectVar;
            bind.LeftRow = LeftRow;
            bind.LeftIndex = LeftIndex;
            bind.RightIndex = RightIndex;
            return bind;
        }

        #endregion

        /// <summary> The row that declares the binding the first time. The
        /// row corresponds directly to the Conditional Element in
        /// the rule. If the second CE declares the binding for the
        /// first time, the row would be 1. 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual int rowDeclared()
        {
            return rowDeclared_Renamed_Field;
        }


        /// <summary> by default bindings are not negated. if it is,
        /// method return true.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool negated()
        {
            return negated_Renamed_Field;
        }


        /// <summary> Since the binding refers to the row and column, the binding
        /// doesn't know the deftemplate.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual String toBindString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("(" + leftrow + ")(");
            buf.Append(leftIndex);
            if (negated_Renamed_Field)
            {
                buf.Append(") " + Constants.NOTEQUAL_STRING + " (0)(");
            }
            else
            {
                buf.Append(") " + Constants.EQUAL_STRING + " (0)(");
            }
            buf.Append(rightIndex);
            buf.Append(")");
            return buf.ToString();
        }

        public virtual String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("?" + varName + " (" + leftrow + ")(");
            buf.Append(leftIndex);
            if (negated_Renamed_Field)
            {
                buf.Append(") " + Constants.NOTEQUAL_STRING + " (0)(");
            }
            else
            {
                buf.Append(") " + Constants.EQUAL_STRING + " (0)(");
            }
            buf.Append(rightIndex);
            buf.Append(")");
            return buf.ToString();
        }

        //UPGRADE_TODO: The equivalent of method 'java.lang.Object.clone' is not an override method. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1143"'

        /// <summary> evaluate will extra the values from each side and evaluate it
        /// </summary>
        /// <param name="">left
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool evaluate(IFact[] left, IFact right)
        {
            if (left[leftrow] == right)
            {
                return false;
            }
            if (negated_Renamed_Field)
            {
                return Evaluate.evaluateNotEqual(left[leftrow].getSlotValue(leftIndex), right.getSlotValue(rightIndex));
            }
            else
            {
                return Evaluate.evaluateEqual(left[leftrow].getSlotValue(leftIndex), right.getSlotValue(rightIndex));
            }
        }
    }
}