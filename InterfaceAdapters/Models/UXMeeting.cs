﻿using System;
using System.Collections.Generic;

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

        /// <summary>
        /// The people attending the meeting.
        /// </summary>
        public IEnumerable<UXMeetingAttendee> Attendees { get; set; }

        /// <summary>
        /// Whether there are enough members to constitute
        /// a quorum according to the bylaws of the organization.
        /// </summary>
        public bool HasQuorum { get; set; }
        
        /// <summary>
        /// What's on the agenda for this meeting.
        /// </summary>
        public IEnumerable<UxAgendaItem> Agenda { get; set; }
        
        /// <summary>
        /// The minutes, ie activity log.
        /// </summary>
        public IEnumerable<UXMinute> Minutes { get; set; }
    }
}