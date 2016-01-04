using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Esperance.Core.Framework.DependencyInjection
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
