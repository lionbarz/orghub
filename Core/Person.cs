using System;

namespace Core
{
    /// <summary>
    /// A person is someone registered in the system.
    /// They're not tied to a specific group.
    /// </summary>
    public class Person
    {
        public Person(): this(string.Empty) {
        }
        
        public Person(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
    }
}