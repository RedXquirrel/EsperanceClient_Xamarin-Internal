// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     RichMessageData
//  Author:          Ben Moore
//  Created date:    07/07/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using System;
using Donky.Core.Framework.Data;

namespace Donky.Messaging.Rich.Logic.Data
{
	/// <summary>
	/// Wrapper for persisting Rich Messages.
	/// </summary>
	public class RichMessageData : IEntity<Guid>
	{
		public Guid Id { get; set; }

		public RichMessage Message { get; set; }
	}
}