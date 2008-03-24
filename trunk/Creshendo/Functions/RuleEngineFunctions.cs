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

namespace Creshendo.Functions
{
    /// <author>  Peter Lin
    /// 
    /// RuleEngineFunction is responsible for loading all the rule functions
    /// related to engine operation.
    /// 
    /// </author>
    [Serializable]
    public class RuleEngineFunctions : IFunctionGroup
    {
        //UPGRADE_NOTE: The initialization of  'funcs' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private List<Object> funcs;

        public RuleEngineFunctions() 
        {
            InitBlock();
        }

        #region FunctionGroup Members

        public virtual String Name
        {
            get { return ("Rule engine functions"); }
        }


        public virtual void loadFunctions(Rete engine)
        {
            AssertFunction assrt = new AssertFunction();
            engine.declareFunction(assrt);
            funcs.Add(assrt);
            AnyEqFunction anyeq = new AnyEqFunction();
            engine.declareFunction(anyeq);
            funcs.Add(anyeq);
            BindFunction bindf = new BindFunction();
            engine.declareFunction(bindf);
            funcs.Add(bindf);
            ClearFunction clr = new ClearFunction();
            engine.declareFunction(clr);
            funcs.Add(clr);
            DefclassFunction defcls = new DefclassFunction();
            engine.declareFunction(defcls);
            funcs.Add(defcls);
            DefmoduleFunction dmod = new DefmoduleFunction();
            engine.declareFunction(dmod);
            funcs.Add(dmod);
            DefruleFunction drule = new DefruleFunction();
            engine.declareFunction(drule);
            funcs.Add(drule);
            DefinstanceFunction defins = new DefinstanceFunction();
            engine.declareFunction(defins);
            funcs.Add(defins);
            DeftemplateFunction dtemp = new DeftemplateFunction();
            engine.declareFunction(dtemp);
            funcs.Add(dtemp);
            EchoFunction efunc = new EchoFunction();
            engine.declareFunction(efunc);
            funcs.Add(efunc);
            EqFunction eq = new EqFunction();
            engine.declareFunction(eq);
            funcs.Add(eq);
            EvalFunction eval = new EvalFunction();
            engine.declareFunction(eval);
            funcs.Add(eval);
            ExitFunction ext = new ExitFunction();
            engine.declareFunction(ext);
            funcs.Add(ext);
            FactsFunction ffun = new FactsFunction();
            engine.declareFunction(ffun);
            funcs.Add(ffun);
            FireFunction fire = new FireFunction();
            engine.declareFunction(fire);
            funcs.Add(fire);
            FocusFunction focus = new FocusFunction();
            engine.declareFunction(focus);
            funcs.Add(focus);
            ModulesFunction modules = new ModulesFunction();
            engine.declareFunction(modules);
            funcs.Add(modules);
            GenerateFactsFunction genff = new GenerateFactsFunction();
            engine.declareFunction(genff);
            funcs.Add(genff);
            GarbageCollectFunction gcf = new GarbageCollectFunction();
            engine.declareFunction(gcf);
            funcs.Add(gcf);
            LazyAgendaFunction laf = new LazyAgendaFunction();
            engine.declareFunction(laf);
            funcs.Add(laf);
            ListDirectoryFunction ldir = new ListDirectoryFunction();
            engine.declareFunction(ldir);
            funcs.Add(ldir);
            ListFunctionsFunction lffnc = new ListFunctionsFunction();
            engine.declareFunction(lffnc);
            engine.declareFunction("functions", lffnc);
            funcs.Add(lffnc);
            ListTemplatesFunction listTemp = new ListTemplatesFunction();
            engine.declareFunction(listTemp);
            funcs.Add(listTemp);
            LoadFunctionsFunction loadfunc = new LoadFunctionsFunction();
            engine.declareFunction(loadfunc);
            funcs.Add(loadfunc);
            LoadFunctionGroupFunction loadfg = new LoadFunctionGroupFunction();
            engine.declareFunction(loadfg);
            funcs.Add(loadfg);
            UsageFunction usage = new UsageFunction();
            engine.declareFunction(usage);
            funcs.Add(usage);
            MatchesFunction mf = new MatchesFunction();
            engine.declareFunction(mf);
            funcs.Add(mf);
            MemberTestFunction mtestf = new MemberTestFunction();
            engine.declareFunction(mtestf);
            funcs.Add(mtestf);
            MemoryFreeFunction mff = new MemoryFreeFunction();
            engine.declareFunction(mff);
            funcs.Add(mff);
            MemoryTotalFunction mtf = new MemoryTotalFunction();
            engine.declareFunction(mtf);
            funcs.Add(mtf);
            MemoryUsedFunction musd = new MemoryUsedFunction();
            engine.declareFunction(musd);
            funcs.Add(musd);
            MillisecondTime mstime = new MillisecondTime();
            engine.declareFunction(mstime);
            funcs.Add(mstime);
            ModifyFunction mod = new ModifyFunction();
            engine.declareFunction(mod);
            funcs.Add(mod);
            PPrintRuleFunction pprule = new PPrintRuleFunction();
            engine.declareFunction(pprule);
            funcs.Add(pprule);
            PPrintTemplateFunction pptemp = new PPrintTemplateFunction();
            engine.declareFunction(pptemp);
            funcs.Add(pptemp);
            PrintProfileFunction pproff = new PrintProfileFunction();
            engine.declareFunction(pproff);
            funcs.Add(pproff);
            ProfileFunction proff = new ProfileFunction();
            engine.declareFunction(proff);
            funcs.Add(proff);
            ResetFunction resetf = new ResetFunction();
            engine.declareFunction(resetf);
            funcs.Add(resetf);
            ResetFactsFunction resetff = new ResetFactsFunction();
            engine.declareFunction(resetff);
            funcs.Add(resetff);
            ResetObjectsFunction resetof = new ResetObjectsFunction();
            engine.declareFunction(resetof);
            funcs.Add(resetof);
            RetractFunction rtract = new RetractFunction();
            engine.declareFunction(rtract);
            funcs.Add(rtract);
            RightMatchesFunction rmfunc = new RightMatchesFunction();
            engine.declareFunction(rmfunc);
            funcs.Add(rmfunc);
            RulesFunction rf = new RulesFunction();
            engine.declareFunction(rf);
            engine.declareFunction(RulesFunction.LISTRULES, rf);
            funcs.Add(rf);
            SaveFactsFunction savefacts = new SaveFactsFunction();
            engine.declareFunction(savefacts);
            funcs.Add(savefacts);
            SetFocusFunction setfoc = new SetFocusFunction();
            engine.declareFunction(setfoc);
            funcs.Add(setfoc);
            SpoolFunction spool = new SpoolFunction();
            engine.declareFunction(spool);
            funcs.Add(spool);
            TemplatesFunction tempf = new TemplatesFunction();
            engine.declareFunction(tempf);
            engine.declareFunction(TemplatesFunction.LISTTEMPLATES, tempf);
            funcs.Add(tempf);
            TestRuleFunction trfunc = new TestRuleFunction();
            engine.declareFunction(trfunc);
            funcs.Add(trfunc);
            UnDefruleFunction udrule = new UnDefruleFunction();
            engine.declareFunction(udrule);
            funcs.Add(udrule);
            UnDeftemplateFunction udt = new UnDeftemplateFunction();
            engine.declareFunction(udt);
            funcs.Add(udt);
            UnWatchFunction uwatchf = new UnWatchFunction();
            engine.declareFunction(uwatchf);
            funcs.Add(uwatchf);
            UnProfileFunction uproff = new UnProfileFunction();
            engine.declareFunction(uproff);
            funcs.Add(uproff);
            ValidateRuleFunction vrf = new ValidateRuleFunction();
            engine.declareFunction(vrf);
            funcs.Add(vrf);
            VersionFunction ver = new VersionFunction();
            engine.declareFunction(ver);
            funcs.Add(ver);
            //ViewFunction view = new ViewFunction();
            //engine.declareFunction(view);
            //funcs.Add(view);
            WatchFunction watchf = new WatchFunction();
            engine.declareFunction(watchf);
            funcs.Add(watchf);
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