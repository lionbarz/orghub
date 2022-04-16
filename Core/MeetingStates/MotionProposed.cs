using System;
using System.Collections.Generic;
using Core.Actions;
using Core.Motions;

namespace Core.MeetingStates
{
    /// <summary>
    /// A motion has been proposed but not seconded yet.
    /// </summary>
    public class MotionProposed : IMeetingState
    {
        private MotionChain MotionChain { get; }
        private Person _mover;
        private IGroupModifier GroupModifier { get; }
        
        public MotionProposed(IGroupModifier groupModifier, Person mover, MotionChain motionChain)
        {
            _mover = mover;
            GroupModifier = groupModifier;
            MotionChain = motionChain;
        }

        public IMeetingState TryHandleAction(MeetingAttendee actor, IAction action)
        {
            if (action is SecondMotion)
            {
                if (MotionChain.Current is EndDebate)
                {
                    // Motion to end debate is not debated.
                    // TODO: Make "isDebatable" a property of a motion.
                    return new VotingState(GroupModifier, MotionChain);
                }
                
                return new DebateState(GroupModifier, MotionChain);
            }
            
            // TODO: If there is no second, go back to main motion or open floor.

            throw new InvalidActionException();
        }

        public IEnumerable<Type> GetSupportedActions(MeetingAttendee actor)
        {
            return new[]
            {
                typeof(SecondMotion)
            };
        }
        
        public IEnumerable<Type> GetSupportedMotions()
        {
            return Array.Empty<Type>();
        }

        public string GetDescription()
        {
            return $"{_mover.Name} proposed \"{MotionChain.Current.GetText()}\".";
        }
    }
}