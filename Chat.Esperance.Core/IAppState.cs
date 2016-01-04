using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core
{
    /// <summary>
    /// Defines the current state of the App.
    /// </summary>
    public interface IAppState
    {
        /// <summary>
        /// Returns true if the app is open.
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// Sets the current state.
        /// </summary>
        /// <param name="isOpen">if set to <c>true</c> the app is open.</param>
        void SetState(bool isOpen);
    }
}
