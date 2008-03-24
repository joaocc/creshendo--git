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
using System.Resources;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// Messages is a basic resource bundle. It's responsible for getting
    /// the error messages and other resource bundle related values.
    /// 
    /// </author>
    public class Messages
    {
        private static System.Resources.ResourceManager _resourceManager;
        private static System.Globalization.CultureInfo _resourceCulture;

        static Messages()
        {
            _resourceManager = new ResourceManager("Creshendo.Resources.CreshendoResource", typeof(Messages).Assembly);
        }

        private Messages()
        {
        }

        public static String getString(String key)
        {
            try
            {
                return _resourceManager.GetString(key, _resourceCulture);
            }
            catch (MissingManifestResourceException e)
            {
                return '!' + key + '!';
            }
        }

        public static bool getBooleanProperty(String key)
        {
            String val = getString(key);
            return val.Equals("true", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}