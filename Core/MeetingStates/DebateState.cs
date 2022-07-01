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

        /// <summary>
        /// Can be used to set group properties.
        /// </summary>
        private IGroupModifier GroupModifier { get; }

        public DebateState(IGroupModifier groupModifier, MotionChain motionChain)
        {
            MotionChain = motionChain;
            GroupModifier = groupModifier;
        }

        public override IMeetingState CallMeetingToOrder(PersonRole actor)
        {
            throw new PersonOutOfOrderException("Cannot call the meeting to order during debate.");
        }

        public override IMeetingState DeclareTimeExpired(PersonRole actor)
        {
            if (!CanDeclareTimeExpired(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }

            return new VotingState(GroupModifier, MotionChain);
        }

        public override IMeetingState MoveSubsidiaryMotion(PersonRole actor, ISubsidiaryMotion motion)
        {
            if (!CanMoveSubsidiaryMotion(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }
            
            return new MotionProposed(GroupModifier, actor.Person, MotionChain.Push(motion));
        }

        public override IMeetingState Second(PersonRole actor)
        {
            throw new PersonOutOfOrderException("There is nothing to second.");
        }

        public override IMeetingState MoveToAdjourn(PersonRole actor)
        {
            if (!CanMoveToAdjourn(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }

            var newChain = MotionChain.Push(new Adjourn(actor.Person));
            return new MotionProposed(GroupModifier, actor.Person, newChain);
        }

        public override IMeetingState MoveMainMotion(PersonRole actor, IMainMotion motion)
        {
            throw new PersonOutOfOrderException("Cannot move a main motion while another is debated.");
        }

        public override IMeetingState Speak(PersonRole actor)
        {
            if (!CanSpeak(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }

            return new SpeakerHasFloorState(GroupModifier, actor.Person, MotionChain);
        }

        public override IMeetingState Vote(PersonRole actor, VoteType type)
        {
            throw new PersonOutOfOrderException("There is no vote in progress.");
        }

        public override IMeetingState Yield(PersonRole actor)
        {
            throw new PersonOutOfOrderException("Cannot yield because nobody has the floor.");
        }

        public override string GetDescription()
        {
            return $"The suggestion is to {MotionChain.Current.GetText()}. Any debate?";
        }

        protected override bool CanMoveToAdjourn(PersonRole actor, out string explanation)
        {
            if (actor.IsGuest)
            {
                explanation = $"{actor.Person.Name} is a guest and cannot move to adjourn.";
                return false;
            }

            explanation = "Moving to adjourn can be made during debate.";
            return true;
        }

        protected override bool CanCallToOrder(PersonRole actor, out string explanation)
        {
            explanation = "Cannot call the meeting to order during debate.";
            return false;
        }

        protected override bool CanDeclareTimeExpired(PersonRole actor, out string explanation)
        {
            // TODO: This action should be "DeclareNoMoreDebate" instead of recycling this.
            
            if (!actor.IsChair)
            {
                explanation = "Only the chair can declare that there is no more debate.";
                return false;
            }
            
            explanation = "When there is no more debate the proceedings can move to a vote.";
            return true;
        }

        protected override bool CanSecond(PersonRole actor, out string explanation)
        {
            explanation = "There is no motion to second.";
            return false;
        }

        protected override bool CanSpeak(PersonRole actor, out string explanation)
        {
            // TODO: Limit the number of times guests and members can speak.
            explanation = "Anyone can speak during the debate stage.";
            return true;
        }

        protected override bool CanMoveMainMotion(PersonRole actor, out string explanation)
        {
            explanation = "Primary motions cannot be moved while debating another motion.";
            return false;
        }

        protected override bool CanMoveSubsidiaryMotion(PersonRole actor, out string explanation)
        {
            if (actor.IsGuest)
            {
                explanation = "Guests cannot move motions.";
                return false;
            }

            explanation = "There is a main motion and subsidiary motions can be made.";
            return true;
        }

        protected override bool CanVote(PersonRole actor, out string explanation)
        {
            explanation = "There is no vote in progress.";
            return false;
        }

        protected override bool CanYield(PersonRole actor, out string explanation)
        {
            explanation = "There's nothing to yield.";
            return false;
        }
    }
}