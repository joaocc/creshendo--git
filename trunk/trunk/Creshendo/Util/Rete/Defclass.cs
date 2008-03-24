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
using System.Reflection;
using Creshendo.Util.Collections;

namespace Creshendo.Util.Rete
{
    /// <summary> Defclass Contains the introspection information for a single object type.
    /// It takes a class and uses java introspection to Get a list of the Get/set
    /// attributes. It also checks to see if the class implements java beans
    /// propertyChangeListener support. If it does, the Method object for those
    /// two are cached.
    /// </summary>
    [Serializable]
    public class Defclass
    {
        private MethodInfo addListener = null;
        private EventInfo eventPropertyChanged = null;
        private Type delegateType;
        private BeanInfo INFO = null;
        private bool ISBEAN = false;
        private IGenericMap<object, object> methods;
        private Type OBJECT_CLASS = null;
        private PropertyInfo[] PROPS = null;
        private MethodInfo removeListener = null;

        /// <summary> 
        /// </summary>
        public Defclass(Type obj)
        {
            InitBlock();
            OBJECT_CLASS = obj;
            init();
        }

        /// <summary> If the class has a method for adding propertyChangeListener,
        /// the method return true.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool JavaBean
        {
            get { return ISBEAN; }
        }

        /// <summary> Return the PropertyDescriptors for the class
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        //UPGRADE_TODO: Class java.beans.PropertyDescriptor was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
        public virtual PropertyInfo[] PropertyDescriptors
        {
            get { return PROPS; }
        }

        /// <summary> Get the BeanInfo for the class
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        //UPGRADE_TODO: Interface java.beans.BeanInfo was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
        public virtual BeanInfo BeanInfo
        {
            get { return INFO; }
        }

        public virtual Type ClassObject
        {
            get { return OBJECT_CLASS; }
        }

        /// <summary> Get the addPropertyChangeListener(PropertyChangeListener) method for
        /// the class.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual MethodInfo AddListenerMethod
        {
            get { return addListener; }
        }

        
        public virtual EventInfo EventPropertyChanged
        {
            get { return eventPropertyChanged; }
        }

        public virtual Type DelegateType
        {
            get { return delegateType; }
        }

        /// <summary> Get the removePropertyChangeListener(PropertyChangeListener) method for
        /// the class.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual MethodInfo RemoveListenerMethod
        {
            get { return removeListener; }
        }

        private void InitBlock()
        {
            methods = CollectionFactory.localMap();
        }

        /// <summary> init is responsible for checking the class to make sure
        /// it implements addPropertyChangeListener(java.beans.PropertyChangeListener)
        /// and removePropertyChangeListener(java.beans.PropertyChangeListener).
        /// We don't require the classes extend PropertyChangeSupport.
        /// </summary>
        public void init()
        {
            try
            {
                INFO = Introspector.getBeanInfo(OBJECT_CLASS);
                // we have to filter out the class PropertyDescriptor
                PropertyInfo[] pd = INFO.getPropertyDescriptors();
                List<Object> list = new List<Object>();
                for (int idx = 0; idx < pd.Length; idx++)
                {
                    if (pd[idx].Name.Equals("class"))
                    {
                        // don't Add
                    }
                    else
                    {
                        // we map the methods using the PropertyDescriptor.getName for
                        // the key and the PropertyDescriptor as the value
                        methods.Put(pd[idx].Name, pd[idx]);
                        list.Add(pd[idx]);
                    }
                }
                PropertyInfo[] newpd = new PropertyInfo[list.Count];
                
                list.CopyTo(newpd,0);
                PROPS = (PropertyInfo[])newpd;
                // logic for filtering the PropertyDescriptors
                if (ObjectFilter.lookupFilter(OBJECT_CLASS) != null)
                {
                    // Remove the props that should be invisible
                    BeanFilter bf = ObjectFilter.lookupFilter(OBJECT_CLASS);
                    PROPS = bf.filter(PROPS);
                }
                if (checkBean())
                {
                    ISBEAN = true;
                }
                // we clean up the array and List<Object>
                list.Clear();
                pd = null;
            }
            catch (System.Exception e)
            {
                // we should log this and throw an exception
            }
        }

