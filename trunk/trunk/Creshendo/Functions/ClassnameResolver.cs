using System;
using System.Collections;
using System.Collections.Generic;
using Creshendo.Util;
using Creshendo.Util.Rete;

namespace Creshendo.Functions
{
    //using Matcher = java.util.regex.Matcher;
    //using Pattern = java.util.regex.Pattern;

    public class ClassnameResolver
    {
        //UPGRADE_NOTE: The initialization of  'classes' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private List<Object> classes;

        //UPGRADE_NOTE: Final was removed from the declaration of 'classnamePattern '. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1003"'
        //UPGRADE_NOTE: The initialization of  'classnamePattern' was moved to static method 'org.jamocha.rete.functions.ClassnameResolver'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        //private static readonly Pattern classnamePattern;

        //UPGRADE_NOTE: Final was removed from the declaration of 'packagePattern '. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1003"'
        //UPGRADE_NOTE: The initialization of  'packagePattern' was moved to static method 'org.jamocha.rete.functions.ClassnameResolver'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        //private static readonly Pattern packagePattern;

        private Rete engine;
        private List<Object> packages;

        static ClassnameResolver()
        {
            //classnamePattern = Pattern.compile("([\\w_][\\w_\\d]*\\.)*([\\w_][\\w_\\d]*)");
            //packagePattern = Pattern.compile("([\\w_][\\w_\\d]*\\.)+\\*");
        }

        public ClassnameResolver(Rete engine)
        {
            InitBlock();
            packages.Add("java.lang.*");
        }

        private void InitBlock()
        {
            packages = new List<Object>();
            classes = new List<Object>();
        }

        public virtual void addImport(String s)
        {
            //if (classnamePattern.matcher(s).matches())
            //{
            //    classes.Add(s);
            //}
            //else if (packagePattern.matcher(s).matches())
            //{
            //    packages.Add(s);
            //}
            //else
            //{
            //    //UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
            //    throw new System.Exception("The import \"" + s + "\" is neither a valid class nor package name.");
            //}
        }

        public virtual Type resolveClass(String name)
        {
            return null;
            //if (!isValidClassname(name))
            //{
            //    //UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
            //    throw new System.Exception("\"" + name + "\" is not a valid class name.");
            //}
            //org.jamocha.rete.util.IList possibleNames = new org.jamocha.rete.util.List<Object>();
            //possibleNames.Add(name);
            //if (!isQualifiedClassname(name))
            //{
            //    for (int idx = 0; idx < classes.Count(); idx++)
            //    {
            //        System.String className = (System.String) classes.Get(idx);
            //        Matcher matcher = classnamePattern.matcher(className);
            //        if (matcher.group(2).Equals(name))
            //        {
            //            possibleNames.Add(className);
            //        }
            //    }
            //    for (int idx = 0; idx < packages.Count(); idx++)
            //    {
            //        System.String packageName = (System.String) packages.Get(idx);
            //        possibleNames.Add(packageName.Replace("*", name));
            //    }
            //}
            //for (int idx = 0; idx < possibleNames.Count(); idx++)
            //{
            //    System.String possibleClassName = (System.String) possibleNames.Get(idx);
            //    //UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
            //    try
            //    {
            //        //UPGRADE_TODO: Format of parameters of method 'java.lang.Class.forName' are different in the equivalent in .NET. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1092"'
            //        return System.Type.GetType(possibleClassName);
            //    }
            //    catch (System.Exception e)
            //    {
            //        /* just try the Current name */
            //    }
            //}
            ////UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
            //throw new System.Exception("Class \"" + name + "\" could ot be found.");
        }

        public virtual bool isValidClassname(String s)
        {
            //Matcher matcher = classnamePattern.matcher(s);
            //return matcher.matches();
            return false;
        }

        public virtual bool isQualifiedClassname(String s)
        {
            return isValidClassname(s) && (s.IndexOf(".") > - 1);
        }
    }
}