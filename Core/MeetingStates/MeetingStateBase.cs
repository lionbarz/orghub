using System;
using System.Collections.Generic;
using Core.Motions;
using Action = Core.Actions.Action;

namespace Core.MeetingStates
{
    public abstract class MeetingStateBase : IMeetingState
    {
        public abstract StateType Type { get; }
        public abstract IMeetingState CallMeetingToOrder(MeetingAttendee actor);
        public abstract IMeetingState DeclareTimeExpired(MeetingAttendee actor);
        public abstract IMeetingState MoveToAdjourn(MeetingAttendee actor);
        public abstract IMeetingState MoveMainMotion(MeetingAttendee actor, IMainMotion motion);
        public abstract IMeetingState MoveSubsidiaryMotion(MeetingAttendee actor, ISubsidiaryMotion motion);
        public abstract IMeetingState Second(MeetingAttendee actor);
        public abstract IMeetingState Speak(MeetingAttendee actor);
        public abstract IMeetingState Vote(MeetingAttendee actor, VoteType type);
        public abstract IMeetingState Yield(MeetingAttendee actor);
        public abstract string GetDescription();

        public ICollection<ActionSupport> GetActionSupportForPerson(MeetingAttendee actor)
        {
            var actionSupports = new LinkedList<ActionSupport>();

            bool isSupported = CanCallToOrder(actor, out string explanation);
            actionSupports.AddLast(ActionSupport.InstanceOf(Action.CallToOrder, isSupported, explanation));
            
            isSupported = CanDeclareTimeExpired(actor, out explanation);
            actionSupports.AddLast(ActionSupport.InstanceOf(Action.DeclareTimeExpired, isSupported, explanation));
            
            isSupported = CanMoveToAdjourn(actor, out explanation);
            actionSupports.AddLast(ActionSupport.InstanceOf(Action.MoveToAdjourn, isSupported, explanation));
            
            isSupported = CanMoveMainMotion(actor, out explanation);
            actionSupports.AddLast(ActionSupport.InstanceOf(Action.MoveMainMotion, isSupported, explanation));
            
            isSupported = CanMoveSubsidiaryMotion(actor, out explanation);
            actionSupports.AddLast(ActionSupport.InstanceOf(Action.MoveSubsidiaryMotion, isSupported, explanation));

            isSupported = CanSecond(actor, out explanation);
            actionSupports.AddLast(ActionSupport.InstanceOf(Action.Second, isSupported, explanation));
            
            isSupported = CanSpeak(actor, out explanation);
            actionSupports.AddLast(ActionSupport.InstanceOf(Action.Speak, isSupported, explanation));
            
            isSupported = CanVote(actor, out explanation);
            actionSupports.AddLast(ActionSupport.InstanceOf(Action.Vote, isSupported, explanation));
            
            isSupported = CanYield(actor, out explanation);
            actionSupports.AddLast(ActionSupport.InstanceOf(Action.Yield, isSupported, explanation));
            
            return actionSupports;
        }

        /// <summary>
        /// Whether or not a person can move to adjourn.
        /// </summary>
        protected abstract bool CanMoveToAdjourn(MeetingAttendee actor, out string explanation);

        /// <summary>
        /// Whether or not a person can call to order.
        /// </summary>
        protected abstract bool CanCallToOrder(MeetingAttendee actor, out string explanation);
        
        /// <summary>
        /// Whether or not a person can declare the time as expired.
        /// </summary>
        protected abstract bool CanDeclareTimeExpired(MeetingAttendee actor, out string explanation);

        /// <summary>
        /// Whether or not a person can second.
        /// </summary>
        protected abstract bool CanSecond(MeetingAttendee actor, out string explanation);
        
        /// <summary>
        /// Whether or not a person can speak.
        /// </summary>
        protected abstract bool CanSpeak(MeetingAttendee actor, out string explanation);
        
        /// <summary>
        /// Whether or not a person can move a main motion.
        /// </summary>
        protected abstract bool CanMoveMainMotion(MeetingAttendee actor, out string explanation);
        
        /// <summary>
        /// Whether or not a person can move a subsidiary motion.
        /// </summary>
        protected abstract bool CanMoveSubsidiaryMotion(MeetingAttendee actor, out string explanation);
        
        /// <summary>
        /// Whether the person can vote.
        /// </summary>
        protected abstract bool CanVote(MeetingAttendee actor, out string explanation);
        
        /// <summary>
        /// Whether or not a person can yield the floor.
        /// </summary>
        protected abstract bool CanYield(MeetingAttendee actor, out string explanation);
    }
}