using System;

namespace Core
{
    /// <summary>
    /// The role used to determine a person's privileges in a group
    /// and during a meeting.
    /// </summary>
    [Flags]
    public enum AttendeeRole
    {
        /// <summary>
        /// The person is attending as a guest.
        /// </summary>
        Guest = 1,
        
        /// <summary>
        /// The person is a member of the group.
        /// </summary>
        Member = 2,
        
        /// <summary>
        /// The person is the chair of the group.
        /// </summary>
        Chair = 4,
    }
}