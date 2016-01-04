// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ServiceContext class.
//  Author:          Ben Moore
//  Created date:    03/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
/*
MIT LICENCE:
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE. */
using System;
using Donky.Core.Framework.Storage;

namespace Donky.Core.Services
{
	internal class ServiceContext : StorageBackedContextBase, IServiceContext
	{
		private string _authenticationToken;
		private string _secureServiceBaseUrl;
		private DateTime? _tokenExpiryTime;
		private string _tokenType;

		internal ServiceContext(IPersistentStorage storage)
			: base(storage)
		{
			PublicServiceBaseUrl = "https://client-api.mobiledonky.com/";
		}

		public string AuthenticationToken
		{
			get { return _authenticationToken ?? (_authenticationToken = GetValue("AuthenticationToken")); }
			set
			{
				_authenticationToken = value;
				SetValue(value, "AuthenticationToken");
			}
		}

		public DateTime? TokenExpiryTime
		{
			get { return _tokenExpiryTime ?? (_tokenExpiryTime = GetDateValue("TokenExpiryTime")); }
			set
			{
				_tokenExpiryTime = value;
				if (value.HasValue)
				{
					SetValue(value.Value, "TokenExpiryTime");
				}
				else
				{
					SetValue(null, "TokenExpiryTime");
				}
			}
		}

		public string PublicServiceBaseUrl { get; set; }

		public string SecureServiceBaseUrl
		{
			get { return _secureServiceBaseUrl ?? (_secureServiceBaseUrl = GetValue("SecureServiceBaseUrl")); }
			set
			{
				_secureServiceBaseUrl = value; 
				SetValue(value, "SecureServiceBaseUrl");
			}
		}

		public string TokenType
		{
			get { return _tokenType ?? (_tokenType = GetValue("TokenType")); }
			set
			{
				_tokenType = value; 
				SetValue(value, "TokenType");
			}
		}

		protected override string CreateId(string storageItem)
		{
			return "ServiceContext_" + storageItem;
		}
	}
}