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
        public DebateState(IMotion motion)
        {
            Motion = motion;
        }
        
        public bool TryHandleAction(MeetingAttendee actor, IAction action, out IMeetingState? newState, out IAction? resultingAction)
        {
            newState = null;
            resultingAction = null;

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

            return false;
        }

        public IEnumerable<ActionType> GetSupportedActions(MeetingAttendee attendee)
        {
            return new[] { ActionType.MoveToAdjourn, ActionType.Speak };
        }

        public string GetDescription()
        {
            return $"Should we adopt: {Motion.GetText()}";
        }
    }
}