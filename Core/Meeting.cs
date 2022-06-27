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
        /// The ID of the group that is meeting.
        /// </summary>
        public Guid GroupId { get; private init; }

        /// <summary>
        /// When the meeting will start.
        /// </summary>
        public DateTimeOffset StartTime { get; private init; }

        /// <summary>
        /// The attendees at any point of the meeting.
        /// </summary>
        public ICollection<PersonRole> Attendees { get; private set; }
        
        /// <summary>
        /// The chair of the meeting. Usually the person who called the meeting.
        /// </summary>
        public Person Chair { get; private set; }

        /// <summary>
        /// How many members need to be present in order
        /// for business to be legally conducted.
        /// Zero means there is no minimum, or that the
        /// quorum is the number present at the time.
        /// </summary>
        public int Quorum { get; set; }

        /// <summary>
        /// The state of the meeting. This handles actions.
        /// </summary>
        public StateManager State { get; private init; }

        /// <summary>
        /// Whether there is a quorum among the attendees.
        /// </summary>
        public bool HasQuorum()
        {
            var numCanVote = Attendees.Count(x => x.IsMember);
            return numCanVote >= Quorum;
        }

        private Meeting(Guid groupId, IGroupModifier groupModifier, Person chair, DateTimeOffset startTime, int quorum)
        {
            Id = Guid.NewGuid();
            StartTime = startTime;
            Attendees = new List<PersonRole>();
            Chair = chair;
            Quorum = quorum;
            State = new StateManager(groupModifier);
            GroupId = groupId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bylaws"></param>
        /// <param name="chair">Who called the meeting</param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Meeting NewInstance(Guid groupId, IGroupModifier groupModifier, Person chair,
            DateTimeOffset startTime, int quorum)
        {
            return new Meeting(groupId, groupModifier, chair, startTime, quorum);
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