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
namespace Test.Creshendo.Model
{
    public interface IAccount
    {
        string Title { get; set; }
        string First { get; set; }
        string Last { get; set; }
        string Middle { get; set; }
        string OfficeCode { get; set; }
        string RegionCode { get; set; }
        string Status { get; set; }
        string AccountId { get; set; }
        string AccountType { get; set; }
        string Username { get; set; }
        string AreaCode { get; set; }
        string Exchange { get; set; }
        string Number { get; set; }
        string Ext { get; set; }
    }
}