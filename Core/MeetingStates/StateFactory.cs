using System;
using Core.Motions;

namespace Core.MeetingStates
{
    public class StateFactory
    {
        public IMeetingState Create(State state, IGroupModifier groupModifier, MotionChain? motionChain, Person mover)
        {
            if (state == State.Adjourned)
            {
                return new AdjournedState(groupModifier);
            }

            if (state == State.Debate)
            {
                return new DebateState(groupModifier, motionChain);
            }

            if (state == State.Voting)
            {
                return new VotingState(groupModifier, motionChain);
            }

            if (state == State.MotionProposed)
            {
                return new MotionProposed(groupModifier, mover, motionChain);
            }

            if (state == State.OpenFloor)
            {
                return OpenFloorState.InstanceOf(groupModifier);
            }

            if (state == State.SpeakerHasFloor)
            {
                return new SpeakerHasFloorState(groupModifier, mover);
            }
            
            throw new ArgumentException($"Don't know how to create state {state}.");
        }
    }
}