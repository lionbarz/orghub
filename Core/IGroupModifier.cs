﻿
namespace Core
{
    public interface IGroupModifier
    {
        // Gets the name of the group that this modifier
        // will be modifying. For sanity checks.
        string GetName();

        void SetChair(Person person);

        void AddResolution(string text);

        /// <summary>
        /// Sets the group's name.
        /// </summary>
        void SetName(string text);

        /// <summary>
        /// Hooray! A new member!
        /// </summary>
        void AddMember(Person member);

        /// <summary>
        /// Add something to the meeting minutes.
        /// In the future, this can be structured, with fields for
        /// agenda items, actors, etc. so they can be linked.
        /// </summary>
        void RecordMinute(string text);
    }
}