// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     PushDataContext class.
//  Author:          Ben Moore
//  Created date:    27/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core.Framework.Data;
using Donky.Core.Framework.Logging;
using Donky.Core.Framework.Storage;

namespace Donky.Messaging.Push.Logic.Data
{
	internal class PushDataContext : DataContextBase, IPushDataContext
	{
        public PushDataContext(IPersistentStorage storage, ILogger logger)
            : base(storage, logger)
		{
		}

		public IEntitySet<PendingSimplePushMessage, Guid> SimplePushMessages
		{
			get { return GetEntitySet<PendingSimplePushMessage, Guid>(); }
		}

		public IEntitySet<PendingMessageInteraction, Guid> MessageInteractions
		{
			get { return GetEntitySet<PendingMessageInteraction, Guid>(); }
		}
	}
}