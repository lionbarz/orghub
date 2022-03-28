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

        public IEnumerable<string> Act(MeetingAttendee actor, IAction action)
        {
            var minutes = new LinkedList<string>();
            IMeetingState currentState = States.Last();

            minutes.AddLast($"{action.RecordEntry(actor.Person)}");
            Console.WriteLine($"State: {currentState.GetDescription()}");
            Console.WriteLine($"Action: {action.RecordEntry(actor.Person)}");

            if (action is MoveToAdjourn)
            {
                // Right now we immediately adjourn as soon as anyone suggests it.
                while (States.Last() is not AdjournedState)
                {
                    States.RemoveLast();
                }

                return minutes;
            }

            VerifyPermission(action, actor);

            if (currentState.TryHandleAction(actor, action, out IMeetingState? newState, out bool replaceCurrentState,
                    out IAction? resultingAction))
            {
                if (newState != null)
                {
                    if (replaceCurrentState)
                    {
                        States.RemoveLast();
                    }
                    
                    States.AddLast(newState);
                    return minutes;
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

            return minutes;
        }

        public IMeetingState GetMeetingState()
        {
            return States.Last();
        }

        private void VerifyPermission(IAction action, MeetingAttendee actor)
        {
            var avail = new ActionAvailability();
            var hasPermission = avail.IsActionAvailableToPerson(actor.IsMember, actor.IsChair, action.GetType());

            if (!hasPermission)
            {
                // TODO: Make a new exception type.
                // TODO: Add to exception string what the person is (guest, chair)
                throw new Exception(
                    $"{actor.Person.Name} cannot take action {action.ToString()} because it is only available to TODO");
            }
        }
    }
}