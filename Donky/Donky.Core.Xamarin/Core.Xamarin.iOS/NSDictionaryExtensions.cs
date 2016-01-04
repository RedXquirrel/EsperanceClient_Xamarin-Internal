// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     NSDictionaryExtensions class
//  Author:          Ben Moore
//  Created date:    20/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core.Framework.Logging;
using Foundation;

namespace Donky.Core.Xamarin.iOS
{
	/// <summary>
	/// Extension methods for NSDictionary
	/// </summary>
	public static class NSDictionaryExtensions
	{
		public static T GetValue<T>(this NSDictionary dictionary, string key)
		{
			object rawValue = dictionary[new NSString(key)];
			Logger.Instance.LogDebug("Trying to get value from NSDictionary with key {0} as {1}",
				key, typeof(T).Name);
			if (!typeof (NSObject).IsAssignableFrom(typeof (T)))
			{
				rawValue = rawValue.ToString();
				return (T)Convert.ChangeType(rawValue, typeof(T));
			}

			return (T)rawValue;
		}

		public static bool ContainsKey(this NSDictionary dictionary, string key)
		{
			return dictionary.ContainsKey(new NSString(key));
		}

		public static T GetValueOrDefault<T>(this NSDictionary dictionary, string key, T defaultValue)
		{
			if (dictionary.ContainsKey(key))
			{
				return dictionary.GetValue<T>(key);
			}

			return defaultValue;
		}
	}
}