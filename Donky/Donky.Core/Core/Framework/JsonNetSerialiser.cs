﻿// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     JsonNetSerialiser class.
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
using Newtonsoft.Json;

namespace Donky.Core.Framework
{
	/// <summary>
	/// IJsonSerialiser implementation that uses JSON.net
	/// </summary>
	internal class JsonNetSerialiser : IJsonSerialiser
	{
		private readonly JsonSerializerSettings _settings;

		public JsonNetSerialiser()
		{
			_settings = JsonSettings.CreateStandardSettings();
		}

		public string Serialise(object value)
		{
			return JsonConvert.SerializeObject(value, _settings);
		}

		public T Deserialise<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json, _settings);
		}
	}
}