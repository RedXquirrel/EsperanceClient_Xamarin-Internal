// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     AndroidEnvironmentInformation
//  Author:          Ben Moore
//  Created date:    06/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using Android.App;
using Android.OS;
using Android.Provider;
using Donky.Core.Framework;

namespace Donky.Core.Xamarin.Android
{
	/// <summary>
	/// Android implementation of environment info
	/// </summary>
	internal class AndroidEnvironmentInformation : IEnvironmentInformation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AndroidEnvironmentInformation"/> class.
		/// </summary>
		public AndroidEnvironmentInformation()
		{
			
		}

		/// <summary>
		/// Gets the operating system.
		/// </summary>
		public string OperatingSystem
		{
			get { return "Android"; }
		}

		/// <summary>
		/// Gets the operating system version.
		/// </summary>
		public string OperatingSystemVersion
		{
			get { return Build.VERSION.Release; }
		}

		/// <summary>
		/// Gets the device identifier.
		/// </summary>
		public string DeviceId
		{
			get { return Settings.Secure.GetString(Application.Context.ContentResolver, Settings.Secure.AndroidId); }
		}

		/// <summary>
		/// Gets the device model.
		/// </summary>
		public string Model
		{
			get { return Build.Model; }
		}

		/// <summary>
		/// Gets the application identifier / bundle name.
		/// </summary>
		public string AppIdentifier
		{
			get { return Application.Context.PackageName;}
		}
	}
}