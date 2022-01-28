using System.Collections.Generic;
using Core.Actions;

namespace Core.MeetingStates
{
    public class AdjournedState : IMeetingState
    {
        public bool TryHandleAction(MeetingAttendee actor, ActionType action, out IMeetingState? newState,
            out ActionType? resultingAction)
        {
            if (actor.IsChair && action == ActionType.CallMeetingToOrder)
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

        public IEnumerable<ActionType> GetSupportedActions()
        {
            return new[] { ActionType.CallMeetingToOrder };
        }
    }
}