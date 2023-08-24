using System;

namespace Core
{
    /// <summary>
    /// A person is someone registered in the system.
    /// They're not tied to a specific group.
    /// </summary>
    public class Person
    {
        // TODO: remove this
        public Person(): this(string.Empty, string.Empty) {
        }
        
        public Person(string name, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
        }
        
        public Guid Id { get; set; }
        
        /// <summary>
        /// Shadow accounts don't have a name specified.
        /// </summary>
        public string? Name { get; set; }
        public string Email { get; set; }
    }
}