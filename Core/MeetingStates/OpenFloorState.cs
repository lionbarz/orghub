﻿using System.Collections.Generic;
using Core.Actions;

namespace Core.MeetingStates
{
    public class OpenFloorState : IMeetingState
    {
        public bool TryHandleAction(MeetingAttendee actor, IAction action, out IMeetingState? newState,
            out IAction? resultingAction)
        {
            newState = null;
            resultingAction = null;

            if (action is MoveToAdjourn)
            {
                // Right now moves to adjourn are automatically approved unanimously.
                return true;
            }

            if (action is Speak)
            {
                newState = new SpeakerHasFloorState(actor);
                return true;
            }

            if (action is Move moveAction)
            {
                // Right now automatically moves to debate.
                newState = new DebateState(moveAction.Motion);
                return true;
            }

            return false;
        }

        public IEnumerable<ActionType> GetSupportedActions(MeetingAttendee attendee)
        {
            return new[] { ActionType.MoveToAdjourn, ActionType.Speak };
        }

        public string GetDescription()
        {
            return "The floor is open for suggestions and speakers.";
        }
    }
}