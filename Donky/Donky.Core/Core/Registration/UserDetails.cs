// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     UserDetails class.
//  Author:          Ben Moore
//  Created date:    29/04/2015
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
using System.Collections.Generic;
using System.Linq;

namespace Donky.Core.Registration
{
	/// <summary>
	/// Details of the user registration.
	/// </summary>
	public class UserDetails
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDetails"/> class.
        /// </summary>
		public UserDetails()
		{
			
		}

		internal UserDetails(UserDetails source)
		{
			UserId = source.UserId;
			IsAnonymous = source.IsAnonymous;
			DisplayName = source.DisplayName;
			FirstName = source.FirstName;
			LastName = source.LastName;
			EmailAddress = source.EmailAddress;
			MobileNumber = source.MobileNumber;
			CountryCode = source.CountryCode;
			AvatarAssetId = source.CountryCode;
			SelectedTags = source.SelectedTags == null
				? new string[0]
				: source.SelectedTags.ToArray();
			AdditionalProperties = source.AdditionalProperties == null
				? null
				: source.AdditionalProperties.ToDictionary(
					s => s.Key,
					s => s.Value
					);
		}

		/// <summary>
		/// The user id, unique within the AppSpace.
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// Indicates whether this registration is anonymous or not.
		/// </summary>
		public bool IsAnonymous { get; internal set; }

		/// <summary>
		/// The user's display name.
		/// </summary>
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the email address.
		/// </summary>
		public string EmailAddress { get; set; }

		/// <summary>
		/// Gets or sets the mobile number.
		/// </summary>
		public string MobileNumber { get; set; }

		/// <summary>
		/// Gets or sets the country code.
		/// </summary>
		/// <remarks>Must be a 3 letter ISO country code.</remarks>
		public string CountryCode { get; set; }

		/// <summary>
		/// Gets or sets the avatar asset identifier.
		/// </summary>
		public string AvatarAssetId { get; set; }

		/// <summary>
		/// Gets or sets the selected tags.
		/// </summary>
		public string[] SelectedTags { get; internal set; }

		/// <summary>
		/// Gets or sets the additional properties.
		/// </summary>
		public Dictionary<string, string> AdditionalProperties { get; set; }
	}
}