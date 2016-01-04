using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core.Services
{
    /// <summary>
    /// Validation Failure.
    /// </summary>
	public class ValidationFailure
    {
        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        /// <value>
        /// The details.
        /// </value>
		public string Details { get; set; }

        /// <summary>
        /// Gets or sets the failure key.
        /// </summary>
        /// <value>
        /// The failure key.
        /// </value>
		public string FailureKey { get; set; }

        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        /// <value>
        /// The property.
        /// </value>
		public string Property { get; set; }
    }
}
