using System;
using System.Collections.Generic;
using System.Linq;
using Core.Actions;
using Core.MeetingStates;
using Core.Motions;

namespace Core
{
    /// <summary>
    /// A group is a collection of people. It can have a chair
    /// and take actions. It's always in some state, like
    /// adjourned or doing something. It can make decisions
    /// about itself, such as changing its properties (name,
    /// chair) or pass decisions. For a decision to be
    /// effective, it has to be made by a quorum.
    /// </summary>
    public class Group : IGroupModifier
    {
        /// <summary>
        /// Unique identifier for this group.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The bylaws governing the group.
        /// </summary>
        public Bylaws Bylaws { get; private set; }

        /// <summary>
        /// Current members of the organization.
        /// </summary>
        public ICollection<Person> Members { get; private init; }

        /// <summary>
        /// If true, then there isn't a strict membership.
        /// Everyone is a member.
        /// </summary>
        public bool IsMassMeeting { get; private set; }

        /// <summary>
        /// The person chairing the organization.
        /// </summary>
        public Person? Chair { get; private set; }

        /// <summary>
        /// Past and future meetings of this group.
        /// </summary>
        public ICollection<Meeting> Meetings { get; private init; }
        
        /// <summary>
        /// All resolutions passed by this group.
        /// </summary>
        public ICollection<string> Resolutions { get; private init; }

        /// <summary>
        /// Tracks the state of this group.
        /// </summary>
        private readonly StateManager _stateManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        private Group()
        {
            Id = Guid.NewGuid();
            Members = new List<Person>();
            Meetings = new List<Meeting>();
            Resolutions = new List<string>();
            
            // TODO: Is there a better way to do this than pass this?
            _stateManager = new StateManager(this);
        }

        public static Group MassMeeting(Person chair)
        {
            var group = new Group
            {
                IsMassMeeting = true,
                Chair = chair,
                Bylaws = Bylaws.MassMeeting()
            };
            return group;
        }

        public static Group WithMembership(IEnumerable<Person> members)
        {
            var membersArray = members as Person[] ?? members.ToArray();

            if (!membersArray.Any())
            {
                throw new ArgumentException("A group must have at least one member.");
            }

            var group = new Group();

            foreach (var member in membersArray)
            {
                group.Members.Add(member);
            }

            group.Bylaws = Bylaws.MassMeeting();

            return group;
        }

        public void SetChairperson(Person person)
        {
            if (!Members.Contains(person))
            {
                throw new ArgumentException($"{person.Name} is not a member of the group.");
            }

            Chair = person;
        }

        public Meeting CreateMeeting(DateTimeOffset startTime)
        {
            if (!IsEnoughNoticeGiven(DateTimeOffset.Now, startTime, Bylaws.MinimumMeetingNotice))
            {
                throw new ArgumentException($"{Bylaws.MinimumMeetingNotice} notice required.");
            }

            // TODO: This fixes the number in time. Need to pull in real time.
            int quorum = Bylaws.MeetingQuorum.GetQuorumNumber(Members.Count);

            var meeting = Meeting.NewInstance(Chair, startTime, quorum);
            Meetings.Add(meeting);
            return meeting;
        }

        public bool IsMember(Guid personId)
        {
            return IsMassMeeting || Members.Any(x => x.Id == personId);
        }

        // TODO: Return result.
        // TODO: Have a method that marks people as present and if Mass Meeting then they are members, and have this method take a person Id and look it up in members. But what about guests?
        public void TakeAction(Person actor, IAction action)
        {
            var attendee = new MeetingAttendee()
            {
                Person = actor,
                IsChair = actor == Chair,
                IsMember = IsMember(actor.Id)
            };

            _stateManager.Act(attendee, action);
        }

        private static bool IsEnoughNoticeGiven(DateTimeOffset currentTime, DateTimeOffset meetingTime,
            TimeSpan requiredNotice)
        {
            return meetingTime - currentTime >= requiredNotice;
        }

        public void SetChair(Person person)
        {
            Chair = person;
        }

        public void AddResolution(string text)
        {
            Resolutions.Add(text);
        }

        public void SetName(string text)
        {
            Bylaws.Name = text;
        }

        public IMeetingState GetState()
        {
            return _stateManager.GetMeetingState();
        }
    }
}