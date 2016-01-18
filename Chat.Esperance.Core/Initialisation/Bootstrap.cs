using Chat.Esperance.Core.Framework.DependencyInjection;
using Chat.Esperance.Core.Framework.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Chat.Esperance.Core.Framework.Network.Donky.Core.Framework.Network;
using Chat.Esperance.Core.Services;
using Chat.Esperance.Core.Framework.Registration;

namespace Chat.Esperance.Core.Initialisation
{
    internal static class Bootstrap
    {
        internal static IObjectBuilder Builder;

        internal static void RegisterDependencies()
        {
            // Need to explicitly hook up internal constructors otherwise DI won't work
            RegisterEsperanceApi();
            RegisterServices();
        }

        internal static void Initialise()
        {
            var builder = new AutofacObjectBuilder();
            RegisterDependencies();

            Builder = builder;
        }

        private static void RegisterServices()
        {
            AutofacObjectBuilder.ContainerBuilder.Register(
                c => new EsperanceHttpClient())
                .As<IHttpClient>()
                .SingleInstance();

            AutofacObjectBuilder.ContainerBuilder.Register(
                c => new RegistrationManager())
                .As<IRegistrationManager>()
                .SingleInstance();
        }

        private static void RegisterEsperanceApi()
        {
            AutofacObjectBuilder.ContainerBuilder.Register(c =>
                new EsperanceCoreImplementation(
                    c.Resolve<IRegistrationManager>(),
                    c.Resolve<IObjectBuilder>()))
                .As<IEsperanceCore>()
                .SingleInstance();

             //Register the public controller interfaces as mappings
            //AutofacObjectBuilder.ContainerBuilder.Register(c =>
            //    c.Resolve<IRegistrationManager>())
            //    .As<IRegistrationManager>()
            //    .SingleInstance();

            //AutofacObjectBuilder.ContainerBuilder.Register(c =>
            //    c.Resolve<INotificationManager>())
            //    .As<INotificationController>()
            //    .SingleInstance();

            //AutofacObjectBuilder.ContainerBuilder.Register(c =>
            //    c.Resolve<IDiagnosticsManager>())
            //    .As<ILoggingController>()
            //    .SingleInstance();
        }
    }
}
