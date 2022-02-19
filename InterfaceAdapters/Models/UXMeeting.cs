using System;

namespace InterfaceAdapters.Models
{
    public class UXMeeting
    {
        /// <summary>
        /// Id of this meeting.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The current state of the meeting.
        /// </summary>
        public string State { get; set; }
        
        /// <summary>
        /// The person chairing the meeting.
        /// </summary>
        public string Chair { get; set; }
    }
}