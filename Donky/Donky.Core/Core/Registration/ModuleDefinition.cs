// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ModuleDefinition class.
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
using System.Text.RegularExpressions;
using Donky.Core.Exceptions;
using Donky.Core.Services;

namespace Donky.Core.Registration
{
	/// <summary>
	/// Definition of a module.
	/// </summary>
	public class ModuleDefinition
	{
		internal ModuleDefinition()
		{
		}

		public ModuleDefinition(string name, string version)
		{
			Name = name;
			Version = version;

		    if (version != null)
		    {
		        
		    }
		}

	    public void Validate()
	    {

            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentNullException(Name);
            }

            if (string.IsNullOrEmpty(Version))
            {
                throw new ArgumentNullException(Version);
            }
            Regex regex = new Regex(@"\d+.\d+.\d+.\d+");
	        Match match = regex.Match(Version);
            if (!match.Success)
            {
                throw new VersionFormatException(Version);
            }
	    }

		public string Name { get; set; }

		public string Version { get; set; }
	}
}