using Core.Motions;

namespace Core.MeetingStates
{
    /// <summary>
    /// A motion has been moved and seconded. The floor is open to
    /// speakers (debaters) and subsidiary motions.
    /// </summary>
    public class DebateState : MeetingStateBase
    {
        private MotionChain MotionChain { get; }

        public override State Type => State.Debate;
        
        /// <summary>
        /// Can be used to set group properties.
        /// </summary>
        private IGroupModifier GroupModifier { get; }

        public DebateState(IGroupModifier groupModifier, MotionChain motionChain)
        {
            MotionChain = motionChain;
            GroupModifier = groupModifier;
        }

        public override IMeetingState CallMeetingToOrder(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("Cannot call the meeting to order during debate.");
        }

        public override IMeetingState DeclareTimeExpired(MeetingAttendee actor)
        {
            if (!CanDeclareTimeExpired(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }

            return new VotingState(GroupModifier, MotionChain);
        }

        public override IMeetingState MoveSubsidiaryMotion(MeetingAttendee actor, ISubsidiaryMotion motion)
        {
            if (!CanMoveSubsidiaryMotion(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }
            
            return new MotionProposed(GroupModifier, actor.Person, MotionChain.Push(motion));
        }

        public override IMeetingState Second(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("There is nothing to second.");
        }

        public override IMeetingState MoveToAdjourn(MeetingAttendee actor)
        {
            if (!CanMoveToAdjourn(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }

            var newChain = MotionChain.Push(new Adjourn(actor.Person));
            return new MotionProposed(GroupModifier, actor.Person, newChain);
        }

        public override IMeetingState MoveMainMotion(MeetingAttendee actor, IMainMotion motion)
        {
            throw new PersonOutOfOrderException("Cannot move a main motion while another is debated.");
        }

        public override IMeetingState Speak(MeetingAttendee actor)
        {
            if (!CanSpeak(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }

            GroupModifier.RecordMinute($"{actor.Person.Name} takes the floor.");
            return new SpeakerHasFloorState(GroupModifier, actor.Person, MotionChain);
        }

        public override IMeetingState Vote(MeetingAttendee actor, VoteType type)
        {
            throw new PersonOutOfOrderException("There is no vote in progress.");
        }

        public override IMeetingState Yield(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("Cannot yield because nobody has the floor.");
        }

        public override string GetDescription()
        {
            return $"The suggestion is {MotionChain.Current.GetText()}. Any debate?";
        }

        protected override bool CanMoveToAdjourn(MeetingAttendee actor, out string explanation)
        {
            if (actor.Roles.HasFlag(AttendeeRole.Guest))
            {
                explanation = $"{actor.Person.Name} is a guest and cannot move to adjourn.";
                return false;
            }

            explanation = "Moving to adjourn can be made during debate.";
            return true;
        }

        protected override bool CanCallToOrder(MeetingAttendee actor, out string explanation)
        {
            explanation = "Cannot call the meeting to order during debate.";
            return false;
        }

        protected override bool CanDeclareTimeExpired(MeetingAttendee actor, out string explanation)
        {
            // TODO: This action should be "DeclareNoMoreDebate" instead of recycling this.
            
            if (!actor.Roles.HasFlag(AttendeeRole.Chair))
            {
                explanation = "Only the chair can declare that there is no more debate.";
                return false;
            }
            
            explanation = "When there is no more debate the proceedings can move to a vote.";
            return true;
        }

        protected override bool CanSecond(MeetingAttendee actor, out string explanation)
        {
            explanation = "There is no motion to second.";
            return false;
        }

        protected override bool CanSpeak(MeetingAttendee actor, out string explanation)
        {
            // TODO: Limit the number of times guests and members can speak.
            explanation = "Anyone can speak during the debate stage.";
            return true;
        }

        protected override bool CanMoveMainMotion(MeetingAttendee actor, out string explanation)
        {
            explanation = "Primary motions cannot be moved while debating another motion.";
            return false;
        }

        protected override bool CanMoveSubsidiaryMotion(MeetingAttendee actor, out string explanation)
        {
            if (actor.Roles.HasFlag(AttendeeRole.Guest))
            {
                explanation = "Guests cannot move motions.";
                return false;
            }

            explanation = "There is a main motion and subsidiary motions can be made.";
            return true;
        }

        protected override bool CanVote(MeetingAttendee actor, out string explanation)
        {
            explanation = "There is no vote in progress.";
            return false;
        }

        protected override bool CanYield(MeetingAttendee actor, out string explanation)
        {
            explanation = "There's nothing to yield.";
            return false;
        }
    }
}