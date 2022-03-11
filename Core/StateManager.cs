using System;
using System.Collections.Generic;
using System.Linq;
using Core.Actions;
using Core.MeetingStates;

namespace Core
{
    public class StateManager
    {
        public StateManager(IGroupModifier groupModifier)
        {
            States = new LinkedList<IMeetingState>();
            States.AddLast(new AdjournedState(groupModifier));
        }

        /// <summary>
        /// The stack of meeting states. The last is the latest.
        /// It always starts with an adjourned state.
        /// </summary>
        private LinkedList<IMeetingState> States { get; }
        
        public void Act(MeetingAttendee actor, IAction action)
        {
            IMeetingState currentState = States.Last();
            
            Console.WriteLine($"State: {currentState.GetDescription()}");
            Console.WriteLine($"Action: {action.Describe(actor.Person)}");

            if (action is MoveToAdjourn)
            {
                // Right now we immediately adjourn as soon as anyone suggests it.
                while (States.Last() is not AdjournedState)
                {
                    States.RemoveLast();
                }
                return;
            }
            
            if (currentState.TryHandleAction(actor, action, out IMeetingState? newState,
                    out IAction? resultingAction))
            {
                if (newState != null)
                {
                    States.AddLast(newState);
                    return;
                }
                
                // The current state is done. Go back to last state on stack.
                States.RemoveLast();
                
                if (resultingAction != null)
                {
                    Act(actor, resultingAction);
                }
            }
            else
            {
                throw new ArgumentException(
                    $"{actor.Person.Name} can't take action {action} in meeting state {currentState}");
            }
        }

        public IMeetingState GetMeetingState()
        {
            return States.Last();
        }
    }
}