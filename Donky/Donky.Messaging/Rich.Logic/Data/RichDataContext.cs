// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     RichDataContext class.
//  Author:          Ben Moore
//  Created date:    07/07/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using System;
using Donky.Core.Framework.Data;
using Donky.Core.Framework.Logging;
using Donky.Core.Framework.Storage;

namespace Donky.Messaging.Rich.Logic.Data
{
	public class RichDataContext : DataContextBase, IRichDataContext
	{
		public RichDataContext(IPersistentStorage storage, ILogger logger) : base(storage, logger)
		{
		}

		public IEntitySet<RichMessageData, Guid> RichMessages
		{
			get { return GetEntitySet<RichMessageData, Guid>(); }
		}
	}
}