using System.Collections.Generic;
using Core.Actions;

namespace Core.MeetingStates
{
    public class OpenFloorState : IMeetingState
    {
        public bool TryHandleAction(MeetingAttendee actor, ActionType action, out IMeetingState? newState,
            out ActionType? resultingAction)
        {
            newState = null;
            resultingAction = null;

            if (action == ActionType.MoveToAdjourn)
            {
                // Right now moves to adjourn are automatically approved unanimously.
                return true;
            }

            if (action == ActionType.Speak)
            {
                newState = new SpeakerHasFloorState(actor);
                return true;
            }

            return false;
        }

        public IEnumerable<ActionType> GetSupportedActions()
        {
            return new[] { ActionType.MoveToAdjourn, ActionType.Speak };
        }
    }
}