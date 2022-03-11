using System;

namespace InterfaceAdapters.Models
{
    public class UXPerson
    {
        /// <summary>
        /// Id of this person.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Full name of the person.
        /// </summary>
        public string Name { get; set; }
    }
}