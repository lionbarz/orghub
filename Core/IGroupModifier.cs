using System;

namespace Core
{
    public interface IGroupModifier
    {
        void SetChair(Person person);

        void AddResolution(string text);

        /// <summary>
        /// Sets the group's name.
        /// </summary>
        void SetName(string text);
    }
}