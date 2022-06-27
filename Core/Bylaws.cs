using System;

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
        
        /// <summary>
        /// The minimum amount of advanced notice that
        /// a group needs to be given before a meeting.
        /// </summary>
        public TimeSpan MinimumMeetingNotice { get; set; }
        
        /// <summary>
        /// The required quorum in a meeting.
        /// </summary>
        public Quorum MeetingQuorum { get; set; }

        public Bylaws(string name, string mission)
        {
            Name = name;
            Mission = mission;
        }

        public static Bylaws Default(string groupName)
        {
            return new Bylaws(groupName, "Make a difference.")
            {
                MinimumMeetingNotice = TimeSpan.FromDays(3)
            };
        }
    }
}