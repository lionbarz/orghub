using System;
using System.Collections.Generic;
using System.Linq;
using Core.Actions;
using Core.MeetingStates;

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
        /// What states and actions have happened so far.
        /// </summary>
        public IList<string> Minutes { get; private init;  }

        /// <summary>
        /// Constructor.
        /// </summary>
        private Group()
        {
            Id = Guid.NewGuid();
            Members = new List<Person>();
            Meetings = new List<Meeting>();
            Resolutions = new List<string>();
            Minutes = new List<string>();
            
            // TODO: Is there a better way to do this than pass this?
            _stateManager = new StateManager(this);
        }

        public static Group NewInstance(Person creator)
        {
            var group = new Group
            {
                Chair = creator,
                Bylaws = Bylaws.Default(),
                Members = new List<Person>() { creator }
            };

            return group;
        }
        
        public static Group NewInstance(Person creator, IEnumerable<Person> members)
        {
            var group = new Group
            {
                Chair = creator,
                Bylaws = Bylaws.Default(),
                Members = new List<Person>() { creator }
            };
            
            foreach (var member in members)
            {
                group.AddMember(member);
            }

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

            group.Bylaws = Bylaws.Default();

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

        public bool IsMember(Guid personId)
        {
            return Members.Any(x => x.Id == personId);
        }

        // TODO: Return result.
        // TODO: Have a method that marks people as present and if Mass Meeting then they are members, and have this method take a person Id and look it up in members. But what about guests?
        public void TakeAction(Person actor, IAction action)
        {
            var attendee = CreateAttendee(actor);
            var minutes = _stateManager.Act(attendee, action);
            foreach (var minute in minutes)
            {
                Minutes.Add(minute);
            }
        }

        private static bool IsEnoughNoticeGiven(DateTimeOffset currentTime, DateTimeOffset meetingTime,
            TimeSpan requiredNotice)
        {
            return meetingTime - currentTime >= requiredNotice;
        }

        public string GetName()
        {
            return Bylaws.Name;
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

        public void AddMember(Person member)
        {
            Members.Add(member);
            Minutes.Add($"{member.Name} was added as a member.");
        }

        public IMeetingState GetState()
        {
            return _stateManager.State;
        }

        /// <summary>
        /// Get the actions available to a person.
        /// </summary>
        /// <param name="person">The person who wants to take actions.</param>
        public IEnumerable<Type> GetAvailableActions(Person person)
        {
            var state = GetState();
            var attendee = CreateAttendee(person);
            var actions = state.GetSupportedActions(attendee);
            var avail = new ActionAvailability();
            var filteredActions =
                actions.Where(x => avail.IsActionAvailableToPerson(attendee.IsMember, attendee.IsChair, x));
            return filteredActions;
        }
        
        public IEnumerable<Type> GetAvailableMotions(Person person)
        {
            var state = GetState();
            var attendee = CreateAttendee(person);
            var motions = state.GetSupportedMotions();
            var avail = new ActionAvailability();
            var filteredMotions =
                motions.Where(x => avail.IsActionAvailableToPerson(attendee.IsMember, attendee.IsChair, x));
            return filteredMotions;
        }
        
        private MeetingAttendee CreateAttendee(Person actor)
        {
            return new MeetingAttendee()
            {
                Person = actor,
                IsChair = actor == Chair,
                IsMember = IsMember(actor.Id)
            };
        }
    }
}