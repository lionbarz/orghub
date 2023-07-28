using System;

namespace Core
{
    public class MeetingAttendee
    {
        private MeetingAttendee(Person person)
        {
            Person = person;
        }

        public static MeetingAttendee AsGuest(Person person)
        {
            return new MeetingAttendee(person)
            {
                // TODO: Replace this with a Role variable.
                IsMember = false,
                IsChair = false
            };
        }

        public static MeetingAttendee AsMember(Person person)
        {
            return new MeetingAttendee(person)
            {
                IsMember = true,
                IsChair = false
            };
        }

        public static MeetingAttendee AsChair(Person person)
        {
            return new MeetingAttendee(person)
            {
                IsMember = true,
                IsChair = true
            };
        }

        /// <summary>
        /// Who the person is.
        /// </summary>
        public Person Person { get; private init; }

        /// <summary>
        /// Is this a voting member?
        /// </summary>
        public bool IsMember { get; private set; }

        /// <summary>
        /// Whether this person is chairing the meeting.
        /// </summary>
        public bool IsChair { get; private set; }

        public override bool Equals(object? obj) => this.Equals(obj as MeetingAttendee);

        public bool IsGuest => !IsMember && !IsChair;

        public string Title
        {
            get
            {
                if (IsChair)
                {
                    return "Chair";
                }

                return IsMember ? "Member" : "Guest";
            }
        }
        
        public bool Equals(MeetingAttendee? obj)
        {
            if (obj is null)
            {
                return false;
            }
            
            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }
            
            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            return Person.Id == obj.Person.Id;
        }
        
        public override int GetHashCode() => Person.Id.GetHashCode();
    }
}