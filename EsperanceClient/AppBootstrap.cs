using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Esperance.Core;
using Chat.Esperance.Core.Framework.Extensions;

namespace EsperanceClient
{
    public static class AppBootstrap
    {
        public static bool LoggedIn { get; set; }

        public static void Initialise()
        {
            // Initialise any Esperance modules (except Core) here

            InitInternal().ExecuteInBackground();
        }

        private static async Task InitInternal()
        {
            var result = EsperanceCore.Instance;
        }
    }
}
