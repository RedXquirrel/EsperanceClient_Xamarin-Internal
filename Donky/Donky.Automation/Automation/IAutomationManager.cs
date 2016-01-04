// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IAutomationManager interface.
//  Author:          Ben Moore
//  Created date:    22/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;

namespace Donky.Automation
{
	/// <summary>
	/// Operations for triggers and automation related actions.
	/// </summary>
	public interface IAutomationManager
	{
		/// <summary>
		/// Queues a request for the Donky Network to process any 3rd party triggers with the specified key.
		/// </summary>
		/// <param name="triggerKey">The trigger key.</param>
		/// <param name="customData">Custom data to be sent along with the request.</param>
		/// <param name="sendImmediately">if set to <c>true</c> the notification will be sent immediately, otherwise it will be queued for the next synchronise operation.</param>
		void ExecuteThirdPartyTrigger(string triggerKey, 
			Dictionary<string, string> customData = null,
			bool sendImmediately = false);
	}
}