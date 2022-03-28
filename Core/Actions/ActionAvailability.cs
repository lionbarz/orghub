using System;
using System.Collections.Generic;
using Core.Motions;

namespace Core.Actions
{
    /// <summary>
    /// Who an action is available to.
    /// </summary>
    public class ActionAvailability
    {
        public Availability GetAvailability(Type actionType)
        {
            return _map[actionType];
        }

        public bool IsActionAvailableToPerson(bool isMember, bool isChair, Type actionType)
        {
            var availability = _map[actionType];
            var isGuest = !isChair && !isMember;
            return (isGuest && availability.IsAvailableToGuests) ||
                   (isMember && availability.IsAvailableToMembers) ||
                   (isChair && availability.IsAvailableToChairs);
        }

        private readonly Dictionary<Type, Availability> _map = new()
        {
            // Actions
            { typeof(CallMeetingToOrder), new Availability(false, false, true) },
            { typeof(DeclareEndDebate), new Availability(false, false, true) },
            { typeof(DeclareMotionPassed), new Availability(false, false, true) },
            { typeof(DeclareMotionSeconded), new Availability(false, false, true) },
            { typeof(ExpireSpeakerTime), new Availability(false, false, true) },
            { typeof(Move), new Availability(false, true, true) },
            { typeof(MoveToAdjourn), new Availability(false, true, true) },
            { typeof(SecondMotion), new Availability(false, true, true) },
            { typeof(Speak), new Availability(true, true, true) },
            { typeof(YieldTheFloor), new Availability(true, true, true) },

            // Motions
            { typeof(Adjourn), new Availability(false, true, true) },
            { typeof(ChangeOrgName), new Availability(false, true, true) },
            { typeof(ElectChair), new Availability(false, true, true) },
            { typeof(EndDebate), new Availability(false, true, true) },
            { typeof(Resolve), new Availability(false, true, true) },
        };
    }
}