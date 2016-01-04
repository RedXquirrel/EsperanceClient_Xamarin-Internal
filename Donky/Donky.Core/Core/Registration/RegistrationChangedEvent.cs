// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     RegistrationChangedEvent class.
//  Author:          Ben Moore
//  Created date:    16/05/2015
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
using Donky.Core.Events;

namespace Donky.Core.Registration
{
	/// <summary>
	/// LocalEvent that is raised when registration details change.
	/// </summary>
	public class RegistrationChangedEvent : LocalEvent
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationChangedEvent"/> class.
        /// </summary>
        /// <param name="details">The details.</param>
		public RegistrationChangedEvent(RegistrationDetails details)
		{
			Details = details;
			Publisher = DonkyCoreImplementation.CoreModuleDefinition;
		}

        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        /// <value>
        /// The details.
        /// </value>
		public RegistrationDetails Details { get; set; }
	}
}