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

        public IMeetingState TryHandleAction(MeetingAttendee actor, IAction action)
        {
            if (action is MoveToAdjourn)
            {
                // Right now moves to adjourn are automatically approved unanimously.
                // TODO: Require a second and vote.
                return new AdjournedState(GroupModifier);
            }

            if (action is Speak)
            {
                return new SpeakerHasFloorState(GroupModifier, actor);
            }

            if (action is Move moveAction)
            {
                return new MotionProposed(GroupModifier, actor.Person, MotionChain.FromMotion(moveAction.Motion));
            }

            throw new InvalidActionException();
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
            return "The floor is open to suggestions and speakers.";
        }
    }
}