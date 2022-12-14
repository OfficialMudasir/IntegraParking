/*
 * Copyright (c) 2014 Optimal Payments
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
 * NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paysafe.Common;

namespace Paysafe.DirectDebit
{
    //Created by Manjiri.Bankar on 03.05.2016. This is Profile class.
    public class Profile : JSONObject
    {
        /// <summary>
        /// Initialize the Profile object with some set of properties
        /// </summary>
        /// <param name="properties">Dictionary<string, object></param>
        public Profile(Dictionary<string, object> properties = null)
            : base(fieldTypes, properties)
        {
        }

        private static new Dictionary<string, object> fieldTypes = new Dictionary<string, object>
         {
             {DirectDebitConstants.firstName, STRING_TYPE},
             {DirectDebitConstants.lastName, STRING_TYPE},
             {DirectDebitConstants.email, EMAIL_TYPE},
            // {DirectDebitConstants.ssn,STRING_TYPE},
             {DirectDebitConstants.dateOfBirth,typeof(DateOfBirth)},
         };


        /// <summary>
        /// Get the firstName
        /// </summary>
        /// <returns>string</returns>
        public string firstName()
        {
            return this.getProperty(DirectDebitConstants.firstName);
        }

        /// <summary>
        /// Set the firstName
        /// </summary>
        /// <returns>void</returns>
        public void firstName(string data)
        {
            this.setProperty(DirectDebitConstants.firstName, data);
        }

        /// <summary>
        /// Get the lastName
        /// </summary>
        /// <returns>string</returns>
        public string lastName()
        {
            return this.getProperty(DirectDebitConstants.lastName);
        }

        /// <summary>
        /// Set the lastName
        /// </summary>
        /// <returns>void</returns>
        public void lastName(string data)
        {
            this.setProperty(DirectDebitConstants.lastName, data);
        }

        /// <summary>
        /// Get the email
        /// </summary>
        /// <returns>string</returns>
        public string email()
        {
            return this.getProperty(DirectDebitConstants.email);
        }

        /// <summary>
        /// Set the email
        /// </summary>
        /// <returns>void</returns>
        public void email(string data)
        {
            this.setProperty(DirectDebitConstants.email, data);
        }

        /// <summary>
        /// Get the dateOfBirth
        /// </summary>
        /// <returns>DateOfBirth</returns>
        public DateOfBirth dateOfBirth()
        {
            return this.getProperty(DirectDebitConstants.dateOfBirth);
        }

        /// <summary>
        /// Set the dateOfBirth
        /// </summary>
        /// <returns>void</returns>
        public void dateOfBirth(DateOfBirth data)
        {
            this.setProperty(DirectDebitConstants.dateOfBirth, data);
        }


        /// <summary>
        /// ProfileBuilder<typeparam name="TBLDR"></typeparam> will allow a Profile to be initialized
        /// within another builder. Set properties and subpropeties, then trigger .Done() to 
        /// get back to the parent builder
        /// </summary>
        public class ProfileBuilder<TBLDR> : NestedJSONBuilder<Profile, TBLDR>
            where TBLDR : GenericJSONBuilder
        {
            /// <summary>
            /// Initialize the Profile builder within the context of a parent builder
            /// </summary>
            /// <param name="parent">TBLDR</param>
            public ProfileBuilder(TBLDR parent)
                : base(parent)
            {
                this.parent = parent;
            }

            /// <summary>
            /// Set the firstname
            /// </summary>
            /// <param name=data>string</param>
            /// <returns>ProfileBuilder<TBLDR></returns>
            public ProfileBuilder<TBLDR> firstName(string data)
            {
                this.properties[DirectDebitConstants.firstName] = data;
                return this;
            }

            /// <summary>
            /// Set the lastname
            /// </summary>
            /// <param name=data>string</param>
            /// <returns>ProfileBuilder<TBLDR></returns>
            public ProfileBuilder<TBLDR> lastName(string data)
            {
                this.properties[DirectDebitConstants.lastName] = data;
                return this;
            }

            /// <summary>
            /// Set the email
            /// </summary>
            /// <param name=data>string</param>
            /// <returns>ProfileBuilder<TBLDR></returns>
            public ProfileBuilder<TBLDR> email(string data)
            {
                this.properties[DirectDebitConstants.email] = data;
                return this;
            }           
        }
    }
}
