using System;
using System.Collections.Generic;
using System.Linq;
using Core.Actions;
using Core.Motions;

namespace Core.MeetingStates
{
    public class VotingState : IMeetingState
    {
        private readonly YesNoBallotBox _ballotBox;
        
        private MotionChain MotionChain { get; init; }
        
        /// <summary>
        /// Can be used to set group properties.
        /// </summary>
        private IGroupModifier GroupModifier { get; }
        
        public VotingState(IGroupModifier groupModifier, MotionChain motionChain)
        {
            _ballotBox = new YesNoBallotBox();
            GroupModifier = groupModifier;
            MotionChain = motionChain;
        }

        public IMeetingState TryHandleAction(MeetingAttendee actor, IAction action)
        {
            if (action is DeclareMotionPassed)
            {
                if (MotionChain.Current is GroupModifyingMotion groupModifyingMotion)
                {
                    groupModifyingMotion.TakeActionAsync();
                }

                if (MotionChain.Current is EndDebate)
                {
                    return new VotingState(GroupModifier, MotionChain.Pop());
                }
                
                return MotionChain.Previous.Any()
                    ? new DebateState(GroupModifier, MotionChain.Pop())
                    : new OpenFloorState(GroupModifier);
            }

            if (action is Vote vote)
            {
                _ballotBox.CastBallot(new YesNoBallot(actor.Person, vote.Type));
                return this;
            }

            throw new InvalidActionException();
        }

        public IEnumerable<Type> GetSupportedActions(MeetingAttendee actor)
        {
            return new[] { typeof(DeclareMotionPassed) };
        }
        
        public IEnumerable<Type> GetSupportedMotions()
        {
            return Array.Empty<Type>();
        }

        public string GetDescription()
        {
            return
                $"Voting on \"{MotionChain.Current.GetText()}\". {_ballotBox.NumAye} in favor. {_ballotBox.NumNay} opposed. {_ballotBox.NumAbstain} abstaining.";
        }
    }
}