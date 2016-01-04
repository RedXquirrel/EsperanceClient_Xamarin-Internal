// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     
//  Author:          Ben Moore
//  Created date:    07/07/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using System;
using Donky.Core.Framework.Data;

namespace Donky.Messaging.Rich.Logic.Data
{
	public interface IRichDataContext : IDataContext
	{
		IEntitySet<RichMessageData, Guid> RichMessages { get; }
	}
}