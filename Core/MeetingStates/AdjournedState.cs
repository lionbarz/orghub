﻿using System;
using Core.Meetings;
using Core.Motions;

namespace Core.MeetingStates
{
    public class AdjournedState : MeetingStateBase
    {
        private IGroupModifier GroupModifier { get; }
        
        private MeetingAgenda Agenda { get; }

        public override StateType Type => StateType.Adjourned;
        
        private IMinuteRecorder MinuteRecorder { get; }
        
        public AdjournedState(IGroupModifier groupModifier, MeetingAgenda agenda, IMinuteRecorder minuteRecorder)
        {
            GroupModifier = groupModifier;
            Agenda = agenda;
            MinuteRecorder = minuteRecorder;
        }

        public override IMeetingState CallMeetingToOrder(MeetingAttendee actor)
        {
            // TODO: Shouldn't be able to call it to order after it adjourned.
            if (!CanCallToOrder(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }
            
            MinuteRecorder.RecordMinute($"{actor.Person.Name} calls the meeting to order.");
            
            // If there is an item on the agenda, create a state from it
            // and advance the agenda.
            if (Agenda.MoveToNextItem(out var nextItem))
            {
                return StateFactory.FromAgendaItem(nextItem, GroupModifier, Agenda, MinuteRecorder);
            }
            else
            {
                // No agenda item. Open the floor.
                // TODO: The open floor should be on the agenda, too. So what happens after all agenda items are done?
                return OpenFloorState.InstanceOf(GroupModifier, Agenda, MinuteRecorder);
            }
        }

        public override IMeetingState DeclareTimeExpired(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("Meeting is not in order");
        }

        public override IMeetingState MoveSubsidiaryMotion(MeetingAttendee actor, ISubsidiaryMotion motion)
        {
            throw new PersonOutOfOrderException("Meeting is not in order");
        }

        public override IMeetingState Second(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("Meeting is not in order");
        }

        public override IMeetingState MoveToAdjourn(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException(
                $"{actor.Person.Name} cannot move to adjourn while the meeting is adjourned.");
        }

        public override IMeetingState MoveMainMotion(MeetingAttendee actor, IMainMotion motion)
        {
            throw new PersonOutOfOrderException(
                $"{actor.Person.Name} cannot move a motion while the meeting is adjourned.");
        }

        public override IMeetingState Speak(MeetingAttendee actorRole)
        {
            throw new PersonOutOfOrderException("Nobody can speak when the meeting is adjourned.");
        }

        public override IMeetingState Vote(MeetingAttendee actor, VoteType type)
        {
            throw new PersonOutOfOrderException("There is no vote in progress.");
        }

        public override IMeetingState Yield(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("Meeting is not in order");
        }

        /// <summary>
        /// Whether or not a person can move to adjourn.
        /// </summary>
        protected override bool CanMoveToAdjourn(MeetingAttendee actor, out string explanation)
        {
            explanation = $"Cannot move to adjourn while the meeting is adjourned.";
            return false;
        }
        
        /// <summary>
        /// Whether or not a person can move to adjourn.
        /// </summary>
        protected override bool CanCallToOrder(MeetingAttendee actor, out string explanation)
        {
            if (!actor.Roles.HasFlag(AttendeeRole.Chair))
            {
                explanation = $"{actor.Person.Name} is not the chair and only the chair can call the meeting to order.";
                return false;
            }

            explanation = $"The chair can call the meeting the order.";
            return true;
        }

        protected override bool CanDeclareTimeExpired(MeetingAttendee actor, out string explanation)
        {
            explanation = "Meeting is adjourned.";
            return false;
        }

        protected override bool CanSecond(MeetingAttendee actor, out string explanation)
        {
            explanation = "Meeting is adjourned.";
            return false;
        }

        /// <summary>
        /// Whether or not a person can speak.
        /// </summary>
        protected override bool CanSpeak(MeetingAttendee actor, out string explanation)
        {
            explanation = "Nobody can speak while the meeting is adjourned.";
            return false;
        }

        protected override bool CanMoveMainMotion(MeetingAttendee actor, out string explanation)
        {
            explanation = "Nobody can move a primary motion while the meeting is adjourned.";
            return false;
        }

        protected override bool CanMoveSubsidiaryMotion(MeetingAttendee actor, out string explanation)
        {
            explanation = "Meeting is adjourned.";
            return false;
        }

        protected override bool CanVote(MeetingAttendee actor, out string explanation)
        {
            explanation = "There is no vote in progress.";
            return false;
        }

        protected override bool CanYield(MeetingAttendee actor, out string explanation)
        {
            explanation = "Meeting is adjourned.";
            return false;
        }

        public override string GetDescription()
        {
            return "The meeting is adjourned.";
        }
    }
}