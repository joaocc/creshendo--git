using System;
using System.Collections;
using System.Collections.Generic;
using Creshendo.Util;
using Creshendo.Util.Rete;
//using List<Object>=Creshendo.Util.List<Object>;

namespace Creshendo.Functions
{
    public class JavaFunctions : IFunctionGroup
    {
        private List<Object> funcs;

        public JavaFunctions() 
        {
            InitBlock();
        }

        #region FunctionGroup Members

        public virtual String Name
        {
            get { return "Java functions"; }
        }


        public virtual void loadFunctions(Rete engine)
        {
            ClassnameResolver classnameResolver = new ClassnameResolver(engine);
            LoadPackageFunction loadpkg = new LoadPackageFunction(classnameResolver);
            engine.declareFunction(loadpkg);
            funcs.Add(loadpkg);
            NewFunction nf = new NewFunction(classnameResolver);
            engine.declareFunction(nf);
            funcs.Add(nf);
            MemberFunction mf = new MemberFunction(classnameResolver);
            engine.declareFunction(mf);
            funcs.Add(mf);
            InstanceofFunction iof = new InstanceofFunction(classnameResolver);
            engine.declareFunction(iof);
            funcs.Add(iof);
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