using System;
using System.Text;
using Core.Motions;

namespace Core.MeetingStates
{
    public class SpeakerHasFloorState : MeetingStateBase
    {
        // The speaker who has the floor.
        private Person Speaker { get; }
        
        // An optional motion. The speaker could be debating
        // a motion or just speaking.
        private MotionChain? MotionChain { get; }

        // The amount of time the speaker has.
        private TimeSpan AllotedTime { get; }
        
        // Need it to pass to other states.
        private IGroupModifier GroupModifier { get; }

        public SpeakerHasFloorState(IGroupModifier groupModifier, Person speaker)
        {
            Speaker = speaker;
            GroupModifier = groupModifier;
        }
        
        public SpeakerHasFloorState(IGroupModifier groupModifier, Person speaker, MotionChain motionChain)
        {
            Speaker = speaker;
            MotionChain = motionChain;
            GroupModifier = groupModifier;
        }

        public override IMeetingState CallMeetingToOrder(PersonRole actor)
        {
            throw new PersonOutOfOrderException("Meeting already in order.");
        }

        public override IMeetingState DeclareTimeExpired(PersonRole actor)
        {
            if (!CanDeclareTimeExpired(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }
            
            GroupModifier.RecordMinute($"{actor.Person.Name}'s time on the floor is expired.");
            return MotionChain == null
                ? OpenFloorState.InstanceOf(GroupModifier)
                : new DebateState(GroupModifier, MotionChain);
        }

        public override IMeetingState MoveMainMotion(PersonRole actor, IMainMotion motion)
        {
            throw new PersonOutOfOrderException("Can't move a motion while someone is speaking.");
        }

        public override IMeetingState MoveSubsidiaryMotion(PersonRole actor, ISubsidiaryMotion motion)
        {
            throw new PersonOutOfOrderException("Can't move a motion while someone is speaking.");
        }

        public override IMeetingState Second(PersonRole actor)
        {
            throw new PersonOutOfOrderException("There is no motion to second.");
        }

        public override IMeetingState Speak(PersonRole actor)
        {
            throw new PersonOutOfOrderException("Can't speak while someone is speaking.");
        }

        public override IMeetingState Vote(PersonRole actor, VoteType type)
        {
            throw new PersonOutOfOrderException("There is no vote in progress.");
        }

        public override IMeetingState Yield(PersonRole actor)
        {
            GroupModifier.RecordMinute($"{actor.Person.Name} yields the floor.");
            
            return MotionChain == null
                ? OpenFloorState.InstanceOf(GroupModifier)
                : new DebateState(GroupModifier, MotionChain);
        }

        public override IMeetingState MoveToAdjourn(PersonRole actor)
        {
            throw new PersonOutOfOrderException("Can't move to adjourn while someone is speaking.");
        }

        public override string GetDescription()
        {
            if (MotionChain != null)
            {
                return $"{Speaker.Name} is debating the motion {MotionChain.Current.GetText()}.";
            }

            return $"{Speaker.Name} is speaking freely.";
        }

        protected override bool CanMoveToAdjourn(PersonRole actor, out string explanation)
        {
            explanation = "Can't move to adjourn while someone is speaking.";
            return false;
        }

        protected override bool CanCallToOrder(PersonRole actor, out string explanation)
        {
            explanation = "The meeting is already in order.";
            return false;
        }

        protected override bool CanDeclareTimeExpired(PersonRole actor, out string explanation)
        {
            // TODO: Use a timer and only allow if it's really expired.
            
            if (!actor.IsChair)
            {
                explanation = "Only the chair can declare the time as expired.";
                return false;
            }

            explanation = "The chair can declare the time as expired.";
            return true;
        }

        protected override bool CanSecond(PersonRole actor, out string explanation)
        {
            explanation = "There is no motion to second.";
            return false;
        }

        protected override bool CanSpeak(PersonRole actor, out string explanation)
        {
            explanation = "Can't speak while someone is speaking.";
            return false;
        }

        protected override bool CanMoveMainMotion(PersonRole actor, out string explanation)
        {
            explanation = "Can't move a motion while someone is speaking.";
            return false;
        }

        protected override bool CanMoveSubsidiaryMotion(PersonRole actor, out string explanation)
        {
            explanation = "Can't move a motion while someone is speaking.";
            return false;
        }

        protected override bool CanVote(PersonRole actor, out string explanation)
        {
            explanation = "There is no vote in progress.";
            return false;
        }

        protected override bool CanYield(PersonRole actor, out string explanation)
        {
            if (!actor.Person.Equals(Speaker))
            {
                explanation = $"Only {actor.Person.Name} can yield because they are the ones speaking.";
                return false;
            }

            explanation = $"{actor.Person.Name} has the floor and can yield it.";
            return true;
        }
    }
}