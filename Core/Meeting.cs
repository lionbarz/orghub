using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    /// <summary>
    /// A group meeting, such as a regular meeting or mass meeting.
    /// This class doesn't know anything about the organization
    /// hosting it, or its membership, nor does it care. It just
    /// cares who can vote here and who is in charge here. It just
    /// runs the meeting.
    /// </summary>
    public class Meeting
    {
        /// <summary>
        /// Uniquely identifies this meeting.
        /// </summary>
        public Guid Id { get; }
        
        /// <summary>
        /// The call for the meeting. What's on the invitation.
        /// </summary>
        public string Description { get; private init; }
        
        /// <summary>
        /// When the meeting is scheduled to take place.
        /// </summary>
        public DateTimeOffset StartTime { get; private init; }
        
        /// <summary>
        /// This can be an online meeting link or a physical address.
        /// </summary>
        public string Location { get; private init; }

        /// <summary>
        /// The attendees at any point of the meeting.
        /// </summary>
        public ICollection<PersonRole> Attendees { get; private set; }

        /// <summary>
        /// How many members need to be present in order
        /// for business to be legally conducted.
        /// Zero means there is no minimum, or that the
        /// quorum is the number present at the time.
        /// </summary>
        public int Quorum { get; set; }

        /// <summary>
        /// Whether there is a quorum among the attendees.
        /// </summary>
        public bool HasQuorum()
        {
            var numCanVote = Attendees.Count(x => x.IsMember);
            return numCanVote >= Quorum;
        }

        private Meeting(DateTimeOffset startTime, string description, string location, int quorum)
        {
            Id = Guid.NewGuid();
            Description = description;
            StartTime = startTime;
            Attendees = new List<PersonRole>();
            Quorum = quorum;
            Location = location;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bylaws"></param>
        /// <param name="chair">Who called the meeting</param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Meeting NewInstance(DateTimeOffset startTime, string description, string location, int quorum)
        {
            return new Meeting(startTime, description, location, quorum);
        }

        public void AddAttendee(PersonRole attendee)
        {
            Attendees.Add(attendee);
        }

        public void RemoveAttendee(PersonRole attendee)
        {
            Attendees.Remove(attendee);
        }
    }
}