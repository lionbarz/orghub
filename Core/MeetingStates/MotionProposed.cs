using System;
using System.Linq;
using Core.Motions;

namespace Core.MeetingStates
{
    /// <summary>
    /// A motion has been proposed but not seconded yet.
    /// </summary>
    public class MotionProposed : MeetingStateBase
    {
        private MotionChain MotionChain { get; }
        private Person Mover { get; }
        private IGroupModifier GroupModifier { get; }

        public override State Type => State.MotionProposed;
        
        public MotionProposed(IGroupModifier groupModifier, Person mover, MotionChain motionChain)
        {
            Mover = mover;
            GroupModifier = groupModifier;
            MotionChain = motionChain;
        }

        public override IMeetingState CallMeetingToOrder(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("The meeting is already in order.");
        }

        public override IMeetingState DeclareTimeExpired(MeetingAttendee actor)
        {
            GroupModifier.RecordMinute($"Nobody seconded the motion to {MotionChain.Current.GetText()}.");
            // Drop this motion and go to previous motions, or the open floor if this is the only motion.
            return MotionChain.Previous.Any()
                ? new DebateState(GroupModifier, MotionChain.Pop())
                : OpenFloorState.InstanceOf(GroupModifier);
        }

        public override IMeetingState MoveMainMotion(MeetingAttendee actor, IMainMotion motion)
        {
            throw new PersonOutOfOrderException("Only seconding is allowed.");
        }

        public override IMeetingState MoveSubsidiaryMotion(MeetingAttendee actor, ISubsidiaryMotion motion)
        {
            throw new PersonOutOfOrderException("Only seconding is allowed.");
        }

        public override IMeetingState Second(MeetingAttendee actor)
        {
            if (!CanSecond(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }
            
            if (MotionChain.Current is PreviousQuestion or Adjourn)
            {
                // Motion to end debate is not debated.
                // TODO: Make "isDebatable" a property of a motion.
                return new VotingState(GroupModifier, MotionChain);
            }
                
            return new DebateState(GroupModifier, MotionChain);
        }

        public override IMeetingState Speak(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("Only seconding is allowed.");
        }

        public override IMeetingState Vote(MeetingAttendee actor, VoteType type)
        {
            throw new PersonOutOfOrderException("There is no vote in progress.");
        }

        public override IMeetingState Yield(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("Only seconding is allowed.");
        }

        public override IMeetingState MoveToAdjourn(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("Only seconding is allowed.");
        }

        public override string GetDescription()
        {
            return $"{Mover.Name} moves {MotionChain.Current.GetText()}. Does anyone second?";
        }

        protected override bool CanMoveToAdjourn(MeetingAttendee actor, out string explanation)
        {
            explanation = "Only seconding is allowed.";
            return false;
        }

        protected override bool CanCallToOrder(MeetingAttendee actor, out string explanation)
        {
            explanation = "The meeting is already in order.";
            return false;
        }

        protected override bool CanDeclareTimeExpired(MeetingAttendee actor, out string explanation)
        {
            if (!actor.Roles.HasFlag(AttendeeRole.Chair))
            {
                explanation = "Only the chair can declare the time as expired.";
                return false;
            }

            explanation = "The chair can declare the time as expired, ie there was no second.";
            return true;
        }

        protected override bool CanSecond(MeetingAttendee actor, out string explanation)
        {
            if (actor.Person.Equals(Mover))
            {
                explanation = $"{actor.Person.Name} moved the motion so they cannot second it.";
                return false;
            }

            if (actor.Roles.HasFlag(AttendeeRole.Guest))
            {
                explanation = "Guests cannot second motions.";
                return false;
            }

            explanation = $"{actor.Person.Name} can second {Mover.Name}'s motion.";
            return true;
        }

        protected override bool CanSpeak(MeetingAttendee actor, out string explanation)
        {
            explanation = "Only seconding is allowed.";
            return false;
        }

        protected override bool CanMoveMainMotion(MeetingAttendee actor, out string explanation)
        {
            explanation = "Only seconding is allowed.";
            return false;
        }

        protected override bool CanMoveSubsidiaryMotion(MeetingAttendee actor, out string explanation)
        {
            explanation = "Only seconding is allowed.";
            return false;
        }

        protected override bool CanVote(MeetingAttendee actor, out string explanation)
        {
            explanation = "There is no vote in progress.";
            return false;
        }

        protected override bool CanYield(MeetingAttendee actor, out string explanation)
        {
            explanation = $"{actor.Person.Name} does not have the floor.";
            return false;
        }
    }
}