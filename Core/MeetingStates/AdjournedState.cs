using System;
using System.Collections.Generic;
using Core.Actions;

namespace Core.MeetingStates
{
    public class AdjournedState : IMeetingState
    {
        private IGroupModifier GroupModifier { get; }

        public AdjournedState(IGroupModifier groupModifier)
        {
            GroupModifier = groupModifier;
        }

        public IMeetingState TryHandleAction(MeetingAttendee actor, IAction action)
        {
            if (action is CallMeetingToOrder)
            {
                return new OpenFloorState(GroupModifier);
            }

            throw new InvalidActionException();
        }

        public IEnumerable<Type> GetSupportedActions(MeetingAttendee actor)
        {
            return new[] { typeof(CallMeetingToOrder) };
        }
        
        public IEnumerable<Type> GetSupportedMotions()
        {
            return Array.Empty<Type>();
        }

        public string GetDescription()
        {
            return "Adjourned until the next meeting.";
        }
    }
}