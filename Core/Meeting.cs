using System;
using System.Collections.Generic;
using System.Linq;
using Core.Meetings;
using Core.MeetingStates;
using Core.Motions;

namespace Core
{
    /// <summary>
    /// A meeting that pertains to a certain group (organization).
    /// A meeting is a bunch of people who get together to act
    /// in the name of an organization.
    /// (Forget mass meetings for now. Later they can get together
    /// to create a group but for now it's only to act on behalf
    /// of a group.)
    /// This class tracks who is in attendance and assigns them
    /// a role, using the group to determine it.
    /// It also determines quorum using the attendance information.
    /// It uses the StateManager to track the state of the meeting.
    /// </summary>
    public class Meeting : IMinuteRecorder
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
        public ICollection<MeetingAttendee> Attendees { get; private set; }

        /// <summary>
        /// The state of this group (adjourned, voting, etc).
        /// This handles actions.
        /// </summary>
        public StateManager State { get; private init; }
        
        /// <summary>
        /// The group that the people are gathering to act on its behalf.
        /// </summary>
        private Group Group { get; init; }

        /// <summary>
        /// The agenda that this meeting will follow.
        /// It will go through the list of all the states in the agenda until they run out,
        /// at which point it goes to the open floor.
        /// </summary>
        public MeetingAgenda Agenda { get; init; }
        
        /// <summary>
        /// What states and actions have happened so far.
        /// </summary>
        public IList<MeetingMinute> Minutes { get; private init;  }

        /// <summary>
        /// Whether there is a quorum among the attendees.
        /// </summary>
        public bool HasQuorum()
        {
            var numCanVote = Attendees.Count(x => x.Roles.HasFlag(AttendeeRole.Member));
            var numberRequired = Group.Bylaws.MeetingQuorum.GetQuorumNumber(Group.Members.Count);   
            return numCanVote >= numberRequired;
        }

        private Meeting(Group group, DateTimeOffset startTime, string description, string location, MeetingAgenda agenda)
        {
            Id = Guid.NewGuid();
            Description = description;
            StartTime = startTime;
            Attendees = new HashSet<MeetingAttendee>();
            Location = location;
            Agenda = agenda;
            State = new StateManager(group, Agenda, this);
            Group = group;
            Minutes = new List<MeetingMinute>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bylaws"></param>
        /// <param name="chair">Who called the meeting</param>
        /// <param name="group">The group that this meeting is acting on behalf of.</param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Meeting NewInstance(Group group, DateTimeOffset startTime, string description, string location, MeetingAgenda agenda)
        {
            return new Meeting(group, startTime, description, location, agenda);
        }

        public MeetingAttendee AddAttendee(Person person)
        {
            var attendee = CreateAttendee(person);
            Attendees.Add(attendee);
            return attendee;
        }
        
        private MeetingAttendee CreateAttendee(Person person)
        {
            if (person == Group.Chair)
            {
                return MeetingAttendee.AsChair(person);
            }

            if (Group.IsMember(person.Id))
            {
                return MeetingAttendee.AsMember(person);
            }

            return MeetingAttendee.AsGuest(person);
        }

        public void RemoveAttendee(MeetingAttendee attendee)
        {
            Attendees.Remove(attendee);
        }

        public MeetingAttendee GetAttendee(Guid personId)
        {
            var attendee = Attendees.FirstOrDefault(x => x.Person.Id == personId);

            if (attendee == null)
            {
                throw new Exception($"Person with ID {personId} is not in attendance of this meeting.");
            }
            return attendee;
        }

        public void RecordMinute(string text)
        {
            Minutes.Add(MeetingMinute.FromText(text));
        }
    }
}