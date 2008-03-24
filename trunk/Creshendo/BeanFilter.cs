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
namespace org.jamocha.rete
{
	using System;
	/// <author>  Peter Lin
	/// *
	/// Base interface for filtering PropertyDescriptor. Depending on the
	/// java version, the filter may use Annotations, BeanInfo or some
	/// other method like an external properties file.
	/// 
	/// </author>
	public interface BeanFilter : 
		{
			//UPGRADE_TODO: Class java.beans.PropertyDescriptor was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
			/// <summary> BeanFilters must implement this interface. It takes the
			/// PropertyDescriptor array returned from Introspection and
			/// removes any that should not be included.
			/// </summary>
			/// <param name="">props
			/// </param>
			/// <returns>
			/// 
			/// </returns>
			PropertyDescriptor[] filter(PropertyDescriptor[] props);
		}
}