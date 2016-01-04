using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donky.Messaging.Rich.Inbox.XamarinForms.Services
{
    using System;

    public sealed class RichInboxUtilityService
    {
        private static volatile RichInboxUtilityService instance;
        private static object syncRoot = new Object();

        private RichInboxUtilityService() { }

        public static RichInboxUtilityService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new RichInboxUtilityService();
                    }
                }

                return instance;
            }
        }

        public int TotalMinutesSinceSent(DateTime now, DateTime sent)
        {
            return Convert.ToInt32((now - sent).TotalMinutes);
        }
    }
}
