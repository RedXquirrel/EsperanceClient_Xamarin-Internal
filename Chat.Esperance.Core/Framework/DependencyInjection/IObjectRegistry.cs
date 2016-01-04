using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core.Framework.DependencyInjection
{
    /// <summary>
    /// Provides access to the Donky dependency injection framework.
    /// </summary>
    public interface IObjectRegistry
    {
        /// <summary>
        /// Begins a batch of registrations.
        /// </summary>
        void BeginRegistrationBatch();

        /// <summary>
        /// Adds a registration using an existing object instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        void AddRegistration<T>(T instance) where T : class;

        /// <summary>
        /// Adds a registration using the specified factory function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="creator">The creator.</param>
        void AddRegistration<T>(Func<T> creator) where T : class;

        /// <summary>
        /// Adds a registration using a type mapping.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        void AddRegistration<TService, TInstance>()
            where TInstance : TService
            where TService : class;

        /// <summary>
        /// Commits a batch of registrations.
        /// </summary>
        void CommitRegistrationBatch();
    }
}
