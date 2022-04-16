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
            State = new AdjournedState(groupModifier);
        }

        public IMeetingState State { get; private set; }

        public IEnumerable<string> Act(MeetingAttendee actor, IAction action)
        {
            var minutes = new LinkedList<string>();
            minutes.AddLast($"{action.RecordEntry(actor.Person)}");

            VerifyPermission(action, actor);

            try
            {
                State = State.TryHandleAction(actor, action);
            }
            catch (InvalidActionException ex)
            {
                throw new ArgumentException(
                    $"{actor.Person.Name} can't take action {action} in meeting state {State}");
            }

            return minutes;
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