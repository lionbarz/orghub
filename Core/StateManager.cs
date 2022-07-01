using System;
using System.Collections.Generic;
using Core.MeetingStates;
using Core.Motions;

namespace Core
{
    /// <summary>
    /// Shuffles underlying state by passing the actions to an underlying state,
    /// and this behaves as a state itself. See the State pattern.
    /// </summary>
    public class StateManager : IMeetingState
    {
        /// <summary>
        /// The current state.
        /// </summary>
        private IMeetingState State { get; set; }
        
        /// <summary>
        /// Creates a StateManager with an initial Adjourned state.
        /// </summary>
        public StateManager(IGroupModifier groupModifier)
        {
            State = new AdjournedState(groupModifier);
        }

        private StateManager(IMeetingState state)
        {
            State = state;
        }

        public static StateManager StartingWithState(IMeetingState initialState)
        {
            return new StateManager(initialState);
        }

        public IMeetingState CallMeetingToOrder(PersonRole actor)
        {
            State = State.CallMeetingToOrder(actor);
            return State;
        }

        public IMeetingState DeclareTimeExpired(PersonRole actor)
        {
            State = State.DeclareTimeExpired(actor);
            return State;
        }

        public IMeetingState MoveMainMotion(PersonRole actor, IMainMotion motion)
        {
            State = State.MoveMainMotion(actor, motion);
            return State;
        }

        public IMeetingState MoveSubsidiaryMotion(PersonRole actor, ISubsidiaryMotion motion)
        {
            State = State.MoveSubsidiaryMotion(actor, motion);
            return State;
        }

        public IMeetingState MoveToAdjourn(PersonRole actor)
        {
            State = State.MoveToAdjourn(actor);
            return State;
        }

        public IMeetingState Second(PersonRole actor)
        {
            State = State.Second(actor);
            return State;
        }

        public IMeetingState Speak(PersonRole actor)
        {
            State = State.Speak(actor);
            return State;
        }

        public IMeetingState Vote(PersonRole actor, VoteType type)
        {
            State = State.Vote(actor, type);
            return State;
        }

        public IMeetingState Yield(PersonRole actor)
        {
            State = State.Yield(actor);
            return State;
        }

        public ICollection<ActionSupport> GetActionSupportForPerson(PersonRole actor)
        {
            return State.GetActionSupportForPerson(actor);
        }

        public string GetDescription()
        {
            return State.GetDescription();
        }
    }
}