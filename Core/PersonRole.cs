using System;

namespace Core
{
    public class PersonRole
    {
        private PersonRole(Person person)
        {
            Person = person;
        }
        
        public static PersonRole AsGuest(Person person)
        {
            return new PersonRole(person)
            {
                IsMember = false,
                IsChair = false
            };
        }
        
        public static PersonRole AsMember(Person person)
        {
            return new PersonRole(person)
            {
                IsMember = true,
                IsChair = false
            };
        }

        public static PersonRole AsChair(Person person)
        {
            return new PersonRole(person)
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

        public override bool Equals(object? obj) => this.Equals(obj as PersonRole);

        public bool IsGuest => !IsMember && !IsChair;
        
        public bool Equals(PersonRole? obj)
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