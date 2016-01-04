using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donky.Core.Exceptions
{
    public class VersionFormatException : Exception
    {
        public VersionFormatException(string version) : base(string.Format("VersionFormatException: The version {0} is not in the format x.x.x.x where x is a digit. [Donky.Core.Exceptions]")) { }
    }
}
