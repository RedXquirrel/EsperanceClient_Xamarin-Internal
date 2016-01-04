using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donky.Core.Exceptions
{
    public class ServerUnavailablePollPeriodConfigurationException : Exception
    {
        public ServerUnavailablePollPeriodConfigurationException(string details, Exception innerException) : base(string.Format("ServerUnavailablePollPeriodConfigurationException: Unable to determine ServerUnavailablePollPeriod Retry policy. Details: {0}. Inner Exception: {1} [Donky.Core.Exceptions]", details, innerException.Message)) { }
    }
}
