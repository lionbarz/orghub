using System;

namespace Core
{
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
        public string Email { get; set; }
    }
}