        /// <summary> method checks to see if the class implements addPropertyChangeListener
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        protected internal virtual bool checkBean()
        {
            bool add = false;
            bool remove = false;

            eventPropertyChanged = OBJECT_CLASS.GetEvent("PropertyChanged");

            if (eventPropertyChanged == null)
            {
                return false;
            }

            delegateType = eventPropertyChanged.EventHandlerType;
            addListener = eventPropertyChanged.GetAddMethod();
            removeListener = eventPropertyChanged.GetRemoveMethod();

            return delegateType != null && addListener != null && removeListener != null;
            /*
            MethodInfo[] methd = INFO.getMethodDescriptors();
            for (int idx = 0; idx < methd.Length; idx++)
            {
                MethodInfo desc = methd[idx];
                if (desc.Name.Equals(Constants.PCS_ADD) && checkParameter(desc))
                {
                    // check the parameter
                    add = true;
                }
                if (desc.Name.Equals(Constants.PCS_REMOVE) && checkParameter(desc))
                {
                    // check the parameter
                    remove = true;
                }
            }
            if (add && remove)
            {
                getUtilMethods();
                return true;
            }
            else
            {
                return false;
            }
            */
        }

        /// <summary> method will try to look up Add and Remove property change listener.
        /// </summary>
        protected internal virtual void getUtilMethods()
        {
            try
            {
                eventPropertyChanged = OBJECT_CLASS.GetEvent("PropertyChanged");
                delegateType = eventPropertyChanged.EventHandlerType;

                // since a class may inherit the addListener method from
                // a parent, we lookup all methods and not just the
                // declared methods.
                //UPGRADE_TODO: Interface java.beans.PropertyChangeListener was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
                addListener = OBJECT_CLASS.GetMethod(Constants.PCS_ADD, (Type[]) new Type[] {typeof (PropertyChangedHandler)});
                //UPGRADE_TODO: Interface java.beans.PropertyChangeListener was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
                removeListener = OBJECT_CLASS.GetMethod(Constants.PCS_REMOVE, (Type[])new Type[] { typeof(PropertyChangedHandler) });
            }
            catch (MethodAccessException e)
            {
                // we should log this
            }
        }

        //UPGRADE_TODO: Class java.beans.MethodDescriptor was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
        /// <summary>
        /// Method checks the MethodDescriptor to make sure it only
        /// has 1 parameter and that it is a propertyChangeListener
        /// </summary>
        /// <param name="desc">The desc.</param>
        /// <returns></returns>
        public virtual bool checkParameter(MethodInfo desc)
        {
            bool ispcl = false;
            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.reflect.Method.getParameterTypes' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
            //UPGRADE_TODO: Method java.beans.MethodDescriptor.getMethod was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'

            ParameterInfo[] parms = desc.GetParameters();

            if (parms.Length == 1)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.reflect.Method.getParameterTypes' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                //UPGRADE_TODO: Method java.beans.MethodDescriptor.getMethod was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
                //UPGRADE_TODO: Interface java.beans.PropertyChangeListener was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
                if (parms[0].ParameterType == typeof(PropertyChangedHandler))
                {
                    ispcl = true;
                }
            }
            return ispcl;
        }


        /// <summary>
        /// Note: haven't decided if the method should throw an exception
        /// or not. Assuming the class has been declared and the defclass
        /// exists for it, it normally shouldn't encounter an exception.
        /// Cases where it would is if the method is not public. We should
        /// do that at declaration time and not runtime.
        /// </summary>
        /// <param name="col">The col.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public virtual Object getSlotValue(int col, Object data)
        {
            try
            {
                //UPGRADE_TODO: Method java.beans.PropertyDescriptor.getReadMethod was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
                return PROPS[col].GetGetMethod().Invoke(data, (Object[]) null);
            }
            catch (UnauthorizedAccessException e)
            {
                return null;
            }
            catch (ArgumentException e)
            {
                return null;
            }
            catch (TargetInvocationException e)
            {
                return null;
            }
        }

        /// <summary>
        /// create the deftemplate for the defclass
        /// </summary>
        /// <param name="tempName">Name of the temp.</param>
        /// <returns></returns>
        public virtual Deftemplate createDeftemplate(String tempName)
        {
            Slot[] st = new Slot[PROPS.Length];
            for (int idx = 0; idx < st.Length; idx++)
            {
                //UPGRADE_TODO: Method java.beans.PropertyDescriptor.getPropertyType was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
                if (PROPS[idx].PropertyType.IsArray)
                {
                    //UPGRADE_TODO: Method java.beans.FeatureDescriptor.getName was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
                    st[idx] = new MultiSlot(PROPS[idx].Name);
                    st[idx].Id = idx;
                }
                else
                {
                    //UPGRADE_TODO: Method java.beans.FeatureDescriptor.getName was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
                    st[idx] = new Slot(PROPS[idx].Name);
                    //UPGRADE_TODO: Method java.beans.PropertyDescriptor.getPropertyType was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
                    st[idx].ValueType = ConversionUtils.getTypeCode(PROPS[idx].PropertyType);
                    // set the column id for the slot
                    st[idx].Id = idx;
                }
            }
            Deftemplate temp = new Deftemplate(tempName, OBJECT_CLASS.FullName, st);
            return temp;
        }

