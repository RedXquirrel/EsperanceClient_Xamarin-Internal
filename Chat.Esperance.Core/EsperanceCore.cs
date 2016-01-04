using Chat.Esperance.Core.Framework.DependencyInjection;
using Chat.Esperance.Core.Initialisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Esperance.Core
{
    public static class EsperanceCore
    {
        private static Lazy<IEsperanceCore> _lazyInstance = new Lazy<IEsperanceCore>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);
        private static bool _isInitialised;
        private static readonly object Lock = new object();

        /// <summary>
        /// Static instance of the Esperance Core APIs.
        /// </summary>
        public static IEsperanceCore Instance
        {
            get { return _lazyInstance.Value; }
        }

        /// <summary>
        /// The object registry.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Object registration can only be performed before the instance is initialised.</exception>
        public static IObjectRegistry Registry
        {
            get
            {
                EnsureInitialised();
                if (_lazyInstance.IsValueCreated)
                {
                    throw new InvalidOperationException("Object registration can only be performed before the instance is initialised");
                }

                return Bootstrap.Builder;
            }
        }

        internal static void Reset()
        {
            _lazyInstance = new Lazy<IEsperanceCore>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);
            _isInitialised = false;
        }

        private static IEsperanceCore CreateInstance()
        {
            EnsureInitialised();
            return Bootstrap.Builder.BuildObject<IEsperanceCore>();
        }

        private static void EnsureInitialised()
        {
            if (!_isInitialised)
            {
                lock (Lock)
                {
                    if (!_isInitialised)
                    {
                        Bootstrap.Initialise();
                        _isInitialised = true;
                    }
                }
            }
        }

    }

}
