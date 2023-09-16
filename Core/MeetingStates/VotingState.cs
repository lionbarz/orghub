using System.Linq;
using Core.Meetings;
using Core.Motions;

namespace Core.MeetingStates
{
    public class VotingState : MeetingStateBase
    {
        public override StateType Type => StateType.Voting;
        
        private YesNoBallotBox BallotBox { get; }
        
        private MotionChain MotionChain { get; }
        
        private MeetingAgenda Agenda { get; }
        
        /// <summary>
        /// Can be used to set group properties.
        /// </summary>
        private IGroupModifier GroupModifier { get; }
        
        public VotingState(IGroupModifier groupModifier, MotionChain motionChain, MeetingAgenda agenda)
        {
            BallotBox = new YesNoBallotBox();
            GroupModifier = groupModifier;
            MotionChain = motionChain;
            Agenda = agenda;
        }

        public override IMeetingState CallMeetingToOrder(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("Meeting is already in order.");
        }

        /// <summary>
        /// When the time expires, whoever has more votes wins.
        /// </summary>
        public override IMeetingState DeclareTimeExpired(MeetingAttendee actor)
        {
            // TODO: Do something about a quorum, so not just declaring a result
            // TODO: when a small fraction has voted.
            
            BallotBox.CloseVoting();
            var motionCarried = BallotBox.GetStatus() == VoteResult.AyesHaveIt;

            if (motionCarried)
            {
                if (MotionChain.Current is IGroupModifyingMotion groupModifyingMotion)
                {
                    GroupModifier.RecordMinute(
                        $"The motion {MotionChain.Current.GetText()} is carried.");
                    groupModifyingMotion.TakeActionAsync(GroupModifier);
                }
                
                if (MotionChain.Current is PreviousQuestion)
                {
                    GroupModifier.RecordMinute(
                        $"The motion to end debate and vote on {MotionChain.Previous.Last().GetText()} is carried.");
                    // The vote was on ending debate, so the next thing is to vote on the main motion.
                    return new VotingState(GroupModifier, MotionChain.Pop(), Agenda);
                }

                if (MotionChain.Current is Adjourn)
                {
                    GroupModifier.RecordMinute($"The motion to adjourn is carried.");
                    return new AdjournedState(GroupModifier, Agenda);
                }
            }
            else
            {
                GroupModifier.RecordMinute(
                    $"The motion {MotionChain.Current.GetText()} didn't get enough votes and is dropped.");
            }

            // TODO: Centralize this logic. It's the same as in MotionProposed.
            
            if (MotionChain.Previous.Any())
            {
                // There was a previous motion, so go back to debating it.
                return new DebateState(GroupModifier, MotionChain.Pop(), Agenda);
            }
            
            // If there is something next on the agenda, do it.
            if (Agenda.MoveToNextItem(out var nextItem))
            {
                return StateFactory.FromAgendaItem(nextItem, GroupModifier, Agenda);
            }
            
            // No agenda items, so open the floor.
            return OpenFloorState.InstanceOf(GroupModifier, Agenda);
        }

        public override IMeetingState MoveMainMotion(MeetingAttendee actor, IMainMotion motion)
        {
            throw new PersonOutOfOrderException("Cannot move a motion during voting.");
        }

        public override IMeetingState MoveSubsidiaryMotion(MeetingAttendee actor, ISubsidiaryMotion motion)
        {
            throw new PersonOutOfOrderException("Cannot move a motion during voting.");
        }

        public override IMeetingState Second(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("There isn't a motion to second.");
        }

        public override IMeetingState Speak(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("Nobody can speak during voting.");
        }

        public override IMeetingState Vote(MeetingAttendee actor, VoteType type)
        {
            if (!CanVote(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }
            
            BallotBox.CastBallot(new YesNoBallot(actor.Person, type));
            return this;
        }

        public override IMeetingState Yield(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("Nobody has the floor during voting.");
        }

        public override IMeetingState MoveToAdjourn(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("The meeting cannot be adjourned during voting.");
        }

        public override string GetDescription()
        {
            return
                $"Voting on motion to {MotionChain.Current.GetText()}. {BallotBox.NumAye} in favor. {BallotBox.NumNay} opposed. {BallotBox.NumAbstain} abstaining. How do you vote?";
        }

        protected override bool CanMoveToAdjourn(MeetingAttendee actor, out string explanation)
        {
            explanation = "The meeting cannot be adjourned during voting.";
            return false;
        }

        protected override bool CanCallToOrder(MeetingAttendee actor, out string explanation)
        {
            explanation = "The meeting is already in order.";
            return false;
        }

        protected override bool CanDeclareTimeExpired(MeetingAttendee actor, out string explanation)
        {
            // TODO: Have a timer dictate this.
            
            if (actor.Roles.HasFlag(AttendeeRole.Chair))
            {
                explanation = "The chair can declare the time for voting as expired.";
                return true;
            }

            explanation = "Only the chair can declare the time for voting as expired.";
            return false;
        }

        protected override bool CanSecond(MeetingAttendee actor, out string explanation)
        {
            explanation = "There is no motion to second.";
            return false;
        }

        protected override bool CanSpeak(MeetingAttendee actor, out string explanation)
        {
            explanation = "No speeches are allowed during voting.";
            return false;
        }

        protected override bool CanMoveMainMotion(MeetingAttendee actor, out string explanation)
        {
            explanation = "No motions can be moved during voting.";
            return false;
        }

        protected override bool CanMoveSubsidiaryMotion(MeetingAttendee actor, out string explanation)
        {
            explanation = "No motions can be moved during voting.";
            return false;
        }

        protected override bool CanVote(MeetingAttendee actor, out string explanation)
        {
            if (actor.Roles.HasFlag(AttendeeRole.Guest))
            {
                explanation = "Voting is only open to members.";
                return false;
            }

            explanation = "Voting is open to all members.";
            return true;
        }

        protected override bool CanYield(MeetingAttendee actor, out string explanation)
        {
            explanation = "Nobody has the floor during voting.";
            return false;
        }
    }
}