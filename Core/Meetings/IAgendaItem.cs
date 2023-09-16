using Core.MeetingStates;

namespace Core.Meetings
{
    /// <summary>
    /// These can be translated into meeting states.
    /// They indicate what's on a meeting agenda.
    /// </summary>
    public interface IAgendaItem
    {
        public string GetTitle();

        /// <summary>
        /// Whether the meeting is currently on this agenda item.
        /// </summary>
        public bool IsCurrent { get; set; }
    }
}