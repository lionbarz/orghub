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
            out bool replaceCurrentState,
            out IAction? resultingAction)
        {
            newState = null;
            resultingAction = null;
            replaceCurrentState = false;

            if (action is ExpireSpeakerTime)
            {
                return true;
            }

            if (actor.Equals(Speaker) && action is YieldTheFloor)
            {
                return true;
            }

            return false;
        }

        public IEnumerable<Type> GetSupportedActions(MeetingAttendee actor)
        {
            var actions = new LinkedList<Type>();
            actions.AddLast(typeof(ExpireSpeakerTime));

            if (actor.Equals(Speaker))
            {
                actions.AddLast(typeof(YieldTheFloor));
            }

            return actions;
        }
        
        public IEnumerable<Type> GetSupportedMotions()
        {
            return Array.Empty<Type>();
        }

        public string GetDescription()
        {
            return $"{Speaker.Person.Name} is speaking.";
        }
    }
}