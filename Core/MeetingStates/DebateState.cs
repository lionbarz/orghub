using System;
using System.Collections.Generic;
using Core.Actions;
using Core.Motions;

namespace Core.MeetingStates
{
    public class DebateState : IMeetingState
    {
        /// <summary>
        /// The motion being debated.
        /// </summary>
        private IMotion Motion { get; }

        /// <summary>
        /// Can be used to set group properties.
        /// </summary>
        private IGroupModifier GroupModifier { get; }

        public DebateState(IMotion motion, IGroupModifier groupModifier)
        {
            Motion = motion;
            GroupModifier = groupModifier;
        }

        public bool TryHandleAction(MeetingAttendee actor, IAction action, out IMeetingState? newState,
            out bool replaceCurrentState, out IAction? resultingAction)
        {
            newState = null;
            resultingAction = null;
            replaceCurrentState = false;

            if (action is MoveToAdjourn)
            {
                // Right now moves to adjourn are automatically approved unanimously.
                resultingAction = new MoveToAdjourn();
                return true;
            }

            if (action is Speak)
            {
                newState = new SpeakerHasFloorState(actor);
                return true;
            }

            if (action is Move { Motion: EndDebate } move)
            {
                newState = new MotionProposed(actor.Person, move.Motion, GroupModifier);
                return true;
            }

            if (action is DeclareMotionPassed)
            {
                if (Motion is IGroupModifyingMotion actionableMotion)
                {
                    actionableMotion.TakeActionAsync(GroupModifier);
                }

                return true;
            }

            if (action is DeclareEndDebate)
            {
                newState = new VotingState(Motion, GroupModifier);
                replaceCurrentState = true;
                return true;
            }

            return false;
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
            return $"Debating \"{Motion.GetText()}\"";
        }
    }
}