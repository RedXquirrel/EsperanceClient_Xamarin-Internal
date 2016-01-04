namespace Donky.Core.Xamarin.Android
{
	/// <summary>
	/// General settings for Donky Android
	/// </summary>
	public class DonkyAndroidSettings
	{
		internal DonkyAndroidSettings()
		{
			NewDeviceNotificationMessageFormat = "A new {0} device ({1}) has been added to your account";
			NewDeviceNotificationEnabled = true;
			NewDeviceNotificateTitle = "New Device Added";
		}
        
		/// <summary>
		/// The message format to use when processing a New Device notification
		/// </summary>
		/// <remarks>
		/// There are 2 available parameters - {0} is the device OS, {1} is the Model
		/// The default value is: A new {0} device ({1}) has been added to your account
		/// </remarks>
		public string NewDeviceNotificationMessageFormat { get; set; }

		/// <summary>
		/// The title to display on the new device notification. 
		/// </summary>
		/// <remarks>
		/// Defaults to 'New Device Added'
		/// </remarks>
		public string NewDeviceNotificateTitle { get; set; }

		/// <summary>
		/// If true, the a local notification will be created when an New Device is added to the user's account.
		/// </summary>
		/// <remarks>
		/// Defaults to true.
		/// 
		/// Requires the New Device feature to be enabled on the Donky Network
		/// </remarks>
		public bool NewDeviceNotificationEnabled { get; set; }

		/// <summary>
		/// The SenderId value to use when registing for GCM.
		/// </summary>
		/// <remarks>
		/// If not set, the Donky default channel will be used.
		/// </remarks>
		public string GcmSenderId { get; set; }
	}
}