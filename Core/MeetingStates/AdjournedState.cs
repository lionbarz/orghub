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
            out IAction? resultingAction)
        {
            if (actor.IsChair && action is CallMeetingToOrder)
            {
                // TODO: Automatically put the agenda item?
                newState = new OpenFloorState(GroupModifier);
                resultingAction = null;
                return true;
            }

            newState = null;
            resultingAction = null;
            return false;
        }

        public IEnumerable<ActionType> GetSupportedActions(MeetingAttendee attendee)
        {
            return attendee.IsChair ? new[] { ActionType.CallMeetingToOrder } : Array.Empty<ActionType>();
        }

        public string GetDescription()
        {
            return "The group is adjourned.";
        }
    }
}