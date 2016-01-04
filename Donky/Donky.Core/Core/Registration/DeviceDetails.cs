// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DeviceDetails class.
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

using System.Collections.Generic;
using System.Linq;

namespace Donky.Core.Registration
{
	/// <summary>
	/// Details of the registered device.
	/// </summary>
	public class DeviceDetails
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceDetails"/> class.
        /// </summary>
		public DeviceDetails()
		{
			
		}

		internal DeviceDetails(DeviceDetails source)
		{
			Type = source.Type;
			DeviceName = source.DeviceName;
			DeviceId = source.DeviceId;
			Model = source.Model;
			OperatingSystem = source.OperatingSystem;
			OperatingSystemVersion = source.OperatingSystemVersion;
			AdditionalProperties = source.AdditionalProperties == null
				? null
				: source.AdditionalProperties.ToDictionary(
					s => s.Key,
					s => s.Value);
		}

		/// <summary>
		/// The device type.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// The name of the device.
		/// </summary>
		public string DeviceName { get; set; }

		/// <summary>
		/// Custom properties attached to the device.
		/// </summary>
		public Dictionary<string, string> AdditionalProperties { get; set; }

		/// <summary>
		/// Unique id for the device.
		/// </summary>
		public string DeviceId { get; internal set; }

		/// <summary>
		/// The device model.
		/// </summary>
		public string Model { get; internal set; }

		/// <summary>
		/// The device operating system.
		/// </summary>
		public string OperatingSystem { get; internal set; }

		/// <summary>
		/// The device operating system version.
		/// </summary>
		public string OperatingSystemVersion { get; internal set; }
	}
}