using System;
using System.Collections.Generic;
using Core.Actions;

namespace Core.MeetingStates
{
    public class AdjournedState : IMeetingState
    {
        public bool TryHandleAction(MeetingAttendee actor, IAction action, out IMeetingState? newState,
            out IAction? resultingAction)
        {
            if (actor.IsChair && action is CallMeetingToOrder)
            {
                // TODO: Automatically put the agenda item?
                newState = new OpenFloorState();
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
            return "The meeting isn't in session, meaning it ended or hasn't started yet.";
        }
    }
}