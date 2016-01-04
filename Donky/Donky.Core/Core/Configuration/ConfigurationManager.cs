// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ConfigurationManager implementation.
//  Author:          Ben Moore
//  Created date:    14/05/2015
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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Donky.Core.Framework.Events;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Logging;
using Donky.Core.Framework.Storage;
using Newtonsoft.Json.Linq;

namespace Donky.Core.Configuration
{
	internal class ConfigurationManager : StorageBackedContextBase, IConfigurationManager
	{
		private readonly IEventBus _eventBus;
	    private readonly ILogger _logger;
		private ConcurrentDictionary<string, string> _values; 

		internal ConfigurationManager(IPersistentStorage storage, IEventBus eventBus, ILogger logger)
			: base(storage)
		{
			_eventBus = eventBus;
		    _logger = logger;
		}

		public async Task InitialiseAsync()
		{
            _logger.LogDebug("Initialising configuration");
			var data = await GetValueAsync<Dictionary<string, string>>("ConfigurationItems");

			_values = data == null
				? new ConcurrentDictionary<string, string>()
				: new ConcurrentDictionary<string, string>(data);
		}

		public async Task UpdateConfigurationAsync(Dictionary<string, string> values, Dictionary<string, JObject> configurationSets)
		{
            if (values != null)
            {
                await SetValuesAsync(values);
            }

			_eventBus.PublishAsync(new ConfigurationUpdatedEvent
			{
				Values = values,
				ConfigurationSets = configurationSets,
				Publisher = DonkyCoreImplementation.CoreModuleDefinition
			}).ExecuteInBackground();
		}

		public async Task SetValuesAsync(Dictionary<string, string> values)
		{
			foreach (var item in values)
			{
				SetValueInternal(item.Key, item.Value);
			}

			await SetValueAsync(_values, "ConfigurationItems");
		}

		public async Task SetValueAsync(string key, string value)
		{
			SetValueInternal(key, value);
			await SetValueAsync(_values, "ConfigurationItems");
		}

		public T GetValue<T>(string key)
		{
			string rawValue;
			if (!_values.TryGetValue(key, out rawValue))
			{
				return default(T);
			}

			return (T) Convert.ChangeType(rawValue, typeof (T));
		}

		private void SetValueInternal(string key, string value)
		{
			if (!_values.ContainsKey(key) || _values[key] != value)
			{
                _logger.LogDebug("Setting config: {0} = {1}", key, value);
				_values[key] = value;
			}
		}

		protected override string CreateId(string storageItem)
		{
			return "ConfigurationManager_" + storageItem;
		}
	}
}