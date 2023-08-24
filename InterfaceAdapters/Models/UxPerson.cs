using System;

namespace InterfaceAdapters.Models
{
    public class UxPerson
    {
        /// <summary>
        /// Id of this person.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Full name of the person.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email of the person.
        /// </summary>
        public string Email { get; set; }
    }
}