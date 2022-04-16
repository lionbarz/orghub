using System;
using System.Collections.Generic;
using System.Text;
using Core.Actions;
using Core.Motions;

namespace Core.MeetingStates
{
    public class SpeakerHasFloorState : IMeetingState
    {
        // The speaker who has the floor.
        private MeetingAttendee Speaker { get; }
        
        // An optional motion. The speaker could be debating
        // a motion or just speaking.
        private MotionChain? MotionChain { get; }

        // The amount of time the speaker has.
        private TimeSpan AllotedTime { get; }
        
        // Need it to pass to other states.
        private IGroupModifier GroupModifier { get; }

        public SpeakerHasFloorState(IGroupModifier groupModifier, MeetingAttendee speaker)
        {
            Speaker = speaker;
            GroupModifier = groupModifier;
        }
        
        public SpeakerHasFloorState(IGroupModifier groupModifier, MeetingAttendee speaker, MotionChain motionChain)
        {
            Speaker = speaker;
            MotionChain = motionChain;
            GroupModifier = groupModifier;
        }

        public IMeetingState TryHandleAction(MeetingAttendee actor, IAction action)
        {
            if (action is ExpireSpeakerTime || (actor.Equals(Speaker) && action is YieldTheFloor))
            {
                return (MotionChain == null)
                    ? new OpenFloorState(GroupModifier)
                    : new DebateState(GroupModifier, MotionChain);
            }

            throw new InvalidActionException();
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
            var sb = new StringBuilder();
            sb.Append($"{Speaker.Person.Name} is speaking");

            if (MotionChain != null)
            {
                sb.Append($" about the motion: {MotionChain.Current.GetText()}");
            }
            
            return sb.ToString();
        }
    }
}