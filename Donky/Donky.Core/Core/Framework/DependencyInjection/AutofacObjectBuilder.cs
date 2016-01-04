// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     AutofacObjectBuilder class.
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
using System.Threading;
using Autofac;

namespace Donky.Core.Framework.DependencyInjection
{
	/// <summary>
	/// DI wrapper that uses Autofac.
	/// </summary>
	internal class AutofacObjectBuilder : IObjectBuilder
	{
		private static Lazy<IContainer> _container = new Lazy<IContainer>(InitialiseContainer, LazyThreadSafetyMode.ExecutionAndPublication);

		private static Lazy<ContainerBuilder> _builder = new Lazy<ContainerBuilder>(() => new ContainerBuilder(), LazyThreadSafetyMode.ExecutionAndPublication);

		private static ContainerBuilder _updateBuilder;

		internal static void Reset()
		{
			_container = new Lazy<IContainer>(InitialiseContainer, LazyThreadSafetyMode.ExecutionAndPublication);

			_builder = new Lazy<ContainerBuilder>(() => new ContainerBuilder(), LazyThreadSafetyMode.ExecutionAndPublication);
		}

		public T BuildObject<T>(string name = null) where T : class
		{
			if (name == null)
			{
				return _container.Value.Resolve<T>();
			}

			return _container.Value.IsRegisteredWithName<T>(name) 
				? _container.Value.ResolveNamed<T>(name) 
				: null;
		}

		public void BeginRegistrationBatch()
		{
			_updateBuilder = new ContainerBuilder();
		}

		public void AddRegistration<T>(T instance) where T : class
		{
			var single = _updateBuilder == null;
			var builder = single ? new ContainerBuilder() : _updateBuilder;

			builder.RegisterInstance(instance)
				.As<T>();

			if (single)
			{
				builder.Update(_container.Value);
			}
		}

		public void AddRegistration<T>(Func<T> creator) where T : class
		{
			var single = _updateBuilder == null;
			var builder = single ? new ContainerBuilder() : _updateBuilder;

			builder.Register(c => creator())
				.As<T>()
				.SingleInstance();

			if (single)
			{
				builder.Update(_container.Value);
			}
		}

		public void AddRegistration<TService, TInstance>() where TService : class where TInstance : TService
		{
			var single = _updateBuilder == null;
			var builder = single ? new ContainerBuilder() : _updateBuilder;

			builder.RegisterType<TInstance>()
				.As<TService>()
				.SingleInstance();

			if (single)
			{
				builder.Update(_container.Value);
			}
		}

		public void CommitRegistrationBatch()
		{
			_updateBuilder.Update(_container.Value);
			_updateBuilder = null;
		}

		public static ContainerBuilder ContainerBuilder
		{
			get { return _builder.Value; }
		}

		private static IContainer InitialiseContainer()
		{
			ContainerBuilder.RegisterType<AutofacObjectBuilder>()
				.As<IObjectBuilder>()
				.SingleInstance();

			return ContainerBuilder.Build();
		}
	}
}