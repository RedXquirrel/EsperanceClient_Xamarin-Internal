using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core.Framework.Extensions
{
    /// <summary>
    /// Extension methods for tasks.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Runs the task in the background without awaiting the response.  Exceptions are logged.
        /// </summary>
        /// <param name="task">The task.</param>
        public static void ExecuteInBackground(this Task task)
        {
            Task.Run(() => task);
        }
    }
}
