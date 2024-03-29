using System;
using Core.Meetings;
using Core.Motions;

namespace Core.MeetingStates
{
    public static class StateFactory
    {
        public static IMeetingState FromAgendaItem(IAgendaItem agendaItem, IGroupModifier groupModifier, MeetingAgenda agenda, IMinuteRecorder minuteRecorder)
        {
            if (agendaItem is ResolutionAgendaItem resolutionAgendaItem)
            {
                minuteRecorder.RecordMinute(
                    $"Moving to next agenda item: {resolutionAgendaItem.GetTitle()}");
                var motion = new Resolve(resolutionAgendaItem.Text);
                return new MotionProposed(groupModifier, resolutionAgendaItem.Sponsor, MotionChain.FromMotion(motion),
                    agenda, minuteRecorder);
            }

            throw new Exception($"Agenda has a type of item that isn't recognized: {agendaItem.GetType()}");
        }
    }
}