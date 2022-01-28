using System;
using System.Collections.Generic;
using Core.Actions;

namespace Core.MeetingStates
{
    public class SpeakerHasFloorState : IMeetingState
    {
        // The speaker who has the floor.
        private MeetingAttendee Speaker { get; }
        
        // The amount of time the speaker has.
        private TimeSpan AllotedTime { get; }

        public SpeakerHasFloorState(MeetingAttendee speaker)
        {
            Speaker = speaker;
        }
        
        public bool TryHandleAction(MeetingAttendee actor, ActionType action, out IMeetingState? newState,
            out ActionType? resultingAction)
        {
            newState = null;
            resultingAction = null;
            
            if (actor.IsChair && action == ActionType.ExpireSpeakerTime)
            {
                return true;
            }

            if (actor == Speaker && action == ActionType.YieldTheFloor)
            {
                return true;
            }

            return false;
        }

        public IEnumerable<ActionType> GetSupportedActions()
        {
            return new[] { ActionType.YieldTheFloor, ActionType.ExpireSpeakerTime };
        }
    }
}