using System;
using Core.Motions;

namespace Core.MeetingStates
{
    public class AdjournedState : MeetingStateBase
    {
        private IGroupModifier GroupModifier { get; }

        public AdjournedState(IGroupModifier groupModifier)
        {
            GroupModifier = groupModifier;
        }

        public override IMeetingState CallMeetingToOrder(PersonRole actor)
        {
            if (!CanCallToOrder(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }
            
            return OpenFloorState.InstanceOf(GroupModifier);
        }

        public override IMeetingState DeclareTimeExpired(PersonRole actor)
        {
            throw new PersonOutOfOrderException("Meeting is not in order");
        }

        public override IMeetingState MoveSubsidiaryMotion(PersonRole actor, ISubsidiaryMotion motion)
        {
            throw new PersonOutOfOrderException("Meeting is not in order");
        }

        public override IMeetingState Second(PersonRole actor)
        {
            throw new PersonOutOfOrderException("Meeting is not in order");
        }

        public override IMeetingState MoveToAdjournUntil(PersonRole actor, DateTimeOffset untilTime)
        {
            throw new PersonOutOfOrderException(
                $"{actor.Person.Name} cannot move to adjourn while the meeting is adjourned.");
        }

        public override IMeetingState MoveMainMotion(PersonRole actor, IMainMotion motion)
        {
            throw new PersonOutOfOrderException(
                $"{actor.Person.Name} cannot move a motion while the meeting is adjourned.");
        }

        public override IMeetingState Speak(PersonRole actorRole)
        {
            if (!CanSpeak(actorRole, out string? error))
            {
                throw new PersonOutOfOrderException(error);
            }
            
            return new SpeakerHasFloorState(GroupModifier, actorRole.Person);
        }

        public override IMeetingState Vote(PersonRole actor, VoteType type)
        {
            throw new PersonOutOfOrderException("There is no vote in progress.");
        }

        public override IMeetingState Yield(PersonRole actor)
        {
            throw new PersonOutOfOrderException("Meeting is not in order");
        }

        /// <summary>
        /// Whether or not a person can move to adjourn.
        /// </summary>
        protected override bool CanMoveToAdjournUntil(PersonRole actor, out string explanation)
        {
            explanation = $"Cannot move to adjourn while the meeting is adjourned.";
            return false;
        }
        
        /// <summary>
        /// Whether or not a person can move to adjourn.
        /// </summary>
        protected override bool CanCallToOrder(PersonRole actor, out string explanation)
        {
            if (!actor.IsChair)
            {
                explanation = $"{actor.Person.Name} is not the chair and only the chair can call the meeting to order.";
                return false;
            }

            explanation = $"The chair can call the meeting the order.";
            return true;
        }

        protected override bool CanDeclareTimeExpired(PersonRole actor, out string explanation)
        {
            explanation = "Meeting is adjourned.";
            return false;
        }

        protected override bool CanSecond(PersonRole actor, out string explanation)
        {
            explanation = "Meeting is adjourned.";
            return false;
        }

        /// <summary>
        /// Whether or not a person can speak.
        /// </summary>
        protected override bool CanSpeak(PersonRole actor, out string explanation)
        {
            explanation = "Nobody can speak while the meeting is adjourned.";
            return false;
        }

        protected override bool CanMoveMainMotion(PersonRole actor, out string explanation)
        {
            explanation = "Nobody can move a primary motion while the meeting is adjourned.";
            return false;
        }

        protected override bool CanMoveSubsidiaryMotion(PersonRole actor, out string explanation)
        {
            explanation = "Meeting is adjourned.";
            return false;
        }

        protected override bool CanVote(PersonRole actor, out string explanation)
        {
            explanation = "There is no vote in progress.";
            return false;
        }

        protected override bool CanYield(PersonRole actor, out string explanation)
        {
            explanation = "Meeting is adjourned.";
            return false;
        }

        public override string GetDescription()
        {
            return "The group is adjourned.";
        }
    }
}