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
        private IMotion _motion;
        private Person _mover;
        private IGroupModifier _groupModifier;
        
        public MotionProposed(Person mover, IMotion motion, IGroupModifier groupModifier)
        {
            _motion = motion;
            _mover = mover;
            _groupModifier = groupModifier;
        }

        public bool TryHandleAction(MeetingAttendee actor, IAction action, out IMeetingState? newState,
            out bool replaceCurrentState,
            out IAction? resultingAction)
        {
            newState = null;
            resultingAction = null;
            replaceCurrentState = false;

            if (actor.IsMember && action is SecondMotion)
            {
                if (_motion is EndDebate)
                {
                    // Motion to end debate is not debated.
                    // TODO: Make "isDebatable" a property of a motion.
                    newState = new VotingState(_motion, _groupModifier);
                }
                else
                {
                    newState = new DebateState(_motion, _groupModifier);
                }

                replaceCurrentState = true;
                return true;
            }

            return false;
        }

        public IEnumerable<ActionType> GetSupportedActions(MeetingAttendee attendee)
        {
            throw new System.NotImplementedException();
        }

        public string GetDescription()
        {
            return $"{_mover.Name} proposed \"{_motion.GetText()}\".";
        }
    }
}