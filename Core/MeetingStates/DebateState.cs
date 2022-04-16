using System;
using System.Collections.Generic;
using System.Linq;
using Core.Actions;
using Core.Motions;

namespace Core.MeetingStates
{
    public class DebateState : IMeetingState
    {
        private MotionChain MotionChain { get; init; }

        /// <summary>
        /// Can be used to set group properties.
        /// </summary>
        private IGroupModifier GroupModifier { get; }

        public DebateState(IGroupModifier groupModifier, MotionChain motionChain)
        {
            MotionChain = motionChain;
            GroupModifier = groupModifier;
        }

        public IMeetingState TryHandleAction(MeetingAttendee actor, IAction action)
        {
            if (action is MoveToAdjourn)
            {
                // Right now moves to adjourn are automatically approved unanimously.
                return new AdjournedState(GroupModifier);
            }

            if (action is Speak)
            {
                return new SpeakerHasFloorState(GroupModifier, actor, MotionChain);
            }

            if (action is Move { Motion: EndDebate } move)
            {
                return new MotionProposed(GroupModifier, actor.Person, MotionChain.Push(move.Motion));
            }

            if (action is DeclareEndDebate)
            {
                return new VotingState(GroupModifier, MotionChain);
            }

            throw new InvalidActionException();
        }

        public IEnumerable<Type> GetSupportedActions(MeetingAttendee actor)
        {
            return new[]
            {
                typeof(MoveToAdjourn), typeof(Speak), typeof(EndDebate), typeof(DeclareMotionPassed),
                typeof(DeclareEndDebate)
            };
        }
        
        public IEnumerable<Type> GetSupportedMotions()
        {
            return new[]
            {
                typeof(EndDebate)
            };
        }

        public string GetDescription()
        {
            return $"Floor is open to speakers to debate {MotionChain.Current.GetText()}.";
        }
    }
}