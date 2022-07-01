using System;
using System.Linq;
using Core.Motions;

namespace Core.MeetingStates
{
    public class VotingState : MeetingStateBase
    {
        private YesNoBallotBox BallotBox { get; }
        
        private MotionChain MotionChain { get; init; }
        
        /// <summary>
        /// Can be used to set group properties.
        /// </summary>
        private IGroupModifier GroupModifier { get; }
        
        public VotingState(IGroupModifier groupModifier, MotionChain motionChain)
        {
            BallotBox = new YesNoBallotBox();
            GroupModifier = groupModifier;
            MotionChain = motionChain;
        }

        public override IMeetingState CallMeetingToOrder(PersonRole actor)
        {
            throw new PersonOutOfOrderException("Meeting is already in order.");
        }

        /// <summary>
        /// When the time expires, whoever has more votes wins.
        /// </summary>
        public override IMeetingState DeclareTimeExpired(PersonRole actor)
        {
            // TODO: Do something about a quorum, so not just declaring a result
            // TODO: when a small fraction has voted.
            
            BallotBox.CloseVoting();
            var motionCarried = BallotBox.GetStatus() == VoteResult.AyesHaveIt;

            if (motionCarried)
            {
                if (MotionChain.Current is GroupModifyingMotion groupModifyingMotion)
                {
                    groupModifyingMotion.TakeActionAsync();
                }
                
                if (MotionChain.Current is PreviousQuestion)
                {
                    // The vote was on ending debate, so the next thing is to vote on the main motion.
                    return new VotingState(GroupModifier, MotionChain.Pop());
                }

                if (MotionChain.Current is Adjourn)
                {
                    return new AdjournedState(GroupModifier);
                }
            }
            else
            {
                // TODO: Record that the motion didn't carry.
            }
            
            return MotionChain.Previous.Any()
                ? new DebateState(GroupModifier, MotionChain.Pop())
                : OpenFloorState.InstanceOf(GroupModifier);
        }

        public override IMeetingState MoveMainMotion(PersonRole actor, IMainMotion motion)
        {
            throw new PersonOutOfOrderException("Cannot move a motion during voting.");
        }

        public override IMeetingState MoveSubsidiaryMotion(PersonRole actor, ISubsidiaryMotion motion)
        {
            throw new PersonOutOfOrderException("Cannot move a motion during voting.");
        }

        public override IMeetingState Second(PersonRole actor)
        {
            throw new PersonOutOfOrderException("There isn't a motion to second.");
        }

        public override IMeetingState Speak(PersonRole actor)
        {
            throw new PersonOutOfOrderException("Nobody can speak during voting.");
        }

        public override IMeetingState Vote(PersonRole actor, VoteType type)
        {
            if (!CanVote(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }
            
            BallotBox.CastBallot(new YesNoBallot(actor.Person, type));
            return this;
        }

        public override IMeetingState Yield(PersonRole actor)
        {
            throw new PersonOutOfOrderException("Nobody has the floor during voting.");
        }

        public override IMeetingState MoveToAdjourn(PersonRole actor)
        {
            throw new PersonOutOfOrderException("The meeting cannot be adjourned during voting.");
        }

        public override string GetDescription()
        {
            return
                $"Voting on motion to {MotionChain.Current.GetText()}. {BallotBox.NumAye} in favor. {BallotBox.NumNay} opposed. {BallotBox.NumAbstain} abstaining.";
        }

        protected override bool CanMoveToAdjourn(PersonRole actor, out string explanation)
        {
            explanation = "The meeting cannot be adjourned during voting.";
            return false;
        }

        protected override bool CanCallToOrder(PersonRole actor, out string explanation)
        {
            explanation = "The meeting is already in order.";
            return false;
        }

        protected override bool CanDeclareTimeExpired(PersonRole actor, out string explanation)
        {
            // TODO: Have a timer dictate this.
            
            if (actor.IsChair)
            {
                explanation = "The chair can declare the time for voting as expired.";
                return true;
            }

            explanation = "Only the chair can declare the time for voting as expired.";
            return false;
        }

        protected override bool CanSecond(PersonRole actor, out string explanation)
        {
            explanation = "There is no motion to second.";
            return false;
        }

        protected override bool CanSpeak(PersonRole actor, out string explanation)
        {
            explanation = "No speeches are allowed during voting.";
            return false;
        }

        protected override bool CanMoveMainMotion(PersonRole actor, out string explanation)
        {
            explanation = "No motions can be moved during voting.";
            return false;
        }

        protected override bool CanMoveSubsidiaryMotion(PersonRole actor, out string explanation)
        {
            explanation = "No motions can be moved during voting.";
            return false;
        }

        protected override bool CanVote(PersonRole actor, out string explanation)
        {
            if (actor.IsGuest)
            {
                explanation = "Voting is only open to members.";
                return false;
            }

            explanation = "Voting is open to all members.";
            return true;
        }

        protected override bool CanYield(PersonRole actor, out string explanation)
        {
            explanation = "Nobody has the floor during voting.";
            return false;
        }
    }
}