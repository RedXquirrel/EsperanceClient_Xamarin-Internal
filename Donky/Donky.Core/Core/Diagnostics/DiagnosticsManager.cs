// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     Diagnostics Manager implementation.
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
using System.Threading;
using System.Threading.Tasks;
using Donky.Core.Configuration;
using Donky.Core.Events;
using Donky.Core.Framework.Events;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Logging;
using Donky.Core.Notifications;
using Donky.Core.Services.Diagnostics;

namespace Donky.Core.Diagnostics
{
	/// <summary>
	/// Implementation of Diagnostics related logic.
	/// </summary>
	internal class DiagnosticsManager : IDiagnosticsManager
	{
		private const string ALWAYS_SUBMIT_ERRORS = "AlwaysSubmitErrors";

		private readonly IDebugLogService _logService;
		private readonly IConfigurationManager _configurationManager;
	    private readonly ILogger _logger;
		private readonly SemaphoreSlim _uploadLock = new SemaphoreSlim(1);

		internal DiagnosticsManager(IDebugLogService logService, IEventBus eventBus, IConfigurationManager configurationManager, ILogger logger)
		{
			_logService = logService;
			_configurationManager = configurationManager;
			eventBus.Subscribe<ErrorEvent>(HandleErrorEvent);
		    _logger = logger;
		}

		public async Task<string> GetLogAsync()
		{
			return await _logger.GetLogContentsAsync();
		}

		public async Task<ApiResult> SubmitLogAsync()
		{
			return await ApiResult.ForOperationAsync(async () =>
			{
				await GetAndSubmitLogAsync(LogSubmissionReason.ManualRequest);
			}, "SubmitLogAsync");
		}

		public Task HandleTransmitDebugLogAsync(ServerNotification notification)
		{
			return GetAndSubmitLogAsync(LogSubmissionReason.ManualRequest);
		}

		private void HandleErrorEvent(ErrorEvent errorEvent)
		{
			if (_configurationManager.GetValue<bool>(ALWAYS_SUBMIT_ERRORS))
			{
				GetAndSubmitLogAsync(LogSubmissionReason.AutomaticByDevice).ExecuteInBackground();
			}
		}

		private async Task GetAndSubmitLogAsync(LogSubmissionReason reason)
		{
			// We only want to allow one upload operation at a time.
			await _uploadLock.WaitAsync();
			try
			{
				var log = await _logger.GetLogContentsAsync();
				await SubmitLogInternalAsync(log, reason);
			}
			finally
			{
				_uploadLock.Release();
			}
		}

		private async Task SubmitLogInternalAsync(string log, LogSubmissionReason reason)
		{
			var result = await _logService.Upload(new DebugLog
			{
				Data = log,
				SubmissionReason = reason
			});

            _logger.LogDebug("UploadLog result - always submit errors: {0}", result.AlwaysSubmitErrors);

			await _configurationManager.SetValueAsync(ALWAYS_SUBMIT_ERRORS, result.AlwaysSubmitErrors.ToString());
		}
	}
}