using System;

namespace Core
{
    /// <summary>
    /// A single entry in the meeting minutes, corresponding to a single event.
    /// </summary>
    public class MeetingMinute
    {
        /// <summary>
        /// The event in present tense, like "the meeting is adjourned" or
        /// "the meeting is called to order." 
        /// </summary>
        public string? Text { get; private init; }

        public DateTime Time { get; private init; }

        public static MeetingMinute FromText(string text)
        {
            return new MeetingMinute()
            {
                Text = text,
                Time = DateTime.UtcNow
            };
        }
    }
}