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
	/// The purpose of ObjectFilter is to lookup and return the BeanFilter
	/// for a given class.
	/// 
	/// </author>
	public class ObjectFilter
	{
		
		public static BeanFilter lookupFilter(System.Type clazz)
		{
			return null;
		}
	}
}