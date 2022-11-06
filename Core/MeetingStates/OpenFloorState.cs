using System;
using Core.Motions;

namespace Core.MeetingStates
{
    /// <summary>
    /// This is the state when the meeting has no business on the floor
    /// and is looking for new business.
    /// </summary>
    public class OpenFloorState : MeetingStateBase
    {
        private IGroupModifier GroupModifier { get; }

        private OpenFloorState(IGroupModifier groupModifier)
        {
            GroupModifier = groupModifier;
        }

        public static OpenFloorState InstanceOf(IGroupModifier groupModifier)
        {
            return new OpenFloorState(groupModifier);
        }

        public override IMeetingState CallMeetingToOrder(PersonRole actor)
        {
            if (!CanCallToOrder(actor, out var explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }

            throw new Exception("Bad state. Shouldn't be here.");
        }

        public override IMeetingState DeclareTimeExpired(PersonRole actor)
        {
            throw new PersonOutOfOrderException("There is nothing being timed.");
        }

        public override IMeetingState MoveSubsidiaryMotion(PersonRole actor, ISubsidiaryMotion motion)
        {
            throw new PersonOutOfOrderException("Cannot move a subsidiary motion when there is no main motion.");
        }

        public override IMeetingState Second(PersonRole actor)
        {
            throw new PersonOutOfOrderException("There is no motion to second.");
        }

        public override IMeetingState MoveToAdjourn(PersonRole actor)
        {
            if (!CanMoveToAdjourn(actor, out string? error))
            {
                throw new PersonOutOfOrderException(error);
            }
            
            GroupModifier.RecordMinute($"{actor.Person.Name} moves to adjourn the meeting.");
            var motion = new Adjourn(actor.Person);
            var motionChain = MotionChain.FromMotion(motion);
            return new MotionProposed(GroupModifier, actor.Person, motionChain);
        }

        public override IMeetingState MoveMainMotion(PersonRole actor, IMainMotion motion)
        {
            if (!CanMoveMainMotion(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }
            
            GroupModifier.RecordMinute($"{actor.Person.Name} moves {motion.GetText()}.");
            return new MotionProposed(GroupModifier, actor.Person, MotionChain.FromMotion(motion));
        }

        public override IMeetingState Speak(PersonRole actor)
        {
            if (!CanSpeak(actor, out string? error))
            {
                throw new PersonOutOfOrderException(error);
            }
            
            GroupModifier.RecordMinute($"{actor.Person.Name} takes the floor.");
            return new SpeakerHasFloorState(GroupModifier, actor.Person);
        }

        public override IMeetingState Vote(PersonRole actor, VoteType type)
        {
            throw new PersonOutOfOrderException("There is no vote in progress.");
        }

        public override IMeetingState Yield(PersonRole actor)
        {
            throw new PersonOutOfOrderException($"{actor.Person.Name} does not have the floor.");
        }

        public override string GetDescription()
        {
            return "The floor is open.";
        }
        
        protected override bool CanMoveToAdjourn(PersonRole actor, out string explanation)
        {
            if (actor.IsGuest)
            {
                explanation = $"{actor.Person.Name} is a guest but only members and the chair can move to adjourn.";
                return false;
            }

            explanation = $"Members and chairs can move to adjourn when the floor is open.";
            return true;
        }
        
        protected override bool CanCallToOrder(PersonRole actor, out string explanation)
        {
            explanation = $"{actor.Person.Name} cannot call the meeting to order because the meeting is already in order.";
            return false;
        }

        protected override bool CanDeclareTimeExpired(PersonRole actor, out string explanation)
        {
            explanation = "There is nothing being timed.";
            return false;
        }

        protected override bool CanSecond(PersonRole actor, out string explanation)
        {
            explanation = "There is no motion to second.";
            return false;
        }

        protected override bool CanSpeak(PersonRole actor, out string explanation)
        {
            explanation = "Anybody can speak when the floor is open.";
            return true;
        }

        protected override bool CanMoveMainMotion(PersonRole actor, out string explanation)
        {
            if (actor.IsGuest)
            {
                explanation = $"{actor.Person.Name} is a guest but only members and the chair can move to primary motion.";
                return false;
            }

            explanation = $"Members and chairs can move a primary motion when the floor is open.";
            return true;
        }

        protected override bool CanMoveSubsidiaryMotion(PersonRole actor, out string explanation)
        {
            explanation = $"There must be a main motion for a subsidiary motion to be moved.";
            return false;
        }

        protected override bool CanVote(PersonRole actor, out string explanation)
        {
            explanation = "There is no vote in progress.";
            return false;
        }

        protected override bool CanYield(PersonRole actor, out string explanation)
        {
            explanation = $"{actor.Person.Name} does not have the floor.";
            return false;
        }
    }
}