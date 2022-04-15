using System;
using System.Collections.Generic;
using Core.Actions;
using Core.Motions;

namespace Core.MeetingStates
{
    public class OpenFloorState : IMeetingState
    {
        private IGroupModifier GroupModifier { get; }

        public OpenFloorState(IGroupModifier groupModifier)
        {
            GroupModifier = groupModifier;
        }

        public bool TryHandleAction(MeetingAttendee actor, IAction action, out IMeetingState? newState,
            out bool replaceCurrentState,
            out IAction? resultingAction)
        {
            newState = null;
            resultingAction = null;
            replaceCurrentState = false;

            if (action is MoveToAdjourn)
            {
                // Right now moves to adjourn are automatically approved unanimously.
                return true;
            }

            if (action is Speak)
            {
                newState = new SpeakerHasFloorState(actor);
                return true;
            }

            if (action is Move moveAction)
            {
                // Right now automatically moves to debate.
                newState = new DebateState(moveAction.Motion, GroupModifier);
                return true;
            }

            return false;
        }

        public IEnumerable<Type> GetSupportedActions(MeetingAttendee actor)
        {
            return new[]
            {
                typeof(MoveToAdjourn), typeof(Speak)
            };
        }
        
        public IEnumerable<Type> GetSupportedMotions()
        {
            return new[]
            {
                typeof(Resolve), typeof(ElectChair), typeof(ChangeOrgName), typeof(GrantMembership)
            };
        }

        public string GetDescription()
        {
            return "The floor is open for suggestions and speakers.";
        }
    }
}