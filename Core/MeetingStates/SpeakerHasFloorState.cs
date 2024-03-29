﻿using System;
using Core.Meetings;
using Core.Motions;

namespace Core.MeetingStates
{
    public class SpeakerHasFloorState : MeetingStateBase
    {
        public override StateType Type => StateType.SpeakerHasFloor;
        
        // The speaker who has the floor.
        private Person Speaker { get; }
        
        // An optional motion. The speaker could be debating
        // a motion or just speaking.
        private MotionChain? MotionChain { get; }

        // The amount of time the speaker has.
        private TimeSpan AllotedTime { get; }
        
        // Need it to pass to other states.
        private IGroupModifier GroupModifier { get; }
        
        private MeetingAgenda Agenda { get; }
        
        private IMinuteRecorder MinuteRecorder { get; }

        public SpeakerHasFloorState(IGroupModifier groupModifier, Person speaker, MeetingAgenda agenda, IMinuteRecorder minuteRecorder)
        {
            Speaker = speaker;
            GroupModifier = groupModifier;
            Agenda = agenda;
            MinuteRecorder = minuteRecorder;
        }
        
        public SpeakerHasFloorState(IGroupModifier groupModifier, Person speaker, MotionChain motionChain, MeetingAgenda agenda, IMinuteRecorder minuteRecorder)
        {
            Speaker = speaker;
            MotionChain = motionChain;
            GroupModifier = groupModifier;
            Agenda = agenda;
            MinuteRecorder = minuteRecorder;
        }

        public override IMeetingState CallMeetingToOrder(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("Meeting already in order.");
        }

        public override IMeetingState DeclareTimeExpired(MeetingAttendee actor)
        {
            if (!CanDeclareTimeExpired(actor, out string explanation))
            {
                throw new PersonOutOfOrderException(explanation);
            }
            
            MinuteRecorder.RecordMinute($"{actor.Person.Name}'s time on the floor is expired.");
            return MotionChain == null
                ? OpenFloorState.InstanceOf(GroupModifier, Agenda, MinuteRecorder)
                : new DebateState(GroupModifier, MotionChain, Agenda, MinuteRecorder);
        }

        public override IMeetingState MoveMainMotion(MeetingAttendee actor, IMainMotion motion)
        {
            throw new PersonOutOfOrderException("Can't move a motion while someone is speaking.");
        }

        public override IMeetingState MoveSubsidiaryMotion(MeetingAttendee actor, ISubsidiaryMotion motion)
        {
            throw new PersonOutOfOrderException("Can't move a motion while someone is speaking.");
        }

        public override IMeetingState Second(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("There is no motion to second.");
        }

        public override IMeetingState Speak(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("Can't speak while someone is speaking.");
        }

        public override IMeetingState Vote(MeetingAttendee actor, VoteType type)
        {
            throw new PersonOutOfOrderException("There is no vote in progress.");
        }

        public override IMeetingState Yield(MeetingAttendee actor)
        {
            MinuteRecorder.RecordMinute($"{actor.Person.Name} yields the floor.");
            
            return MotionChain == null
                ? OpenFloorState.InstanceOf(GroupModifier, Agenda, MinuteRecorder)
                : new DebateState(GroupModifier, MotionChain, Agenda, MinuteRecorder);
        }

        public override IMeetingState MoveToAdjourn(MeetingAttendee actor)
        {
            throw new PersonOutOfOrderException("Can't move to adjourn while someone is speaking.");
        }

        public override string GetDescription()
        {
            if (MotionChain != null)
            {
                return $"{Speaker.Name} is debating the motion {MotionChain.Current.GetText()}.";
            }

            return $"{Speaker.Name} has the floor.";
        }

        protected override bool CanMoveToAdjourn(MeetingAttendee actor, out string explanation)
        {
            explanation = "Can't move to adjourn while someone is speaking.";
            return false;
        }

        protected override bool CanCallToOrder(MeetingAttendee actor, out string explanation)
        {
            explanation = "The meeting is already in order.";
            return false;
        }

        protected override bool CanDeclareTimeExpired(MeetingAttendee actor, out string explanation)
        {
            // TODO: Use a timer and only allow if it's really expired.
            
            if (!actor.Roles.HasFlag(AttendeeRole.Chair))
            {
                explanation = "Only the chair can declare the time as expired.";
                return false;
            }

            explanation = "The chair can declare the time as expired.";
            return true;
        }

        protected override bool CanSecond(MeetingAttendee actor, out string explanation)
        {
            explanation = "There is no motion to second.";
            return false;
        }

        protected override bool CanSpeak(MeetingAttendee actor, out string explanation)
        {
            explanation = "Can't speak while someone is speaking.";
            return false;
        }

        protected override bool CanMoveMainMotion(MeetingAttendee actor, out string explanation)
        {
            explanation = "Can't move a motion while someone is speaking.";
            return false;
        }

        protected override bool CanMoveSubsidiaryMotion(MeetingAttendee actor, out string explanation)
        {
            explanation = "Can't move a motion while someone is speaking.";
            return false;
        }

        protected override bool CanVote(MeetingAttendee actor, out string explanation)
        {
            explanation = "There is no vote in progress.";
            return false;
        }

        protected override bool CanYield(MeetingAttendee actor, out string explanation)
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