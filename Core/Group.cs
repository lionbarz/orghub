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
        /// The bylaws governing the group.
        /// </summary>
        public Bylaws Bylaws { get; private init; }
        
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
        /// Constructor.
        /// </summary>
        private Group()
        {
            Id = Guid.NewGuid();
            Bylaws = Bylaws.MassMeeting();
            Members = new List<Person>();
            Meetings = new List<Meeting>();
        }

        public static Group NewInstance(IEnumerable<Person> members)
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
        
        private static bool IsEnoughNoticeGiven(DateTimeOffset currentTime, DateTimeOffset meetingTime,
            TimeSpan requiredNotice)
        {
            return meetingTime - currentTime >= requiredNotice;
        }
    }
}
