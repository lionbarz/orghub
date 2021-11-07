using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    /// <summary>
    /// Any group of people. It could be a chartered permanent society
    /// or a subcommittee or a mass meeting.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Unique identifier for this organization.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The bylaws governing the organization.
        /// For mass meetings (non-permanent societies), it's null.
        /// </summary>
        public Bylaws? Bylaws { get; set; }
        
        /// <summary>
        /// Current members of the organization.
        /// </summary>
        public ICollection<Person> Members { get; private init; }
        
        /// <summary>
        /// Motions made by this group.
        /// </summary>
        public IList<Motion> Motions { get; private init; }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public Group()
        {
            Members = new List<Person>();
            Motions = new List<Motion>();
        }

        /// <summary>
        /// Adds a new member to the organization.
        /// </summary>
        public void AddMember(Person person)
        {
            Members.Add(person);
        }

        /// <summary>
        /// Make a motion.
        /// </summary>
        public void Move(Motion motion)
        {
            Motions.Add(motion);
        }

        /// <summary>
        /// Get whatever motion is active now, or null.
        /// </summary>
        public Motion? GetActiveMotion()
        {
            if (!Motions.Any())
            {
                return null;
            }
            
            // TODO
            return null;
        }
    }
}