using System;

namespace Core
{
    public interface IGroupModifier
    {
        void SetChair(Person person);

        void AddResolution(string text);
    }
}