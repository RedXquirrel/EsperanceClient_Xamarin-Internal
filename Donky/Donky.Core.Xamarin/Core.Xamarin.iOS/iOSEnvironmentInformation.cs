// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     iSOEnvironmentInformation class.
//  Author:          Ben Moore
//  Created date:    06/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core.Framework;
using Foundation;
using Security;
using UIKit;

namespace Donky.Core.Xamarin.iOS
{
	/// <summary>
	/// iOS environment info
	/// </summary>
	internal class iOSEnvironmentInformation : IEnvironmentInformation
	{
		private const string DONKY_DEVICE_ID = "DonkyDeviceId";

		public iOSEnvironmentInformation()
		{
		}

		public string OperatingSystem
		{
			get { return "iOS"; }
		}

		public string OperatingSystemVersion
		{
			get { return UIDevice.CurrentDevice.SystemVersion; }
		}

		public string DeviceId
		{
			get
			{
				string deviceId;
				var record = new SecRecord(SecKind.GenericPassword)
				{
					Service = DONKY_DEVICE_ID
				};

				var match = SecKeyChain.QueryAsData(record);
				if (match != null)
				{
					deviceId = match.ToString();
				}
				else
				{
					deviceId = Guid.NewGuid().ToString();
					record.ValueData = NSData.FromString(deviceId);
					SecKeyChain.Add(record);
				}

				return deviceId;
			}
		}

		public string Model
		{
			get { return UIDevice.CurrentDevice.Model; }
		}

		public string AppIdentifier
		{
			get { return NSBundle.MainBundle.BundleIdentifier; }
		}
	}
}