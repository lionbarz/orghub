using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// The bylaws that define an organization.
    /// </summary>
    public class Bylaws
    {
        /// <summary>
        /// The name of the organization.
        /// Ex: SWANA Democratic Club, Toastmasters
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The mission of the organization.
        /// </summary>
        public string Mission { get; set; }
    }
}