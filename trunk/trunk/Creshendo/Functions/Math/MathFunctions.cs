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
using System.Collections;
using System.Collections.Generic;
using Creshendo.Util;
using Creshendo.Util.Rete;
//using List<Object>=Creshendo.Util.List<Object>;

namespace Creshendo.Functions.Math
{
    public class MathFunctions : IFunctionGroup
    {
        //UPGRADE_NOTE: The initialization of  'funcs' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private List<Object> funcs;

        public MathFunctions() 
        {
            InitBlock();
        }

        #region FunctionGroup Members

        public virtual String Name
        {
            get { return "Math functions"; }
        }


        public virtual void loadFunctions(Rete engine)
        {
            Abs abs = new Abs();
            engine.declareFunction(abs);
            funcs.Add(abs);
            Acos acos = new Acos();
            engine.declareFunction(acos);
            funcs.Add(acos);
            Add add = new Add();
            engine.declareFunction(add);
            funcs.Add(add);
            Asin asin = new Asin();
            engine.declareFunction(asin);
            funcs.Add(asin);
            Atan atan = new Atan();
            engine.declareFunction(atan);
            funcs.Add(atan);
            Ceil ceil = new Ceil();
            engine.declareFunction(ceil);
            funcs.Add(ceil);
            Const cnst = new Const();
            engine.declareFunction(cnst);
            funcs.Add(cnst);
            Cos cos = new Cos();
            engine.declareFunction(cos);
            funcs.Add(cos);
            Degrees degrees = new Degrees();
            engine.declareFunction(degrees);
            funcs.Add(degrees);
            Divide div = new Divide();
            engine.declareFunction(div);
            funcs.Add(div);
            EqFunction eqf = new EqFunction();
            engine.declareFunction(eqf);
            funcs.Add(eqf);
            Evenp evenp = new Evenp();
            engine.declareFunction(evenp);
            funcs.Add(evenp);
            Exp exp = new Exp();
            engine.declareFunction(exp);
            funcs.Add(exp);
            Floor floor = new Floor();
            engine.declareFunction(floor);
            funcs.Add(floor);
            Greater gr = new Greater();
            engine.declareFunction(gr);
            funcs.Add(gr);
            GreaterOrEqual gre = new GreaterOrEqual();
            engine.declareFunction(gre);
            funcs.Add(gre);
            Less le = new Less();
            engine.declareFunction(le);
            funcs.Add(le);
            LessOrEqual leoe = new LessOrEqual();
            engine.declareFunction(leoe);
            funcs.Add(leoe);
            Log log = new Log();
            engine.declareFunction(log);
            funcs.Add(log);
            Max max = new Max();
            engine.declareFunction(max);
            funcs.Add(max);
            Min min = new Min();
            engine.declareFunction(min);
            funcs.Add(min);
            Multiply mul = new Multiply();
            engine.declareFunction(mul);
            funcs.Add(mul);
            NeqFunction neq = new NeqFunction();
            engine.declareFunction(neq);
            funcs.Add(neq);
            Oddp oddp = new Oddp();
            engine.declareFunction(oddp);
            funcs.Add(oddp);
            Pow pow = new Pow();
            engine.declareFunction(pow);
            funcs.Add(pow);
            Radians radians = new Radians();
            engine.declareFunction(radians);
            funcs.Add(radians);
            Random random = new Random();
            engine.declareFunction(random);
            funcs.Add(random);
            Rint rint = new Rint();
            engine.declareFunction(rint);
            funcs.Add(rint);
            Round round = new Round();
            engine.declareFunction(round);
            funcs.Add(round);
            Sin sin = new Sin();
            engine.declareFunction(sin);
            funcs.Add(sin);
            Sqrt sqrt = new Sqrt();
            engine.declareFunction(sqrt);
            funcs.Add(sqrt);
            Subtract sub = new Subtract();
            engine.declareFunction(sub);
            funcs.Add(sub);
            Tan tan = new Tan();
            engine.declareFunction(tan);
            funcs.Add(tan);
            // now we Add the functions under alias
            engine.declareFunction("+", add);
            engine.declareFunction("-", sub);
            engine.declareFunction("*", mul);
            engine.declareFunction("/", div);
            engine.declareFunction("**", pow);
            engine.declareFunction(">", gr);
            engine.declareFunction(">=", gre);
            engine.declareFunction("<", le);
            engine.declareFunction("<=", leoe);
        }

        public virtual IList listFunctions()
        {
            return funcs;
        }

        #endregion

        private void InitBlock()
        {
            funcs = new List<Object>();
        }
    }
}