using System.Collections.Generic;
using Core.Actions;
using Core.Motions;

namespace Core.MeetingStates
{
    public class VotingState : IMeetingState
    {
        private readonly IMotion _motion;
        private readonly IGroupModifier _groupModifier;
        
        public VotingState(IMotion motion, IGroupModifier groupModifier)
        {
            _motion = motion;
            _groupModifier = groupModifier;
        }
        
        public bool TryHandleAction(MeetingAttendee actor, IAction action, out IMeetingState? newState, out bool replaceCurrentState,
            out IAction? resultingAction)
        {
            newState = null;
            resultingAction = null;
            replaceCurrentState = false;
            
            if (action is DeclareMotionPassed && actor.IsChair)
            {
                if (_motion is IGroupModifyingMotion actionableMotion)
                {
                    actionableMotion.TakeActionAsync(_groupModifier);
                }

                if (_motion is IActionTakingMotion actionTakingMotion)
                {
                    resultingAction = actionTakingMotion.GetAction();
                }

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
            return $"Voting on \"{_motion.GetText()}\"";
        }
    }
}