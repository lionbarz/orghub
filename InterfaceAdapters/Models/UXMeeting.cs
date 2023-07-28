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
        /// Why this meeting was called, and other details.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// When this meeting was scheduled to start.
        /// </summary>
        public string StartTime { get; set; }
        
        /// <summary>
        /// The Zoom link or physical address.
        /// </summary>
        public string Location { get; set; }
        
        /// <summary>
        /// A string description in plain English about the
        /// state of this group. Ex: Mohamed is speaking.
        /// </summary>
        public string State { get; set; }
        
        /// <summary>
        /// A unique string that identifies this state, such
        /// as Adjourned or Voting.
        /// </summary>
        public string StateType { get; set; }
    }
}