using System;

namespace Core
{
    public class PersonOutOfOrderException : Exception
    {
        public PersonOutOfOrderException(string message) : base(message)
        {
        }
    }
}