using System;
using System.Collections.Generic;
using System.Linq;

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
        /// The currently set meeting for the group,
        /// either coming up or in progress. It becomes
        /// a past meeting when it is adjourned.
        /// </summary>
        public Meeting? CurrentMeeting { get; set; }
        
        /// <summary>
        /// Past meetings of this group.
        /// </summary>
        public ICollection<Meeting> PastMeetings { get; }
        
        /// <summary>
        /// All resolutions passed by this group.
        /// </summary>
        public ICollection<string> Resolutions { get; }

        /// <summary>
        /// The state of this group (adjourned, voting, etc).
        /// This handles actions.
        /// </summary>
        public StateManager State { get; private init; }
        
        /// <summary>
        /// What states and actions have happened so far.
        /// </summary>
        public IList<MeetingMinute> Minutes { get; private init;  }
        
        /// <summary>
        /// Whether this is an established group.
        /// TODO: Create another class that implements the interface for mass meetings.
        /// </summary>
        public bool IsMassMeeting { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        private Group()
        {
            Id = Guid.NewGuid();
            Members = new List<Person>();
            PastMeetings = new List<Meeting>();
            Resolutions = new List<string>();
            Minutes = new List<MeetingMinute>();
            
            // TODO: This is always true for now.
            IsMassMeeting = true;
            
            // TODO: Is there a better way to do this than pass this?
            State = new StateManager(this);
        }

        public static Group NewInstance(Person creator)
        {
            var group = new Group
            {
                Chair = creator,
                Bylaws = Bylaws.Default("New group"),
                Members = new List<Person>() { creator }
            };

            return group;
        }
        
        public static Group NewInstance(Person creator, IEnumerable<Person> members)
        {
            var group = new Group
            {
                Chair = creator,
                Bylaws = Bylaws.Default($"{creator.Name}'s new group"),
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

            group.Bylaws = Bylaws.Default("New group");

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

        public string GetName()
        {
            return Bylaws.Name;
        }

        public void SetChair(Person person)
        {
            Minutes.Add(MeetingMinute.FromText($"{person.Name} is now the chair."));
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
            if (Members.Contains(member)) return;
            
            Members.Add(member);
            Minutes.Add(MeetingMinute.FromText($"{member.Name} is added as a member."));
        }

        public void RecordMinute(string text)
        {
            Minutes.Add(MeetingMinute.FromText(text));
        }

        public PersonRole CreatePersonRole(Person person)
        {
            if (person == Chair)
            {
                return PersonRole.AsChair(person);
            }

            if (IsMember(person.Id))
            {
                return PersonRole.AsMember(person);
            }

            return PersonRole.AsGuest(person);
        }

        public void MarkAttendance(Person person)
        {
            if (IsMassMeeting)
            {
                AddMember(person);
            }
        }
    }
}