// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     Logger class.
//  Author:          Ben Moore
//  Created date:    29/04/2015
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Donky.Core.Events;
using Donky.Core.Framework.Events;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Storage;
using Donky.Core.Initialisation;
using Donky.Core.Services;

namespace Donky.Core.Framework.Logging
{
	/// <summary>
	/// Logging implementation.
	/// </summary>
	public class Logger : ILogger
	{
		private const long MAX_LOG_FILE_SIZE = 4096;
		private const string FILE1_NAME = "DonkyLog1.txt";
		private const string FILE2_NAME = "DonkyLog2.txt";
		private const string LOG_FOLDER = "DonkyLogs";
		private readonly bool _logToFile;
		private static ILogger _instance = new Logger();
		public static Lazy<IFileStorageFactory> StorageFactory = new Lazy<IFileStorageFactory>(
			() => Bootstrap.Builder.BuildObject<IFileStorageFactory>(), 
			LazyThreadSafetyMode.ExecutionAndPublication);
		private readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1);


		/// <summary>
		/// Initializes a new instance of the <see cref="Logger"/> class.
		/// </summary>
		public Logger() : this(false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Logger"/> class.
		/// </summary>
		/// <param name="logToFile">if set to <c>true</c> [log to file].</param>
		internal Logger(bool logToFile)
		{
			_logToFile = logToFile;
		}

		/// <summary>
		/// Gets or sets the event bus.
		/// </summary>
		internal static IEventBus EventBus { get; set; }

		/// <summary>
		/// Logs a debug level message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">The arguments.</param>
		public void LogDebug(string message, params object[] args)
		{
			Log(LogLevel.Debug, message, args);
		}

		/// <summary>
		/// Logs an information level message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">The arguments.</param>
		public void LogInformation(string message, params object[] args)
		{
			Log(LogLevel.Info, message, args);
		}

		/// <summary>
		/// Logs a warning level message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">The arguments.</param>
		public void LogWarning(string message, params object[] args)
		{
			Log(LogLevel.Warning, message, args);
		}

		/// <summary>
		/// Logs a validation warning.
		/// </summary>
		/// <param name="validationFailures">The validation failures.</param>
		/// <param name="message">The message.</param>
		/// <param name="args">The arguments.</param>
		public void LogValidationWarning(IEnumerable<ValidationFailure> validationFailures, string message, params object[] args)
		{
			Log(LogLevel.Warning, message, args);
			foreach (var failure in validationFailures)
			{
				Log(LogLevel.Warning, "{0} - {1} - {2}", failure.Property, failure.FailureKey, failure.Details);
			}
		}

		/// <summary>
		/// Logs an error level message for the specified exception.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="message">The message.</param>
		/// <param name="args">The arguments.</param>
		public void LogError(Exception exception, string message, params object[] args)
		{
			if (exception != null)
			{
				Log(LogLevel.Error, exception.ToString());
			}

			Log(LogLevel.Error, message, args);
		}

		/// <summary>
		/// Logs an error level message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">The arguments.</param>
		public void LogError(string message, params object[] args)
		{
			LogError(null, message, args);
		}

		/// <summary>
		/// Gets the log contents.
		/// </summary>
		/// <returns></returns>
		public async Task<string> GetLogContentsAsync()
		{
		    try
		    {
		        await _fileLock.WaitAsync();
		        using (var storage = StorageFactory.Value.GetUserStore(LOG_FOLDER))
		        {
		            var logContent = String.Empty;
		            if (storage.FileExists(FILE1_NAME))
		            {
		                using (var reader = storage.OpenReader(FILE1_NAME))
		                {
		                    logContent += await reader.ReadToEndAsync();
		                }
		            }

		            if (storage.FileExists(FILE2_NAME))
		            {
		                using (var reader = storage.OpenReader(FILE2_NAME))
		                {
		                    logContent += await reader.ReadToEndAsync();
		                }
		            }

		            return logContent;
		        }
		    }
			finally
			{
				_fileLock.Release();
			}
		}

		private void Log(LogLevel level, string message, params object[] args)
		{
			var fullMessage = String.Format("{0:yyyy-MM-dd HH:mm:ss}\t{1}: {2}",
				DateTime.UtcNow,
				level.ToString().ToUpperInvariant(),
				message.FormatUsing(args));

			if (level > LogLevel.Debug && _logToFile)
			{
				AppendToLogFile(fullMessage);
			}

			if (EventBus != null)
			{
				EventBus.PublishAsync(new LogEvent
				{
					Message = fullMessage,
					Level = level
				}).ExecuteInBackground();

				if (level >= LogLevel.Error)
				{
					EventBus.PublishAsync(new ErrorEvent()).ExecuteInBackground();
				}
			}

			Debug.WriteLine(fullMessage);
		}

		private void AppendToLogFile(string fullMessage)
		{
			if (StorageFactory == null)
			{
				return;
			}

			try
			{
				_fileLock.Wait();
				using (var storage = StorageFactory.Value.GetUserStore(LOG_FOLDER))
				{
					using (var writer = GetLogWriter(storage))
					{
						writer.WriteLine(fullMessage);
					}
				}
			}
			finally
			{
				_fileLock.Release();
			}
		}

		private StreamWriter GetLogWriter(IFileStorage storage)
		{
			string fileToUse;
			if (!storage.FileExists(FILE1_NAME) || storage.GetFileSize(FILE1_NAME) < MAX_LOG_FILE_SIZE)
			{
				fileToUse = FILE1_NAME;
			}
			else if (!storage.FileExists(FILE2_NAME) || storage.GetFileSize(FILE2_NAME) < MAX_LOG_FILE_SIZE)
			{
				fileToUse = FILE2_NAME;
			}
			else
			{
				storage.DeleteFileIfExists(FILE1_NAME);
				storage.RenameFile(FILE2_NAME, FILE1_NAME);
				fileToUse = FILE2_NAME;
			}

			return storage.OpenWriter(fileToUse, true);
		}

		/// <summary>
		/// Instance of the Logger to use for general logging.
		/// </summary>
		public static ILogger Instance
		{
			get { return _instance; }
			set { _instance = value; }
		}
	}
}