// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ApiResult class.
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
using System.Threading.Tasks;
using Donky.Core.Exceptions;
using Donky.Core.Framework.Logging;
using Donky.Core.Services;

namespace Donky.Core
{
	/// <summary>
	/// Represents the result of a Donky Network operation.
	/// </summary>
	public class ApiResult
	{
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public static ILogger Logger { get; set; }

		private ApiResult(bool success, List<ValidationFailure> validationFailures = null)
		{
			Success = success;
			ValidationFailures = validationFailures;
		}

		/// <summary>
		/// Indicates whether the operation was successful.
		/// </summary>
		public bool Success { get; private set; }

		/// <summary>
		/// Details of any validation failures.
		/// </summary>
		public List<ValidationFailure> ValidationFailures { get; private set; }

		/// <summary>
		/// Reason for other failures.
		/// </summary>
		public string OtherFailureReason { get; private set; }

        /// <summary>
        /// Gets the success result.
        /// </summary>
        /// <value>
        /// The success result.
        /// </value>
		public static ApiResult SuccessResult
		{
			get { return new ApiResult(true); }
		}

        /// <summary>
        /// Fors the operation asynchronous.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
		public static async Task<ApiResult> ForOperationAsync(Func<Task> action, string operation)
		{
			ApiResult result;
			try
			{
				await action();
				result = SuccessResult;
			}
			catch (ArgumentException argumentException)
			{
				var failure = new ValidationFailure
				{
					Property = argumentException.ParamName,
					Details = argumentException.Message
				};
                result = new ApiResult(false, new List<ValidationFailure> { failure });
                Logger.LogValidationWarning(result.ValidationFailures,
                    "ArgumentException processing operation {0}", operation);

			}
			catch (ValidationException validationException)
			{
				Logger.LogValidationWarning(validationException.ValidationFailures,
					"Validation failure processing operation {0}", operation);
				result = new ApiResult(false, validationException.ValidationFailures);
			}
			catch (SuspendedException exception)
			{
				Logger.LogInformation("Client is suspended and cannot perform this operation.");
				result = new ApiResult(false) {OtherFailureReason = exception.Message};
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, "Unhandled exception in operation {0}", operation);
				result = new ApiResult(false);
			}

			return result;
		}
	}
}