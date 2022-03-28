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

        public bool TryHandleAction(MeetingAttendee actor, IAction action, out IMeetingState? newState,
            out bool replaceCurrentState,
            out IAction? resultingAction)
        {
            newState = null;
            resultingAction = null;
            replaceCurrentState = false;
            
            if (action is CallMeetingToOrder)
            {
                // TODO: Automatically put the agenda item?
                newState = new OpenFloorState(GroupModifier);
                return true;
            }

            return false;
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