        /// <summary>
        /// Create the Deftemplate for the class, but with a given parent. If a
        /// template has a parent, only call this method. If the other method is
        /// called, the template is not gauranteed to work correctly.
        /// </summary>
        /// <param name="tempName">Name of the temp.</param>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public virtual Deftemplate createDeftemplate(String tempName, ITemplate parent)
        {
            reOrderDescriptors(parent);
            return createDeftemplate(tempName);
        }

        /// <summary>
        /// the purpose of this method is to re-order the PropertyDescriptors, so
        /// that template inheritance works correctly.
        /// </summary>
        /// <param name="parent">The parent.</param>
        protected internal virtual void reOrderDescriptors(ITemplate parent)
        {
            List<Object> desc = null;
            bool add = false;
            Slot[] pslots = parent.AllSlots;
            //UPGRADE_TODO: Class java.beans.PropertyDescriptor was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
            PropertyInfo[] newprops = new PropertyInfo[PROPS.Length];
            // first thing is to make sure the existing slots from the parent
            // are in the same column
            // now check to see if the new class has more fields
            if (newprops.Length > pslots.Length)
            {
                desc = new List<Object>();
                add = true;
            }
            for (int idx = 0; idx < pslots.Length; idx++)
            {
                newprops[idx] = getDescriptor(pslots[idx].Name);
                if (add)
                {
                    desc.Add(pslots[idx].Name);
                }
            }
            if (add)
            {
                List<Object> newfields = new List<Object>();
                for (int idz = 0; idz < PROPS.Length; idz++)
                {
                    //UPGRADE_TODO: Method java.beans.FeatureDescriptor.getName was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
                    if (!desc.Contains(PROPS[idz].Name))
                    {
                        // we Add it to the new fields
                        newfields.Add(PROPS[idz]);
                    }
                }
                int c = 0;
                // now we start from where parent slots left off
                for (int n = pslots.Length; n < newprops.Length; n++)
                {
                    //UPGRADE_TODO: Class java.beans.PropertyDescriptor was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
                    newprops[n] = (PropertyInfo)newfields[c];
                    c++;
                }
            }
            PROPS = newprops;
        }

        //UPGRADE_TODO: Class java.beans.PropertyDescriptor was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
        /// <summary>
        /// Find the PropertyDescriptor with the same name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        protected internal virtual PropertyInfo getDescriptor(String name)
        {
            //UPGRADE_TODO: Class java.beans.PropertyDescriptor was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
            PropertyInfo pd = null;
            for (int idx = 0; idx < PROPS.Length; idx++)
            {
                //UPGRADE_TODO: Method java.beans.FeatureDescriptor.getName was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
                if (PROPS[idx].Name.Equals(name))
                {
                    pd = PROPS[idx];
                    break;
                }
            }
            return pd;
        }


        /// <summary>
        /// Return the write method using slot name for the key
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual MethodInfo getWriteMethod(String name)
        {
            //UPGRADE_TODO: Method java.beans.PropertyDescriptor.getWriteMethod was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
            //UPGRADE_TODO: Class java.beans.PropertyDescriptor was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
            return ((PropertyInfo) methods.Get(name)).GetSetMethod();
        }

        /// <summary>
        /// Return the read method using the slot name for the key
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual MethodInfo getReadMethod(String name)
        {
            //UPGRADE_TODO: Method java.beans.PropertyDescriptor.getReadMethod was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
            //UPGRADE_TODO: Class java.beans.PropertyDescriptor was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
            return ((PropertyInfo)methods.Get(name)).GetGetMethod();
        }

        /// <summary> Method will make a copy and return it. When a copy is made, the 
        /// Method classes are not cloned. Instead, just the HashMap is cloned.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual Defclass cloneDefclass()
        {
            Defclass dcl = new Defclass(OBJECT_CLASS);
            dcl.addListener = addListener;
            dcl.INFO = INFO;
            dcl.ISBEAN = ISBEAN;
            dcl.PROPS = PROPS;
            dcl.removeListener = removeListener;
            dcl.methods = CollectionFactory.localMap();
            dcl.methods.putAll(methods);
            return dcl;
        }
    }
}