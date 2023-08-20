using System;

namespace Core
{
    public class MeetingAttendee
    {
        /// <summary>
        /// Who the person is.
        /// </summary>
        public Person Person { get; private init; }
        
        public AttendeeRole Roles { get; private init; }
        
        private MeetingAttendee(Person person)
        {
            Person = person;
        }

        public static MeetingAttendee AsGuest(Person person)
        {
            return new MeetingAttendee(person)
            {
                Roles = AttendeeRole.Guest
            };
        }

        public static MeetingAttendee AsMember(Person person)
        {
            return new MeetingAttendee(person)
            {
                Roles = AttendeeRole.Member
            };
        }

        public static MeetingAttendee AsChair(Person person)
        {
            return new MeetingAttendee(person)
            {
                // Chairs are always members (for now?)
                // because they are elected from membership.
                Roles = AttendeeRole.Chair | AttendeeRole.Member
            };
        }

        public override bool Equals(object? obj) => this.Equals(obj as MeetingAttendee);
        
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