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
        
        public bool TryHandleAction(MeetingAttendee actor, IAction action, out IMeetingState? newState,
            out IAction? resultingAction)
        {
            newState = null;
            resultingAction = null;
            
            if (actor.IsChair && action is ExpireSpeakerTime)
            {
                return true;
            }

            if (actor == Speaker && action is YieldTheFloor)
            {
                return true;
            }

            return false;
        }

        public IEnumerable<ActionType> GetSupportedActions(MeetingAttendee attendee)
        {
            var actions = new List<ActionType>();

            if (attendee.IsChair)
            {
                actions.Add(ActionType.ExpireSpeakerTime);
            }

            if (attendee == Speaker)
            {
                actions.Add(ActionType.YieldTheFloor);
            }

            return actions;
        }

        public string GetDescription()
        {
            return $"{Speaker.Person.Name} is speaking.";
        }
    }
}