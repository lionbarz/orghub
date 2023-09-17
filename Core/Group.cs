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
            
            // TODO: This is always true for now.
            IsMassMeeting = true;
        }

        /// <summary>
        /// Create a group using default bylaws.
        /// </summary>
        public static Group NewInstance(Person creator, string name, string mission)
        {
            var group = new Group
            {
                Chair = creator,
                Bylaws = Bylaws.Default(name, mission),
                Members = new List<Person>() { creator }
            };

            return group;
        }
        
        public static Group NewInstance(Person creator, string name, string mission, IEnumerable<Person> members)
        {
            var group = NewInstance(creator, name, mission);
            
            foreach (var member in members)
            {
                group.AddMember(member);
            }

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
            // TODO: Record somewhere.
            //Minutes.Add(MeetingMinute.FromText($"{person.Name} is now the chair."));
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
            // TODO: Record somewhere.
            //Minutes.Add(MeetingMinute.FromText($"{member.Name} is added as a member."));
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