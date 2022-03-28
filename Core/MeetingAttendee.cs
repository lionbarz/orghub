using System;

namespace Core
{
    public class MeetingAttendee
    {
        /// <summary>
        /// Who the person is.
        /// </summary>
        public Person Person { get; init; }
        
        /// <summary>
        /// Is this a voting member?
        /// </summary>
        public bool IsMember { get; set; }
        
        /// <summary>
        /// Whether this person is chairing the meeting.
        /// </summary>
        public bool IsChair { get; set; }

        public override bool Equals(object? obj) => this.Equals(obj as MeetingAttendee);

        public bool IsGuest => !IsMember && !IsChair;
        
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