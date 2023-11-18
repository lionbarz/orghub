using System;
using System.Collections.Generic;

namespace InterfaceAdapters.Models
{
    public class UXGroup
    {
        /// <summary>
        /// Id of this group.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The people in this group.
        /// </summary>
        public IEnumerable<UxPerson> Members { get; set; }
        
        /// <summary>
        /// The person chairing the group.
        /// </summary>
        public UxPerson Chair { get; set; }

        /// <summary>
        /// All resolutions passed by the group.
        /// </summary>
        public IEnumerable<string> Resolutions { get; set; }
        
        /// <summary>
        /// The group name.
        /// </summary>
        public string Name { get; set; }
    }
}